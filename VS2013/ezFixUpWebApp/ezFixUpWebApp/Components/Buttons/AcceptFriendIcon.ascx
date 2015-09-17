<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="AcceptFriendIcon.ascx.cs" 
            Inherits="ezFixUp.Components.Buttons.AcceptFriendIcon" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagName="GenericIcon" Src="~/Components/Buttons/GenericIcon.ascx" TagPrefix="btn" %>

<btn:GenericIcon ID="btnIcon" runat="server" 
                 ToolTip="Accept friendship request" 
                 TipGravity="e" 
                 IconSize="Mini"
                 IconBaseCss="AcceptFriendBtn"
                 UrlTemplate="~/AcceptDenyFriendship.aspx?cmd=accept&src=profile&uid={0}"/>