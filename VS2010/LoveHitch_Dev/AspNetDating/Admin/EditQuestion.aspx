<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Page language="c#" Codebehind="EditQuestion.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.EditQuestion" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Edit Question") %></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_question.jpg"> 
					<div id="crumpstxt"><%= Lang.TransA("Question") %></div>
				</div>
				<div id="sidebar">
					<uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
				</div>
				<div id="main">
					<div id="content-head">
					    <div class="ch-head"><%= Lang.TransA("Question") %></div>
			            <div class="ch-description">
			            	<%= Lang.TransA("Here you can describe your question and its possible answers.Also you can set question's view, edit and search styles...") %>
			            </div>	
		            </div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>	
				    <table border="0" cellPadding="0" cellspacing="0" id="stable">
					    <tr>
						    <td class="table_header" colSpan="2"><%= Lang.TransA("Question Details") %></td>
					    </tr>
					    <tr>
						    <td class="table_cell" width="25%"><%= Lang.TransA("Name") %></td>
						    <td class="table_cell"><asp:textbox id="txtName" runat="server"></asp:textbox></td>
					    </tr>
					    <tr>
						    <td class="table_cell" noWrap><%= Lang.TransA("Alternative Name") %></td>
						    <td class="table_cell"><asp:textbox id="txtAltName" runat="server"></asp:textbox></td>
					    </tr>
					    <tr>
						    <td class="table_cell" noWrap><%= Lang.TransA("Description") %></td>
						    <td class="table_cell">
						    	<asp:textbox id="txtDescription" CssClass="textbox" Rows="5" runat="server" TextMode="MultiLine" Width="100%"></asp:textbox>
						    </td>
					    </tr>
					    <tr>
						    <td class="table_cell" noWrap><%= Lang.TransA("Hint") %></td>
						    <td class="table_cell">
						    	<asp:textbox id="txtHint" runat="server" Width="100%"></asp:textbox>
					        </td>
					    </tr>
					    <tr>
						    <td class="table_cell"><%= Lang.TransA("Edit Style") %></td>
						    <td class="table_cell"><asp:dropdownlist id="dropEditStyle" runat="server" AutoPostBack="True"></asp:dropdownlist></td>
					    </tr>
					    <tr id="pnlShowStyle" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Show Style") %></td>
						    <td class="table_cell"><asp:dropdownlist id="dropShowStyle" runat="server"></asp:dropdownlist></td>
					    </tr>
					    <tr id="pnlSearchStyle" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Search Style") %></td>
						    <td class="table_cell"><asp:DropDownList ID="dropSearchStyle" runat="server"></asp:DropDownList></td>
					    </tr>
					    <tr id="pnlMatchField" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Match field") %></td>
						    <td class="table_cell"><asp:DropDownList ID="ddMatchFieldQuestion" runat="server" 
                                    onselectedindexchanged="ddMatchFieldQuestion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
					    </tr>
					    <tr id="pnlShowCondition" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Show condition") %></td>
						    <td class="table_cell"><asp:DropDownList ID="ddCondition" runat="server" 
                                    AutoPostBack="true" 
                                    onselectedindexchanged="ddCondition_SelectedIndexChanged"></asp:DropDownList></td>
					    </tr>
					    <tr id="pnlRequired" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Required") %></td>
						    <td class="table_cell"><asp:checkbox id="cbRequired" runat="server"></asp:checkbox></td>
					    </tr>
					    <tr>
						    <td class="table_cell"><%= Lang.TransA("Requires Approval") %></td>
						    <td class="table_cell"><asp:checkbox id="cbRequiresApproval" runat="server"></asp:checkbox></td>
					    </tr>
					    <tr>
						    <td class="table_cell"><%= Lang.TransA("Viewable only by paid members")%></td>
						    <td class="table_cell"><asp:checkbox id="cbVisibleOnlyForPaidMembers" runat="server"></asp:checkbox></td>
					    </tr>
					    <tr>
						    <td class="table_cell"><%= Lang.TransA("Editable only by paid members")%></td>
						    <td class="table_cell"><asp:checkbox id="cbEditableOnlyByPaidMembers" runat="server"></asp:checkbox></td>
					    </tr>
					    <tr id="pnlAppliesTo" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Applies To") %></td>
						    <td class="table_cell">
							    <asp:CheckBox ID="cbVisibleToMale" Runat="server"></asp:CheckBox><br>
							    <asp:CheckBox ID="cbVisibleToFemale" Runat="server"></asp:CheckBox><br>
							    <asp:CheckBox ID="cbVisibleToCouple" Visible="False" Runat="server"></asp:CheckBox>
						    </td>
					    </tr>
				    </table>
					<div class="separator10"></div>
					<asp:Panel ID="pnlAnswers" Runat="server">
					    <TABLE cellSpacing="0" cellPadding="0" class="stable2">
						    <TR>
							    <TD class="table_header">
						      <%= Lang.TransA("Answers") %></TD>
						    </TR>
						    <TR>
							    <TD>
								    <asp:datagrid id="dgChoices" BorderWidth="0" Runat="server" Width="100%"
								     cellspacing="0" cellpadding="0" AutoGenerateColumns="False" AllowPaging="False"
									    PageSize="10" ShowHeader="False" GridLines="None">
									    <AlternatingItemStyle></AlternatingItemStyle>
									    <Columns>
										    <asp:TemplateColumn>
											    <ItemStyle CssClass="table_cell" Wrap="False" HorizontalAlign="Center" Width="1%"></ItemStyle>
											    <ItemTemplate>
												    <input type="checkbox" id="cbSelect" value='<%# Eval("ChoiceID") %>' runat="server" NAME="cbSelect" />
											    </ItemTemplate>
										    </asp:TemplateColumn>
										    <asp:TemplateColumn>
											    <ItemStyle CssClass="table_cell" Width="100%"></ItemStyle>
											    <ItemTemplate>
												    <asp:TextBox ID="txtValue" Text='<%# Eval("Value")%>' Runat=server>
												    </asp:TextBox>
											    </ItemTemplate>
										    </asp:TemplateColumn>
									    </Columns>
									    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
								    </asp:datagrid>
								    <div class="separator06"></div>
								    <DIV align="center" class="font">
									    <%= Lang.TransA("Add") %>
									    <asp:dropdownlist id="dropNewChoicesCount" runat="server" Font-Size="11px" Font-Name="Verdana"></asp:dropdownlist>
									    <%= Lang.TransA("new choices") %>&nbsp;
									    <asp:button id="btnAddNewChoices" runat="server"></asp:button>
									    <div class="separator10"></div>
									    <asp:button id="btnDeleteSelectedChoices" runat="server"></asp:button></DIV>
							    </TD>
						    </TR>
					    </TABLE>
					</asp:Panel>
					<div class="separator10"></div>
					<asp:Panel ID="pnlConditionAnswers" runat="server">
					    <TABLE cellSpacing="0" cellPadding="0" class="stable2">
						    <TR>
							    <TD class="table_header">
						      <%= Lang.TransA("Conditional Answers") %></TD>
						    </TR>
						    <TR>
							    <TD>
							        <asp:CheckBoxList ID="cblConditionAnswers" runat="server"></asp:CheckBoxList>
							    </TD>
						    </TR>
					    </TABLE>
					</asp:Panel>
					<div class="separator10"></div>
					<div class="add-buttons2">
						<div class="add-buttons">
						    <asp:button id="btnCancel" runat="server"></asp:button>
						    <asp:button id="btnSave" runat="server"></asp:button>
						</div>
					</div>
					<div class="separator10"></div>
			    </div>
			</div>
		</form>
	</body>
</HTML>
