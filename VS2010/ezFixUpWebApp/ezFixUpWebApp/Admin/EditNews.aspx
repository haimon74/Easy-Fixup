<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>

<%@ Page Language="c#" Codebehind="EditNews.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.Admin.EditNews" %>

<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head runat="server">
	    <title><%= Lang.TransA("Edit News") %></title>
	    <meta content="C#" name="CODE_LANGUAGE">
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
    	<form id="Form1" method="post" runat="server">

            <uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_editnews.jpg"> 
					<div id="crumpstxt"><%= Lang.TransA("Edit News") %></div>
				</div>
				<div id="sidebar">
					<uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
				</div>
				<div id="main">
					<div id="content-head">
					    <div class="ch-head"><%= Lang.TransA("Edit News") %></div>
			            <div class="ch-description"><%= Lang.TransA("Use this section to edit your website news...") %></div>
					</div>
					<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>--%>
                    <div id="pnlLanguage" runat="server">
                        <table cellSpacing="0" cellPadding="0" class="filter">
                		    <tr>
							    <td class="table_header" colSpan="2"><%= Lang.TransA("Languages") %></td>
						    </tr>
						    <tr>
                                <td class="table_cell">
                                    <%= Lang.TransA("Select Language:") %>
                                </td>
                                <td class="table_cell">
                                    <asp:DropDownList ID="ddLanguage" AutoPostBack="True" runat="server" onselectedindexchanged="ddLanguage_SelectedIndexChanged">
                                    </asp:DropDownList></td>
                            </tr>
                        </table>
                    </div>
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
                    <div class="separator10"></div>
                    <div id="divNews" runat="server" visible="false">
                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:DataGrid ID="dgNews" CssClass="btable1" Width="100%"  BorderWidth="0" 
                                        PageSize="10" AllowPaging="False" AutoGenerateColumns="False" CellPadding="0"
                                        runat="server" GridLines="None" onitemcommand="dgNews_ItemCommand" 
                                        onitemdatabound="dgNews_ItemDataBound">
                                        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                                        <HeaderStyle></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderStyle CssClass="table_header2c" Wrap="False">
                                                </HeaderStyle>
                                                <ItemStyle CssClass="table_cell3"></ItemStyle>
                                                <ItemTemplate>
                                                    <span><%# Eval("Title")%></span>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle CssClass="table_header2" Wrap="False">
                                                </HeaderStyle>
                                                <ItemStyle CssClass="table_cell3" Width="220px"></ItemStyle>
                                                <ItemTemplate>
                                                    <span><%# ((DateTime)Eval("Date")).ToShortDateString() %></span>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn>
                                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                                <ItemTemplate>
                                                    <nobr>
                                                        <asp:LinkButton id="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("NID") %>' CssClass="gridlink">
                                                            <img src="images/edit_icon.jpg" border="0">
                                                            <%= Lang.TransA("Edit")%>
                                                        </asp:LinkButton>
                                                            &nbsp;
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("NID") %>' CssClass="gridlink">
                                                            <img src="images/edit_icon.jpg" border="0">
                                                            <%= Lang.TransA("Delete")%></asp:LinkButton>
                                                    </nobr>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>                                            
                                        </Columns>
                                        <PagerStyle CssClass="table_cell" HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                                    </asp:DataGrid>
                            	</td>
                    		</tr>
						</table>
						<div class="separator06"></div>
					    <div class="add-buttons">
	                		<asp:Button ID="btnAddNews" runat="server" onclick="btnAddNews_Click"></asp:Button>
	                	</div>	
						<div class="separator10"></div>	
                        <div style="width:550px; margin:0; padding:0 0 0 20px">					
                        <div id="pnlAddEditNews" visible="false" runat="server">
                            <%= "Publish date:".TranslateA() + DateTime.Now.ToString()%>
                            <uc2:DatePicker id="Calendar" DateRestriction="false" runat="server" Visible="false">
                            </uc2:DatePicker>                    
                        <div class="separator10"></div>
                            <%= "News title:".TranslateA() %>
                            <asp:TextBox CssClass="txtNewsTitle" Width="98%" ID="txtNewsTitle" runat="server" Columns="61" /><div class="separator10"></div>
                            <%= "News content:".TranslateA() %>
                            <%--<asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>--%>
                            <br />
                            <asp:TextBox ID="txtEditor" runat="server" TextMode="MultiLine" Rows="20" Width="90%"></asp:TextBox>
                            <div class="separator10"></div>	
                            <div align="center">
                            <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click"></asp:Button>
                            <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" />             
                            </div>
                            <div class="separator10"></div>	
                        </div>	</div>					
                    </div>
                  <%-- </ContentTemplate>
                  </asp:UpdatePanel>--%>
				</div>
			</div>
	    </form>
	</body>
</html>
