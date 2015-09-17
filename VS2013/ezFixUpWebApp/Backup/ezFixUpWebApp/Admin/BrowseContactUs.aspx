<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrowseContactUs.aspx.cs" Inherits="ezFixUp.Admin.BrowseContactUs" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<HEAD  runat="server">
	<title> <%= Lang.TransA("Browse Contact Us") %></title>
	<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
	<meta name="CODE_LANGUAGE" Content="C#">
	<meta name="vs_defaultClientScript" content="JavaScript">
	<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
	<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
</HEAD>
<body>
    <form id="form1" runat="server">
    <div>
		<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
		<div id="layout">
			<div id="crupms">
				<img src="images/i_abuse.jpg" /> 
				<div id="crumpstxt"> <%= Lang.TransA("Contact Us") %></div>
			</div>
			<div id="sidebar">
				<uc1:AdminMenu id="Adminmenu1" runat="server"></uc1:AdminMenu>
			</div>
		    <div id="main">
				<div id="content-head">
					<div class="ch-head"><%= Lang.TransA("Contact Us")%></div>
					<div class="ch-description"><%= Lang.TransA("Use this section to view contact us messages...")%></div>
				</div>
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
	            <asp:Panel id="pnlSearchBox" DefaultButton="btnSearch" runat="server">
                    <table id="tblSearch" cellpadding="0" cellspacing="0" class="filter" style="width: 400px;">
                	    <tr>
                    	    <td colspan="2" class="table_header">
                        	    <%= Lang.TransA("Filter") %>
                    	    </td>
                        </tr>
                        <tr>
	                         <td class="table_cell">
	                     	    <%= Lang.TransA("Reported By") %>:
	                         </td>
                             <td class="table_cell">
                         	    <asp:TextBox ID="txtReportedBy" runat="server"></asp:TextBox>
                             </td>
                        </tr>
                        <tr>
                     	    <td class="table_cell">
                        	    <%= Lang.TransA("Reported User") %>:
                            </td>
                            <td class="table_cell">
                        	    <asp:TextBox ID="txtReportedUser" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                     	    <td class="table_cell">
                        	    <%= Lang.TransA("ReportType") %>:
                            </td>
                            <td class="table_cell">
                        	    <asp:DropDownList ID="ddReportType" runat="server">
                        		    <asp:ListItem Value=""></asp:ListItem>
                        	    </asp:DropDownList>
                    	    </td>
					    </tr>
                        <tr>
                            <td class="table_cell" style="height: 30px">
                                <%= Lang.TransA("From Date") %>
                                :</td>
                            <td class="table_cell" style="height: 30px">
                                <uc2:DatePicker ID="dpFromDate" runat="server" />
                                </td>
                        </tr>
                        <tr>
                            <td class="table_cell" style="height: 30px">
                                <%= Lang.TransA("To Date") %>
                                :</td>
                            <td class="table_cell" style="height: 30px">
                                <uc2:DatePicker ID="dpToDate" runat="server" />
                                </td>
                        </tr>
					    <tr>
	                        <td class="table_cell">
                        	    <%= Lang.TransA("Reviewed") %>:
                            </td>
                            <td class="table_cell">
                        	    <asp:DropDownList ID="ddReviewed" runat="server">
                            	    <asp:ListItem Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
					    </tr>
                        <tr>
                    	    <td colspan="2" class="table_footer" align="center">
                        	    <div class="separator06"></div>
                                <asp:Button ID="btnSearch" Font-Name="Verdana" runat="server" OnClick="btnSearch_Click"></asp:Button>
                            </td>
					    </tr>
				    </table>
                </asp:Panel>					
                <script type="text/javascript">
					if (document.all['<%= tblHideSearch.ClientID %>'])
					document.all['tblSearch'].style.display = 'none';
                </script>
				<div class="separator06"></div>
				<asp:UpdatePanel ID="UpdatePanel1" runat="server">
				    <ContentTemplate>
				    <uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
                    <asp:DataGrid Width="100%" ID="dgAbuseReports" runat="server"  AutoGenerateColumns="False"
                        AllowSorting="True" CssClass="btable1" CellPadding="0" AllowPaging="false" CellSpacing="0" BorderWidth="0" OnSortCommand="dgAbuseReports_SortCommand" OnItemCommand="dgAbuseReports_ItemCommand" OnItemCreated="dgAbuseReports_ItemCreated">
                        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                        <Columns>
                            <asp:TemplateColumn SortExpression="DateReported">
                                <HeaderStyle CssClass="table_header2" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                        <asp:HiddenField ID="hidReportId" Value='<%# Eval("ReportId") %>' runat="server" />
                                        <%# Eval("DateReported")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="ReportedBy">
                                <HeaderStyle CssClass="table_header2" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <a class="gridlink" target="_blank" href='<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("ReportedBy") %>'>
                                        <%# Eval("ReportedBy")%></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                             <asp:TemplateColumn SortExpression="ReportedUser">
                                <HeaderStyle CssClass="table_header2" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <a class="gridlink" target="_blank" href='<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("ReportedUser") %>'>
                                        <%# Eval("ReportedUser")%></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="True" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Report") %>
                                    <div class="separator06"></div>
                                    <a id="A1" Visible='<%# ((AbuseReport.ReportType)Eval("ReportType")) == AbuseReport.ReportType.Photo %>' HRef='<%# "~/Image.ashx?id=" + Eval("ReportedItemId") %>' runat="server">
                                        <img border=0 src='<%# Config.Urls.Home + "/Image.ashx?id=" + Eval("ReportedItemId") + "&width=100&height=100&cache=1"%>' />
                                    </a>
                                    <div Visible='<%# ((AbuseReport.ReportType)Eval("ReportType")) == AbuseReport.ReportType.Message %>' runat="server">
                                        <%# Lang.TransA("Message")+ ": " + Eval("ReportedMessage") %>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateColumn>                            
                            <asp:TemplateColumn SortExpression="ReportedType">
                                <HeaderStyle CssClass="table_header2" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="True" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <%# ((AbuseReport.ReportType)Eval("ReportType")).ToString()%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2" HorizontalAlign="Center" Font-Bold="True"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="True" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                <ul>
                                    <li>
                                        <asp:LinkButton id="lnkMarkReviewed" CommandName="MarkReviewed" runat="server"><%# Lang.TransA("Mark as Reviewed") %></asp:LinkButton>
                                    </li>
                                    <li runat="server" Visible='false' >
                                        <asp:LinkButton id="lnkDeleteReportedMessage" CommandName="DeleteMessage" CommandArgument='<%# Eval("ReportedItemId") %>' runat="server"><%# Lang.TransA("Delete Message") %></asp:LinkButton>
                                    </li>
                                    <li runat="server" Visible='false'>
                                        <asp:LinkButton id="lnkDeleteReportedPhoto" CommandName="DeletePhoto" CommandArgument='<%# Eval("ReportedItemId") %>' runat="server"><%# Lang.TransA("Delete Photo") %></asp:LinkButton>                                    
                                    </li>
                                    <li visible="false">
                                        <a onclick="javascript: document.getElementById('lnkDeleteUser_<%# Eval("RowIndex") %>_panel').style.display = 'block'; return false;" href="#"><%# Lang.TransA("Delete User") %></a>
                                        <div id='lnkDeleteUser_<%# Eval("RowIndex") %>_panel' style="display: none">
                                            <%= Lang.TransA("Reason") %>:
                                            <asp:TextBox id="txtReason1" runat="server"></asp:TextBox>
                                            <asp:Button ID="btnDeleteUser" CommandName="DeleteUser" CommandArgument='<%# Eval("ReportedUser") %>' Text='<%# Lang.TransA("Delete") %>' runat="server" />
                                            <input type="button" value='<%# Lang.TransA("Cancel") %>' onclick="javascript: document.getElementById('lnkDeleteUser_<%# Eval("RowIndex") %>_panel').style.display = 'none';" />                              
                                        </div>                                    
                                    </li>
                                    <li runat="server" Visible='false'>
                                        <a onclick="javascript: document.getElementById('lnkDeleteUserAndTheirMessages_<%# Eval("RowIndex") %>_panel').style.display = 'block';return false;" href="#"><%# Lang.TransA("Delete User and Their Messages") %></a>
                                        <div id='lnkDeleteUserAndTheirMessages_<%# Eval("RowIndex") %>_panel' style="display: none">
                                            <%= Lang.TransA("Reason") %>:
                                            <asp:TextBox id="txtReason2" runat="server"></asp:TextBox>
                                            <asp:Button ID="btnDeleteUserAndTheirMessages" CommandName="DeleteUserAndTheirMessages" CommandArgument='<%# Eval("ReportedUser") %>' Text='<%# Lang.TransA("Delete") %>' runat="server" />
                                            <input type="button" value='<%# Lang.TransA("Cancel") %>' onclick="javascript: document.getElementById('lnkDeleteUserAndTheirMessages_<%# Eval("RowIndex") %>_panel').style.display = 'none';" />                               
                                        </div>                                     
                                    </li>
                                </ul>
                                </ItemTemplate>
                            </asp:TemplateColumn>                                                                                                                                              
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>				
                    <asp:Panel ID="pnlPaginator" Visible="false" runat="server">
                	    <div class="line"></div>
                        <table cellspacing="0" cellpadding="0" width="100%" class="pagenumber">
                    	    <tr>
                        	    <td>
                            	    <asp:LinkButton ID="lnkFirst" CssClass="pagelink" runat="server"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkPrev" CssClass="pagelink" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
                                    <asp:Label ID="lblPager" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkNext" CssClass="pagelink" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkLast" CssClass="pagelink" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                   </asp:Panel> 
                </ContentTemplate>
               </asp:UpdatePanel>				
			</div>
		</div>    
    </div>
    </form>
</body>
</html>
