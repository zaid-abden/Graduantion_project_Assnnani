using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.StartReview
{
    public class StartReviewCommand : IRequest<Result<string>>
    {
        public int ScanId { get; set; }

        public StartReviewCommand(int scanId)
        {
            ScanId = scanId;
        }
    }
}
