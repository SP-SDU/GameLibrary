function previewImage(event) {
    const files = event.target.files;
    if (files.length > 0) {
        const preview = document.getElementById('imagePreview');
        preview.src = URL.createObjectURL(files[0]);
        preview.style.display = 'block';
    }
}
