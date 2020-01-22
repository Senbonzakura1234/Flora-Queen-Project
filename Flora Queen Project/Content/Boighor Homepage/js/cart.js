$(function () {

    // ************************************************
    // Shopping Cart API
    // ************************************************

    var shoppingCart = (function () {
        // =============================
        // Private methods and proprieties
        // =============================
        var cart = [];

        // Constructor
        function Item(id, name, price, imgUrl, count) {
            this.id = id;
            this.name = name;
            this.price = price;
            this.imgUrl = imgUrl;
            this.count = count;
        }

        // Save cart
        function saveCart() {
            sessionStorage.setItem("shoppingCart", JSON.stringify(cart));
        }

        // Load cart
        function loadCart() {
            cart = JSON.parse(sessionStorage.getItem("shoppingCart"));
        }

        if (sessionStorage.getItem("shoppingCart") != null) {
            loadCart();
        }


        // =============================
        // Public methods and proprieties
        // =============================
        const obj = {};

        // Add to cart
        obj.addItemToCart = function (id, name, price, imgUrl, count) {
            var item;
            for (item in cart) {
                if (cart.hasOwnProperty(item)) {
                    if (cart[item].id === id) {
                        cart[item].count++;
                        saveCart();
                        return;
                    }
                }
            }
            item = new Item(id, name, price, imgUrl, count);
            console.log(item);
            cart.push(item);
            saveCart();
        };
        // Set count from item
        obj.setCountForItem = function (id, count) {
            for (let i in cart) {
                if (cart.hasOwnProperty(i)) {
                    if (cart[i].id === id) {
                        cart[i].count = count;
                        break;
                    }
                }
            }
        };
        // Remove item from cart
        obj.removeItemFromCart = function (id) {
            for (let item in cart) {
                if (cart.hasOwnProperty(item)) {
                    if (cart[item].id === id) {
                        cart[item].count--;
                        if (cart[item].count === 0) {
                            cart.splice(item, 1);
                        }
                        break;
                    }
                }
            }
            saveCart();
        };

        // Remove all items from cart
        obj.removeItemFromCartAll = function (id) {
            for (let item in cart) {
                if (cart.hasOwnProperty(item)) {
                    if (cart[item].id === id) {
                        cart.splice(item, 1);
                        break;
                    }
                }
            }
            saveCart();
        };

        // Clear cart
        obj.clearCart = function () {
            cart = [];
            saveCart();
        };

        // Count cart 
        obj.totalCount = function () {
            var totalCount = 0;
            for (let item in cart) {
                if (cart.hasOwnProperty(item)) {
                    totalCount += cart[item].count;
                }
            }
            return totalCount;
        };

        // Total cart
        obj.totalCart = function () {
            var totalCart = 0;
            for (let item in cart) {
                if (cart.hasOwnProperty(item)) {
                    totalCart += cart[item].price * cart[item].count;
                }
            }
            return Number(totalCart.toFixed(2));
        };

        // List cart
        obj.listCart = function () {
            const cartCopy = [];
            var i;
            for (i in cart) {
                if (cart.hasOwnProperty(i)) {
                    const item = cart[i];
                    const itemCopy = {};
                    let p;
                    for (p in item) {
                        if (item.hasOwnProperty(p)) {
                            itemCopy[p] = item[p];

                        }
                    }
                    itemCopy.total = Number(item.price * item.count).toFixed(2);
                    cartCopy.push(itemCopy);
                }
            }
            return cartCopy;
        };

        // cart : Array
        // Item : Object/Class
        // addItemToCart : Function
        // removeItemFromCart : Function
        // removeItemFromCartAll : Function
        // clearCart : Function
        // countCart : Function
        // totalCart : Function
        // listCart : Function
        // saveCart : Function
        // loadCart : Function
        return obj;
    })();


    // *****************************************
    // Triggers / Events
    // ***************************************** 
    // Add item
    $(".add-to-cart").click(function (event) {
        event.preventDefault();

        const data = $(this).data("id");
        $.ajax({
            url: "/Home/GetProductInfo",
            type: "GET",
            data: {
                id: data
            },
            success: function (res) {
                if (res != null) {
                    console.log(res);
                    const id = res.id;
                    const name = res.name;
                    const price = res.price;
                    const imgUrl = res.imgUrl;
                    shoppingCart.addItemToCart(id, name, price, imgUrl, 1);
                    displayCart();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });


    // Clear items
    $(".clear-cart").click(function () {
        shoppingCart.clearCart();
        displayCart();
    });
    //$(".createOrder").click(function (event) {
    //    event.preventDefault();
    //    const cartArray = shoppingCart.listCart();
    //    console.log(cartArray);

    //    if (cartArray != null && cartArray.count !== 0 && shoppingCart.totalCount() !== 0) {
    //        shoppingCart.clearCart();
    //        displayCart();
    //        $.ajax({
    //            contentType: "application/json; charset=utf-8",
    //            dataType: "json",
    //            type: "POST",
    //            url: "/ShoppingCart/CreateOrder",
    //            data: JSON.stringify({
    //                'cartArrays': cartArray
    //            }),
    //            success: function (res) {
    //                alert(res.message);
    //                window.location.href = res.url;
    //            },
    //            error: function (xhr, ajaxOptions, thrownError) {
    //                alert(xhr.status);
    //                alert(thrownError);
    //            }
    //        });
    //    }
    //});


    function displayCart() {
        const cartArray = shoppingCart.listCart();
        var data = "";

        for (let i in cartArray) {
            if (cartArray.hasOwnProperty(i)) {
                //output += `<tr><td>${cartArray[i].name}</td><td>(${cartArray[i].price
                //    })</td><td><div class="form-group"><div class='input-group'><input type="button" class='minus-item input-group-prepend btn btn-primary' value="-" data-name=${cartArray[i].name
                //    }><input type='number' class='item-count form-control' data-name='${cartArray[i].name}' value='${cartArray[i].count
                //    }'><input  type="button" class='plus-item btn btn-primary input-group-append' value="+" data-name=${cartArray[i].name
                //    }/></div></div></td><td><button class='delete-item btn btn-danger btn-sm' data-name=${cartArray[i].name}>X</button></td> = <td>${cartArray[i].total
                //    }</td></tr>`;

                data += `
                        <div class="item01 d-flex mt--20">
                            <div class="thumb">
                                <a href="#/">
                                    <img src="${cartArray[i].imgUrl}" alt="product images">
                                </a>
                            </div>
                            <div class="content">
                                <h6><a href="#/">${cartArray[i].name}</a></h6>
                                <span class="prize">$ ${cartArray[i].price}</span>
                                <div class="product_prize d-flex justify-content-between">
                                    <span class="qun">Qty: ${cartArray[i].count}</span>
                                    <ul class="d-flex justify-content-end">
                                        <li>
                                            <a class="minus-item" href="#/" data-id="${cartArray[i].id}">
                                                <i class="zmdi zmdi-minus-circle"></i>
                                            </a>
                                        </li>
                                        <li>
                                            <a class="plus-item" href="#/" data-id="${cartArray[i].id}">
                                                <i class="zmdi zmdi-plus-circle"></i>
                                            </a>
                                        </li>
                                        <li>
                                            <a class="delete-item" href="#/" data-id="${cartArray[i].id}">
                                                <i class="zmdi zmdi-delete"></i>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                        `;
            }
        }
        const output = `<div class="miniproduct show-cart">
                            ${data}      
                        </div>`;
        if (shoppingCart.totalCount() > 0) {
            $(".checkout").show();
            $(".single__items").show();
            $(".mini_action.cart").show();
        } else {
            $(".checkout").hide();
            $(".single__items").hide();
            $(".mini_action.cart").hide();
        }
        $(".show-cart").html(output);
        $(".total-cart").html(shoppingCart.totalCart());
        $(".total-count").html(shoppingCart.totalCount());
    }

    // Delete item button

    // ReSharper disable once UnusedParameter
    $(".show-cart").on("click",
        ".delete-item",
        function (event) {
            const id = $(this).data("id");
            shoppingCart.removeItemFromCartAll(id);
            displayCart();
        });


    // -1
    // ReSharper disable once UnusedParameter
    $(".show-cart").on("click",
        ".minus-item",
        function (event) {
            const id = $(this).data("id");
            shoppingCart.removeItemFromCart(id);
            displayCart();
        });
    // +1
    // ReSharper disable once UnusedParameter
    $(".show-cart").on("click",
        ".plus-item",
        function (event) {
            const id = $(this).data("id");
            shoppingCart.addItemToCart(id);
            displayCart();
        });

    // Item count input
    // ReSharper disable once UnusedParameter
    $(".show-cart").on("change",
        ".item-count",
        function (event) {
            const id = $(this).data("id");
            const count = Number($(this).val());
            shoppingCart.setCountForItem(id, count);
            displayCart();
        });

    displayCart();
})