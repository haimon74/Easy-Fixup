<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="MoreIcon.ascx.cs" 
            Inherits="ezFixUp.Components.Buttons.MoreIcon" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagName="GenericIcon" Src="~/Components/Buttons/GenericIcon.ascx" TagPrefix="btn" %>

<btn:GenericIcon ID="btnIcon" runat="server" 
                 ToolTip="More options" 
                 TipGravity="e" 
                 IconSize="Mini"
                 IconBaseCss="MoreBtn"
                 UrlTemplate="#"/>