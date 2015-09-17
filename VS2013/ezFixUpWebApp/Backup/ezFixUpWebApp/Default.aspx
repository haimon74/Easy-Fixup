<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="Default.aspx.cs"
    AutoEventWireup="True" Inherits="ezFixUp._default" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">

    <div class="defaultPageBg">
        <div style="overflow: hidden; width: 500px; float: right; margin-left: 1px; height: 500px;
            margin-top: 500px;">
            <fb:like href="https://ezFixUp.com" send="true" width="450" show_faces="false" font="arial">
            </fb:like>
            <iframe src="https://www.facebook.com/plugins/facepile.php?app_id=139801212735949"
                scrolling="no" frameborder="0" style="border: none; overflow: hidden; width: 500px;"
                allowtransparency="true"></iframe>
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
