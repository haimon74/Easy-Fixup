<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginCardSpace.aspx.cs"
    Inherits="ezFixUp.LoginCardSpace" Theme="" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Log in using CardSpace</title>
</head>
<body style="margin: 0; padding: 0; background-image: url(images/infocard_300x210.png); background-repeat: no-repeat; cursor: pointer"
 onclick="document.form1.submit();">
    <form id="form1" runat="server">
    <div>
        <object type="application/x-informationcard" name="xmlToken">
            <param name="tokenType" value="urn:oasis:names:tc:SAML:1.0:assertion" />
            <param name="issuer" value="http://schemas.xmlsoap.org/ws/2005/05/identity/issuer/self" />
            <param name="requiredClaims" value="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/locality
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/stateorprovince
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/gender
                    http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier" />
        </object>
    </div>
    </form>
</body>
</html>
