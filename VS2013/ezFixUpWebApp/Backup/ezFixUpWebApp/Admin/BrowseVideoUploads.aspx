<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrowseVideoUploads.aspx.cs" Inherits="ezFixUp.Admin.BrowseVideoUploads" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
	    <title><%= Lang.TransA("Browse Video Uploads") %></title>
    	<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
    	<form id="Form1" method="post" runat="server" defaultbutton="btnSearch">
       		<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
        	<div id="layout">
	    		<div id="crupms">
            		<img src="images/i_browsemessage.jpg">
                	<div id="crumpstxt">
                		<%= Lang.TransA("Browse Video Uploads")%>
                	</div>
            	</div>
            	<div id="sidebar">
            		<uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
            	</div>
            	<div id="main">
	        		<div id="content-head">
		        		<div class="ch-head"><%= Lang.TransA("Video Uploads Management")%></div>
                    	<div class="ch-description">
                    	    <%= Lang.TransA("Use this section to browse video uploads...") %>
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
                            	<%= Lang.TransA("Username") %>:
                            </td>
                            <td class="table_cell">
                            	<asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                            </td>
						</tr>
                        <tr>
	                        <td class="table_cell">
                            	<%= Lang.TransA("Approved") %>:
                            </td>
                        	<td class="table_cell">
                            	<asp:DropDownList ID="ddApproved" runat="server">
                            	    <asp:ListItem></asp:ListItem>
                            	</asp:DropDownList></td>
                  		</tr>
                  		<tr>
	                        <td class="table_cell">
                            	<%= Lang.TransA("Private") %>:
                            </td>
                        	<td class="table_cell">
                            	<asp:DropDownList ID="ddIsPrivate" runat="server">
                            	    <asp:ListItem></asp:ListItem>
                            	</asp:DropDownList></td>
                  		</tr>
                        <tr>
                        	<td colspan="2" class="table_footer">
                        		<div class="separator06"></div>
                        		<div class="add-buttons">
                      				<asp:Button ID="btnSearch" Font-Name="Verdana" runat="server" 
                                        onclick="btnSearch_Click"></asp:Button>
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
                            	<asp:DropDownList ID="ddVideoUploadsPerPage" CssClass="pages" 
                                    Font-Name="Verdana" runat="server" AutoPostBack="True" 
                                    onselectedindexchanged="ddVideoUploadsPerPage_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
					</table>
                    <div class="separator06"></div>
                    <asp:DataGrid Width="100%" ID="dgVideoUploads" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                    AllowSorting="True" CssClass="btable1" CellPadding="0" CellSpacing="0" 
                            BorderWidth="0" GridLines="None" onitemcommand="dgVideoUploads_ItemCommand" 
                            onsortcommand="dgVideoUploads_SortCommand" 
                            onitemcreated="dgVideoUploads_ItemCreated">
	                    <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
	                        <Columns>
                                <asp:TemplateColumn SortExpression="2">
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                                        <a target="_blank" href="<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("Username") %>">
                                            <%# Eval("Username")%></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="True"></ItemStyle>
                                    <ItemTemplate>
                                    <div id="pnlVideoUpload" runat="server" visible="false">
                                        <div class="center">
                                            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                                width="325" height="262">
                                                <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf">
                                                <param name="quality" value="high">
                                                <param name="bgcolor" value="#FFFFFF">
                                                <param name="wmode" value="transparent">
                                                <param name="allowfullscreen" value="true">
                                                <param name="flashvars" value="width=325&height=262&file=<%# Eval("VideoUrl") %>&image=<%# ((string) Eval("VideoUrl")).Replace(".flv", ".png") %>" />
                                                <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"
                                                    quality="high" wmode="transparent" bgcolor="#FFFFFF" width="325" height="262"
                                                    name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                                    pluginspage="https://www.macromedia.com/go/getflashplayer" flashvars="width=325&height=262&file=<%# Eval("VideoUrl")%>&image=<%# ((string) Eval("VideoUrl")).Replace(".flv", ".png") %>"></embed></object>
                                        </div>       
                                    </div>
                                        <asp:ImageButton ID="imgbtnViewVideo" runat="server" ImageUrl='<%# Eval("ThumbnailUrl") %>' CommandName="ViewVideo" CommandArgument='<%# Eval("ID") + "|" + Eval("VideoUrl")%>' class="photoframe" style="behavior: none" /> 
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                                        <%# Boolean.Parse((string)Eval("Approved")) ? Lang.TransA("Yes") : Lang.TransA("No") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                                        <%# Boolean.Parse((string)Eval("IsPrivate")) ? Lang.TransA("Yes") : Lang.TransA("No") %>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete"
                                            CommandArgument='<%# Eval("ID") %>'>
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
                       	        	<asp:LinkButton ID="lnkFirst" runat="server" CssClass="pagelink" 
                                        onclick="lnkFirst_Click"></asp:LinkButton>
                       	        	<asp:LinkButton ID="lnkPrev" runat="server" CssClass="pagelink" 
                                        onclick="lnkPrev_Click"></asp:LinkButton>
                       	        	<asp:Label ID="lblPager" runat="server"></asp:Label>
                       	        	<asp:LinkButton ID="lnkNext" runat="server" CssClass="pagelink" 
                                        onclick="lnkNext_Click"></asp:LinkButton>
                       	        	<asp:LinkButton ID="lnkLast" runat="server" CssClass="pagelink" 
                                        onclick="lnkLast_Click"></asp:LinkButton>
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
</html>
