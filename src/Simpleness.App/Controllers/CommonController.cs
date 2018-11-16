using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Options;
using Simpleness.Infrastructure.AspNetCore.Captcha;
using Microsoft.AspNetCore.Authorization;

namespace Simpleness.App.Controllers
{


    /// <summary>
    /// 网站通用功能
    /// </summary>
    [AllowAnonymous]
    public class CommonController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly CaptchaOption _captchaOption;

        public CommonController(IHostingEnvironment hostingEnvironment,IOptions<CaptchaOption> options)
        {
            _hostingEnvironment = hostingEnvironment;
            _captchaOption = options.Value;
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
       
        [HttpGet("get-captcha-image")]
        [ProducesResponseType(200)]
        public IActionResult GetCaptchaImage()
        {
            var captchaCode = Captcha.GenerateCaptchaCode(_captchaOption.WordLenth);
            var result = Captcha.GenerateCaptchaImage(_captchaOption.Width, _captchaOption.Height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return  File(s, "image/png");             
        }

    }
}