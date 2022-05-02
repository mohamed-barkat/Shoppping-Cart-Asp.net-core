var quant;


$(function () {


    var button = document.getElementById("AddtoCart");


    quant = document.getElementsByName("quantity").value




});

function AddToCart(productId) {

    var quant = $("#" + productId + "quantity").val();
    $.ajax('/Cart/AddtoCart', {

        method: 'POST',
        data: {
            quantity: quant,
            productId: productId,
        }
        ,
        success: function (data) {
            alert("done");
            cartCounter($("#userName").val());
        }



    });




}