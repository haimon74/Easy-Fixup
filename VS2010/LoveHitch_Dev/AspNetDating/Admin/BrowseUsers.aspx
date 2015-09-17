<%@ Page Language="c#" Codebehind="BrowseUsers.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.BrowseUsers" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
	    <title><%= Lang.TransA("Browse Users") %></title>
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
    		<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader><div id="layout">
			    <div id="crupms">
			        <img src="images/i_browsemembers.jpg">
	    	        <div id="crumpstxt"><%= Lang.TransA("User Management") %></div>
	    	    </div>
		        <div id="sidebar">
		        	<uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
		        </div>
		        <div id="main">
		        	<div id="content-head">
			        	<div class="ch-head"><%= Lang.TransA("User Management") %></div>
			            <div class="ch-description">
			            	<%= Lang.TransA("Use this section to browse, edit or delete members of your site...") %>
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
		                        href="javascript: void(0);"><%= Lang.TransA("Expand filter") %></a>&nbsp;]
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
            			    <td class="table_cell"><%= Lang.TransA("Country") %></td>
            			    <td class="table_cell">
            				    <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
            			    </td>
            		    </tr>
            		    <tr>
            			    <td class="table_cell"><%= Lang.TransA("Region/State") %></td>
            			    <td class="table_cell">
            				    <asp:TextBox ID="txtRegion" runat="server"></asp:TextBox>
            			    </td>
            		    </tr>
            		    <tr>
            			    <td class="table_cell"><%= Lang.TransA("City") %></td>
            			    <td class="table_cell">
            				    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
            			    </td>
            		    </tr>
	                    <tr>
		                     <td class="table_cell">
		                     	<%= Lang.TransA("Username") %>:
		                     </td>
	                         <td class="table_cell">
	                         	<asp:TextBox ID="txtUsername" onkeydown="javascript: KeyDownHandler(btnSearch)" runat="server"></asp:TextBox>
	                         </td>
	                    </tr>
	                    <tr>
	                     	<td class="table_cell">
	                        	<%= Lang.TransA("Name") %>:
	                        </td>
	                        <td class="table_cell">
	                        	<asp:TextBox ID="txtName" onkeydown="javascript: KeyDownHandler(btnSearch)" runat="server"></asp:TextBox></td>
	                    </tr>
	                    <tr id="pnlGender" runat="server">
	                     	<td class="table_cell">
	                        	<%= Lang.TransA("Gender") %>:
	                        </td>
	                        <td class="table_cell">
	                        	<asp:DropDownList ID="ddGender" runat="server">
	                        		<asp:ListItem Value=""></asp:ListItem>
	                        	</asp:DropDownList>
	                    	</td>
						</tr>
	                    <tr>
	                    	<td class="table_cell">
            			        <%= Lang.TransA("Age Range") %>&nbsp;<%= Lang.TransA("from") %>
            			    </td>
            			    <td class="table_cell">
                                <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="smalltextbox" Size="2" MaxLength="2"></asp:TextBox>
                                <%= Lang.TransA("to") %>
                                <asp:TextBox ID="txtAgeTo" runat="server" CssClass="smalltextbox" Size="2" MaxLength="2"></asp:TextBox>
            			    </td>						
						</tr>						
	                    <tr>
	                    	<td class="table_cell">
	                        	<%= Lang.TransA("E-mail") %>:
	                        </td>
	                        <td class="table_cell">
	                        	<asp:TextBox ID="txtEmail" onkeydown="javascript: KeyDownHandler(btnSearch)" runat="server"></asp:TextBox>
	                        </td>
						</tr>
	                    <tr>
	                    	<td class="table_cell">
	                        	<%= Lang.TransA("Signup IP") %>:
	                        </td>
	                        <td class="table_cell">
	                        	<asp:TextBox ID="txtIP" onkeydown="javascript: KeyDownHandler(btnSearch)" runat="server"></asp:TextBox>
	                        </td>
						</tr>
	                    <tr>
	                    	<td class="table_cell">
	                        	<%= Lang.TransA("Has Photo") %>:
	                        </td>
	                        <td class="table_cell">
	                        	<asp:DropDownList ID="ddPhoto" runat="server">
	                            	<asp:ListItem Value=""></asp:ListItem>
	                            </asp:DropDownList>
	                        </td>
						</tr>
						<tr>
	                    	<td class="table_cell">
	                        	<%= Lang.TransA("Activated") %>:
	                        </td>
	                        <td class="table_cell">
	                        	<asp:DropDownList ID="ddActive" runat="server">
	                            	<asp:ListItem Value=""></asp:ListItem>
	                            </asp:DropDownList>
	                        </td>
						</tr>
						<tr>
		                    <td class="table_cell">
	                        	<%= Lang.TransA("Deleted") %>:
	                        </td>
	                        <td class="table_cell">
	                        	<asp:DropDownList ID="ddDeleted" runat="server">
	                            	<asp:ListItem Value=""></asp:ListItem>
	                            </asp:DropDownList>
	                        </td>
						</tr>
						<tr>
	                    	<td class="table_cell">
	                        	<%= Lang.TransA("Paid Member") %>:
	                        </td>
	                        <td class="table_cell">
	                        	<asp:DropDownList ID="ddPaid" runat="server">
	                            	<asp:ListItem Value=""></asp:ListItem>
	                            </asp:DropDownList>
	                    </td>
						</tr>
	                    <tr>
	                    	<td colspan="2" class="table_footer" align="center">
	                        	<div class="separator06"></div>
	                            <asp:Button ID="btnSearch" Font-Name="Verdana" runat="server"></asp:Button>
	                        </td>
						</tr>
					</table>
	                <script>
						if (document.all['<%= tblHideSearch.ClientID %>'])
						document.all['tblSearch'].style.display = 'none';
	                </script>
					<div class="separator06"></div>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
	                <table cellpadding="0" cellspacing="0" width="100%">
	                	<tr>
	                    	<td align="right" class="perpage">
	                        	<%= Lang.TransA("Users per page") %>:
	                            <asp:DropDownList ID="dropUsersPerPage" CssClass="pages" Font-Name="Verdana" runat="server" AutoPostBack="True"></asp:DropDownList>
	                            <div class="separator06"></div>
	                        </td>
						</tr>
					</table>
                	<asp:DataGrid Width="100%" CssClass="btable1" ID="dgUsers" runat="server" AllowPaging="False"
                		AutoGenerateColumns="False" AllowSorting="True" CellPadding="0" CellSpacing="0" BorderWidth="0" GridLines="None">
                		<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                		<Columns>
                    		<asp:TemplateColumn SortExpression="Username">
                        		<HeaderStyle CssClass="table_header2"></HeaderStyle>
                            	<ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
	                        	<ItemTemplate>
	                            	<a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>">
	                                <%# Eval("Username")%></a>
	                            </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="Name">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="Gender">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Gender") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn Visible="False" SortExpression="Birthdate">
                                <HeaderStyle CssClass="table_header2" ></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Birthdate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="SignupDate">
                                <HeaderStyle CssClass="table_header2" ></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("SignupDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="SignupIP">
                                <HeaderStyle CssClass="table_header2" ></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("CountryCode").ToString().Length > 0 ? String.Format("<img src=\"images/countryicons/{0}.png\" title=\"{1}\">", Eval("CountryCode"), Eval("CountryName")) : "" %>
                                    <%# Eval("SignupIP") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="Email">
                                <HeaderStyle CssClass="table_header2" ></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                <ItemTemplate>
                                    <a href='<%# "mailto:" + Eval("Email") %>'>
                                        <%# Eval("Email") %></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2" ></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                <ItemTemplate>
                                    <nobr>
                                        <a href="EditProfile.aspx?username=<%# Eval("Username") %>">
                                            <img src="images/edit_icon.jpg" border="0">
                                            <%= Lang.TransA("Profile")%></a><a href="EditPhotos.aspx?username=<%# Eval("Username") %>">
                                            <img src="images/edit_icon.jpg" border="0">
                                            <%= Lang.TransA("Photos")%></a>
                                    </nobr>
                            	</ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2" ></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkManage" runat="server" CommandName="Manage"
                                        CommandArgument='<%# Eval("Username")%>'>
										<%# Lang.TransA("Manage")%></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <asp:Panel ID="pnlPaginator" runat="server">
                    	<div class="separator06"></div>
                        <table cellspacing="0" cellpadding="0" width="100%" class="pagenumber">
                        	<tr>
                            	<td>
                                	<asp:LinkButton ID="lnkFirst" CssClass="pagelink" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkPrev" CssClass="pagelink" runat="server"></asp:LinkButton>
                                    <asp:Label ID="lblPager" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkNext" CssClass="pagelink" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkLast" CssClass="pagelink" runat="server"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                   </ContentTemplate>
                  </asp:UpdatePanel>
                  <asp:Button ID="btnGetCSV" runat="server" onclick="btnGetCSV_Click" Visible="false"/>
				</div>
        	</div>
   		</form>
	</body>
</html>