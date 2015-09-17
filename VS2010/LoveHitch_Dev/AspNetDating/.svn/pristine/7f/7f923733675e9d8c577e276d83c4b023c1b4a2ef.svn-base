<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="EditPhotos.ascx.cs"
    Inherits="AspNetDating.Components.Profile.EditPhotos" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<input type="hidden" id="hidUsername" runat="server" />
<uc1:HeaderLine ID="PostingPhotosHeaderLine" runat="server" />
<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="updatePanelNotes" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
    <ContentTemplate>
        <div class="note fl">
            <components:ContentView ID="cvNote" Key="UploadPhotosNotes" runat="server"></components:ContentView>
        </div>
        <asp:MultiView ID="mvPhotoAlbum" runat="server">
            <asp:View ID="viewPhotoAlbums" runat="server">
                <div align="center">
                    <%= "Upload to".Translate() %>:&nbsp;<asp:DropDownList CssClass="dropdownlist" ID="ddPhotoAlbums"
                        runat="server" OnSelectedIndexChanged="ddPhotoAlbums_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <br />
                    <ul class="AlbumManagement">
                        <li>
                            <asp:LinkButton ID="lnkCreateNewAlbum" runat="server" OnClick="lnkCreateNewAlbum_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkEditAlbum" runat="server" OnClick="lnkEditAlbum_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkDeleteAlbum" runat="server" OnClick="lnkDeleteAlbum_Click"></asp:LinkButton></li>
                    </ul>
                </div>
            </asp:View>
            <asp:View ID="viewCreatePhotoAlbum" runat="server">
                <%= "Album name".Translate() %>&nbsp;<asp:TextBox CssClass="textbox" ID="txtPhotoAlbumName"
                    runat="server"></asp:TextBox>
                <div class="separator">
                </div>
                <%= "Who can view this album?" %>&nbsp;<asp:DropDownList CssClass="dropdownlist"
                    ID="ddPhotoAlbumAccess" runat="server">
                </asp:DropDownList>
                <div class="separator">
                </div>
                <asp:Button ID="btnCreateEditPhotoAlbum" runat="server" OnClick="btnCreateEditPhotoAlbum_Click" />
                <asp:Button ID="btnCancelCreatePhotoAlbum" runat="server" OnClick="btnCancelCreatePhotoAlbum_Click" />
                <div class="separator">
                </div>
            </asp:View>
        </asp:MultiView>
        <asp:Panel ID="pnlManagePhotos" runat="server">
            <div class="separator">
            </div>
            <asp:DataList ID="dlPhotos" CssClass="editphotos" runat="server" RepeatDirection="Horizontal"
                RepeatColumns="2" CellSpacing="2" CellPadding="2" Width="100%">
                <ItemStyle CssClass="overflow BoxWrapStyle" Width="48%"></ItemStyle>
                <ItemTemplate>
                    <table cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td rowspan="2" valign="top" width="125px" headers="125px" align="center">
                                <img class="photoframe" src="<%# Eval("PhotoUrl") %>"
                                    title="<%# Eval("Description") %>" border="0" alt="user photo" />
                            </td>
                            <td valign="top">
                            <table>
                              <tr><td>
                                &nbsp;<b><%# Eval("Name") %></b><div class="clear">
                                </div>
                                <%# Convert.ToBoolean(Eval("Approved"))?"":Lang.Trans("(pending approval)") %>
                                <div class="clear">
                                </div>
                                <%# Convert.ToBoolean(Eval("Primary"))?Lang.Trans("(primary)"):"" %>
                                <div class="clear">
                                </div>
                                <%# Convert.ToBoolean(Eval("Private"))?Lang.Trans("(private)"):"" %>
                                <div class="clear">
                                </div>
                              </td></tr>
                            
                             <tr><td valign="bottom">
                                <div class="line">
                                </div>
                                <div class="EditPhotoLinks">
                                    <asp:LinkButton ID="lnkUploadPhoto" runat="server" CssClass="medium zurbtn blue" CommandName="UploadPhoto"
                                        CommandArgument='<%# Eval("PhotoId") %>'>
                                <%# (Convert.ToInt32(Eval("PhotoId")) > 0 ? "Edit" : "Add New").Translate() %><br />
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkDeletePhoto" runat="server" CssClass="medium zurbtn red" CommandName="DeletePhoto"
                                        CommandArgument='<%# Eval("PhotoId") %>' Visible='<%# Convert.ToInt32(Eval("PhotoId")) > 0 %>'>
                                <%# Lang.Trans("Delete") %><br />
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkPrimaryPhoto" runat="server" CssClass="medium zurbtn blue" CommandName="PrimaryPhoto"
                                        CommandArgument='<%# Eval("PhotoId") %>' Visible='<%# !Convert.ToBoolean(Eval("Primary")) && Convert.ToInt32(Eval("PhotoId")) > 0 %>'>
                                <%# Lang.Trans("Set Primary") %><br />
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkAddNotes" runat="server" CssClass="medium zurbtn blue" CommandName="AddNotes"
                                        CommandArgument='<%# Eval("PhotoId") %>' Visible='<%# Config.Photos.EnablePhotoNotes && Convert.ToInt32(Eval("PhotoId")) > 0 && !Request.IsIE6() %>'>
                                <%# Lang.Trans("Add Tag") %>
                                    </asp:LinkButton>
                                </div>
                            </td></tr>
                          </table>
                          </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <div id="divUploadMultiplePhotosFlash" class="center" runat="server">
                <div class="separator">
                </div>
                [&nbsp;<asp:LinkButton ID="lnkUploadMultipleFlash" runat="server" OnClick="lnkUploadMultipleFlash_Click"></asp:LinkButton>&nbsp;]
            </div>
            <div id="divUploadMultiplePhotosSilverlight" class="center" runat="server">
                <div class="separator">
                </div>
                [&nbsp;<asp:LinkButton ID="lnkUploadMultipleSilverlight" runat="server" OnClick="lnkUploadMultipleSilverlight_Click"></asp:LinkButton>&nbsp;]
            </div>
            <div id="divUploadViaWebCam" runat="server" class="center">
                <div class="separator">
                </div>
                [&nbsp;<asp:LinkButton ID="lnkUploadViaWebCam" runat="server" OnClick="lnkUploadViaWebCam_Click"></asp:LinkButton>&nbsp;]
            </div>
        </asp:Panel>
        <div class="center">
            <asp:Panel ID="pnlUploadMultiplePhotosFlash" runat="server" Visible="false">
                <components:FlashUpload ID="flashUpload" runat="server" OnUploadComplete="flashUploadIsCompleted()"
                    UploadPage="../../FlashUpload.ashx" FileTypeDescription="Images" FileTypes="*.gif; *.png; *.jpg; *.jpeg"
                    UploadFileSizeLimit="5242880" TotalUploadSizeLimit="20971520" />
            </asp:Panel>
            <div id="pnlUploadMultiplePhotosSilverlight" runat="server" visible="false">
                <asp:Silverlight ID="Silverlight1" runat="server" Source="~/ClientBin/MultiFileUploader.xap"
                    MinimumVersion="2.0.31005.0" Width="400" Height="300">
                </asp:Silverlight>
            </div>
            <div id="pnlUploadViaWebCam" runat="server" visible="false">
                <components:WebcamUpload ID="webcamUpload" runat="server" OnUploadComplete="webcamUploadIsCompleted()"
                    Width="400" Height="400" FileTypeDescription="Images" FileTypes="*.gif; *.png; *.jpg; *.jpeg"
                    UploadFileSizeLimit="5242880" TotalUploadSizeLimit="20971520" />
            </div>
            <div class="separator">
            </div>
            <asp:LinkButton ID="lnkBackToPhotos" runat="server" Visible="false" OnClick="lnkBackToPhotos_Click"></asp:LinkButton>
        </div>
        <asp:Panel ID="pnlEditImage" Visible="False" runat="server">
            <table cellpadding="2" cellspacing="0" class="BoxWrapStyle blockTable" width="98%" style="text-indent:3px;margin:0 auto;">
                <tr style="height: 20px;">
                    <td colspan="3">
                    </td>
                </tr>
                <tr>
                    <td valign="bottom" class="w30" >
                        <asp:Label CssClass="label" ID="lblPhoto" runat="server" />
                    </td>
                    <td align="center" class="w40" valign="bottom">
                        <div id="divFileUploadControls" runat="server" style="display:inline-block;" class="fl">
                            <img class="photoframe" id="tempPhoto" border="0" style="display: none;" alt="tmp photo"
                                src="Image.ashx?id=session&width=150&height=150&seed=<%= new Random().NextDouble().ToString() %>" />
                            <div id="updDiv" class="updDiv fl" style="width: 90px; height: 90px; text-align: center;">
                                <img id="loadedImage" alt="Avatar" src="images/empty.gif" style="clear: left; float: left;
                                    width: 90px; height: 90px;" />
                            </div>
                            <div class="fl">
                                <asp:Button ID="btnUpload" runat="server" Style="display: none" OnClick="btnUpload_Click">
                                </asp:Button>
                                <button id="btnBrowseTrigger" onclick="javascript: triggerAsyncImgUpload();return false;"
                                    class="medium zurbtn blue" style="position: relative;padding:3px; top: 65px;">
                                    <%= "Browse".Translate() %></button>
                                <ajaxToolkit:AsyncFileUpload OnClientUploadError="uploadError" OnClientUploadComplete="uploadComplete"
                                    runat="server" ID="asyncImageUpload" Width="150px" UploaderStyle="Traditional"
                                    UploadingBackColor="lightskyblue" ThrobberID="myThrobber" CssClass="fl" Style="margin-top: 60px;
                                    opacity: 0; position: absolute; left: -150px;" />
                                &nbsp;
                                <asp:Label runat="server" ID="myThrobber" Style="display: none;">
                                    <%--<img align="middle" alt="" src="images/uploading.gif" />--%>
                                    <asp:Image ID="imgLoadingProgress" ImageUrl="~/images/loaderAnimation.gif" Width="30"
                                                    Height="30" runat="server" />
                                </asp:Label>
                                <asp:FileUpload ID="ufPhoto" Style="display: none" runat="server" />
                            </div>
                        </div>
                        <div id="divImageRotateControls" visible="false" runat="server">
                            <asp:Button ID="btnRotateLeft" runat="server" OnClick="btnRotateLeft_Click" />
                            &nbsp;
                            <asp:Button ID="btnRotateRight" runat="server" OnClick="btnRotateRight_Click" />
                        </div>
                        <br />
                        <asp:CheckBox ID="chkPrivatePhoto" runat="server"></asp:CheckBox>
                    </td>
                    <td valign="bottom" style="width: 90px;">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="srvmsg">
                            <asp:Label ID="lblError" CssClass="error" EnableViewState="False" runat="server" />
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr id="pnlPhotoAlbum" runat="server">
                    <td valign="top">
                        <span class="label">
                            <%= Lang.Trans("Album") %>:</span>
                    </td>
                    <td>
                        <asp:Label ID="lblPhotoAlbumName" runat="server"></asp:Label>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="w30">
                        <span class="label">
                            <%= Lang.Trans("Name") %>:</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" CssClass="textbox fl" Width="200px" runat="server" MaxLength="80"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td valign="top"  class="w30">
                        <span class="label">
                            <%= Lang.Trans("Description")%>:</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescription" CssClass="multilinetextbox fl" Width="200px" runat="server"
                            MaxLength="200" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:PlaceHolder ID="plhPhotoFaces" Visible="false" runat="server">
                            <div class="line">
                            </div>
                            <div class="text">
                                <%= Lang.Trans("Which photo crop shows your face?") %></div>
                            <asp:DataList ID="dlPhotoFaces" CssClass="choosephoto" runat="server" RepeatColumns="4"
                                RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing="0" CellPadding="0">
                                <ItemTemplate>
                                    <div class="center">
                                        <img src="<%# "Image.ashx?id=session&width=90&height=90&findFace=1&faceX=" + Eval("X") + "&faceY=" + Eval("Y") + "&faceW=" + Eval("Width") + "&faceH=" + Eval("Height") %>"
                                            class="photoframe" alt="" /><br />
                                        <components:GroupRadioButton ID="rbFace" GroupName="faces" runat="server" />
                                        <asp:HiddenField ID="hidFace" Value='<%# Eval("X") + "|" + Eval("Y") + "|" + Eval("Width") + "|" + Eval("Height") %>'
                                            runat="server" />
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                            <div class="clear">
                            </div>
                            <div class="buttons">
                                <components:GroupRadioButton ID="rbNoFace" GroupName="faces" runat="server"></components:GroupRadioButton>
                                <%= Lang.Trans("None of the above") %></div>
                        </asp:PlaceHolder>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center"  style="display:block;">
                        <div class="separator6">
                        </div>
                        <div class="line">
                        </div>
                        <div class="separator6">
                        </div>
                        <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click" CssClass="medium zurbtn blue"
                            Width="65"></asp:LinkButton>&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click" CssClass="medium zurbtn red"
                            Width="65"></asp:LinkButton>
                        <div class="separator">
                        </div>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlCropImage" Visible="false" runat="server">
            <table cellpadding="0" cellspacing="0" class="uploadphoto" width="100%">
                <tr>
                    <td>
                        <div class="text">
                            <%= Lang.Trans("To make a photo thumbnail, click a corner of the crop frame, and then drag it. When you're ready click \"Save Thumbnail\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <img id="imgFaceCropPhoto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <ul>
                            <li>
                                <asp:LinkButton ID="lnkSaveThumbnail" runat="server" OnClick="lnkSaveFaceCrop_Click"></asp:LinkButton>
                                | </li>
                            <li>
                                <asp:LinkButton ID="lnkSkipThumbnail" runat="server" OnClick="lnkCancelFaceCrop_Click"></asp:LinkButton>
                            </li>
                        </ul>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hidCropPhotoId" runat="server" />
            <input type="hidden" id="hidCropX" runat="server" />
            <input type="hidden" id="hidCropY" runat="server" />
            <input type="hidden" id="hidCropW" runat="server" />
            <input type="hidden" id="hidCropH" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pnlPhotoNote" Visible="false" runat="server">
            <table cellpadding="0" cellspacing="0" class="uploadphoto" width="100%">
                <tr>
                    <td>
                        <div class="text">
                            <%= Lang.Trans("To add a photo note, click a corner of the note frame, and then drag it. When you're ready click \"Add Note\"")%></div>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center">
                        <div class="separator">
                        </div>
                        <img id="imgAddPhotoNote" class="photoframe" runat="server" />
                        <div class="separator">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <table id="tblCropPreview" runat="server" visible="false" cellpadding="5" cellspacing="0">
                            <tr>
                                <td valign="top" style="width: 95px; height: 95px; text-align: center;" align="center">
                                    <div id="divNoteCropPreview" class="photoframe" style="width: 92px; height: 92px;
                                        overflow: hidden;">
                                        <img id="imgNoteCropPreview" runat="server" alt="crop preview" src="" />
                                    </div>
                                </td>
                                <td>
                                    <%= "Enter note:".Translate() %>
                                    <asp:TextBox CssClass="multilinetextbox" ID="txtNoteText" runat="server" TextMode="MultiLine"
                                        Rows="3"></asp:TextBox>
                                    <div class="separator">
                                    </div>
                                    <%= "Is this a friend registered here?".Translate() %>&nbsp;<asp:DropDownList CssClass="l_form_ddl"
                                        ID="ddNoteFriend" runat="server">
                                    </asp:DropDownList>
                                    <div class="separator">
                                    </div>
                                    <asp:Button ID="btnAddNote" runat="server" OnClick="btnAddNote_Click" CssClass="SubmitBtn" />
                                    <div class="separator">
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <asp:Repeater ID="rptNotes" OnItemCommand="rptNotes_ItemCommand" runat="server">
                            <HeaderTemplate>
                                <table class="photonotes" width="100%" border="0" cellspacing="0" cellpadding="0">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td valign="top" width="55px" align="center">
                                        <img src="<%# Eval("ImageCropUrl") %>" class="photoframe" alt="" />
                                    </td>
                                    <td valign="top">
                                        <%# Eval("Username") != DBNull.Value ? ("Username: ".Translate() + Eval("Username") + "<br />") : "" %><%# Eval("Notes") != DBNull.Value ? ("Notes: ".Translate() + Eval("Notes") + "<br />") : "" %><asp:Button
                                            ID="btnDeleteNote" runat="server" CommandName="DeleteNote" CommandArgument='<%# Eval("Id") %>'
                                            Text='<%# "Delete".Translate() %>' CssClass="medium zurbtn red" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
                <tr>
                    <td>
                        <components:FlexButton ID="lnkDoneAddingPhotoNotes" runat="server" 
                                                OnClick="lnkDoneAddingPhotoNotes_Click" RenderAs="Button" 
                                                CssClass="medium zurbtn red"></components:FlexButton>
                    </td>
                </tr>
            </table>
            <input type="hidden" id="hidNotePhotoId" runat="server" />
            <input type="hidden" id="hidNoteX" runat="server" />
            <input type="hidden" id="hidNoteY" runat="server" />
            <input type="hidden" id="hidNoteW" runat="server" />
            <input type="hidden" id="hidNoteH" runat="server" />
        </asp:Panel>
        <div class="clear">
        </div>
        <div class="separator">
        </div>
        <uc1:HeaderLine ID="PhotoGuidelinesHeaderLine" runat="server" />
        <div class="note">
            <components:ContentView ID="cvGuideLines" Key="UploadPhotosGuideLines" runat="server">
            </components:ContentView>
        </div>
        <div class="clear">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
