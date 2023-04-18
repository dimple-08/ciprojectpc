

function loadItems(page, missionId) {
  
    $.ajax({
        url: '/Home/RelatedMissionPage',
        type: 'POST',
        data: { pageIndex: page, id: missionId },
        success: function (result) {
            $('.recentvol').html($(result).find('.recentvol').html());
        }
    });
}





function ratemission(starId, missionId) {
    $.ajax({
        url: '/Home/AddRating',
        type: 'POST',
        data: { missionId: missionId, rating: starId },
        success: function (result) {
            if (result.isRated) {
                //     Update the heart icon to show that the mission has been liked
                for (i = 1; i <= parseInt(result.ratingExists.rating, 10); i++) {

                    var starbtn = document.getElementById(String(i));

                    starbtn.style.color = "#F88634";
                }
                for (i = parseInt(result.ratingExists.rating, 10) + 1; i <= 5; i++) {

                    var starbtn = document.getElementById(String(i));

                    starbtn.style.color = "black";
                }                                            
                const displaystar = document.querySelector('.Stars');
                displaystar.style.setProperty('--rating', result.ratingDisplay);


            } else {
                //    Update the heart icon to show that the mission has been unliked
                for (i = 1; i <= parseInt(result.newRating.rating, 10); i++) {

                    var starbtn = document.getElementById(String(i));

                    starbtn.style.color = "#F88634";
                }
                for (i = parseInt(result.newRating.rating, 10) + 1; i <= 5; i++) {

                    var starbtn = document.getElementById(String(i));

                    starbtn.style.color = "black";
                }
                const displaystar = document.querySelector('.Stars');
                displaystar.style.setProperty('--rating', result.ratingDisplay);
               
            }
        },
        error: function () {
            // Handle error response from the server, e.g. show an error message to the user
            
            Swal.fire('You have to Apply');
        }
    });
}
function likeMission(missionId) {
    $.ajax({
        url: '/Home/AddFav',
        type: 'POST',
        data: { missionId: missionId },
        success: function (result) {
            if (result.isLiked) {
                // Update the heart icon to show that the mission has been liked
                document.getElementById("AddFav").style.color = "#F88634";
                //for landingpage
                document.getElementById(missionId).style.color = "#F88634";
            } else {
                // Update the heart icon to show that the mission has been unliked
                document.getElementById("AddFav").style.color = "black";
                //for landing page
                document.getElementById(missionId).style.color = "black";
            }
           
        },
        error: function () {
            // Handle error response from the server, e.g. show an error message to the user
            Swal.fire('Could not like mission Please Login First.');
            
        }
    });
}


function likeMissionLanding(missionId) {
    $.ajax({
        url: '/Home/AddFav',
        type: 'POST',
        data: { missionId: missionId },
        success: function (result) {
            if (result.isLiked) {
               
                //for landingpage
                document.getElementById(missionId).style.color = "#F88634";
            } else {
                
                //for landing page
                document.getElementById(missionId).style.color = "white";
            }

        },
        error: function () {
            // Handle error response from the server, e.g. show an error message to the user
            Swal.fire('Could not like mission Please Login First.');

        }
    });
}
function comment(missionId) {
    var text = document.getElementById("exampleFormControlTextarea1").value;
   
    
    $.ajax({
        url: '/Home/Addcomment',
        type: 'POST',
        data: { missionId: missionId, coment: text },
        success: function (result) {
            $("#commentDiv").empty();
            $("#commentDiv").html(result);
        },
        error: function () {
            // Handle error response from the server, e.g. show an error message to the user
            Swal.fire('Could not comment mission.');
           
        }
    });
    document.getElementById("exampleFormControlTextarea1").value = " ";
}

function sendRec(missionId) {
    $("#emailLoader").removeClass('d-none');
    $("#emaillist").addClass('d-none');
    const toMail = Array.from(document.querySelectorAll('input[name="Checkme"]:checked')).map(el => el.value);
    if (toMail.length > 0) {
        $.ajax({
            url: '/Home/SendRec',
            type: 'POST',
            data: { missionId: missionId, ToMail: toMail },
            success: function (result) {

                $("#emailLoader").addClass('d-none');
                $("#emaillist").removeClass('d-none');
                var send = document.getElementById('sent1').innerText = "Sent";

                setTimeout(() => {


                    var send = document.getElementById('sent1').innerText = "Send Email";

                }, 4000);
                Swal.fire('send');
            }
        });
    }
    else {
        $("#emailLoader").addClass('d-none');
        $("#emaillist").removeClass('d-none');
        Swal.fire('Select Co-worker');
    }
   
}

function sendRec1() {
    var missionId = document.getElementById('missionId').innerHTML;
    

    $("#emailLoader").removeClass('d-none');
    $("#emaillist").addClass('d-none');
    const toMail = Array.from(document.querySelectorAll('input[name="Checkme"]:checked')).map(el => el.value);
    if (toMail.length > 0) {
        $.ajax({
            url: '/Home/SendRec',
            type: 'POST',
            data: { missionId: missionId, ToMail: toMail },
            success: function (result) {

                $("#emailLoader").addClass('d-none');
                $("#emaillist").removeClass('d-none');
                var send = document.getElementById('sent1').innerText = "Sent";

                setTimeout(() => {


                    var send = document.getElementById('sent1').innerText = "Send Email";

                }, 4000);
                Swal.fire('send');
            }
        });
    }
    else {
        $("#emailLoader").addClass('d-none');
        $("#emaillist").removeClass('d-none');
        Swal.fire('Select Co-worker');
    }
}

function ApplyMission(MissionId) {
    $.ajax({
        url: '/Mission/ApplyMission',
        type: 'POST',
        dataType: 'json',
        data: { id: MissionId },
        success: function (d) {
            Filter();
            Swal.fire(d);
        },
        error: function () {
            Swal.fire('error.');
        }
    });
}

function recomand(Email, MI) {
    $("#emailLoader").removeClass('d-none');
    $("#emaillist").addClass('d-none');
    $.ajax({
        url: '/Home/RecomandUser',
        type: 'POST',
        data: { EmailId: Email, MissionId: MI },
        success: function (result) {
            $("#emailLoader").addClass('d-none');
            $("#emaillist").removeClass('d-none');
            var send = document.getElementById('sent').innerText = "Sent";

            setTimeout(() => {


                var send = document.getElementById('sent').innerText = "Send Email";

            }, 4000);
            Swal.fire('send');

        }


    });
}

function recomand1(Email) {
    var missionId = document.getElementById('missionId').innerHTML;
             alert(missionId);

    $("#emailLoader").removeClass('d-none');
    $("#emaillist").addClass('d-none');
    $.ajax({
        url: '/Home/RecomandUser',
        type: 'POST',
        data: { EmailId: Email, MissionId: missionId },
        success: function (result) {
            $("#emailLoader").addClass('d-none');
            $("#emaillist").removeClass('d-none');
            var send = document.getElementById('sent').innerText = "Sent";

            setTimeout(() => {


                var send = document.getElementById('sent').innerText = "Send Email";

            }, 4000);
            Swal.fire('send');

        }


    });
}