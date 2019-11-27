using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using YfCloud.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace YfCloud.App.Module.FileUpload.Controllers
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly AppConfig _appConfig;

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="option"></param>
        public FilesController(IOptions<AppConfig> option)
        {
            _appConfig = option.Value;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file">需要上传的文件,只能上传单个文件</param>
        /// <returns></returns>
        [HttpOptions("Upload")]
        [HttpPost("Upload")]
        public IActionResult Post(IFormFile file)
        {
            if (string.IsNullOrEmpty(_appConfig.FileUploadDirectory))
                return new NoContentResult();
            if (file == null&&( Request.Form.Files==null|| Request.Form.Files.Count==0))
                return new NotFoundResult();

            var fileAim = file == null ? Request.Form.Files[0] : file;
            try
            {
                var fileDir = $"{_appConfig.FileUploadDirectory}/{DateTime.Now.ToString("yyyyMMdd")}";
                if (!Directory.Exists(fileDir))
                {
                    Directory.CreateDirectory(fileDir);
                }
                //文件名称
                var currFileName = fileAim.FileName;
                var fileSuffix = currFileName.Substring(currFileName.LastIndexOf("."));
                var newFileName = GetNewFileName();
                currFileName = newFileName + fileSuffix;
                //上传的文件的路径
                var filePath = $"{fileDir}/{currFileName}";
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    fileAim.CopyTo(fs);
                    fs.Flush();
                }
                return new OkObjectResult($"{DateTime.Now.ToString("yyyyMMdd")}/{currFileName}");
            }
            catch
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// 删除不需要上传的文件
        /// </summary>
        /// <param name="strFileName">需要删除的文件名称,需要带后缀</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public IActionResult Delete(string strFileName)
        {
            if (string.IsNullOrEmpty(_appConfig.FileUploadDirectory))
                return new NoContentResult();
            if (string.IsNullOrEmpty(strFileName))
                return new NotFoundResult();
            try
            {
                var filePath = $"{_appConfig.FileUploadDirectory}/{strFileName}";
                if (!System.IO.File.Exists(filePath))
                {
                    return new OkObjectResult("文件不存在，无需删除");
                }
                else
                {
                    System.IO.File.Delete(filePath);
                    return new OkObjectResult("删除文件成功!");
                }
            }
            catch
            {
                return new NotFoundResult();
            }
        }

        private string GetNewFileName()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
