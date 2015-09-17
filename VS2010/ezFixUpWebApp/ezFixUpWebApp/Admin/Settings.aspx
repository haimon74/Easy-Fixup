﻿<%@ Page language="c#" Codebehind="Settings.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.Admin.Settings" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Settings") %></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
		<script type="text/javascript">

			/***********************************************
			* Show Hint script-  Dynamic Drive (www.dynamicdrive.com)
			* This notice MUST stay intact for legal use
			* Visit https://www.dynamicdrive.com/ for this script and 100s more.
			***********************************************/
					
			var horizontal_offset="9px" //horizontal offset of hint box from anchor link
	
			/////No further editting needed
	
			var vertical_offset="0" //horizontal offset of hint box from anchor link. No need to change.
			var ie=document.all
			var ns6=document.getElementById&&!document.all
	
			function getposOffset(what, offsettype){
			var totaloffset=(offsettype=="left")? what.offsetLeft : what.offsetTop;
			var parentEl=what.offsetParent;
			while (parentEl!=null){
			totaloffset=(offsettype=="left")? totaloffset+parentEl.offsetLeft : totaloffset+parentEl.offsetTop;
			parentEl=parentEl.offsetParent;
			}
			return totaloffset;
			}
	
			function iecompattest(){
			return (document.compatMode && document.compatMode!="BackCompat")? document.documentElement : document.body
			}
	
			function clearbrowseredge(obj, whichedge){
			var edgeoffset=(whichedge=="rightedge")? parseInt(horizontal_offset)*-1 : parseInt(vertical_offset)*-1
			if (whichedge=="rightedge"){
			var windowedge=ie && !window.opera? iecompattest().scrollLeft+iecompattest().clientWidth-30 : window.pageXOffset+window.innerWidth-40
			dropmenuobj.contentmeasure=dropmenuobj.offsetWidth
			if (windowedge-dropmenuobj.x < dropmenuobj.contentmeasure)
			edgeoffset=dropmenuobj.contentmeasure+obj.offsetWidth+parseInt(horizontal_offset)
			}
			else{
			var windowedge=ie && !window.opera? iecompattest().scrollTop+iecompattest().clientHeight-15 : window.pageYOffset+window.innerHeight-18
			dropmenuobj.contentmeasure=dropmenuobj.offsetHeight
			if (windowedge-dropmenuobj.y < dropmenuobj.contentmeasure)
			edgeoffset=dropmenuobj.contentmeasure-obj.offsetHeight
			}
			return edgeoffset
			}
	
			function showhint(menucontents, obj, e, tipwidth){
			if ((ie||ns6) && document.getElementById("hintbox")){
			dropmenuobj=document.getElementById("hintbox")
			dropmenuobj.innerHTML=menucontents
			dropmenuobj.style.left=dropmenuobj.style.top=-500
			if (tipwidth!=""){
			dropmenuobj.widthobj=dropmenuobj.style
			dropmenuobj.widthobj.width=tipwidth
			}
			dropmenuobj.x=getposOffset(obj, "left")
			dropmenuobj.y=getposOffset(obj, "top")
			dropmenuobj.style.left=dropmenuobj.x-clearbrowseredge(obj, "rightedge")+obj.offsetWidth+"px"
			dropmenuobj.style.top=dropmenuobj.y-clearbrowseredge(obj, "bottomedge")+"px"
			dropmenuobj.style.visibility="visible"
			obj.onmouseout=hidetip
			}
			}
	
			function hidetip(e){
			dropmenuobj.style.visibility="hidden"
			dropmenuobj.style.left="-500px"
			}
	
			function createhintbox(){
			var divblock=document.createElement("div")
			divblock.setAttribute("id", "hintbox")
			document.body.appendChild(divblock)
			}
	
			if (window.addEventListener)
			window.addEventListener("load", createhintbox, false)
			else if (window.attachEvent)
			window.attachEvent("onload", createhintbox)
			else if (document.getElementById)
			window.onload=createhintbox
		</script>		
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
		            <img src="images/i_settings.jpg"> 
		            <div id="crumpstxt"><%= Lang.TransA("Settings") %></div>
				</div>
				<div id="sidebar">
		            <uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
					<div id="content-head">
			            <div class="ch-head"><%= Lang.TransA("Settings") %></div>
			            <div class="ch-description"><%= Lang.TransA("Here you can customize your website by modifying its settings...") %></div>
					</div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>	
						<table width="100%" border="0" cellpadding="0" cellspacing="0" >
							<tr>
							<td id="settnigstbl">
								<asp:placeholder id="phSettings" runat="server"></asp:placeholder>
							</td>
						</tr>
						<tr>
							<td>
								<br>
								<table cellpadding="0" cellspacing="0" class="filter">
									<tr>
										<td class="table_header" align="center" colspan="2">
											<%= Lang.TransA("Change Your Password") %>
										</td>
									</tr>
									<tr>
										<td class="table_cell"><%= Lang.TransA("Current password") %></td>
										<td class="table_cell"><asp:textbox id="txtCurrentPassword" Runat="server" TextMode="Password"></asp:textbox></td>
									</tr>
									<tr>
										<td class="table_cell"><%= Lang.TransA("New password") %></td>
										<td class="table_cell"><asp:TextBox ID="txtNewPassword" Runat="server" TextMode="Password"></asp:TextBox></td>
									</tr>
									<tr>
										<td class="table_cell"><%= Lang.TransA("Confirm new password") %></td>
										<td class="table_cell"><asp:TextBox ID="txtConfirmNewPassword" Runat="server" TextMode="Password"></asp:TextBox></td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
					<div class="separator10"></div>
					<div class="add-buttons">
						<asp:button id="btnSave" runat="server" onclick="btnSave_Click"></asp:button>
					</div>
					<div class="separator10"></div>
				</div>
			</div>
		</form>
	</body>
</HTML>
