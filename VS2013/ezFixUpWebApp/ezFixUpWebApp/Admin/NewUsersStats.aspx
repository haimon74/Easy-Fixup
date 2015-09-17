<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewUsersStats.aspx.cs" Inherits="ezFixUp.Admin.NewUsersStats" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.TransA("New Users Stats") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="form1" runat="server">
        	<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
            	<div id="crupms">
                	<img src="images/i_browsemembers.jpg">
                	<div id="crumpstxt"><%= Lang.TransA("New Users Stats") %></div>
                </div>
                <div id="sidebar">
                	<uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
                </div>
                <div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("New Users Stats")%></div>
                        <div class="ch-description">
                            <%= Lang.TransA("Use this section to view new users statistics of your site...") %>
                        </div>
                    </div>
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
					        <div class="separator10"></div>
                            <table id="tblSearch" cellpadding="0" cellspacing="0" class="filter">
                    	        <tr>
                        	        <td colspan="2" class="table_header">
                            	        <%= Lang.TransA("Filter") %>
                       		        </td>
                   		        </tr>
                                <tr>
                        	        <td class="table_cell">
                            	        <%= Lang.TransA("From") %>:
                                    </td>
                                    <td class="table_cell">
                            	        <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                                    </td>
						        </tr>
                                <tr>
                                    <td class="table_cell">
                            	        <%= Lang.TransA("To") %>:
                                    </td>
                                    <td class="table_cell">
                            	        <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
                                    </td>
						        </tr>
                                <tr>
							        <td class="table_cell">
								        <%= Lang.TransA("Type") %>:
							        </td>
                                    <td class="table_cell">
                            	        <asp:DropDownList ID="ddType" runat="server" OnSelectedIndexChanged="ddType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </td>
						        </tr>
                                    <td colspan="2" class="table_footer" align="center">
                                        <div class="separator10"></div>
                                        <asp:Button ID="btnShowStatistics" Font-Name="Verdana" runat="server" OnClick="btnShowStatistics_Click"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                            <script>
						        if (document.all['<%= tblHideSearch.ClientID %>'])
							        document.all['tblSearch'].style.display = 'none';
                            </script>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                            <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
                            <div id="pnlNewUsersChart" runat="server">
                                <asp:Chart ID="ChartNewRegistrations" runat="server" Width="600" Height="200" Palette="BrightPastel" BackColor="#D3DFF0" BorderDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2" BorderColor="26, 59, 105">
                                    <Titles>
                                        <asp:Title Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Alignment="TopCenter" ForeColor="26, 59, 105"></asp:Title>
                                    </Titles>
                                    <series>
					                    <asp:Series Name="Default" ChartType="Column" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240">
					                    </asp:Series>
				                    </series>
				                    <ChartAreas>
				                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
				                            <area3dstyle Rotation="10" Inclination="30" Perspective="5" PointDepth="200" IsRightAngleAxes="False" Enable3D="true" WallWidth="0" IsClustered="False"/>
				                            <axisy LineColor="64, 64, 64, 64">
										        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
										        <MajorGrid LineColor="64, 64, 64, 64" />
									        </axisy>
									        <axisx LineColor="64, 64, 64, 64">
										        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
										        <MajorGrid LineColor="64, 64, 64, 64" />
									        </axisx>
				                        </asp:ChartArea>
				                    </ChartAreas>
                                </asp:Chart>
                            </div>
                            <asp:DataGrid ID="dgNewUsers" runat="server" Width="100%" CssClass="btable1" AllowPaging="False"
                		        AutoGenerateColumns="False" AllowSorting="True" CellPadding="0" CellSpacing="0" BorderWidth="0" GridLines="None" OnItemCreated="dgNewUsers_ItemCreated">
                		        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                		        <Columns>
                    		        <asp:TemplateColumn>
                        		        <HeaderStyle CssClass="table_header2"></HeaderStyle>
                            	        <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
	                        	        <ItemTemplate>
	                                        <%# Eval("Time")%>
	                                    </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                        <ItemStyle CssClass="table_cell3" Wrap="False" Width="0%"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("NewUsers") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                       </ContentTemplate>
                  </asp:UpdatePanel>    
                  <asp:Button ID="btnGetCSV" runat="server" onclick="btnGetCSV_Click" Visible="false"/>    	        
 	           </div>
		    </div>
	    </form>
	</body>
</html>
