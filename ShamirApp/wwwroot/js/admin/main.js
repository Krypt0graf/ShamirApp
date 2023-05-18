//$(document).ready(function () {

$("#newUser").on("click", function () {
    $('#usermodal').modal('show'); // Показываем модалку
});

/*$("#usermodal_ok").on("click", function () {
	
});*/

$("#newUserForm").submit(function (event) {
	event.preventDefault();
	let login = $('#input-login').val();
	let password = $('#input-password').val();
	let fio = $('#input-fio').val();
	console.log(login);
	$.ajax({
		url: '/Admin/AddNewUser',
		method: 'post',
		dataType: 'html',
		data: {
			login: login,
			password: password,
			fio: fio
		},
		success: function (data) {
			let id = JSON.parse(data).id;
			if (id === 0)
				alert('Пользователь не добавлен - попробуйте еще раз');
			else if (id === -1)
				alert('Пользователь не добавлен - пользователь с такими данными уже существует');
			else
				AddNewUserRow(id, login, password, fio);
			$('#usermodal').modal('hide');
		}
	});
});
function AddNewUserRow(id, login, password, fio) {
	let row = $(
		'<td>' + id + '</td>' +
		'<td>' + login + '</td>' +
		'<td>' + password + '</td>' +
		'<td>' + fio + '</td>' +
		'<td>' +
			'<button class="mt-1 mb-1 btn btn-sm btn-info btn-block" onclick="editUser(' + id + ')">Редактировать</button>' +
			'<button class="mt-1 mb-1 btn btn-sm btn-danger btn-block" onclick="deleteUser(' + id + ')">Удалить</button>' +
        '</td>'
	);
	$('#t_users > tbody').append(row)
	$('#t_users > thead').removeClass('d-none');
}
function editUser(id) {
	alert('edit ' + id);
}
function deleteUser(id) {
	alert('delete ' + id);
}