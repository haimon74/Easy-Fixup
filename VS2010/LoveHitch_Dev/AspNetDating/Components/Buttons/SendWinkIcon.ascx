<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="SendWinkIcon.ascx.cs" 
            Inherits="AspNetDating.Components.Buttons.SendWinkIcon" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagName="GenericIcon" Src="~/Components/Buttons/GenericIcon.ascx" TagPrefix="btn" %>

<btn:GenericIcon ID="btnIcon" runat="server" 
                 ToolTip="Send wink" 
                 TipGravity="e" 
                 IconSize="Mini"
                 IconBaseCss="WinkBtn"
                 UrlTemplate="~/SendWink_to_{0}.aspx"/>