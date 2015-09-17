<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Page Language="c#" Codebehind="BrowseMessages.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.BrowseMessages" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
	    <title><%= Lang.TransA("Browse Messages") %></title>
    	<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
    	<meta content="C#" name="CODE_LANGUAGE">
    	<meta content="JavaScript" name="vs_defaultClientScript">
   		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
    	<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
    	<script language="javascript">
			function KeyDownHandler(btn)
			{
				// process only the Enter key
				if (event.keyCode == 13)
				{
					// cancel the default submit
					event.returnValue=false;
					event.cancel = true;
					// submit the form by programmatically clicking the specified button
					btn.click();
				}
			}
    	</script>
	</head>
	<body>
    	<form id="Form1" method="post" runat="server">
       		<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
        	<div id="layout">
	    		<div id="crupms">
            		<img src="images/i_browsemessage.jpg">
                	<div id="crumpstxt">
                		<%= Lang.TransA("Browse Messages") %>
                	</div>
            	</div>
            	<div id="sidebar">
            		<uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
            	</div>
            	<div id="main">
	        		<div id="content-head">
		        		<div class="ch-head"><%= Lang.TransA("Messages Management") %></div>
                    	<div class="ch-description">
                    	    <%= Lang.TransA("Use this section to browse user messages...") %>
                    	</div>
					</div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
                    <table id="tblHideSearch" cellpadding="0" cellspacing="0" class="filter" runat="server" visible="false">
                    	<tr>
                        	<td class="table_header">
                            	<%= Lang.TransA("Filter") %>
                        	</td>
                        	<td class="table_header">
                        	[&nbsp;<a onclick="document.all['tblSearch'].style.display = 'block'; document.all['tblHideSearch'].style.display = 'none';"
                        	href="javascript: void(0);"><%= Lang.TransA("Expand filter") %></a>&nbsp;]&nbsp;
                         	</td>
                     	</tr>
					</table>
                    <table id="tblSearch" cellpadding="0" cellspacing="0" class="filter">
                    	<tr>
                    	    <td colspan="2" class="table_header">
                    	        <%= Lang.TransA("Filter") %>
                            </td>
                        </tr>
                        <tr>
                    		<td class="table_cell">
                            	<%= Lang.TransA("Sender") %>:
                            </td>
                            <td class="table_cell">
                            	<asp:TextBox ID="txtFromUsername" onkeydown="javascript: KeyDownHandler(btnSearch)"
                            	runat="server"></asp:TextBox>
                            </td>
						</tr>
                        <tr>
                        	<td class="table_cell">
                            	<%= Lang.TransA("Recipient") %>:
                            </td>
                            <td class="table_cell">
                            	<asp:TextBox ID="txtToUsername" onkeydown="javascript: KeyDownHandler(btnSearch)"
                            	runat="server"></asp:TextBox>
                            </td>
						</tr>
                        <tr>
	                        <td class="table_cell">
                            	<%= Lang.TransA("Keyword") %>:
                            </td>
                        	<td class="table_cell">
                            	<asp:TextBox ID="txtKeyword" onkeydown="javascript: KeyDownHandler(btnSearch)" runat="server"></asp:TextBox></td>
                  		</tr>
                        <tr>
                        	<td colspan="2" class="table_footer">
                        		<div class="separator06"></div>
                        		<div class="add-buttons">
                      				<asp:Button ID="btnSearch" Font-Name="Verdana" runat="server"></asp:Button>
                      			</div>
                      		</td>
                        </tr>
					</table>
                    <script>
						if (document.all['<%= tblHideSearch.ClientID %>'])
						document.all['tblSearch'].style.display = 'none';
                    </script>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <table align="center" width="100%" cellpadding="0" cellspacing="0">
                    	<tr>
                        	<td align="right" class="perpage">
                            	<%= Lang.TransA("Messages per page") %>:
                            	<asp:DropDownList ID="dropMessagesPerPage" CssClass="pages" Font-Name="Verdana" runat="server" AutoPostBack="True"></asp:DropDownList>
                            </td>
                        </tr>
					</table>
                    <div class="separator06"></div>
                    <asp:DataGrid Width="100%" ID="dgMessages" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                    AllowSorting="True" CssClass="btable1" CellPadding="0" CellSpacing="0" BorderWidth="0" GridLines="None">
	                    <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
	                        <Columns>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                                        <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("FromUsername") %>">
                                            <%# Eval("FromUsername")%></a><%# Eval("FromGender") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                                        <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("ToUsername") %>">
                                            <%# Eval("ToUsername") %></a><%# Eval("ToGender") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="True"></ItemStyle>
                                    <ItemTemplate>
                                        <%# Eval("Body") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                                        <%# Eval("Timestamp") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete"
                                            CommandArgument='<%# Eval("Id") %>'>
												<%# Lang.TransA("Delete") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                    	    </Columns>
                    	<PagerStyle Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <asp:Panel ID="pnlPaginator" runat="server">
                       	<table cellspacing="0" cellpadding="0" width="100%" class="pagenumber">
                       		<tr>
                       	    	<td>
                       	        	<asp:LinkButton ID="lnkFirst" runat="server" CssClass="pagelink"></asp:LinkButton>
                       	        	<asp:LinkButton ID="lnkPrev" runat="server" CssClass="pagelink"></asp:LinkButton>
                       	        	<asp:Label ID="lblPager" runat="server"></asp:Label>
                       	        	<asp:LinkButton ID="lnkNext" runat="server" CssClass="pagelink"></asp:LinkButton>
                       	        	<asp:LinkButton ID="lnkLast" runat="server" CssClass="pagelink"></asp:LinkButton>
                       	        </td>
							</tr>
						</table>
                    </asp:Panel>
                   </ContentTemplate>
                  </asp:UpdatePanel>
                  <asp:Button ID="btnGetCSV" runat="server" Visible="false" 
                        onclick="btnGetCSV_Click"/>
	            </div>
	        </div>
    	</form>
	</body>
</html>
