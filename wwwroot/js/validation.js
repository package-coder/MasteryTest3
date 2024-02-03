function ValidateRequestForm() {
    $("#order-form").validate({
        rules: {
            name: "required",
            quantity: {
                required: true,
                number: true
            },
            uomId: "required"
        },
        messages: {
            name: "Enter the name of the product",
            quantity: {
                required: "Specify the quantity needed",
                number: "Please input a valid number"
            },
            uomId: "Select a unit of measurement" 
        }
    });
}