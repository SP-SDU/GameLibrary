const gameDetails = {
    init(gameId) {
        this.gameId = gameId;
        this.loadGameDetails();
        this.loadReviews();
        this.setupEventListeners();
    },

    setupEventListeners() {
        const form = document.getElementById('reviewForm');
        if (form) {
            form.addEventListener('submit', (e) => {
                e.preventDefault();
                this.submitReview(form);
            });
        }
    },

    async loadGameDetails() {
        try {
            const response = await fetch(`/api/games/${this.gameId}`);
            if (!response.ok) throw new Error('Failed to load game details');
            const data = await response.json();
            this.updateGameDetails(data);
        } catch (error) {
            console.error('Error loading game details:', error);
            this.showError('Failed to load game details. Please try again.');
        }
    },

    async loadReviews() {
        try {
            const response = await fetch(`/api/games/${this.gameId}/reviews`);
            if (!response.ok) throw new Error('Failed to load reviews');
            const reviews = await response.json();
            this.updateReviews(reviews);
        } catch (error) {
            console.error('Error loading reviews:', error);
            this.showError('Failed to load reviews. Please try again.');
        }
    },

    async submitReview(form) {
        const submitButton = form.querySelector('button[type="submit"]');
        const originalText = submitButton.textContent;
        submitButton.disabled = true;
        submitButton.textContent = 'Submitting...';

        try {
            const rating = form.querySelector('select[name="Rating"]').value;
            const comment = form.querySelector('textarea[name="Comment"]').value;

            const response = await fetch(`/api/games/${this.gameId}/reviews`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({ rating: parseInt(rating), comment })
            });

            if (!response.ok) throw new Error('Failed to submit review');
            
            const newReview = await response.json();
            this.addNewReview(newReview);
            this.updateGameRating(newReview.gameRating);
            form.reset();
            this.showSuccess('Review submitted successfully!');
        } catch (error) {
            console.error('Error submitting review:', error);
            this.showError('Failed to submit review. Please try again.');
        } finally {
            submitButton.disabled = false;
            submitButton.textContent = originalText;
        }
    },

    updateGameDetails(game) {
        document.getElementById('gameTitle').textContent = game.title;
        document.getElementById('gameDescription').textContent = game.description;
        document.getElementById('gameDeveloper').textContent = game.developer;
        document.getElementById('gamePublisher').textContent = game.publisher;
        document.getElementById('gameReleaseDate').textContent = new Date(game.releaseDate).toLocaleDateString();
        this.updateGameRating(game.rating);
    },

    updateGameRating(rating) {
        const ratingContainer = document.querySelector('.game-rating');
        if (ratingContainer) {
            const stars = ratingContainer.querySelectorAll('i');
            stars.forEach((star, index) => {
                if (rating >= index + 1) {
                    star.className = 'fas fa-star text-warning';
                } else {
                    star.className = 'far fa-star text-warning';
                }
            });
            const ratingText = ratingContainer.querySelector('span');
            if (ratingText) {
                ratingText.textContent = `(${rating.toFixed(1)})`;
            }
        }
    },

    updateReviews(reviews) {
        const container = document.getElementById('reviewsList');
        if (!container) return;

        container.innerHTML = reviews.map(review => `
            <div class="review mb-3">
                <div class="d-flex justify-content-between">
                    <h5>${review.userName}</h5>
                    <div class="text-warning">
                        ${this.getStarRating(review.rating)}
                    </div>
                </div>
                <p class="review-text">${review.content}</p>
                <small class="text-muted">${new Date(review.createdAt).toLocaleDateString()}</small>
            </div>
        `).join('');
    },

    addNewReview(review) {
        const container = document.getElementById('reviewsList');
        if (!container) return;

        const reviewElement = document.createElement('div');
        reviewElement.className = 'review mb-3';
        reviewElement.innerHTML = `
            <div class="d-flex justify-content-between">
                <h5>${review.userName}</h5>
                <div class="text-warning">
                    ${this.getStarRating(review.rating)}
                </div>
            </div>
            <p class="review-text">${review.content}</p>
            <small class="text-muted">${new Date(review.createdAt).toLocaleDateString()}</small>
        `;
        container.insertBefore(reviewElement, container.firstChild);
    },

    getStarRating(rating) {
        return Array(5).fill(0).map((_, i) => 
            `<i class="${rating > i ? 'fas' : 'far'} fa-star"></i>`
        ).join('');
    },

    showError(message) {
        const errorAlert = document.getElementById('errorAlert');
        const successAlert = document.getElementById('successAlert');
        if (successAlert) successAlert.classList.add('d-none');
        if (errorAlert) {
            errorAlert.textContent = message;
            errorAlert.classList.remove('d-none');
            setTimeout(() => errorAlert.classList.add('d-none'), 5000);
        }
    },

    showSuccess(message) {
        const successAlert = document.getElementById('successAlert');
        const errorAlert = document.getElementById('errorAlert');
        if (errorAlert) errorAlert.classList.add('d-none');
        if (successAlert) {
            successAlert.textContent = message;
            successAlert.classList.remove('d-none');
            setTimeout(() => successAlert.classList.add('d-none'), 5000);
        }
    }
};
