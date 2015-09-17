<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="FAQs.ascx.cs" 
            Inherits="ezFixUp.Components.FAQs" %>
<%--<%@OutputCache Duration="3600000" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<script type="text/javascript">
    jQuery(function($) {
        $("div.faqQ").live('click', function() {
            $(".faqA").each(function(){$(this).hide()});
            $(this).next(".faqA").show('bounce');
            return false;
        });
    });
</script>
<ul>
<asp:DataList ID="dlFAQsPerCategory" runat="server">
<ItemTemplate>
    <li class="liFAQ" style="list-style-type:none; ">
        <div class="faqQ" style="color:DeepPink !important; text-indent:-15px; line-height:2em; font-size:1.2em; cursor:pointer; ">
            <a style="color:DeepPink !important;"><%# (string)Eval("Question") %></a></div>
        <div class="faqA" style="color:Teal;display:none;line-height:1.2em;"><%# (string)Eval("Answer") %></div>
    </li>
</ItemTemplate>
</asp:DataList>
</ul>