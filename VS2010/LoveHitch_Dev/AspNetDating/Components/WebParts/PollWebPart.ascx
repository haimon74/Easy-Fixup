<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="PollWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.PollWebPart" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>
<div id="polls">
	<div class="SideMenuBoxContent">
    	<div class="SideMenuBoxInfo">      
            <asp:MultiView ID="mvPolls" ActiveViewIndex="0" runat="server">
            <asp:View ID="vPoll" runat="server">
                <asp:Label ID="lblQuestion" runat="server"></asp:Label>
                <asp:Repeater ID="rptPoll" Runat="server">
                    <ItemTemplate>
                    <div class="text">
                        <asp:HiddenField ID="hidID" Value='<%# Eval("ID") %>' runat="server" />
                        <components:GroupRadioButton ID="rbChoice" GroupName="PollChoices" runat="server" /> <%# Eval("ChoiceValue") %></div>			
                    </ItemTemplate>
                </asp:Repeater>
                <div class="separator"></div>
                <div align="center">
                <uc1:FlexButton ID="fbVote" runat="server" RenderAs="Button" OnClick="fbVote_Click" />
                <uc1:FlexButton ID="fbResults" runat="server" RenderAs="Button" OnClick="fbResults_Click" />
                </div>
            </asp:View>
            <asp:View id="vPollResults" runat="server">
                <asp:Label ID="lblQuestion2" runat="server"></asp:Label>
                <asp:Repeater ID="rptResults" Runat="server">
                    <ItemTemplate>
                        <div class="text">
                        <span class="label"><%# Eval("ChoiceValue") %></span><br>
                        <img src="Images/pollline.gif" height='5px' width='<%# Convert.ToInt32((double)Eval("Percentage")*100) %>px' /> &nbsp; <%# String.Format("{0:P}", Eval("Percentage")) %></div>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="separator"></div>
                <asp:Label ID="lblTotalVotes" runat="server"></asp:Label>
                <uc1:FlexButton ID="fbBack" runat="server" RenderAs="Button" OnClick="fbBack_Click" />
            </asp:View>
            <asp:View ID="vNoPolls" runat="server">
                <div class="center">
                    <%= Lang.Trans("There are no polls available!") %></div>
            </asp:View>
            </asp:MultiView>
        </div>
	</div>    
	<div class="clear"></div>
</div>
<div class="SideMenuWebpartBtm"></div>