<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="AddFriendIcon.ascx.cs" 
            Inherits="ezFixUp.Components.Buttons.AddFriendIcon" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagName="GenericIcon" Src="~/Components/Buttons/GenericIcon.ascx" TagPrefix="btn" %>

<btn:GenericIcon ID="btnIcon" runat="server" 
                 ToolTip="Add to friends list" 
                 TipGravity="e" 
                 IconSize="Mini"
                 IconBaseCss="AddFriendBtn"
                 UrlTemplate="~/AddRemoveFriend.aspx?cmd=add&src=profile&uid={0}"/>