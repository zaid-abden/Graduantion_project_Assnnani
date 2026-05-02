using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Features.Admin.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Admin.Queries.GetStudents
{
	public record GetStudentsQuery : IRequest<Result<List<StudentListDto>>>;
}
