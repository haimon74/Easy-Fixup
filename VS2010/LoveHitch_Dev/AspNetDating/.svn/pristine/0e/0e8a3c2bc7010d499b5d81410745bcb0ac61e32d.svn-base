<%@ Page Language="c#" 
         MasterPageFile="~/Site.Master" 
         CodeBehind="FAQs.aspx.cs"
         AutoEventWireup="True" 
         Inherits="AspNetDating.FAQs" %>

<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FAQ" Src="Components/FAQs.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <div id="sidebar">
        <asp:UpdatePanel ID="upMenu" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkCategory1" />
                <asp:AsyncPostBackTrigger ControlID="lnkCategory2" />
                <asp:AsyncPostBackTrigger ControlID="lnkCategory3" />
            </Triggers>
            <ContentTemplate>
                <uc1:SideMenuStart ID="SmallBoxStart1" runat="server"></uc1:SideMenuStart>
                <div id="fmt">
                    <ul class="fmt">
                        <li>
                            <asp:LinkButton ID="lnkCategory1" runat="server" />
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCategory2" runat="server" />
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkCategory3" runat="server" />
                        </li>                        
                    </ul>
                </div>
                <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
                <div style="height:300px;position:static;">
                    <components:BannerView ID="bvFAQsLeftBottom" runat="server" Key="MailboxLeftBottom">
                    </components:BannerView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="main">
        <div class="hide">
            <asp:UpdatePanel ID="UpdatePanelGridMessages" ChildrenAsTriggers="true" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lnkCategory1" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCategory2" />
                    <asp:AsyncPostBackTrigger ControlID="lnkCategory3" />                    
                </Triggers>
                <ContentTemplate>
                    <uc1:LargeBoxStart ID="lbsFAQs" runat="server" Title="Frequently Asked Questions" />
                    <div>
                        <uc1:FAQ ID="faqPerCategory" runat="server" ></uc1:FAQ>
                    </div>
                    <uc1:LargeBoxEnd ID="lbeFAQs" runat="server" />
                    <div style="height:100px;position:static;">
                        <components:BannerView ID="bvFAQsRightBottom" runat="server" Key="FAQsRightBottom">
                        </components:BannerView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        
    </div>
</asp:Content>
