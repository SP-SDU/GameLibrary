document.addEventListener('DOMContentLoaded', function () {
    // Game Genres Chart
    let gameGenresCtx = document.getElementById('gameGenresChart').getContext('2d');
    let gameGenresChart = new Chart(gameGenresCtx, {
        type: 'pie',
        data: {
            labels: JSON.parse(document.getElementById('gameGenresLabels').textContent),
            datasets: [{
                data: JSON.parse(document.getElementById('gameGenresData').textContent),
                backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40']
            }]
        }
    });

    // User Activity Chart
    let userActivityCtx = document.getElementById('userActivityChart').getContext('2d');
    let userActivityChart = new Chart(userActivityCtx, {
        type: 'line',
        data: {
            labels: JSON.parse(document.getElementById('userActivityLabels').textContent),
            datasets: [{
                label: 'User Activity',
                data: JSON.parse(document.getElementById('userActivityData').textContent),
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        }
    });

    // Review Ratings Chart
    let reviewRatingsCtx = document.getElementById('reviewRatingsChart').getContext('2d');
    let reviewRatingsChart = new Chart(reviewRatingsCtx, {
        type: 'bar',
        data: {
            labels: JSON.parse(document.getElementById('reviewRatingsLabels').textContent),
            datasets: [{
                label: 'Review Ratings',
                data: JSON.parse(document.getElementById('reviewRatingsData').textContent),
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                borderColor: 'rgba(255, 99, 132, 1)',
                borderWidth: 1
            }]
        }
    });
});
