using FProjectCampingBackend.Models.EFModels;
using FProjectCampingBackend.Models.Repositories;
using FProjectCampingBackend.Models.Services;
using FProjectCampingBackend.Models.ViewModels.Home;
using FProjectCampingBackend.Models.ViewModels.Orders;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace FProjectCampingBackend.Controllers
{

    public class HomeController : Controller
	{
        private readonly LoginService _loginService = new LoginService();
        private AppDbContext db = new AppDbContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            try
            {
                _loginService.ValidLogin(vm);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            var processResult = _loginService.ProcessLogin(vm);

            Response.Cookies.Add(processResult.Cookie);
            return Redirect(processResult.ReturnUrl);
        }


        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("/Home/Login/");
        }

        [Authorize]
        public ActionResult Index(int? page,int pageSize=2)
        {
            string connectionString = "data source=.\\sql2022;initial catalog=FDB06;user id=sa5;password=sa5;MultipleActiveResultSets=True;App=EntityFramework\" providerName=\"System.Data.SqlClient";
   
            int pageNumber = (page ?? 1); 

            using (var dbConnection = new SqlConnection(connectionString))
            {
                var repo = new MemberRepository(dbConnection);
                var getmember = repo.GetLatestMembers();
				var getNewOrders = repo.GetNewOrders();
                var getTodayCheck = repo.GetTodayCheckOrder();
                var getUser = repo.GetUser();
                ViewBag.NewOrders = getNewOrders;
                ViewBag.UserAccount = getUser;

                //今日入住page
                //var todayCheckList = getTodayCheck.ToList(); 
                IPagedList<TodayCheckVm> todayCheckPagedList = getTodayCheck.ToPagedList(pageNumber, pageSize);

                ViewBag.TodayCheck = todayCheckPagedList;

                return View(getmember);
            }
        }

	}
}