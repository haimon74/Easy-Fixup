<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true"
    CodeBehind="ShowUser.aspx.cs" Inherits="AspNetDating.Mobile.ShowUser" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine">
    </div>
    <div align="center">
        <asp:Label ID="lblSlogan" runat="server"></asp:Label>
        <div class="SeparatorLine">
        </div>
        <asp:Literal ID="ltrPhoto" runat="server"></asp:Literal>
        <div class="Separator">
        </div>
        <asp:Panel ID="pnlPaginator" Visible="True" runat="server">
            <asp:ImageButton ID="lnkPrev" ImageUrl="~/Mobile/Images/IconPrev.jpg" runat="server"
                OnClick="lnkPrev_Click"></asp:ImageButton>
            <asp:Label ID="lblPager" CssClass="Pager" runat="server"></asp:Label>
            <asp:ImageButton ID="lnkNext" ImageUrl="~/Mobile/Images/IconNext.jpg" runat="server"
                OnClick="lnkNext_Click"></asp:ImageButton>
        </asp:Panel>
        <div id="pnlUnlockPhotos" runat="server" visible="false">
            <components:ContentView ID="cvUnlockPhotos" Key="UnlockPhotosMobile" runat="server">
            </components:ContentView>
            <asp:Button ID="btnUnlockPhotos" runat="server" OnClick="btnUnlockPhotos_Click" />
        </div>
    </div>
    <div class="SeparatorLine">
    </div>
    <div class="PaddingWrap">
        <b>
            <%= Lang.Trans("Username") %>:</b>
        <asp:Label ID="lblUsername" runat="server"></asp:Label>
        <div id="pnlAge" runat="server">
            <b>
                <%= Lang.Trans("Age") %>:</b>
            <asp:Label ID="lblAge" runat="server"></asp:Label>
        </div>
        <div id="pnlGender" runat="server">
            <b>
                <%= Lang.Trans("Gender") %>:</b>
            <asp:Label ID="lblGender" runat="server"></asp:Label>
        </div>
        <div id="pnlLocation" runat="server">
            <b>
                <%= Lang.Trans("Location") %>:</b>
            <%= User.LocationString %>
        </div>
        <b>
            <%= Lang.Trans("Last Online") %>:</b>
        <asp:Label ID="lblLastOnline" runat="server"></asp:Label>
        <div id="pnlStatusText" runat="server" visible="false">
            <b>
                <%= Lang.Trans("Status") %>:</b>
            <asp:Label ID="lblStatusText" runat="server"></asp:Label>
        </div>
        <a id="lnkSendMessage" class="SideMenuLink skinlinkcolor skinactionlinkcolor" runat="server">
            <%= "Send Message".Translate() %></a>
    </div>
    <br />
    <asp:Repeater ID="rptTopics" runat="server" OnItemDataBound="rptTopics_ItemDataBound">
        <ItemTemplate>
            <div class="SeparatorLine">
            </div>
            <div class="PaddingWrap">
                <h3 class="SectionHeading">
                    <%# Eval("TopicName")%></h3>
                <asp:Repeater ID="rptQuestions" runat="server">
                    <ItemTemplate>
                        <b>
                            <%# Eval("QuestionName")%></b>
                        <%# Eval("Answer") %>
                        <br />
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </ItemTemplate>
        <SeparatorTemplate>
        </SeparatorTemplate>
    </asp:Repeater>
</asp:Content>
