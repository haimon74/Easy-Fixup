<%@ Page Language="c#" Codebehind="ApprovePhotos2.aspx.cs" AutoEventWireup="True"
    Inherits="AspNetDating.Admin.ApprovePhotos2" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
	    <title><%= Lang.TransA("Approve Photos") %></title>
	    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
	    <meta name="CODE_LANGUAGE" content="C#">
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
	</head>
	<body>
    	<form id="Form1" method="post" runat="server">
	        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
	        <div id="layout">
	            <div id="crupms">
	                <img src="images/i_browsephotos.jpg">
	                <div id="crumpstxt">
	                    <%= Lang.TransA("Approve Photos") %>
	                </div>
	            </div>
	            <div id="sidebar">
	                <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
	            </div>
	            <div id="main">
	                <div id="content-head">
	                    <div class="ch-head">
	                        <%= Lang.TransA("Approve Photos") %>
	                    </div>
	                    <div class="ch-description">
	                        <%= Lang.TransA("Use this section to approve or reject pending photos...") %>
	                    </div>
	                </div>
	                <asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
	                    <ContentTemplate>
	                        <uc1:MessageBox ID="MessageBox" runat="server"></uc1:MessageBox>
                           <ajaxToolkit:TextBoxWatermarkExtender ID="TBWEReason" runat="server"
                                TargetControlID="txtReason"
                                WatermarkCssClass="watermarked" />		                    
	                        <table id="stable" runat="server" border="0" cellpadding="0" cellspacing="0">
	                            <tr>
	                                <td align="center" class="table_header" colspan="2">
	                                    <%= Lang.TransA("Photo Approval") %></td>
	                            </tr>
	                            <tr>
	                                <td class="table_cell">
	                                    <%= Lang.TransA("Username") %>
	                                    :</td>
	                                <td class="table_cell">
	                                    <asp:Label Font-Name="Verdana" Font-Size="11px" ID="lblUsername" runat="server"></asp:Label>
	                                    &nbsp;</td>
	                            </tr>
	                            <tr>
	                                <td class="table_cell">
	                                    <%= Lang.TransA("Photo Name") %>
	                                    :</td>
	                                <td class="table_cell">
	                                    <asp:Label Font-Name="Verdana" Font-Size="11px" ID="lblPhotoName" runat="server"></asp:Label>
	                                    &nbsp;</td>
	                            </tr>
	                            <tr>
	                                <td class="table_cell">
	                                    <%= Lang.TransA("Photo Description") %>
	                                    :</td>
	                                <td class="table_cell">
	                                    <asp:Label Font-Name="Verdana" Font-Size="11px" ID="lblPhotoDescription" runat="server"></asp:Label>
	                                    &nbsp;</td>
	                            </tr>
	                            <tr id="trExplicitPhoto" runat="server">
	                                <td colspan="2" class="table_cell">
	                                    <asp:CheckBox ID="chkExplicitPhoto" runat="server"></asp:CheckBox>
	                                </td>
	                            </tr>
	                            <tr>
	                                <td class="table_cell">
	                                    <asp:Button ID="btnApprove" runat="server" OnClick="btnApprove_Click"></asp:Button>
	                                </td>
	                                <td class="table_cell">
	                                    <asp:Button ID="btnReject" runat="server" OnClick="btnReject_Click"></asp:Button>
	                                    <asp:TextBox ID="txtReason" onkeydown="javascript: KeyDownHandler(btnReject)" runat="server"></asp:TextBox>
	                                    <asp:CheckBox ID="cbDeleteAccount" runat="server" />
	                                </td>
	                            </tr>
	                            <tr>
	                                <td class="table_cell" valign="middle" align="center" colspan="2">
	                                    <asp:Image ID="imgBigPhoto" CssClass="app-photo" runat="server"></asp:Image></td>
	                            </tr>
	                        </table>
	                    </ContentTemplate>
	                </asp:UpdatePanel>
                	<div class="separator10"></div>
	                <table cellpadding="0" cellspacing="0" class="approvephoto2">
	                    <!--
								<tr>
									<td align="right">
										<asp:Panel ID="pnlPhotosPerPage" runat="server">
										<asp:Label id="lblPhotosPerPage" Font-Name="Arial" Font-Size="11px" runat="server"></asp:Label>:
										<asp:DropDownList id="dropPhotosPerPage" Font-Name="Arial" Font-Size="9px" runat="server" AutoPostBack="True"></asp:DropDownList>
									</asp:Panel>
									</td>
								</tr>
								-->
	                    <tr>
	                        <td>
	                            <asp:UpdatePanel ID="UpdatePanelPhoto" runat="server">
	                                <ContentTemplate>
	                                    <asp:DataList CssClass="approvephoto2-wrap" ID="listPendingApproval" runat="server"
	                                        RepeatLayout="Table" Width="100%" RepeatDirection="Horizontal" RepeatColumns="10">
	                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle"></ItemStyle>
	                                        <ItemTemplate>
	                                            <asp:LinkButton ID="lnkApprovePhoto" CommandArgument='<%# Eval("PhotoID")%>'
	                                                runat="server">
											<img border=0 src="<%= Config.Urls.Home%>/Image.ashx?id=<%# Eval("PhotoID")%>&width=100&height=100&cache=1"></asp:LinkButton>
	                                        </ItemTemplate>
	                                    </asp:DataList>
	                                    <div class="separator10">
	                                    </div>
	                                    <div class="add-buttons">
	                                        <asp:Button ID="btnApproveAll" runat="server" OnClick="btnApproveAll_Click" />
	                                    </div>
	                                    <div class="separator10">
	                                    </div>
	                                </ContentTemplate>
	                            </asp:UpdatePanel>
	                        </td>
	                    </tr>
	                </table>
	            </div>
	        </div>
	    </form>
	</body>
</html>
