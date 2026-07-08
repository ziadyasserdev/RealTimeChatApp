using Microsoft.AspNetCore.Mvc;
using RealTimeChatApp.Application.Commons.Results;

namespace RealTimeChatApp.Api.Commons.Responses
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
                        Message = string.IsNullOrEmpty(result.Message) ? "Success" : result.Message
                    }),

                ResultStatus.RequiresTwoFactor =>
                    new OkObjectResult(new ApiResponse<T>
                    {
                        Succeeded = false,
                        Message = result.Error
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

                ResultStatus.Unauthorized =>
                    new UnauthorizedObjectResult(new ApiResponse<T>
                    {
                        Succeeded = false,
                        Message = result.Error
                    }),

                ResultStatus.Forbidden =>
                    new ObjectResult(new ApiResponse<T>
                    {
                        Succeeded = false,
                        Message = result.Error
                    })
                    { StatusCode = 403 },

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
