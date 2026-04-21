//using GraduationProject.Application.Common.Results;
//using GraduationProject.Application.Contracts.Repositories;
//using GraduationProject.Data.Identity;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion
//{

//    internal class DoctorFilterationQueryHandler : IRequestHandler<DoctorFilterationQuery, Result<PagedResult<DoctorFDTO>>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly UserManager<User> _userManager;

//        public DoctorFilterationQueryHandler(IUnitOfWork unitOfWork,UserManager<User> userManager )
//        {
//            _unitOfWork = unitOfWork;
//            _userManager = userManager;
//        }
//        public Task<Result<PagedResult<DoctorFDTO>>> Handle(DoctorFilterationQuery request, CancellationToken cancellationToken)
//        {
//            var query = _unitOfWork.Doctors.GetAll();

//            // 🔹 FILTERS (ALL TRANSLATED TO SQL)
//            if (!string.IsNullOrWhiteSpace(request.Search))
//                query = query.Where(d => d.User.FullName.Contains(request.Search));

//            if (!string.IsNullOrWhiteSpace(request.City))
//                query = query.Where(d => d.City == request.City);


//            if (!string.IsNullOrWhiteSpace(request.Gender))
//                query = query.Where(d => d.User.Gender == request.Gender);

//            if (request.MinPrice.HasValue)
//                query = query.Where(d => d.Price >= request.MinPrice);

//            if (request.MaxPrice.HasValue)
//                query = query.Where(d => d.Price <= request.MaxPrice);

//            // 🔥 RATING (SQL SUBQUERY - IMPORTANT FIX)
//            var queryWithRating = query.Select(d => new
//            {
//                Doctor = d,
//                Rating =
//                    d.Feedbacks.Any()
//                        ? d.Feedbacks.Average(f => f.Rating)
//                        : 0
//            });

//            if (request.MinRating.HasValue)
//            {
//                queryWithRating = queryWithRating
//                    .Where(x => x.Rating >= request.MinRating.Value);
//            }

//            // 🔥 SORTING (STILL SQL)
//            bool isAsc = request.SortDirection?.ToLower() == "asc";

//            queryWithRating = request.SortBy?.ToLower() switch
//            {
//                "price" => isAsc
//                    ? queryWithRating.OrderBy(x => x.Doctor.)
//                    : queryWithRating.OrderByDescending(x => x.Doctor.Price),

//                _ => isAsc
//                    ? queryWithRating.OrderBy(x => x.Rating)
//                    : queryWithRating.OrderByDescending(x => x.Rating)
//            };

//            // 🔥 COUNT (BEFORE PAGINATION)
//            var totalCount = await queryWithRating.CountAsync(cancellationToken);

//            // 🔥 PAGINATION (SQL LEVEL)
//            var items = await queryWithRating
//                .Skip((request.PageNumber - 1) * request.PageSize)
//                .Take(request.PageSize)
//                .Select(x => new DoctorDto
//                {
//                    Id = x.Doctor.DoctorID,
//                    Name = x.Doctor.Name,
//                    Specialization = x.Doctor.Specialization,
//                    Price = x.Doctor.Price,
//                    City = x.Doctor.City,
//                    Gender = x.Doctor.Gender,
//                    YearsOfExperience = x.Doctor.YearsOfService,
//                    Rating = x.Rating
//                })
//                .ToListAsync(cancellationToken);

//            return Result.Success(new PagedResult<DoctorDto>
//            {
//                PageNumber = request.PageNumber,
//                PageSize = request.PageSize,
//                TotalCount = totalCount,
//                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize),
//                Items = items
//            });
//        }
//    }
//}
