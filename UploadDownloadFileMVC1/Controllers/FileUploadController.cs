using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace UploadDownloadFileMVC1.Controllers
{
    public class FileUploadController : Controller
    {
        public IActionResult Index()
        {
            var items = GetFiles();
            return View(items);
        }

        [HttpPost]
        public IActionResult Index(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception error)
                {
                    ViewBag.Message = "ERROR: " + error.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            var items = GetFiles();
            return View(items);
        }

        public IActionResult Download(string ImageName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles", ImageName);
            return PhysicalFile(filePath, "application/force-download", Path.GetFileName(filePath));
        }

        private List<string> GetFiles()
        {
            var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadedFiles"));
            var fileNames = dir.GetFiles("*.pdf");

            var items = new List<string>();

            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            return items;
        }
    }
}
