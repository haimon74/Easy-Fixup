<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditBanners.aspx.cs" Inherits="AspNetDating.Admin.EditBanners" %>

<%@ Import Namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title><%= Lang.TransA("Edit Banners") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
		    <asp:UpdatePanel ID="UpdatePanelEditBanners" runat="server">
				<ContentTemplate>
		        <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCountry" runat="server" TargetControlID="ddCountry"
                    Category="Country" ServicePath="~/Services/Service.asmx" PromptText=" "
                    ServiceMethod="GetCountries" />
                <ajaxToolkit:CascadingDropDown ID="CascadingDropDownState" runat="server" TargetControlID="ddRegion"
                    Category="Region" ServicePath="~/Services/Service.asmx" PromptText=" "
                    ServiceMethod="GetRegionsByCountry" ParentControlID="ddCountry" />
                <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity" runat="server" TargetControlID="ddCity"
                    Category="City" ServicePath="~/Services/Service.asmx" PromptText=" "
                    ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="ddRegion" />
			    <uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			    <div id="layout">
				    <div id="crupms">
	                    <img src="images/i_editpages.jpg" /> 
					    <div id="crumpstxt"><%= Lang.TransA("Edit Banners")%></div>
			        </div>
				    <div id="sidebar">
		                <uc1:adminmenu id="Adminmenu1" runat="server"></uc1:adminmenu>
				    </div>
			        <div id="main">
			    	    <div id="content-head">
					        <div class="ch-head"><%= Lang.TransA("Edit Banners")%></div>
		                    <div class="ch-description"><%= Lang.TransA("Use this section to edit banners")%></div>	
					    </div>
			            <uc1:messagebox id="Messagebox1" runat="server"></uc1:messagebox>	
	                        <table id="stable" border="0" cellpadding="0" cellspacing="0">
	                            <tr>
	                                <td align="center" class="table_header" colspan="2">
	                                    <%= Lang.TransA("Edit Banner") %></td>
	                            </tr>
	                            <tr>
	                                <td class="table_cell">
	                                     <%= "Position".TranslateA() %>:</td>
	                                <td class="table_cell">
	                                    <asp:DropDownList ID="ddPosition" runat="server" AutoPostBack="true" onselectedindexchanged="ddPosition_SelectedIndexChanged"></asp:DropDownList></td>
	                            </tr>
	                            <tr id="pnlBannerCode" runat="server" visible="false">
	                                <td class="table_cell" colspan="2" align="center">
                                    
                                   

			            <div >
			                <asp:Label ID="lblDescription" runat="server"></asp:Label>
			                <br />
			                <img id="imgBannerCode" runat="server" />
                            			            <div class="separator10"></div>
			                <%= "Target".TranslateA() %>:
			                <asp:DropDownList ID="ddTarget" runat="server" AutoPostBack="true" onselectedindexchanged="ddTarget_SelectedIndexChanged"></asp:DropDownList>
			                			            <div class="separator10"></div>
			                <div id="pnlTarget" runat="server" visible="false">
                            <table cellpadding="0" cellspacing="0" width="100%">
                            	<tr>
                                	<td width="30%" class="table_cell" ><%= "Visible for".TranslateA() %></td>
                                	<td class="table_cell" ><asp:DropDownList ID="ddVisibleFor" runat="server"></asp:DropDownList></td>
                                </tr>
                            	<tr>
                                	<td class="table_cell" ><%= "Name".TranslateA() %>:</td>
                                	<td class="table_cell" ><asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                                </tr>
                            	<tr>
                                	<td class="table_cell" ><%= "Country".TranslateA() %></td>
                                	<td class="table_cell" ><asp:DropDownList ID="ddCountry" runat="server"></asp:DropDownList></td>
                                </tr>
                            	<tr>
                                	<td class="table_cell" ><%= "Region".TranslateA() %></td>
                                	<td class="table_cell" ><asp:DropDownList ID="ddRegion" runat="server"></asp:DropDownList></td>
                                </tr>
                            	<tr>
                                	<td class="table_cell" ><%= "City".TranslateA() %></td>
                                	<td class="table_cell" ><asp:DropDownList ID="ddCity" runat="server"></asp:DropDownList></td>
                                </tr>
                            	<tr>
                                	<td class="table_cell" ><%= "Gender".TranslateA() %></td>
                                	<td class="table_cell" ><asp:DropDownList ID="ddGender" runat="server"></asp:DropDownList></td>
                                </tr>
                            	<tr>
                                	<td class="table_cell" ><%= "Paid".TranslateA() %></td>
                                	<td class="table_cell" ><asp:DropDownList ID="ddPaid" runat="server"></asp:DropDownList></td>
                                </tr>
                            	<tr>
                                	<td class="table_cell" ><%= "Age range" %></td>
                                	<td class="table_cell" ><asp:DropDownList ID="ddFromAge" runat="server"></asp:DropDownList>
                                    <%= "to".TranslateA() %><asp:DropDownList ID="ddToAge" runat="server"></asp:DropDownList>
                                    </td>
                                </tr>
                            	<tr>
                                	<td class="table_cell" ><%= "Priority".TranslateA() %></td>
                                	<td class="table_cell" ><asp:DropDownList ID="ddPriority" runat="server"></asp:DropDownList></td>
                                </tr>
                            </table>
			                </div>
			            <div class="separator10"></div>
			                <%= Lang.TransA("Paste advertisment code below.")%>
			                <br />
			                <asp:TextBox ID="txtBannerCode" runat="server" TextMode="MultiLine" Rows="10" Columns="50"></asp:TextBox>
			                <br />

			           
			                <div align="center">
			                    <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" />
			                    <asp:Button ID="btnDelete" runat="server" Visible="false" onclick="btnDelete_Click"/>
			                </div>
                                    </td>
	                            </tr>
                        </table>
			            <div class="separator10"></div>
			            </div>
			            <div class="separator10"></div>
				    </div>
			    </div>
			</ContentTemplate>
			</asp:UpdatePanel>
		</form>
	</body>
</HTML>
