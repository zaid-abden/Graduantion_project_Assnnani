using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion.Enums;
using MediatR;

namespace GraduationProject.Application.Features.Patients.Queries.DoctorFilteraion
{
    public class DoctorFilterationQuery : IRequest<Result<PagedResult<DoctorFDTO>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? Search { get; set; }
        public string? City { get; set; }
        public string? Gender { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }

        public DoctorSortBy SortBy { get; set; } = DoctorSortBy.Rating;

        public SortDirection SortDirection { get; set; } = SortDirection.Desc;
    }
}
