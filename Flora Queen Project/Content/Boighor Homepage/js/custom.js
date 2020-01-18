

$(function () {
    setTimeout(
        function () {
            $(".loader-wrapper").fadeOut();
            $("html").css("overflow-y", "scroll");
        }, 1500);
});