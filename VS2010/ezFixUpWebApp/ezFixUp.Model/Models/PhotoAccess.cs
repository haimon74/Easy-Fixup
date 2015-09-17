
namespace ezFixUp.Model.Models
{
    public class PhotoAccess
    {
        public string pa_photoowner { get; set; }
        public string pa_photoviewer { get; set; }
        public System.DateTime pa_dateaccessgranted { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
