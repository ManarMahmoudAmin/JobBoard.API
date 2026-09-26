using JobBoard.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace JobBoard.Infrastructure.Services
{
	public class FileService : IFileService
	{
		private readonly IWebHostEnvironment _env;
		private readonly IConfiguration _configuration;
        private readonly IDocumentService _documentService;

        public FileService(IWebHostEnvironment env, IConfiguration configuration, IDocumentService documentService)
        {
            _env = env;
            _configuration = configuration;
            _documentService = documentService;
        }

        // ===================== Handle generic file upload ====================
        public async Task<string?> HandleFileUploadAsync(
			IFormFile? file,
			string? existingFileUrl,
			string folderPath,
			bool removeFile = false,
			string? defaultFileUrl = null
		)
		{
			// If the user chose to remove the file
			if (removeFile)
			{
				if (!string.IsNullOrEmpty(existingFileUrl) && !IsDefaultImage(existingFileUrl, defaultFileUrl))
				{
                    _documentService.DeleteFile(existingFileUrl, folderPath);
				}
				return defaultFileUrl;
			}

			// If the user uploaded a new file
			if (file != null && file.Length > 0)
			{
				if (!string.IsNullOrEmpty(existingFileUrl) && !IsDefaultImage(existingFileUrl, defaultFileUrl))
				{
					_documentService.DeleteFile(existingFileUrl, folderPath);
				}
				var uploadedUrl = await _documentService.UploadFileAsync(file, folderPath);
				return uploadedUrl;
			}

			// Return existing or default if nothing changed
			return existingFileUrl ?? defaultFileUrl;
		}

		// ===================== Helper to check if image is default ====================
		public bool IsDefaultImage(string? imageUrl, string? defaultImageUrl)
		{
			if (string.IsNullOrEmpty(imageUrl) || string.IsNullOrEmpty(defaultImageUrl))
				return false;

			if (imageUrl.Equals(defaultImageUrl, StringComparison.OrdinalIgnoreCase))
				return true;

			// Check common default filenames
			if (imageUrl.EndsWith("/images/profilepic/user.jpg", StringComparison.OrdinalIgnoreCase) ||
				imageUrl.Contains("user.jpg", StringComparison.OrdinalIgnoreCase))
				return true;

			if (imageUrl.EndsWith("/images/companies/default.jpg", StringComparison.OrdinalIgnoreCase) ||
				imageUrl.Contains("default.jpg", StringComparison.OrdinalIgnoreCase))
				return true;

			return false;
		}
	}
}
