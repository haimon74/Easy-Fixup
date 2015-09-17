function pageLoad() {
    selectedGiftCount = 0;
    selectedGiftsIds = new Array();
}
function hideGiftOptions(giftTblId) {
    var jqTblSelector = "#" + giftTblId + " tr td";
    var jqAddSelector = "#" + giftTblId + " tr td.addGift";
    var jqViewSelector = "#" + giftTblId + " tr td.viewGift";
    jQuery(jqTblSelector).css('border', 'solid 2px transparent');
    jQuery(jqAddSelector).css('background', 'transparent');
    jQuery(jqViewSelector).css('background', 'transparent');
    jQuery(jqTblSelector).css('color', 'transparent');
}
function showGiftOptions(giftTblId) {
    var jqTblSelector = "#" + giftTblId + " tr td";
    var jqAddSelector = "#" + giftTblId + " tr td.addGift";
    var jqViewSelector = "#" + giftTblId + " tr td.viewGift";
    jQuery(jqTblSelector).css('border', 'solid 2px gray');
    jQuery(jqTblSelector).css('color', 'black');
    jQuery(jqAddSelector).css('background', 'powderblue');
    jQuery(jqViewSelector).css('background', 'lightpink');
}
var selectedGiftCount;
var selectedGiftsIds;
if (selectedGiftCount == null) selectedGiftCount = 0;
if (selectedGiftsIds == null) selectedGiftsIds = new Array();
function addGift(id) {
    debugger;
    var cont = true;
    for (i = 0; i < selectedGiftsIds.length; i++) {
        if (id == selectedGiftsIds[i]) cont = false;
    }
    if (cont) {
        selectedGiftCount++;
        var pnlSelector = "#" + pnlSelectedGiftsClientID;
        var uniqueId = "selgiftid" + selectedGiftCount;
        var jqUniqueId = "#" + uniqueId;
        var xDiv = "<div onclick=\"removeGift(" + id + ", '" + jqUniqueId + "');\" " +
        "style=\"valign:top;float:right;background:purple;color:white;display:inline;padding:0 0 2px 2px;text-align:right;cursor:pointer;\"><b>X</b></div>";
        var giftImgDiv = "<div id=\"" + uniqueId + "\" style=\"display:inline; width:90px;height:90px;float:left;margin:2px;border:solid 1px purple;" +
                     "background: url(GiftImage.ashx?id=" + id + "&width=90&height=90) no-repeat;\">" + xDiv +
                     "</div>"
        jQuery(pnlSelector).append(giftImgDiv);
        selectedGiftsIds.push(id);        
    }
}
function removeGift(gid, jqUniqueId) {
    jQuery(jqUniqueId).detach();
    for (i = 0; i < selectedGiftsIds.length; i++) {
        if (selectedGiftsIds[i] == gid) {
            var idx = i + 1;
            if (idx == selectedGiftsIds.length) {
                selectedGiftsIds.pop();
            }
            else {
                var arr1 = selectedGiftsIds.slice(idx);
                var arr2 = selectedGiftsIds.slice(0, i);
                selectedGiftsIds = arr1.concat(arr2);
            }
            break;
        }
    }
}
function viewGift(id) {
    alert(id);
}
var jqGiftsGalleryId = "#" + giftsGalleryClientID;
function cancelGifts() {
    selectedGiftCount = 0;
    selectedGiftsIds = new Array();
    hideGiftGallery();    
}
function hideGalleryView() {
    jQuery("#galleryView").hide();
    return false;
}
function showGalleryView() {
    jQuery("#galleryView").show();
    return false;
}
function showGiftGallery() {
    jQuery(jqGiftsGalleryId).show();
    return false;
}
function hideGiftGallery() {
    jQuery(jqGiftsGalleryId).hide();
    return false;
}
