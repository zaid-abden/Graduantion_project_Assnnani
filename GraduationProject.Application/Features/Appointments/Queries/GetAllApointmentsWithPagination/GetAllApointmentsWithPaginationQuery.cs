using GraduationProject.Application.Common.PaginatedResults;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Appointments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAllApointmentsWithPagination
{
    public class GetAllApointmentsWithPaginationQuery:IRequest<Result<PaginatedResult<AppointmentDtto>>>
    {
            public int PageSize { get; set; }
            public int PageNumber { get; set; }
        public GetAllApointmentsWithPaginationQuery(int pn , int ps)
        {
            PageNumber = pn;
            PageSize = ps;
        }
    }
}
