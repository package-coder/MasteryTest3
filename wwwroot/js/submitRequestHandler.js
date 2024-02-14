
var tableRequestForm = document.getElementById("form");
var modalRequestForm = document.getElementById("modal-request-form");

window.addEventListener("DOMContentLoaded", () => {
    tableRequestForm.addEventListener("submit", function (e) {

        e.preventDefault();

         const form = {};
         const fields = ['quantity', 'unit', 'name', 'remarks'];
    
        fields.forEach((field) => {
            var fieldElement = document.getElementById(field);
             Object.assign(form, { [field]: fieldElement.value });
             fieldElement.value = null;
         });
    
         if (validateForm(form, fields)) {
             addRequestItem(form);
        }

        document.getElementById(fields[0]).focus();
    
     });

    modalRequestForm.addEventListener("submit", function (e) {

        e.preventDefault();

        const form = {};
        const fieldIdName = ['quantity2', 'unit2', 'name2', 'remark2'];
        const fields = ['quantity', 'unit', 'name', 'remarks'];

        fields.forEach((field, index) => {
            var fieldElement = document.getElementById(fieldIdName[index]);

            if (fieldElement.tagName == 'SELECT') {
                var value = fieldElement.selectedIndex != 0 ? fieldElement.options[fieldElement.selectedIndex].text : '';
                Object.assign(form, { [field]: value });
            } else {
                Object.assign(form, { [field]: fieldElement.value });
            }
        });

        if (validateForm(form, fieldIdName)) {
            addRequestItem(form);
            modalRequestForm.reset();
        };

        document.getElementById(fields[0]).focus();

    });
});
