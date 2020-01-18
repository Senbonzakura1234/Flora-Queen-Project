

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
        $.ajax({
            url: "/Home/QuickView",
            type: "GET",
            data: {
                id: data
            },
            success: function (res) {
                $("#update-quick-view").html(res);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    });
});