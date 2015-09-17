<%@ Control Language="c#" AutoEventWireup="True" Codebehind="AdminHeader.ascx.cs" Inherits="ezFixUp.Admin.AdminHeader" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Controls" TagPrefix="cc1" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<script type="text/javascript">
<!--
    //Sys.Services._AuthenticationService.DefaultWebServicePath = 'Authentication_JSON_AppService.axd';
    //Sys.Services.AuthenticationService.set_path('/Authentication_JSON_AppService.axd');
// -->
</script>

<ajaxToolkit:ToolkitScriptManager ID="ScriptManagerMaster" runat="server" 
    CombineScripts="true" EnablePageMethods="true" ScriptMode="auto" 
    EnablePartialRendering="true" EnableScriptLocalization="false" 
    LoadScriptsBeforeUI="false" >
</ajaxToolkit:ToolkitScriptManager>
<%--<asp:ScriptManager ID="ScriptManagerMaster" runat="server"  >
</asp:ScriptManager>
<asp:UpdateProgress ID="UpdateProgressMaster" runat="server" DisplayAfter="0">
    <ProgressTemplate>
        <div style="position:absolute; top: 0; right: 0">
            <asp:Image ID="imgLoadingProgress" ImageUrl="~/Images/loading2.gif" runat="server" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<div id="headerwrap">
	<div id="logo">
		<div id="headerlogin">
			<asp:Label id="lblWelcome" runat="server"></asp:Label>
			<a href="https://support.ezFixUp.com" target="_blank" class="exitlink">Support</a>
			<asp:LinkButton id="lnkLogout" runat="server" CssClass="exitlink"></asp:LinkButton>
		</div>
	</div>
	<div id="glance">
		<asp:Panel ID="pnlLogout" Runat="server">
			<table cellspacing="0" cellpadding="0" class="header-table">
				<tr>
					<td align="left" class="header-links">
					<b><%= Lang.TransA("At a glance")%></b>:
					<b><%=NewUsersForTheLast24hours%></b>&nbsp;<%= Lang.TransA("new members for the last twenty-four hours") %>;
					<b><%=PendingPhotos%></b>&nbsp;<%= Lang.TransA("photos pending") %>;&nbsp;<b><%= PendingAnswers%></b>
					<%= Lang.TransA("answers pending") %></td>
				</tr>
			</table>
		</asp:Panel>
	</div>
</div>
