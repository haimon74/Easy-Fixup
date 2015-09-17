using System.Data.Entity;
using System.Linq;
using ezFixUp.Model;
using IPersonsRepository = ezFixUp.Data.Contracts.IPersonsRepository;

//using ezFixUp.Model;

namespace ezFixUp.Data
{
    public class PersonsRepository : EFRepository<Person>, IPersonsRepository
    {
        public PersonsRepository(DbContext context) : base(context) { }

        /// <summary>
        /// Get <see cref="Speaker"/>s at sessions.
        /// </summary>
        ///<remarks>
        ///See <see cref="IPersonsRepository.GetSpeakers"/> for details.
        ///</remarks>

        public IQueryable<Speaker> GetSpeakers()
        {
            return DbContext
                .Set<Session>()
                .Select(session => session.Speaker)
                .Distinct().Select(s =>
                    new Speaker
                        {    
                                Id = s.Id,
                                FirstName = s.FirstName,
                                LastName = s.LastName,
                                ImageSource = s.ImageSource,
                        });
       
        }
    }
}
