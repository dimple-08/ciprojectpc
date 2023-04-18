var modal = document.getElementById("exampleModaladd");
var Examplemodal = document.getElementById("Examplemodal");
var modalclose = document.getElementById("modalclose");

modalclose.onclick = function () {

    modal.style.display = "none";

};
Examplemodal.onclick = function () {
    var Firstname = document.getElementById("firstName");
    var Lastname = document.getElementById("lastName");
    var Email = document.getElementById("email");
    var Password = document.getElementById("password");
    var Avtar = document.getElementById("profile-img-input");

    var EmployeeId = document.getElementById("employeeId");
    var Department = document.getElementById("department");
    var City = document.getElementById("city");
    var Country = document.getElementById("country");
    var Profile = document.getElementById("profileText");
    var status = document.getElementById("status");



    Firstname.value = "";
    Lastname.value = '';
    Email.value = '';
    Password.value = '';
    Avtar.value = '';
    EmployeeId.value = '';
    Department.value = '';
    City.value = '';
    Country.value = '';
    Profile.value = '';
    status.value = '';
    profileimgcard.style.display = "none";
}
function AddUser() {
    var Firstname = document.getElementById("firstName").value;
    var Lastname = document.getElementById("lastName").value;
    var Email = document.getElementById("email").value;
    var Password = document.getElementById("password").value;
    var Avtar = document.getElementById("profileImg").value;
    var userId = document.getElementById("userId").value;
    var EmployeeId = document.getElementById("employeeId").value;
    var Department = document.getElementById("department").value;
    var City = document.getElementById("city").value;
    var Country = document.getElementById("country").value;
    var Profile = document.getElementById("profileText").value;
    var status = document.getElementById("status").value;

    $.ajax({
        url: '/Admin/Admin/AddUser',
        type: 'POST',
        data: { "FirstName": Firstname, "Lastname": Lastname, "Avtar": Avtar, "Email": Email, "EmployeeId": EmployeeId, "Password": Password, "Department": Department, "City": City, "Country": Country, "Profile": Profile, "Status": status, "UserId": userId },
        success: function (result) {
            if (result.success) {
                alert("Your Story Is Added");
                
            }
            else {
                Swal.fire("You alreay share the story");


            }
        }
    });
}
function userDelete(userId) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success ms-2',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/Admin/Admin/UserDelete',
                type: 'POST',
                data: { userId: userId },
                success: function (result) {
                    swalWithBootstrapButtons.fire(
                        'Deleted!',
                        'Your record has been deleted.',
                        'success'
                    ).then((res) => {
                        location.reload();
                    });
                }
            });
        } else if (
            
                    result.dismiss === Swal.DismissReason.cancel
                ) {
        swalWithBootstrapButtons.fire(
            'Cancelled'
        )
    }
})
        }
function USerEdit(UserId) {
   
    var Firstname = document.getElementById("firstName");
    var Lastname = document.getElementById("lastName");
    var Email = document.getElementById("email");
    var Password = document.getElementById("password");
    var Avtar = document.getElementById("profileImg");
    var userId = document.getElementById("userId");
    var EmployeeId = document.getElementById("employeeId");
    var Department = document.getElementById("department");
    var City = document.getElementById("city");
    var Country = document.getElementById("country");
    var Profile = document.getElementById("profileText");
    var status = document.getElementById("status");
    $.ajax({
        url: '/Admin/Admin/EditUser',
        type: 'POST',
        data: { "UserId": UserId },
        success: function (result) {
            if (result) {
                modal.classList.add("show");
                modal.style.display = "block";
                Firstname.value = result.firstName;
                Lastname.value = result.lastName;
                Email.value = result.email;
                Password.value = result.password;
                userId.value = result.userId;
                EmployeeId.value = result.employeeId;
                Department.value = result.department;
                City.value = result.cityId;
                Country.value = result.countryId;
                Profile.value = result.profileText;
                profileimgcard.style.display = "flex";
                profileimgcard.src = result.avatar;
                Avtar.value = result.avatar;




             
                // Update file input tag's placeholder or label with image name
                var base64ImageSrc = result.avatar; // Replace with the actual property that contains the base64 image source in the response
                //var matches = base64ImageSrc.match(/^data:image\/([a-zA-Z0-9]+);base64,/);
                //var imageName = '';
                //if (matches) {
                //    var imageExtension = matches[1];
                //    imageName = 'image.' + imageExtension; // Replace 'image' with desired file name
                //}
                //Avtar.setAttribute('placeholder', imageName);
                //console.log('Image Name: ' + imageName);
                if (result.status) {
                    status.value = "Active";
                }
                else {
                    status.value = "In-Active";
                }
                console.log(result);
            }
            else {
                Swal.fire("You alreay share the story");


            }
        }
    });

}