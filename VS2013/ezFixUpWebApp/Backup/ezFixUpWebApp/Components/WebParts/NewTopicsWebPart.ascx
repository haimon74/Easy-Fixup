<%@ Import namespace="ezFixUp"%>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="NewTopicsWebPart.ascx.cs" 
            Inherits="ezFixUp.Components.WebParts.NewTopicsWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Import namespace="ezFixUp.Classes"%>
<asp:MultiView ID="mvNewTopics" ActiveViewIndex="0" runat="server">
<asp:View ID="vNewTopics" runat="server">
<asp:Repeater ID="rptNewGroupTopics" runat="server" Visible="false">
    <ItemTemplate>
    	<table cellpadding="0" cellspacing="0" width="100%">
    		<tr>
    			<td width="70" valign="top" align="center">
	    			<div class="overflow" style="width:70px">
						<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'>
		                    <%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
		                <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a></div>
		        </td>
    			<td valign="top">
    				<span class="label"><%= Lang.Trans("Group") %></span>:&nbsp;<%# Eval("GroupName") %><br>
                    <span class="label"><%= Lang.Trans("Topic") %></span>:&nbsp;<a href='<%# UrlRewrite.CreateShowGroupTopicsUrl((string) Eval("GroupID"), (string) Eval("GroupTopicID")) %>'><%# Eval("GroupTopicName") %></a><div class="clear"></div>
                    <div class="info left"><%= Lang.Trans("Date created") %>:&nbsp;<%# Eval("GroupTopicDateCreated")%></div></td>
    		</tr>
    	</table>
    </ItemTemplate>
        <separatortemplate>
        	<div class="separator6"></div>
        </separatortemplate>    
</asp:Repeater>
</asp:View>
<asp:View ID="vNoNewTopics" runat="server">
    <div class="center">
   		<div class="separator"></div>
	    <%= Lang.Trans("There are no new group topics!") %><div class="separator"></div>
    </div>
</asp:View>
</asp:MultiView>
<div class="StandardWebpartBtm"></div>