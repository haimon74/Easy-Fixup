<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditSkinColor.ascx.cs"
    Inherits="ezFixUp.Components.Profile.EditSkinColor" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<ajaxToolkit:ColorPickerExtender runat="server"
    TargetControlID="txtColor" 
    PopupButtonID="btnPickColor" 
    />
<table>
    <tr>
        <td>
            <%= "Color:".Translate() %>
        </td>
        <td>
            <asp:TextBox ID="txtColor" runat="server"/>&nbsp;<asp:ImageButton ID="btnPickColor" runat="server" ImageUrl="~/Images/color.png"/>
        </td>
    </tr>
</table>
