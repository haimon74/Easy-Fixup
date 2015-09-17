<%@ Control Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="MatchToFriend.ascx.cs" 
    Inherits="AspNetDating.Components.Matchmaker.MatchToFriend" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="~/Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="~/Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MatchToFriendContent" Src="~/Components/Matchmaker/MatchToFriendContent.ascx" %>

<asp:ScriptManagerProxy ID="smProxy" runat="server">
    <Scripts>
        <asp:ScriptReference Path="~/scripts/components/matchToFriend.js" />
    </Scripts>
</asp:ScriptManagerProxy>

<script type="text/javascript">
    asyncScripts.push('scripts/components/matchToFriend.js');
</script>

<uc1:SmallBoxStart ID="SmallBoxStart1" runat="server" EnableViewState="false" >
</uc1:SmallBoxStart>
<uc1:MatchToFriendContent ID="MatchToFriendContent1" runat="server">
</uc1:MatchToFriendContent>
<uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server">
</uc1:SmallBoxEnd>

