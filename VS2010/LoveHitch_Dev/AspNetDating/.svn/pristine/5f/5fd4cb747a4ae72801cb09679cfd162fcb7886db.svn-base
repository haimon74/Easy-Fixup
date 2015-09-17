<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupPoll.ascx.cs" Inherits="AspNetDating.Components.Groups.GroupPoll" %>
<asp:MultiView ID="mvPoll" runat="server">
    <asp:View ID="viewChoices" runat="server">
        <asp:RadioButtonList ID="rblChoices" runat="server">
        </asp:RadioButtonList>
        <asp:Button ID="btnVote" runat="server" OnClick="btnVote_Click" />
    </asp:View>
    <asp:View ID="viewResults" runat="server">
        <div id="divResultsChart" align="center" runat="server">
            <div class="separator"></div>
            <asp:Chart ID="ChartPollResults" runat="server" Width="500" Height="400" >
                <Titles>
                    <asp:Title Name="Title1" ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Alignment="TopCenter" ForeColor="26, 59, 105"></asp:Title>
                </Titles>
                <legends>
					<asp:Legend Name="Default" BackColor="Transparent" Alignment="Center" Docking="Right" Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" LegendStyle="Column">
					</asp:Legend>
				</legends>
                <series>
					<asp:Series Name="Default" ChartType="Pie" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ToolTip="#VALX\n#VALY" LegendText="#VALX #VALY">
					</asp:Series>
				</series>
				<chartareas>
					<asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="Transparent" ShadowColor="Transparent" BorderWidth="0">
						<area3dstyle Rotation="0" Inclination="55" Enable3D="true"/>
						<axisy LineColor="64, 64, 64, 64">
							<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
							<MajorGrid LineColor="64, 64, 64, 64" />
						</axisy>
						<axisx LineColor="64, 64, 64, 64">
							<LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
							<MajorGrid LineColor="64, 64, 64, 64" />
						</axisx>
					</asp:ChartArea>
				</chartareas>
            </asp:Chart>
        </div>
    </asp:View>
</asp:MultiView>
