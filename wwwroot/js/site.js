function AddProduct(Id) {
    $.ajax({
        type: "POST",
        url: `/Product/AddItem/${Id}`,
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