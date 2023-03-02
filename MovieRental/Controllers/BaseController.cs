using Microsoft.AspNetCore.Mvc;

namespace MovieRental.Controllers
{
    public abstract class BaseController : Controller
    {
        protected void AddModelErrors(OperationResponse response)
        {
            foreach (var key in response.Errors.Keys)
            {
                ModelState.AddModelError(key, response.Errors[key]);
            }
        }
    }
}
