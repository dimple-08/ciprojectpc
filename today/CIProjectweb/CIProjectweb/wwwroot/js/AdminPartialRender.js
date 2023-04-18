function UserRender() {
  
    $.ajax({
        url: '/Admin/Admin/GetUsers',
        type: 'POST',
        dataType: 'html',
        data: {  },
        success: function (d) {
            $("#content").html("");
            $("#content").html(d);
            console.log($("#content"));
        },
        error: function () {
            alert('Error');
        }
    });
}
