const { forEach } = require("../../lib/fontawesome/js/v4-shims");

var model;
var pageNumber;
$(function () {

    $.ajax("/Product/GetCategoriesPag",
        {
        dataType: "json", // type of response data
        timeout: 500,
        data: {
            pageNumber: 1,
            SearchString: "",
            },
            success: function (data) {
                model = data;
                pageNumber = model.pageIndex;
                console.log(data);
               // for (int i = 0; i < data.c)
                    
                  


             
            },
        error: function (jqXhr, textStatus, errorMessage) {
            // error callback
            $("p").append("Error: " + errorMessage);
        },
    });

});

