$(function () {
    $(".occasion-click").each(function () {
        if ($(this).data("id") === $('input[name="occasion"]').val()) {
            $(this).addClass("active font-weight-bold");
        } else {
            $(this).removeClass("active font-weight-bold");
        }
    });
    $(".type-click").each(function () {
        if ($(this).data("id") === $('input[name="type"]').val()) {
            $(this).addClass("active font-weight-bold");
        } else {
            $(this).removeClass("active font-weight-bold");
        }
    });
    $(".color-click").each(function () {
        if ($(this).data("id") === $('input[name="color"]').val()) {
            $(this).addClass("active");
        } else {
            $(this).removeClass("active");
        }
    });
    $(".occasion-click").click(function () {
        const data = $(this).data("id");
        $('input[name="occasion"]').val(data);
        $("#shopFilter").submit();
    });
    $(".type-click").click(function () {
        const data = $(this).data("id");
        $('input[name="type"]').val(data);
        $("#shopFilter").submit();
    });
    $(".color-click").click(function () {
        const data = $(this).data("id");
        $('input[name="color"]').val(data);
        $("#shopFilter").submit();
    });
});