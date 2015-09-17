<%@ Page language="c#" Codebehind="ApproveVideos.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.ApproveVideos" %>
<%@ Register TagPrefix="uc1" TagName="VideoRecorder" Src="../VR/VideoRecorder.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DeleteButton" Src="DeleteVideoButton/DeleteButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
    <head>
    	<title><%= Lang.TransA("Approve Videos") %></title>
	    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
	    <meta name="CODE_LANGUAGE" Content="C#">
	    <meta name=vs_defaultClientScript content="JavaScript">
	    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
   			<div id="layout">						
   				<div id="crupms">
				    <img src="images/i_avpprovevideos.jpg"> 
			        <div id="crumpstxt"> <%= Lang.TransA("Approve Videos") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
        			<div id="content-head">
	        			<div class="ch-head">
							<%= Lang.TransA("Approve Videos") %>
						</div>
			            <div class="ch-description">
			            	<%= Lang.TransA("Use this section to approve or reject pending videos...") %>
			            </div>
			        </div>
			        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
			        <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<table width="100%" cellpadding=0 cellspacing=0>
						<tr>
							<td align="right">
								<asp:Panel ID="pnlVideosPerPage" runat="server" CssClass="perpage">
									<%= Lang.TransA("Videos per page") %>:
									<asp:DropDownList id="dropVideosPerPage" CssClass="pages" Font-Name="Verdana" Font-Size="9px" runat="server" AutoPostBack="True"></asp:DropDownList>
								</asp:Panel>
	                            <div class="separator06"></div>
							</td>
						</tr>
					</table>					
					<asp:DataGrid id="dgPendingVideos" Runat="server" Width="100%" PageSize="2" AllowPaging="True" 
					AutoGenerateColumns="False"	CssClass="btable1" cellpadding="0" cellspacing="0"  BorderWidth="0" GridLines="None">
						<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
						<HeaderStyle></HeaderStyle>
						<Columns>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2"></HeaderStyle>
								<ItemStyle CssClass="table_cell3" Wrap="False" Width="10%"></ItemStyle>
								<ItemTemplate>
									<a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>"><%# Eval("Username")%></a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2c" Wrap="False"></HeaderStyle>
								<ItemStyle CssClass="table_cell3c" Width="90%"></ItemStyle>
								<ItemTemplate>
									<uc1:VideoRecorder id="VideoRecorder1" runat="server"></uc1:VideoRecorder>
									<div class="separator06"></div>
									<uc1:DeleteButton id="DeleteButton" runat="server"></uc1:DeleteButton>
									<div class="separator06"></div>
									<asp:Button ID="btnApprove" Runat="server"></asp:Button>
								</ItemTemplate>
							</asp:TemplateColumn>																					
						</Columns>
						<PagerStyle CssClass="table_cell" Font-Name="Verdana" Font-Size="11px" HorizontalAlign="center" Mode="NumericPages"></PagerStyle>
					</asp:DataGrid>	
				</ContentTemplate>
				</asp:UpdatePanel>				
				</div>
			</div>
	    </form>	
	</body>
</html>
