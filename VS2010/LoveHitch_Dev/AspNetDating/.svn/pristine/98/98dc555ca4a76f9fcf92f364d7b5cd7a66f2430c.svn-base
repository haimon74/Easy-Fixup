<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditPhotosCtrl.ascx.cs" Inherits="AspNetDating.Admin.EditPhotosCtrl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
<asp:DataList id="dlPhotos" runat="server" RepeatDirection="Horizontal" RepeatColumns="1" CellSpacing="0" CellPadding="0" Width="100%">
	<ItemStyle VerticalAlign="Top" />
		<ItemTemplate>
			<table cellSpacing="0" cellPadding="0" id="stable" border="0">
				<tr>
					<td colspan="2" class="table_header">&nbsp;</td>
			    </tr>
			    <tr>
					<td class="editphotos-img" align="center">
						<table cellSpacing="0" cellPadding="0" border="0">
							<tr>
								<td vAlign="middle" align="center">
									<a href="<%= Config.Urls.Home%>/Image.ashx?id=<%# Eval("PhotoID") %>&seed=<%= new Random().NextDouble().ToString() %>" target="_new">
									<img src="<%= Config.Urls.Home%>/Image.ashx?id=<%# Eval("PhotoID") %>&width=120&height=120&seed=<%= new Random().NextDouble().ToString() %>" border="0"></a>
					            </td>
						    </tr>
						</table>
		        	</td>
					<td class="editphotos-wrap">
					<!--<asp:Label ID="lblError" ForeColor="Red" EnableViewState="False" Font-Bold="True" Runat="server" />-->
						<table cellpadding="3" cellspacing="0">
							<tr>
								<td>
									<asp:Label id="lblName" Runat="server">
									<%# Lang.TransA("Name") %></asp:Label>:
								</td>
							    <td>
									<asp:TextBox id="txtName" Runat="server" Width="250px" MaxLength="80" Text='<%# Eval("Name") %>'>
								    </asp:TextBox>
								</td>
							</tr>
					        <tr>
						        <td>
							        <asp:Label id="lblDescription" Runat="server">
								        <%# Lang.TransA("Description")%></asp:Label>:
								</td>
							    <td>
									<asp:TextBox id="txtDescription" Runat="server" Width="250px" MaxLength="200" TextMode="MultiLine" Text='<%# Eval("Description") %>'>
								    </asp:TextBox>
								</td>
							</tr>
					        <tr>
						        <td>
							        <asp:Label ID="lblPhoto" Runat="server">
									<%# Lang.TransA("Photo")%></asp:Label>:
							    </td>
							    <td>
									<input id="ufPhoto" type="file" runat="server" NAME="ufPhoto" /> 
							    </td>
						    </tr>
						</table>
					    <asp:LinkButton id="btnUpload" CommandName="UploadPhoto" CommandArgument='<%# ((string)(Eval("PhotoId")) == "0")?NewTempID:Eval("PhotoId") %>' Runat="server" Text = '<%# AspNetDating.Classes.Lang.TransA("Upload")%>'>
					    </asp:LinkButton>
					    <div class="separator06"></div>
					    <input id="hidPictureID" type="hidden" value='<%# ((string)(Eval("PhotoId")) == "0")?CurrentTempID:Eval("PhotoId") %>' runat="server" NAME="hidPictureID" />
					</td>
		        </tr>		
		        <tr>
					<td class="editphotos-links" colspan="2">
						<asp:CheckBox ID="chkExplicitPhoto" Runat="server" Text='<%# AspNetDating.Classes.Lang.TransA("Explicit Photo")%>' Checked='<%# Eval("ExplicitPhoto") %>' Visible='<%# (AspNetDating.Classes.Config.Photos.EnableExplicitPhotos && ((string)Eval("PhotoId") != "0"))%>'></asp:CheckBox>
					    <%#  (AspNetDating.Classes.Config.Photos.EnableExplicitPhotos && ((string)Eval("PhotoId") != "0"))?"|":""%>					
					    <asp:LinkButton id="lnkSetPrimary" CommandName="SetPrimary" Runat="server" Visible='<%# !Convert.ToBoolean(Eval("Primary"))&&((string)Eval("PhotoId")) != "0" %>' CommandArgument='<%# Eval("PhotoId")%>'><%# Lang.TransA("Set Primary")%></asp:LinkButton>
					    <%# (!Convert.ToBoolean(Eval("Primary"))&&((string)Eval("PhotoId")) != "0" )?"|":""%>
					    <asp:LinkButton id="lnkDelete" CommandName="Delete" Runat="server" Visible='<%# ((string)Eval("PhotoId")) != "0" %>' CommandArgument='<%# Eval("PhotoId")%>'><%# Lang.TransA("Delete")%></asp:LinkButton>
					 </td>
				</tr>
			</table>
	  </ItemTemplate>
</asp:DataList>
<div class="add-buttons2">
    <div class="add-buttons">
    	<asp:Button ID="btnCancel" runat="server"></asp:Button>
    	<asp:Button ID="btnSave" runat="server"></asp:Button>	
    </div>
</div>
<div class="separator10"></div>
<input id="hidUsername" type="hidden" name="hidUsername" runat="server" />