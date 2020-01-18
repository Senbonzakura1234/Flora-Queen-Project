

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
});