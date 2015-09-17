<%@ Control Language="c#" AutoEventWireup="True" Codebehind="UploadVideo.ascx.cs"
    Inherits="AspNetDating.Components.Profile.UploadVideo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="VideoRecorder" Src="../../VR/VideoRecorder.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls" TagPrefix="asp" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register src="RecordVideo.ascx" tagname="RecordVideo" tagprefix="uc2" %>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<input type="hidden" id="hidUsername" runat="server" name="hidUsername" />
<div id="pnlButtons" class="center" runat="server">
    <asp:Button ID="btnShowRecordVideo" runat="server" 
        onclick="btnShowRecordVideo_Click" />
    <asp:Button ID="btnShowUploadVideo" runat="server" 
        onclick="btnShowUploadVideo_Click" />
    <asp:Button ID="btnShowEmbedVideo" runat="server" 
        onclick="btnShowEmbedVideo_Click" />
</div>
<asp:MultiView ID="mvVideo" ActiveViewIndex="0" runat="server">
<asp:View ID="vRecordVideo" runat="server">
<uc2:RecordVideo ID="RecordVideo1" runat="server" />
</asp:View>
<asp:View ID="vUploadVideo" runat="server">
<uc1:HeaderLine ID="HeaderLine1" runat="server" />
<div class="note">
    <components:ContentView ID="cvNote" Key="UploadVideosNotes" runat="server"></components:ContentView>
</div>
<div class="center">
    <asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="false"></asp:Label>
    <div id="divUploadVideo" runat="server">
        <%= Lang.Trans("Video file") %>:
        <asp:FileUpload ID="fileVideo" runat="Server" />
        <div class="separator"></div>
        <asp:CheckBox ID="cbPrivateVideo" runat="server" />
        <%= Lang.Trans("Set this video as private")%><div class="separator"></div>
        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" />
    </div>
    <div id="divUploadMultipleVideosFlash" runat="server">
        [&nbsp;<asp:LinkButton ID="lnkUploadMultipleFlash" runat="server" 
            onclick="lnkUploadMultipleFlash_Click"></asp:LinkButton>&nbsp;]
    </div>
    <div id="divUploadMultipleVideosSilverlight" runat="server">
        [&nbsp;<asp:LinkButton ID="lnkUploadMultipleSilverlight" runat="server" 
            onclick="lnkUploadMultipleSilverlight_Click"></asp:LinkButton>&nbsp;]
    </div>
</div>
<asp:Repeater ID="rptVideoUploads" runat="server" 
        onitemcommand="rptVideoUploads_ItemCommand" 
        onitemcreated="rptVideoUploads_ItemCreated">
    <ItemTemplate>
    	<table cellpadding="5" cellspacing="0" width="100%">
    		<tr>
    			<td width="70px">
			        <img src='<%# Eval("ImageURL") %>' />
			    </td>
			    <td valign="middle">
			    	<asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
			        <asp:Button ID="btnDeleteVideoUpload" runat="server" CommandArgument='<%# Eval("ID") %>' CommandName="Delete"/>			    
			    </td>
    		</tr>
    	</table>
    </ItemTemplate>
</asp:Repeater>
<asp:Panel id="pnlUploadMultipleVideosFlash" runat="server" Visible="false">
<components:FlashUpload ID="flashUpload" runat="server" OnUploadComplete="flashUploadIsCompleted()"
            UploadPage="../../FlashUpload.ashx"
            FileTypeDescription="Videos" FileTypes="*.mpeg; *.mov; *.mpg; *.avi; *.wmv" 
            UploadFileSizeLimit="52428800" TotalUploadSizeLimit="209715200" />
</asp:Panel>
<asp:Panel ID="pnlUploadMultipleVideosSilverlight" runat="server" Visible="false">
    <asp:Silverlight ID="Silverlight1" runat="server" Source="~/ClientBin/MultiFileUploader.xap" MinimumVersion="2.0.31005.0" Width="400" Height="300"></asp:Silverlight>
</asp:Panel>
<asp:LinkButton ID="lnkBackToVideoUploads" runat="server" 
        onclick="lnkBackToVideoUploads_Click"></asp:LinkButton>
</asp:View>
<asp:View ID="vEmbedVideo" runat="server">
<uc1:HeaderLine ID="HeaderLine2" runat="server" />
<div class="note">
    <components:ContentView ID="cvNote2" Key="EmbedVideosNotes" runat="server"></components:ContentView>
</div>
<div class="center">
    <asp:Label ID="lblError2" runat="server" CssClass="error" EnableViewState="false"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanelVideoEmbeds" runat="server">
        <ContentTemplate>
            <asp:Panel ID="divVideoKeywords" DefaultButton="btnSearchVideoKeywords" runat="server">
                <%= Lang.Trans("Keywords") %>
                :
                <asp:TextBox ID="txtVideoKeywords" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearchVideoKeywords" runat="server" OnClick="btnSearchVideoKeywords_Click" />
            </asp:Panel>
            <br />
            <div id="divVideoPreview" class="center" visible="false" runat="server">
                <asp:Literal ID="ltrVideoPreview" runat="server"></asp:Literal>
                <br />
                <asp:Button ID="btnEmbedVideo" runat="server" OnClick="btnEmbedVideo_Click" />
                <br />
            </div>
            <div id="divVideoThumbnails" runat="server">
                <div class="clear"></div>
                <asp:DataList ID="dlVideos" RepeatColumns="3"  Width="100%" CellPadding="4" CellSpacing="0"
                runat="server" RepeatDirection="Horizontal" OnItemCommand="dlVideos_ItemCommand">
                    <ItemStyle CssClass="center"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton ImageUrl='<%# Eval("ThumbnailUrl") %>' CommandName="SelectVideo"
                            CommandArgument='<%# Eval("ThumbnailUrl") + "|" + Eval("VideoUrl") + "|" + Eval("Title") %>'
                            runat="server" />
                        <br />
                        <asp:LinkButton Text='<%# Eval("Title") %>' CommandName="SelectVideo" CommandArgument='<%# Eval("ThumbnailUrl") + "|" + Eval("VideoUrl") + "|" + Eval("Title") %>'
                            runat="server" />
                        <div>
                            <asp:Button Text='<%# Lang.Trans("Remove") %>' CommandName="RemoveVideo" CommandArgument='<%# ((int?)Eval("ID")) %>'
                                Visible='<%# ((int)Eval("ID")) != 0 %>' runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div id="pnlAddYouTubeVideoButton" runat="server">
            <br />
                <asp:Button id="btnAddYouTubeVideo" runat="server" 
                    onclick="btnAddYouTubeVideo_Click" />
            </div>
            <div id="pnlBackButton" runat="server">
                <asp:Button ID="btnBack" runat="server" onclick="btnBack_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:View>
</asp:MultiView>
<br>
                <div class="clear"></div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
