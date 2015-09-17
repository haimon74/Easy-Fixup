<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Page language="c#" Codebehind="EditTemplates.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.Admin.EditTemplates" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>

<%--<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "DTD/xhtml1-transitional.dtd">
<html>	
    <head id="Head1" runat="server">
        <title><%= Lang.TransA("Edit Templates") %></title>
        <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1" />
		<meta name="CODE_LANGUAGE" Content="C#"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<%--<meta http-equiv="X-UA-Compatible" content="IE=8" />--%>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all"/>
	</head>
	<body>
	    <form id="Form1" method="post" runat="server">
		    <uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
		    <%--<asp:ScriptManagerProxy ID="ScriptManagerProxy" runat="server" ></asp:ScriptManagerProxy>--%>
		<%--<asp:ScriptManager ID="ScriptManagerMaster" runat="server" LoadScriptsBeforeUI="true"/>--%>
<%--<ajaxToolkit:ToolkitScriptManager ID="ScriptManagerMaster" runat="server" CombineScripts="false" ScriptMode="Auto" EnablePartialRendering="true" LoadScriptsBeforeUI="false" CompositeScript-NotifyScriptLoaded="true" >
</ajaxToolkit:ToolkitScriptManager>--%>
<%--<asp:UpdateProgress ID="UpdateProgressMaster" runat="server" DisplayAfter="0">
    <ProgressTemplate>
        <div style="position:absolute; top: 0; right: 0">
            <asp:Image ID="imgLoadingProgress" ImageUrl="~/Images/loading2.gif" runat="server" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>

			<div id="layout">
				<div id="crupms">
					<img src="images/i_edittemplates.jpg" alt=""/> 
					<div id="crumpstxt"><%= Lang.TransA("Templates") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
			    <div id="main">
				    <div id="content-head">
	                    <div class="ch-head"><%= Lang.TransA("Templates") %></div>
	                    <div class="ch-description"><%= Lang.TransA("Here you can change your email and other templates...") %></div>
					</div>
					<asp:UpdatePanel ID="upTemplate" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
					    <ContentTemplate>
			        <div id="pnlLanguage" runat="server">
				        <table cellSpacing="0" cellPadding="0" class="filter">
					        <tr>
						        <td class="table_header" colSpan="2"><%= Lang.TransA("Languages") %></td>
					        </tr>
					        <tr id="trLanguage" runat="server">
						        <td class="table_cell"><%= Lang.TransA("Language") %>:</td>
						        <td class="table_cell">
						        	<asp:dropdownlist id="ddLanguage" Runat="server" AutoPostBack="True" onselectedindexchanged="ddLanguage_SelectedIndexChanged">
								        <asp:ListItem></asp:ListItem>
							        </asp:dropdownlist>
							    </td>
					        </tr>
					        <tr id="trTemplateName" runat="server">
						        <td class="table_cell"><%= Lang.TransA("Select Template:") %></td>
						        <td class="table_cell"><asp:dropdownlist id="ddTemplateName" AutoPostBack="True" 
                                        Runat="server" onselectedindexchanged="ddTemplateName_SelectedIndexChanged"></asp:dropdownlist></td>
					        </tr>
				        </table>
			        </div>
			        <table cellpadding="0" cellspacing="0" width="100%">
			        	<tr>
			        		<td>
			        		
			        		
			        <div class="servermsg"><uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox></div>
                    <div class="fckeditor">
                    <asp:PlaceHolder ID="phTemplate" runat="server"></asp:PlaceHolder>
                    </div>
      				<asp:PlaceHolder id="phTemplates" runat="server"></asp:PlaceHolder>
				    <div class="tadd-buttons">
				     	<asp:Button id="btnSave" runat="server" Visible="false" onclick="btnSave_Click"></asp:Button>
			        </div>		
			        		
			        		
			        		
			        		</td>
			        	</tr>
			        </table>
			        
			            </ContentTemplate>
			        </asp:UpdatePanel>
			    </div>
		    </div>
		</form>
	</body>
</html>
