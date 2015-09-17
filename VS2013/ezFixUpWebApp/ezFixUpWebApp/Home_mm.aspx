<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="Home_mm.aspx.cs" AutoEventWireup="True"
    Inherits="ezFixUp.Home_mm" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Register TagPrefix="mm" TagName="MatchToFriend" Src="Components/Matchmaker/MatchToFriend.ascx" %>
<%@ Register TagPrefix="CustomWebPartManager" Namespace="ezFixUp.Classes"
    Assembly="ezFixUpWebApp" %>
<%@ Register TagPrefix="CustomWebPartZone" Namespace="ezFixUp.Classes"
    Assembly="ezFixUpWebApp" %>
<%@ Register TagPrefix="CustomEditorZone" Namespace="ezFixUp.Classes"
    Assembly="ezFixUpWebApp" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <CustomWebPartManager:CustomWebPartManager ID="WebPartManager2" Personalization-InitialScope="User"
        Personalization-Enabled="true" runat="server">
    </CustomWebPartManager:CustomWebPartManager>
    <div id="sidebar">
        <mm:MatchToFriend ID="MatchToFriend1" runat="server" IsVisible="true"/>
 <%--       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <CustomWebPartZone:CustomWebPartZone BorderStyle="None" BorderWidth="0"
                    CssClass="SideMenuWebpartsWrap" ID="wpzHomePageLeftZone" WebPartVerbRenderMode="TitleBar"
                    HeaderText="" PartChromePadding="0" runat="server">
                    <PartStyle CellPadding="0" CssClass="SideMenuWebpartContent roundedBoxContent"></PartStyle>
                    <PartTitleStyle CssClass="SideMenuWebpartTop roundedBoxHeader"></PartTitleStyle>
                    <ZoneTemplate>
                    </ZoneTemplate>
                    <CloseVerb Text="Close" Visible="false" />
                    <MinimizeVerb Visible="false" ImageUrl="~/Images/minimize2.png" />
                    <RestoreVerb Visible="false" ImageUrl="~/Images/restore.gif" />
                    <DeleteVerb Visible="false" ImageUrl="~/Images/delete2.png" />
                    <EditVerb Visible="false" ImageUrl="~/Images/edit.gif" />
                </CustomWebPartZone:CustomWebPartZone>
                <div id="pnlEditorZoneLeft" runat="server">
                    <CustomEditorZone:CustomEditorZone CssClass="SideMenuEditorZone" Padding="0" 
                        ID="ezEditorZoneLeft" PartChromePadding="0" runat="server">
                        <HeaderStyle CssClass="SideMenuWebpartTop" />
                        <FooterStyle CssClass="SideMenuWebpartBtm"></FooterStyle>
                        <HeaderCloseVerb ImageUrl="~/Images/delete2.png" />
                        <ZoneTemplate>
                        </ZoneTemplate>
                    </CustomEditorZone:CustomEditorZone>
                </div>
                
<%--                </ContentTemplate>
        </asp:UpdatePanel>--%>
         <asp:UpdatePanel ID="upLeftCatalogButton" runat="server">
            <Triggers >
                <asp:AsyncPostBackTrigger ControlID="lnkCatalogForLeftParts" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkCatalogForRightParts" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="imgbCloseCatalog" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelCatalog" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div class="mode">
                   <asp:LinkButton ID="lnkCatalogForLeftParts" runat="server"  Font-Size="Small" Font-Bold="false"
                        CssClass="fr" OnClick="lnkCatalogForLeftParts_Click"></asp:LinkButton>
                </div>                
                <div style="height:350px;position:static;">
                    <components:BannerView ID="bvHomeLeftBottom" runat="server" Key="HomeLeftBottom">
                    </components:BannerView>
                </div>              
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="main">
        <%--<img src="App_Themes/Colors2_MM/find_matchmaking.png" width="100%" height="60"
            alt="find matchmaking" title="matchmaking state" />--%>
        <%--<asp:UpdatePanel ID="upWebParts" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="imgbCloseCatalog" EventName="Click" />
        </Triggers>
            <ContentTemplate>--%>
                <CustomWebPartZone:CustomWebPartZone BorderStyle="None"  BorderWidth="0"
                    CssClass="StandardWebpartsWrap"  runat="server"
                    ID="wpzHomePageRightZone" WebPartVerbRenderMode="TitleBar" HeaderText=""
                    PartChromePadding="0" Padding="0">
                    <PartStyle CellPadding="0" CssClass="StandardWebpartContent roundedBoxContent"></PartStyle>
                    <PartTitleStyle CssClass="StandardWebpartTop roundedBoxHeader"></PartTitleStyle>
                    <ZoneTemplate>
                    </ZoneTemplate>
                    <CloseVerb Visible="false" />
                    <RestoreVerb Visible="false" ImageUrl="~/Images/restore.gif" />
                    <MinimizeVerb Visible="false" ImageUrl="~/Images/minimize2.png" />
                    <EditVerb Visible="false" Enabled="false" ImageUrl="~/Images/edit.gif" />
                    <DeleteVerb Visible="false" ImageUrl="~/Images/delete2.png" />
                </CustomWebPartZone:CustomWebPartZone>
                <div class="clear"></div>                
                <div id="pnlEditorZoneRight" runat="server">
                    <CustomEditorZone:CustomEditorZone PartChromePadding="0" Padding="0"
                        CssClass="StandardEditorZone" ID="ezEditorZoneRight" runat="server">
                        <HeaderStyle CssClass="StandardWebpartTop roundedBoxHeader" />
                        <FooterStyle CssClass="StandardWebpartBtm"></FooterStyle>
                        <HeaderCloseVerb ImageUrl="~/Images/delete2.png" />
                        <ZoneTemplate>
                        </ZoneTemplate>
                    </CustomEditorZone:CustomEditorZone>
                </div>
