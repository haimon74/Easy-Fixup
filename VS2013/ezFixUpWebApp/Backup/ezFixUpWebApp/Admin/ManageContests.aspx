<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageContests.aspx.cs" Inherits="ezFixUp.Admin.ManageContests" %>

<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title> <%= Lang.TransA("Contests") %></title>
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
					<img src="images/i_contests.jpg"> 
					<div id="crumpstxt"> <%= Lang.TransA("Manage Contests") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="Adminmenu1" runat="server"></uc1:AdminMenu>
				</div>
			    <div id="main">
					<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Manage Contests") %></div>
						<div class="ch-description"><%= Lang.TransA("Here you can create, edit and delete contests") %></div>
					</div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<div id="pnlSearchInfo" runat="server">
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
	                    	    <td class="table_cell">
	                        	    <%= Lang.TransA("Active") %>:
	                            </td>
	                            <td class="table_cell">
	                        	    <asp:DropDownList ID="ddActive" runat="server">
	                            	    <asp:ListItem Value=""></asp:ListItem>
	                                </asp:DropDownList>
	                            </td>
						    </tr>
                            <tr>
                                <td colspan="2" class="table_footer" align="center">
                                    <div class="separator10"></div>
                                        <asp:Button ID="btnSearch" Font-Name="Verdana" runat="server" OnClick="btnSearch_Click"></asp:Button>
                                </td>
                            </tr>						
					    </table>
					    <div class="separator10"></div>
                    <script type="text/javascript">
					    if (document.all['<%= tblHideSearch.ClientID %>'])
					    document.all['tblSearch'].style.display = 'none';
	                </script>
                    <asp:DataGrid Width="100%" CssClass="btable1" ID="dgContests" GridLines="None" runat="server" AllowPaging="False"
                        AutoGenerateColumns="False" AllowSorting="True" CellPadding="0" CellSpacing="0"
                        BorderWidth="0" OnSortCommand="dgContests_SortCommand" OnItemCommand="dgContests_ItemCommand" OnItemDataBound="dgContests_ItemDataBound">
                        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                        <Columns>
                            <asp:TemplateColumn SortExpression="Name">
                                <HeaderStyle CssClass="table_header2" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Name")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="Gender">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Gender")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>                            
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("AgeRange")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="DateCreated">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="True"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("DateCreated")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>                            
                            <asp:TemplateColumn SortExpression="DateEnds">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("DateEnds")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                    <img src="images/edit_icon.jpg" border="0">&nbsp;<asp:LinkButton ID="lnkViewEntries" CommandName="ViewEntries" CommandArgument='<%# Eval("Id") %>' runat="server"></asp:LinkButton>&nbsp;&nbsp;
                                    <img src="images/edit_icon.jpg" border="0">&nbsp;<asp:LinkButton ID="lnkEditContest" CommandName="EditContest" CommandArgument='<%# Eval("Id") %>' runat="server"></asp:LinkButton>&nbsp;&nbsp;
                                    <img src="images/edit_icon.jpg" border="0">&nbsp;<asp:LinkButton ID="lnkDeleteContest" CommandName="DeleteContest" CommandArgument='<%# Eval("Id") %>' runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>                            
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <div class="separator06"></div>
                    <table cellspacing="0" cellpadding="0" width="100%" class="pagenumber">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkFirst" CssClass="pagelink" runat="server" OnClick="lnkFirst_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lnkPrev" CssClass="pagelink" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
                                <asp:Label ID="lblPager" runat="server"></asp:Label>
                                <asp:LinkButton ID="lnkNext" CssClass="pagelink" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lnkLast" CssClass="pagelink" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <div class="separator06"></div>
                    </div>
                    <div align="center">
                    	<asp:Button ID="btnAddNewContest" runat="server" OnClick="btnAddNewContest_Click" />
                    </div>
                    <div class="separator06"></div>
                    <div id="pnlAddNewContest" visible="false" runat="server">
                        <table cellpadding="0" cellspacing="0" class="ttable">
                    	    <tr>
                        	    <td class="table_header" colspan="2">
                            	    <%= Lang.TransA("Contest") %>
                       		    </td>
                   		    </tr>
                   		    <tr>
                   		        <td class="table_cell">
                                    <%= Lang.TransA("Contest Name") %>:
                   		        </td>
                   		        <td class="table_cell">
                                    <asp:TextBox id="txtName" CssClass="tsingleline" runat="server"></asp:TextBox>
                   		        </td>
							</tr>
                   		    <tr>
                   		        <td class="table_cell">
                                    <%= Lang.TransA("Description") %>:
                   		        </td>
                   		        <td class="table_cell">
                                    <asp:TextBox id="txtDescription" TextMode="MultiLine" CssClass="tmultiline" runat="server"></asp:TextBox>
                   		        </td>
							</tr>
                   		    <tr id="pnlGender" runat="server">
                   		        <td class="table_cell">
                                    <%= Lang.TransA("Restricted to Gender") %>:
                   		        </td>
                   		        <td class="table_cell">
                                    <asp:DropDownList ID="ddGender" runat="server"></asp:DropDownList>
                   		        </td>
							</tr>
                   		    <tr>
                   		        <td class="table_cell">
                                    <%= Lang.TransA("Terms") %>:
                   		        </td>
                   		        <td class="table_cell">
                                    <asp:TextBox id="txtTerms" TextMode="MultiLine" CssClass="tmultiline" runat="server"></asp:TextBox>
                   		        </td>
							</tr>
                   		    <tr id="pnlAge" runat="server">
                   		        <td class="table_cell">
                                    <%= Lang.TransA("Age Range") %>
                   		        </td>
                   		        <td class="table_cell">
                                    <%= Lang.TransA("from") %>&nbsp;<asp:DropDownList ID="ddFromAge" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;<%= Lang.TransA("to") %>&nbsp;<asp:DropDownList id="ddToAge" runat="server"></asp:DropDownList>
                   		        </td>
							</tr>
                   		    <tr>
                   		        <td class="table_cell">
                                    <%= Lang.TransA("Date Ends") %>:
                   		        </td>
                   		        <td class="table_cell">
                                	<uc2:DatePicker ID="dpDateEnds" runat="server" />   
                   		        </td>
							</tr>
							<tr>
                   		        <td class="table_cell" colspan="2">     
                   		        	<div class="tadd-buttons">
	                                    <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" /><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click"/>                   		        
                                    </div>   
                   		        </td>
                   		    </tr>                  
                        </table>  
                        <div class="separator06"></div>                        
                    </div>
				</div>
			</div>
		</form>
	</body>
</HTML>

