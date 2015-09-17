using System.Linq;
using System.Web;
using ezFixUp.Classes;
using ezFixUp.Behaviors;

namespace ezFixUp.Handlers
{
    /// <summary>
    /// Summary description for AddRemoveConnection
    /// </summary>
    public class AddRemoveConnection : ezFixUpBaseHandler
    {
        private string[] types = { "friend", "favorite", "block" };
        private string[] commands = { "add", "remove" };

        public override void ProcessRequest(HttpContext context)
        {
            ISocialConnector socialConnector = null;
            var type = (context.Request.Params["type"] ?? "").Trim().ToLower() ;
            var cmd = (context.Request.Params["cmd"] ?? "").Trim().ToLower();
            var username = (context.Request.Params["id"] ?? "").Trim().ToLower();

            if (!ValidateParams(type,cmd,username))
                return;

            switch (type)
            {
                case "friend":
                    socialConnector = new Behaviors.AddRemoveFriend();
                    break;
                case "favorite":
                    socialConnector = new Behaviors.AddRemoveFavorite();
                    break;
                case "block":
                    socialConnector = new Behaviors.AddRemoveFriend();
                    break;
            }
            var userSession = PageBase.GetCurrentUserSession();

            string actionResult = "";
            
            if (cmd == "add")
                actionResult = socialConnector.Add(userSession, username);
            else
                actionResult = socialConnector.Remove(userSession, username);

            context.Response.ContentType = "text/plain";
            context.Response.Write(actionResult);
        }

        private bool ValidateParams(string type, string cmd, string username)
        {
            if (type.IsNullOrEmpty() || cmd.IsNullOrEmpty() || username.IsNullOrEmpty())
                return false;

            if (!(types.Contains(type) && commands.Contains(cmd)))
                return false;

            var user = User.Load(username);
            if (user == null)
                return false;

            return true;
        }
        
    }
}