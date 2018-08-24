using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Simpleness.App.Controllers
{

    /// <summary>
    /// 文件上传/下载管理
    /// </summary>
    public class UploadController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;


        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 文件上传
        /// </summary>     
        /// <returns></returns>
        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<IActionResult> UploadAsync()
        {
            var files = HttpContext.Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var fileExt = Path.GetExtension(formFile.FileName);//文件扩展名，不含“.”               
                    long fileSize = formFile.Length; //获得文件大小，以字节为单位
                    string newFileName = Guid.NewGuid().ToString() + fileExt; //随机生成新的文件名
                    var fileDir = Path.Combine(webRootPath, $"upload/{DateTime.Now.ToString("yyyyMMdd")}");
                    if (!Directory.Exists(fileDir))
                    {
                        Directory.CreateDirectory(fileDir);
                    }
                    var filePath = Path.Combine(fileDir, newFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
            return Ok(new { count = files.Count, size });
        }
    }
}