using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetDating.Model;

namespace AspNetDating.Classes
{
    public class PollChoice
    {
        public int ID
        {
            get;
            private set;
        }

        public int PollID
        {
            get;
            set;
        }

        public string Answer
        {
            get;
            set;
        }

        private PollChoice()
        {
            
        }

        public PollChoice(int pollID, string answer)
        {
            this.PollID = pollID;
            this.Answer = answer;
        }

        public static PollChoice[] FetchByPollID(int pollID)
        {
            using (var db = new AspNetDatingDataContext())
            {
                return (from pc in db.PollChoices
                        where pc.p_id == pollID
                        select new PollChoice
                        {
                            ID = pc.pc_id,
                            PollID = pc.p_id,
                            Answer = pc.pc_answer
                        }).ToArray();
            }
        }

        public static PollChoice Fetch(int choiceID)
        {
            using (var db = new AspNetDatingDataContext())
            {
                return (from pc in db.PollChoices
                        where pc.pc_id == choiceID
                        select new PollChoice
                        {
                            ID = pc.pc_id,
                            PollID = pc.p_id,
                            Answer = pc.pc_answer
                        }).FirstOrDefault();
            }
        }

        public void Save()
        {
            using (var db = new AspNetDatingDataContext())
            {
                var pollChoice = new Model.PollChoice
                {
                    p_id = PollID,
                    pc_id = ID,
                    pc_answer = Answer
                };
                if (ID == 0)
                    db.PollChoices.InsertOnSubmit(pollChoice);
                else
                {
                    db.PollChoices.Attach(pollChoice, true);
                }

                db.SubmitChanges();

                if (ID == 0)
                    ID = pollChoice.pc_id;
            }
        }

        public static void Delete(int id)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var pollChoice = db.PollChoices.FirstOrDefault(pc => pc.pc_id == id);
                if (pollChoice != null)
                {
                    db.PollChoices.DeleteOnSubmit(pollChoice);
                    db.SubmitChanges();
                }
            }
        }
    }
}
