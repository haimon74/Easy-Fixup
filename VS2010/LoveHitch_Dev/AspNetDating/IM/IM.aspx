<%@ Page language="c#" Codebehind="IM.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.IM.IM" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>IM</title>
    <meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" Content="C#">
    <meta name=vs_defaultClientScript content="JavaScript">
    <meta name=vs_targetSchema content="http://schemas.microsoft.com/intellisense/ie5">
	<script language="JavaScript" type="text/javascript">
	<!--
	// -----------------------------------------------------------------------------
	// Globals
	// Major version of Flash required
	var requiredMajorVersion = 8;
	// Minor version of Flash required
	var requiredMinorVersion = 0;
	// Minor version of Flash required
	var requiredRevision = 0;
	// the version of javascript supported
	var jsVersion = 1.0;
	// -----------------------------------------------------------------------------
	// -->
	</script>
	<script language="VBScript" type="text/vbscript">
	<!-- // Visual basic helper required to detect Flash Player ActiveX control version information
	Function VBGetSwfVer(i)
	on error resume next
	Dim swControl, swVersion
	swVersion = 0
	  
	set swControl = CreateObject("ShockwaveFlash.ShockwaveFlash." + CStr(i))
	if (IsObject(swControl)) then
		swVersion = swControl.GetVariable("$version")
	end if
	VBGetSwfVer = swVersion
	End Function
	// -->
	</script>
	<script language="JavaScript1.1" type="text/javascript">
	<!-- // Detect Client Browser type
	var isIE  = (navigator.appVersion.indexOf("MSIE") != -1) ? true : false;
	var isWin = (navigator.appVersion.toLowerCase().indexOf("win") != -1) ? true : false;
	var isOpera = (navigator.userAgent.indexOf("Opera") != -1) ? true : false;
	jsVersion = 1.1;
	// JavaScript helper required to detect Flash Player PlugIn version information
	function JSGetSwfVer(i){
		// NS/Opera version >= 3 check for Flash plugin in plugin array
		if (navigator.plugins != null && navigator.plugins.length > 0) {
			if (navigator.plugins["Shockwave Flash 2.0"] || navigator.plugins["Shockwave Flash"]) {
				var swVer2 = navigator.plugins["Shockwave Flash 2.0"] ? " 2.0" : "";
      			var flashDescription = navigator.plugins["Shockwave Flash" + swVer2].description;
				descArray = flashDescription.split(" ");
				tempArrayMajor = descArray[2].split(".");
				versionMajor = tempArrayMajor[0];
				versionMinor = tempArrayMajor[1];
				if ( descArray[3] != "" ) {
					tempArrayMinor = descArray[3].split("r");
				} else {
					tempArrayMinor = descArray[4].split("r");
				}
      			versionRevision = tempArrayMinor[1] > 0 ? tempArrayMinor[1] : 0;
				flashVer = versionMajor + "." + versionMinor + "." + versionRevision;
      		} else {
				flashVer = -1;
			}
		}
		// MSN/WebTV 2.6 supports Flash 4
		else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.6") != -1) flashVer = 4;
		// WebTV 2.5 supports Flash 3
		else if (navigator.userAgent.toLowerCase().indexOf("webtv/2.5") != -1) flashVer = 3;
		// older WebTV supports Flash 2
		else if (navigator.userAgent.toLowerCase().indexOf("webtv") != -1) flashVer = 2;
		// Can't detect in all other cases
		else {
			
			flashVer = -1;
		}
		return flashVer;
	} 
	// When called with reqMajorVer, reqMinorVer, reqRevision returns true if that version or greater is available
	function DetectFlashVer(reqMajorVer, reqMinorVer, reqRevision) 
	{
 		reqVer = parseFloat(reqMajorVer + "." + reqRevision);
   		// loop backwards through the versions until we find the newest version	
		for (i=25;i>0;i--) {	
			if (isIE && isWin && !isOpera) {
				versionStr = VBGetSwfVer(i);
			} else {
				versionStr = JSGetSwfVer(i);		
			}
			if (versionStr == -1 ) { 
				return false;
			} else if (versionStr != 0) {
				if(isIE && isWin && !isOpera) {
					tempArray         = versionStr.split(" ");
					tempString        = tempArray[1];
					versionArray      = tempString .split(",");				
				} else {
					versionArray      = versionStr.split(".");
				}
				versionMajor      = versionArray[0];
				versionMinor      = versionArray[1];
				versionRevision   = versionArray[2];
				
				versionString     = versionMajor + "." + versionRevision;   // 7.0r24 == 7.24
				versionNum        = parseFloat(versionString);
        		// is the major.revision >= requested major.revision AND the minor version >= requested minor
				if ( (versionMajor > reqMajorVer) && (versionNum >= reqVer) ) {
					return true;
				} else {
					return ((versionNum >= reqVer && versionMinor >= reqMinorVer) ? true : false );	
				}
			}
		}	
	}
	// -->
	</script>    
  </head>
  <body bottommargin="0" leftmargin="0" marginheight="0" marginwidth="0" rightmargin="0" topmargin="0">
	
    <form id="Form1" method="post" runat="server">
