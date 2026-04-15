using GraduationProject.Api.Common.Responses;
using GraduationProject.Application.Contracts.Identity;
using GraduationProject.Application.Features.Doctors.Commands.CreateDoctor;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GraduationProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserService currentUserService;

        public DoctorsController(IMediator mediator,ICurrentUserService currentUserService)
        {
            this.mediator = mediator;
            this.currentUserService = currentUserService;
        }
       
    }
}
