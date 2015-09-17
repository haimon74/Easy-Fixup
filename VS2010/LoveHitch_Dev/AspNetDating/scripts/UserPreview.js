/// <reference name="MicrosoftAjax.js"/>

var mouseX, mouseY;
var lastPreviewedUsername;
var userPreviewLoading = false;
var userPreviewCancelled = false;

function InitializeMouseTracking() 
{
    $addHandler($get(bodyId), "mousemove", trackMouse);
}

function trackMouse(eventElement)
{
    mouseX = eventElement.clientX;
    mouseY = eventElement.clientY;
}

function showUserPreview(username)
{
    if (typeof AspNetDating == 'undefined' || userPreviewLoading) return;
    userPreviewLoading = true;
    AspNetDating.Services.OnlineCheck.LoadUserPreviewInfo(username, OnLoadUserPreviewInfoSucceeded, 
        OnLoadUserPreviewInfoError);
}

function OnLoadUserPreviewInfoSucceeded(result)
{
    if (userPreviewCancelled)
    {
        userPreviewCancelled = false;
        userPreviewLoading = false;
        return;
    }
    if (result == null || lastPreviewedUsername == result.Username) return;

    // Compute the initial offset
    var offsetX, offsetY;
    if (document.documentElement && document.documentElement.scrollTop) 
    {
        offsetX = document.documentElement.scrollLeft;
        offsetY = document.documentElement.scrollTop;
    } else {
        offsetX = document.body.scrollLeft;
        offsetY = document.body.scrollTop;
    }
    
    lastPreviewedUsername = result.Username;
    if (result.ThumbnailUrl != null && result.ThumbnailUrl != '')
    {
        $get("divUserPreviewImage").innerHTML = String.format("<img src=\"{0}\" class=\"photoframe\">", 
            result.ThumbnailUrl);
    }
    else
    {
        $get("divUserPreviewImage").innerHTML = "";
    }
    $get("divUserPreviewDetails").innerHTML = String.format("<b>{0}</b><br>{1}/{2}<br>{3}",
        result.Username, result.Gender, result.Age, result.LastOnline);
    $get("divUserPreview").style.left = (offsetX + mouseX + 10) + 'px';
    $get("divUserPreview").style.top = (offsetY + mouseY - 20) + 'px';
    $get("divUserPreview").style.display = '';
    
    userPreviewLoading = false;
}

function OnLoadUserPreviewInfoError()
{
    // Do nuthin'
}

function hideUserPreview(username)
{
    lastPreviewedUsername = null;
    $get("divUserPreview").style.display = 'none';
    if (userPreviewLoading)
        userPreviewCancelled = true;
}