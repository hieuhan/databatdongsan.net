using databatdongsan.library;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class HeaderVM : BaseViewModel
    {
        public UserSessionVM UserSession { get; set; }
        public List<Actions> ActionsList { get; set; }
    }
}