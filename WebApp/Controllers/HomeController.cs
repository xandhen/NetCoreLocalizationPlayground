using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using lib.x;
using System.Resources;
using System.Globalization;

namespace WebApp
{
	public class HomeController : Controller
    {
        IStringLocalizer<Class1> _resources;
        IStringLocalizer<LocalClass> _localResources;

        public HomeController(IStringLocalizer<Class1> resources, IStringLocalizer<LocalClass> localResources)
        {
            _resources = resources;
            _localResources = localResources;
        }
        
        public IActionResult Index()
        {

            ViewData["LocalizedMessage"] = _resources["Test"];
            ViewData["LocalizedMessage2"] = $"{CultureInfo.CurrentCulture.TwoLetterISOLanguageName}:{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}";
            ViewData["LocalizedMessage4"] = _localResources["Test"];

            ResourceManager rm = new ResourceManager(typeof(Class1));
            ViewData["LocalizedMessage3"]  = rm.GetString("Test");
            var view = View();
            return view;
        }
    }
}