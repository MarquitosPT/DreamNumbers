
export function renderSmartHybridChart(canvasId, labels, baseScores, adjustedScores) {
    dreamNumbersCharts.renderChart(canvasId, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [
                {
                    label: 'Score Base',
                    data: baseScores,
                    backgroundColor: 'rgba(0, 120, 212, 0.6)',
                    borderColor: 'rgba(0, 120, 212, 1)',
                    borderWidth: 1
                },
                {
                    label: 'Score Ajustado',
                    data: adjustedScores,
                    backgroundColor: 'rgba(255, 152, 0, 0.6)',
                    borderColor: 'rgba(255, 152, 0, 1)',
                    borderWidth: 1
                }
            ]
        },
        options: {
            responsive: true,
            scales: {
                y: {
                    beginAtZero: false
                }
            }
        }
    });
}
