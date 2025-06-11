
    // Открыть окно "Войти/ Зарегистрироваться"
const authBtn = document.getElementById('openAuthForm');
const authPopup = document.getElementById('authPopup');

authBtn.addEventListener('click', () => {
    authPopup.classList.toggle('show');
});

document.addEventListener('click', (e) => {
    if (!authBtn.contains(e.target) && !authPopup.contains(e.target)) {
        authPopup.classList.remove('show');
    }
});