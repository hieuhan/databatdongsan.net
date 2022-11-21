using databatdongsan.library;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using WebApplication.Helpers.Attributes;

namespace WebApplication.Models
{
    public class UserViewModel : BaseViewModel
    {
        public byte UserStatusId { get; set; }
        public List<Users> UsersList { get; set; }
        public List<UserStatus> UserStatusList { get; set; }
    }

    public class UserLoginVM : BaseViewModel
    {
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class UserProfileVM : BaseViewModel
    {
        [Display(Name = "Họ và tên*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-zA-Z0-9_\.]+@[a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z]+)*", ErrorMessage = "{0} không hợp lệ !")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(84|0)\\d{9,10}$", ErrorMessage = "{0} không đúng ! \n {0} hợp lệ dạng 84xxxxxxxxx hoặc 0xxxxxxxxx")]
        public string Mobile { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Hình đại diện")]
        public string Avatar { get; set; }

        [AllowFileExtensions("jpg,jpeg,png,gif,svg", ErrorMessage = "Vui lòng chọn file ảnh.")]
        [AllowFileSize(FileSize = 20 * 1024 * 1024, ErrorMessage = "Kích thước file cho phép tối đa là 20 MB")]
        public HttpPostedFileBase PostedFile { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        [Display(Name = "Ghi chú")]
        public string Comment { get; set; }

        [Display(Name = "Giới tính")]
        public byte GenderId { get; set; }

        public List<Genders> GendersList { get; set; }
    }

    public class UserChangePasswordVM : BaseViewModel
    {
        [Display(Name = "Mật khẩu hiện tại")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(100, ErrorMessage = "{0} bao gồm từ {2} ký tự trở lên.", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(100, ErrorMessage = "{0} bao gồm từ {2} ký tự trở lên.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Display(Name = "Mật khẩu xác nhận")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(100, ErrorMessage = "{0} bao gồm từ {2} ký tự trở lên.", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "{0} không đúng.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserInsertVM : BaseViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Tên tài khoản")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage =
            "{0} không bao gồm khoảng trắng, chỉ bao gồm chữ cái và số.")]
        public string UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(100, ErrorMessage = "{0} bao gồm từ {2} ký tự trở lên.", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "Mật khẩu xác nhận")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(100, ErrorMessage = "{0} bao gồm từ {2} ký tự trở lên.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "{0} không chính xác")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-zA-Z0-9_\.]+@[a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z]+)*", ErrorMessage = "{0} không hợp lệ !")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(84|0)\\d{9,10}$", ErrorMessage = "{0} không đúng ! \n {0} hợp lệ dạng 84xxxxxxxxx hoặc 0xxxxxxxxx")]
        public string Mobile { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Hình đại diện")]
        public string Avatar { get; set; }

        [AllowFileExtensions("jpg,jpeg,png,gif,svg", ErrorMessage = "Vui lòng chọn file ảnh.")]
        [AllowFileSize(FileSize = 20 * 1024 * 1024, ErrorMessage = "Kích thước file cho phép tối đa là 20 MB")]
        public HttpPostedFileBase PostedFile { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        [Display(Name = "Ghi chú")]
        public string Comment { get; set; }

        [Display(Name = "Giới tính")]
        public byte GenderId { get; set; }

        [Display(Name = "Trạng thái")]
        public byte UserStatusId { get; set; }

        public Users Users { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> RoleIdsRemove { get; set; }
        public List<Roles> RolesList { get; set; }
        public List<Genders> GendersList { get; set; }

        public List<UserStatus> UserStatusList { get; set; }
    }

    public class UserUpdateVM : BaseViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string FullName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-zA-Z0-9_\.]+@[a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z]+)*", ErrorMessage = "{0} không hợp lệ !")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("^(84|0)\\d{9,10}$", ErrorMessage = "{0} không đúng ! \n {0} hợp lệ dạng 84xxxxxxxxx hoặc 0xxxxxxxxx")]
        public string Mobile { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Hình đại diện")]
        public string Avatar { get; set; }

        [AllowFileExtensions("jpg,jpeg,png,gif,svg", ErrorMessage = "Vui lòng chọn file ảnh.")]
        [AllowFileSize(FileSize = 20 * 1024 * 1024, ErrorMessage = "Kích thước file cho phép tối đa là 20 MB")]
        public HttpPostedFileBase PostedFile { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        [Display(Name = "Ghi chú")]
        public string Comment { get; set; }

        [Display(Name = "Giới tính")]
        public byte GenderId { get; set; }

        [Display(Name = "Trạng thái")]
        public byte UserStatusId { get; set; }

        public Users Users { get; set; }
        public List<int> RoleIds { get; set; }
        public List<int> RoleIdsRemove { get; set; }
        public List<Roles> RolesList { get; set; }
        public List<Genders> GendersList { get; set; }

        public List<UserStatus> UserStatusList { get; set; }
    }

    public class UserSessionVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public byte GenderId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
    }

    public class UserImportExcelVM
    {
        [Required(ErrorMessage = "Vui lòng chọn file Excel cần nhập dữ liệu.")]
        [AllowFileExtensions("xlsx", ErrorMessage = "Vui lòng chọn file Excel.")]
        [AllowFileSize(FileSize = 20 * 1024 * 1024, ErrorMessage = "Kích thước file cho phép tối đa là 20 MB")]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}