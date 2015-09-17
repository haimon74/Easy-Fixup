function StartTimer()
{
    setInterval("RefreshGadgetData()", 60 * 1000);
    RefreshGadgetData();
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

function RefreshGadgetData()
{
    ezFixUp.Services.Gadgets.GetRandomNewUser(GetParam("gender"),
        OnGetRandomNewUser_Success);
}

function OnGetRandomNewUser_Success(value)
{
    $get('profilelink').href = value.profileUrl;
    $get('profilelink2').href = value.profileUrl;
    $get('profilelink2').innerHTML = value.username;
    $get('profileimage').src = value.imageUrl;
}