using System.Linq;
using ezFixUp.Model;

namespace ezFixUp.Data.Contracts
{
    public interface IAttendanceRepository : IRepository<Attendance>
    {
        IQueryable<Attendance> GetByPersonId(int id);
        IQueryable<Attendance> GetBySessionId(int id);
        Attendance GetByIds(int personId, int sessionId);
        void Delete(int personId, int sessionId);
    }
}
