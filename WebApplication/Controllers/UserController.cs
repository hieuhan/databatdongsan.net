using databatdongsan.helper;
using databatdongsan.library;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication.Helpers.Attributes;
using WebApplication.Helpers.Extensions;
using WebApplication.Helpers.Sercurity;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    [DatabatdongsanAuthorize]
    public class UserController : Controller
    {
        private readonly string redirectUrl = "UserRedirectUrl";
        //private readonly UserSessionVM _currentUser = SessionHelper.Get<UserSessionVM>(ConstantHelper.UserSession);

        [HttpGet]
        public async Task<ActionResult> Index(string keyword = "", byte userStatusId = 0, int orderBy = 3, int page = 1)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Users users = new Users
            {
                ActBy = myPrincipal.UserName,
                UserStatusId = userStatusId
            };

            SessionHelper.Save(redirectUrl, Request.RawUrl);

            keyword = keyword.StripTags();
            string orderByClause = string.Empty;

            if (orderBy > 0)
            {
                switch (orderBy)
                {
                    case 1:
                        orderByClause = "CrDateTime DESC";
                        break;
                    case 2:
                        orderByClause = "UpdDateTime DESC";
                        break;
                    case 3:
                        orderByClause = "Counter DESC";
                        break;
                    case 4:
                        orderByClause = "Counter";
                        break;
                }
            }

            var usersTask = users.GetPage(keyword, orderByClause, page > 0 ? page - 1 : 0, ConstantHelper.PageSize);
            var userStatusTask = UserStatus.Static_GetList();

            await Task.WhenAll(usersTask, userStatusTask);

            var model = new UserViewModel
            {
                OrderBy = orderBy,
                UsersList = usersTask.Result.Item1,
                UserStatusList = userStatusTask.Result,
                Pagination = new PaginationVM(page, ConstantHelper.PageSize, usersTask.Result.Item2)
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Insert()
        {
            var gendersTask = Genders.Static_GetList();
            var userStatusTask = UserStatus.Static_GetList();
            var rolesTask = Roles.Static_GetList();

            await Task.WhenAll(gendersTask, userStatusTask, rolesTask);

            var model = new UserInsertVM
            {
                PreviousPage = SessionHelper.Get<string>(redirectUrl),
                GendersList = gendersTask.Result,
                UserStatusList = userStatusTask.Result,
                RolesList = rolesTask.Result
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(UserInsertVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Users userInsert = new Users
                {
                    ActBy = myPrincipal.UserName
                };

                userInsert.CopyPropertiesFrom(model);
                userInsert.Password = HashHelper.HashPassword(model.Password);

                string rootDir = Request.PhysicalApplicationPath,
                    filePath = string.Empty;

                if (model.PostedFile != null)
                {
                    filePath = FileUploadHelper.SaveFile(model.PostedFile,
                            rootDir, ConstantHelper.MediaPath, true);
                }

                if (!string.IsNullOrEmpty(filePath))
                {
                    userInsert.Avatar = filePath.Replace("\\", "/");
                }

                if (model.Day > 0 && model.Month > 0 && model.Year > 0)
                {
                    userInsert.BirthDay = new DateTime(model.Year, model.Month, model.Day);
                }

                Tuple<string, string> userInsertResult = await userInsert.Insert();

                if (userInsertResult.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.RoleIds.IsAny())
                    {
                        await UserRoles.Static_InsertMultiple(myPrincipal.UserName, userInsert.UserId, string.Join(",", model.RoleIds));
                    }

                    this.ToastrSuccess("Thông báo", userInsertResult.Item2);
                }
                else
                {
                    this.ToastrError("Thông báo", userInsertResult.Item2);
                }

                return Redirect(StringHelper.RedirectUrl(redirectUrl, Url.Action("Index", "User")));
            }

            var gendersTask = Genders.Static_GetList();
            var userStatusTask = UserStatus.Static_GetList();
            var rolesTask = Roles.Static_GetList();

            await Task.WhenAll(gendersTask, userStatusTask, rolesTask);

            model.PreviousPage = SessionHelper.Get<string>(redirectUrl);
            model.GendersList = gendersTask.Result;
            model.UserStatusList = userStatusTask.Result;
            model.RolesList = rolesTask.Result;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Update(int userId = 0)
        {
            if (userId <= 0)
            {
                return RedirectToAction("BadRequest", "Error");
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Users userUpdate = await Users.Static_GetById(myPrincipal.UserName, userId);

            if (userUpdate == null || userUpdate.UserId <= 0)
            {
                return RedirectToAction("NotFound", "Error");
            }

            var gendersTask = Genders.Static_GetList();
            var userStatusTask = UserStatus.Static_GetList();
            var rolesByUserTask = Roles.Static_GetListBy(myPrincipal.UserName, userId);

            await Task.WhenAll(gendersTask, userStatusTask, rolesByUserTask);

            var model = new UserUpdateVM
            {
                Users = userUpdate,
                RolesList = rolesByUserTask.Result,
                PreviousPage = SessionHelper.Get<string>(redirectUrl),
                GendersList = gendersTask.Result,
                UserStatusList = userStatusTask.Result
            };

            model.CopyPropertiesFrom(userUpdate);

            if (userUpdate.BirthDay != DateTime.MinValue)
            {
                model.Day = userUpdate.BirthDay.Day;
                model.Month = userUpdate.BirthDay.Month;
                model.Year = userUpdate.BirthDay.Year;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(UserUpdateVM model)
        {
            if (model.UserId <= 0)
            {
                return RedirectToAction("BadRequest", "Error");
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            model.Users = await Users.Static_GetById(myPrincipal.UserName, model.UserId);

            if (model.Users == null || model.Users.UserId <= 0)
            {
                return RedirectToAction("NotFound", "Error");
            }

            //if (model.Users.BuildIn > 0 && model.Users.UserId != _currentUser.UserId)
            //{
            //    return RedirectToAction("AccessDenied", "Error");
            //}

            if (ModelState.IsValid)
            {
                model.Users.ActBy = myPrincipal.UserName;
                model.Users.FullName = model.FullName;
                model.Users.Email = model.Email.TrimmedOrDefault(string.Empty);
                model.Users.Mobile = model.Mobile.TrimmedOrDefault(string.Empty);
                model.Users.GenderId = model.GenderId;
                model.Users.Address = model.Address.TrimmedOrDefault(string.Empty);
                if (model.Users.BuildIn <= 0)
                    model.Users.UserStatusId = model.UserStatusId;

                string rootDir = Request.PhysicalApplicationPath,
                    filePath = string.Empty;

                if (model.PostedFile != null)
                {
                    filePath = FileUploadHelper.SaveFile(model.PostedFile,
                            rootDir, ConstantHelper.MediaPath, true);
                }

                if (!string.IsNullOrEmpty(filePath))
                {
                    model.Users.Avatar = filePath.Replace("\\", "/");
                }

                if (model.Day > 0 && model.Month > 0 && model.Year > 0)
                {
                    model.Users.BirthDay = new DateTime(model.Year, model.Month, model.Day);
                }

                Tuple<string, string> userUpdateResult = await model.Users.Update();

                if (userUpdateResult.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if(model.Users.BuildIn <= 0)
                    {
                        Tuple<string, string> userRolesInsertResult = await UserRoles.Static_InsertMultiple(myPrincipal.UserName, model.Users.UserId, model.RoleIds.IsAny() ? string.Join(",", model.RoleIds) : string.Empty,
                        model.RoleIdsRemove.IsAny() ? string.Join(",", model.RoleIdsRemove) : string.Empty);

                        if (userRolesInsertResult.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                        {
                            this.ToastrSuccess("Thông báo", userUpdateResult.Item2);
                        }
                        else
                        {
                            this.ToastrError("Thông báo", userRolesInsertResult.Item2);
                        }
                    }
                    else
                    {
                        this.ToastrSuccess("Thông báo", userUpdateResult.Item2);
                    }
                }
                else
                {
                    this.ToastrError("Thông báo", userUpdateResult.Item2);
                }

                return Redirect(StringHelper.RedirectUrl(redirectUrl, Url.Action("Index", "User")));
            }

            var gendersTask = Genders.Static_GetList();
            var userStatusTask = UserStatus.Static_GetList();
            var rolesByUserTask = Roles.Static_GetListBy(myPrincipal.UserName, model.Users.UserId);

            await Task.WhenAll(gendersTask, userStatusTask, rolesByUserTask);

            model.PreviousPage = SessionHelper.Get<string>(redirectUrl);
            model.GendersList = gendersTask.Result;
            model.UserStatusList = userStatusTask.Result;
            model.RolesList = rolesByUserTask.Result;

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int userId = 0)
        {
            if (userId <= 0)
            {
                return RedirectToAction("BadRequest", "Error");
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Users userDelete = await Users.Static_GetById(myPrincipal.UserName, userId);

            if (userDelete == null || userDelete.UserId <= 0)
            {
                return RedirectToAction("NotFound", "Error");
            }

            userDelete.ActBy = myPrincipal.UserName;

            Tuple<string, string> userInsertResult = await userDelete.Delete();

            if (userInsertResult.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                this.ToastrSuccess("Thông báo", userInsertResult.Item2);
            }
            else
            {
                this.ToastrError("Thông báo", userInsertResult.Item2);
            }

            return Redirect(StringHelper.RedirectUrl(redirectUrl, Url.Action("Index", "User")));
        }
    }
}