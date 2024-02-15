
let order = null;
let orderItems = [];
let deletedOrderItems = [];
let productList = [];

const orderItemsElement = document.getElementById('request-list');
const alertElement = document.querySelector('#form .alert');
const actionButtons = document.querySelectorAll("footer button[type=submit]");
const btnBrowseProduct = document.getElementById("btn-browse-product");

const formElement = document.getElementById("form");
const modalFormElement = document.getElementById("modal-form");
const modalUploadFormElement = document.getElementById("upload-form");
const selectForm = document.getElementById("name2");
const formFields = ['quantity', 'unit', 'name', 'remarks'];


document.addEventListener('DOMContentLoaded', async () => {
    await fetchDraftOrderRequest();
    
    ///// Validation
    formElement.addEventListener("submit", (e) => {
        e.preventDefault();
        const form = {};
        let valid = true;
        
        formFields.forEach((field) => {
            const fieldElement = document.getElementById(field);
            
            if(!fieldElement.value && fieldElement.hasAttribute("required")) {
                fieldElement.classList.add('border-danger');
                valid = false;
                return;
            } else {
                fieldElement.classList.remove('border-danger');
            }
            
            Object.assign(form, { [field]: fieldElement.value });
            fieldElement.value = null;
        });
        
        if(!valid) return;
        
        addOrderItem(form);
        document.getElementById(formFields[0]).focus();
    });

    modalFormElement.addEventListener("submit", (e) => {

        e.preventDefault();

        const form = {};
        let valid = true;
        const fieldIdName = ['quantity2', 'unit2', 'name2', 'remark2'];
        console.log("clicked");

        formFields.forEach((field, index) => {
            const fieldElement = document.getElementById(fieldIdName[index]);

            if (!fieldElement.value && fieldElement.hasAttribute("required")) {
                fieldElement.classList.add('border-danger');
                valid = false;
            } else {
                fieldElement.classList.remove('border-danger');
            }

            if (fieldElement.tagName == 'SELECT') {
                var value = fieldElement.selectedIndex != 0 ? fieldElement.options[fieldElement.selectedIndex].text : '';
                Object.assign(form, { [field]: value });
            } else {
                Object.assign(form, { [field]: fieldElement.value });
            }
        });

        if (!valid) return;

        addOrderItem(form);
        modalFormElement.reset();

        document.getElementById(fields[0]).focus();

    });

    modalUploadFormElement.addEventListener("submit", (e) => {
        e.preventDefault();

        var formData = new FormData();

        const fileInput = document.getElementById("product-list");
        const excelFile = fileInput.files[0];

        formData.append("productList", excelFile);

        $.ajax({
            type: "POST",
            url: "/Order/UploadExcelFile",
            data: formData,
            dataType: false,
            processData: false,
            contentType: false,
            enctype: "multipart/form-data",
            success: (data) => {
                data.forEach(item => addOrderItem({ ...item }));
            }
        })
    });

    btnBrowseProduct.addEventListener('click', () => {
        fetchProductList();

    });

    document.getElementById('send-request').addEventListener('click', () => saveOrderRequest('FOR_APPROVAL'));
    document.getElementById('save-request').addEventListener('click', () => saveOrderRequest('DRAFT'));
    document.getElementById('discard-request').addEventListener('click', discardOrderRequest);
    
    ///// Styling
    document.querySelectorAll("input[required]")
        .forEach(element => 
            element.addEventListener('change', () => {
                if(element.value) {
                    element.classList.remove('border-danger');
                }
            }))

    selectForm.addEventListener("change", () => {

        if (selectForm.selectedIndex != 0) {
            selectForm.classList.remove('border-danger');
        }

        const unit = document.getElementById("unit2");
        unit.value = selectForm.options[selectForm.selectedIndex].getAttribute("data-unit").toUpperCase();
    })

    document.getElementById("quantity").addEventListener("keypress", function (e) { validateKeyInput(e) });
    document.getElementById("quantity2").addEventListener("keypress", function (e) { validateKeyInput(e) });
})

async function fetchDraftOrderRequest() {
    const response = await fetch('/Order/GetDraftOrderRequest', { method: "GET" });
    const data = await response.json();

    data?.orderItems?.forEach(item => addOrderItem({ ...item }))
    order = data;

    setDisableActionButtons(true);
}
async function fetchProductList() {
    var productList = {}
    if (localStorage.getItem("productList")) {

        productList = JSON.parse(localStorage.getItem("productList"));
        populateSelectList(productList);

       
    } else {
        const response = await fetch('/Product/GetAllProducts', { method: "GET" });
        const data = await response.json();

        localStorage.setItem("productList", JSON.stringify(data));
        productList = JSON.parse(localStorage.getItem("productList"));

        populateSelectList(productList);
    }
    
}

