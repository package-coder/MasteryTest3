
let order = null;
let orderItems = [];
let deletedOrderItems = [];

const orderItemsElement = document.getElementById('request-list');
const alertElement = document.querySelector('#alert');
const actionButtons = document.querySelectorAll("footer button[type=submit]");
const btnBrowseProduct = document.getElementById("btn-browse-product");

const formElement = document.getElementById("form");
const modalFormElement = document.getElementById("modal-form");
const modalUploadFormElement = document.getElementById("upload-form");
const selectForm = document.getElementById("name2");
const formFields = ['quantity', 'unit', 'name', 'remark'];
const btnSendRequest = document.getElementById("send-request");
const btnSaveRequest = document.getElementById("save-request");
const table = document.getElementById("table-request");
const paramId = new URLSearchParams(window.location.search).get("id");


document.addEventListener('DOMContentLoaded', async () => {

    if (paramId != null) {
        await fetchDraftOrderRequest(paramId);
    }
    
    const formSubmit = () => {
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
            fieldElement.value = null
        });

        if (!valid) return;
        
        addOrderItem(form);
        document.getElementById(formFields[0]).focus();
    }
    
    modalFormElement.addEventListener("submit", (e) => {

        e.preventDefault();

        const form = {};
        let valid = true;
        const fieldIdName = ['quantity2', 'unit2', 'name2', 'remark2'];

        formFields.forEach((field, index) => {
            const fieldElement = document.getElementById(fieldIdName[index]);

            if (!fieldElement.value && fieldElement.hasAttribute("required")) {
                fieldElement.classList.add('border-danger');
                valid = false;
            } else {
                fieldElement.classList.remove('border-danger');
            }

            if (fieldElement.tagName == 'SELECT') {
                var productId = selectForm.options[selectForm.selectedIndex].value;
                var value = fieldElement.selectedIndex != 0 ? fieldElement.options[fieldElement.selectedIndex].text : '';
                
                Object.assign(form, { [field]: value });
                Object.assign(form, { ['product']: {id: productId} });
            } else {
                Object.assign(form, { [field]: fieldElement.value });
            }

        });

        if (!valid) return;

        addOrderItem(form);
        modalFormElement.reset();

        document.getElementById(formFields[0]).focus();

    });

    modalUploadFormElement.addEventListener("submit", (e) => {
        e.preventDefault();

        var formData = new FormData();

        const fileInput = document.getElementById("product-list");
        const excelFile = fileInput.files[0];

        if (excelFile != null) {
            var extension = excelFile.name.substring(excelFile.name.lastIndexOf('.') + 1);
            if (extension == 'xlsx') {
                formData.append("productList", excelFile);
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

    btnBrowseProduct.addEventListener('click', () => {
        fetchProductList();

    });

    document.getElementById('send-request').addEventListener('click', () => saveOrderRequest('FOR_APPROVAL'));
    document.getElementById('save-request').addEventListener('click', () => saveOrderRequest('DRAFT'));
    document.getElementById('discard-request').addEventListener('click', discardOrderRequest);

    document.getElementById('product-list').addEventListener('change', () => {
        document.getElementById('product-list').classList.remove("border-danger");
    });
    
    ///// Styling
    document.querySelectorAll(".table-input input[required]")
        .forEach(element => 
            element.addEventListener('change', () => {
                    if(element.value) {
                        element.classList.remove('border-danger');
                    }
            })
        )
    
    console.log(document.querySelectorAll(".table-input input"))
    document.querySelectorAll(".table-input input")
        .forEach((element, index, array) => {
            element.addEventListener('keyup', (e) => {
                e.preventDefault();

                const currentIndex = parseInt(element.dataset.index)
                const lastIndex = array.length - 1
                
                if(e.key !== "Enter") return;
                if(currentIndex !== lastIndex && !element.value) return;

                const nextIndex = (currentIndex + 1) % array.length;
                const valid = Array.from(array.values()).every(item => {
                    const required = item.hasAttribute('required')
                    if(!required) return true;
                    
                    return item.value;
                })

                if(!valid || currentIndex !== lastIndex) {
                    const nextElement = Array.from(array.values()).find(item => item.dataset.index === nextIndex.toString());
                    nextElement?.focus();
                } else if(currentIndex === lastIndex) {
                    formSubmit()
                }
            })
        })

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
    try {
        const response = await fetch('/Order/GetDraftOrderRequest', { method: "GET" });
        const data = await response.json();

        data?.orderItems?.forEach(item => addOrderItem({ ...item }))
        order = data;

        setDisableActionButtons(table.tBodies[0].rows.length - 1 === 0);
    } catch (e) {
        console.info(e);
    }
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
        success: (data) => {
            console.log(data);
            Swal.fire({
                title: "Submitted Successfully!",
                text: "Request has been submitted",
                icon: "success",
                background: '#151515',
                showCancelButton: false,
                allowOutsideClick: false
            }).then((result) => {
                if (result.isConfirmed) {
                    window.location.href = `/Order/Request?id=${data}`
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
                            window.location.href = '/Order/Draft';
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
    rowElement.appendChild(document.createElement('td')).textContent = value.remark

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
