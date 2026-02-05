window.dreamNumbersCharts = {
    charts: {},

    renderChart: function (canvasId, chartData) {
        const ctx = document.getElementById(canvasId);

        // Se já existe um gráfico neste canvas, destrói-o
        if (this.charts[canvasId]) {
            this.charts[canvasId].destroy();
        }

        // Cria novo gráfico
        this.charts[canvasId] = new Chart(ctx, {
            type: chartData.type,
            data: chartData.data,
            options: chartData.options
        });
    }
};


window.renderNumbersChart = (labels, absenceData, frequencyData) => {

    dreamNumbersCharts.renderChart("numbersChart", {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Frequência',
                data: frequencyData,
                backgroundColor: 'rgba(54, 162, 235, 0.6)'
            },
            {
                label: 'Ausência',
                data: absenceData,
                backgroundColor: 'rgba(255, 99, 132, 0.6)'
            }]
        }
    });
};

window.renderDreamNumbersChart = (labels, absenceData, frequencyData) => {

    dreamNumbersCharts.renderChart("dreamNumbersChart", {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Frequência',
                    data: frequencyData,
                    backgroundColor: 'rgba(54, 162, 235, 0.6)'
                },
                {
                    label: 'Ausência',
                    data: absenceData,
                    backgroundColor: 'rgba(255, 99, 132, 0.6)'
                }
            ]
        }
    });
};
