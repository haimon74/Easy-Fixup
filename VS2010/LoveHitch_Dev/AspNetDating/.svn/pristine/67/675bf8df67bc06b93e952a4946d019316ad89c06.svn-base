<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportAbuse.ascx.cs" Inherits="AspNetDating.Components.ReportAbuse" %>
<%@ Register TagPrefix="cv" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<asp:Panel ID="pnlReportAbusePopup" runat="server"> 
	<div class="srvmsg">
	    <asp:Label ID="lblError" CssClass="InfoMessage" EnableViewState="false" runat="server" Visible="false"></asp:Label>
    </div>
    <asp:MultiView ID="mvContentViews" runat="server">
        <asp:View ID="vProfile" runat="server">
            <cv:ContentView ID="cvProfile" runat="server">
                <!-- Place your content for reporting profile abuse here -->
            </cv:ContentView>
        </asp:View>
        <asp:View ID="vMessage" runat="server">
            <cv:ContentView ID="cvMessage" runat="server">
                <!-- Place your content for reporting message abuse here -->
            </cv:ContentView>        
        </asp:View>
        <asp:View ID="vPhoto" runat="server">
            <cv:ContentView ID="cvPhoto" runat="server">
                <!-- Place your content for reporting photo abuse here -->
            </cv:ContentView>        
        </asp:View>     
    </asp:MultiView>
    <div class="clear"></div>
    <div class="wrap-sized">
	    <div class="note">
	        <asp:Literal ID="litText" runat="server"></asp:Literal>
	    </div>
	    <asp:TextBox ID="txtReport" CssClass="multilinetextbox" TextMode="MultiLine" runat="server"></asp:TextBox>
    </div>
    <div class="buttons">
        <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" CssClass="medium zurbtn blue"/>
        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="medium zurbtn red"/>
    </div>  
</asp:Panel>

