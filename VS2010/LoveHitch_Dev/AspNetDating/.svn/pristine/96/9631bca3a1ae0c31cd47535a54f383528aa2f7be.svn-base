<%@ Import namespace="AspNetDating"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPosts.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewPosts" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="GroupPoll" Src="~/Components/Groups/GroupPoll.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="False"></asp:Label></div>
<div class="headerfix"><b><%= Lang.Trans("Topic") %>:</b>&nbsp;<asp:Label ID="lblTopicName" runat="server"></asp:Label></div>
<uc1:GroupPoll id="GroupPoll1" runat="server"></uc1:GroupPoll>
<div class="clear"></div>
<div class="separator"></div>
<asp:MultiView ID="mvPosts" runat="server">
<asp:View ID="viewPosts" runat="server">
    <asp:DataList ID="dlGroupPosts" runat="server" HorizontalAlign="Center" 
    Width="100%" ShowHeader="false" CellPadding="0" OnItemCommand="dlGroupPosts_ItemCommand" 
    OnItemDataBound="dlGroupPosts_ItemDataBound" EnableViewState="false">
        <ItemTemplate>
        <table cellpadding="2" cellspacing="2" width="100%">
        	<tr>
        		<td width="60" valign="top" align="center">
        		<div class="overflow" style="width:60px">
		           <a href='ShowUser.aspx?uid=<%# Eval("Username") %>' ><%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
		               <table cellpadding="0" cellspacing="0">
		               	<tr>
		               		<td valign="middle" height="24px">
		               <%# (bool)Eval("IsWarned") ? String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", Config.Urls.ImagesHome + "/warned.png", String.Format("Warned:{0}".Translate(), Eval("WarnReason"))) : String.Empty%></td>
		               <td valign="middle">
		               &nbsp;<a class="user-name" href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# Eval("Username") %></a></td>
		               	</tr>
		               </table>
		               <div class="clear"></div>
					    <%# (!(bool) Eval("HideUserLevelIcon")) && Eval("UserLevel") is UserLevel ?
					        String.Format("<img src=\"{0}\" border=\"0\" title=\"{1} - {2}\" />",
                                                        ((UserLevel)Eval("UserLevel")).GetIconUrl(), String.Format(Lang.Trans("Level {0}"), ((UserLevel)Eval("UserLevel")).LevelNumber), ((UserLevel)Eval("UserLevel")).Name) : ""%></div>
        		</td>
        		<td>
            		<div class="info left"><%= Lang.Trans("Posted on") %>:&nbsp;<%# Eval("DatePosted")%></div>
            <div class="list right">
			     <ul>
                    <li id="liReply" class="left" runat="server">
					    <asp:LinkButton ID="lnkReply" CommandName="Reply" CommandArgument='<%# Eval("GroupPostID") %>' Runat="server">
					    </asp:LinkButton>
				    </li>
				    <li id="liEdit" class="left" runat="server">
				        <asp:LinkButton ID="lnkEdit"  CommandName="Edit" CommandArgument='<%# Eval("GroupPostID") %>' Runat="server">
				        </asp:LinkButton>
				    </li>
				    <li id="liDelete" class="left" runat="server">
		        	    <asp:LinkButton ID="lnkDelete" CommandName="Delete" CommandArgument='<%# Eval("GroupPostID") %>' Runat="server">
					    </asp:LinkButton>
				    </li>
                </ul>
             </div>
			 <br>
			 <div class="clear"></div>
			 <div class="justify overflow contentfix">
             	<asp:Literal ID="ltrPost" runat="server" Text='<%# Eval("Post") %>'></asp:Literal>
             </div>
             <div class="separator6"></div>
             	<div id="pnlDateEdited" class="info italic" runat="server">
             	   <%= Lang.Trans("Edited on") %>:&nbsp;<%# Eval("DateEdited")%>&nbsp;
                </div><br />
             	<div id="pnlEditNotes" class="info italic" runat="server">
             	   <%= Lang.Trans("Reason to edit") %>:&nbsp;<%# Eval("EditNotes")%>
                </div>
       	   </td>
				</tr>
	        </table>
        </ItemTemplate>
    	<SeparatorTemplate>
			<div class="line"></div>
    	</SeparatorTemplate>
    </asp:DataList>
    <div class="line"></div>
    <asp:Panel ID="pnlPaginator" runat="server">
	    <div class="endbox">
            <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"></asp:LinkButton>
            <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
            <asp:Label ID="lblPager" runat="server"></asp:Label>
            <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
            <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
        </div>
    </asp:Panel>
</asp:View>
<asp:View ID="viewDeleteOptions" runat="server">
    <%= Lang.Trans("What do you want to do?") %><div class="separator6"></div>
    <asp:RadioButtonList ID="rbList" runat="server" />
    <div class="separator"></div>
    <asp:UpdatePanel ID="UpdatePanelWarn" runat="server">
        <ContentTemplate>
            <ajaxToolkit:TextBoxWatermarkExtender ID="TBWEReason" runat="server"
                                TargetControlID="txtWarnReason"
                                WatermarkCssClass="watermarked" />
            <asp:CheckBox ID="cbKickMember" CssClass="lineh" runat="server" 
                AutoPostBack="true" oncheckedchanged="cbKickMember_CheckedChanged"/>
            <br />
            <asp:CheckBox ID="cbBanMember" CssClass="lineh"  runat="server" AutoPostBack="true" oncheckedchanged="cbBanMember_CheckedChanged"/>
            <asp:DropDownList ID="ddBanPeriod" CssClass="dropdownlist" runat="server"></asp:DropDownList>
            <div id="pnlWarn" runat="server">
                <asp:CheckBox ID="cbWarn" runat="server" CssClass="lineh" AutoPostBack="true" oncheckedchanged="cbWarn_CheckedChanged"/>
                <asp:DropDownList ID="ddWarnExpirationDate"  CssClass="dropdownlist" runat="server"></asp:DropDownList>
                <br />
                <asp:TextBox ID="txtWarnReason" runat="server" CssClass="textbox" Visible="false"></asp:TextBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="separator"></div>
    <div class="buttons">
	    <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
	    <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
    </div>
    </asp:View>
</asp:MultiView>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>