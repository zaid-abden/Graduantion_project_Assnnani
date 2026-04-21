using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Patients.Queries.DoctorByID;
using MediatR;

namespace GraduationProject.Application.Features.Doctors.Queries.GetDoctorById
{
    public class GetDoctorByIdQuery : IRequest<Result<DoctorById_Dto>>
    {
        public int DoctorId { get; set; }
    }
}