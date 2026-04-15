using GraduationProject.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace GraduationProject.Application.Logics.pationtLogics
{
    public class CreatePationtLogic
    {
        private readonly UserManager<User> _userManager;

        public CreatePationtLogic(UserManager<User> userManager)
        {
            _userManager = userManager;
        }



    }
}
