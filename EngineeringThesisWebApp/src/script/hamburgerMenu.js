
document.addEventListener('DOMContentLoaded', function () {
    const commentButton = document.getElementById('hamburgerButton');
    const unwrapMenu = document.getElementById('unwrap');

    commentButton.addEventListener('click', function () {
        // Po kliknięciu przycisku pokaż sekcję komentarzy
        unwrapMenu.style.display = unwrapMenu.style.display === 'none' ? 'block' : 'none';
    });
});