using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
    public interface IFileService
    {
        Task<string?> HandleFileUploadAsync(IFormFile? file, string? existingFileUrl,
            string folderPath, bool removeFile = false, string? defaultFileUrl = null);

        bool IsDefaultImage(string? imageUrl, string? defaultImageUrl);
    }
}
