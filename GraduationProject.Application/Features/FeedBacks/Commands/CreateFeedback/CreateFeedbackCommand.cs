using GraduationProject.Application.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject.Application.Features.FeedBacks.Commands.CreateFeedback
{
    public class CreateFeedbackCommand : IRequest<Result<int>>
    {
        public int DoctorId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
