<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AddPost.ascx.cs" Inherits="ezFixUp.Components.Blog.AddPostCtrl"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<uc1:largeboxstart id="LargeBoxStart" runat="server"></uc1:largeboxstart>
<uc1:headerline id="hlBlogSettings" runat="server"></uc1:headerline>
<div class="wrap-sized" >
	<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
    <div class="clear"></div>
    <%= Lang.Trans("Title") %>
    <br/>
    <asp:TextBox ID="txtName" CssClass="titletextbox" runat="server"></asp:TextBox>
    <div class="separator"></div>
    <%= Lang.Trans("Description") %>
    <br/>
    <div class="fckeditor">
        <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
    </div>
</div>
<div class="separator"></div>
<div class="buttons">
    <asp:Button runat="server" ID="btnCancel" Visible="False"></asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button
        runat="server" ID="btnSaveChanges" onclick="btnSaveChanges_Click"></asp:Button>
</div>
<div class="separator"></div>
<uc1:largeboxend runat="server" id="Largeboxend1">
</uc1:largeboxend>