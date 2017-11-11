using System.Web.Mvc;

namespace GetMeACookie.Controllers
{
    public class AccountDetailsController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}