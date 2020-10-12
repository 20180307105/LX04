using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Service.Controllers
{
    [Route("api/[controller]")]
  
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public ActionResult Post(List<IFormFile>files)
        {
            var path = Path.Combine(_hostingEnvironment.WebRootPath, "img", "editor");
            string fName = Guid.NewGuid().ToString().Replace("-", "");
            var fileName = $"{path}/{fName}";
            try
            {
                var ext = DAL.Upload.Instance.UpImg(files[0], fileName);
                if (ext == null)
                    return Json(Result.Err("请上传文件"));
                else
                {
                    var file = $"https://{HttpContext.Request.Host.Value}/img/Editor/{fName}{ext}";
                    return Json(Result.Ok("上传成功", file));
                }
            }
            catch (Exception ex)
            {
                return Json(Result.Err(ex.Message));
            }
        }
    }
}
