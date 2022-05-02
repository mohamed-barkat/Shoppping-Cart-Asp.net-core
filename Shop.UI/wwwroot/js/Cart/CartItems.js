

$(document).ready(function () {

    getcartItems();
});

function callback(data) {
    console.log(data.cartId);
    if (data.cartItems.length == 0) {

        $(".cartItems").html(" <div class='row '>" + "<div class= 'col-md-12' >" +
            " <div class='CartContainer'>" +
            "<h3 class='text-center CartTitle'>Your Cart is empty.</h3>" +
            "<p class='CartContent'>" +
            "Your Shopping Cart lives to serve. Give it purpose — fill it with groceries, clothing, household supplies, electronics, and more." +
            "Continue shopping on the Zinz.com homepage, learn about today's deals, or visit your Wish List." +
            "</p>"


            + "</div>"

            + "</div >"


            + "</div >");
    }
    else {
        console.log(data);

        var elem = "";
        $.each(data.cartItems, function (i, v) {

            console.log(data.cartItems[i].productId);
            elem = elem + "<div class='row' style='padding:20px;margin:10px'> " +
                "<div class='col-md-4'>" +
                "<div class='imgItemCart'>" +
                "<img src='/ProductsImages/" + data.cartItems[i].imageUrl + "' style='height:200px;width:200px' />" +
                "</div>" +
                "</div>" +
                " <div class='col-md-6'>" +
                "<div class='CartDetails'>" +
                "<span>" + data.cartItems[i].name + "</span>" +
                "<p>" + data.cartItems[i].description + "</p>" +
                "<p>Quantity : " + data.cartItems[i].quantity + "</p>" +
                " Price: <span>$" + data.cartItems[i].price + "</span>" +
                "</div>" +
                "</div>" +
                "<div class='col-md-2' style='position:relative'>" +
                "<button class='btn btn-danger' onclick='DeleteFromCart(" + data.cartItems[i].productId + ")' style='position: absolute " +
                /* right: 50%; */
                /* top: 50%; */
                " right: 0;" +
                "bottom: 40%;'" +
                ">Delete</button> " + " </div></div>" +



                "<hr/>";

        });


        $(".cartItems").html(elem);
        $(".cartItems").append(
            "<div >" +
            "Total - Price : <span>$" + data.totalPrice + "</span>" +
            "</div >" +

            "<div class='text-center'>" +

            "<button class='btn btn-primary'>Check Out</button>" +
            "  </div>");

    }


}
function getcartItems() {

    $.ajax("/Cart/getcartItems", {
        // dataType: "json", // type of response data
        timeout: 500,
        data: {

        },
        success: callback,
        error: function (jqXhr, textStatus, errorMessage) {
            // error callback
            $("p").append("Error: " + errorMessage);
        },
    });

};



function DeleteFromCart(productId) {




    if (confirm("are you sure")) {
        $.ajax('/Cart/DeleteFromCart', {


            method: 'POST',
            data: {

                productId: productId,

            },

            success: function (data) {
                getcartItems();
                cartCounter($("#userName").val());
            },



        });
    }



}