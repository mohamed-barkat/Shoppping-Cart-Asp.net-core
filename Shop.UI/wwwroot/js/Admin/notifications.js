


$(document).ready(function () {



    GetUserNotifications();

    $("#Notifiy").on("click", function () {

        $.ajax("/Notification/UpdatedUserNotifcaions", {


            timeout: 500,
            method: "POST",

            success: function (data) {

                GetUserNotifications();
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }

        });





    });


});
function getuserncallback(data) {


    var cou = $("#counterNoti");
    console.log(data);

    let count = 0;

    $.each(data, function () {

        if (this.isReaded == true) {
            return false;


        }
        else {

            count++;
        }


    });
    if (count == "0") {
        cou.html("")
    }
    else {
        cou.html("" + count + "")
    }






}
function GetUserNotifications() {

    $.ajax("/Notification/GetUserNotifications", {
        timeout: 500,
        dataType: "json",
        method: "GET",

        success: getuserncallback,
        error: function (jqXhr, textStatus, errorMessage) {
            // error callback
            $("p").append("Error: " + errorMessage);
        },



    });


}
var connection = new signalR.HubConnectionBuilder().withUrl("/noti").build();




connection.start().then(function () {


    connection.on("ReceiveNotification", (message) => {

        GetUserNotifications();
    });
}).catch(function (err) {
    return console.error(err);
});

