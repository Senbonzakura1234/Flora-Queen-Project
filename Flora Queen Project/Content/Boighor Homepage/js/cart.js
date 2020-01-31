$(function () {
    $(document).on("click", ".add-to-cart", function () {
        event.preventDefault();

        const id = $(this).data("id"), dataAction = $(this).data("action"), url = "/Shop/UpdateCart";;
        var quantity;
        if (dataAction === "addOne") {
            quantity = 1;
        } else if (dataAction === "minusOne") {
            quantity = -1;
        } else if (dataAction === "delete") {
            quantity = 0;
        } else if (dataAction === "addByInput") {
            quantity = $("#qty").val();
            if (quantity === undefined) {
                quantity = 0;
            }
        } else {
            quantity = 0;
        }

        $.ajax({
            url: url,
            type: "GET",
            data: {
                proId: id,
                quantity: quantity,
                dataAction : dataAction
            },
            success: function (res) {
                if (res != null) {
                    let replaceCartNav = "", cartOldTotal = 0, cartCurrentTotal = 0;
                    const cartCount = res.shoppingCart.length;
                    if (cartCount > 0) {
                        for (let i = 0; i < cartCount; i++) {
                            console.log(res.shoppingCart[i]);
                            cartOldTotal += res.shoppingCart[i].price * res.shoppingCart[i].count;
                            cartCurrentTotal += res.shoppingCart[i].total;
                            replaceCartNav += `
                            <div class="item01 d-flex mb-3 mb-sm-1">
                                <div class="thumb">
                                    <a href="/Shop/Single/${res.shoppingCart[i].id}"><img src="${res.shoppingCart[i].imgUrl}" alt="product images"></a>
                                </div>
                                <div class="content">
                                    <h6><a href="/Shop/Single/${res.shoppingCart[i].id}">${res.shoppingCart[i].name}</a></h6>
                                    <span class="text-small">
                                        <span class="current-price-cart">$${(res.shoppingCart[i].price * res.shoppingCart[i].discount).toFixed(2)}</span>
                                        <span class="old-price-cart">$${res.shoppingCart[i].price.toFixed(2)}</span>
                                    </span>
                                    <div class="product_prize d-flex justify-content-between">
                                        <span class="qun">Qty: ${res.shoppingCart[i].count}</span>
                                        <ul class="cart-utilities justify-content-end d-flex">
                                            <li>
                                                <a class="add-to-cart" href="#/" data-id="${res.shoppingCart[i].id}" data-action="addOne">
                                                    <i class="zmdi zmdi-plus-circle"></i>
                                                </a>
                                            </li>
                                            <li>
                                                <a class="add-to-cart" href="#/" data-id="${res.shoppingCart[i].id}" data-action="minusOne">
                                                    <i class="zmdi zmdi-minus-circle"></i>
                                                </a>
                                            </li>
                                            <li>
                                                <a class="add-to-cart" href="#/" data-id="${res.shoppingCart[i].id}" data-action="delete">
                                                    <i class="zmdi zmdi-delete"></i>
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        `;
                        }
                        cartOldTotal = cartOldTotal.toFixed(2);
                        cartCurrentTotal = cartCurrentTotal.toFixed(2);

                        $(".price-1st").addClass("current-price-cart");
                        $(".price-2nd").removeClass("d-none");
                        $(".mini_action.checkout").removeClass("d-none");
                        $(".single__items").removeClass("d-none");
                        $(".mini_action.cart").removeClass("d-none");
                    } else {
                        $(".price-1st").removeClass("current-price-cart");
                        $(".price-2nd").addClass("d-none");
                        $(".mini_action.checkout").addClass("d-none");
                        $(".single__items").addClass("d-none");
                        $(".mini_action.cart").addClass("d-none");
                    }

                    $(".miniproduct").html(replaceCartNav);
                    $(".cart-count").html(cartCount);
                    $(".price-1st").html(cartCurrentTotal);
                    $(".price-2nd").html(cartOldTotal);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
})