﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WCA.Consumer.Api.Extensions
{
    public static class AppBuilderExtensions
    {
        
        public static void RegisterGlobalExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory, bool isProd)
        {
            // Global exception handler
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionHandlerFeature != null)
                    {
                        var logger = loggerFactory.CreateLogger("Global exception logger");
                        logger.LogError(
                            500,
                            exceptionHandlerFeature.Error,
                            exceptionHandlerFeature.Error.Message);
                    }

                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    var responseMessage = //isProd ? new
                    //{
                    //    error = "An unexpected error happened. Try again later",
                    //    errorMessage = "An unexpected error happened. Try again later"
                    //} : 
                    new
                    {
                        error = exceptionHandlerFeature.Error.ToString(),
                        errorMessage = exceptionHandlerFeature.Error.Message
                    };
                    await context.Response.WriteAsJsonAsync(responseMessage);
                });
            });
        }
    }
}
