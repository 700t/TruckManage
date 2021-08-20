using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MS.WebCore.Core;
using Qiniu2;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MS.WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UploadController : AuthorizeV1Controller //ControllerBase
    {

        [HttpPost]
        public IActionResult Image([FromForm(Name ="photo")] IFormFile file)
        {
            if (file == null) return BadRequest();
            string extend = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            Stream stream = file.OpenReadStream();
            byte[] data = new BinaryReader(stream).ReadBytes((int)stream.Length);
            string url = QiniuUploadHelper.UploadFile(extend, data);
            var result = new ExecuteResult<dynamic>(new { url });
            return Ok(result);
        }
    }
}
