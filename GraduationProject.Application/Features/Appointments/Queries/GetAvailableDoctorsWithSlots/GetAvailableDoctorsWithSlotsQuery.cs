using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Appointments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAvailableDoctorsWithSlots
{
    public class GetAvailableDoctorsWithSlotsQuery
     : IRequest<Result<List<AvailableDoctorWithSlotsDto>>>
    {
        public DateOnly Date { get; set; }
        public string? Location { get; set; }
        public int? SpecializationId { get; set; }

        public GetAvailableDoctorsWithSlotsQuery(DateOnly date, string? location, int ? specialization)
        {
            Date = date;
            Location = location;
            this.SpecializationId = specialization;
        }
    }
}
