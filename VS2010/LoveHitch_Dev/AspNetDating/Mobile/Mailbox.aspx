<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true"
    CodeBehind="Mailbox.aspx.cs" Inherits="AspNetDating.Mobile.Mailbox" %>

<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine">
    </div>
    <asp:Repeater ID="rptMessages" runat="server">
        <HeaderTemplate>
            <table cellpadding="0" class="TableWrap" cellspacing="0" width="100%">
        </HeaderTemplate>
        <AlternatingItemTemplate>
            <tr>
                <td align="center" valign="top">
                    <a href="<%# UrlRewrite.CreateMobileShowUserUrl((string)Eval("Username")) %>">
                        <%# ImageHandler.RenderImageTag((int)Eval("PhotoID"), 50, 50, "ImgBorder", false, true, true) %></a><br />
                    <a href="<%# UrlRewrite.CreateMobileShowUserUrl((string)Eval("Username")) %>">
                        <%# Eval("Username") %></a>
                </td>
                <td align="left" valign="top">
                    <%# (DateTime.Now.Add(Config.Misc.TimeOffset).ToString("d") == Convert.ToDateTime(Eval("Date")).ToString("d"))?Convert.ToDateTime(Eval("Date")).ToString("t"):Convert.ToDateTime(Eval("Date")).ToString("d") %>
                    <br />
                    <%# Convert.ToBoolean(Eval("IsRead"))?"":"<img src=\"images/email-unread.gif\" border=\"0\">" %>&nbsp;<a
                        href="ShowMessage.aspx?mid=<%# DataBinder.Eval(Container, "DataItem.Id") %> <%# Session[""] %>"><%# DataBinder.Eval(Container, "DataItem.Message") %></a>
                </td>
            </tr>
        </AlternatingItemTemplate>
        <ItemTemplate>
            <tr bgcolor="#eff2f8">
                <td align="center" valign="top">
                    <a href="<%# UrlRewrite.CreateMobileShowUserUrl((string)Eval("Username")) %>">
                        <%# ImageHandler.RenderImageTag((int)Eval("PhotoID"), 50, 50, "ImgBorder", false, true, true) %></a><br />
                    <a href="<%# UrlRewrite.CreateMobileShowUserUrl((string)Eval("Username")) %>">
                        <%# Eval("Username") %></a>
                </td>
                <td align="left" valign="top">
                    <%# (DateTime.Now.Add(Config.Misc.TimeOffset).ToString("d") == Convert.ToDateTime(Eval("Date")).ToString("d"))?Convert.ToDateTime(Eval("Date")).ToString("t"):Convert.ToDateTime(Eval("Date")).ToString("d") %>
                    <br />
                    <%# Convert.ToBoolean(Eval("IsRead"))?"":"<img src=\"images/email-unread.gif\" border=\"0\">" %>&nbsp;<a
                        href="ShowMessage.aspx?mid=<%# DataBinder.Eval(Container, "DataItem.Id") %> <%# Session[""] %>"><%# DataBinder.Eval(Container, "DataItem.Message") %></a>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table></FooterTemplate>
    </asp:Repeater>
    <div class="SeparatorLine"></div>
    <asp:Panel ID="pnlPaginator" CssClass="Paginator" Visible="True" runat="server">
       <span class="First"><asp:ImageButton ID="lnkFirst" runat="server" ImageAlign="AbsMiddle" ImageUrl="Images/IconFirst.jpg" OnClick="lnkFirst_Click"></asp:ImageButton></span>
        <span class="Prev"><asp:ImageButton ID="lnkPrev" runat="server" ImageAlign="AbsMiddle" ImageUrl="Images/IconPrev.jpg" OnClick="lnkPrev_Click"></asp:ImageButton></span>
        <span class="Pager"><asp:Label ID="lblPager" runat="server"></asp:Label></span>
        <span class="Last"><asp:ImageButton ID="lnkLast" runat="server" ImageAlign="AbsMiddle" ImageUrl="Images/IconLast.jpg" OnClick="lnkLast_Click"></asp:ImageButton></span>        
        <span class="Next"><asp:ImageButton ID="lnkNext" runat="server" ImageAlign="AbsMiddle" ImageUrl="Images/IconNext.jpg" OnClick="lnkNext_Click"></asp:ImageButton></span>
    </asp:Panel>
</asp:Content>
