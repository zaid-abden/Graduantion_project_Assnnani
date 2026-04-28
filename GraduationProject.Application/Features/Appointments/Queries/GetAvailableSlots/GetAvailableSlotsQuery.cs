using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Appointments.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.Appointments.Queries.GetAvailableSlots
{
    public class GetAvailableSlotsQuery
        : IRequest<Result<List<AvailableSlotDto>>>
    {
        public int DoctorId { get; set; }
        public DateOnly Date { get; set; }

        public GetAvailableSlotsQuery(int doctorId, DateOnly date)
        {
            DoctorId = doctorId;
            Date = date;
        }
    }
}
