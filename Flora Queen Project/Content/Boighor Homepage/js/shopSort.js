$(function () {
    $("#sortSelect").on("change", function () {
        const sortValue = $("#sortSelect").val();
        $("#sortBy").val(sortValue);
        console.log($("#sortBy").val());
        $("#shopFilter").submit();
    });
    $("#nav-grid-click").click(function () {
        $("#viewMode").val(0);
    });
    $("#nav-list-click").click(function () {
        $("#viewMode").val(1);
    });
});