<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Groups.aspx.cs" Inherits="ezFixUp.Groups" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="~/Components/Groups/SearchResults.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
		<div class="SideMenuItem"><asp:linkbutton id="lnkBrowseGroups" CssClass="SideMenuLink" Runat="server" OnClick="lnkBrowseGroups_Click"></asp:linkbutton>
		</div>
		<div id="pnlMyGroups" runat="server" class="SideMenuItem"><asp:linkbutton id="lnkMyGroups" CssClass="SideMenuLink" Runat="server" OnClick="lnkMyGroups_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem"><asp:linkbutton id="lnkNewGroups" CssClass="SideMenuLink" Runat="server" OnClick="lnkNewGroups_Click"></asp:linkbutton>
		</div>
		<div id="pnlPendingInvitations" runat="server" class="SideMenuItem"><asp:linkbutton id="lnkPendingInvitations" CssClass="SideMenuLink" Runat="server" OnClick="lnkPendingInvitations_Click"></asp:linkbutton>
		</div>
		<div id="pnlCreateGroup" runat="server" class="SideMenuItem"><asp:linkbutton id="lnkCreateGroup" CssClass="SideMenuLink" Runat="server" OnClick="lnkCreateGroup_Click"></asp:linkbutton>
		</div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
	</div>
	<div id="main">
	<asp:UpdatePanel ID="UpdatePanelGroups" runat="server">
	<ContentTemplate>
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
		<components:BannerView id="bvGroupsRightTop" runat="server" Key="GroupsRightTop"></components:BannerView>
		<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
			<asp:MultiView ID="mvGroups" runat="server">
			    <asp:View ID="viewCategories" runat="server">
			        <asp:Panel ID="pnlFilterGroup" DefaultButton="btnSearchGroup" runat="server">
                    	<div class="UserFilter">
			            <%= Lang.Trans("Find a group") %>:
			            <asp:TextBox ID="txtGroupToSearch" runat="server"></asp:TextBox>
			            <asp:Button ID="btnSearchGroup" runat="server" OnClick="btnSearchGroup_Click" />
			            <div class="separator6"></div>
			            <asp:CheckBox ID="cbSearchInDescription" runat="server" />
			            <%= Lang.Trans("Search in description") %>
						</div>
			        </asp:Panel>
			        <asp:DataList ID="dlCategories" SkinID="Categories" GridLines="None" Width="100%"
			         runat="server" CellPadding="5" CellSpacing="0" OnItemCommand="dlCategories_ItemCommand">
			         <ItemStyle Width="50%" VerticalAlign="Top" HorizontalAlign="Left"></ItemStyle>
			            <ItemTemplate>
			                <asp:LinkButton id="lnkViewGroups" CssClass="links" runat="server" CommandName="ViewGroups" CommandArgument='<%# Eval("CategoryID") %>'><%# Eval("Name") %></asp:LinkButton>&nbsp;(<%# Eval("GroupsNumber")%>)
					 </ItemTemplate>    
			        </asp:DataList>
			    </asp:View>
			    <asp:View ID="viewGroups" runat="server">
			        <div id="pnlSearchParameters" runat="server">
			            <asp:Panel ID="pnlFilterGroupByCategory" DefaultButton="btnSearchGroupByCategory" runat="server">
			                <%= Lang.Trans("Name") %>:
			                <asp:TextBox ID="txtSearchGroupByCategory" runat="server"></asp:TextBox>
			                <asp:Button ID="btnSearchGroupByCategory" runat="server" OnClick="btnSearchGroupByCategory_Click" />
				            <div class="separator6"></div>
			                <asp:CheckBox ID="cbSearchInDescriptionByCategory" runat="server" />
			                <%= Lang.Trans("Search in description") %>
			            </asp:Panel>
      			    <div class="separator6"></div>
			        <div class="line"></div>
      			    <div class="separator6"></div>
          			</div> 
			        <asp:DataList ID="dlGroups" Width="100%" runat="server" OnItemCreated="dlGroups_ItemCreated" OnItemCommand="dlGroups_ItemCommand">
			            <ItemTemplate>
			            	<table cellpadding="0" cellspacing="0" width="100%">
			            		<tr>
			            			<td valign="top" width="94">
			            				<a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'>
			                        		<img class="photoframe" src='GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=90&height=90' border="0"/>
			                    		</a>
			            			</td>
			            			<td valign="top" class="pleft5">
                    					<a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>'><%# Eval("Name") %></a>&nbsp;(<%# Eval("AccessLevel")%>)&nbsp;<b><%# Eval("MemberType")%></b>&nbsp;<b><%# Eval("Pending") %></b>
		                    			<br />
			                    		<div class="justify" >
			                    			<%# Eval("Description") %><a href='<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>' id="lnkDescriptionMore" runat="server" visible='<%# Eval("IsVisibleLinkMore") %>'><%# Lang.Trans("more") %></a><br />
			                    		</div>
			                    		<%# Lang.Trans("Members") %>:&nbsp;<b><%# Eval("MembersCount")%></b>     
			                    		<div class="date">
			                    			<%# Lang.Trans("Created") %>&nbsp;<%# Eval("DateCreated")%>     
			                    		</div>
			                    		<div class="clear"></div>
			                    		<div id="pnlPendingActions" class="itemlinks" runat="server" visible='<%# ShowPnlPendingActions %>'>
											<ul>
												<li>	
							                        <asp:LinkButton ID="lnkAccept" runat="server" CommandArgument='<%# Eval("GroupID") %>' CommandName="Accept"/>
												</li>
												<li>
							                        <asp:LinkButton ID="lnkReject" runat="server" CommandArgument='<%# Eval("GroupID") %>' CommandName="Reject"/>
												</li>
											</ul>
										</div>
			            			</td>
			            		</tr>
			            	</table>
			            </ItemTemplate>	   
			            <SeparatorTemplate>
		                    <div class="line"></div>
		                    <div class="separator6"></div>
			            </SeparatorTemplate>        
			        </asp:DataList>
                    <div class="clear"></div>
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
			    <asp:View ID="viewGroupSearchResults" runat="server">
			        <uc1:SearchResults id="SearchResults1" runat="server"></uc1:SearchResults>
			    </asp:View>
			</asp:MultiView>
			<components:BannerView id="bvGroupsRightBottom" runat="server" Key="GroupsRightBottom"></components:BannerView>
			<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
			</ContentTemplate>
			</asp:UpdatePanel>
	</div>
</asp:Content>
