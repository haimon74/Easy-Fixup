
<%@ Page Language="c#" MasterPageFile="~/Site.Master" CodeBehind="MailBox.aspx.cs"
    AutoEventWireup="True" Inherits="ezFixUp.MailBox" %>

<%@ Import Namespace="ezFixUp" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <div id="sidebar">
        <asp:UpdatePanel ID="upMenu" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkInbox" />
                <asp:AsyncPostBackTrigger ControlID="lnkOutbox" />
                <asp:AsyncPostBackTrigger ControlID="lnkTrash" />
            </Triggers>
            <ContentTemplate>
                <uc1:SideMenuStart ID="SmallBoxStart1" runat="server" ></uc1:SideMenuStart>
                <div id="fmt">
                    <ul class="fmt">
                        <li>
                            <asp:LinkButton ID="lnkInbox" runat="server" />
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkOutbox" runat="server" />
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkTrash" runat="server" />
                        </li>
                    </ul>
                </div>
                <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
                <div style="height:300px;position:static;">
                    <components:BannerView ID="bvMailboxLeftBottom" runat="server" Key="MailboxLeftBottom">
                    </components:BannerView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="main">
        <div class="hide">
            <asp:UpdatePanel ID="UpdatePanelGridMessages" ChildrenAsTriggers="true" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lnkInbox" />
                    <asp:AsyncPostBackTrigger ControlID="lnkOutbox" />
                    <asp:AsyncPostBackTrigger ControlID="lnkTrash" />
                </Triggers>
                <ContentTemplate>
                    <asp:Panel ID="pnlMailBox" runat="server">
                        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
                        <components:BannerView ID="bvMailboxRightTop" runat="server" Key="MailboxRightTop">
                        </components:BannerView>
                        <asp:Panel DefaultButton="btnFilter" runat="server">
                            <table><tr>
                            <td style="width:100px;"><%= Lang.Trans("Username") %>:</td>
                            <td style="width:170px;">
                            <asp:TextBox ID="txtSearchMail" runat="server" CssClass="MailboxInput"></asp:TextBox>
                            </td><td>
                            <asp:Button ID="btnFilter" runat="server" OnClick="btnFilter_Click" CssClass="medium zurbtn blue" />
                            <asp:Button ID="btnClearFilter" runat="server" OnClick="btnClearFilter_Click" CssClass="medium zurbtn red" />
                            </td></tr>
                            </table>
                        </asp:Panel>
                        <div class="separator">
                        </div>
                        <style type="text/css">
                               table#ctl00_cphContent_gridMessages tr {border-top:solid 2px white;}
                        </style>
                        <asp:DataGrid ID="gridMessages" runat="server" AutoGenerateColumns="False" HorizontalAlign="Center"
                                CssClass="TableWrap" ShowHeader="true" CellPadding="0" CellSpacing="0" 
                                GridLines="None" OnItemDataBound="gridMessages_ItemDataBound">
                            <HeaderStyle CssClass="HeaderRow" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="&lt;input type=checkbox  
                                        onClick=&quot;a=0;for(i=0; i&lt;
                                                      this.form.elements.length;i++){
                                                        if(this.form.elements[i]==this) {a=3}; 
                                                        if ((this.form.elements[i].type=='checkbox') &amp;&amp; (a!=0) &amp;&amp; (i&gt;a)) {
                                                            this.form.elements[i].checked=this.checked}}&quot;/&gt;" > 
                                                    <%--&lt;label style='color:white;' for='allcb' &gt;All&lt;/label&gt;">--%>
                                    <HeaderStyle Width="70px" ForeColor="White"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center" Width="70px"></ItemStyle>
                                    <ItemTemplate>
                                        <div id="msgItemContainer" runat="server">
                                            <input type="checkbox" id="cbSelect" value='<%# Eval("Id") %>' runat="server" />
                                            <%# Convert.ToBoolean(Eval("IsRepliedTo")) 
                                                    ? String.Format("<img src=\"images/email-reply.png\" border=\"0\" title=\"{0}\">", "Replied".Translate()) 
                                                    : Convert.ToBoolean(Eval("IsRead")) 
                                                        ? String.Format("<img src=\"images/email-read.png\" border=\"0\" title=\"{0}\">", "Read".Translate()) 
                                                        : String.Format("<img src=\"images/email-unread.png\" border=\"0\" title=\"{0}\">", "Unread".Translate())%>&nbsp;
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle Width="55" HorizontalAlign="Center"></HeaderStyle>
                                    <HeaderTemplate>
                                        <%= "Sender".Translate() %></HeaderTemplate>
                                    <ItemStyle Width="55" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <a href="<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>" 
                                            title="<%# String.Format("Show profile of {0}".Translate(), Eval("DisplayName")) %>" class="tipsys">
                                            <%# (string)Eval("ImageTag") %>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle Width="80"></HeaderStyle>
                                    <HeaderTemplate><%= "User name".Translate() %></HeaderTemplate>
                                    <ItemStyle Width="80" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <a href="<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>" 
                                            title="<%# String.Format("Show profile of {0}".Translate(), Eval("DisplayName")) %>" class="tipsys">
                                            <%# Eval("DisplayName")%>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <HeaderTemplate><%= "Message".Translate() %></HeaderTemplate>
                                    <ItemTemplate>
                                        <a id="lnkReadMessage" runat="server" href='<%# "ShowMessage.aspx?mid=" + Eval("Id") %>'
                                            title='<%# String.Format("Open message from {0}".Translate(), Eval("DisplayName")) %>' class="tipsys">
                                            <span><b><%# Eval("Subject") %></b></span><br />
                                            <span><%# Eval("Message") %></span>
                                        </a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <HeaderTemplate><%= "Date".Translate() %></HeaderTemplate>
                                    <ItemStyle Wrap="False" Width="12%" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <%--<%# (DateTime.Now.Add(Config.Misc.TimeOffset).ToString("d") == Convert.ToDateTime(Eval("Date")).ToString("d"))?Convert.ToDateTime(Eval("Date")).ToString("t"):Convert.ToDateTime(Eval("Date")).ToString("d") %>--%>
                                        <%# Convert.ToDateTime(Eval("Date")).ToString("d")%><br />
                                        <%# Convert.ToDateTime(Eval("Date")).ToString("t")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle></HeaderStyle>
                                    <HeaderTemplate>
                                        &nbsp;
                                    </HeaderTemplate>
                                    <ItemStyle></ItemStyle>
                                    <ItemTemplate>
                                        <div class="<%# (string)Eval("MessageTypeIconClass") %>" />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                        <div class="srvmsg">
                            <asp:Label ID="lblError" CssClass="InfoMessage" runat="server" Visible="false"></asp:Label></div>
                        <div class="clear">
                        </div>
                        <div class="separator">
                        </div>
                        <div class="buttons">
                            <asp:Button ID="btnUndelete" CssClass="medium zurbtn blue" runat="server" Visible="false"></asp:Button>
                            <asp:Button ID="btnDelete" CssClass="medium zurbtn red" runat="server"></asp:Button>
                            <asp:Button ID="btnMarkUnread" CssClass="medium zurbtn blue" runat="server"></asp:Button>
                        </div>
                        <asp:Panel ID="pnlPaginator" Visible="True" runat="server">
                            <div class="separator">
                            </div>
                            <div class="clear">
                            </div>
                            <div class="line">
                            </div>
                            <div class="separator">
                            </div>
                            <div class="endbox">
                                <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
                                <asp:Label ID="lblPager" runat="server"></asp:Label>
                                <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
                            </div>
                        </asp:Panel>
                        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
                    </asp:Panel>
                    <div style="height:100px;position:static;">
                        <components:BannerView ID="bvMailboxRightBottom" runat="server" Key="MailboxRightBottom">
                        </components:BannerView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%--<asp:UpdatePanel ID="UpdatePanelGridEcards" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlEcards" runat="server">
                    <uc1:LargeBoxStart ID="LargeBoxStart2" runat="server"></uc1:LargeBoxStart>
                    <asp:DataGrid ID="dgEcards" runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False"
                        HorizontalAlign="Center" CssClass="TableWrap" CellPadding="0" CellSpacing="0"
                        GridLines="None">
                        <HeaderStyle CssClass="HeaderRow" />
                        <AlternatingItemStyle CssClass="AlternativeRow" />
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="20px"></ItemStyle>
                                <HeaderTemplate>
                                    &nbsp;
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="checkbox" id="cbSelect2" value='<%# Eval("Id") %>' runat="server" name="Checkbox2" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center" Width="25%"></ItemStyle>
                                <ItemTemplate>
                                    <a href="<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>" target="_new">
                                        <%# Eval("Username") %></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="center"></HeaderStyle>
                                <ItemStyle Width="20px" HorizontalAlign="center"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Age") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="center"></ItemStyle>
                                <ItemTemplate>
                                    <%# currentEcardFolder == Ecard.eFolder.Sent || Convert.ToBoolean(Eval("IsOpened")) 
                                        ? String.Format("<img src=\"images/email-read.png\" border=\"0\" title=\"{0}\">", "Read".Translate()) 
                                        : String.Format("<img src=\"images/email-unread.png\" border=\"0\" title=\"{0}\">", "Unread".Translate())%>&nbsp;
                                    <a href='<%# (EcardType.Fetch(int.Parse((string)Eval("EcardTypeID"))).Type != EcardType.eType.Wink)
                                                        ? String.Format("ShowEcard.aspx?ect={0}&ecid={1}", Eval("EcardTypeID"), Eval("Id")) 
                                                        : "#" %>'><%# Eval("EcardTypeName")%></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="center"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                                <ItemTemplate>
                                    <%# (DateTime.Now.Add(Config.Misc.TimeOffset).ToString("d") == Convert.ToDateTime(Eval("DatePosted")).ToString("d"))?Convert.ToDateTime(Eval("DatePosted")).ToString("t"):Convert.ToDateTime(Eval("DatePosted")).ToString("d") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Font-Bold="True" HorizontalAlign="center"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="center"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("LastOnline") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <br />
                    <div class="clear">
                    </div>
                    <div class="separator">
                    </div>
                    <div class="buttons">
                        <asp:Button ID="btnDeleteEcard" runat="server" CssClass="DeleteBtn"></asp:Button></div>
                    <div class="srvmsg">
                        <asp:Label ID="lblMessage2" runat="server"></asp:Label></div>
                    <uc1:LargeBoxEnd ID="Largeboxend2" runat="server"></uc1:LargeBoxEnd>
                </asp:Panel>
                <components:BannerView ID="bvMailboxRightBottom" runat="server" Key="MailboxRightBottom">
                </components:BannerView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkInbox" />
                <asp:AsyncPostBackTrigger ControlID="lnkOutbox" />
                <asp:AsyncPostBackTrigger ControlID="lnkTrash" />
                <asp:AsyncPostBackTrigger ControlID="lnkSentEcards" />
                <asp:AsyncPostBackTrigger ControlID="lnkReceivedEcards" />
            </Triggers>
        </asp:UpdatePanel>--%>
    </div>
</asp:Content>
