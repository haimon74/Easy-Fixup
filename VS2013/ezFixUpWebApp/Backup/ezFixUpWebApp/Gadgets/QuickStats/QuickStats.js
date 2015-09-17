function StartTimer()
{
    setInterval("RefreshGadgetDataFast()", 60 * 1000);
    setInterval("RefreshGadgetDataSlow()", 10 * 60 * 1000);
    RefreshGadgetDataFast();
    RefreshGadgetDataSlow();
}

function GetParam(name)
{
    name = name.replace(/[\[]/,"\\\[").replace(/[\]]/,"\\\]");  
    var regexS = "[\\?&]"+name+"=([^&#]*)";  
    var regex = new RegExp( regexS );  
    var results = regex.exec( window.location.href );  
    if( results == null )    
        return "";  
    else    
        return results[1];
}

function RefreshGadgetDataFast()
{
    ezFixUp.Services.Gadgets.GetOnlineUsersCount(OnGetOnlineUsersCount_Success);
    ezFixUp.Services.Gadgets.GetNewMessagesCount(GetParam("username"), GetParam("hash"),
        OnGetNewMessagesCount_Success);
}

function RefreshGadgetDataSlow()
{
    ezFixUp.Services.Gadgets.GetNewUsersCount(GetParam("username"), GetParam("hash"),
        OnGetNewUsersCount_Success);
    ezFixUp.Services.Gadgets.GetNewGiftsCount(GetParam("username"), GetParam("hash"),
        OnGetNewEcardsCount_Success);
    ezFixUp.Services.Gadgets.GetNewProfileViewsCount(GetParam("username"), GetParam("hash"),
        OnGetNewProfileViewsCount_Success);
}

function OnGetOnlineUsersCount_Success(value)
{
    $get('onlineusers').innerHTML = value;
}

function OnGetNewMessagesCount_Success(value)
{
    $get('newmessages').innerHTML = value;
}

function OnGetNewUsersCount_Success(value)
{
    $get('newusers').innerHTML = value;
}

function OnGetNewEcardsCount_Success(value)
{
    $get('newinterests').innerHTML = value;
}

function OnGetNewProfileViewsCount_Success(value)
{
    $get('profileviews').innerHTML = value;
}