using System.Security.Principal;

namespace WebApplication.Helpers.Sercurity
{
    public class MyPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public MyPrincipal(string userName)
        {
            this.Identity = new GenericIdentity(userName);
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
    }
}