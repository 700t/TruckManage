using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MS.WebApi.Controllers
{
    //[Route("[controller]")]
    [Route("/mp/v1_0/[controller]/[action]")]
    [Authorize]
    public class AuthorizeV1Controller : ControllerBase
    {
    }
}