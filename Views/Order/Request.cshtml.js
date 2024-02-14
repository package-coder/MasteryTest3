
let order = null;
let orderItems = []
let deletedOrderItems = []

const orderItemsElement = document.getElementById('request-list');
const alertElement = document.querySelector('#form .alert');
const actionButtons = document.querySelectorAll("footer button[type=submit]");

const formElement = document.getElementById("form")
const fields = ['quantity', 'unit', 'name', 'remarks']


document.addEventListener('DOMContentLoaded', async () => {
    await fetchOrderItems();

    formElement.addEventListener("submit", (e) => {
        e.preventDefault();
        const form = {};

        fields.forEach((field) => {
            const fieldElement = document.getElementById(field);
            Object.assign(form, { [field]: fieldElement.value });
            fieldElement.value = null;
        });

        addOrderItem(form);
        document.getElementById(fields[0]).focus();
    });
})

async function fetchOrderItems() {
    const response = await fetch('/Product/DraftOrder', { method: "GET" });
    const data = await response.json();

    data?.orderItems?.forEach(item => addOrderItem({ ...item, unit: item.uom.unit }))
    order = data;

    setDisableActionButtons(true);
}
function saveOrderRequest(status = "DRAFT") {
    const data = { status, Id: order?.Id, orderItems: orderItems }

    $.ajax({
        type: "POST",
        url: "/Product/Request/",
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
    if (orderItems.length === 0) {
        alertElement.classList.add('d-none');
    }
    setDisableActionButtons(false);
    
    const index = orderItems.length;
    orderItemsElement.append(createOrderItemRowElement(index, value))
    orderItems.push(value);
}
function deleteOrderItem(index, rowElement) {
    const item = orderItems[index];
    orderItems.splice(index, 1);
    orderItemsElement.removeChild(rowElement)

    if(item.id) {
        deletedOrderItems.push(item);
    }
    
    if (orderItems.length === 0) {
        alertElement.classList.remove('d-none');
    }

    setDisableActionButtons(orderItems.length === 0);
    
}
function createOrderItemRowElement(index, value) {
    const rowElement = document.createElement('tr');
    rowElement.id = index.toString();

    const quantityElement = rowElement.appendChild(document.createElement('td'))
    quantityElement.textContent = value.quantity;
    quantityElement.classList.add('text-center');

    const unitElement = rowElement.appendChild(document.createElement('td'))
    unitElement.textContent = value.unit;
    unitElement.classList.add('text-center');

    rowElement.appendChild(document.createElement('td')).textContent = value.name
    rowElement.appendChild(document.createElement('td')).textContent = value.remarks

    const actionElement = rowElement.appendChild(document.createElement('td'))
    actionElement.classList.add('py-0')
    actionElement.append(createOrderItemDeleteButtonElement(index, rowElement))
    
    return rowElement;
}
function createOrderItemDeleteButtonElement(index, rowElement) {
    const buttonElement = document.createElement('button');
    buttonElement.classList.add('btn', 'd-flex', 'align-items-center');
    buttonElement.type = 'button';
    buttonElement.addEventListener('click', () => deleteOrderItem(index, rowElement))

    const child = buttonElement.appendChild(document.createElement('span'))
    child.textContent = "delete";
    child.classList.add('material-symbols-outlined', 'h-100');

    return buttonElement;
}
function setDisableActionButtons(value = true) {
    actionButtons.forEach(button => button.disabled = value)
}