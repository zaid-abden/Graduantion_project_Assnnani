using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Tokens.DTOs;
using MediatR;

namespace GraduationProject.Application.Features.Users
{
    public class RotationTokensCommand : IRequest<Result<NewTokens>>
    {
        public string UserId { get; set; }
        public string accussToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
