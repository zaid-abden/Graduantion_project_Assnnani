using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Auth.Dtos;
//using GraduationProject.Application.Features.Doctors.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Contracts.Services
{
    public interface IFileServices
    {
        Task<Result<MediaDto>> UploadImageAsync(IFormFile file);
        Task<Result<MediaDto>> UploadVideoAsync(IFormFile file);
        Task<Result<MediaDto>> UploadFileAsync(IFormFile? file);
        Result<string> Remove(string url);
        Task<Result<byte[]>> DownloadFileAsync(string url);
    }
}
