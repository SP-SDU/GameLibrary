function previewImage(event) {
    const files = event.target.files;
    if (files.length > 0) {
        const preview = document.getElementById('imagePreview');
        preview.src = URL.createObjectURL(files[0]);
        preview.style.display = 'block';
    }
}

// Function to get a random integer between min and max (inclusive)
function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function getRandomInt1(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

// Flight path configuration for the first spaceship
const flightPath = {
    curviness: 1.5,
    autoRotate: true,
    path: Array.from({ length: 15 }, (_, i) => ({
        x: window.innerWidth * (i + 1) * 0.1,
        y: window.innerHeight * getRandomInt(-10, -1) * 0.1,
    })),
};

// Flight path configuration for the second spaceship
const flightPath1 = {
    curviness: 1.5,
    autoRotate: false,
    path: Array.from({ length: 15 }, (_, i) => ({
        x: window.innerWidth * (i + 1) * -0.1,
        y: window.innerHeight * getRandomInt1(-10, -1) * 0.1,
    })),
};

// Create a new timeline for the first spaceship animation
gsap.timeline()
    .to(".RRspaceship", {
        duration: 60,
        motionPath: flightPath,
        ease: "power1.inOut",
    });

// Create a new timeline for the second spaceship animation
gsap.timeline()
    .to(".Uspaceship", {
        duration: 60,
        motionPath: flightPath1,
        ease: "power1.inOut",
    });

// Animate hero section elements on page load
document.addEventListener("DOMContentLoaded", function () {
    const heroTitle = document.querySelector(".hero-title");
    const heroSubtitle = document.querySelector(".hero-subtitle");
    const heroDescription = document.querySelector(".hero-description");
    const heroButtons = document.querySelector(".hero-buttons");

    // Set initial opacity to 0 for all hero section elements
    gsap.set([heroTitle, heroSubtitle, heroDescription, heroButtons], { opacity: 0 });

    // Animate opacity to 1 with delays
    gsap.to(heroTitle, { opacity: 1, duration: 1, delay: 1 });
    gsap.to(heroSubtitle, { opacity: 1, duration: 1, delay: 3 });
    gsap.to(heroDescription, { opacity: 1, duration: 1, delay: 5 });
    gsap.to(heroButtons, { opacity: 1, duration: 1, delay: 8 });
});

// Add hover and touch effects to feature cards
document.addEventListener('DOMContentLoaded', function () {
    const featureCards = document.querySelectorAll('.feature-card');

    // Desktop hover effects
    featureCards.forEach(card => {
        card.addEventListener('mouseover', () => {
            gsap.to(card, { y: -16, boxShadow: "0 1.5rem 3rem rgba(138, 43, 226, 1)", duration: 0.2 });
        });
        card.addEventListener('mouseout', () => {
            gsap.to(card, { y: 0, boxShadow: "0 .125rem .25rem rgba(0, 0, 0, .075)", duration: 0.2 });
        });
    });

    // Mobile touch effects
    if (window.innerWidth <= 600) {
        featureCards.forEach(card => {
            card.addEventListener('touchstart', () => {
                gsap.to(card, { y: -16, boxShadow: "0 1.5rem 3rem rgba(138, 43, 226, 1)", duration: 0.2 });
            });
            card.addEventListener('touchend', () => {
                gsap.to(card, { y: 0, boxShadow: "0 .125rem .25rem rgba(0, 0, 0, .075)", duration: 0.2 });
            });
        });
    }
});
