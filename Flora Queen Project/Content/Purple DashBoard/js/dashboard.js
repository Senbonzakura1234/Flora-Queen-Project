$(function () {

    // Remove pro banner on close
    document.querySelector("#bannerClose").addEventListener("click", function () {
        document.querySelector("#proBanner").classList.add("d-none");
    });

    window.Chart.defaults.global.legend.labels.usePointStyle = true;

    if ($("#serviceSaleProgress").length) {
        var serviceSaleProgressbar = new window.ProgressBar.Circle(window.serviceSaleProgress, {
            color: "url(#gradient)",
            // This has to be the same size as the maximum width to
            // prevent clipping
            strokeWidth: 8,
            trailWidth: 8,
            easing: "easeInOut",
            duration: 1400,
            text: {
                autoStyleContainer: false
            },
            from: { color: "#aaa", width: 6 },
            to: { color: "#57c7d4", width: 6 }
        });

        serviceSaleProgressbar.animate(.65);  // Number from 0.0 to 1.0
        serviceSaleProgressbar.path.style.strokeLinecap = "round";
        let linearGradient = '<defs><linearGradient id="gradient" x1="0%" y1="0%" x2="100%" y2="0%" gradientUnits="userSpaceOnUse"><stop offset="20%" stop-color="#da8cff"/><stop offset="50%" stop-color="#9a55ff"/></linearGradient></defs>';
        serviceSaleProgressbar.svg.insertAdjacentHTML("afterBegin", linearGradient);
    }
    if ($("#productSaleProgress").length) {
        var productSaleProgressbar = new window.ProgressBar.Circle(window.productSaleProgress, {
            color: "url(#productGradient)",
            // This has to be the same size as the maximum width to
            // prevent clipping
            strokeWidth: 8,
            trailWidth: 8,
            easing: "easeInOut",
            duration: 1400,
            text: {
                autoStyleContainer: false
            },
            from: { color: "#aaa", width: 6 },
            to: { color: "#57c7d4", width: 6 }
        });

        productSaleProgressbar.animate(.6);  // Number from 0.0 to 1.0
        productSaleProgressbar.path.style.strokeLinecap = "round";
        let linearGradient = '<defs><linearGradient id="productGradient" x1="0%" y1="0%" x2="100%" y2="0%" gradientUnits="userSpaceOnUse"><stop offset="40%" stop-color="#36d7e8"/><stop offset="70%" stop-color="#b194fa"/></linearGradient></defs>';
        productSaleProgressbar.svg.insertAdjacentHTML("afterBegin", linearGradient);
    }
    if ($("#points-chart").length) {
        var ctxpointschart = document.getElementById("points-chart").getContext("2d");

        var gradientStrokeVioletpointschart = ctxpointschart.createLinearGradient(0, 0, 0, 181);
        gradientStrokeVioletpointschart.addColorStop(0, "rgba(218, 140, 255, 1)");
        gradientStrokeVioletpointschart.addColorStop(1, "rgba(154, 85, 255, 1)");

        // ReSharper disable once UnusedLocals
        var myChartpointschart = new window.Chart(ctxpointschart,
            {
                type: "bar",
                data: {
                    labels: [1, 2, 3, 4, 5, 6, 7, 8],
                    datasets: [
                        {
                            label: "North Zone",
                            borderColor: gradientStrokeVioletpointschart,
                            backgroundColor: gradientStrokeVioletpointschart,
                            hoverBackgroundColor: gradientStrokeVioletpointschart,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [20, 40, 15, 35, 25, 50, 30, 20]
                        },
                        {
                            label: "South Zone",
                            borderColor: "#e9eaee",
                            backgroundColor: "#e9eaee",
                            hoverBackgroundColor: "#e9eaee",
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [40, 30, 20, 10, 50, 15, 35, 20]
                        }
                    ]
                },
                options: {
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    display: false,
                                    min: 0,
                                    stepSize: 10
                                },
                                gridLines: {
                                    drawBorder: false,
                                    display: false
                                }
                            }
                        ],
                        xAxes: [
                            {
                                gridLines: {
                                    display: false,
                                    drawBorder: false,
                                    color: "rgba(0,0,0,1)",
                                    zeroLineColor: "#eeeeee"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "#9c9fa6",
                                    autoSkip: true
                                },
                                barPercentage: 0.7
                            }
                        ]
                    }
                },
                elements: {
                    point: {
                        radius: 0
                    }
                }
            });
    }
    if ($("#events-chart").length) {
        var ctxeventschart = document.getElementById("events-chart").getContext("2d");

        var gradientStrokeBlueeventschart = ctxeventschart.createLinearGradient(0, 0, 0, 181);
        gradientStrokeBlueeventschart.addColorStop(0, "rgba(54, 215, 232, 1)");
        gradientStrokeBlueeventschart.addColorStop(1, "rgba(177, 148, 250, 1)");

        // ReSharper disable once UnusedLocals
        var myCharteventschart = new window.Chart(ctxeventschart,
            {
                type: "bar",
                data: {
                    labels: [1, 2, 3, 4, 5, 6, 7, 8],
                    datasets: [
                        {
                            label: "Domestic",
                            borderColor: gradientStrokeBlueeventschart,
                            backgroundColor: gradientStrokeBlueeventschart,
                            hoverBackgroundColor: gradientStrokeBlueeventschart,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [20, 40, 15, 35, 25, 50, 30, 20]
                        },
                        {
                            label: "International",
                            borderColor: "#e9eaee",
                            backgroundColor: "#e9eaee",
                            hoverBackgroundColor: "#e9eaee",
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [40, 30, 20, 10, 50, 15, 35, 20]
                        }
                    ]
                },
                options: {
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    display: false,
                                    min: 0,
                                    stepSize: 10
                                },
                                gridLines: {
                                    drawBorder: false,
                                    display: false
                                }
                            }
                        ],
                        xAxes: [
                            {
                                gridLines: {
                                    display: false,
                                    drawBorder: false,
                                    color: "rgba(0,0,0,1)",
                                    zeroLineColor: "#eeeeee"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "#9c9fa6",
                                    autoSkip: true
                                },
                                barPercentage: 0.7
                            }
                        ]
                    }
                },
                elements: {
                    point: {
                        radius: 0
                    }
                }
            });
    }


    if ($("#visit-sale-chart").length) {
        var monthArray = [];
        var revenue = [];
        $.ajax({
            url: "/Chart/GetOrderData",
            type: "GET",
            success: function(res) {
                if (res != null) {
                    console.log(res);
                    for (let i = 0; i < res.listOrder.length; i++) {
                        monthArray.push(`${res.listOrder[i].Month}/${res.listOrder[i].Year}`);
                        revenue.push(res.listOrder[i].Revenue / res.listOrder[i].Quantity);
                    }

                    window.Chart.defaults.global.legend.labels.usePointStyle = true;
                    const ctxvisitsalechart = document.getElementById("visit-sale-chart").getContext("2d");

                    const gradientStrokeVioletvisitsalechart = ctxvisitsalechart.createLinearGradient(0, 0, 0, 181);
                    gradientStrokeVioletvisitsalechart.addColorStop(0, "rgba(218, 140, 255, 1)");
                    gradientStrokeVioletvisitsalechart.addColorStop(1, "rgba(154, 85, 255, 1)");
                    const gradientLegendVioletvisitsalechart = "linear-gradient(to right, rgba(218, 140, 255, 1), rgba(154, 85, 255, 1))";

                    const gradientStrokeBluevisitsalechart = ctxvisitsalechart.createLinearGradient(0, 0, 0, 360);
                    gradientStrokeBluevisitsalechart.addColorStop(0, "rgba(54, 215, 232, 1)");
                    gradientStrokeBluevisitsalechart.addColorStop(1, "rgba(177, 148, 250, 1)");
                    //var gradientLegendBluevisitsalechart = "linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))";

                    const gradientStrokeRedvisitsalechart = ctxvisitsalechart.createLinearGradient(0, 0, 0, 300);
                    gradientStrokeRedvisitsalechart.addColorStop(0, "rgba(255, 191, 150, 1)");
                    gradientStrokeRedvisitsalechart.addColorStop(1, "rgba(254, 112, 150, 1)");
                    // ReSharper disable once UnusedLocals
                    const gradientLegendRedvisitsalechart = "linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))";

                    const myChartvisitsalechart = new window.Chart(ctxvisitsalechart,
                        {
                            type: "line",
                            data: {
                                labels: monthArray,
                                datasets: [
                                    {
                                        label: "Showing data of last 12 months",
                                        borderColor: gradientStrokeVioletvisitsalechart,
                                        backgroundColor: gradientStrokeVioletvisitsalechart,
                                        hoverBackgroundColor: gradientStrokeVioletvisitsalechart,
                                        legendColor: gradientLegendVioletvisitsalechart,
                                        pointRadius: 0,
                                        borderWidth: 2,
                                        fill: false,
                                        data: revenue
                                    }
                                ]
                            },
                            options: {
                                responsive: true,
                                legend: false,
                                legendCallback: function (chart) {
                                    const text = [];
                                    text.push("<ul>");
                                    for (let i = 0; i < chart.data.datasets.length; i++) {
                                        text.push(`<li><span class="legend-dots" style="background:${chart.data.datasets[i].legendColor}"></span>`);
                                        if (chart.data.datasets[i].label) {
                                            text.push(chart.data.datasets[i].label);
                                        }
                                        text.push("</li>");
                                    }
                                    text.push("</ul>");
                                    return text.join("");
                                },
                                scales: {
                                    yAxes: [
                                        {
                                            ticks: {
                                                display: true,
                                                min: 0,
                                                stepSize: 50,
                                                max: 250
                                            },
                                            gridLines: {
                                                drawBorder: false,
                                                color: "rgba(235,237,242,1)",
                                                zeroLineColor: "rgba(235,237,242,1)"
                                            }
                                        }
                                    ],
                                    xAxes: [
                                        {
                                            gridLines: {
                                                display: false,
                                                drawBorder: false,
                                                color: "rgba(0,0,0,1)",
                                                zeroLineColor: "rgba(235,237,242,1)"
                                            },
                                            ticks: {
                                                padding: 20,
                                                fontColor: "#9c9fa6",
                                                autoSkip: true
                                            },
                                            categoryPercentage: 0.5,
                                            barPercentage: 0.5
                                        }
                                    ]
                                }
                            },
                            elements: {
                                point: {
                                    radius: 0
                                }
                            }
                        });
                    $("#visit-sale-chart-legend").html(myChartvisitsalechart.generateLegend());
                }
            }
        });
    }
    if ($("#visit-sale-chart-dark").length) {
        window.Chart.defaults.global.legend.labels.usePointStyle = true;
        var ctxvisitsalechartdark = document.getElementById("visit-sale-chart-dark").getContext("2d");

        var gradientStrokeVioletvisitsalechartdark = ctxvisitsalechartdark.createLinearGradient(0, 0, 0, 181);
        gradientStrokeVioletvisitsalechartdark.addColorStop(0, "rgba(218, 140, 255, 1)");
        gradientStrokeVioletvisitsalechartdark.addColorStop(1, "rgba(154, 85, 255, 1)");
        var gradientLegendVioletvisitsalechartdark = "linear-gradient(to right, rgba(218, 140, 255, 1), rgba(154, 85, 255, 1))";

        var gradientStrokeBluevisitsalechartdark = ctxvisitsalechartdark.createLinearGradient(0, 0, 0, 360);
        gradientStrokeBluevisitsalechartdark.addColorStop(0, "rgba(54, 215, 232, 1)");
        gradientStrokeBluevisitsalechartdark.addColorStop(1, "rgba(177, 148, 250, 1)");
        var gradientLegendBluevisitsalechartdark = "linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))";

        var gradientStrokeRedvisitsalechartdark = ctxvisitsalechartdark.createLinearGradient(0, 0, 0, 300);
        gradientStrokeRedvisitsalechartdark.addColorStop(0, "rgba(255, 191, 150, 1)");
        gradientStrokeRedvisitsalechartdark.addColorStop(1, "rgba(254, 112, 150, 1)");
        var gradientLegendRedvisitsalechartdark = "linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))";

        var myChartvisitsalechartdark = new window.Chart(ctxvisitsalechartdark,
            {
                type: "bar",
                data: {
                    labels: ["JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG"],
                    datasets: [
                        {
                            label: "CHN",
                            borderColor: gradientStrokeVioletvisitsalechartdark,
                            backgroundColor: gradientStrokeVioletvisitsalechartdark,
                            hoverBackgroundColor: gradientStrokeVioletvisitsalechartdark,
                            legendColor: gradientLegendVioletvisitsalechartdark,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [20, 40, 15, 35, 25, 50, 30, 20]
                        },
                        {
                            label: "USA",
                            borderColor: gradientStrokeRedvisitsalechartdark,
                            backgroundColor: gradientStrokeRedvisitsalechartdark,
                            hoverBackgroundColor: gradientStrokeRedvisitsalechartdark,
                            legendColor: gradientLegendRedvisitsalechartdark,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [40, 30, 20, 10, 50, 15, 35, 40]
                        },
                        {
                            label: "UK",
                            borderColor: gradientStrokeBluevisitsalechartdark,
                            backgroundColor: gradientStrokeBluevisitsalechartdark,
                            hoverBackgroundColor: gradientStrokeBluevisitsalechartdark,
                            legendColor: gradientLegendBluevisitsalechartdark,
                            pointRadius: 0,
                            borderWidth: 1,
                            fill: "origin",
                            data: [70, 10, 30, 40, 25, 50, 15, 30]
                        }
                    ]
                },
                options: {
                    responsive: true,
                    legend: false,
                    legendCallback: function (chart) {
                        const text = [];
                        text.push("<ul>");
                        for (let i = 0; i < chart.data.datasets.length; i++) {
                            text.push(`<li><span class="legend-dots" style="background:${chart.data.datasets[i].legendColor}"></span>`);
                            if (chart.data.datasets[i].label) {
                                text.push(chart.data.datasets[i].label);
                            }
                            text.push("</li>");
                        }
                        text.push("</ul>");
                        return text.join("");
                    },
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    display: false,
                                    min: 0,
                                    stepSize: 20,
                                    max: 80
                                },
                                gridLines: {
                                    drawBorder: false,
                                    color: "#322f2f",
                                    zeroLineColor: "#322f2f"
                                }
                            }
                        ],
                        xAxes: [
                            {
                                gridLines: {
                                    display: false,
                                    drawBorder: false,
                                    color: "rgba(0,0,0,1)",
                                    zeroLineColor: "rgba(235,237,242,1)"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "#9c9fa6",
                                    autoSkip: true
                                },
                                categoryPercentage: 0.5,
                                barPercentage: 0.5
                            }
                        ]
                    }
                },
                elements: {
                    point: {
                        radius: 0
                    }
                }
            });
        $("#visit-sale-chart-legend-dark").html(myChartvisitsalechartdark.generateLegend());
    }
    //Color chart
    if ($("#color-chart").length) {
        var colorNameArray = [];
        var colorCount = 0;
        var colorCountArray = [];
        $.ajax({
            url: "/Chart/GetColorData",
            type: "GET",
            success: function (res) {
                if (res != null) {
                    for (let i = 0; i < res.listColor.length; i++) {
                        colorNameArray[i] = res.listColor[i].name;
                        colorCount += res.listColor[i].count;
                    }
                    for (let i = 0; i < res.listColor.length; i++) {
                        colorCountArray[i] = ((res.listColor[i].count / colorCount) * 100).toFixed(2);
                    }
                }

                const ctxColor = document.getElementById("color-chart").getContext("2d");
                const colorGradientStrokeBlue = ctxColor.createLinearGradient(0, 0, 0, 181);
                colorGradientStrokeBlue.addColorStop(0, "rgba(54, 215, 232, 1)");
                colorGradientStrokeBlue.addColorStop(1, "rgba(177, 148, 250, 1)");
                const colorGradientLegendBlue = "linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))";

                const colorGradientStrokeRed = ctxColor.createLinearGradient(0, 0, 0, 50);
                colorGradientStrokeRed.addColorStop(0, "rgba(255, 191, 150, 1)");
                colorGradientStrokeRed.addColorStop(1, "rgba(254, 112, 150, 1)");
                const colorGradientLegendRed = "linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))";

                const colorGradientStrokeGreen = ctxColor.createLinearGradient(0, 0, 0, 300);
                colorGradientStrokeGreen.addColorStop(0, "rgba(6, 185, 157, 1)");
                colorGradientStrokeGreen.addColorStop(1, "rgba(132, 217, 210, 1)");
                const colorGradientLegendGreen = "linear-gradient(to right, rgba(6, 185, 157, 1), rgba(132, 217, 210, 1))";

                var colorsChartData = {
                    datasets: [{
                        data: colorCountArray,
                        backgroundColor: [
                            colorGradientStrokeBlue,
                            colorGradientStrokeGreen,
                            colorGradientStrokeRed
                        ],
                        hoverBackgroundColor: [
                            colorGradientStrokeBlue,
                            colorGradientStrokeGreen,
                            colorGradientStrokeRed
                        ],
                        borderColor: [
                            colorGradientStrokeBlue,
                            colorGradientStrokeGreen,
                            colorGradientStrokeRed
                        ],
                        legendColor: [
                            colorGradientLegendBlue,
                            colorGradientLegendGreen,
                            colorGradientLegendRed
                        ]
                    }],

                    // These labels appear in the legend and in the tooltips when hovering different arcs
                    labels: colorNameArray
                };
                const colorsChartOptions = {
                    responsive: true,
                    animation: {
                        animateScale: true,
                        animateRotate: true
                    },
                    legend: false,
                    // ReSharper disable once UnusedParameter
                    legendCallback: function (chart) {
                        const text = [];
                        text.push("<ul>");
                        for (let i = 0; i < colorsChartData.datasets[0].data.length; i++) {
                            text.push(`<li><span class="legend-dots" style="background:${colorsChartData.datasets[0].legendColor[i]}"></span>`);
                            if (colorsChartData.labels[i]) {
                                text.push(colorsChartData.labels[i]);
                            }
                            text.push(`<span class="float-right">${colorsChartData.datasets[0].data[i]}%</span>`);
                            text.push("</li>");
                        }
                        text.push("</ul>");
                        return text.join("");
                    }
                };
                const colorsChartCanvas = $("#color-chart").get(0).getContext("2d");
                const colorsChart = new window.Chart(colorsChartCanvas, {
                    type: "doughnut",
                    data: colorsChartData,
                    options: colorsChartOptions
                });
                $("#color-chart-legend").html(colorsChart.generateLegend());
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    //Occasion chart
    if ($("#occasion-chart").length) {
        var occasionNameArray = [];
        var occasionCount = 0;
        var occasionCountArray = [];
        $.ajax({
            url: "/Chart/GetOccasionData",
            type: "GET",
            success: function (res) {
                if (res != null) {
                    for (let i = 0; i < res.listOccasion.length; i++) {
                        occasionNameArray[i] = res.listOccasion[i].name;
                        occasionCount += res.listOccasion[i].count;
                    }
                    for (let i = 0; i < res.listOccasion.length; i++) {
                        occasionCountArray[i] = ((res.listOccasion[i].count / occasionCount) * 100).toFixed(2);
                    }
                }

                const ctxOccasion = document.getElementById("occasion-chart").getContext("2d");
                const occasionGradientStrokeBlue = ctxOccasion.createLinearGradient(0, 0, 0, 181);
                occasionGradientStrokeBlue.addColorStop(0, "rgba(54, 215, 232, 1)");
                occasionGradientStrokeBlue.addColorStop(1, "rgba(177, 148, 250, 1)");
                const occasionGradientLegendBlue = "linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))";

                const occasionGradientStrokeRed = ctxOccasion.createLinearGradient(0, 0, 0, 50);
                occasionGradientStrokeRed.addColorStop(0, "rgba(255, 191, 150, 1)");
                occasionGradientStrokeRed.addColorStop(1, "rgba(254, 112, 150, 1)");
                const occasiongradientLegendRed = "linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))";

                const occasionGradientStrokeGreen = ctxOccasion.createLinearGradient(0, 0, 0, 300);
                occasionGradientStrokeGreen.addColorStop(0, "rgba(6, 185, 157, 1)");
                occasionGradientStrokeGreen.addColorStop(1, "rgba(132, 217, 210, 1)");
                const occasionGradientLegendGreen = "linear-gradient(to right, rgba(6, 185, 157, 1), rgba(132, 217, 210, 1))";

                const occasionGradientStrokeYellow = ctxOccasion.createLinearGradient(0, 0, 0, 132);
                occasionGradientStrokeYellow.addColorStop(0, "#f6e384");
                occasionGradientStrokeYellow.addColorStop(1, "#ffd500");
                const occasiongradientLegendYellow = "linear-gradient(to right, #f6e384, #ffd500)";

                var occasionsChartData = {
                    datasets: [{
                        data: occasionCountArray,
                        backgroundColor: [
                            occasionGradientStrokeBlue,
                            occasionGradientStrokeGreen,
                            occasionGradientStrokeRed,
                            occasionGradientStrokeYellow
                        ],
                        hoverBackgroundColor: [
                            occasionGradientStrokeBlue,
                            occasionGradientStrokeGreen,
                            occasionGradientStrokeRed,
                            occasionGradientStrokeYellow
                        ],
                        borderColor: [
                            occasionGradientStrokeBlue,
                            occasionGradientStrokeGreen,
                            occasionGradientStrokeRed,
                            occasionGradientStrokeYellow
                        ],
                        legendColor: [
                            occasionGradientLegendBlue,
                            occasionGradientLegendGreen,
                            occasiongradientLegendRed,
                            occasiongradientLegendYellow
                        ]
                    }],

                    // These labels appear in the legend and in the tooltips when hovering different arcs
                    labels: occasionNameArray
                };
                const occasionsChartOptions = {
                    responsive: true,
                    animation: {
                        animateScale: true,
                        animateRotate: true
                    },
                    legend: false,
                    // ReSharper disable once UnusedParameter
                    legendCallback: function (chart) {
                        const text = [];
                        text.push("<ul>");
                        for (let i = 0; i < occasionsChartData.datasets[0].data.length; i++) {
                            text.push(`<li><span class="legend-dots" style="background:${occasionsChartData.datasets[0].legendColor[i]}"></span>`);
                            if (occasionsChartData.labels[i]) {
                                text.push(occasionsChartData.labels[i]);
                            }
                            text.push(`<span class="float-right">${occasionsChartData.datasets[0].data[i]}%</span>`);
                            text.push("</li>");
                        }
                        text.push("</ul>");
                        return text.join("");
                    }
                };
                const occasionsChartCanvas = $("#occasion-chart").get(0).getContext("2d");
                const occasionsChart = new window.Chart(occasionsChartCanvas, {
                    type: "doughnut",
                    data: occasionsChartData,
                    options: occasionsChartOptions
                });
                $("#occasion-chart-legend").html(occasionsChart.generateLegend());
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    //Type chart
    if ($("#type-chart").length) {
        var typeNameArray = [];
        var typeCount = 0;
        var typeCountArray = [];
        $.ajax({
            url: "/Chart/GetTypeData",
            type: "GET",
            success: function (res) {
                if (res != null) {
                    for (let i = 0; i < res.listType.length; i++) {
                        typeNameArray[i] = res.listType[i].name;
                        typeCount += res.listType[i].count;
                    }
                    for (let i = 0; i < res.listType.length; i++) {
                        typeCountArray[i] = ((res.listType[i].count / typeCount) * 100).toFixed(2);
                    }
                }

                const ctxType = document.getElementById("type-chart").getContext("2d");
                const typeGradientStrokeBlue = ctxType.createLinearGradient(0, 0, 0, 181);
                typeGradientStrokeBlue.addColorStop(0, "rgba(54, 215, 232, 1)");
                typeGradientStrokeBlue.addColorStop(1, "rgba(177, 148, 250, 1)");
                const typeGradientLegendBlue = "linear-gradient(to right, rgba(54, 215, 232, 1), rgba(177, 148, 250, 1))";

                const typeGradientStrokeRed = ctxType.createLinearGradient(0, 0, 0, 50);
                typeGradientStrokeRed.addColorStop(0, "rgba(255, 191, 150, 1)");
                typeGradientStrokeRed.addColorStop(1, "rgba(254, 112, 150, 1)");
                const typeGradientLegendRed = "linear-gradient(to right, rgba(255, 191, 150, 1), rgba(254, 112, 150, 1))";

                const typeGradientStrokeGreen = ctxType.createLinearGradient(0, 0, 0, 300);
                typeGradientStrokeGreen.addColorStop(0, "rgba(6, 185, 157, 1)");
                typeGradientStrokeGreen.addColorStop(1, "rgba(132, 217, 210, 1)");
                const typeGradientLegendGreen = "linear-gradient(to right, rgba(6, 185, 157, 1), rgba(132, 217, 210, 1))";

                var typesChartData = {
                    datasets: [{
                        data: typeCountArray,
                        backgroundColor: [
                            typeGradientStrokeBlue,
                            typeGradientStrokeGreen,
                            typeGradientStrokeRed
                        ],
                        hoverBackgroundColor: [
                            typeGradientStrokeBlue,
                            typeGradientStrokeGreen,
                            typeGradientStrokeRed
                        ],
                        borderColor: [
                            typeGradientStrokeBlue,
                            typeGradientStrokeGreen,
                            typeGradientStrokeRed
                        ],
                        legendColor: [
                            typeGradientLegendBlue,
                            typeGradientLegendGreen,
                            typeGradientLegendRed
                        ]
                    }],

                    // These labels appear in the legend and in the tooltips when hovering different arcs
                    labels: typeNameArray
                };
                const typesChartOptions = {
                    responsive: true,
                    animation: {
                        animateScale: true,
                        animateRotate: true
                    },
                    legend: false,
                    // ReSharper disable once UnusedParameter
                    legendCallback: function (chart) {
                        const text = [];
                        text.push("<ul>");
                        for (let i = 0; i < typesChartData.datasets[0].data.length; i++) {
                            text.push(`<li><span class="legend-dots" style="background:${typesChartData.datasets[0].legendColor[i]}"></span>`);
                            if (typesChartData.labels[i]) {
                                text.push(typesChartData.labels[i]);
                            }
                            text.push(`<span class="float-right">${typesChartData.datasets[0].data[i]}%</span>`);
                            text.push("</li>");
                        }
                        text.push("</ul>");
                        return text.join("");
                    }
                };
                const typesChartCanvas = $("#type-chart").get(0).getContext("2d");
                const typesChart = new window.Chart(typesChartCanvas, {
                    type: "doughnut",
                    data: typesChartData,
                    options: typesChartOptions
                });
                $("#type-chart-legend").html(typesChart.generateLegend());
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.status);
                alert(thrownError);
            }
        });
    }
    if ($("#inline-datepicker").length) {
        $("#inline-datepicker").datepicker({
            enableOnReadonly: true,
            todayHighlight: true
        });
    }
});
