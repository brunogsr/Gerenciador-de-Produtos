using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(ms => ms.Value.Errors.Any())
                .SelectMany(ms => ms.Value.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            context.Result = new BadRequestObjectResult(errors);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
