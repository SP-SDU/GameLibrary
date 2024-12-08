// Function to get a random integer between min and max (inclusive)
function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

// Function to get a random integer between min and max (inclusive)
function getRandomInt1(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

// Flight path configuration for the first spaceship
const flightPath = {
    curviness: 1.5,
    autoRotate: true,
    values: Array.from({ length: 15 }, (_, i) => ({
        x: window.innerWidth * (i + 1) * 0.1,
        y: window.innerHeight * getRandomInt(-10, -1) * 0.1
    }))
};

// Flight path configuration for the second spaceship
const flightPath1 = {
    curviness: 1.5,
    autoRotate: true,
    values: Array.from({ length: 15 }, (_, i) => ({
        x: window.innerWidth * (i + 1) * -0.1,
        y: window.innerHeight * getRandomInt1(-10, -1) * 0.1
    }))
};

// Create a new timeline for the first spaceship animation
const tween = new TimelineLite();

// Create a new timeline for the second spaceship animation
const tween1 = new TimelineLite();

// Add the first spaceship animation to the timeline
tween.add(
    TweenLite.to(".spaceship", 60, {
        bezier: flightPath,
        ease: Power1.easeInOut
    })
);

// Add the second spaceship animation to the timeline
tween1.add(
    TweenLite.to(".spaceship1", 60, {
        bezier: flightPath1,
        ease: Power1.easeInOut
    })
);

// Animate hero section elements on page load
document.addEventListener("DOMContentLoaded", function () {
    const heroTitle = document.getElementsByClassName("hero-title")[0];
    const heroSubtitle = document.getElementsByClassName("hero-subtitle")[0];
    const heroDescription = document.getElementsByClassName("hero-description")[0];
    const heroButtons = document.getElementsByClassName("hero-buttons")[0];

    // Set initial opacity to 0 for all hero section elements
    TweenLite.set([heroTitle, heroSubtitle, heroDescription, heroButtons], { opacity: 0 });

    // Animate opacity to 1 with delays
    TweenLite.to(heroTitle, 1, { opacity: 1, delay: 1 });
    TweenLite.to(heroSubtitle, 1, { opacity: 1, delay: 3 });
    TweenLite.to(heroDescription, 1, { opacity: 1, delay: 5 });
    TweenLite.to(heroButtons, 1, { opacity: 1, delay: 8 });
});

// Add hover and touch effects to feature cards
document.addEventListener('DOMContentLoaded', function () {
    const featureCards = document.querySelectorAll('.feature-card');

    // Desktop screen size
    featureCards.forEach(card => {
        card.addEventListener('mouseover', function () {
            card.style.transform = 'translateY(-1rem)';
            card.style.boxShadow = '0px 1rem 3rem rgba(138, 43, 226, 1)';
        });

        card.addEventListener('mouseout', function () {
            card.style.transform = 'translateY(0)';
            card.style.boxShadow = '0 .125rem .25rem rgba(0, 0, 0, .075)';
        });
    });

    // Mobile screen size
    if (window.innerWidth <= 600) {
        featureCards.forEach(card => {
            card.addEventListener('touchstart', function () {
                card.style.transform = 'translateY(-1rem)';
                card.style.boxShadow = '0px 1rem 3rem rgba(138, 43, 226, 1)';
            });

            card.addEventListener('touchend', function () {
                card.style.transform = 'translateY(0)';
                card.style.boxShadow = '0 .125rem .25rem rgba(0, 0, 0, .075)';
            });
        });
    }
});
