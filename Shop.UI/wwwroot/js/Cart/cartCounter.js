
$(document).ready(function () {


    var userName = $("#userName").val();
    cartCounter(userName);



});


function cartCounter(userName) {

    if (userName == "") {
        $.ajax("/Cart/CartCount", {

            method: "GET",
            success: function (data) {


                if (data == 0) {
                    $("#cartCounterSession").html("");
                }
                else {

                    $("#cartCounterSession").html("" + data + "");

                }




                console.log(data);


            },



        });
    }

    else {

        $.ajax("/Cart/CartCount", {

            method: "GET",
            success: function (data) {


                if (data == 0) {
                    $("#cartCounterdatabase").html("");
                }
                else {

                    $("#cartCounterdatabase").html("" + data + "");

                }




                console.log(data);


            },



        });
    }


}
//$("button.AddtoCart").on("click", function () {
//    var s = $("#userName").val();

//    setTimeout(cartCounter, 1000, s);
//});