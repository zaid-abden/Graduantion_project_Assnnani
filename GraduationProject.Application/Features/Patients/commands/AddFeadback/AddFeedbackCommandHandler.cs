using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Data.Models;
using MediatR;

namespace GraduationProject.Application.Features.Patients.commands.AddFeadback
{
    internal class AddFeedbackCommandHandler : IRequestHandler<AddFeedbackCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddFeedbackCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<int>> Handle(
            AddFeedbackCommand request,
            CancellationToken cancellationToken)
        {
            var feedback = new Feedback
            {
                DoctorId = request.DoctorId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow,
                PatientId = 1
            };

            await _unitOfWork.Feedbacks.AddAsync(feedback);

            return Result<int>.Success(feedback.FeedbackId);
        }
    }
}
