using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Scans.Commands.ReopenScan
{
    public class ReopenScanCommand : IRequest<Result<string>>
    {
        public int ScanId { get; set; }

        public ReopenScanCommand(int scanId)
        {
            ScanId = scanId;
        }
    }
}
