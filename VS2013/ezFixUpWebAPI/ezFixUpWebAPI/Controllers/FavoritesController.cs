using System.Collections.Generic;
using System.Linq;
using ezFixUp.Model;
using IezFixUpUow = ezFixUp.Data.Contracts.IezFixUpUow;

//using ezFixUp.Model;

namespace ezFixUpWebAPI.Controllers
{
    public class FavoritesController : ApiControllerBase
    {
        public FavoritesController(IezFixUpUow uow)
        {
            Uow = uow;
        }

        #region OData Future: IQueryable<T>
        //[Queryable]
        // public IQueryable<Attendance> Get()
        #endregion

        // GET: api/favorites/{personId}
        public IEnumerable<Attendance> GetByPersonId(int id)
        {
            return Uow.Attendance.GetByPersonId(id)
                .OrderBy(ps => ps.Session.Title);
        }
    }
}