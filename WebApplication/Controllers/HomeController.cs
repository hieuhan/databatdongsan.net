using ClosedXML.Excel;
using databatdongsan.helper;
using databatdongsan.library;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication.Helpers.Attributes;
using WebApplication.Helpers.Extensions;
using WebApplication.Helpers.Sercurity;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [DatabatdongsanAuthorize]
    public class HomeController : Controller
    {
        private readonly string redirectUrl = "HomeRedirectUrl";
        //private readonly UserSessionVM _currentUser = SessionHelper.Get<UserSessionVM>(ConstantHelper.UserSession);

        public async Task<ActionResult> Index(string keyword = "", int sortedBy = 0, int userId = 0, int page = 1, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            var customers = new Customers
            {
                ActBy = myPrincipal.UserName,
                UserId = userId
            };

            SessionHelper.Save(redirectUrl, Request.RawUrl);

            keyword = keyword.StripTags();
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            
            if(pageSize <= 0)
            {
                pageSize = 50;
            }

            if (pageSize > 400)
            {
                pageSize = 400;
            }

            if (sortedBy > 0)
            {
                DateTime today = DateTime.Today;
                switch (sortedBy)
                {
                    case 1:
                        dateFrom = today.Date;
                        dateTo = today.AddDays(1);
                        break;
                    case 2:
                        dateFrom = today.AddDays(-1);
                        dateTo = today;
                        break;
                    case 3:
                        dateFrom = today.AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
                        dateTo = today.AddDays(1);
                        break;
                    case 4:
                        DateTime dt = today.AddDays(-(int)today.DayOfWeek - 6);
                        dateFrom = dt;
                        dateTo = dt.AddDays(7);
                        break;
                    case 5:
                        dateFrom = new DateTime(today.Year, today.Month, 1);
                        dateTo = today.AddDays(1);
                        break;
                    case 6:
                        DateTime firstDayLastMonth = today.AddDays(1 - today.Day).AddMonths(-1);
                        DateTime lastDayLastMonth = new DateTime(firstDayLastMonth.Year, firstDayLastMonth.Month,
                            DateTime.DaysInMonth(firstDayLastMonth.Year, firstDayLastMonth.Month));
                        dateFrom = firstDayLastMonth;
                        dateTo = lastDayLastMonth.AddDays(1);
                        break;
                    case 7:
                        dateFrom = new DateTime(today.Year, 1, 1);
                        dateTo = today.AddDays(1);
                        break;
                }
            }

            var customersTask = customers.GetPage(dateFrom, dateTo, 0, keyword, "", page > 0 ? page - 1 : 0, pageSize);
            var usersTask = Users.Static_GetById(myPrincipal.UserName, userId, 1);

            await Task.WhenAll(customersTask, usersTask);

            var model = new CustomerVM
            {
                UserId = userId,
                SortedBy = sortedBy,
                CustomersList = customersTask.Result.Item1,
                Users = usersTask.Result,
                Pagination = new PaginationVM(page, pageSize, customersTask.Result.Item2)
            };

            if (Request.HttpMethod.ToLower() == "post")
            {
                if (model.CustomersList.IsAny())
                {
                    int number = 0;
                    DataTable dt = new DataTable("databatdongsan");
                    dt.Columns.AddRange(new DataColumn[6] { new DataColumn("STT"),
                                                     new DataColumn("DienThoai"),
                                                     new DataColumn("Email"),
                                                     new DataColumn("GhiChu"),
                                                     new DataColumn("NguoiTao"),
                                                     new DataColumn("NgayTao")});

                    for (int index = 0; index < model.CustomersList.Count; index++)
                    {
                        number = index + (model.Pagination.PageIndex > 0 ? model.Pagination.PageIndex - 1 : model.Pagination.PageIndex) * model.Pagination.PageSize + 1;
                        dt.Rows.Add(number, model.CustomersList[index].Mobile, model.CustomersList[index].Email, model.CustomersList[index].Note, model.CustomersList[index].CreatedBy, model.CustomersList[index].CrDateTime.ToString("dd/MM/yyy HH:mm:ss"));
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ExcelFile_{DateTime.Now:ddMMyyyHHmmss}.xlsx");
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportFromExcel(UserImportExcelVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.PostedFile != null && model.PostedFile.ContentLength > 0)
                {
                    string rootDir = Request.PhysicalApplicationPath,
                    filePath = string.Empty;

                    try
                    {
                        MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                        filePath = FileUploadHelper.SaveFile(model.PostedFile,
                            rootDir, ConstantHelper.MediaPath, true);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            filePath.Replace("\\", "/");
                            filePath = rootDir + filePath;

                            DataTable dt = new DataTable();
                            string excelConnectionString = string.Format(ConstantHelper.ExcelConstr, filePath);

                            using (OleDbConnection connExcel = new OleDbConnection(excelConnectionString))
                            {
                                using (OleDbCommand cmdExcel = new OleDbCommand())
                                {
                                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                    {
                                        cmdExcel.Connection = connExcel;

                                        //Get the name of First Sheet.
                                        connExcel.Open();
                                        DataTable dtExcelSchema;
                                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                        connExcel.Close();

                                        //Read Data from First Sheet.
                                        connExcel.Open();
                                        cmdExcel.CommandText = $"SELECT DienThoai, Email, GhiChu, {myPrincipal.UserId} AS UserId FROM [{sheetName}] WHERE (DienThoai <>'')";
                                        odaExcel.SelectCommand = cmdExcel;
                                        odaExcel.Fill(dt);
                                        connExcel.Close();
                                    }
                                }
                            }

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                Tuple<string, string, int, int, int> importExcelResult = await Customers.Static_Import(myPrincipal.UserName, myPrincipal.UserId, dt);

                                if (importExcelResult.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                                {
                                    if(importExcelResult.Item3 == 0 && importExcelResult.Item4 == 0)
                                    {
                                        this.ToastrInfo("Thông báo", "Không có dữ liệu được nhập vào hệ thống.");
                                    }
                                    else
                                    {
                                        string message = string.Empty;

                                        if (importExcelResult.Item3 > 0)
                                        {
                                            message = $"Thêm thành công {importExcelResult.Item3} khách hàng.<br/>";
                                        }

                                        if (importExcelResult.Item4 > 0)
                                        {
                                            message += $"Cập nhật thành công {importExcelResult.Item4} khách hàng.<br/>";
                                        }

                                        this.ToastrSuccess("Thông báo", message);
                                    }
                                }
                                else
                                {
                                    this.ToastrError("Thông báo", importExcelResult.Item2);
                                }
                            }
                            else
                            {
                                this.ToastrInfo("Thông báo", "Không có dữ liệu để nhập.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.ToastrError("Thông báo", "Quý khách vui lòng thử lại sau.");
                    }
                }
            }

            return Redirect(StringHelper.RedirectUrl(redirectUrl, Url.Action("Index", "Home")));
        }

        public async Task<ActionResult> Charts(int chartsType = 0)
        {
            DateTime dateTo = DateTime.Now, dateFrom = dateTo.Date.AddDays(-7);

            if(chartsType == 1)
            {
                dateFrom = dateTo.Date.AddDays(-30);
            }
            else if (chartsType == 2)
            {
                dateFrom = dateTo.Date.AddMonths(-3);
            }
            else if (chartsType == 3)
            {
                dateFrom = dateTo.Date.AddMonths(-6);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            var usersTask = new Users { ActBy = myPrincipal.UserName, UserStatusId = ConstantHelper.UserStatusIdApproved }.GetPage("", "", 0, 50);
            var customersTask = Customers.Static_GetReport(myPrincipal.UserName, myPrincipal.UserId, dateFrom, dateTo);

            await Task.WhenAll(usersTask, customersTask);

            ChartsViewModel model = new ChartsViewModel
            {
                UsersList = usersTask.Result.Item1,
                CustomersList = customersTask.Result
            };

            return View(model);
        }

        public ActionResult FAQ()
        {
            return View(new BaseViewModel());
        }
    }
}