using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Receptionists.Dtos;
using GraduationProject.Data.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Receptionists.Queries.GetReceptionistAppointmentsDashboard
{
    public class GetReceptionistAppointmentsDashboardQuery:IRequest<Result<ReceptionistAppointmentsDashboardDto>>
    {
        public string? Search { get; set; }

        public BookingType? BookingType { get; set; } 
        public ReceptionistAppointmentStatus? AppointmentStatus { get; set; }
    }
}
