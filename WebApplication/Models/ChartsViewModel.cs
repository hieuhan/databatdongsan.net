using databatdongsan.library;
using System.Collections.Generic;

namespace WebApplication.Models
{
    public class ChartsViewModel : BaseViewModel
    {
        public List<Users> UsersList { get; set; }
        public List<Customers> CustomersList { get; set; }
    }
}