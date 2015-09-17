<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="AddFavoriteIcon.ascx.cs" 
            Inherits="AspNetDating.Components.Buttons.AddFavoriteIcon" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagName="GenericIcon" Src="~/Components/Buttons/GenericIcon.ascx" TagPrefix="btn" %>

<btn:GenericIcon ID="btnIcon" runat="server" 
                 ToolTip="Add to favorites list" 
                 TipGravity="e" 
                 IconSize="Mini"
                 IconBaseCss="AddFavoriteBtn"
                 UrlTemplate="~/AddRemoveFavorite.aspx?cmd=add&src=profile&uid={0}"/>