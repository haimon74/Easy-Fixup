<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditSkinBackground.ascx.cs"
    Inherits="ezFixUp.Components.Profile.EditSkinBackground" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<ajaxToolkit:ColorPickerExtender runat="server"
    TargetControlID="txtBackgroundColor" 
    PopupButtonID="btnPickColor" 
    />
    
<table>
    <tr>
        <td>
            <%= "Background image:".Translate() %>
        </td>
        <td>
            <asp:FileUpload ID="fileUploadBackgroundImage" runat="server" />
            or url
            <asp:TextBox ID="txtBackgroundImageUrl" runat="server" />
        </td>
    </tr>
    <tr>
        <td>
            <%= "Background color:".Translate() %>
        </td>
        <td>
            <asp:TextBox ID="txtBackgroundColor" runat="server" />
            &nbsp;<asp:ImageButton ID="btnPickColor" runat="server" ImageUrl="~/Images/color.png"/>
        </td>
    </tr>
    <tr>
        <td>
            <%= "Position:".Translate() %>
        </td>
        <td>
            <table>
                <tr>
                    <td>
                        <components:GroupRadioButton ID="rbPositionTopLeft" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <components:GroupRadioButton ID="rbPositionTopCenter" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <components:GroupRadioButton ID="rbPositionTopRight" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <%= "Top".Translate() %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <components:GroupRadioButton ID="rbPositionMiddleLeft" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <components:GroupRadioButton ID="rbPositionMiddleCenter" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <components:GroupRadioButton ID="rbPositionMiddleRight" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <%= "Middle".Translate() %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <components:GroupRadioButton ID="rbPositionBottomLeft" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <components:GroupRadioButton ID="rbPositionBottomCenter" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <components:GroupRadioButton ID="rbPositionBottomRight" GroupName="position" runat="server" />
                    </td>
                    <td>
                        <%= "Bottom".Translate() %>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <%= "Attachment:".Translate() %>
        </td>
        <td>
            <asp:RadioButtonList ID="rblAttachment" runat="server">
                <asp:ListItem />
                <asp:ListItem />
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
        <td>
            <%= "Repeat:".Translate() %>
        </td>
        <td>
            <asp:CheckBoxList ID="cblRepeat" runat="server">
                <asp:ListItem />
                <asp:ListItem />
            </asp:CheckBoxList>
        </td>
    </tr>
</table>
