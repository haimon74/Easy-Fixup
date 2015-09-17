<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true" CodeBehind="ShowMessage.aspx.cs" Inherits="AspNetDating.Mobile.ShowMessage" %>
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
                	<a id="lnkShowUser" runat="server"><asp:Literal ID="ltrPhoto" runat="server"></asp:Literal></a>
                </td>
                <td valign="top">
                    <b><%= Lang.Trans("From") %>:</b>&nbsp;<asp:Label ID="lblFromUsername" runat="server"></asp:Label><br />
                    <b><%= Lang.Trans("To") %>:</b>&nbsp;<asp:Label ID="lblToUsername" runat="server"></asp:Label><br />
                    <b><%= Lang.Trans("Time") %>:</b>&nbsp;<asp:Label ID="lblMessageTime" runat="server"></asp:Label>                
                </td>
            </tr>
        </table>  
        <div class="Separator"></div>  
    	<asp:Label ID="lblMessage" runat="server"></asp:Label>
	</div>    
    <div id="pnlPreviousMessages" visible="false" runat="server" class="text">
        <h1><%= Lang.Trans("Conversation Archive") %></h1>
		<div class="SeparatorLine"></div>         
        <div class="ContentWrap">
            <asp:Repeater ID="rptPreviousMessages" runat="server">
            	<alternatingitemtemplate>
                	<div><b><%# Eval("Username") %>:</b>&nbsp;<span class="translatable"><%# Eval("Message") %></span></div>
                </alternatingitemtemplate>
                <ItemTemplate>
                    <div class="Reply"><b><%# Eval("Username") %>:</b>&nbsp;<span class="translatable"><%# Eval("Message") %></span></div>
                </ItemTemplate>
                <SeparatorTemplate>
                </SeparatorTemplate>
            </asp:Repeater>
    	</div>
    </div>
    <div class="SeparatorLine"></div>    
    <div class="FooterLinks" >
        <asp:LinkButton ID="lnkReply" runat="server" OnClick="lnkReply_Click" />
        <asp:LinkButton ID="lnkDelete" runat="server" OnClick="lnkDelete_Click" /><br />
        <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" />
   </div>
</asp:Content>
