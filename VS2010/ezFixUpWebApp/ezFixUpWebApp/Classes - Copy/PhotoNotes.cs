using System;
using System.Linq;
using ezFixUp.Model;

namespace ezFixUp.Data
{
    public class PhotoNote
    {
        public int Id { get; set; }
        public int PhotoId { get; set; }
        public string Username { get; set; }
        public string Notes { get; set; }
        public DateTime Timestamp { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        /// <summary>
        /// Loads the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="photoId">The photo id.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static PhotoNote[] Load(int? id, int? photoId, string username)
        {
            using (var db = new ezFixUpDataContext())
            {
                var photoNotes = from pn in db.PhotoNotes
                                 where (!id.HasValue || pn.pn_id == id)
                                       && (!photoId.HasValue || pn.p_id == photoId)
                                       && (username == null || pn.u_username == username)
                                 select new PhotoNote
                                            {
                                                Id = pn.pn_id,
                                                PhotoId = pn.p_id,
                                                Username = pn.u_username,
                                                Notes = pn.pn_notes,
                                                Timestamp = pn.pn_timestamp,
                                                X = pn.pn_x,
                                                Y = pn.pn_y,
                                                Width = pn.pn_width,
                                                Height = pn.pn_height
                                            };
                return photoNotes.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (var db = new ezFixUpDataContext())
            {
                var photoNote = new Model.PhotoNote
                                    {
                                        pn_id = Id,
                                        p_id = PhotoId,
                                        u_username = Username,
                                        pn_notes = Notes,
                                        pn_timestamp = Timestamp,
                                        pn_x = X,
                                        pn_y = Y,
                                        pn_width = Width,
                                        pn_height = Height
                                    };
                if (Id == 0)
                    db.PhotoNotes.InsertOnSubmit(photoNote);
                else
                {
                    db.PhotoNotes.Attach(photoNote, true);
                }

                db.SubmitChanges();

                if (Id == 0)
                    Id = photoNote.pn_id;
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            using (var db = new ezFixUpDataContext())
            {
                var photoNote = db.PhotoNotes.Single(n => n.pn_id == Id);
                db.PhotoNotes.DeleteOnSubmit(photoNote);
                db.SubmitChanges();
            }
        }

        /// <summary>
        /// Deletes the specified notes.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="photoId">The photo id.</param>
        /// <param name="username">The username.</param>
        public static void Delete(int? id, int? photoId, string username)
        {
            foreach (var photoNote in Load(id, photoId, username))
            {
                photoNote.Delete();
            }
        }
    }
}