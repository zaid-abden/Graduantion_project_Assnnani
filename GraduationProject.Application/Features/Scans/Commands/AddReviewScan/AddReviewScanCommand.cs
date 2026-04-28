using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.AddReviewScan
{
    public class AddReviewScanCommand : IRequest<Result<string>>
    {
        public int ScanId { get; set; }

        public string Findings { get; set; } = null!;
        public string Recommendations { get; set; } = null!;
    }
}
