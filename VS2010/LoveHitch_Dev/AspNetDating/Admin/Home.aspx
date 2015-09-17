<%@ Page Language="c#" CodeBehind="Home.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.Home" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>
        <%= Lang.TransA("Home") %>
    </title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
    <div id="layout">
        <div id="crupms">
            <img src="images/i_home.jpg">
            <div id="crumpstxt">
                <%= Lang.TransA("Welcome!") %>
            </div>
        </div>
        <div id="sidebar">
            <uc1:AdminMenu ID="Adminmenu1" runat="server"></uc1:AdminMenu>
        </div>
        <div id="main">
            <div id="content-head">
                <div class="ch-head">
                    <%= String.Format("{0} Administrative Tool".TranslateA(), Config.Misc.SiteTitle) %>
                </div>
                <div class="ch-description">
                </div>
            </div>
            <uc1:MessageBox ID="MessageBox" runat="server"></uc1:MessageBox>
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <div id="divRegistrationsChart" runat="server">
                            <asp:Chart ID="ChartNewRegistrations" runat="server" Width="790" Height="200" Palette="BrightPastel"
                                BackColor="#D3DFF0" BorderDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2"
                                BorderColor="26, 59, 105">
                                <Titles>
                                    <asp:Title Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold"
                                        ShadowOffset="3" Alignment="TopCenter" ForeColor="26, 59, 105">
                                    </asp:Title>
                                </Titles>
                                <Series>
                                    <asp:Series Name="Default" ChartType="Column" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                                        BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                                        <Area3DStyle Rotation="10" Inclination="30" Perspective="5" PointDepth="200" IsRightAngleAxes="False"
                                            Enable3D="true" WallWidth="0" IsClustered="False" />
                                        <AxisY LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                        </AxisY>
                                        <AxisX LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                        </AxisX>
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divUsersPie" runat="server">
                            <asp:Chart ID="ChartTotalRegistrations" runat="server" Width="790" Height="300">
                                <Titles>
                                    <asp:Title Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold"
                                        ShadowOffset="3" Alignment="TopCenter" ForeColor="26, 59, 105">
                                    </asp:Title>
                                </Titles>
                                <Legends>
                                    <asp:Legend Name="Default" BackColor="Transparent" Alignment="Center" Docking="Right"
                                        Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" LegendStyle="Column">
                                    </asp:Legend>
                                </Legends>
                                <Series>
                                    <asp:Series Name="Default" ChartType="Pie" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240"
                                        ToolTip="#VALX" LegendText="#VALX">
                                    </asp:Series>
                                </Series>
                                <ChartAreas>
                                    <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent"
                                        BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
                                        <Area3DStyle Rotation="0" Inclination="55" Enable3D="true" />
                                        <AxisY LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                        </AxisY>
                                        <AxisX LineColor="64, 64, 64, 64">
                                            <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            <MajorGrid LineColor="64, 64, 64, 64" />
                                        </AxisX>
                                        <InnerPlotPosition Width="100" Height="100" />
                                    </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Repeater ID="rptPlugins" runat="server">
                            <HeaderTemplate>
                                <div id="content-head" class="NewsHeader">
                                    <div class="ch-head">
                                <%= "Plug-ins status".TranslateA() %></div>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="NewsWrap">
                                    <div>
                                        <img src='images/<%# Eval("Icon") %>.png' border="0" />
                                        <%# Eval("Name") %>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Repeater ID="rptHelpdeskTopics" runat="server">
                            <HeaderTemplate>
                                <div id="content-head" class="NewsHeader">
                                    <div class="ch-head">
                                        <%= "The Latest Helpdesk Topics".TranslateA() %></div>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="NewsWrap">
                                    <div>
                                        <a href='<%# Eval("Link") %>' target="_blank">
                                            <%# Eval("Title") %></a>
                                    </div>
                                    <div class="date">
                                        <%# String.Format("Published on <b>{0}</b> in category <b>{1}</b>".TranslateA(), Convert.ToDateTime(Eval("PubDate")).ToShortDateString(), Eval("Category")) %>
                                    </div>
                                    <div>
                                        <%# ((string)Eval("Description")).StripHTML().Shorten(100) %>
                                        (<a href='<%# Eval("Link") %>' target="_blank"><%# "read".TranslateA() %></a>)
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
