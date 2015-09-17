<%@ Page language="c#" Codebehind="ApproveAnswers.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.ApproveAnswers" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title><%= Lang.TransA("Approve Answers") %></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_approveanswers.jpg"> 
					<div id="crumpstxt"> <%= Lang.TransA("Approve Answers") %></div>    
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
        			<div id="content-head">
	        			<div class="ch-head"><%= Lang.TransA("Approve Answers") %></div>
	        			<div class="ch-description">
	        				<%= Lang.TransA("Use this section to approve or reject pending answers that require approval...") %>
	        			</div>
        			</div>
        			<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
        			<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<table width="100%" cellpadding=0 cellspacing=0>
						<tr>
							<td align="right" class="perpage">
								<asp:Label id="lblAnswersPerPage" Font-Name="Verdana" Font-Size="11px" runat="server"></asp:Label>
								<asp:DropDownList id="dropAnswersPerPage" CssClass="pages" runat="server" AutoPostBack="True"></asp:DropDownList>
								<div class="separator06"></div>
							</td>
						</tr>
					</table>					
					<asp:DataGrid id="dgPendingApproval" Runat="server" Width=100% PageSize="2" AllowPaging="True" AutoGenerateColumns="False"
					CssClass="btable1" cellpadding="0" cellspacing="0"  BorderWidth="0" GridLines="None">
						<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
						<HeaderStyle></HeaderStyle>
						<Columns>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2"></HeaderStyle>
								<ItemStyle CssClass="table_cell3" Wrap="False" Width="15%"></ItemStyle>
								<ItemTemplate>
								<a target="_blank" href="<%= AspNetDating.Classes.Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>"><%# Eval("Username")%></a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2" Wrap=False></HeaderStyle>
								<ItemStyle CssClass="table_cell3" Width="20%"></ItemStyle>
								<ItemTemplate>
								<%# Eval("Question")%>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2" Wrap=False></HeaderStyle>
								<ItemStyle CssClass="table_cell3" Width="45%"></ItemStyle>
								<ItemTemplate>
									<%# Eval("Answer")%>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2"></HeaderStyle>
								<ItemStyle CssClass="table_cell3" Width="10%" VerticalAlign="Middle"></ItemStyle>
								<ItemTemplate>
									<asp:LinkButton ID="lnkApprove" CommandName="Approve" CommandArgument='<%# Eval("Username") + ":" + Eval("QuestionID")%>' Runat="server"><IMG title="Approve" border=0 SRC=images/approve.gif></asp:LinkButton>
									<asp:LinkButton ID="lnkReject" CommandName="Reject" CommandArgument='<%# Eval("Username") + ":" + Eval("QuestionID")%>'  Runat="server"><IMG title="Reject" border=0 SRC=images/disapprove.gif></asp:LinkButton>										
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2"></HeaderStyle>
								<ItemStyle CssClass="table_cell3" Width="10%"></ItemStyle>
								<ItemTemplate>
									<a href="ApproveAnswer.aspx?uid=<%# Eval("Username")%>&qid=<%# Eval("QuestionID")%>"><img src="images/edit_icon.jpg" border=0> <%# Lang.TransA("Edit")%></a>
								</ItemTemplate>
							</asp:TemplateColumn>																						
						</Columns>
						<PagerStyle CssClass="table_cell" Font-Name="Verdana" Font-Size="11px" HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
					</asp:DataGrid>
				</ContentTemplate>
				</asp:UpdatePanel>
				</div>
			</div>			
		</form>
	</body>
</HTML>
