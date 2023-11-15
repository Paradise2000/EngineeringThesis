document.addEventListener('DOMContentLoaded', function () {
    const starCount = document.getElementsByName('starCount');
    const starContainer = document.getElementsByName('star-container');

    function updateLayout() {
      const screenWidth = window.innerWidth;

      if (screenWidth <= 285) {
        starCount.forEach(element => (element.style.display = 'flex'));
        starContainer.forEach(element => (element.style.display = 'none'));
      } else {
        starCount.forEach(element => (element.style.display = 'none'));
        starContainer.forEach(element => (element.style.display = 'flex'));
      }
    }

    // Wywołaj funkcję aktualizacji przy załadowaniu strony i przy zmianie rozmiaru okna
    updateLayout();
    window.addEventListener('resize', updateLayout);
  });