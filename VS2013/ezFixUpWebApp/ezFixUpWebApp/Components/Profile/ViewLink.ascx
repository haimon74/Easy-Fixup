<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="ViewLink.ascx.cs" 
            Inherits="ezFixUp.Components.Profile.ViewLink" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div class="skincolor">
<asp:Label CssClass="label skincolor skincolorlabel" id="lblName" runat="server" />:
<a id="lnkValue" CssClass="skincolor translatable" Runat="server" />
</div>