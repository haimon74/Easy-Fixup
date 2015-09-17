<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="UserEventsWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.UserEventsWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<asp:MultiView ID="mvUserEvents" runat="server" ActiveViewIndex="0">
    <asp:View ID="viewEvents" runat="server">
    	    <asp:PlaceHolder ID="plhEvents" runat="server" />
        	<asp:Literal ID="ltrEvents" runat="server"></asp:Literal>
    </asp:View>
    <asp:View ID="viewNoEvents" runat="server">
        <div class="center">
		<div class="separator"></div>
		    <%= Lang.Trans("There are no user events!") %><div class="separator"></div>
	    </div>
    </asp:View>
</asp:MultiView>
<div class="StandardWebpartBtm"></div>
