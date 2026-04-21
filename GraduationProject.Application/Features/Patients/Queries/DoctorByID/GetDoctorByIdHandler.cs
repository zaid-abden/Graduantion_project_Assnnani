using Fluent.Infrastructure.FluentModel;
using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Features.Doctors.Dtos;
using GraduationProject.Application.Features.Doctors.Queries.GetDoctorById;
using GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;


namespace GraduationProject.Application.Features.Patients.Queries.DoctorByID
{
    public class GetDoctorByIdHandler
    : IRequestHandler<GetDoctorByIdQuery, Result<DoctorById_Dto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;

        public GetDoctorByIdHandler(IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        public async Task<Result<DoctorById_Dto>> Handle(
            GetDoctorByIdQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                //var doctor = await _context.Doctors
                //    .Include(d => d.User)
                //    .Include(d => d.Feedbacks)
                //    .FirstOrDefaultAsync(d => d.DoctorId == request.Id);

                var doctor = await _unitOfWork.Doctors.GetByIdAsync(request.DoctorId);

                if (doctor == null)
                    return Result<DoctorById_Dto>.NotFound("Doctor not found");

                var dto = new DoctorById_Dto
                {
                    DoctorId = doctor.DoctorId,
                    FullName = doctor.User.FirstName + " " + doctor.User.LastName,
                    Email = doctor.User.Email,
                    Phone = doctor.User.PhoneNumber,

                    Specialization = doctor.Specialization.Name,
                    YearsOfService = doctor.YearsOfService,
                    Degree = doctor.Degree.ToString(),
                    About = doctor.About,

                    City = doctor.City,
                    Country = doctor.Country,

                    Rating = doctor.Feedbacks.Any()
                        ? doctor.Feedbacks.Average(f => f.Rating)
                        : 0
                };

                return Result<DoctorById_Dto>.Success(dto);
            }
            catch (Exception ex)
            {
                // أي error unexpected
                return Result<DoctorById_Dto>.InternalError(ex.Message);
            }
        }
    }
}
