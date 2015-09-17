(function($)
{
    $.fn.imgNotes = function(n)
    {
        if (undefined != n)
        {
            notes = n;
        }

        imgOffset = $(this).offset();
        if (typeof (lastNotes) != 'undefined' && notes == lastNotes)
        {
            return;
        }
        lastNotes = notes;

        $('.photonote').remove();
        $('.photonotep').remove();

        $(notes).each(function()
        {
            appendnote(this);
        });

        $(this).click(
            function()
            {
                lastNotes = undefined;
            }
        );

        $(this).hover(
			function()
			{
			    $('.photonote').show();
			},
			function()
			{
			    $('.photonote').hide();
			    $('.photonotep').hide();
			}
		);

        $('.photonote').hover(
			function()
			{
			    $('.photonote').show();
			    $(this).next('.photonotep').show();
			    $(this).css("z-index", 10000);
			    $(this).addClass('photonote_hover');
			},
			function()
			{
			    $('.photonote').show();
			    $(this).next('.photonotep').hide();
			    $(this).css("z-index", 0);
			    $(this).removeClass('photonote_hover');
			}
		);
    }

    $.fn.outlineUsername = function(username)
    {
        $('.photonote').hide();
        $('.photonotep').hide();
        $('#note_' + username).show();
        $('#note_' + username).next('.photonotep').show();
        $('#note_' + username).css("z-index", 10000);
        $('#note_' + username).addClass('photonote_hover');
    }

    $.fn.removeOutlineUsername = function(username)
    {
        $('.photonote').hide();
        $('.photonotep').hide();
        $('#note_' + username).css("z-index", 0);
        $('#note_' + username).removeClass('photonote_hover');
    }

    function appendnote(note_data)
    {
        note_left = parseInt(imgOffset.left) + parseInt(note_data.x1);
        note_top = parseInt(imgOffset.top) + parseInt(note_data.y1);
        note_p_top = note_top + parseInt(note_data.height) + 5;
        note_url = note_data.redirectUrl;

        note_id = '';
        if (note_data.username != undefined && note_data.username != "")
        {
            $('#note_' + note_data.username).remove();
            note_id = 'id="note_' + note_data.username + '"';
            if (note_data.note != "")
                note_data.note = note_data.note + "<br />";
            note_data.note = note_data.note + "<b>" + note_data.username + "</b>";
        }

        note_area_div = $("<div " + note_id + " class='photonote'><div class='photonote2' style='height: " + (note_data.height - 2) +
            "px'><div class='photonote3' style='height: " + (note_data.height - 4) + "px'>" +
            "</div></div></div>").css({ left: note_left + 'px', top: note_top + 'px', width: note_data.width +
            'px', height: note_data.height + 'px'
            });

        note_text_div = $('<div class="photonotep">' + note_data.note + '</div>').css(
            { left: note_left + 'px', top: note_p_top + 'px' });

        $('body').append(note_area_div);
        $('body').append(note_text_div);
    }

})(jQuery);