function populateSelectList(productList) {
    productList.forEach((item) => {
        var option = document.createElement("option");
        var selectElement = document.getElementById("name2");
        option.text = item.name;
        option.value = item.id;
        option.setAttribute("data-unit", item.uom.unit);
        selectElement.append(option);
    });
}

function saveOrderRequest(status) {
    const data = {
        ...order,
        status,
        orderItems: orderItems,
        deletedOrderItems
    };

    $.ajax({
        type: "POST",
        url: "/Order/Request",
        data: JSON.stringify(data),
        contentType: "application/json",
        processData: false,
        dataType: false,
        success: () => {
            Swal.fire({
                title: "Submitted Successfully!",
                text: "Request has been submitted",
                icon: "success",
                background: '#151515',
                showCancelButton: false,
                allowOutsideClick: false
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.replace('/');
                }
            });
        },
        error: () => {
            Swal.fire({
                title: "Something went wrong!",
                text: "Your request has not been submitted",
                icon: "error",
                background: '#151515',
                showCancelButton: false,
                allowOutsideClick: false
            });
        }
    });
}
function addOrderItem(value) {
    if(!value) return;
    
    if (orderItems.length === 0) {
        alertElement.classList.add('d-none');
    }
    setDisableActionButtons(false);
    
    const index = orderItems.length;
    orderItemsElement.append(createOrderItemRowElement(index, value));
    orderItems.push(value);
}
function deleteOrderItem(index, rowElement) {
    const item = orderItems[index];
    orderItems.splice(index, 1);
    orderItemsElement.removeChild(rowElement);

    if(item.id) {
        deletedOrderItems.push(item);
    }
    
    if (orderItems.length === 0) {
        alertElement.classList.remove('d-none');
    }

    setDisableActionButtons(orderItems.length === 0);
}

function discardOrderRequest() {
    Swal.fire({
        title: "Discard Order Request?",
        text: "All the items will be deleted",
        icon: "question",
        background: '#151515',
        showCancelButton: true,
        allowOutsideClick: false
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: "/Order/DeleteDraftOrderRequest",
                data: JSON.stringify(order),
                contentType: "application/json",
                processData: false,
                dataType: false,
                success: () => {
                    Swal.fire({
                        title: "Order request has been discarded!",
                        icon: "success",
                        background: '#151515',
                        showCancelButton: false,
                        allowOutsideClick: false
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload();
                        }
                    });
                },
                error: () => {
                    Swal.fire({
                        title: "Something went wrong!",
                        text: "Your request has not been submitted",
                        icon: "error",
                        background: '#151515',
                        showCancelButton: false,
                        allowOutsideClick: false
                    });
                }
            });
        }
    });
}

///// Element Creation
function createOrderItemRowElement(index, value) {
    const rowElement = document.createElement('tr');
    rowElement.id = index.toString();

    const quantityElement = rowElement.appendChild(document.createElement('td'));
    quantityElement.textContent = value.quantity;
    quantityElement.classList.add('text-center');

    const unitElement = rowElement.appendChild(document.createElement('td'));
    unitElement.textContent = value.unit;
    unitElement.classList.add('text-center');

    rowElement.appendChild(document.createElement('td')).textContent = value.name
    rowElement.appendChild(document.createElement('td')).textContent = value.remarks

    const actionElement = rowElement.appendChild(document.createElement('td'));
    actionElement.classList.add('py-0')
    actionElement.append(createOrderItemDeleteButtonElement(index, rowElement));
    
    return rowElement;
}
function createOrderItemDeleteButtonElement(index, rowElement) {
    const buttonElement = document.createElement('button');
    buttonElement.classList.add('btn', 'd-flex', 'align-items-center');
    buttonElement.type = 'button';
    buttonElement.addEventListener('click', () => deleteOrderItem(index, rowElement));

    const child = buttonElement.appendChild(document.createElement('span'));
    child.textContent = "delete";
    child.classList.add('material-symbols-outlined', 'h-100');

    return buttonElement;
}

///// Styling
function setDisableActionButtons(value = true) {
    actionButtons.forEach(button => button.disabled = value);
}

function validateKeyInput(e) {

    if ((isNaN(e.key)) && (e.key != 8) || e.target.value.length >= 5) {
        e.preventDefault();
    }
    return true;
}

