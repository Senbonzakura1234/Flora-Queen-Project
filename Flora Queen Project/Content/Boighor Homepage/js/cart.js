$(function () {
    $(".add-to-cart").click(function (event) {
        event.preventDefault();

        const data = $(this).data("id");
        var quantity = $('#qty').val();
        if (quantity === undefined) {
            quantity = 1;
        }

        console.log("quantity: " + quantity);
        $.ajax({
            url: "/Shop/AddItem",
            type: "GET",
            data: {
                ProId: data,
                quantity: quantity
            },
            success: function (res) {
                if (res != null) {
                    console.log(res);

                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
})