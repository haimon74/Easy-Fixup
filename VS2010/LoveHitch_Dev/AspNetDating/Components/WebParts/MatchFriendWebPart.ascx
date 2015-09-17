<%@ Control Language="C#" AutoEventWireup="true" 
            CodeBehind="MatchFriendWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.MatchFriendWebPart" %>
<%@ Import Namespace="AspNetDating" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<table cellspacing="2" style="background:pink;">
<tr>
<td>
    <div id="matchToImageDiv" style="width:100px;height:100px; border:solid 2px purple;
                background:url(<%# ImageHandler.CreateImageUrl(FriendImageId, 100,100, false,true,true) %>);">
    </div>
</td>
<td>
    <div style="width:35%;height:100px;background:green;">
    </div>
</td>
<td>
    <div style="width:15%;height:100px;background:red;">
    </div>
</td>
</tr>
</table>