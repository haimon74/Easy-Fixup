<%@ Control Language="C#" ClassName="SearchResultGridModal" %>
<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<script runat="server">

    private string _viewer;
    private string _viewed;
    private User[] _mutualFriends;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            _viewer = Request.QueryString["viewer"] ?? "Haimon74"; 
            _viewed = Request.QueryString["viewed"];
            LoadStrings();
            GetMutualFriends(0, 3);
        }
    }

    private void LoadStrings()
    {
        lbBlock.ToolTip = Lang.Trans("Block user");
        lbFavorite.ToolTip = Lang.Trans("Add to favorite list");
        lbInviteFriend.ToolTip = Lang.Trans("Friendship invitation");
        lbMatchRequest.ToolTip = Lang.Trans("Request matchmaking");
        lbSendMsg.ToolTip = Lang.Trans("Send message");
        lbSendGift.ToolTip = Lang.Trans("Send gift");
        lbWink.ToolTip = Lang.Trans("Wink");
    }
    
    private void GetMutualFriends(int pageNum, int pageSize)
    {
        var viewer = (new UsernameSearch {Username = _viewer}).GetResults().Get();
        var viewed = (new UsernameSearch {Username = _viewed}).GetResults().Get();
        var mfSearch = new MutualFriendsSearch { Viewer = _viewer, Viewed = _viewed };
        var searchResults = mfSearch.GetResults();
        User[] mf = (searchResults != null)
                        ? searchResults.Get()
                        : new User[] {viewer[0], viewed[0], viewer[0]};
                                                                         
        //var result = new JsonObject();
        User[] takedUsers = null;
        if (pageSize > mf.Length)
        {
            takedUsers = mf;
        }
        else if (pageNum*pageSize > mf.Length)
        {
            takedUsers = new User[pageSize];
            for (int j = 0, i = mf.Length - pageSize; j < pageSize; i++, j++)
            {
                takedUsers[j] = mf[i];
            }
        }
        else
        {
            takedUsers = new User[pageSize];
            for (int j = 0, i = pageNum * pageSize; j < pageSize; i++, j++)
            {
                takedUsers[j] = mf[i];
            }
        }
        _mutualFriends = takedUsers;
    }

</script>

<script type="text/javascript">
    jQuery(function($) {
        $("#mf1div").hover( function() { $("#mf1pop").attr('visibility', "visible"); }, 
                            function() {$("#mf1pop").attr('visibility', "hidden");
                            });
        $("#mf2div").hover(function() { $("#mf2pop").attr('visibility', "visible"); },
                            function() {$("#mf2pop").attr('visibility', "hidden");
                            });
        $("#mf3div").hover( function() { $("#mf3pop").attr('visibility', "visible"); },
                            function() {$("#mf2pop").attr('visibility', "hidden");
                            });
        $("#" + "<%=lbFavorite.ClientID%>").hover(
          function() {
              $(this).attr('href', "AddFavorite.aspx?id=" + username);
              $(this).addClass('FavoriteBtnBigH'); $(this).removeClass('FavoriteBtnBig');
          }, function() {
              $(this).removeClass('FavoriteBtnBigH'); $(this).addClass('FavoriteBtnBig');
          });
        
        $("#" + "<%=lbBlock.ClientID%>").hover(
          function() {
              $(this).attr('href', "BlockMember.aspx?id=" + username);
              $(this).addClass('BlockBtnBigH'); $(this).removeClass('BlockBtnBig');
          }, function() {
              $(this).removeClass('BlockBtnBigH'); $(this).addClass('BlockBtnBig');
          });
        $("#" + "<%=lbFavorite.ClientID%>").hover(
          function() {
              $(this).attr('href', "AddFavorite.aspx?id=" + username);
              $(this).addClass('FavoriteBtnBigH'); $(this).removeClass('FavoriteBtnBig');
          }, function() {
              $(this).removeClass('FavoriteBtnBigH'); $(this).addClass('FavoriteBtnBig');
          });
         $("#" + "<%=lbInviteFriend.ClientID%>").hover(
          function() {
              $(this).attr('href', "InviteFriend.aspx?id=" + username);
              $(this).addClass('AddBtnBigH'); $(this).removeClass('AddBtnBig');
          }, function() {
              $(this).removeClass('AddBtnBigH'); $(this).addClass('AddBtnBig');
          });
        $("#" + "<%=lbMatchRequest.ClientID%>").hover(
            function() {
                $(this).attr('href', "MatchmakingRequest.aspx?id=" + username);
                $(this).addClass('MatchBtnBigH'); $(this).removeClass('MatchBtnBig');
            }, function() {
                $(this).removeClass('MatchBtnBigH'); $(this).addClass('MatchBtnBig');
            });
        $("#" + "<%=lbSendMsg.ClientID%>").hover(
          function() {
              $(this).addClass('MsgBtnBigH'); $(this).removeClass('MsgBtnBig');
          }, function() {
              $(this).removeClass('MsgBtnBigH'); $(this).addClass('MsgBtnBig');
          });
         $("#" + "<%=lbSendGift.ClientID%>").hover(
          function() {
              $(this).attr('href', "SendGift.aspx?id=" + username);
              $(this).addClass('GiftBtnBigH'); $(this).removeClass('GiftBtnBig');
          }, function() {
              $(this).removeClass('GiftBtnBigH'); $(this).addClass('GiftBtnBig');
          });
         $("#" + "<%=lbWink.ClientID%>").hover(
          function() {
              $(this).attr('href', "SendWink.aspx?id=" + username);
              $(this).addClass('WinkBtnBigH'); $(this).removeClass('WinkBtnBig');
          }, function() {
              $(this).removeClass('WinkBtnBigH'); $(this).addClass('WinkBtnBig');
          });
         //$('.tipsys').tipsy({ gravity: 's' });
    });
