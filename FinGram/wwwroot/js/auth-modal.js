        // Открытие формы авторизации
document.getElementById('openAuthForm').addEventListener('click', function (e) {
    e.preventDefault();
    document.getElementById('authModal').style.display = 'block';
    document.getElementById('overlay').style.display = 'block';
});

    // Закрытие формы авторизации
document.getElementById('authClose').addEventListener('click', function () {
    document.getElementById('authModal').style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
});

    // Переход к форме регистрации
document.getElementById('registerLink').addEventListener('click', function (e) {
    e.preventDefault();
    document.getElementById('authModal').style.display = 'none';
    document.getElementById('regModal').style.display = 'block';
});

    // Закрытие формы регистрации
document.getElementById('regClose').addEventListener('click', function () {
    document.getElementById('regModal').style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
});

    // Закрытие формы при клике на оверлей
document.getElementById('overlay').addEventListener('click', function () {
    document.getElementById('authModal').style.display = 'none';
    document.getElementById('regModal').style.display = 'none';
    document.getElementById('overlay').style.display = 'none';
});