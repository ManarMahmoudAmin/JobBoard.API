using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IDocumentService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderPath);
        void DeleteFile(string fileUrl, string folderPath);
    }
}
