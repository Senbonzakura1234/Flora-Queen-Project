﻿

$(function () {
    setTimeout(
        function () {
            $(".loader-wrapper").fadeOut();
            $("html").css("overflow-y", "scroll");
        }, 1500);
    $(".product__nav .nav-item.nav-link").click(function () {
        $(".loader-wrapper").fadeIn();

        setTimeout(
            function () {
                $(".loader-wrapper").fadeOut();
                $("html").css("overflow-y", "scroll");
            }, 1000);
    });


    $(".quickview").click(function () {
        const data = $(this).data("id");
        $(".quickview-loader-wrapper").show();
        $.ajax({
            url: "/Home/QuickView",
            type: "GET",
            data: {
                id: data
            },
            success: function (res) {
                $("#update-quick-view").html(res);
                setTimeout(
                    function () {
                        $(".quickview-loader-wrapper").fadeOut();
                    }, 500);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });

    $(".modal").on("hide.bs.modal", function () {
        $("#update-quick-view").empty();
    });



    $(".quickview-loader").css({ "margin-top": (350 - parseFloat($(".quickview-loader").css("font-size"))) / 2 + "px"});
});