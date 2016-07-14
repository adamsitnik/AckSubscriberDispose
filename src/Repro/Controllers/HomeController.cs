using Microsoft.AspNetCore.Mvc;

namespace Repro.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(SomeClassThatHasRequiresSignalR someClassThatHasRequiresSignalR)
        {
            SomeClassThatHasRequiresSignalR = someClassThatHasRequiresSignalR;
        }

        private SomeClassThatHasRequiresSignalR SomeClassThatHasRequiresSignalR { get; }

        [HttpGet]
        public IActionResult Index()
        {
            return new EmptyResult();
        }
    }
}
