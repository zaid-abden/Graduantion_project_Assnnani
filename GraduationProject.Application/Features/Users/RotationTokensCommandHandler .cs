using GraduationProject.Application.Common.Results;
using GraduationProject.Application.Contracts.Repositories;
using GraduationProject.Application.Tokens.Authentication;
using GraduationProject.Application.Tokens.DTOs;
using GraduationProject.Data.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Features.Users
{
    public class RotationTokensCommandHandler : Result<RotationTokensCommandHandler>,
      IRequestHandler<RotationTokensCommand, Result<NewTokens>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAthentication _athentication;
        private readonly UserManager<User> _usermanager;

        public RotationTokensCommandHandler(IUnitOfWork unitOfWork
            , IAthentication athentication
            , UserManager<User> usermanager)
        {
            _unitOfWork = unitOfWork;
            _athentication = athentication;
            _usermanager = usermanager;
        }
        async Task<Result<NewTokens>> IRequestHandler<RotationTokensCommand, Result<NewTokens>>.Handle(RotationTokensCommand request, CancellationToken cancellationToken)
        {
            var user = await _usermanager.FindByIdAsync(request.UserId);
            if (user == null)
                return Result<NewTokens>.NotFound("UserNotFound");
            try
            {
                var GenerateToken = await _athentication.RotationRefandJWT(user, request.accussToken, request.RefreshToken);
                if (GenerateToken == null) return Result<NewTokens>.NotFound("Not Found");
                if (GenerateToken.Result is null)
                {
                    return Result<NewTokens>.BadRequest($"{GenerateToken.Error}");
                }
                return Result<NewTokens>.Success(GenerateToken);
            }
            catch (ArgumentNullException ex)
            {
                return Result<NewTokens>.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return Result<NewTokens>.NotFound(ex.Message);

            }


        }
    }
}
