<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AffiliateMenu.ascx.cs" Inherits="ezFixUp.Affiliates.AffiliateMenu" %>

<div id="leftnav">
    <div class="navlabel">
        <%= Lang.Trans("Menu") %>
    </div>
    <div class="navblock">
        <ul>
            <li><a href="EditAffiliate.aspx">
                <%= Lang.Trans("Edit Account Details")%>
            </a></li>
            <li><a href="PaymentHistory.aspx">
                <%= Lang.Trans("Payment History")%>
            </a></li>
            <li><a href="AffiliateBanners.aspx">
                <%= Lang.Trans("Affiliate Banners")%>
            </a></li>
            <li><a href="CommissionsHistory.aspx">
                <%= Lang.Trans("View Commissions")%>
            </a></li>
        </ul>
    </div>
</div>