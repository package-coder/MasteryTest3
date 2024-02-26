﻿
let order = null;
let orderItems = [];
let deletedOrderItems = [];
let productList = [];

const orderItemsElement = document.getElementById('request-list');
const alertElement = document.querySelector('#alert');
const actionButtons = document.querySelectorAll("footer button[type=submit]");
const formElement = document.getElementById("form");
const modalUploadFormElement = document.getElementById("upload-form");
const selectFormElement = document.getElementById("product");
const formFields = ['quantity', 'unit', 'name', 'remark'];
const btnSendRequest = document.getElementById("send-request");
const btnSaveRequest = document.getElementById("save-request");
const table = document.getElementById("table-request");
const modalFormElement = document.getElementById("modal-form");
const paramId = new URLSearchParams(window.location.search).get("id");

document.addEventListener('DOMContentLoaded', async () => {

    if (paramId != null) {
        await fetchDraftOrderRequest(paramId);
    }

    document.getElementById("item-submit-button").addEventListener("click", () => {
        const form = {};
        let valid = true;

        formFields.forEach((field) => {
            const fieldElement = document.getElementById(field);
            if (!fieldElement) return;

            if (fieldElement.hasAttribute("required") && !fieldElement.value) {
                fieldElement.classList.add('border-danger');
                valid = false;
                return;
            } else {
                fieldElement.classList.remove('border-danger');
            }

            Object.assign(form, { [field]: fieldElement.value });
            fieldElement.value = null;
        });

        if (!valid) {
            return
        }
        
        addOrderItem(form);
        document.getElementById(formFields[formFields.indexOf('quantity')])?.focus();
    })

    modalFormElement.addEventListener("submit", (e) => {
        e.preventDefault();

        const modalFormInput = ['quantity2', 'unit2', 'product', 'remark2'];
        const form = {};
        let valid = true;

        modalFormInput.forEach((field) => {
            const fieldElement = document.getElementById(field);
            if (!fieldElement.value && field != 'remark2') {
                fieldElement.classList.add('border-danger');
                valid = false;
            } else {
                fieldElement.classList.remove('border-danger');
            }
        });

        formFields.forEach((field, index) => {
            const fieldElement = document.getElementById(modalFormInput[index]);

            if (fieldElement.tagName == 'SELECT') {
                var productId = fieldElement.value;
                var productName = fieldElement.options[fieldElement.selectedIndex].text;

                Object.assign(form, { ['product']: { id: productId } });
                Object.assign(form, { [field]: productName });
            } else {
                Object.assign(form, { [field]: fieldElement.value });
            }
        });

        if (!valid) return

        var browseProductModal = bootstrap.Modal.getInstance(document.getElementById('browseProducts'));
        browseProductModal.hide();

        modalFormElement.reset();
        addOrderItem(form);
    });

    modalUploadFormElement.addEventListener("submit", (e) => {
        e.preventDefault();

        var formData = new FormData();

        const fileInput = document.getElementById("product-list");
        const excelFile = fileInput.files[0];

        if (excelFile != null) {
            var extension = excelFile.name.substring(excelFile.name.lastIndexOf('.') + 1);
            if (extension == 'xlsx') {
                formData.append("file", excelFile);

                document.getElementById("btn-upload-active").style.display = "none";
                document.getElementById("btn-upload-progress").style.display = "block";

                $.ajax({
                    type: "POST",
                    url: "/Order/UploadExcelFile",
                    data: formData,
                    dataType: false,
                    processData: false,
                    contentType: false,
                    enctype: "multipart/form-data",
                    success: (data) => {
                        document.getElementById("btn-upload-progress").style.display = "none";
                        document.getElementById("btn-upload-active").style.display = "block";

                        Swal.fire({
                            title: "Success!",
                            text: "Your product list has been uploaded",
                            icon: "success",
                            background: '#151515',
                            showCancelButton: false,
                            allowOutsideClick: false
                        }).then((result) => {
                            if (result.isConfirmed) {
                                bootstrap.Modal.getInstance(document.getElementById("uploadProductList")).hide();
                                data.forEach(item => addOrderItem({ ...item }));
                            }
                        });
                    },
                    error: () => {
                        document.getElementById("btn-upload-progress").style.display = "none";
                        document.getElementById("btn-upload-active").style.display = "block";

                        Swal.fire({
                            title: "Something went wrong!",
                            text: "Your uploaded file could not be processed",
                            icon: "error",
                            background: '#151515',
                            showCancelButton: false,
                        });
                    }

                });

                return;
            }
        }

        fileInput.classList.add("border-danger");

    });

    document.getElementById('send-request').addEventListener('click', () => saveOrderRequest('FOR_APPROVAL'));
    document.getElementById('save-request').addEventListener('click', () => saveOrderRequest('DRAFT'));
    document.getElementById('discard-request').addEventListener('click', discardOrderRequest);

    ///// Styling

    document.querySelectorAll(".table-input input[required], #modal-form input, select")
        .forEach(element =>
            element.addEventListener('change', () => {
                if (element.value) {
                    element.classList.remove('border-danger');
                }
            })
        );

    document.querySelectorAll(".table-input input")
        .forEach((element, index, array) => {
            element.addEventListener('keyup', (e) => {
                e.preventDefault();

                const currentIndex = parseInt(element.dataset.index)
                const lastIndex = array.length - 1

                if (e.key !== "Enter") return;
                if (currentIndex !== lastIndex && !element.value) return;

                const nextIndex = (currentIndex + 1) % array.length;
                const valid = Array.from(array.values()).every(item => {
                    const required = item.hasAttribute('required')
                    if (!required) return true;

                    return item.value;
                })

                if (!valid || currentIndex !== lastIndex) {
                    const nextElement = Array.from(array.values()).find(item => item.dataset.index === nextIndex.toString());
                    nextElement?.focus();
                } else if (currentIndex === lastIndex) {
                    document.getElementById("item-submit-button").click();
                }
            })
        })
    document.getElementById("quantity").addEventListener("keypress", function (e) { validateKeyInput(e) });
    document.getElementById("quantity2").addEventListener("keypress", function (e) { validateKeyInput(e) });
});

