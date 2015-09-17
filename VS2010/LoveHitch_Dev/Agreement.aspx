<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Agreement.aspx.cs" Inherits="AspNetDating.Agreement" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:wideboxstart id="WideBoxStart1" runat="server"></uc1:wideboxstart>
		<components:ContentView ID="cvAgreement" Key="Agreement" runat="server"></components:ContentView>
	<uc1:wideboxend id="WideBoxEnd1" runat="server"></uc1:wideboxend>
</asp:Content>
