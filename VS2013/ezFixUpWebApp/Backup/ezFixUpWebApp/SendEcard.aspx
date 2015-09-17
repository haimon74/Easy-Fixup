<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="SendEcard.aspx.cs" Inherits="ezFixUp.SendEcard" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"></uc1:WideBoxStart>
		<div class="srvmsg"><asp:label id="lblError" CssClass="error" runat="server"></asp:label></div>
		<div class="separator"></div>
		<div class="center"><%= Lang.Trans("E-card's Name") %>&nbsp;
        <asp:DropDownList CssClass="dropdownlist" ID="ddEcards" runat="server" onselectedindexchanged="ddEcards_SelectedIndexChanged" AutoPostBack="true">
        </asp:DropDownList></div>
		<div class="separator"></div>
		<div class="center">
			<div id="pnlImage" runat="server"><img class="photoframe" alt='ecard image' src='<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue %>' /></div>
			<div id="pnlFlash" runat="server">
			    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                        id="flvplayer">
                                        <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue + "&seed=" + new Random().NextDouble() %>"/>
                                        <param name="quality" value="high"/>
                                        <param name="bgcolor" value="#FFFFFF"/>
                                        <param name="wmode" value="transparent"/>
                                        <param name="allowfullscreen" value="true"/>
                                        <param name="flashvars" value="file=<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue + "&seed=" + new Random().NextDouble() %>&shuffle=false" />
                                        <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue + "&seed=" + new Random().NextDouble() %>"
                                            quality="high" wmode="transparent" bgcolor="#FFFFFF" 
                                            name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                            pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="file=<%= "EcardContent.ashx?ect=" + ddEcards.SelectedValue + "&seed=" + new Random().NextDouble() %>&shuffle=false"></embed>
				</object>
			</div>
		</div>
		<div class="separator"></div>
		<div id="pnlMessage" runat="server" align="center">
			<%= Lang.Trans("Add a personal message:") %>
            <div class="fckeditor" >
                <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
            </div>
		</div>
		<div class="buttons"><asp:button id="btnSend" tabIndex="1" runat="server" onclick="btnSend_Click"></asp:button></div>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
</asp:Content>