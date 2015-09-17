<%@ Page language="c#" Codebehind="SpamCheck.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.SpamCheck" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
    	<title><%= Lang.TransA("Spam Check") %></title>
	    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
	    <meta name="CODE_LANGUAGE" Content="C#">
	    <meta name=vs_defaultClientScript content="JavaScript">
	    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_spam.jpg"> 
					<div id="crumpstxt"><%= Lang.TransA("Spam Check") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="Adminmenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
		        	<div id="content-head">
			        	<div class="ch-head"><%= Lang.TransA("Spam Check") %></div>
	                    <div class="ch-description">
	                    	<%= Lang.TransA("Use this section to check members mail for spam...") %>
	                    </div>
	                </div>
	                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
	                <uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
				    <table width="100%" cellpadding=0 cellspacing=0 border="0">
					    <tr>
						    <td align="right" class="perpage">
							    <asp:Label id="lblMessagesPerPage" Font-Name="Verdana" Font-Size="11px" runat="server"></asp:Label>
							    <asp:DropDownList id="dropMessagesPerPage" CssClass="pages" Font-Name="Verdana" runat="server" AutoPostBack="True"></asp:DropDownList>
	                            <div class="separator06"></div>
						    </td>
					    </tr>
				    </table>		
                    <div class="separator06"></div>			
				    <asp:DataGrid id="dgPendingMessages" CssClass="btable1" Runat="server" Width=100% PageSize="2" AllowPaging="True" AutoGenerateColumns="False"
					    cellpadding="0" cellspacing="0"  BorderWidth="0" GridLines="None">
					    <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
					    <HeaderStyle></HeaderStyle>
					    <Columns>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3" Wrap="False" Width="15%"></ItemStyle>
							    <ItemTemplate>
							    <a target="_blank" href="<%= AspNetDating.Classes.Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("FromUsername")%>"><%# Eval("FromUsername")%></a>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3" Wrap="False" Width="15%"></ItemStyle>
							    <ItemTemplate>
							    <a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("ToUsername")%>"><%# Eval("ToUsername")%></a>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2" Wrap=False></HeaderStyle>
							    <ItemStyle CssClass="table_cell3" Width="50%"></ItemStyle>
							    <ItemTemplate>
								    <%# Eval("Message")%>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3" Width="10%"></ItemStyle>
							    <ItemTemplate>
								    <nobr>
								    <asp:LinkButton ID="lnkApprove" CommandName="Approve" CommandArgument='<%# Eval("MessageID")%>' Runat=server><IMG title="Approve" border=0 SRC=images\approve.gif></asp:LinkButton>
								    <asp:LinkButton ID="lnkReject" CommandName="Reject" CommandArgument='<%# Eval("MessageID")%>'  Runat=server><IMG title="Reject" border=0 SRC=images\disapprove.gif></asp:LinkButton>
								    </nobr>
							    </ItemTemplate>
						    </asp:TemplateColumn>
						    <asp:TemplateColumn>
							    <HeaderStyle CssClass="table_header2"></HeaderStyle>
							    <ItemStyle CssClass="table_cell3" Width="10%"></ItemStyle>
							    <ItemTemplate>
								    <asp:LinkButton ID="lnkDeleteUser" CommandName="DeleteUser" CommandArgument='<%# Eval("FromUsername")%>'  Runat=server><%# Lang.TransA("Delete User")%></asp:LinkButton>
							    </ItemTemplate>
						    </asp:TemplateColumn>																						
					    </Columns>
					    <PagerStyle CssClass="table_cell" Font-Name="Verdana" Font-Size="11px" HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
				    </asp:DataGrid>
				</ContentTemplate>
				</asp:UpdatePanel>
				</div>
			</div>	
		</form>
	</body>
</HTML>
