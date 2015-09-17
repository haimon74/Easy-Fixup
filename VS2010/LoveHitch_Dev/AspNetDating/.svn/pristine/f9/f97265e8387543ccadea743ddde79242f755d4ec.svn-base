<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="Default.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating._default" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">

    <div class="defaultPageBg">
        <div style="overflow: hidden; width: 500px; float: right; margin-left: 1px; height: 500px;
            margin-top: 500px;">
            <%--<iframe src="//www.facebook.com/plugins/likebox.php?href=http%3A%2F%2Fwww.facebook.com%2Fplatform&amp;width=292&amp;height=590&amp;colorscheme=light&amp;show_faces=true&amp;border_color&amp;stream=true&amp;header=true&amp;appId=142092645886252"
                 scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:292px; height:590px;" allowTransparency="true"></iframe>--%>
            <%--<iframe src="//www.facebook.com/plugins/likebox.php?appId=139801212735949&amp;href=http%3A%2F%2Fapps.facebook.com%2Flovehitch%2F&amp;width=292&amp;colorscheme=light&amp;show_faces=false&amp;border_color&amp;stream=false&amp;header=true&amp;height=62" 
            scrolling="no" frameborder="0" style="border:none; overflow:hidden; width:500px; height:62px;" allowTransparency="true"></iframe>--%>
            <fb:like href="http://lovehitch.com" send="true" width="450" show_faces="false" font="arial">
            </fb:like>
            <iframe src="http://www.facebook.com/plugins/facepile.php?app_id=139801212735949"
                scrolling="no" frameborder="0" style="border: none; overflow: hidden; width: 500px;"
                allowtransparency="true"></iframe>
            <%--<iframe scrolling="yes" frameborder="0" allowtransparency="true" 
	            style="border: none; overflow: hidden; width: 500px; height: 260px; position: relative; left: 2px; bottom: 7px;" 
	            src="http://www.facebook.com/plugins/likebox.php?id=160629763494&amp;show_faces=false&amp;stream=false&amp;
	                width=500&amp;connections=10&amp;header=false&amp;height=260&amp;locale=en_US">
	    </iframe>--%>
            <%--<div id="fb-root"></div>
        <script type="text/javascript">
            (function(d, s, id) {
                  var js, fjs = d.getElementsByTagName(s)[0];
                  if (d.getElementById(id)) {return;}
                  js = d.createElement(s); js.id = id;
                  js.src = "//connect.facebook.net/en_US/all.js#appId={160629763494}&amp;xfbml=1";
                  fjs.parentNode.insertBefore(js, fjs);
            } (document, 'script', 'facebook-jssdk'));
        </script>

        <fb:facepile href="http://www.lovehitch.com" size="large" width="500" max_rows="3"></fb:facepile>--%>
        </div>
        <asp:PlaceHolder ID="plhComponents" runat="server" />
        <div style="width: 220px; height: 100px; margin-left: 10px; font-size: medium; font-weight: bold;
            text-align: center; line-height: 1.2em;">
            <table>
                <tr>
                    <%= "<td style='color:#0797A9;'>Take advantage of our <br /> one time launch offer</td>".Translate()%>
                </tr>
                <tr>
                    <%= "<td style='color:#B6006D;'>Register now and get<br /><span style='font-size:x-large;color:#0797A9;'><b>2 months</b></span> <br />Premium Membership !</td>".Translate()%>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        function detectPopupBlocker() {
            var windowRef = window.open("ShowStatus.aspx", "", "directories=no,height=100,width=100,menubar=no,resizable=no,scrollbars=no,status=no,titlebar=no,top=0,location=no");
            if (!windowRef || typeof windowRef.closed == 'undefined') {
                alert("Popup is blocked in your browser, please turn off popup blocker before joining site.");
            }
            else {
                windowRef.close();
            }
        }
        window.onload = detectPopupBlocker;
    </script>
</asp:Content>
