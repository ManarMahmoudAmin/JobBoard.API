using JobBoard.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public DocumentService(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var uploadPath = Path.Combine(_env.WebRootPath, folderPath);

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{_configuration["ApiBaseUrl"]}/{folderPath.Replace("\\", "/")}/{fileName}";
        }

        public void DeleteFile(string fileUrl, string folderPath)
        {
            if (string.IsNullOrEmpty(fileUrl)) return;

            var fileName = Path.GetFileName(fileUrl);
            var fullPath = Path.Combine(_env.WebRootPath, folderPath, fileName);

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

    }
}
