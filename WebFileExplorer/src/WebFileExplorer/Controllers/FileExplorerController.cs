using Microsoft.AspNetCore.Mvc;

namespace WebFileExplorer.Controllers
{
    public class FileExplorerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