document.addEventListener("keypress", (e) => {
    if (e.key == "/") {
        if (!document.getElementById('browseProducts').classList.contains("show")) {
            var browseProductModal = new bootstrap.Modal(document.getElementById('browseProducts'));
            browseProductModal.show();
        }

    }
})
selectFormElement.addEventListener("click", fetchProductList);

selectFormElement.addEventListener("change", () => {
    var inputUnit = document.getElementById("unit2");
    inputUnit.value = selectFormElement.options[selectFormElement.selectedIndex].getAttribute("data-unit").toUpperCase();
});

async function fetchDraftOrderRequest(id) {
    try {
        const response = await fetch(`/Order/GetOrderById/${id}`, { method: "GET" });
        const data = await response.json();

        data?.orderItems?.forEach(item => addOrderItem({ ...item }))
        order = data;

        setDisableActionButtons(table.tBodies[0].rows.length - 1 === 0);
    } catch (e) {
        console.info(e);
    }
}
async function fetchProductList() {

    if (productList.length === 0) {
        const response = await fetch('/Product/GetAllProducts', { method: "GET" });
        productList = await response.json();
        populateSelectList(productList);
    }
}

function populateSelectList(productList) {
    productList.forEach((item) => {
        var option = document.createElement("option");
        option.text = item.name;
        option.value = item.id;
        option.setAttribute("data-unit", item.uom.unit.trim());
        selectFormElement.append(option);
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
        success: (data) => {
            Swal.fire({
                title: "Submitted Successfully!",
                text: "Request has been submitted",
                icon: "success",
                background: '#151515',
                showCancelButton: false,
                allowOutsideClick: false
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = `/`
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
    if (!value) return;

    if (orderItems.length === 0) {
        alertElement.classList.add('d-none');
    }

    const index = orderItems.length;
    orderItemsElement.append(createOrderItemRowElement(index, value));
    orderItems.push(value);

    setDisableActionButtons(table.tBodies[0].rows.length - 1 === 0);
}
function deleteOrderItem(index, rowElement) {

    const item = orderItems[index];
    orderItems.splice(index, 1);
    orderItemsElement.removeChild(rowElement);

    deletedOrderItems.push(item);

    if (orderItems.length === 0) {
        alertElement.classList.remove('d-none');
    }

    setDisableActionButtons(table.tBodies[0].rows.length - 1 === 0);
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
                url: "/Order/DeleteOrderRequest",
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
                            window.location.href = '/';
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

    rowElement.appendChild(document.createElement('td')).textContent = value.name;
    rowElement.appendChild(document.createElement('td')).textContent = value.remark;

    const actionElement = rowElement.appendChild(document.createElement('td'));
    actionElement.classList.add('py-0');
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
function setDisableActionButtons(value) {

    btnSaveRequest.disabled = value;
    btnSendRequest.disabled = value;
}

function validateKeyInput(e) {

    if ((isNaN(e.key)) && (e.key != '.') || e.target.value.length >= 5) {
        e.preventDefault();
    }
    return true;
}
