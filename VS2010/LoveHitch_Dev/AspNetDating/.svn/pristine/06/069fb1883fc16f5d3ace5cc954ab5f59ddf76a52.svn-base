<%@ Page Language="C#" AutoEventWireup="true" Codebehind="SendFile.aspx.cs" Inherits="Estream.AjaxChat.SendFile" %>
<%@ Import namespace="Estream.AjaxChat.Classes"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%= Lang.Trans("Send file") %></title>
</head>
<body class="sendfile">
    <form id="formSendFile" runat="server">
        <div class="sendfile_wrap">
            <div class="sendfile_head">
                <h1>
                    <%= Lang.Trans("Send files") %>
                </h1>
                <%= Lang.Trans("Select a file to send, and then click send button.") %>
            </div>
            <div class="sendfile_content">
                <asp:label ID="lblError" runat="server"></asp:label>
                <div class="separator">
                </div>
                <asp:FileUpload ID="fileUpload" runat="server" />
                <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" />
            </div>
            <div class="sendfilebtn">
                <input type="button" value='<%= Lang.Trans("Close") %>' onclick="window.close();" /></div>
        </div>
    </form>
</body>
</html>
