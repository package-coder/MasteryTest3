var buttons = document.querySelectorAll("#action-buttons button");
var table = document.querySelector("#request-list tbody");
var inputFields = document.querySelectorAll("input");
var selectField = document.getElementById("name2");
var qtyInput = document.querySelectorAll("input[name='quantity']");


window.addEventListener("DOMContentLoaded", () => {
   
    qtyInput.forEach((field) => {
        field.addEventListener("keypress", function (e) {

            if ((isNaN(e.key) && (e.key != 8)) || (e.target.value.length >= 5)) {
                e.preventDefault();
            }
        });
    });

    inputFields.forEach((field) => {
        field.addEventListener("change", () => { field.removeAttribute("style") });
    });

    selectField.addEventListener("change", () => { selectField.removeAttribute("style") });
    
});

function setDisabledButton(isRequestEmpty) {
    buttons.forEach((button) => {
        button.disabled = isRequestEmpty;
    });
}

function validateForm(form, fieldName) {
    var styleAttribute = "border: 2px solid #ff0000 !important";

    if (!form.quantity) document.getElementById(fieldName[0]).setAttribute("style", styleAttribute);
    if (!form.unit) document.getElementById(fieldName[1]).setAttribute("style", styleAttribute);
    if (!form.name) document.getElementById(fieldName[2]).setAttribute("style", styleAttribute);

    if (form.quantity && form.unit && form.name) {
        return true;
    }
}
