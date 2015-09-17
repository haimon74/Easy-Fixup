<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Page language="c#" Codebehind="ManageUser.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.ManageUser" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD runat="server">
		<title><%= Lang.TransA("Manage User") %></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body >
		<form id="Form1" method="post" runat="server">
			<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
		    <div id="layout">						
		    	<div id="crupms">
					<img src="images/i_manageuser.jpg" width="50" height="50"> 
					<div id="crumpstxt"> <%= Lang.TransA("Manage User") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="Adminmenu1" runat="server"></uc1:AdminMenu>
				</div>
			    <div id="main">
		        	<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Manage User") %></div>
					    <div class="ch-description">
							<%= Lang.TransA("Here you can manage the settings for a particular user") %>
					    </div>
				    </div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<div id="pnlActivateUser" runat="server">
						<table cellSpacing="0" cellPadding="0" class="stable2">
							<tr>
								<td class="table_header" align="center"><%= Lang.TransA("User Activation") %></td>
							</tr>
							<tr>
								<td class="table_cell">
									<div class="separator06"></div>
									<div class="add-buttons">
										<asp:Button ID="btnActivateUser" Runat="server" OnClick="btnActivateUser_Click" />
									</div>
								</td>
							</tr>
						</table>
						<div class="separator10"></div>
					</div>					
					<div id="pnlVerificationsLeft" runat="server">
						<table cellSpacing="0" cellPadding="0" class="stable2">
							<tr>
								<td class="table_header" align="center"><%= Lang.TransA("Message Verifications") %></td>
							</tr>
							<tr>
								<td class="table_cell">
									<%= Lang.TransA("Message verifications left:") %>
									<asp:TextBox ID="txtVerificationsLeft" Runat="server"></asp:TextBox>
								</td>
							</tr>
							<tr>
								<td class="table_cell">
									<div class="separator06"></div>
									<div class="add-buttons">
										<asp:Button ID="btnSave" Runat="server" onclick="btnSave_Click"></asp:Button>
									</div>
								</td>
							</tr>
						</table>
						<div class="separator10"></div>
					</div>
					<table cellSpacing="0" cellPadding="0" border="0" class="stable2">
						<tr>
							<td class="table_header" align="center"><%= Lang.TransA("Delete User") %></td>
						</tr>
						<tr>
							<td class="table_cell" align="center">
						    	<div class="separator06"></div>
								<asp:CheckBox ID="cbAllRelatedData" Runat="server"></asp:CheckBox>
						    	<div class="separator06"></div>
						    	<%= Lang.TransA("Reason") %>:
						    	<asp:TextBox ID="txtDeleteReason" runat="server" />
						    	<div class="separator06"></div>
								<div class="add-buttons">
									<asp:Button ID="btnDelete" Runat="server" onclick="btnDelete_Click"></asp:Button>
								</div>								    
							</td>
						</tr>
					</table>
		            <div id="pnlManualSubscription" runat="server">
		            	<div class="separator10"></div>
						<table cellSpacing="0" cellPadding="0" border="0" class="stable2">
							<tr>
								<td class="table_header" align="center"><%= Lang.TransA("Subscription") %></td>
							</tr>         
							<tr>
								<td class="table_cell" align="center">
							   		<div class="separator06"></div>
		                        	<asp:DropDownList id="ddPlans" runat="server"></asp:DropDownList>
							    	<div class="separator06"></div>
		                        	<asp:Label ID="lblCurrentPlan" Runat="server"></asp:Label>
		                        	<div id="pnlExpirationDate" runat="server">
		                             	<%= Lang.TransA("Expires on:") %> <uc2:DatePicker ID="dpExpirationDate" runat="server" />
		                            </div>
		                            <div id="pnlAffiliateCommission" runat="server">
		                                <asp:CheckBox ID="cbAffiliateCommission" runat="server"/>
		                                <%= Lang.TransA("Do not apply commission") %>
		                            </div>
		                            <div class="add-buttons">
		                            	<asp:Button ID="btnSubscribeUpdate" runat="server" OnClick="btnSubscribeUpdate_Click" />
		                            </div>
								</td>
							</tr>			                       
						</table>  	
		            	<div class="separator10"></div>
		             </div>
		             <div id="pnlCredits" runat="server">
		            	<div class="separator10"></div>
						<table cellSpacing="0" cellPadding="0" border="0" class="stable2">
							<tr>
								<td class="table_header" align="center"><%= Lang.TransA("Credits") %></td>
							</tr>         
							<tr>
								<td class="table_cell" align="center">
							   		<div class="separator06"></div>
	                             	<%= Lang.TransA("Credits:") %>
	                             	<asp:TextBox ID="txtCredits" runat="server"></asp:TextBox>
	                             	<div class="separator06"></div>
		                            <div class="add-buttons">
		                            	<asp:Button ID="btnUpdateCredits" runat="server" 
                                            onclick="btnUpdateCredits_Click"/>
		                            </div>
								</td>
							</tr>			                       
						</table>  	
		            	<div class="separator10"></div>
		             </div>
				</div>
			</div>
		</form>
	</body>
</HTML>
