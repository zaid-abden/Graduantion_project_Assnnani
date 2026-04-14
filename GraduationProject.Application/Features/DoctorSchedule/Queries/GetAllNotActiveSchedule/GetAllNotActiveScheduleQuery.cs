using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.DoctorSchedule.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.DoctorSchedule.Queries.GetAllNotActiveSchedule
{
    public class GetAllNotActiveScheduleQuery : IRequest<Result<List<DoctorScheduleDto>>>
    {
    }
}
