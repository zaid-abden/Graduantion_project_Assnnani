using GraduationProject.Application.Common.Results;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject.Api.Common.Responses
{
    public static class ResultToApiMapper
    {
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            return result.Status switch
            {
                ResultStatus.Success =>
                    new OkObjectResult(new ApiResponse<T>
                    {
                        Succeeded = true,
                        Data = result.Value,
                        Message = "Success"
                    }),
                ResultStatus.Failure =>   
       new BadRequestObjectResult(new ApiResponse<T>
       {
           Succeeded = false,
           Message = result.Error
       }),
                ResultStatus.ValidationError =>
                    new BadRequestObjectResult(new ApiResponse<T>
                    {
                        Succeeded = false,
                        Message = result.Error
                    }),

                ResultStatus.NotFound =>
                    new NotFoundObjectResult(new ApiResponse<T>
                    {
                        Succeeded = false,
                        Message = result.Error
                    }),

                ResultStatus.Conflict =>
                    new ConflictObjectResult(new ApiResponse<T>
                    {
                        Succeeded = false,
                        Message = result.Error
                    }),

                _ =>
                    new ObjectResult(new ApiResponse<T>
                    {
                        Succeeded = false,
                        Message = result.Error
                    })
                    { StatusCode = 500 }
            };
        }
    }

}
