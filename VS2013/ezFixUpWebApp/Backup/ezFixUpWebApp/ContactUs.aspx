<%@ Page Language="c#" MasterPageFile="~/Site.Master" CodeBehind="ContactUs.aspx.cs"
    AutoEventWireup="True" Inherits="ezFixUp.ContactUs" %>

<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <div id="sidebar">
        <uc1:SideMenuStart ID="SmallBoxStart1" runat="server"></uc1:SideMenuStart>
        <div id="fmt">
            <ul class="fmt">
                <li>
                    <asp:LinkButton ID="lnkFAQ" runat="server" PostBackUrl="~/FAQs.aspx" />
                </li>
                <%--<li>
                    <asp:LinkButton ID="lnkAboutUs" runat="server" PostBackUrl="~/FAQs.aspx" />
                </li>
                <li>
                    <asp:LinkButton ID="lnkPrivacyPolicy" runat="server" PostBackUrl="~/FAQs.aspx" />
                </li>
                <li>
                    <asp:LinkButton ID="lnkTermsOfUse" runat="server" PostBackUrl="~/FAQs.aspx" />
                </li>
                <li>
                    <asp:LinkButton ID="lnkMore" runat="server" PostBackUrl="~/FAQs.aspx" />
                </li>--%>
            </ul>
        </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
        <div style="height:300px;position:static;">
            <components:BannerView ID="bvContactUsLeftBottom" runat="server" Key="ContactUsLeftBottom">
            </components:BannerView>
        </div>
        
    </div>
    <div id="main">
        <div style="background: transparent;">
            <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server" ></uc1:LargeBoxStart>                
                 <div class="srvmsg">
	                <asp:Label ID="lblError" CssClass="InfoMessage" EnableViewState="false" runat="server" Visible="false"></asp:Label>
                </div>
                <table>
                     <tr>
                     <td><%= "Select Option".Translate() %></td>
                     <td>
                        <select id="ddlCategory" runat="server" enableviewstate="true">
                            <option>Other</option>
                            <option>Requesting Support</option>
                            <option>Want to Advertise</option>
                            <option>Reporting a Problem</option>                    
                        </select>
                     </td>
                     </tr>
                     <tr>
                     <td><%= "Your Name".Translate() %></td>
                     <td><asp:TextBox ID="txtName" runat="server" Width="140" CssClass="l_form_txt"/></td>
                     </tr>
                     <tr>
                     <td><%= "Your Email".Translate() %></td>
                     <td><asp:TextBox ID="txtEmail" runat="server" Width="140" CssClass="l_form_txt"/></td>
                     </tr>
                     <tr>
                     <td style="vertical-align:top"><%= "Content".Translate() %></td>
                     <td><asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="10" Width="400"/></td>
                     </tr>
                     <tr><td></td><td><asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" CssClass="medium zurbtn blue" /></td></tr>
                 </table>
        
            <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>        
        </div>
        <div style="height:150px;position:static;">
            <components:BannerView ID="ContactUsRightBottom" runat="server" Key="ContactUsRightBottom">
            </components:BannerView>
        </div>
     </div>
</asp:Content>
