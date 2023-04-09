
//list view seeting element
function showList(e) {
    var $gridCont = $('.grid-container');
    e.preventDefault();
    $gridCont.hasClass('list-view') ? $gridCont.removeClass('list-view') : $gridCont.addClass('list-view');
}
function gridList(e) {
    var $gridCont = $('.grid-container')
    e.preventDefault();
    $gridCont.removeClass('list-view');
}
$(document).on('click', '.btn-grid', gridList);
$(document).on('click', '.btn-list', showList);
if (localStorage.getItem("view") === "list") {
    list();
}
function grid() {
    localStorage.setItem("view", "grid");
    let card = document.querySelectorAll(".card");
    let cardBody = document.querySelectorAll(".card-body");
    let ratings = document.querySelector(".ratings");
    let star = document.querySelector(".star");
    let applyButton = document.querySelector(".apply-btn");
    ratings.classList.add("justify-content-between");
    ratings.classList.remove("ms-1");
    star.classList.remove("ms-2");
    applyButton.classList.remove("ms-2");
}
function list() {
    localStorage.setItem("view", "list");
    let card = document.querySelectorAll(".card");
    let cardBody = document.querySelectorAll(".card-body");
    let ratings = document.querySelector(".ratings");
    let star = document.querySelector(".star");
    let applyButton = document.querySelector(".apply-btn");
    ratings.classList.remove("justify-content-between");
    ratings.classList.add("ms-1");
    star.classList.add("ms-2");
    applyButton.classList.add("ms-2");
}

function addtofav(MI, UI) {
    $.ajax({
        url: '/LandingPage/AddToFav',
        type: 'POST',
        data: { MissionId: MI, UserId: UI },
        success: function (result) {
           
            if (result.missionAdd) {
                $("#afterApply").html('<i class="bi bi-heart-fill" id="heart"></i>Already Added');
                $(".like-btn").html('<i class="bi bi-heart-fill" style="color:red;" id="heart"></i>');
               
            }
            else if (result.missionDel) {
                $("#afterApply").html('<i class="bi bi-heart" id="heart"></i> Addto Favourite');
                $(".like-btn").html('<i class="bi bi-heart" style="color:white;" id="heart"></i>');
                
            }
        }
    });
}

function recomand(Email, MI) {
    $("#emailLoader").removeClass('d-none');
    $("#emaillist").addClass('d-none');
    $.ajax({
        url: '/LandingPage/RecomandUser',
        type: 'POST',
        data: { EmailId: Email, MissionId: MI },
        success: function (result) {
            $("#emailLoader").addClass('d-none');
            $("#emaillist").removeClass('d-none');
            var send = document.getElementById('sent').innerText = "Sent";

            setTimeout(() => {


                var send = document.getElementById('sent').innerText = "";

            }, 4000);


        }


    });
}


function Addrating(starId, missionId, Id) {
    $.ajax({
        url: '/LandingPage/Addrating',
        type: 'POST',
        data: { missionId: missionId, Id: Id, rating: starId },
        success: function (result) {
            if (parseInt(result.ratingUpdated, 10)) {
                for (i = 1; i <= parseInt(result.ratingUpdated, 10); i++) {
                    var starbtn = document.getElementById(String(i));
                   

                    starbtn.style.color = "#F88634";
                }
                for (i = parseInt(result.ratingUpdated, 10) + 1; i <= 5; i++) {
                    var starbtn = document.getElementById(String(i));
                    
                    starbtn.style.color = "black";
                }
            }
            else {
                for (i = 1; i <= parseInt(result.NewRating, 10); i++) {
                    var starbtn = document.getElementById(String(i));
                   
                    starbtn.style.backgroundColor = "#F88634";
                }
                for (i = parseInt(result.ratingUpdated, 10) + 1; i <= 5; i++) {
                    var starbtn = document.getElementById(String(i));
                   
                    starbtn.style.color = "#F88634";
                }
            }
            $('.rated').html($(result).find('.rated').html());
        },
        error: function () {
            alert("could not like mission");
        }
    });
}


function postcomment(MI, UI) {


    var cmttxt = document.getElementById("commenttext").value;
    $.ajax({

        url: '/LandingPage/PostComment',
        type: 'POST',
        dataType: "html",
        data: { MissionId: MI, UserId: UI, commenttext: cmttxt },
        success: function (result) {
            alert("Comment Successfully Added");

            $("#commentSection").html("");

            $("#commentSection").html(result);
        }
    });
    document.getElementById("commenttext").value = "";
}



function pleaseLogin() {
    alert("Please Login First");
}


function applyNow(MI,UI) {
    alert("apllied");
    $.ajax({

        url: '/LandingPage/ApplyNow',
        type: 'POST',
        dataType: "html",
        data: { MissionId: MI, UserId: UI },
        success: function (result) {
            if (result.missionAdd) {
                alert("applied");
            }
            else if (result.missionDel) {
                alert("already applied");

            }
        }
    });
}

function noAccessRating() {
        Swal.fire('Only applied user can do rating')
    /*alert("only applied user can rating");*/
}