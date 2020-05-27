using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace asp.net_NoteB.Controllers
{
    public class UploadController : Controller
    {
        private readonly IHostingEnvironment _environment;

        public UploadController(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        // http://www.example.com/Upload/ImageUpload
        // http://www.example.com/api/upload

        [HttpPost, Route("api/upload")]
        public IActionResult ImageUpload(IFormFile file)
        {
            // #이미지나 파일을 업로드할 때 필요한 구성
            // 1. Path(경로) - 어디에 저장할 지 결정
            var path = Path.Combine(_environment.WebRootPath, @"images\upload");
            // 2. Name(이름) - DateTime, GUID
            // 3. Extension(확장자) - jpg, png...txt
            var fileName = file.FileName;

            using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return Ok(new { file = "/images/upload/"+ fileName, success = true});
        }
    }
}
