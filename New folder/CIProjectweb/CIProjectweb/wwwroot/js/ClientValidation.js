
//function ValidateEmail(inputText) {
//    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
//    if (inputText.value.match(mailformat)) {


//        return true;
//    }
//    else {
//        alert("You have entered an invalid email address!");
//        document.form1.text1.focus();
//        return false;
//    }
//}
var form = document.querySelector('form');


function validatePassword() {
    
    var x = document.getElementById("reg-pass").value;
    var y = document.getElementById("reg-con-pass").value;
    if (x != y) {
        var submitButton = document.getElementById("btnlogin");
        submitButton.disabled = true;


    }
    else {
        var submitButton = document.getElementById("btnlogin");
        form.submit();
        submitButton.disabled = false;
    }
}
