<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="CreateGroup.aspx.cs" Inherits="ezFixUp.CreateGroup" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">

    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCountry" runat="server" TargetControlID="dropCountry"
        Category="Country" ServicePath="~/Services/Service.asmx" ServiceMethod="GetCountries" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownState" runat="server" TargetControlID="dropRegion"
        Category="Region" ServicePath="~/Services/Service.asmx" ServiceMethod="GetRegionsByCountry"
        ParentControlID="dropCountry" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity" runat="server" TargetControlID="dropCity"
        Category="City" ServicePath="~/Services/Service.asmx" 
        ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="dropRegion" />
        
	<div id="sidebar"><uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
		<div class="SideMenuItem"><asp:linkbutton id="lnkBrowseGroups" Runat="server" CssClass="SideMenuLink" OnClick="lnkBrowseGroups_Click"></asp:linkbutton>
		</div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
    </div>
    <div id="main">
        <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
        <components:ContentView ID="cvCreateGroupRequirement" Key="CreateGroupRequirements" runat="server">
        </components:ContentView>
        <div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
        <div class="clear"></div>        
        <br />
        <%= Lang.Trans("Name") %><br/>
        <asp:TextBox ID="txtName" CssClass="titletextbox" runat="server"></asp:TextBox><br />
        <div class="separator"></div>
        <%= Lang.Trans("Categories") %><br/>
        <asp:ListBox ID="lbCategories" runat="server" CssClass="listbox" SelectionMode="Multiple"></asp:ListBox>
        <br />
        <div class="hint">*<%= Lang.Trans("Hold Ctrl for multiple selection") %></div>
        <%= Lang.Trans("Description") %><br/>
        <asp:TextBox ID="txtDescription" runat="server" CssClass="multilinetextbox" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
        <br />
        <div class="separator"></div>
        <%= Lang.Trans("Terms & Conditions") %><br/>
        <asp:TextBox ID="txtTerms" CssClass="multilinetextbox" TextMode="MultiLine" runat="server"></asp:TextBox><br />        
        <div class="separator"></div>
        <table cellpadding="0" cellspacing="0">
        	<tr>
        		<td>
        			<%= Lang.Trans("Group Image") %><div class="separator"></div>
        		</td>
 	       		<td class="pleft5">
 	       			<asp:FileUpload ID="fuGroupImage" runat="server"/>
        			<div class="separator"></div>
        		</td>
        	</tr>
        	<tr>
        		<td>
        			<%= Lang.Trans("Access Level") %></td>
 	       		<td class="pleft5">
 	       			<asp:DropDownList ID="ddAccessLevel" AutoPostBack="true" CssClass="dropdownlist" runat="server" OnSelectedIndexChanged="ddAccessLevel_SelectedIndexChanged"></asp:DropDownList> 	       			
 	       					        <div class="separator"></div>
        		</td>
        	</tr>
        	<tr>
 	       		<td valign="middle">
   			        <%= Lang.Trans("Minimum age restriction")%></td>
   			    <td class="pleft5">
   			        <asp:DropDownList ID="ddAgeRestriction" CssClass="dropdownlist" runat="server"></asp:DropDownList>
        		</td>
        	</tr>
        	<tr id="pnlAutomaticallyJoin" runat="server" Visible="false">
 	       		<td>
   			        <%= Lang.Trans("Users automatically join group")%></td>
   			    <td class="pleft5">
   			        <asp:CheckBox ID="cbAutomaticallyJoin" runat="server" AutoPostBack="true"
                        oncheckedchanged="cbAutomaticallyJoin_CheckedChanged" />
        		</td>
        	</tr>
        	<tr id="trCountry" runat="server" visible="false">
				<td><%= Lang.Trans("Country") %></td>
				<td class="pleft5">
					<asp:DropDownList ID="dropCountry" CssClass="dropdownlist" runat="server"></asp:DropDownList>
					<div class="separator"></div>
				</td>
			</tr>
			<tr id="trState" runat="server" visible="false">
				<td><%= Lang.Trans("Region/State") %></td>
				<td class="pleft5">
    				<asp:DropDownList ID="dropRegion" CssClass="dropdownlist" runat="server">
                    	<asp:ListItem Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <div class="separator"></div>
				</td>
			</tr>
			<tr id="trCity" runat="server" visible="false">
				<td><%= Lang.Trans("City") %></td>
				<td class="pleft5">
               		<asp:DropDownList ID="dropCity" CssClass="dropdownlist" runat="server">
                    	<asp:ListItem Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <div class="separator"></div>
				</td>
			</tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
            <ContentTemplate>
                <div id="pnlQuestion" visible="false" runat="server">
                    <br />
                    <%= Lang.Trans("Enter a question to ask the user when joining the group") %><br/>
                    <asp:TextBox ID="txtQuestion" runat="server" CssClass="multilinetextbox" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                    <br />
                    <div class="hint">*<%= Lang.Trans("You may leave this field blank") %></div>            
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddAccessLevel" EventName="SelectedIndexChanged" />
            </Triggers>
        </asp:UpdatePanel>        
        <div class="separator"></div>
        <div class="buttons">
            <asp:Button ID="btnCreateGroup" runat="server" OnClick="btnCreateGroup_Click" />
            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Button" />
		</div>
    	<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
	</div>
</asp:Content>
