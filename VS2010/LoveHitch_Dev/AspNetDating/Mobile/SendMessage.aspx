<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true" CodeBehind="SendMessage.aspx.cs" Inherits="AspNetDating.Mobile.SendMessage" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine"></div> 
    <div class="ContentWrap">  
    	<table cellpadding="0" cellspacing="0" width="100%">
        	<tr>
            	<td valign="top" align="left" width="98px">        
					<asp:Literal ID="ltrPhoto" runat="server"></asp:Literal>
                </td>
                <td valign="top">  
                    <b><%= Lang.Trans("From") %>:</b>&nbsp;<asp:Label ID="lblFromUsername" runat="server"></asp:Label><br />
                    <b><%= Lang.Trans("To") %>:</b>&nbsp;<asp:Label ID="lblToUsername" runat="server"></asp:Label><br />
                </td>
            </tr>
        </table>  
        <div class="Separator"></div>                                          
        <div id="pnlUserResponse" runat="server">
        <div class="Separator"></div>  
            <asp:TextBox ID="txtMessageBody" CssClass="TextField" runat="server" TextMode="MultiLine"></asp:TextBox><br />
        </div>
        <div class="Separator"></div>  
		<asp:Button ID="btnSend" CssClass="LoginBtn" runat="server" onclick="btnSend_Click"></asp:Button>
    </div>      
    <div id="pnlPreviousMessages" class="text" visible="false" runat="server">
        <div class="SeparatorLine"></div>         
        <div class="ContentWrap">          
            <asp:Repeater ID="rptPreviousMessages" runat="server">
            	<alternatingitemtemplate>
                	<div class="Reply"><b><%# Eval("Username") %>:</b>&nbsp;<%# Eval("Message") %></div>
                </alternatingitemtemplate>
                <ItemTemplate>
                    <div ><b><%# Eval("Username") %>:</b>&nbsp;<%# Eval("Message") %></div>
                </ItemTemplate>
                <SeparatorTemplate>
                </SeparatorTemplate>
            </asp:Repeater>
        </div>
    </div>        
    <div class="SeparatorLine"></div>    
    <div class="FooterLinks" >    
		<asp:LinkButton ID="lnkBack" runat="server" onclick="lnkBack_Click" />
    </div>
</asp:Content>
