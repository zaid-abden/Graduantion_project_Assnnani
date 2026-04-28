using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Services;
using GraduationProject.Application.Features.Auth.Dtos;
//using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Application.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Infrastructure.Services
{
    public class FileStorageService : IFileServices
    {
        public readonly FileStorageSettings fileStorageSettings;
        private readonly IWebHostEnvironment webHostEnvironment;
        public FileStorageService(IOptions<FileStorageSettings> options, IWebHostEnvironment webHostEnvironment)
        {
            fileStorageSettings = options.Value;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<Result<byte[]>> DownloadFileAsync(string url)
        {
            var fullPath = Path.Combine(webHostEnvironment.WebRootPath, url);
            if (!File.Exists(fullPath))
                return Result<byte[]>.Failure(ResultStatus.Failure, "File not exist");

            var fileBytes = await File.ReadAllBytesAsync(fullPath);
            return Result<byte[]>.Success(fileBytes);
        }

        public Result<string> Remove(string url)
        {
            var fullPath = Path.Combine(webHostEnvironment.WebRootPath, url);
            if (!File.Exists(fullPath))
                return Result<string>.Failure(ResultStatus.Failure, $"file not exist");
            File.Delete(fullPath);
            return Result<string>.Success("File Remove");
        }
        public Task<Result<MediaDto>> UploadImageAsync(IFormFile file)
      => UploadAsync(file, new[] { ".jpg", ".png", ".jpeg" }, fileStorageSettings.ImageSizeInMB, fileStorageSettings.ImagesFolder);

        //public Task<Result<MediaDto>> UploadImageAsync(IFormFile file) =>
        //   UploadAsync(file, fileStorageSettings.ImageExtentionAllowed, fileStorageSettings.ImageSizeInMB, fileStorageSettings.ImagesFolder);

        public Task<Result<MediaDto>> UploadVideoAsync(IFormFile file) =>
            UploadAsync(file, fileStorageSettings.VideoExtentionAllowed, fileStorageSettings.VideoSizeInMB, fileStorageSettings.VideosFolder);
        public Task<Result<MediaDto>> UploadFileAsync(IFormFile file) =>
            UploadAsync(file, fileStorageSettings.FileExtentionAllowed, fileStorageSettings.FileSizeInMB, fileStorageSettings.FilesFolder);

        private Result<string> ValidateFile(IFormFile file, string[] allowedExtensions, int maxSizeInMB)
        {
            if (file == null || file.Length == 0)
                return Result<string>.Failure(ResultStatus.Failure, "File cannot be null or empty.");

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Any(e => e.Equals(extension, StringComparison.OrdinalIgnoreCase)))
                return Result<string>.Failure(ResultStatus.Failure, $"Extension '{extension}' is not allowed.");

            if (file.Length > maxSizeInMB * 1024 * 1024)
                return Result<string>.Failure(ResultStatus.Failure, $"File size exceeds {maxSizeInMB}MB.");

            return Result<string>.Success("");
        }

        private async Task<Result<MediaDto>> UploadAsync(IFormFile file, string[] extentionAllowed, int maxSizeInMB, string folderName)
        {
            //var validationResult = ValidateFile(file, extentionAllowed, maxSizeInMB);
            //if (!validationResult.IsSuccess)
            //    return Result<MediaDto>.Failure(ResultStatus.Failure, validationResult.Error);

            //var folderPath = EnsureFolder(folderName);
            //string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            //string newName = $"{Guid.NewGuid()}{extension}";
            //string fullPath = Path.Combine(folderPath, newName);

            //using (var fileStream = new FileStream(fullPath, FileMode.Create))
            //    await file.CopyToAsync(fileStream);

            //var mediaDto = new MediaDto
            //{
            //    ContentType = file.ContentType,
            //    FileUrl = $"{fileStorageSettings.UploadsFolder}/{folderName}/{newName}",
            //};
            //return Result<MediaDto>.Success(mediaDto);
            var validationResult = ValidateFile(file, extentionAllowed, maxSizeInMB);
            if (!validationResult.IsSuccess)
                return Result<MediaDto>.Failure(ResultStatus.Failure, validationResult.Error);

            var folderPath = EnsureFolder(folderName);

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            string newName = $"{Guid.NewGuid()}{extension}";
            string fullPath = Path.Combine(folderPath, newName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
                await file.CopyToAsync(fileStream);

            var fileSizeInKB = file.Length / 1024.0;
            var fileSizeInMB = file.Length / (1024.0 * 1024.0);

            var mediaDto = new MediaDto
            {
                FileUrl = $"{fileStorageSettings.UploadsFolder}/{folderName}/{newName}",
                ContentType = file.ContentType,
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                FileType = extension,
                FileSize = file.Length
            };

            return Result<MediaDto>.Success(mediaDto);
        }
        private string EnsureFolder(string folderName)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, fileStorageSettings.UploadsFolder, folderName);
            Directory.CreateDirectory(path);
            return path;
        }

    }
 
}
