using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Scans.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Doctors.Queries.GetPendingScans
{
    public class GetPendingScansQuery : IRequest<Result<List<ScanDto>>>
    {
    }
}
