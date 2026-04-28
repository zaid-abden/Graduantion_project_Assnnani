using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.RejectScan
{
    public class RejectScanCommand : IRequest<Result<string>>
    {
        public int ScanId { get; set; }

        public RejectScanCommand(int scanId)
        {
            ScanId = scanId;
        }
    }
}
