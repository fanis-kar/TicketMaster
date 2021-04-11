using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TicketMaster.Api.Filter
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (context.ActionDescriptor.DisplayName.Contains("Post"))
            {
                if (context.ActionArguments.Count(p => p.Value == null) == 1)
                {
                    context.Result = new BadRequestObjectResult("Post needs a body");
                    return;
                }
            }
            //if it's a GET and there is an id it should not be 0 or negative (< 1)
            else if (context.ActionDescriptor.DisplayName.Contains("Get"))
            {
                if (context.ActionArguments.Count(p => p.Key.ToLowerInvariant().Contains("id") && ((long)p.Value) < 1) > 0)
                {
                    context.Result = new BadRequestObjectResult("Zero or negative identifiers are not allowed");
                    return;
                }
            }
            //if it is a DELETE then it should have an id > 0
            else if (context.ActionDescriptor.DisplayName.Contains("Delete"))
            {
                if (context.ActionArguments.Count() ==  0)
                {
                    context.Result = new BadRequestObjectResult("Cannot delete without id");
                    return;
                }

                if (context.ActionArguments.Count(p => p.Key.ToLowerInvariant().Contains("id") && ((long)p.Value) < 1) > 0)
                {
                    context.Result = new BadRequestObjectResult("Zero or negative identifiers are not allowed");
                    return;
                }
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }            
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {            
        }
    }
}
