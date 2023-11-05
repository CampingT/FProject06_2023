using FProjectCampingBackend.Models.EFModels;
using FProjectCampingBackend.Models.Infra;
using FProjectCampingBackend.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace FProjectCampingBackend.Models.Services
{
    public interface ILoginService
    {
        void ValidLogin(LoginVm vm);
        (string ReturnUrl, HttpCookie Cookie) ProcessLogin(LoginVm vm);
    }

    public class LoginService : ILoginService
    {
        public void ValidLogin(LoginVm vm)
        {
            var db = new AppDbContext();
            var user = db.Users.FirstOrDefault(p => p.Account == vm.Account);

            if (user == null)
            {
                throw new Exception("帳號或密碼有誤");
            }
            if (user.Password != vm.Password)
            {
                throw new Exception("帳號或密碼有誤");
            }
        }

        public (string ReturnUrl, HttpCookie Cookie) ProcessLogin(LoginVm vm)
        {
            var rememberMe = true;
            var account = vm.Account;
            var roles = string.Empty;

            var ticket =
                new FormsAuthenticationTicket(
                    1,
                    account,
                    DateTime.Now,
                    DateTime.Now.AddDays(10),
                    rememberMe,
                    roles,
                    "/"
                );

         
            var value = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

            var url = FormsAuthentication.GetRedirectUrl(account, true);

            return (url, cookie);
        }
    }
}