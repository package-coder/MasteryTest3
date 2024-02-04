function AddOrderItem(productId, name, uomId) {
    var formData = new FormData();

    formData.append("product.Id", productId);
    formData.append("uom.Id", uomId);
    formData.append("quantity", 1);
    formData.append("name", name);

    $.ajax({
        type: "POST",
        url: `/Order/AddOrderItem`,
        data: formData,
        contentType: false,
        dataType: false,
        processData: false,
        success: () => {
            Swal.fire({
                title: "Product has been added!",
                text: "Item added to cart",
                icon: "success",
                background: '#151515',
                showCancelButton: false,
                allowOutsideClick: false
            });

        },
        error: () => {
            Swal.fire({
                title: "Something went wrong!",
                text: "Your item has not been added",
                icon: "error",
                background: '#151515',
                showCancelButton: false,
                allowOutsideClick: false
            });
        }
    });
}

function RemoveCartItem(Id) {
    Swal.fire({
        title: "Proceed?",
        text: "Do you wish to remove this item from your cart?",
        icon: "warning",
        background: "#151515",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: `/Cart/RemoveCartItem/${Id}`,
                contentType: false,
                dataType: false,
                processData: false,
                success: ()=>{
                    Swal.fire({
                        title: "Success!",
                        text: "Item has been removed from your cart",
                        icon: "success",
                        background: '#151515',
                        showCancelButton: false,
                        allowOutsideClick: false
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.reload();

                        }
                    });
                }
            });
        }
    });
}

function PlaceOrder() {
    Swal.fire({
        title: "Proceed?",
        text: "Do you wish to finalize this order?",
        icon: "warning",
        background: "#151515",
        showCancelButton: true
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: "/Order/PlaceOrder",
                dataType: false,
                contentType: false,
                processData: false,
                success: () => {
                    console.log("success");
                    Swal.fire({
                        title: "Success!",
                        text: "Your order has been placed!",
                        icon: "success",
                        background: '#151515',
                        showCancelButton: false,
                        allowOutsideClick: false
                    }).then((result) => {
                        if (result.isConfirmed) {
                            window.location.href = "/Order";
                        }
                    });
                },
                error: () => {
                    Swal.fire({
                        title: "Something went wrong",
                        text: "We were unable to place your order",
                        icon: "error",
                        background: '#151515',
                        showCancelButton: false,
                    })
                }
            })
        }
    })
}