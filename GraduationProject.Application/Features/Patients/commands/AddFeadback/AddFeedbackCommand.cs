using GraduationProject.Application.Common.Results;
using MediatR;

namespace GraduationProject.Application.Features.Patients.commands.AddFeadback
{
    public class AddFeedbackCommand : IRequest<Result<int>>
    {
        public int DoctorId { get; set; }
        public int Rating { get; set; } // 1 - 5
        public string Comment { get; set; }
    }
}
