// Game Details Page JavaScript

document.addEventListener('DOMContentLoaded', function () {
    initializeStarRating();
    setupReviewForm();
});

// Initialize star rating system
function initializeStarRating() {
    const ratingInputs = document.querySelectorAll('.rating-input input[type="radio"]');
    const ratingLabels = document.querySelectorAll('.rating-input label');

    ratingLabels.forEach(label => {
        // Replace the default star with a filled star when selected
        label.addEventListener('click', function () {
            const input = document.querySelector(`#${this.getAttribute('for')}`);
            const rating = input.value;
            updateStars(rating);
        });
    });

    function updateStars(rating) {
        ratingLabels.forEach(label => {
            const star = label.querySelector('i');
            const starValue = label.getAttribute('for').replace('star', '');

            if (starValue <= rating) {
                star.classList.remove('far');
                star.classList.add('fas');
            } else {
                star.classList.remove('fas');
                star.classList.add('far');
            }
        });
    }
}

// Setup review form submission
function setupReviewForm() {
    const form = document.querySelector('.add-review form');
    if (!form) return;

    // Add client-side validation
    form.addEventListener('submit', function (e) {
        const rating = form.querySelector('input[name="rating"]:checked');
        const comment = form.querySelector('textarea[name="comment"]');

        if (!rating) {
            e.preventDefault();
            alert('Please select a rating');
            return;
        }

        if (!comment.value.trim()) {
            e.preventDefault();
            alert('Please write a review comment');
            return;
        }
    });
}

// HTMX after swap event handler
document.addEventListener('htmx:afterSwap', function (event) {
    if (event.detail.target.classList.contains('game-reviews')) {
        // Reinitialize any JavaScript components in the reviews section
        // This runs after HTMX updates the reviews content
    }
});

// Add loading indicator for HTMX requests
document.addEventListener('htmx:beforeRequest', function (event) {
    const target = event.detail.target;
    if (target.classList.contains('game-reviews')) {
        target.innerHTML = '<div class="loading">Loading reviews...</div>';
    }
});

// Handle HTMX request errors
document.addEventListener('htmx:responseError', function (event) {
    const target = event.detail.target;
    if (target.classList.contains('game-reviews')) {
        target.innerHTML = '<div class="error">Error loading reviews. Please try again later.</div>';
    }
});

// Smooth scroll to reviews section when adding a new review
document.addEventListener('htmx:afterSettle', function (event) {
    if (event.detail.target.id === 'reviews-list') {
        event.detail.target.scrollIntoView({ behavior: 'smooth' });
    }
});

// Add animation when new reviews are loaded
document.addEventListener('htmx:afterSwap', function (event) {
    if (event.detail.target.classList.contains('reviews-list')) {
        const reviews = event.detail.target.querySelectorAll('.review-item');
        reviews.forEach((review, index) => {
            review.style.opacity = '0';
            review.style.transform = 'translateY(20px)';
            setTimeout(() => {
                review.style.transition = 'opacity 0.5s ease, transform 0.5s ease';
                review.style.opacity = '1';
                review.style.transform = 'translateY(0)';
            }, index * 100);
        });
    }
});
