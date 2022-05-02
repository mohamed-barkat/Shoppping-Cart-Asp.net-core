
var myResponce;


function PagRequest(pageNumber, term) {
    $.ajax("/Product/GetMyCategoriesPag", {
        dataType: "json", // type of response data
        timeout: 500,
        data: {
            pageNumber: pageNumber,
            term: term,
        },
        success: callback,
        error: function (jqXhr, textStatus, errorMessage) {
            // error callback
            $("p").append("Error: " + errorMessage);
        },
    });
}



$(document).ready(function () {

    var currenrpage = 1;
    PagRequest(1);

    $('#selectCategory').scroll(function () {

        if ($(this).scrollTop() == ($(this).height() / 4)) {

            if ($(this).scrollTop() == 0) {
                currenrpage = currenrpage - 1;
            }
            currenrpage += 1;

            PagRequest(currenrpage);
        }
        console.log($(this).scrollTop());
    });



});








function callback(data) {

    myResponce = data;

    console.log('suc');
    document.getElementById("selectCategory").innerHTML = "";

    for (var i = 0; i < data.categories.length; i++) {
        $("#selectCategory").append(

            "<div> <input type=checkbox value=" + data.categories[0].id + " />      <span> " + data.categories[i].name + "</span>            </div>"
        );
    }



}

