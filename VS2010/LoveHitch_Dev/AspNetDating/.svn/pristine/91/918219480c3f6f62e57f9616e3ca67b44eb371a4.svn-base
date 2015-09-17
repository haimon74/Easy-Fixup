// JScript File

//@ shahin@themorningoutline.com provided this UNDER GNU GPL 
//@ For more inforamtion visit www.themorningoutline.com

var initProgressPanel=false;
var prgCounter=0;
var strLoadingMessage ='Loading...';
   


 function initLoader(strSplash)
 {
    if (strSplash) strLoadingMessage =strSplash;
    var myNewObj= document.getElementById('divContainer');
    if (!myNewObj )
    {
    strID='divContainer';
    strClass='divContainer';

            myNewObj = createNewDiv(strID, strClass);
            document.getElementById('layout').appendChild(myNewObj);
           //document.body.appendChild(myNewObj);
     }
   
     var myNewObj= document.getElementById('divLoadingStat');
     if (!myNewObj )
     {
            strID='divLoadingStat';  
            strClass='divLoadingStat';
            myNewObj =  createNewDiv( strID,strClass);        
            var mytext=document.createTextNode(strLoadingMessage);
            myNewObj.appendChild(mytext);
            document.getElementById('divContainer').appendChild(myNewObj);
       
        }
  
   var myNewObj= document.getElementById('divLoaderBack');
    if (!myNewObj )
    {
            strID='divLoaderBack';
            strClass='divLoaderBack';

            myNewObj =  createNewDiv( strID,strClass);
            document.getElementById('divContainer').appendChild(myNewObj);
     }
     
     var myNewObj= document.getElementById('divLoaderProgress');
     if (!myNewObj )
        {
     
            strID='divLoaderProgress';
            strClass='divLoaderProgress'
            myNewObj =  createNewDiv( strID,strClass);
            document.getElementById('divLoaderBack').appendChild(myNewObj);
     
        }
        initProgressPanel=true;
          
     }
       
       
       function setProgress(intPrc,strMessage)
  {
  
       if (!initProgressPanel) initLoader('Loading...');
      if (strMessage)  strLoadingMessage=strMessage;
      if(!intPrc) return
      var mytext=document.createTextNode( strLoadingMessage+' ' + prgCounter +'%');
      var lodStat= document.getElementById('divLoadingStat');
          lodStat.removeChild(lodStat.lastChild );
          lodStat.appendChild(mytext);
          prgCounter++;
          prgDiv= document.getElementById('divLoaderProgress');
           prgDiv.style.width=prgCounter*5+'px';
       if (prgCounter<=intPrc) 
           {
                setTimeout( 'setProgress('+intPrc+')',0.1);
            }
            else if(prgCounter>100)
            {
                 completed();
            }
 
   }

function completed()
{
  //document.body
  document.getElementById('layout').removeChild(document.getElementById('divContainer'));
}
        function createNewDiv()
        {
            newDiv = document.createElement('div');
            newDiv.id=strID;
            var styleCollection = newDiv.style;
            newDiv.className=strClass;
            return newDiv;

        }
   function resetProgress()
     {
            prgCounter=0;
             strLoadingMessage ='Loading...';
     }


