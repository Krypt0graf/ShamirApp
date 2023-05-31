$(document).ready(function () {
    const urlParams = new URLSearchParams(window.location.search);
    const myParam = urlParams.get('error');
    if (myParam === '1') {
        swal('Ошибка', "Некорректный логин или пароль", 'error');
    }
});