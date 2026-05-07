using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Receptionists.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistDashboard
{
    public class GetReceptionistDashboardQuery : IRequest<Result<ReceptionistDashboardDto>>
    {
    }
}
