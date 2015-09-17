<%@ Page language="c#" 
MasterPageFile="Site.Master" 
Codebehind="ImportFriends.aspx.cs" 
AutoEventWireup="True" Trace="false" 
Inherits="ezFixUp.ImportFriends" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"></uc1:WideBoxStart>
		<div class="wrap-sized">
		    <div id="fb-root"></div>
		    <script src="http://connect.facebook.net/en_US/all.js" type="text/javascript"></script>
            <script type="text/javascript">
                FB.init({
                    appId: "<%= FacebookHelper.ApiKey %>",
                    status: true, // check login status
                    cookie: true, // enable cookies to allow the server to access the session
                    xfbml: true, // parse XFBML
                    channelURL: 'http://ezFixUp.COM/channel.html', // channel.html file
                    oauth: true // enable OAuth 2.0
                });
                FB.login(function(response) {
        	        if (response.session) {
        		        if (response.perms) {
        			        window.location='LoginThroughFacebook.aspx?access_token='+response.session.access_token
        		        } else {
        			        window.location='Default.aspx'
        		        }
        	        } else {
        	        }
                }, {scope:'<%=ConfigurationManager.AppSettings["FacebookPermissions"] %>'});
                (function(d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id)) { return; }
                    js = d.createElement(s); js.id = id;
                    js.src = "//connect.facebook.net/en_US/all.js#xfbml=1";
                    fjs.parentNode.insertBefore(js, fjs);
                } (document, 'script', 'facebook-jssdk'));
                jQuery(function($){
                    $(".fb-like").attr('data-href', 'http://ezFixUp.com?invitedBy=' + "<%= GetCurrentUserSession().Username %>");
                });
            </script>
            <%--<div class="fb-send" data-href="http://ezFixUp.com" data-font="arial"></div>--%>
            <div class="fb-like" data-href="http://ezFixUp.com" data-send="true" data-width="450" data-show-faces="true" data-font="arial"></div>
            
		    <asp:MultiView ID="mvImportFriends" runat="server" ActiveViewIndex="0">
		        <asp:View ID="viewEmail" runat="server">
		            <%= "NOTE: We don't store your email and password information.".Translate() %><br />
		            <table>
		            <tr>
		                <td><%= Lang.Trans("Email address:") %>&nbsp;</td>
		                <td>
		                    <input class="textbox" type="text" ID="txtEmail" Runat="server"/>@
                            <asp:DropDownList  CssClass="dropdownlist" ID="ddEmailProvider" runat="server">
                                <asp:ListItem>gmail.com</asp:ListItem>
                                <asp:ListItem>yahoo.com</asp:ListItem>
                                <asp:ListItem>live.com</asp:ListItem>
                                <asp:ListItem>hotmail.com</asp:ListItem>
                            </asp:DropDownList>
		                </td>
		            </tr>
		            <tr>
		                <td><%= Lang.Trans("Password:") %>&nbsp;</td>
		                <td><input class="textbox" id="txtPassword" type="password" runat="server"/></td>
		            </tr>
		            <tr>
		                <td colspan="2">
		                    <div class="srvmsg">
		                        <asp:label id="lblError" CssClass="error" EnableViewState="false" runat="server"></asp:label>
		                    </div>
			            </td>
			        </tr>
			        <tr>
			            <td colspan="2">
			                <div class="buttons">			                    
                                <asp:Button ID="btnImport" runat="server" onclick="btnImport_Click" CssClass="SendBtn" />
			                </div>                            
						    <div class="center">
			                    <a href="InviteFriend.aspx"><%= "... or click here to invite by e-mail".Translate() %></a><br />
                                <a href="InviteFriendsFromFacebook.aspx"><%= "... or click here to invite friends from facebook".Translate() %></a><br />
                            </div>
			            </td>
			        </tr>
		            </table>
			    </asp:View>
			    <asp:View ID="viewMessage" runat="server">
			        <asp:DataGrid ID="dgContacts" runat="server" AutoGenerateColumns="false">
			            <Columns>
			                <asp:TemplateColumn HeaderText="&lt;input type=checkbox onClick=&quot;a=0;for(i=0; i&lt;this.form.elements.length;i++){if(this.form.elements[i]==this) {a=3}; if ((this.form.elements[i].type=='checkbox') &amp;&amp; (a!=0) &amp;&amp; (i&gt;a)) {this.form.elements[i].checked=this.checked}}&quot;&gt;">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="cbSelect" value='<%# Eval("Email") %>' runat="server" name="cbSelect" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
			                <asp:TemplateColumn>
			                    <ItemTemplate>
			                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
			                    </ItemTemplate>
			                </asp:TemplateColumn>
			                <asp:TemplateColumn>
			                    <ItemTemplate>
			                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
			                    </ItemTemplate>
			                </asp:TemplateColumn>
			            </Columns>
			        </asp:DataGrid>
			        <br />
			        <div class="separator6"></div>
			        <%= Lang.Trans("Add a personal note to this message:") %>
			        <div class="fckeditor">
			            <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
			        </div>
			        <div class="buttons">
				        <asp:button id="btnSubmit" tabIndex="1" runat="server"></asp:button>
			        </div>
			    </asp:View>
			</asp:MultiView>
		</div>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
</asp:Content>
