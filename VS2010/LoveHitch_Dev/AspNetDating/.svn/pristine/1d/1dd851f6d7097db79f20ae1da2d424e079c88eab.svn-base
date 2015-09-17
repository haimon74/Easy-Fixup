<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true" CodeBehind="UploadPhotos.aspx.cs" Inherits="AspNetDating.Mobile.UploadPhotos" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine"></div>
    <components:ContentView ID="cvNote" Key="UploadPhotosNotes" runat="server"></components:ContentView>
    <asp:DataList ID="dlPhotos" CssClass="editphotos" runat="server" RepeatDirection="Horizontal"
        RepeatColumns="1" CellSpacing="2" CellPadding="2" Width="100%" OnItemCreated="dlPhotos_ItemCreated" OnItemCommand="dlPhotos_ItemCommand">
        <ItemStyle CssClass="overflow BoxWrapStyle"></ItemStyle>
        <ItemTemplate>
        <div class="Separator"></div>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td width="100px" valign="top">
                <img class="photoframe" src="../Image.ashx?id=<%# Eval("PhotoId") %>&width=75&height=75" border="0" />
            </td>
            <td valign="top">
                    <%# Convert.ToBoolean(Eval("Approved"))?"":Lang.Trans("(pending approval)") %>
                    <div class="Clear"></div>
                    <%# Convert.ToBoolean(Eval("Primary"))?Lang.Trans("(primary)"):"" %><div class="Clear"></div>
                    <%# Convert.ToBoolean(Eval("Private"))?Lang.Trans("(private)"):"" %><div class="Clear"></div>
                <span>
                    <asp:LinkButton ID="lnkDeletePhoto" CommandName="DeletePhoto" CommandArgument='<%# Eval("PhotoId") %>'
                            Visible='<%# Convert.ToInt32(Eval("PhotoId")) > 0 %>' runat="server">
				    <%# Lang.Trans("Delete Photo") %></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                </span>
                <span id="pnlPrimary" runat="server" visible='<%# !Convert.ToBoolean(Eval("Primary")) && Convert.ToInt32(Eval("PhotoId")) > 0 %>'>
                    <asp:LinkButton ID="lnkPrimaryPhoto" CommandName="PrimaryPhoto" CommandArgument='<%# Eval("PhotoId") %>'
                        Visible='<%# !Convert.ToBoolean(Eval("Primary")) && Convert.ToInt32(Eval("PhotoId")) > 0 %>'
                        runat="server">
				<%# Lang.Trans("Make Primary") %></asp:LinkButton>
				</span>
            </td>
        </tr>
    </table>                
        <div class="Separator"></div>
        </ItemTemplate>

    </asp:DataList>
    <asp:Panel ID="pnlEditImage" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td width="100px" valign="top">
             <img class="photoframe" id="tempPhoto" src="../Image.ashx?id=session&width=90&height=90&seed=<%= new Random().NextDouble().ToString() %>"
                        border="0" />
            </td>
            <td valign="top">
                    <asp:CheckBox ID="chkPrivatePhoto" runat="server" Visible="false"></asp:CheckBox>   <div class="Separator"></div>          
                <div class="srvmsg">
                    <asp:Label ID="lblError" CssClass="error" EnableViewState="False" runat="server" /></div>
                <div id="divFileUploadControls" runat="server">
                    <asp:Label CssClass="label" ID="lblPhoto" runat="server" /><br />
                    <asp:FileUpload ID="ufPhoto" runat="server" />
                    <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click"></asp:Button>
                </div>            
            </td>
        </tr>
        <tr>
        	<td colspan="2" align="center"> 
                       <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click"></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"></asp:LinkButton>                 
            </td>
        </tr>
    </table>
          <div class="Separator"></div> 
</asp:Panel>
</asp:Content>
