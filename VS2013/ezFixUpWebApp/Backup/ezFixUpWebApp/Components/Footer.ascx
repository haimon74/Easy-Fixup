<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Footer.ascx.cs" Inherits="ezFixUp.Components.Footer" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%--<%@OutputCache Duration="3600000" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<div id="footer" class="FooterColor">
    
   <div id="divFacebookConnected" visible="false" runat="server">
      
    </div>
	<div id="cplinks" style="width:98%;">
		<ul>
    		<asp:Repeater ID="rptPages" Runat="server">
				<ItemTemplate>
					<li>
						<a href='<%# Eval("URL") == null ? UrlRewrite.CreateContentPageUrl((int)Eval("ID")) : (string) Eval("URL")%>' class="skinnavlinkcolor skinfooterlinkcolor"><%# Eval("Title")%></a></li>
				</ItemTemplate>
			</asp:Repeater>
		</ul>
	</div>
	<components:BannerView id="bvDefaultFooter" runat="server" Key="DefaultFooter"></components:BannerView>
	
</div>