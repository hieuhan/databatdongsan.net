using databatdongsan.library;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class CustomerVM : BaseViewModel
    {
        public int UserId { get; set; }
        public Users Users { get; set; }
        public List<Customers> CustomersList { get; set; }
    }

    public class CustomerInsertOrUpdateVM : BaseViewModel
    {
        public int UserId { get; set; }
        public int CustomerId { get; set; }

        [Display(Name = "Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        //[Remote("CheckMobileAlreadyExists", "Ajax", HttpMethod = "Post")]
        //[Remote("CheckMobileAlreadyExists", "Ajax", HttpMethod = "Post", AdditionalFields = "CustomerId")]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression("^(84|0)\\d{9,10}$", ErrorMessage = "{0} không đúng ! \n {0} hợp lệ dạng 84xxxxxxxxx hoặc 0xxxxxxxxx")]
        public string Mobile { get; set; }

        [Display(Name = "Địa chỉ email")]
        [DataType(DataType.EmailAddress)]
        //[Remote("CheckEmailAlreadyExists", "Ajax", HttpMethod = "Post")]
        [RegularExpression(@"[a-zA-Z0-9_\.]+@[a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z]+)*", ErrorMessage = "{0} không hợp lệ !")]

        public string Email { get; set; }
        public string Avatar { get; set; }

        public string Note { get; set; }
        public int Counter { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
    }

    public class CustomerReportVM : BaseViewModel
    {
        public List<Customers> CustomersList { get; set; }
    }
}