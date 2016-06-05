$(document).ready(function () {
    
    $("#displayTransactions").on("click", function () {
        var month = $("#transactionsByMonth .monthDropdown").val();
        var year = $("#transactionsByMonth .yearDropdown").val();
        $('#pieChart').load('/Home/_TransactionsByMonth/' + month + '/' + year, function (response) {
            console.log('/Home/_TransactionsByMonth/' + month + '/' + year);
            var chart = new CanvasJS.Chart("pieChart", {
                backgroundColor: "#EFFDE9",
                data: [
                {
                    type: "pie",
                    dataPoints: $.parseJSON(response)
                }
                ]
            });
            chart.render();
        });
    });

    $("#displayBudgetVsTransactions").on("click", function () {
        var month = $("#budgetVsTransactions .monthDropdown").val();
        var year = $("#budgetVsTransactions .yearDropdown").val();
        $('#comparisonChart').load('/Home/_BudgetVsTransactions/' + month + '/' + year, function (response) {
            var object = $.parseJSON(response);
            console.log(response);
            var chart = new CanvasJS.Chart("comparisonChart",
            {
                theme: "theme3",
                animationEnabled: true,
                toolTip: {
                    shared: true
                },
                axisY: {
                    title: "dollars"
                },
                data: [
                {
                    type: "column",
                    name: "Budget Item",
                    legendText: "Budget Item",
                    showInLegend: true,
                    dataPoints:
                    object.budgetData
                },
                {
                    type: "column",
                    name: "Transactions",
                    legendText: "Transactions",
                    showInLegend: true,
                    dataPoints:
                    object.transactionData
                }

                ],
                legend: {
                    cursor: "pointer",
                    itemclick: function (e) {
                        if (typeof (e.dataSeries.visible) === "undefined" || e.dataSeries.visible) {
                            e.dataSeries.visible = false;
                        }
                        else {
                            e.dataSeries.visible = true;
                        }
                        chart.render();
                    }
                }
            });
                chart.render();

        });
    });

});