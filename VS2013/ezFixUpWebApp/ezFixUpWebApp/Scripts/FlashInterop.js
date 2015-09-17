var webcamdetected = false;

function webcamdetect_DoFSCommand(command, args) 
{ 
    var divNoWebCam = $get("nowebcam");
    var divWebCamPresent = $get("webcampresent");
    
    if (command == 'webcam_detected')
    {
        if (args == "true")
        {
            webcamdetected = true;
            if (divWebCamPresent != null)
            {
                divWebCamPresent.style.display = '';
                divNoWebCam.style.display = 'none';
            }
        }
        else if (args == "false")
        {
            webcamdetected = false;
            
            if (divWebCamPresent != null)
            {
                divWebCamPresent.style.display = 'none';
                divNoWebCam.style.display = '';
            }            
        }
    }
}
