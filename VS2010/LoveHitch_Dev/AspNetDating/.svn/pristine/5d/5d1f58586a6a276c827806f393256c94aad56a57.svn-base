<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContestEntries.aspx.cs" Inherits="AspNetDating.Admin.ContestEntries" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title> <%= Lang.TransA("Contest Entries") %></title>
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
					<img src="images/i_contestentries.jpg" /> 
					<div id="crumpstxt"> <%= Lang.TransA("Contest Entries") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="Adminmenu1" runat="server"></uc1:AdminMenu>
				</div>
			    <div id="main">
					<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Contest Entries") %></div>
						<div class="ch-description"><%= Lang.TransA("") %></div>
					</div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
            		<table id="tblHideSearch" cellpadding="0" cellspacing="0" class="filter" runat="server" visible="false">
            			<tr>
            		    	<td class="table_header">
            		    		<%= Lang.TransA("Filter") %>
	                    </td>
	                    <td class="table_header">
	                    	[&nbsp;<a onclick="document.all['tblSearch'].style.display = 'block'; document.all['tblHideSearch'].style.display = 'none';"
	                        href="javascript: void(0);" style="color: Black;"><%= Lang.TransA("Expand filter") %></a>&nbsp;]&nbsp;
	                    </td>
	                 </tr>
	                </table>
	                <asp:Panel ID="pnlFilter" DefaultButton="btnSearch" runat="server">
                        <table id="tblSearch" cellpadding="0" cellspacing="0" class="filter">
	             	    <tr>
	               		    <td colspan="2" class="table_header">
	                    	    <%= Lang.TransA("Filter") %></td>
	             	    </tr>
	             	    <tr>
	                	    <td class="table_cell">
	                    	    <%= Lang.TransA("Contest Name") %>:
	                        </td>
	                        <td class="table_cell">
	                    	    <asp:DropDownList ID="ddContests" runat="server"></asp:DropDownList></td>
	                    </tr>
	             	    <tr>
	                	    <td class="table_cell">
	                    	    <%= Lang.TransA("Username") %>:
	                        </td>
	                        <td class="table_cell">
	                    	    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox></td>
	                    </tr>	                
	                    <tr>
	                	    <td colspan="2" class="table_footer" align="center">
	                    	    <div class="separator06"></div>
	                    	    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click"></asp:Button>
	                        </td>
	                    </tr>
	                    </table>
	                </asp:Panel>
	                <script>
				        if (document.all['<%= tblHideSearch.ClientID %>'])
				        document.all['tblSearch'].style.display = 'none';
	                </script>
	                <div class="separator06"></div>
	                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
    	            <table align="center" width="100%" cellpadding="0" cellspacing="0">
            	        <tr>
                	        <td align="right" class="perpage">
                    	        <asp:Label ID="lblPhotosPerPage" Font-Name="Verdana" Font-Size="11px" runat="server"></asp:Label>:
                    	        <asp:DropDownList ID="dropPhotosPerPage" CssClass="pages" Font-Name="Verdana" runat="server" AutoPostBack="True" OnSelectedIndexChanged="dropPhotosPerPage_SelectedIndexChanged">
                    	        </asp:DropDownList>
                    	        <div class="separator06"></div>
                	        </td>
            	        </tr>
                    </table>
                    <div class="separator06"></div>
                    <asp:DataGrid Width="100%" ID="dgContestEntries" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                    AllowSorting="True" CssClass="btable1" CellPadding="0" CellSpacing="0" BorderWidth="0" GridLines="None" OnItemDataBound="dgContestEntries_ItemDataBound" OnSortCommand="dgContestEntries_SortCommand" OnItemCommand="dgContestEntries_ItemCommand">
            	        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                	        <Columns>
                    	        <asp:TemplateColumn SortExpression="Username">
                        	        <HeaderStyle CssClass="table_header2" HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                            	        <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("Username") %>">
                            	        <%# Eval("Username")%></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
					            <asp:TemplateColumn>
                    	            <HeaderStyle CssClass="table_header2c"></HeaderStyle>
                    	            <ItemStyle CssClass="table_cell3c" Wrap="False" VerticalAlign="Middle"></ItemStyle>
                    	            <ItemTemplate>
							            <a href='<%# String.Format("../Image.ashx?id={0}", Eval("PhotoId")) %>' target="_blank">
								            <img src='<%# String.Format("../Image.ashx?id={0}&width=150&height=80", Eval("PhotoId")) %>' style="border: 0" />
							            </a>
						            </ItemTemplate>
					            </asp:TemplateColumn>
                                <asp:TemplateColumn SortExpression="ContestName">
            	                    <HeaderStyle CssClass="table_header2c"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3c" Wrap="False"></ItemStyle>
            	                    <ItemTemplate>
                	                    <%# Eval("ContestName")%>
				                    </ItemTemplate>
			                    </asp:TemplateColumn>					            
                                <asp:TemplateColumn>
            	                    <HeaderStyle CssClass="table_header2c"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3c" Wrap="False"></ItemStyle>
            	                    <ItemTemplate>
                	                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Id") %>'>
						                    <%# Lang.TransA("Delete") %></asp:LinkButton>
				                    </ItemTemplate>
			                    </asp:TemplateColumn>
		                    </Columns>
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
	                </asp:DataGrid>
					<asp:Panel ID="pnlPaginator" runat="server">
						<table cellspacing="0" cellpadding="0" width="100%"  class="pagenumber">
							<tr>
                                <td>
                                    <asp:LinkButton ID="lnkFirst" runat="server" CssClass="pagelink" OnClick="lnkFirst_Click"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkPrev" runat="server" CssClass="pagelink" OnClick="lnkPrev_Click"></asp:LinkButton>
                                    <asp:Label ID="lblPager" runat="server"></asp:Label>
                                    <asp:LinkButton ID="lnkNext" runat="server" CssClass="pagelink" OnClick="lnkNext_Click"></asp:LinkButton>
                    	            <asp:LinkButton ID="lnkLast" runat="server" CssClass="pagelink" OnClick="lnkLast_Click"></asp:LinkButton>
                    	        </td>
                    	    </tr>
                    	</table>
                    </asp:Panel>
                   </ContentTemplate>
                  </asp:UpdatePanel>
				</div>
			</div>
		</form>
	</body>
</HTML>
