<%@ Page language="c#" Codebehind="ApprovePhoto.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.Admin.ApprovePhoto" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title><%= Lang.TransA("Approve Photo") %></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
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
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_browsephotos.jpg"> 
					<div id="crumpstxt"><%= Lang.TransA("Approve Photo") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
					<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Approve Photo") %></div>
			       		<div class="ch-description"><%= Lang.TransA("Use this section to approve photos...") %></div>
					</div>
				    <table cellpadding="0" cellspacing="0" id="stable">
                    	<tr>
                        	<td class="table_header" colspan="2"><%= Lang.TransA("Photo Approval") %></td>
                        </tr>
                        <tr>
                        	<td colspan="2" valign="middle" align="center" class="table_cell">
                        		<asp:Image CssClass="app-photo" ID="imgBigPhoto" runat="server"></asp:Image></td>
                        </tr>
                        <tr>
                              <td class="table_cell"><%= Lang.TransA("Username") %>:</td>
                              <td class="table_cell"><a id="lnkUsername" runat="server"></a></td>
                            </tr>
                            <tr>
                              <td class="table_cell"><%= Lang.TransA("Photo Name") %>:</td>
                              <td class="table_cell"><asp:Label ID="lblPhotoName" runat="server"></asp:Label>&nbsp;</td>
                            </tr>
                            <tr>
                              <td class="table_cell"><%= Lang.TransA("Photo Description") %>:</td>
                              <td class="table_cell"><asp:Label ID="lblPhotoDescription" runat="server"></asp:Label>&nbsp;</td>
                            </tr>
                            <tr>
                              <td class="table_cell">
                              	<asp:CheckBox ID="chkExplicitPhoto" runat="server"></asp:CheckBox>
                              </td>
                              <td class="table_cell">
                              	<asp:Button ID="btnApprove" runat="server" OnClick="btnApprove_Click"></asp:Button>
                              	<asp:Button ID="btnReject" runat="server" OnClick="btnReject_Click"></asp:Button>
                              	<asp:TextBox ID="txtReason" onkeydown="javascript: KeyDownHandler(btnReject)" runat="server"></asp:TextBox>
                              </td>
                            </tr>
                        </table>
						<div class="separator10"></div>
						<div class="add-buttons2">
							<div class="add-buttons">
				       			<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click"></asp:Button>
							</div>
						</div>
						<div class="separator10"></div>
			      </div>
			</div>
		</form>
	</body>
</HTML>
