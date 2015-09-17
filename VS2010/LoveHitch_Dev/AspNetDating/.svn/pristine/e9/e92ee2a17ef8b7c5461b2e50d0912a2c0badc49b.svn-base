<%@ Page Language="c#" Codebehind="ImportDummyProfiles.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.ImportDummyProfiles" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
	    <title><%= Lang.TransA("Import Dummy Profiles")%></title>
	    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
	    <meta content="C#" name="CODE_LANGUAGE"/>
	    <meta content="JavaScript" name="vs_defaultClientScript"/>
	    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all"/>
	    <script type="text/javascript" >
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
			        <img src="images/i_browsemembers.jpg" alt="" />
	    	        <div id="crumpstxt"><%= Lang.TransA("Import Dummy Profiles")%></div>
	    	    </div>
		        <div id="sidebar">
		        	<uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
		        </div>
		        <div id="main">
		        	<div id="content-head">
			        	<div class="ch-head"><%= Lang.TransA("Import Dummy Profiles")%></div>
			            <div class="ch-description">
			            	<%= Lang.TransA("Use this section to inport dummy members to your site")+"..." %>
			            </div>
		            </div>
	            	<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
		            <table id="tblSearch" cellpadding="0" cellspacing="0" class="filter">
	                	<tr>
            			    <td class="table_cell"><%= Lang.TransA("Force Country") %></td>
            			    <td class="table_cell">
            				    <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
            			    </td>
            		    </tr>
            		    <tr>
            			    <td class="table_cell"><%= Lang.TransA("Force Region/State")%></td>
            			    <td class="table_cell">
            				    <asp:TextBox ID="txtRegion" runat="server"></asp:TextBox>
            			    </td>
            		    </tr>
            		    <tr>
            			    <td class="table_cell"><%= Lang.TransA("Force City")%></td>
            			    <td class="table_cell">
            				    <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
            			    </td>
            		    </tr>
	                    </table>
	                <div class="separator06"></div>
					<asp:Button ID="btnGenerateMembers" runat="server" onclick="BtnGenerateMembersCsvClick" />
				</div>
        	</div>
   		</form>
	</body>
</html>