using Microsoft.AspNetCore.Diagnostics;
using ProductAPI.Models.Dto;
using System.Net;

namespace ProductAPI.Extensions
{
	public static class ExceptionMiddlewareExtensions
	{
		public static void ConfigureExceptionHandler(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(appError =>
			{
				appError.Run(async context =>
				{
					context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
					context.Response.ContentType = "application/json";
					var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						await context.Response.WriteAsync(new ResponceDto()
						{
							IsSucces = false,
							ErrorMessages = new List<string>() { contextFeature.Error.Message.ToString() },
						}.ToString()); 
					}
				});
			});
		}
	}
}
