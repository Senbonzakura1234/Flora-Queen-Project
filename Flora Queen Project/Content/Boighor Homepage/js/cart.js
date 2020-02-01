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
                    let replaceCartNav = "", cartTableContent = "", replaceCartCheckout = "", cartOldTotal = 0, cartCurrentTotal = 0;
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
                            </div>`;

                            if ($(".cart-table").length) {
                                cartTableContent +=
                                    `
                                        <tr>
                                            <td class="product-thumbnail"  style="min-width: 120px">
                                                <a href="/Shop/Single/${res.shoppingCart[i].id}">
                                                    <img src="${res.shoppingCart[i].imgUrl}" alt="product img">
                                                </a>
                                            </td>
                                            <td class="product-name" style="min-width: 160px">
                                                <a href="/Shop/Single/${res.shoppingCart[i].id}">
                                                    ${res.shoppingCart[i].name}
                                                </a>
                                            </td>
                                            <td class="product-price"  style="min-width: 160px">
                                                <span class="amount text-small">
                                                    <span class="current-price-cart">$${(res.shoppingCart[i].price * res.shoppingCart[i].discount).toFixed(2)}</span>
                                                    <span class="old-price-cart">$${res.shoppingCart[i].price.toFixed(2)}</span>
                                                </span>
                                            </td>
                                            <td class="product-quantity" style="min-width: 150px">
                                                <div class="input-group d-flex" >
                                                    <button class="btn btn-dark add-to-cart input-group-prepend ml-auto mr-1" data-id="${res.shoppingCart[i].id}" data-action="minusOne">
                                                        <i class="zmdi zmdi-minus"></i>
                                                    </button>
                                                    <input class="text-center cart-quantity-input" type="number" value="${res.shoppingCart[i].count}" readonly="readonly">
                                                    <button class="btn btn-dark add-to-cart input-group-prepend mr-auto ml-1" data-id="${res.shoppingCart[i].id}" data-action="addOne">
                                                        <i class="zmdi zmdi-plus"></i>
                                                    </button>
                                                </div>
                                            </td>
                                            <td class="product-subtotal" style="min-width: 100px">
                                                $${res.shoppingCart[i].total.toFixed(2)}
                                            </td>
                                            <td class="product-remove" style="min-width: 100px">
                                                <a href="#/" class="add-to-cart" data-id="${res.shoppingCart[i].id}" data-action="delete">
                                                    <i class="zmdi zmdi-delete"></i>
                                                </a>
                                            </td>
                                        </tr>
                                    `;
                            }
                            if ($("ul.order_product").length) {
                                replaceCartCheckout += 
                                    `
                                        <li>
                                            ${res.shoppingCart[i].name} × ${res.shoppingCart[i].count} 
                                            <text class="current-price-cart ${res.shoppingCart[i].discount === 1? "d-none": ""}">
                                                ${(res.shoppingCart[i].discount - 1) * 100}%
                                            </text>
                                            <span>$${res.shoppingCart[i].total}</span>
                                        </li>
                                    `;
                            }
                        }


                        cartOldTotal = cartOldTotal.toFixed(2);
                        cartCurrentTotal = cartCurrentTotal.toFixed(2);

                        $(".price-1st-wrapper").addClass("current-price-cart");
                        $(".price-2nd-wrapper").fadeIn();
                        $(".mini_action.checkout").fadeIn();
                        $(".single__items").fadeIn();
                        $(".mini_action.cart").fadeIn();
                        $(".cart-count").fadeIn();
                        $(".cart-checkout-btn").fadeIn();

                        if ($(".cart-table").length) {
                            var replaceCartTable =
                                `
                                    <table class="table">
                                        <thead>
                                            <tr class="title-top">
                                                <th class="product-thumbnail">Image</th>
                                                <th class="product-name">Product</th>
                                                <th class="product-price">Price</th>
                                                <th class="product-quantity">Quantity</th>
                                                <th class="product-subtotal">Total</th>
                                                <th class="product-remove">Remove</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            ${cartTableContent}
                                        </tbody>
                                    </table>
                                `;
                            $(".cart-table.table-content").html(replaceCartTable);
                            $(".total-cart-table").html(cartCurrentTotal);
                            $(".grand-total-cart-table").html(cartCurrentTotal);
                            $(".cartbox__total__area").slideDown();
                        }

                        if ($("ul.order_product").length) {
                            $(".order_product").html(replaceCartCheckout);
                            $(".subtotal-cart-checkout").html(`$${cartCurrentTotal}`);
                            $(".total-cart-checkout").html(`$${cartCurrentTotal}`);
                        }
                    } else {
                        $(".price-1st-wrapper").removeClass("current-price-cart");
                        $(".price-2nd-wrapper").fadeOut();
                        $(".mini_action.checkout").fadeOut();
                        $(".single__items").fadeOut();
                        $(".mini_action.cart").fadeOut();
                        $(".cart-count").fadeOut();
                        $(".cart-checkout-btn").fadeOut();

                        if ($(".cart-table").length) {
                            $(".cart-table.table-content").html(
                                    `
                                        <table class="table">
                                            <thead>
                                                <tr class="title-top">
                                                    <th class="product-thumbnail">Image</th>
                                                    <th class="product-name">Product</th>
                                                    <th class="product-price">Price</th>
                                                    <th class="product-quantity">Quantity</th>
                                                    <th class="product-subtotal">Total</th>
                                                    <th class="product-remove">Remove</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                ${cartTableContent}
                                            </tbody>
                                        </table>
                                    `   
                            );
                            $(".total-cart-table").html(cartCurrentTotal);
                            $(".grand-total-cart-table").html(cartCurrentTotal);
                            $(".cartbox__total__area").slideUp();
                        }

                        if ($("ul.order_product").length) {
                            $(".order_product").html(replaceCartCheckout);
                            $(".subtotal-cart-checkout").html(`$${cartCurrentTotal}`);
                            $(".total-cart-checkout").html(`$${cartCurrentTotal}`);
                        }
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