</script>

<div style="background-color: rgb(228,239,243);height:140px; width:100%;z-index:5;text-align:justify;">
    <table style="display:block;width:100%;text-align:justify;clear:both;"><tr>
        <td  style="width:40px;"></td>
        <td><asp:LinkButton ID="lbBlock" runat="server" CssClass="BlockBtnBig tipsys"/></td>
        <td><asp:LinkButton ID="lbMatchRequest" runat="server"  CssClass="MatchBtnBig tipsys" /></td>
        <td><asp:LinkButton ID="lbFavorite" runat="server" CssClass="FavoriteBtnBig tipsys" /></td>
        <td  style="width:40px;"></td>
    </tr></table>
    <table style="display:block;width:100%;text-align:justify;clear:both;"><tr>
        <td  style="width:15px;"></td>
        <td><asp:LinkButton ID="lbWink" runat="server" CssClass="WinkBtnBig tipsys" /></td>
        <td><asp:LinkButton ID="lbSendMsg" runat="server" CssClass="MsgBtnBig tipsys" /></td>
        <td><asp:LinkButton ID="lbSendGift" runat="server" CssClass="GiftBtnBig tipsys" /></td>
        <td><asp:LinkButton ID="lbInviteFriend" runat="server" CssClass="AddBtnBig tipsys" /></td>
        <td  style="width:15px;"></td>
    </tr></table>
</div>
<div style="width:100%;height:150px;">
<table>
<tr>
<td>
<div id="mf1pop" style="visibility:hidden;background:red;">
action icons MF1
</div><br />
<div id="mf1div" class="ThumbsWrap" mu="<%=_mutualFriends[0].Username%>" 
    style="background: url(<%# ImageHandler.CreateImageUrl( 
     _mutualFriends[0].GetPrimaryPhoto().Id, 60,80, false, true, true) %>) no-repeat" />
</td>
<td>
<div id="mf2pop" style="visibility:hidden;background:orange;">
action icons MF2
</div><br />
<div id="mf2div" class="ThumbsWrap" mu="<%=_mutualFriends[1].Username%>" 
    style="background: url(<%# ImageHandler.CreateImageUrl( 
     _mutualFriends[1].GetPrimaryPhoto().Id, 60,80, false, true, true) %>) no-repeat" />
</td>
<td>
<div id="mf3pop" style="visibility:hidden;background:green;">
action icons MF3
</div><br />
<div id="mf3div" class="ThumbsWrap" mu="<%=_mutualFriends[2].Username%>" 
    style="background: url(<%# ImageHandler.CreateImageUrl( 
     _mutualFriends[2].GetPrimaryPhoto().Id, 60,80, false, true, true) %>) no-repeat" />
</td>
</tr>
</table>
</div>