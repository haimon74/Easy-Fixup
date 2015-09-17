using System.Collections.Generic;
using System.Linq;
using ezFixUp.Model;
using IezFixUpUow = ezFixUp.Data.Contracts.IezFixUpUow;

namespace ezFixUp.Web.Controllers
{
    public class SessionBriefsController : ApiControllerBase
    {
        public SessionBriefsController(IezFixUpUow uow)
        {
            Uow = uow;
        }

        #region OData Future: IQueryable<T>
        //[Queryable]
        // public IQueryable<SessionBrief> Get()
        #endregion

        // GET /api/sessionbriefs
        public IEnumerable<SessionBrief> Get()
        {
            return Uow.Sessions.GetSessionBriefs()
                .OrderBy(sb => sb.TimeSlotId);
        }
    }
}