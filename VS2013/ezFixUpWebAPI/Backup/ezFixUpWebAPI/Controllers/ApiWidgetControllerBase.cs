using System.Web.Http;
using ezFixUp.Data.Contracts;


namespace ezFixUpWebAPI.Controllers
{
    public abstract class ApiWidgetControllerBase : ApiControllerBase
    {
        public string Get()
        {
            return "Hello World";
        }
    }

}
