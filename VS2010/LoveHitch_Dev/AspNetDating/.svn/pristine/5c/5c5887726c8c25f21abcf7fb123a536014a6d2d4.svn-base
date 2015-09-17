<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="Blogs.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.MemberBlog" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Settings" Src="Components/Blog/Settings.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AddPost" Src="Components/Blog/AddPost.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewBlog" Src="Components/Blog/ViewBlog.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
		<uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
		<div class="SideMenuItem">
			<asp:linkbutton id="lnkNewPost" Runat="server" CssClass="SideMenuLink" onclick="lnkNewPost_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem">
			<asp:linkbutton id="lnkViewBlog" Runat="server" CssClass="SideMenuLink" onclick="lnkViewBlog_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem">
			<asp:linkbutton id="lnkEditBlog" Runat="server" CssClass="SideMenuLink" onclick="lnkEditBlog_Click"></asp:linkbutton>
		</div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
	</div>
	<div id="main">
		<uc1:settings id="Settings" runat="server"></uc1:settings>
		<uc1:addpost id="AddPost" runat="server"></uc1:addpost>
		<uc1:viewblog id="ViewBlog" runat="server"></uc1:viewblog>
	</div>
</asp:Content>