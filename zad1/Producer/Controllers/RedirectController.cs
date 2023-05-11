using Microsoft.AspNetCore.Mvc;

namespace Netland.Controllers;

[ApiController]
[Route("")]
[Route("api")]
[Route("api/email")]
public class RedirectController : Controller
{
    public ActionResult Index()
    {
        return RedirectPermanent("/api/email/send");
    }
}
