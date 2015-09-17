function SetHeight()
{
	var divLeft = $get("left");
	var divRight = $get("right");
	if (divLeft && divRight )
	{
		divLeft.style.height =  '';	
		divRight.style.height =  '';	
		divLeft.style.minHeight =  '';	
		divRight.style.minHeight =  '';	
		if (divLeft.clientHeight > divRight.clientHeight)
		{
			if (Sys.Browser.agent == Sys.Browser.InternetExplorer && Sys.Browser.version <= 6)		
				divRight.style.height =  divLeft.clientHeight + "px";
			else
				divRight.style.minHeight =  divLeft.clientHeight + "px";
		}
		else
		{
			if (Sys.Browser.agent == Sys.Browser.InternetExplorer && Sys.Browser.version <= 6)		
				divLeft.style.height =  divRight.clientHeight + "px";
			else
				divLeft.style.minHeight =  divRight.clientHeight + "px";
		}
	}
}

function InitializeSetHeight()
{
	var prm = Sys.WebForms.PageRequestManager.getInstance();
	prm.add_endRequest(EndRequest_SetHeight);
	SetHeight();
}

function EndRequest_SetHeight(sender, args)
{
   SetHeight();
}

function fixPng(img)
{
  var imgName = img.src.toUpperCase();
  if (imgName.substring(imgName.length-3, imgName.length) == "PNG")
  {
     var imgID = (img.id) ? "id='" + img.id + "' " : ""
     var imgClass = (img.className) ? "class='" + img.className + "' " : ""
     var imgTitle = (img.title) ? "title='" + img.title + "' " : "title='" + img.alt + "' "
     var imgStyle = "display:inline-block;" + img.style.cssText 
     if (img.align == "left") imgStyle = "float:left;" + imgStyle
     if (img.align == "right") imgStyle = "float:right;" + imgStyle
     if (img.parentElement.href) imgStyle = "cursor:hand;" + imgStyle
     var strNewHTML = "<span " + imgID + imgClass + imgTitle
     + " style=\"" + "width:" + img.width + "px; height:" + img.height + "px;" + imgStyle + ";"
     + "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"
     + "(src=\'" + img.src + "\', sizingMethod='scale');\"></span>" 
     img.outerHTML = strNewHTML
  }
}

window.onload=InitializeSetHeight;
if (document.body)
{
	document.body.onresizeend=SetHeight;
}