<%--</ContentTemplate>
        </asp:UpdatePanel>--%>
        <asp:UpdatePanel ID="upCatalog" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkCatalogForLeftParts" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkCatalogForRightParts" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="pnlCatalog" Visible="false" runat="server">
                    <div class="StandardBox WebpartsCatalogWrap">
                        <div class="StandardWebpartTop">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td >
                                        <span><%= Lang.Trans("Add Components") %></span>
                                    </td>
                                    <td class="fr" valign="middle" >
                                    <div style="margin-top:5px;height:22px;">
                                        <asp:ImageButton ID="imgbCloseCatalog" runat="server" ImageUrl="~/Images/delete.gif" Height="20" 
                                            OnClick="imgbCloseCatalog_Click" style='padding:0;margin:0;'></asp:ImageButton>&nbsp;
                                            </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="StandardBoxContent">
                            <asp:Repeater ID="rptCatalogWebParts" runat="server" 
                                OnItemCommand="rptCatalogWebParts_ItemCommand">
                                <ItemTemplate>
                                    <table cellpadding="0" cellspacing="0"  border="0" class="WebpartsCatalogZone w100">
                                        <tr>
                                            <td valign="top" width="100">
                                                <img border="0" title='<%# ((string)Eval("Name")).Translate() %>' src='<%# Eval("ThumbnailURL") %>' />
                                            </td>
                                            <td valign="top">
                                                <div class="SectionHeading">
                                                    <%# ((string)Eval("Name")).Translate() %></div>
                                                <div class="note">
                                                    <%# ((string)Eval("Description")).Translate() %></div>
                                                <div class="buttons fr">
                                                    <asp:Button ID="btnAdd" CommandName="Add" Text='<%# "Add".Translate() %>' CommandArgument='<%# Eval("ControlPath") %>'
                                                        runat="server" CssClass="medium zurbtn blue" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <div class="separator6"></div>
                                </SeparatorTemplate>
                            </asp:Repeater>
                            <div class="separator6"></div>
                            <div class="buttons">
                                <asp:Button ID="btnCancelCatalog" runat="server" OnClick="btnCancelCatalog_Click" CssClass="medium zurbtn red fr" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="clear"></div>
               </ContentTemplate>
        </asp:UpdatePanel>
         <asp:UpdatePanel ID="upRightCatalogButton" runat="server">
            <Triggers >
                <asp:AsyncPostBackTrigger ControlID="lnkCatalogForLeftParts" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkCatalogForRightParts" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="imgbCloseCatalog" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancelCatalog" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <div class="mode">
                    <asp:LinkButton ID="lnkCatalogForRightParts" runat="server" CssClass="fr" Font-Size="Small" Font-Bold="false"
                        OnClick="lnkCatalogForRightParts_Click"></asp:LinkButton>
                </div>                
                <div style="height:100px;position:static;">
                    <components:BannerView ID="bvHomeRightBottom" runat="server" Key="HomeRightBottom">
                    </components:BannerView>
                </div>                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>