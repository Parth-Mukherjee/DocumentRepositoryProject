using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using DocumentRepository.Data;
using DocumentRepository.Models;

namespace DocumentRepository.CustomFilters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<CustomExceptionFilter> _logger;
        private readonly ApplicationDbContext _AppDbcontext;
        //private readonly  _AppDbcontext;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger , ApplicationDbContext context)
        {
            _logger = logger;
            _AppDbcontext = context;
        }

        public void OnException(ExceptionContext exceptionContext)
        {
            _logger.LogError(exceptionContext.Exception, "An unhandled exception occurred.");

            var LogID = SaveErrorDetailsToDatabase(exceptionContext);
            exceptionContext.Result = new RedirectToActionResult("Error", "Home", new { LogID});
            exceptionContext.ExceptionHandled = true;
        }

        private Guid SaveErrorDetailsToDatabase(ExceptionContext exceptionContext)
        {
            ErrorModel errorLogDetail = new ErrorModel();
            errorLogDetail.errorMessage = exceptionContext.Exception.Message;
            errorLogDetail.errorStackTrace= exceptionContext.Exception.StackTrace;
            errorLogDetail.errorLogOn = DateTime.Now;
            errorLogDetail.errorRoute = exceptionContext.RouteData.Values["Controller"].ToString() + "/" + exceptionContext.RouteData.Values["Action"].ToString();
            _AppDbcontext.errorLogDetails.Add(errorLogDetail);
            _AppDbcontext.SaveChanges();

            return errorLogDetail.logID;
        }
    }
}
