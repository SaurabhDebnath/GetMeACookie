using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GetMeACookie.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(Models.User user)
        {
            if (ModelState.IsValid)
            {
                if (user.IsValid(user.UserName, user.Password))
                {
                    //FormsAuthentication.SetAuthCookie(user.UserName, user.RememberMe);

                    FormsAuthenticationTicket authTicket
                        = new FormsAuthenticationTicket(1, user.UserName, DateTime.Now,
                        DateTime.Now.Add(FormsAuthentication.Timeout), user.RememberMe, "your custom data");

                    string encryptedAuthTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedAuthTicket);

                    if (user.RememberMe)
                        authCookie.Expires = authTicket.Expiration;

                    authCookie.Path = FormsAuthentication.FormsCookiePath;
                    Response.Cookies.Add(authCookie);

                    HttpCookie cookie1 = new HttpCookie("stuff", "details");
                    cookie1.Expires = authTicket.Expiration;
                    Response.Cookies.Add(cookie1);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View(user);
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }

    }
}