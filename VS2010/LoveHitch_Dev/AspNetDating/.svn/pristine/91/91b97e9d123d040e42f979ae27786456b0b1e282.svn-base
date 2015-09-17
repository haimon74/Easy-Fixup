var updateOnlineTimer;
var updateOnlineLock = false;
var currentNotificationDivOffset = 0;
var scrollSpeed = 1;
var scrollSign = +1;
var scrollStarted = false;

function InitializeOnlineCheck(interval)
{
    updateOnlineTimer = setInterval("UpdateOnline()", interval);
    UpdateOnline();
}

function ShowNotification()
{
    document.getElementById("divNotificationInner").scrollTop = 0;
    $get('divNotificationHeader').style.visibility = 'visible';
    $get('divNotificationInner').style.visibility = 'visible';
    if ($get('ctl00_divNotification'))
        $get('ctl00_divNotification').style.visibility = 'visible';
    if ($get('ctl00_ctl00_divNotification'))
        $get('ctl00_ctl00_divNotification').style.visibility = 'visible';
}

function HideNotification()
{
    $get('divNotificationInner').innerHTML = '';
    $get('divNotificationHeader').style.visibility = 'hidden';
    $get('divNotificationInner').style.visibility = 'hidden';
    if ($get('ctl00_divNotification'))
        $get('ctl00_divNotification').style.visibility = 'hidden';
    if ($get('ctl00_ctl00_divNotification'))
        $get('ctl00_ctl00_divNotification').style.visibility = 'hidden';
}

function UpdateOnline()
{
    if (updateOnlineLock == true) return;
    updateOnlineLock = true;
    
    AspNetDating.Services.OnlineCheck.UpdateOnline(OnUpdateOnlineSucceeded, OnUpdateOnlineError);
}

function OnUpdateOnlineSucceeded(result)
{
    try
    {
        if (result == null) return;

        if (result.Notifications != null && result.Notifications.length)
        {
            for (var i = 0; i < result.Notifications.length; i++)
            {
                var notification = result.Notifications[i];
                if (notification.Type == "NewMessageNotification")
                {
                    var eventHtml = "<table cellpadding=\"0\" cellspacing=\"0\" class=\"innerwrap\"><tr><td class=\"tdleft\" align=\"center\" valign=\"middle\"><img src=\"" + notification.ThumbnailUrl +
                        "\" /></td><td class=\"tdright\" valign=\"middle\">" + notification.Text + "</td></tr></table><div class=\"clear\"></div>";
                    eventHtml = "<div class=\"NotificationInactive\" onmouseover=\"OnNotificationMouseOver(this);\" onmouseout=\"OnNotificationMouseOut(this);\"" +
                        " onclick=\"window.location.href = 'ShowMessage.aspx?mid=" + notification.MessageId + "'\">" + eventHtml + "</div>";
                    $get('divNotificationInner').innerHTML += eventHtml;
                    ShowNotification();
                }
                if (notification.Type == "GenericEventNotification")
                {
                    var eventHtml = "<table cellpadding=\"0\" cellspacing=\"0\" class=\"innerwrap\"><tr><td class=\"tdleft\" align=\"center\" valign=\"middle\"><img src=\"" + notification.ThumbnailUrl +
                        "\" /></td><td class=\"tdright\" valign=\"middle\">" + notification.Text + "</td></tr></table><div class=\"clear\"></div>";
                    eventHtml = "<div class=\"NotificationInactive\" onmouseover=\"OnNotificationMouseOver(this);\" onmouseout=\"OnNotificationMouseOut(this);\"" +
                        " onclick=\"window.location.href = '" + notification.RedirectUrl + "'\">" + eventHtml + "</div>";
                    $get('divNotificationInner').innerHTML += eventHtml;
                    ShowNotification();
                }
            }

            if (!scrollStarted)
            {
                scrollStarted = true;
                ScrollNotificationDiv();
            }
        }
    }
    finally
    {
        updateOnlineLock = false;
    }
}

function OnNotificationMouseOver(element)
{
    element.className = 'NotificationHover';
}

function OnNotificationMouseOut(element)
{
    element.className = 'NotificationInactive';
}

function OnUpdateOnlineError()
{
    // Do nuthin'
}

function ScrollNotificationDiv()
{
    var div = document.getElementById("divNotificationInner");
    var timerDelay = 1000;
    if (div.scrollHeight > 65)
    {
        currentNotificationDivOffset = currentNotificationDivOffset + (scrollSpeed * scrollSign);
        div.scrollTop = currentNotificationDivOffset;
        if (scrollSign == 1 && currentNotificationDivOffset > div.scrollHeight - 65)
        {
            scrollSign = -1;
            timerDelay = 1000;
        }
        else if (scrollSign == -1 && currentNotificationDivOffset <= 0)
        {
            scrollSign = +1;
            timerDelay = 1000;
        }
        else
        {
            timerDelay = (currentNotificationDivOffset % 65 == 0) ? 1000 : 50;
        }
    }
    setTimeout("ScrollNotificationDiv()", timerDelay);
}
InitializeOnlineCheck(60000);