function SetHeight()
{
	if (document.documentElement.clientHeight > 220)
	{
		$get("left").style.height = document.documentElement.clientHeight - 152 + "px";
		$get("right").style.height = document.documentElement.clientHeight - 138 + "px";       	
		$get("usersright").style.height = document.documentElement.clientHeight - 220 + "px";
		$get("messages").style.height = document.documentElement.clientHeight - 194 + "px";
		$get("messages").scrollTop = $get("messages").scrollHeight;
	}
	if (document.documentElement.clientWidth > 163)
	{
		$get("left").style.width = document.documentElement.clientWidth - 163 + "px";
	    $get("txtSendMessage").style.width = document.documentElement.clientWidth - 100 + "px";
	}
	
    var loginBox = $get("loginBox");
    loginBox.style.left = (loginBox.offsetParent.clientWidth/2)-(loginBox.clientWidth/2)+loginBox.offsetParent.scrollLeft + "px";
    loginBox.style.top = (loginBox.offsetParent.clientHeight/2)-(loginBox.clientHeight/2)+loginBox.offsetParent.scrollTop + "px";
}

function SetHeightNoHeader()
{
	$get("header").style.display="none";
    $get("left").style.top="0px";
    $get("right").style.top="0px";
	if (document.documentElement.clientHeight > 160)
	{
		$get("left").style.height = document.documentElement.clientHeight - 92 + "px";
		$get("right").style.height = document.documentElement.clientHeight - 78 + "px";       	
		$get("usersright").style.height = document.documentElement.clientHeight - 160 + "px";
		$get("messages").style.height = document.documentElement.clientHeight - 134 + "px";
		$get("messages").scrollTop = $get("messages").scrollHeight;
	}
	if (document.documentElement.clientWidth > 163)
	{
		$get("left").style.width = document.documentElement.clientWidth - 163 + "px";
	    $get("txtSendMessage").style.width = document.documentElement.clientWidth - 100 + "px";
	}
	
    var loginBox = $get("loginBox");
    loginBox.style.left = (loginBox.offsetParent.clientWidth/2)-(loginBox.clientWidth/2)+loginBox.offsetParent.scrollLeft + "px";
    loginBox.style.top = (loginBox.offsetParent.clientHeight/2)-(loginBox.clientHeight/2)+loginBox.offsetParent.scrollTop + "px";

}

function SetMessengerHeight()
{
	if (document.documentElement.clientHeight > 210)
	{
		$get("am_content-wrap_left").style.height = document.documentElement.clientHeight - 114 + "px";
		$get("am_content-wrap_right").style.height = document.documentElement.clientHeight - 208 + "px";
		$get("messages").style.height = document.documentElement.clientHeight - 160 + "px";
		$get("messages").scrollTop = $get("messages").scrollHeight;
	}
	if (document.documentElement.clientWidth > 217)
	{
		$get("am_content-wrap_left").style.width = document.documentElement.clientWidth - 217 + "px";
		$get("am_footer_left").style.width = document.documentElement.clientWidth - 217 + "px";
	}
}