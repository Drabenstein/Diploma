using Core.Amazon;
using GlobalExceptionHandler.WebApi;
using Newtonsoft.Json;

namespace WebApi.Helpers
{
    public static class WebApplicationExtensions
    {
        public static void UseGlobalExceptionHandling(this WebApplication app)
        {
            app.UseGlobalExceptionHandler(x =>
            {
                x.ContentType = "application/json";

                x.ResponseBody(x => JsonConvert.SerializeObject(new
                {
                    Message = $"An error occured while handling your request: {x.Message}"
                }));

                x.Map<AmazonException>().ToStatusCode(StatusCodes.Status400BadRequest);
            });
        }
    }
}
