using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ezFixUp.Model;

namespace ezFixUp.Data
{
    public class Poll
    {
        public int ID
        {
            get; 
            private set;
        }

        public string Title
        {
            get; set;
        }

        public DateTime StartDate
        {
            get; set;
        }

        public DateTime EndDate
        {
            get;
            set;
        }

        public DateTime ShowResultsUntil
        {
            get;
            set;
        }
        
        private Poll()
        {
            
        }

        public Poll(string title)
        {
            Title = title;
        }

        //public static int FetchPollsCount()
        //{
        //    using (var db = new ezFixUpDataContext())
        //    {
        //        return (from p in db.Polls
        //                select p).Count();
        //    }
        //}

        public static Poll FetchRandom(bool? answered, string username)
        {
            using (var db = new ezFixUpDataContext())
            {
                var ids = (from p in db.Polls
                           where answered == null || 
                           (answered == true && DateTime.Now.Date <= p.p_showresultsuntil.Date &&
                           ((from pa in db.PollAnswers where pa.u_username == username select pa.p_id).Contains(p.p_id) ||
                            DateTime.Now.Date > p.p_enddate)
                           ) ||
                           (answered == false && DateTime.Now.Date <= p.p_enddate &&
                           !(from pa in db.PollAnswers where pa.u_username == username select pa.p_id).Contains(p.p_id))
                           select p.p_id).ToArray();

                if (ids.Length > 0)
                {
                    var randomID = ids[new Random().Next(ids.Length)];

                    return (from p in db.Polls
                            where p.p_id == randomID
                            select new Poll
                                       {
                                           ID = p.p_id,
                                           Title = p.p_title,
                                           StartDate = p.p_startdate,
                                           EndDate = p.p_enddate,
                                           ShowResultsUntil = p.p_showresultsuntil
                                       }).Single();
                }

                return null;
            }
        }

        public static bool IsAnswered(int pollID, string username)
        {
            using (var db = new ezFixUpDataContext())
            {
                return (from pa in db.PollAnswers
                        where pa.p_id == pollID && pa.u_username == username
                        select pa).Count() > 0;
            }
        }

        public static void AddAnswer(int pollID, string username, int choiceID)
        {
            using (var db = new ezFixUpDataContext())
            {
                var pollAnswer = new Model.PollAnswer
                {
                    p_id = pollID,
                    pc_id = choiceID,
                    u_username = username
                };

                db.PollAnswers.InsertOnSubmit(pollAnswer);

                db.SubmitChanges();
            }                
        }

        /// <summary>
        /// Fetches number of votes for each answer
        /// </summary>
        /// <param name="pollID">the poll id</param>
        /// <returns>The key is the poll choice ID and the value is the number of votes</returns>
        public static Dictionary<int, int> FetchResults(int pollID)
        {
            var votesForEachChoice = new Dictionary<int, int>();

            using (var db = new ezFixUpDataContext())
            {
                var choiceIDs = (from c in db.PollChoices
                                 where c.p_id == pollID
                                 select c.pc_id).ToArray();

                foreach (var id in choiceIDs)
                {
                    votesForEachChoice.Add(id, 0);                    
                }

                var answers = (from pa in db.PollAnswers
                               where pa.p_id == pollID
                               select pa).ToArray();

                foreach (var answer in answers)
                {
                    if (votesForEachChoice.ContainsKey(answer.pc_id))
                    {
                        votesForEachChoice[answer.pc_id]++;
                    }
                }
            }

            return votesForEachChoice;
        }

        public static Poll[] Fetch()
        {
            using (var db = new ezFixUpDataContext())
            {
                return (from p in db.Polls
                        select new Poll
                        {
                            ID = p.p_id,
                            Title = p.p_title,
                            StartDate = p.p_startdate,
                            EndDate = p.p_enddate,
                            ShowResultsUntil = p.p_showresultsuntil
                        }).ToArray();
            }
        }

        public static Poll Fetch(int id)
        {
            using (var db = new ezFixUpDataContext())
            {
                return (from p in db.Polls
                        where p.p_id == id
                        select new Poll
                                   {
                                       ID = p.p_id,
                                       Title = p.p_title,
                                       StartDate = p.p_startdate,
                                       EndDate = p.p_enddate,
                                       ShowResultsUntil = p.p_showresultsuntil
                                   }).FirstOrDefault();
            }            
        }
        
        public void Save()
        {
            using (var db = new ezFixUpDataContext())
            {
                var poll = new Model.Poll
                                    {
                                        p_id = ID,
                                        p_title = Title,
                                        p_startdate = StartDate,
                                        p_enddate = EndDate,
                                        p_showresultsuntil = ShowResultsUntil
                                    };
                if (ID == 0)
                    db.Polls.InsertOnSubmit(poll);
                else
                {
                    db.Polls.Attach(poll, true);
                }

                db.SubmitChanges();

                if (ID == 0)
                    ID = poll.p_id;
            }            
        }

        public static void Delete(int id)
        {
            using (var db = new ezFixUpDataContext())
            {
                var pollAnswers = db.PollAnswers.Where(pa => pa.p_id == id);
                var pollChoices = db.PollChoices.Where(pc => pc.p_id == id);
                var poll = db.Polls.FirstOrDefault(p => p.p_id == id);
                if (poll != null)
                {
                    db.PollAnswers.DeleteAllOnSubmit(pollAnswers);
                    db.PollChoices.DeleteAllOnSubmit(pollChoices);
                    db.Polls.DeleteOnSubmit(poll);
                    db.SubmitChanges();
                }
            }
        }

    }
}
