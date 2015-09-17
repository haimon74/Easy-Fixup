function StartTimer()
{
    setInterval("RefreshGadgetData()", 60 * 1000);
    RefreshGadgetData();
}

function RefreshGadgetData()
{
    ezFixUp.Services.Gadgets.GetPendingPhotosCount(OnGetPendingPhotosCount_Success);
    ezFixUp.Services.Gadgets.GetPendingAnswersCount(OnGetPendingAnswersCount_Success);
    ezFixUp.Services.Gadgets.GetNewUsersForTheLast24hoursCount(OnGetNewUsersForTheLast24hoursCount_Success);
}

function OnGetPendingPhotosCount_Success(value)
{
    $get('pendingphotos').innerHTML = value;
}

function OnGetPendingAnswersCount_Success(value)
{
    $get('pendinganswers').innerHTML = value;
}

function OnGetNewUsersForTheLast24hoursCount_Success(value)
{
    $get('newusers').innerHTML = value;
}