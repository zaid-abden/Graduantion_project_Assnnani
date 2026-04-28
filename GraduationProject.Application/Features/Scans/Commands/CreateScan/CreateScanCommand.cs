using GraduationProject.Application.Common.Results;
using GraduationProject.Data.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.CreateScan
{
    public class CreateScanCommand : IRequest<Result<int>>
    {
        public IFormFile File { get; set; } 

        public int PatientId { get; set; }
        public ScanType ScanType { get; set; }
        public ScanPriority Priority { get; set; } = ScanPriority.Normal;
        public string? Notes { get; set; }

    }
}
