var myResponse;

var pageIndex;
var term;
var draggables;

function removeAllChildNodes(parent) {
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}
function callback(data) {
    myResponse = data;

    //var select = document.getElementById("select");
    //var length = select.options.length;
    //for (i = length - 1; i >= 0; i--) {
    //    select.options[i] = null;
    //}
    //  document.getElementById("select").innerHTML = "";

    for (var i = 0; i < data.categories.length; i++) {
        $("#select").append(
            "<div style='cursor:move; border:1px solid #eee ;background-color:#ffc1c1' draggable='true' class='draggables'>" +
            "<option class='draggable' name='" +
            data.categories[i].id +
            "' draggable='true' selected value='" +
            data.categories[i].id +
            "' >" +
            data.categories[i].name +
            "</option>" +
            "</div>"
        );
    }
    pageIndex = myResponse.pageIndex;
    draggables = document.querySelectorAll(".draggable");

    var selectcontainer = document.querySelectorAll(".selectcontainer");

    draggables.forEach((draggable) => {
        draggable.addEventListener("dragstart", () => {
            draggable.classList.add("dragging");
        });
    });

    draggables.forEach((draggable) => {
        draggable.addEventListener("dragend", () => {
            draggable.classList.remove("dragging");
        });
    });
    selectcontainer.forEach((selectcontainer) => {
        selectcontainer.addEventListener("dragover", (e) => {
            e.preventDefault();
            var draggable = document.querySelector(".dragging");

            var afterElement = getDragAfterElement(selectcontainer, e.clientY);
            if (afterElement == null) {
                selectcontainer.appendChild(draggable);
            }
            else {
                selectcontainer.insertBefore(draggable, afterElement);
            }
        });
    });

    console.log(draggables);

    if (data.hasNextPage) {
        $("#Next").addClass("");
        $("#Next").removeClass("disabled");
    } else {
        $("#Next").addClass("disabled");
    }
    if (data.hasPreviousPage) {
        $("#Previous").removeClass("disabled");
        $("#Previous").addClass("");
    } else {
        $("#Previous").addClass("disabled");
    }
}
function getDragAfterElement(container, y) {
    const draggableElements = [...container.querySelectorAll('.draggable:not(.dragging)')]

    return draggableElements.reduce((closest, child) => {
        const box = child.getBoundingClientRect()
        const offset = y - box.top - box.height / 2
        if (offset < 0 && offset > closest.offset) {
            return { offset: offset, element: child }
        } else {
            return closest
        }
    }, { offset: Number.NEGATIVE_INFINITY }).element
}

function PagRequest(pageNumber, term) {
    $.ajax("/Product/GetCategoriesPag", {
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

$(function () {
    PagRequest(pageIndex, term);
});
$("#Next").on("click", function () {
    PagRequest(pageIndex + 1, term);
});

$("#Previous").on("click", function () {
    PagRequest(pageIndex - 1, term);
});

$("#term").on("input", function () {
    term = $("#term").val();

    PagRequest(1, term);
});
