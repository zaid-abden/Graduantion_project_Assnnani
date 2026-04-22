//using GraduationProject.Application.Common.Results;
//using GraduationProject.Application.Contracts.Repositories;
//using GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion.Enums;
//using GraduationProject.Data.Identity;
//using MediatR;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion
//{

//    internal class DoctorFilterationQueryHandler : IRequestHandler<DoctorFilterationQuery, Result<PagedResult<DoctorFDTO>>>
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly UserManager<User> _userManager;

//        public DoctorFilterationQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
//        {
//            _unitOfWork = unitOfWork;
//            _userManager = userManager;
//        }
//        public async Task<Result<PagedResult<DoctorFDTO>>> Handle(DoctorFilterationQuery request, CancellationToken cancellationToken)
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
//                query = query.Where(d => d.price >= request.MinPrice);

//            if (request.MaxPrice.HasValue)
//                query = query.Where(d => d.price <= request.MaxPrice);

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


//            queryWithRating = request.SortBy switch
//            {
//                DoctorSortBy.Price => request.SortDirection == SortDirection.Asc
//                    ? queryWithRating.OrderBy(x => x.Doctor.price)
//                    : queryWithRating.OrderByDescending(x => x.Doctor.price),

//                _ => request.SortDirection == SortDirection.Asc
//                    ? queryWithRating.OrderBy(x => x.Rating)
//                    : queryWithRating.OrderByDescending(x => x.Rating)
//            };

//            // 🔥 COUNT (BEFORE PAGINATION)
//            var totalCount = await queryWithRating.CountAsync(cancellationToken);

//            // 🔥 PAGINATION (SQL LEVEL)
//            var items = await queryWithRating
//                .Skip((request.PageNumber - 1) * request.PageSize)
//                .Take(request.PageSize)
//                .Select(x => new DoctorFDTO
//                {
//                    Id = x.Doctor.DoctorId,
//                    Name = x.Doctor.User.FullName,
//                    Price = x.Doctor.price,
//                    City = x.Doctor.City,
//                    Gender = x.Doctor.User.Gender,
//                    YearsOfExperience = x.Doctor.YearsOfExperience,
//                    Rating = x.Rating
//                })
//                .ToListAsync(cancellationToken);

//            return Result<PagedResult<DoctorFDTO>>.Success(new PagedResult<DoctorFDTO>
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