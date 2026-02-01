window.renderDashboardCharts = (labels, absenceData, frequencyData) => {

    // Absence Chart
    new Chart(document.getElementById("absenceChart"), {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Absence',
                data: absenceData,
                backgroundColor: 'rgba(255, 99, 132, 0.6)'
            }]
        }
    });

    // Frequency Chart
    new Chart(document.getElementById("frequencyChart"), {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Frequency',
                data: frequencyData,
                backgroundColor: 'rgba(54, 162, 235, 0.6)'
            }]
        }
    });
};
