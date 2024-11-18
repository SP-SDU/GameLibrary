document.addEventListener('DOMContentLoaded', function() {
    // Filter buttons
    const filterButtons = document.querySelectorAll('[data-filter]');
    filterButtons.forEach(button => {
        button.addEventListener('click', function() {
            // Remove active class from all buttons
            filterButtons.forEach(btn => btn.classList.remove('active'));
            // Add active class to clicked button
            this.classList.add('active');
            
            const filter = this.dataset.filter;
            filterGames(filter);
        });
    });

    // Upcoming games filter
    const upcomingFilter = document.getElementById('upcomingFilter');
    upcomingFilter.addEventListener('change', function() {
        const activeFilter = document.querySelector('[data-filter].active').dataset.filter;
        filterGames(activeFilter);
    });

    // Status update dropdown items
    document.querySelectorAll('.dropdown-item[data-status]').forEach(item => {
        item.addEventListener('click', async function(e) {
            e.preventDefault();
            const gameId = this.dataset.gameId;
            const status = this.dataset.status;
            
            try {
                const response = await fetch('/Games/Library?handler=UpdateStatus', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                    },
                    body: `gameId=${gameId}&status=${status}`
                });

                if (response.ok) {
                    window.location.reload();
                } else {
                    console.error('Failed to update status');
                }
            } catch (error) {
                console.error('Error:', error);
            }
        });
    });

    // Remove game buttons
    document.querySelectorAll('button[data-game-id]').forEach(button => {
        button.addEventListener('click', async function() {
            if (confirm('Are you sure you want to remove this game from your library?')) {
                const gameId = this.dataset.gameId;
                
                try {
                    const response = await fetch('/Games/Library?handler=Remove', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/x-www-form-urlencoded',
                            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                        },
                        body: `gameId=${gameId}`
                    });

                    if (response.ok) {
                        const libraryItem = this.closest('.library-item');
                        libraryItem.remove();
                    } else {
                        console.error('Failed to remove game');
                    }
                } catch (error) {
                    console.error('Error:', error);
                }
            }
        });
    });
});

function filterGames(filter) {
    const showUpcoming = document.getElementById('upcomingFilter').checked;
    const items = document.querySelectorAll('.library-item');
    
    items.forEach(item => {
        const status = item.dataset.status;
        const isUpcoming = item.dataset.upcoming === 'true';
        
        const matchesFilter = filter === 'all' || status === filter;
        const matchesUpcoming = !showUpcoming || isUpcoming;
        
        item.style.display = matchesFilter && matchesUpcoming ? 'block' : 'none';
    });
}
