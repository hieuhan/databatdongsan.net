using databatdongsan.helper;
using databatdongsan.library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication.Helpers.Attributes;
using WebApplication.Helpers.Extensions;
using WebApplication.Helpers.Sercurity;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [DatabatdongsanAuthorize]
    public class AjaxController : Controller
    {
        //private readonly UserSessionVM _currentUser = SessionHelper.Get<UserSessionVM>(ConstantHelper.UserSession);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> AddNewCustomer(CustomerInsertOrUpdateVM model)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Customers customerInsert = new Customers
            {
                ActBy = myPrincipal.UserName
            };

            if (ModelState.IsValid)
            {
                customerInsert.CopyPropertiesFrom(model);

                customerInsert.UserId = myPrincipal.UserId;

                Tuple<string, string> customerInsertResult = await customerInsert.Insert();

                if (customerInsertResult.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Message = "Thêm khách hàng thành công!",
                        Callback = Convert.ToBase64String(Encoding.UTF8.GetBytes("window.location.href=window.location.href")),
                        Completed = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        Message = customerInsertResult.Item2,
                        Completed = false
                    });
                }
            }

            return Json(new
            {
                Message = "Quý khách vui lòng thử lại sau.",
                Completed = false
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CustomerEdit(CustomerInsertOrUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                var customer = await new Customers
                {
                    ActBy = myPrincipal.UserName,
                    CustomerId = model.CustomerId
                }.Get();

                if (customer == null || customer.CustomerId <= 0)
                {
                    return Json(new
                    {
                        Message = "Không tìm thấy khách hàng phù hợp.",
                        Completed = false
                    });
                }

                customer.ActBy = myPrincipal.UserName;
                customer.Mobile = model.Mobile;
                customer.Email = model.Email;
                customer.Note = model.Note;

                Tuple<string, string> customerUpdateResult = await customer.Update();

                if (customerUpdateResult.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Message = "Cập nhật thông tin khách hàng thành công.",
                        Callback = Convert.ToBase64String(Encoding.UTF8.GetBytes("window.location.href=window.location.href")),
                        Completed = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        Message = customerUpdateResult.Item2,
                        Completed = false
                    });
                }
            }

            return Json(new
            {
                Message = "Quý khách vui lòng thử lại sau.",
                Completed = false
            });
        }

        [HttpPost]
        public async Task<JsonResult> RemoveCustomer(int customerId)
        {
            if (customerId > 0)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                var customer = await new Customers
                {
                    ActBy = myPrincipal.UserName,
                    CustomerId = customerId
                }.Get();

                if (customer == null || customer.CustomerId <= 0)
                {
                    return Json(new
                    {
                        Message = "Không tìm thấy khách hàng phù hợp.",
                        Completed = false
                    });
                }

                customer.ActBy = myPrincipal.UserName;
                Tuple<string, string> removeCustomerResult = await customer.Delete();

                if (removeCustomerResult.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Message = "Xóa khách hàng thành công.",
                        Callback = Convert.ToBase64String(Encoding.UTF8.GetBytes("window.location.href=window.location.href")),
                        Completed = true
                    });
                }
                else
                {
                    return Json(new
                    {
                        Message = removeCustomerResult.Item2,
                        Completed = false
                    });
                }
            }

            return Json(new
            {
                Message = "Quý khách vui lòng thử lại sau.",
                Completed = false
            });
        }

        [HttpPost]
        public async Task<PartialViewResult> EditCustomerRow(int customerId = 0, int orderBy = 0)
        {
            var model = new CustomerInsertOrUpdateVM
            {
                OrderBy = orderBy
            };

            if (customerId > 0)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                var customer = await Customers.Static_GetById(myPrincipal.UserName, customerId);

                if (customer != null && customer.CustomerId > 0)
                {
                    model.CopyPropertiesFrom(customer);
                }
            }

            return PartialView("~/Views/AjaxTemplates/EditCustomerRow.cshtml", model);
        }

        [HttpPost]
        public async Task<PartialViewResult> ViewCustomerRow(int customerId = 0, int number = 0, int orderBy = 0)
        {
            var model = new CustomerInsertOrUpdateVM
            {
                OrderBy = orderBy
            };

            if (customerId > 0)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                var customer = await Customers.Static_GetById(myPrincipal.UserName, customerId);

                if (customer != null && customer.CustomerId > 0)
                {
                    model.CopyPropertiesFrom(customer);
                }
            }

            return PartialView("~/Views/AjaxTemplates/ViewCustomerRow.cshtml", model);
        }

        [HttpGet]
        public async Task<JsonResult> GetUsers(string name = "")
        {
            List<ObjectJsonVM> resultVar = new List<ObjectJsonVM>()
            {
                new ObjectJsonVM
                {
                    Id = 0,
                    Name = "Chọn tài khoản"
                }
            };

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            var usersGetPage = await new Users { ActBy = myPrincipal.UserName }.GetPage(name.StripTags(), "", 0, ConstantHelper.PageSize);

            if (usersGetPage.Item1.IsAny())
            {
                resultVar.AddRange(usersGetPage.Item1.Select(x => new ObjectJsonVM()
                {
                    Id = x.UserId,
                    Name = x.Counter > 0 ? $"{x.UserName} ({ x.Counter.ToString("#,###")} khách hàng)" : x.UserName
                }));
            }

            return Json(resultVar, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> GetCharts(int chartsType = 0)
        {
            List<ObjectJsonVM> resultVar = new List<ObjectJsonVM>();

            DateTime dateTo = DateTime.Now, dateFrom = dateTo.Date.AddDays(-7);

            if (chartsType == 1)
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

            var customersList = await Customers.Static_GetReport(myPrincipal.UserName, myPrincipal.UserId, dateFrom, dateTo);

            if (customersList.IsAny())
            {
                resultVar.AddRange(customersList.Select(x => new ObjectJsonVM()
                {
                    Id = x.Total,
                    Name = x.CrDateTime.ToString("yyyy-MM-dd")
                }));
            }

            return Json(resultVar, JsonRequestBehavior.AllowGet);
        }
    }
}