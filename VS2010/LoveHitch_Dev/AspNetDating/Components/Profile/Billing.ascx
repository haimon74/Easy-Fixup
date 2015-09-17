<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="Billing.ascx.cs" 
            Inherits="AspNetDating.Components.Profile.Billing" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<uc1:LargeBoxStart ID="LargeBoxStart" runat="server"></uc1:LargeBoxStart>
<input id="hidUsername" type="hidden" name="hidUsername" runat="server" />
<div class="extraPadContent">
    <div id="divPaymentType" runat="server">
        <uc1:HeaderLine ID="hlPaymentType" runat="server"></uc1:HeaderLine>
        <components:ContentView ID="cvPaymentTypeInfo" Key="PaymentTypeInfo" runat="server">
        </components:ContentView>
        <div>
            <asp:RadioButton ID="radioSubscription" GroupName="PaymentType" AutoPostBack="true" 
                OnCheckedChanged="radioPaymentType_CheckedChanged" runat="server"  />
        </div>
        <div class="separator"></div>
    <div id="divSubscriptionTypes" visible="false" runat="server" style='margin-left:50px;'>
        <uc1:HeaderLine ID="hlSubscriptionTypes" runat="server"></uc1:HeaderLine>
        <asp:Label ID="lblCurrentPlan" runat="server"></asp:Label>
        <div class="wrap-view">
            <asp:RadioButtonList ID="rlPlans" runat="server" Visible="false" />            
            <asp:DataList ID="dlSubscriptionPlans" runat="server"  Visible="true" 
                            RepeatColumns="4" RepeatDirection="Horizontal" RepeatLayout="Table"
                           BorderColor="#E6F3FB" CellSpacing="2"  EnableViewState="false"
                           CellPadding="5" GridLines="None" BackColor="Transparent" BorderWidth="10"
                           >
                <ItemStyle  BackColor="White" BorderWidth="10" BorderColor="#E6F3FB" Width="130px" 
                            Font-Bold="true" Font-Size="Large" HorizontalAlign="Center" CssClass='roundedBox10px' /> 
                <ItemTemplate >
                    <div class="fl roundedBox10px" style="text-align:center;width:100%;margin:0 auto;">
                        <p><%# (string)Eval("Name")%></p>
                        <p>$USD <%# (string)Eval("Amount") %></p>
                        <p><%# String.Format("x {0} {1}", (int)Eval("Cycles"), "months".Translate())%></p>
                        <p><%# String.Format("+ {0} {1}", (string)Eval("Bonus"), "credits".Translate())%></p>
                        <%--<form action="https://www.paypal.com/cgi-bin/webscr" method="post">
                             <input type="hidden" name="cmd" value="_xclick" />
                             <input type="hidden" name="business" value="Enter your PayPal e-mail address here..." />
                             <input type="hidden" name="button_subtype" value="products" />
                             <input type="image" src="https://www.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!" />
                        </form>--%>
                        <input type="image" src="https://www.paypal.com/en_US/i/btn/btn_subscribeCC_LG.gif" 
                               style='width:120px; height:40px;border:0;' 
                               onmousedown="$('input[name=pkg_id]').attr('value',''); $('input[name=plan_id]').attr('value','<%# (int)Eval("PlanId")%>')"
                               name="submit" alt="PayPal - The safer, easier way to pay online!" />
                        
                        <%--<button id='BuyNowBtn' planId='<%# (int)Eval("PlanId")%>'> <%# "SUBSCRIBE".Translate()%> </button>--%>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <input type="hidden" name="plan_id"  value='' />
        </div>
    </div>
        <div>
            <asp:RadioButton ID="radioCredits" GroupName="PaymentType" AutoPostBack="true" 
                OnCheckedChanged="radioPaymentType_CheckedChanged" runat="server" />
        </div>
        <div class="separator"></div>
        <div id="divCreditPackages" visible="false" runat="server"  style='margin-left:50px;'>
        <uc1:HeaderLine ID="hlCreditPackages" runat="server"></uc1:HeaderLine>
        <div class="wrap-view">
            <asp:RadioButtonList ID="rlCreditPackages" runat="server" Visible="false"/>
            <asp:DataList ID="dlCreditPackages" runat="server"  Visible="true" EnableViewState="false"
                           BorderColor="Transparent" CellSpacing="2"  BorderWidth="10"
                           RepeatDirection="Horizontal" RepeatLayout="table" RepeatColumns="4"
                           CellPadding="5" GridLines="None" BackColor="Transparent"                           
                           >
                <ItemStyle  BackColor="White" BorderWidth="10" BorderColor="#E6F3FB" Width="130px" 
                            Font-Bold="true" Font-Size="Large" HorizontalAlign="Center" CssClass="roundedBox10px" />                
                <ItemTemplate >
                    <div class="fl roundedBox10px" style="text-align:center;width:100%;margin:0 auto;">
                        <p><%# (string)Eval("Name")%></p>
                        <p><%# (int)Eval("Credits")%></p>
                        <p>$USD <%# (string)Eval("Amount")%></p>
                        <input type="image" src="https://www.paypal.com/en_US/i/btn/btn_buynowCC_LG.gif" 
                               style='width:120px; height:40px;border:0;' 
                               onmousedown="$('input[name=plan_id]').attr('value',''); $('input[name=pkg_id]').attr('value','<%# (int)Eval("PlanId")%>')"
                               name="submit" alt="PayPal - The safer, easier way to pay online!" />
                        <%--<button id='BuyNowBtn' planId='<%# (int)Eval("PlanId")%>'> <%# "BUY NOW".Translate()%> </button>--%>
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <input type="hidden" name="pkg_id"  value='' />
        </div>
    </div>
    </div>
    <div class="separator">
    </div>
    <div id="divPaymentMessage" visible="false" runat="server">
        <uc1:HeaderLine ID="hlPaymentMessage" runat="server"></uc1:HeaderLine>
    </div>
    
    <div class="separator">
    </div>
    <div id="divPaymentMethods" visible="false" runat="server">
        <uc1:HeaderLine ID="hlPaymentMethods" runat="server"></uc1:HeaderLine>
        <asp:RadioButtonList ID="radiolistPaymentMethods" Width="100%" RepeatColumns="3"
            RepeatLayout="Table" AutoPostBack="true" OnSelectedIndexChanged="radiolistPaymentMethods_SelectedIndexChanged"
            CssClass="PaymentMethodsWrap" runat="server" CellPadding="0" CellSpacing="0" />
    </div>
    <div class="separator">
    </div>
    <div id="divCouponPayment" visible="false" runat="server">
        <uc1:HeaderLine ID="hlCouponPayment" runat="server"></uc1:HeaderLine>
        <asp:TextBox ID="txtCoupon" runat="server"></asp:TextBox>
        <asp:Label ID="lblCouponMsg" runat="server" Font-Bold="true" BorderColor="Gray"
             style="margin:5px;padding:5px;"></asp:Label>
        <div class="buttons">
            <asp:Button ID="btnUseCoupon" OnClick="btnUseCoupon_Click" runat="server" />
        </div>        
    </div>
    <div id="divOutsidePayment" visible="false" runat="server">
        <uc1:HeaderLine ID="hlOutsidePayment" runat="server"></uc1:HeaderLine>
        <div class="buttons">
            <asp:Button ID="btnOutsidePayment" OnClick="btnOutsidePayment_Click" runat="server" /></div>
    </div>
    <div id="divCreditCardPayment" visible="false" runat="server">
        <uc1:HeaderLine ID="hlCreditCardPayment" runat="server"></uc1:HeaderLine>
        <div class="wrap-view">
            <table cellpadding="4" cellspacing="0" class="settings">
                <tr>
                    <td>
                        <%= "First name".Translate() %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "Last name".Translate() %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "Address".Translate() %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "City".Translate() %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "State".Translate() %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtState" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "ZIP".Translate() %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "Country".Translate() %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "Phone".Translate() %>
                        <div class="separator">
                        </div>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
                        <div class="separator">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "Card Number".Translate() %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCardNumber" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= "Card Expiration".Translate() %>
                    </td>
                    <td>
                        <asp:DropDownList CssClass="dropdownlist" ID="ddMonth" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>01</asp:ListItem>
                            <asp:ListItem>02</asp:ListItem>
                            <asp:ListItem>03</asp:ListItem>
                            <asp:ListItem>04</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>06</asp:ListItem>
                            <asp:ListItem>07</asp:ListItem>
                            <asp:ListItem>08</asp:ListItem>
                            <asp:ListItem>09</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList CssClass="dropdownlist" ID="ddYear" runat="server">
                            <asp:ListItem></asp:ListItem>
                            <asp:ListItem>2010</asp:ListItem>
                            <asp:ListItem>2011</asp:ListItem>
                            <asp:ListItem>2012</asp:ListItem>
                            <asp:ListItem>2013</asp:ListItem>
                            <asp:ListItem>2014</asp:ListItem>
                            <asp:ListItem>2015</asp:ListItem>
                            <asp:ListItem>2016</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="buttons">
            <asp:Button ID="btnCreditCardPayment" OnClick="btnCreditCardPayment_Click" runat="server" /></div>
    </div>
    <div id="divCheckPayment" visible="false" runat="server">
        <uc1:HeaderLine ID="hlCheckPayment" runat="server"></uc1:HeaderLine>
        <components:ContentView ID="cvCheckPayment" Key="CheckPaymentInfo" runat="server"></components:ContentView>
    </div>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd" runat="server"></uc1:LargeBoxEnd>
