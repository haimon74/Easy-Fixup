/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    [Serializable]
    public class GroupMember
    {
        #region fields

        /// <summary>
        /// Specifies the type of member for the group.
        /// </summary>
        public enum eType
        {
            /// <summary>
            /// The member is admin.
            /// </summary>
            Admin = 1,

            /// <summary>
            /// The member is moderator.
            /// </summary>
            Moderator = 2,
            
            /// <summary>
            /// The member is user.
            /// </summary>
            Member = 3,

            /// <summary>
            /// The member is V.I.P.
            /// </summary>
            VIP = 4
        }

        public enum eSortColumn
        {
            None,
            JoinDate
        }

        private int? groupID;
        private string username;
        private eType type = eType.Member;
        private bool active = false;
        private DateTime joinDate = DateTime.Now;
        private string invitedBy = null;
        private string joinAnswer = String.Empty;
        private bool isWarned = false;
        private string warnReason = null;
        private DateTime? warnExpirationDate = null;

        #endregion

        #region Constructors

        private GroupMember()
        {}

        public GroupMember(int groupID, string username)
        {
            this.groupID = groupID;
            this.username = username;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the group ID.
        /// The property is read-only.
        /// Throws Exception exception.
        /// </summary>
        /// <value>The group ID.</value>
        public int GroupID
        {
            get
            {
                if (groupID.HasValue)
                {
                    return groupID.Value;    
                }
                else
                {
                    throw new Exception("The field groupID is not set.");
                }
            }
        }

        /// <summary>
        /// Gets the username.
        /// The property is read-only.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return username; }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public eType Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GroupMember"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets or sets the join date.
        /// </summary>
        /// <value>The join date.</value>
        public DateTime JoinDate
        {
            get { return joinDate; }
            set { joinDate = value; }
        }

        /// <summary>
        /// Gets or sets the invited by.
        /// </summary>
        /// <value>The invited by.</value>
        public string InvitedBy
        {
            get { return invitedBy; }
            set { invitedBy = value; }
        }

        /// <summary>
        /// Gets or sets the answer for the join question (if any).
        /// </summary>
        /// <value>The answer.</value>
        public string JoinAnswer
        {
            get { return joinAnswer; }
            set { joinAnswer = value; }
        }

        public bool IsWarned
        {
            get { return isWarned; }
            set { isWarned = value; }
        }

        public string WarnReason
        {
            get { return warnReason; }
            set { warnReason = value; }
        }

        public DateTime? WarnExpirationDate
        {
            get { return warnExpirationDate; }
            set { warnExpirationDate = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group members from DB.
        /// If there are no group members in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupMember[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches all group members by specified username.
        /// If there are no group members by specified username it returns an empty array. 
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>Group member array or an empty array if no group members are found in DB.</returns>
        public static GroupMember[] Fetch(string username)
        {
            return Fetch(null, username, null, null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches all group members by specified group id.
        /// If there are no group members by specified group id it returns an empty array. 
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns>Group member array or an empty array if no group members are found in DB.</returns>
        public static GroupMember[] Fetch(int groupID)
        {
            return Fetch(groupID, null, null, null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches all group members from DB by specified group ID and type.
        /// If there are no group members by specified parameters it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="type">The type.</param>
        /// <returns>Group members array or an empty array if no group members are found in DB.</returns>
        public static GroupMember[] Fetch(int groupID, eType type)
        {
            return Fetch(groupID, null, type, null, null, null, null, eSortColumn.None);
        }

        public static GroupMember[] Fetch(int groupID, eType type, eSortColumn sortColumn)
        {
            return Fetch(groupID, null, type, null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches group members for the specified group ID.
        /// It returns all active or inactive group members based on the specified parameter.
        /// If there are no group members by specified parameters it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="active">if set to <c>true</c> this method returns all active group members otherwise
        /// it returns all inactive group members.
        /// </param>
        /// <returns></returns>
        public static GroupMember[] Fetch(int groupID, bool active)
        {
            return Fetch(groupID, null, null, active, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches active or inactive specified number of members from DB for the specified group and order
        /// them by specified sort column.
        /// If there are no group members in DB by specified arguments it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <param name="numberOfMembers">The number of members.</param>
        /// <returns></returns>
        public static GroupMember[] Fetch(int groupID, bool active, int numberOfMembers, eSortColumn sortColumn)
        {
            return Fetch(groupID, null, null, active, null, null, numberOfMembers, sortColumn);
        }

        /// <summary>
        /// Fetches group member by specified group ID and username.
        /// If the member doesn't exist it returns NULL.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <returns>Group member or null if there are no member in DB by specified arguments.</returns>
        public static GroupMember Fetch(int groupID, string username)
        {
            GroupMember[] groupMembers = Fetch(groupID, username, null, null, null, null, null, eSortColumn.None);

            if (groupMembers.Length > 0)
            {
                return groupMembers[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches group members from DB by specified group ID, username, type or active status.
        /// If all arguments are null it returns all group members from DB.
        /// If it cannot find a record in DB by specified arguments it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="type">The type.</param>
        /// <param name="active">The active.</param>
        /// <param name="joinDate">The join date.</param>
        /// <param name="invitedBy">The invited by.</param>
        /// <param name="numberOfMembers">The number of members.</param>
        /// <returns>Group members array or an empty array if no group members are found in DB.</returns>
        private static GroupMember[] Fetch(int? groupID, string username, eType? type, bool? active,
                                                DateTime? joinDate, string invitedBy,
                                                int? numberOfMembers, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchGroupMembers",
                                                                groupID, username, type, active, joinDate, invitedBy, numberOfMembers, sortColumn);

                List<GroupMember> groupMembers = new List<GroupMember>();

                while (reader.Read())
                {
                    GroupMember groupMember = new GroupMember();

                    groupMember.groupID = (int) reader["GroupID"];
                    groupMember.username = (string) reader["Username"];
                    groupMember.type = (eType) reader["Type"];
                    groupMember.active = (bool) reader["Active"];
                    groupMember.joinDate = (DateTime) reader["JoinDate"];
                    groupMember.invitedBy = reader["InvitedBy"] != DBNull.Value
                                                                    ? (string) reader["InvitedBy"] : null;
                    groupMember.joinAnswer = (string) reader["JoinAnswer"];
                    groupMember.isWarned = (bool) reader["IsWarned"];
                    groupMember.warnReason = reader["WarnReason"] != DBNull.Value ? (string) reader["WarnReason"] : null;
                    groupMember.warnExpirationDate = reader["WarnExpirationDate"] != DBNull.Value
                                                         ? (DateTime?) reader["WarnExpirationDate"]
                                                         : null;

                    groupMembers.Add(groupMember);
                }

                return groupMembers.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance into DB.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "SaveGroupMember",
                                            groupID, username, type, active, joinDate, invitedBy, joinAnswer,
                                            isWarned, warnReason, warnExpirationDate);
            }
        }

        /// <summary>
        /// Deletes group member from DB by specified group ID and username.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        public static void Delete(int groupID, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteGroupMember",
                                            groupID, username);
            }
        }

        /// <summary>
        /// Determines whether the specified username is member for the specified group.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="groupID">The group ID.</param>
        /// <returns>
        /// 	<c>true</c> if the specified username is member; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMember(string username, int groupID)
        {
            GroupMember member = Fetch(groupID, username);

            if (member != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Counts the group members for the specified group.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static int Count(int groupID)
        {
            return Count(groupID, null, null);
        }

        /// <summary>
        /// Counts all active or inactive group members for the specified group.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="active">if set to <c>true</c> [active].</param>
        /// <returns></returns>
        public static int Count(int groupID, bool active)
        {
            return Count(groupID, null, active);
        }

        /// <summary>
        /// Counts group members by specified group ID and type.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static int Count(int groupID, eType type)
        {
            return Count(groupID, type, null);
        }

        /// <summary>
        /// Counts group members by specified arguments.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="type">The type.</param>
        /// <param name="active">The active.</param>
        /// <returns></returns>
        private static int Count(int groupID, eType? type, bool? active)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(conn, "FetchGroupMembersCount", groupID, type, active));
            }
        }

        /// <summary>
        /// Counts the invitations for the specified username from the specifed date.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="from">From.</param>
        /// <returns></returns>
        public static int InvitationsCount(string username, DateTime from)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(conn, "FetchGroupInvitationsCount", username, from));
            }
        }

        /// <summary>
        /// Determines whether the specified username is banned for the specified group id.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if the specified group ID is banned; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBanned(string username, int groupID)
        {
            return GroupBan.Fetch(groupID, username).Length > 0 ? true : false;
        }

        public static bool IsAuthorized(UserSession userSession, GroupMember groupMember, Group group)
        {
            if (userSession != null && userSession.IsAdmin())
            {
                return true;
            }
            else if (group == null || (groupMember == null && group.AccessLevel == Group.eAccessLevel.Private))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool HasPermission(UserSession userSession, GroupMember groupMember, Group group, eGroupPermissionType permissionType)
        {
            bool hasNonMembersPermissions = false;
            bool hasMembersPermissions = false;
            bool hasVipMembersPermissions = false;

            switch(permissionType)
            {
                case eGroupPermissionType.ViewGroup:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewGroupNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewGroupMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewGroupVip);
                    break;
                case eGroupPermissionType.ViewMessageBoard:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardVip);
                    break;
                case eGroupPermissionType.ViewGallery:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryVip);
                    break;
                case eGroupPermissionType.ViewMembers:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewMembersMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewMembersVip);
                    break;
                case eGroupPermissionType.ViewEvents:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewEventsMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.ViewEventsVip);
                    break;
                case eGroupPermissionType.UploadPhoto:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.UploadPhotoNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.UploadPhotoMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.UploadPhotoVip);
                    break;
                case eGroupPermissionType.UseChat:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.UseChatNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.UseChatMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.UseChatVip);
                    break;
                case eGroupPermissionType.AddTopic:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddTopicMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddTopicVip);
                    break;
                case eGroupPermissionType.AddPost:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddPostNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddPostMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddPostVip);
                    break;
                case eGroupPermissionType.AddEvent:
                    hasNonMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers);
                    hasMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddEventMembers);
                    hasVipMembersPermissions = group.IsPermissionEnabled(eGroupPermissions.AddEventVip);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("permissionType");
            }

            if (userSession != null)
            {
                if (groupMember == null && !hasNonMembersPermissions)
                {
                    return false;
                }
                else if (groupMember != null)
                {
                    if (!groupMember.Active)
                    {
                        if (!hasNonMembersPermissions)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if ((groupMember.Type == eType.Member && !hasMembersPermissions) ||
                            (groupMember.Type == eType.VIP && !hasVipMembersPermissions))
                        {
                            return false;
                        }
                    }
                }
            }
            else // is not logged in
            {
                if (!hasNonMembersPermissions)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
