using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BulkyBookWeb.Middleware
{
    public class AdminRoleMiddleware : IMiddleware
    {
        public AdminRoleMiddleware()
        {
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Path.StartsWithSegments("/Admin") && context.Session.GetString("UserRole") != "Admin")
            {
                    var tempData = context.RequestServices.GetRequiredService<ITempDataProvider>().LoadTempData(context);
                    tempData["error"] = "You do not have access to the Admin section.";
                    context.RequestServices.GetRequiredService<ITempDataProvider>().SaveTempData(context, tempData);

                    context.Response.Redirect("/");
                    return;
            }
            // Request is not for the Admin controller or user is an admin, continue to the next middleware
            await next(context);
        }
    }

}
