<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true"
    CodeBehind="Search.aspx.cs" Inherits="AspNetDating.Mobile.Search" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Import Namespace="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine"></div>
    <asp:MultiView ID="mvSearch" runat="server">
       
        <asp:View ID="viewBasicSearch" runat="server">
    <div class="ContentWrap">         
            <asp:Panel ID="pnlGenderQuickSearch" runat="server">
                <%= Lang.Trans("I am looking for") %>
                <br />
                <asp:DropDownList ID="dropGender" CssClass="DropDownList" runat="server">
                </asp:DropDownList>
            </asp:Panel>
            <asp:Panel ID="trInterestedIn" runat="server">
			    <div class="Separator"></div>            
                <%= Lang.Trans("seeking") %>
                <br />
                <asp:DropDownList ID="dropInterestedIn" CssClass="DropDownList" runat="server">
                </asp:DropDownList>
            </asp:Panel>
            <asp:Panel ID="pnlCountry" runat="server">
			    <div class="Separator"></div>            
                <%= Lang.Trans("Country") %>
                <br />
                <asp:DropDownList ID="dropCountry" runat="server" CssClass="DropDownList" AutoPostBack="true" OnSelectedIndexChanged="dropCountry_SelectedIndexChanged">
                </asp:DropDownList>
            </asp:Panel>
            <asp:Panel ID="pnlState" runat="server">
			    <div class="Separator"></div>            
                <%= Lang.Trans("Region/State") %>
                <br />
                <asp:DropDownList ID="dropRegion" runat="server" CssClass="DropDownList" AutoPostBack="true" OnSelectedIndexChanged="dropRegion_SelectedIndexChanged">
                </asp:DropDownList>
            </asp:Panel>
            <asp:Panel ID="pnlCity" runat="server">
			    <div class="Separator"></div>            
                <%= Lang.Trans("City") %>
                <br />
                <asp:DropDownList ID="dropCity" runat="server" CssClass="DropDownList" AutoPostBack="true">
                </asp:DropDownList>
            </asp:Panel>
            
            <asp:Panel ID="pnlZip" runat="server">
			    <div class="Separator"></div>            
                <%= Lang.Trans("Zip/Postal Code") %>
                <br />
                <asp:TextBox ID="txtZip" runat="server" CssClass="TextField"></asp:TextBox>
            </asp:Panel>
            <asp:Panel ID="pnlAgeRangeQuickSearch" runat="server">
			    <div class="Separator"></div>            
                <%= Lang.Trans("Age Range") %>&nbsp;<%= Lang.Trans("from") %>
                <asp:TextBox ID="txtAgeFrom" runat="server" CssClass="SmallTextField" Size="2" MaxLength="2"></asp:TextBox>
                <%= Lang.Trans("to") %>
                <asp:TextBox ID="txtAgeTo" runat="server" CssClass="SmallTextField" Size="2" MaxLength="2"></asp:TextBox>
            </asp:Panel>
		    <div class="Separator"></div>            
            <%= Lang.Trans("Photo Required") %>
            <asp:CheckBox ID="cbPhotoReq" runat="server" Checked="True"></asp:CheckBox>
		    </div> 
                <div class="SeparatorLine"></div> 
                 <div class="Separator"></div>         
            <asp:Button ID="btnBasicSearchGo" runat="server" CssClass="LoginBtn" OnClick="btnBasicSearchGo_Click">
            </asp:Button>
        </asp:View>
        
        <asp:View ID="viewSearchResults" runat="server">
            <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand" OnItemCreated="rptUsers_ItemCreated"
                OnItemDataBound="rptUsers_ItemDataBound">
                        <HeaderTemplate>
            <table cellpadding="0" class="TableWrap" cellspacing="0" width="100%">
        </HeaderTemplate>
                <alternatingitemtemplate>
                <tr>
                <td align="center" valign="top">
                    <a href='<%# UrlRewrite.CreateMobileShowUserUrl((string)Eval("Username")) %>' runat="server">
                        <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 100, 100, "ImgBorder", false, true, true) %>
                    </a>
                </td>
                <td valign="top">
                    <b><%# Server.HtmlEncode((((string) Eval("Slogan")).Length > 80 ?((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan")) as string) %></b><br />
                    <b><%= Lang.Trans("Username") %>:</b>&nbsp;<%# Eval("Username") %><br />
                    <% if (showAge) { %>
                    <b><%= Lang.Trans("Age")%>:</b>&nbsp;<%# Eval("Age") %><br />
                    <% } %>
                    <% if (showGender) { %>
                    <b><%= Lang.Trans("Gender") %>:</b>&nbsp;<%# Lang.Trans((string)Eval("Gender")) %><br />
                    <% } %>
                    <% if (showCity) { %>
                    <b><%= Lang.Trans("Location") %>:</b>&nbsp;<%# Eval("Location") %><br />
                    <% } %>
                    <b><%= Lang.Trans("Last Online") %>:</b>&nbsp;<%# Eval("LastOnlineString") %>
                 </td>
                 </tr>                
                </alternatingitemtemplate>
                <ItemTemplate>
                <tr  bgcolor="#eff2f8">
                <td align="center" valign="top">
                    <a href='<%# UrlRewrite.CreateMobileShowUserUrl((string)Eval("Username")) %>' runat="server">
                        <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 100, 100, "ImgBorder", false, true, true) %>
                    </a>
                </td>
                <td valign="top">
                    <b><%# Server.HtmlEncode((((string) Eval("Slogan")).Length > 80 ?((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan")) as string) %></b><br />
                    <b><%= Lang.Trans("Username") %>:</b>&nbsp;<%# Eval("Username") %><br />
                    <% if (showAge) { %>
                    <b><%= Lang.Trans("Age")%>:</b>&nbsp;<%# Eval("Age") %><br />
                    <% } %>
                    <% if (showGender) { %>
                    <b><%= Lang.Trans("Gender") %>:</b>&nbsp;<%# Lang.Trans((string)Eval("Gender")) %><br />
                    <% } %>
                    <% if (showCity) { %>
                    <b><%= Lang.Trans("Location") %>:</b>&nbsp;<%# Eval("Location") %><br />
                    <% } %>
                    <b><%= Lang.Trans("Last Online") %>:</b>&nbsp;<%# Eval("LastOnlineString") %>
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
        </asp:View>
    </asp:MultiView>
</asp:Content>
