<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="UserEvent.ascx.cs" 
            Inherits="ezFixUp.Components.UserEvent" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div class="separator6"></div>
<table class="eventwrap" cellpadding="0" cellspacing="0">
	<tr>
		<td valign="top" class="EventLeftImg">
		    <div class="eventimg">
		        <a id="lnkLeftImage" runat="server" >
		            <img id="imgLeft" runat="server" alt='' src='' class="photoframe" />
		        </a>
		    </div>
		</td>
		<td valign="top" class="skincolor">
		    <div id="divEventText" runat="server">
		        <asp:Literal ID="ltrEventText" runat="server" />
		        <span id="spanEventTime" class="EventTime SkinInfoText" visible="false" runat="server">-
		            <asp:Literal ID="ltrEventTime" runat="server" />
		        </span>		        
		        <span id="spanLeaveComment" runat="server">-
		            <asp:LinkButton CssClass="skinlinkcolor" ID="lnkLeaveComment" runat="server" 
		            onclick="lnkLeaveComment_Click" />
		        </span>
		        <span id="spanDelete" runat="server">-
		            <asp:LinkButton ID="lnkDelete" CssClass="skinlinkcolor" runat="server" onclick="lnkDelete_Click" />
		        </span>		        	        
		    </div>
		    <asp:Repeater ID="rptEventImages" EnableViewState="false" runat="server"
		        onitemdatabound="rptEventImages_ItemDataBound">
		        <ItemTemplate>
		            <div class="eventimg">
		                <a href='<%# Eval("ShowUserUrl") %>' >
		                    <img id="imgUrl" runat="server" src='<%# Eval("SmallImageUrl") %>' alt='' class="photoframe" /></a>
		            </div>
		        </ItemTemplate>
		    </asp:Repeater>
		    <div id="pnlEventComments" runat="server" class="EventCommentsWrap">  	
            	<div align="center">
                    <span id="spanAddComment" runat="server" visible="false">
                        <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </span>
                    <div class="clear"></div>
                    <span id="spanCancel" runat="server" visible="false">
                        <asp:Button ID="btnAddComment" runat="server" CssClass="small zurbtn blue"
                            onclick="btnAddComment_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="small zurbtn red" 
                            onclick="btnCancel_Click" />
                    </span>	            
                </div>		  
			    <div class="separator"></div>                         	      
			    <asp:Repeater ID="rptEventComments" runat="server" onitemcommand="rptEventComments_ItemCommand" 
			        onitemcreated="rptEventComments_ItemCreated" onitemdatabound="rptEventComments_ItemDataBound">
			        <ItemTemplate>
			            <div class="EventComment skincolor">
			            <div>
			            <a class="skinlinkcolor" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a>&nbsp;<span class="EventCommentDate SkinInfoText"><%# Eval("Date") %></span><br>
			            <%# Eval("Comment") %>
			            <asp:LinkButton ID="lnkDelete" runat="server" CssClass="skinlinkcolor" CommandArgument='<%# Eval("ID") %>' CommandName="Delete"></asp:LinkButton>
			            </div></div>
			        </ItemTemplate>
			    </asp:Repeater>
			    <div id="pnlShowMore" runat="server" visible="false" class="EventComment">
				    <asp:LinkButton ID="lnkShowMore" CssClass="ShowMoreLink skinlinkcolor" runat="server" onclick="lnkShowMore_Click"></asp:LinkButton>
			    </div>
			</div>			
		</td>
	</tr>		
</table>
<div class="separator6"></div>
<div class="line"></div>