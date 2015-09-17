<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="SendGiftIcon.ascx.cs" 
            Inherits="AspNetDating.Components.Buttons.SendGiftIcon" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagName="GenericIcon" Src="~/Components/Buttons/GenericIcon.ascx" TagPrefix="btn" %>

<btn:GenericIcon ID="btnIcon" runat="server" 
                 ToolTip="Send gift" 
                 TipGravity="e" 
                 IconSize="Mini"
                 IconBaseCss="GiftBtn"
                 UrlTemplate="~/SendGift_to_{0}.aspx"/>