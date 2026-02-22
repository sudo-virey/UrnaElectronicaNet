using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UrnaElectronica.Filters
{
    public class HtmxLayoutFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ViewResult viewResult)
            {
                // Verificar si es una solicitud HTMX
                bool isHtmxRequest = context.HttpContext.Request.Headers.ContainsKey("HX-Request");

                if (isHtmxRequest)
                {
                    // Para solicitudes HTMX, usar layout parcial (sin sidebar/header)
                    viewResult.ViewName = viewResult.ViewName ?? GetDefaultViewName(context);
                    viewResult.ViewData["Layout"] = "~/Views/Shared/_PartialLayout.cshtml";
                }
                else
                {
                    // Para solicitudes normales, usar layout completo
                    viewResult.ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
                }
            }

            base.OnActionExecuted(context);
        }

        private string GetDefaultViewName(ActionExecutedContext context)
        {
            var actionName = context.RouteData.Values["action"]?.ToString();
            return actionName ?? "Index";
        }
    }
}
