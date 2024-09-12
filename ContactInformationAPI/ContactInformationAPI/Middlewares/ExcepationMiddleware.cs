using ContactInformationAPI.Controllers;
using ContactInformationAPI.Modal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContactInformationAPI.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExcepationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExcepationMiddleware> logger;
        public ExcepationMiddleware(RequestDelegate next, ILogger<ExcepationMiddleware> _logger)
        {
            _next = next;
            this.logger = _logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception er)
            {
                httpContext.Response.ContentType = "application/problem+json";
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                this.logger.LogError(httpContext.Request.Path, er);
                var _response = new APIResponse();
                _response.Status = false;
                _response.Message= "Internal Server error";
                var result =JsonConvert.SerializeObject(_response);
                await httpContext.Response.WriteAsync(result);
            }

        }
    }
}
