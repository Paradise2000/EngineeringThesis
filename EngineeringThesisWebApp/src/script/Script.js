document.addEventListener('DOMContentLoaded', function () {
    var navItems = document.querySelectorAll('nav ul li > a:not(:only-child)');
    var navDropdowns = document.querySelectorAll('.nav-dropdown');
    var navToggle = document.getElementById('nav-toggle');

    function toggleNavDropdown(e) {
        var siblings = this.nextElementSibling;

        siblings.style.display = (siblings.style.display === 'block') ? 'none' : 'block';

        navDropdowns.forEach(function (item) {
            if (item !== siblings) {
                item.style.display = 'none';
            }
        });

        e.stopPropagation();
    }

    function hideNavDropdowns() {
        navDropdowns.forEach(function (item) {
            item.style.display = 'none';
        });
    }

    function toggleNav() {
        document.querySelector('nav ul').style.display = (document.querySelector('nav ul').style.display === 'block') ? 'none' : 'block';
    }

    function toggleActive() {
        this.classList.toggle('active');
    }

    navItems.forEach(function (item) {
        item.addEventListener('click', toggleNavDropdown);
    });

    document.addEventListener('click', hideNavDropdowns);
    navToggle.addEventListener('click', toggleNav);
    navToggle.addEventListener('click', toggleActive);
});
