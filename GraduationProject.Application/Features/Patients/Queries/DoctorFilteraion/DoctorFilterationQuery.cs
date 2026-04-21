using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion
{
    internal class DoctorFilterationQuery : IRequest<Result<PagedResult<DoctorFDTO>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public string? City { get; set; }
        public string? Specialization { get; set; }
        public string? Gender { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }

        public string? SortBy { get; set; } = "rating"; // rating | price
        public string? SortDirection { get; set; } = "desc"; // asc | desc
    }
}
