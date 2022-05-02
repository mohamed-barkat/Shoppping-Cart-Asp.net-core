
var myResponse;
var selected="";
var pageIndex;
var term;
var id = $("#productId").val();

function removeAllChildNodes(parent) {
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}
function callback(data)
{

    myResponse = data;


    //var select = document.getElementById("select");
    //var length = select.options.length;
    //for (i = length - 1; i >= 0; i--) {
    //    select.options[i] = null;
    //}
    document.getElementById('select').innerHTML = '';

    for (var i = 0; i < data.categories.length; i++) {


        if (data.categories[i].isSelected) {
            selected = "selected";
        }
        else {
            selected = "";
        }
      
        $("#select").append(
            "<option " + selected + " value='" +
                data.categories[i].id +
                "'>" +
                data.categories[i].name +
                "</option>"
            );
           }
      pageIndex = myResponse.pageIndex;
 

        if (data.hasNextPage) {
            $("#Next").addClass("");
            $("#Next").removeClass("disabled");
        }
        else {
            $("#Next").addClass("disabled");
        }
    if (data.hasPreviousPage) {
        $("#Previous").removeClass("disabled");
            $("#Previous").addClass("");
        }
        else {
            $("#Previous").addClass("disabled");
        }
    }



function PagRequest(id,pageNumber,term) {
    $.ajax("/Product/GetAllCategoriesForEdit", {
        dataType: "json", // type of response data
        timeout: 500,
        data: {
            pageNumber: pageNumber,
            term: term,
            id:id,
        },
        success: callback,
        error: function (jqXhr, textStatus, errorMessage) {
            // error callback
            $("p").append("Error: " + errorMessage);
        },
    });
}


$(function () {

    
    PagRequest(id,pageIndex,term);

  
   
});
$("#Next").on("click", function () {
   

    PagRequest(id,pageIndex+1,term);

});

$("#Previous").on("click", function () {
   
    PagRequest(id,pageIndex-1,term);
});

$("#term").on("input", function () {


   
   term = $("#term").val();
      
    PagRequest(id,1, term);

   


    });


