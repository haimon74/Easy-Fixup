<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="RemoveFavoriteIcon.ascx.cs" 
            Inherits="AspNetDating.Components.Buttons.RemoveFavoriteIcon" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagName="GenericIcon" Src="~/Components/Buttons/GenericIcon.ascx" TagPrefix="btn" %>

<btn:GenericIcon ID="btnIcon" runat="server" 
                 ToolTip="Remove from favorites list" 
                 TipGravity="e" 
                 IconSize="Mini"
                 IconBaseCss="RemoveFavoriteBtn"
                 UrlTemplate="~/AddRemoveFavorite.aspx?cmd=remove&src=profile&uid={0}"/>