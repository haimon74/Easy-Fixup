<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Page language="c#" Codebehind="EditTopic.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.EditTopic" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>
			<%= Lang.TransA("Edit Topic") %>
		</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body >
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_question.jpg"> 
					<div id="crumpstxt"><%= Lang.TransA("Edit Topic") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
					<div id="content-head">
			            <div class="ch-head"><%= Lang.TransA("Edit Topic") %></div>
			        	<div class="ch-description">
			        		<%= Lang.TransA("Here you can add new questions for selected topic or modify existing ones...") %>
			        	</div>		
			        </div>
					<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
				    <table  cellpadding="0" cellspacing="0" class="stable2">
					    <tr>
						    <td align="center" colspan="2" class="table_header">
							    <%= Lang.TransA("Topic Details") %>
						    </td>
					    </tr>
					    <tr>
						    <td class="table_cell"><%= Lang.TransA("Name") %></td>
						    <td class="table_cell"><asp:textbox id="txtTopicTitle" Width="70%" Runat="server"></asp:textbox></td>
					    </tr>
					    <tr>
						    <td class="table_cell"><%= Lang.TransA("EditColumns") %></td>
						    <td class="table_cell"><asp:dropdownlist id="dropEditColumns" Runat="server"></asp:dropdownlist></td>
					    </tr>
					    <tr>
						    <td class="table_cell"><%= Lang.TransA("ViewColumns") %></td>
						    <td class="table_cell"><asp:dropdownlist id="dropViewColumns" Runat="server"></asp:dropdownlist></td>
					    </tr>
				    </table>
				    <div class="separator10"></div>
					<div class="label"><%= Lang.TransA("Topic Questions") %></div>
					<div class="separator06"></div>
				    <asp:datagrid id="dgQuestions" Runat="server" PageSize="10" AllowPaging="False" AutoGenerateColumns="False"
					    Width="100%" CssClass="btable1" cellpadding="0" cellspacing="0"  BorderWidth="0" GridLines="None">
					    <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
					    <HeaderStyle></HeaderStyle>
					    <Columns>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2" Width="20px"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3" Width="20px"></ItemStyle>
							    <ItemTemplate>
								    <input type=checkbox id="cbSelect" value='<%# Eval("QuestionID") %>' runat=server NAME="cbSelect">
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3"></ItemStyle>
							    <ItemTemplate>
								    <%# Eval("Name")%>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3"></ItemStyle>
							    <ItemTemplate>
								    <%# (((string)Eval("Description")).Length > 30)?((string)Eval("Description")).Substring(0,30)+"...":Eval("Description")%>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3"></ItemStyle>
							    <ItemTemplate>
								    <%# ((bool)Eval("Required"))?Lang.TransA("Yes"):Lang.TransA("No")%>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3"></ItemStyle>
							    <ItemTemplate>
								    <asp:LinkButton Text="<IMG border=0 SRC=images\up.gif>" id="lnkUp" CommandName="ChangeOrder" CommandArgument="Up" Runat="server"></asp:LinkButton>
								    <asp:LinkButton Text="<IMG border=0 SRC=images\down.gif>" id="lnkDown" CommandName="ChangeOrder" CommandArgument="Down" Runat="server"></asp:LinkButton>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3"></ItemStyle>
							    <ItemTemplate>
								    <asp:LinkButton ID="lnkEdit" CommandName="EditQuestion" Runat="server">
									    <img src="images/edit_icon.jpg" border="0">
									    <%# Lang.TransA("Edit")%></asp:LinkButton>
							    </ItemTemplate>
						    </asp:TemplateColumn>
					    </Columns>
					    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
				    </asp:datagrid>
				    <div class="separator06"></div>
				    <div class="add-buttons">
					    <asp:Button id="btnAddNewQuestion" runat="server" onclick="btnAddNewQuestion_Click"></asp:Button>
					    <asp:Button id="btnDeleteSelectedQuestions" runat="server" onclick="btnDeleteSelectedQuestions_Click"></asp:Button>
				    </div>
				    <div class="separator10"></div>
				    <div class="add-buttons">
					    <asp:Button id="btnCancel" runat="server" onclick="btnCancel_Click"></asp:Button>
					    <asp:Button id="btnSave" runat="server" onclick="btnSave_Click"></asp:Button>
					</div>
					<div class="separator10"></div>
			    </div>
			</div>
		</form>
	</body>
</HTML>