<% if (isInitiator && isUserBlocked){}
	else
	{
%>
	<script language="JavaScript" type="text/javascript">
	<!-- 
	// Version check for the Flash Player that has the ability to start Player Product Install (6.0r65)
	var hasProductInstall = DetectFlashVer(6, 0, 65);

	// Version check based upon the values entered above in "Globals"
	var hasReqestedVersion = DetectFlashVer(requiredMajorVersion, requiredMinorVersion, requiredRevision);

	// Location visited after installation is complete if installation is required
	var MMredirectURL = window.location;

	// Stored value of document title used by the installation process to close the window that started the installation process
	// This is necessary to remove browser windows that will still be utilizing the older version of the player after installation is complete
	// DO NOT MODIFY THE FOLLOWING TWO LINES
	document.title = document.title.slice(0, 47) + " - Flash Player Installation";
	var MMdoctitle = document.title;

	// Check to see if a player with Flash Product Install is available and the version does not meet the requirements for playback
	if ( hasProductInstall && !hasReqestedVersion ) {
		var productInstallOETags = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"'
		+ 'width="360" height="460"'
		+ 'codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab">'
		+ '<param name="movie" value="playerUpdater.swf?MMredirectURL='+MMredirectURL+'&MMplayerType=ActiveX&MMdoctitle='+MMdoctitle+'" />'
		+ '<param name="quality" value="high" /><param name="bgcolor" value="#3A6EA5" />'
		+ '<embed src="playerUpdater.swf?MMredirectURL='+MMredirectURL+'&MMplayerType=PlugIn" quality="high" bgcolor="#3A6EA5" '
		+ 'width="360" height="460" name="detectiontest" aligh="middle"'
		+ 'play="true"'
		+ 'loop="false"'
		+ 'quality="high"'
		+ 'allowScriptAccess="sameDomain"'
		+ 'type="application/x-shockwave-flash"'
		+ 'pluginspage="https://www.macromedia.com/go/getflashplayer">'
		+ '<\/embed>'
		+ '<\/object>';
		document.write(productInstallOETags);   // embed the Flash Product Installation SWF
	} else if (hasReqestedVersion) {  // if we've detected an acceptable version
		var oeTags = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"'
		+ 'width="360" height="460"'
		+ 'codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab">'
		+ '<param name="movie" value="av_chat.swf?sessID=<%= strSessID %>&myName=<%= strUserID %>&chatToUsername=<%= strTargetUserID%>&myRealName=<%= strUserRealName %>&userRealName=<%= strTargetUserRealName%>&chatToSex=<%= strSex%>&chatToAge=<%= strAge%>&chatToLocation=<%= strLocation%>&" /><param name="quality" value="high" /><param name="bgcolor" value="#CCCCCC" />'
		+ '<embed src="av_chat.swf?sessID=<%= strSessID %>&myName=<%= strUserID %>&chatToUsername=<%= strTargetUserID%>&myRealName=<%= strUserRealName %>&userRealName=<%= strTargetUserRealName%>&chatToSex=<%= strSex%>&chatToAge=<%= strAge%>&chatToLocation=<%= strLocation%>&" quality="high" bgcolor="#CCCCCC" '
		+ 'width="360" height="460" name="detectiontest" align="middle"'
		+ 'play="true"'
		+ 'loop="false"'
		+ 'quality="high"'
		+ 'allowScriptAccess="sameDomain"'
		+ 'type="application/x-shockwave-flash"'
		+ 'pluginspage="https://www.macromedia.com/go/getflashplayer">'
		+ '<\/embed>'
		+ '<\/object>';
		document.write(oeTags);   // embed the Flash Content SWF when all tests are passed
	} else {  
		var oeTags = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"'
		+ 'width="360" height="460"'
		+ 'codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab">'
		+ '<param name="movie" value="av_chat.swf?sessID=<%= strSessID %>&myName=<%= strUserID %>&chatToUsername=<%= strTargetUserID%>&myRealName=<%= strUserRealName %>&userRealName=<%= strTargetUserRealName%>&chatToSex=<%= strSex%>&chatToAge=<%= strAge%>&chatToLocation=<%= strLocation%>&" /><param name="quality" value="high" /><param name="bgcolor" value="#CCCCCC" />'
		+ '<embed src="av_chat.swf?sessID=<%= strSessID %>&myName=<%= strUserID %>&chatToUsername=<%= strTargetUserID%>&myRealName=<%= strUserRealName %>&userRealName=<%= strTargetUserRealName%>&chatToSex=<%= strSex%>&chatToAge=<%= strAge%>&chatToLocation=<%= strLocation%>&" quality="high" bgcolor="#CCCCCC" '
		+ 'width="360" height="460" name="detectiontest" align="middle"'
		+ 'play="true"'
		+ 'loop="false"'
		+ 'quality="high"'
		+ 'allowScriptAccess="sameDomain"'
		+ 'type="application/x-shockwave-flash"'
		+ 'pluginspage="https://www.macromedia.com/go/getflashplayer">'
		+ '<\/embed>'
		+ '<\/object>';
		document.write(oeTags);   // embed the Flash Content SWF when all tests are passed	
	    // flash is too old or we can't detect the plugin
		/*var alternateContent = 'Alternate HTML content should be placed here.'
  		+ 'This content requires the Macromedia Flash Player.'
   		+ '<a href="https://www.macromedia.com/go/getflash/" target="_blank">Get Flash</a>';
		document.write(alternateContent); */ // insert non-flash content
	}
	// -->
	</script>
	<noscript>
		// Provide alternate content for browsers that do not support scripting
		// or for those that have scripting disabled.
  		This content requires the Macromedia Flash Player.
  		<a href="https://www.macromedia.com/go/getflash/" target="_blank">Get Flash</a>  	
	</noscript>
<% }%>
    </form>
	
  </body>
</html>
