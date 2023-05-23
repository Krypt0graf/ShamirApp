
let CurrentId = 0;

// Кнопка новый пользователь
$("#newUser").on("click", function () {
	DefaultModal();
    $('#usermodal').modal('show'); // Показываем модалку
});
// Кнопки Добавить / Сохранить на модалке
$("#newUserForm").submit(function (event) {

	let button = $(event.originalEvent.submitter).attr('id');
	event.preventDefault();

	let login = $('#input-login').val();
	let password = $('#input-password').val();
	let fio = $('#input-fio').val();

	if (button === "add") {
		$.ajax({
			url: '/Admin/AddNewUser',
			method: 'post',
			data: {
				login: login,
				password: password,
				fio: fio
			},
			success: function (data) {
				let id = JSON.parse(data).id;
				if (id === 0)
					alert('Пользователь не добавлен - некорректные данные');
				else if (id === -1)
					alert('Пользователь не добавлен - скорее всего, пользователь с такими данными уже существует');
				else
					AddNewUserRow(id, login, password, fio);
				$('#usermodal').modal('hide');
			}
		});
	} else if (button === "save") {
		$.ajax({
			url: '/Admin/EditUser',
			method: 'put',
			data: {
				id: CurrentId,
				login: login,
				password: password,
				fio: fio
			},
			success: function (data) {
				let rows = JSON.parse(data).rows;
				if (rows === 0)
					alert('Произошла ошибка - кажется, такого пользователя нет');
				else if (rows === -1)
					alert('Пользователь не отредактирован - ошибка при сохранении изменений, проверьте данные');
				else
					EditUserRow(CurrentId, login, password, fio);
				$('#usermodal').modal('hide');
			}
		});
	}
});

// Редактирование пользователя
function EditUser(e) {
	var row = $(e).parent().parent();
	let id = $(row).children('td')[0].innerHTML;
	let login = $(row).children('td')[1].innerHTML;
	let password = $(row).children('td')[2].innerHTML;
	let fio = $(row).children('td')[3].innerHTML;
	EditModal(id, login, password, fio);
	CurrentId = id;
	$('#usermodal').modal('show'); // Показываем модалку
}

// Удаление пользователя
function DeleteUser(e) {
	var row = $(e).parent().parent();
	let id = $(row).children('td')[0].innerHTML;
	CurrentId = id;
	if (confirm("Удалить пользователя Id=" + id)) {
		$.ajax({
			url: '/Admin/DeleteUser',
			method: 'delete',
			data: {
				id: id
			},
			success: function (data) {
				let rows = JSON.parse(data).rows;
				if (rows === 0)
					alert('Пользователь не удален - некорректные данные');
				else if (rows === -1)
					alert('Пользователь не удален - ошибка при сохранении изменений, проверьте данные');
				else
					DeleteUserRow(id);
			}
		});
	}
}

// Добавление новой строки в таблицу
function AddNewUserRow(id, login, password, fio) {
	let row = $(
		'<td>' + id + '</td>' +
		'<td>' + login + '</td>' +
		'<td>' + password + '</td>' +
		'<td>' + fio + '</td>' +
		'<td>' +
		'<button class="mt-1 mb-1 btn btn-sm btn-info btn-block" onclick="EditUser(this)">Редактировать</button>' +
		'<button class="mt-1 mb-1 btn btn-sm btn-danger btn-block" onclick="DeleteUser(this)">Удалить</button>' +
		'</td>'
	);
	$('#t_users > tbody').append(row)
	$('#t_users > thead').removeClass('d-none');
}

// Редактирование строки пользователя
function EditUserRow(id, login, password, fio) {
	row = findrow(id, $('#t_users > tbody > tr'));
	$(row).children('td')[1].innerHTML = login;
	$(row).children('td')[2].innerHTML = password;
	$(row).children('td')[3].innerHTML = fio;
}

function DeleteUserRow(id) {
	row = findrow(id, $('#t_users > tbody > tr'));
	$(row).remove();
}

function findrow(id, rows) {
	for (let row of rows) {
		if (row.children[0].innerText == id)
			return row;
	}
}

// Дефолтная модалка
function DefaultModal() {
	$('#ModalTitle').text('Новый пользователь');
	$('#input-login').val('');
	$('#input-password').val('');
	$('#input-fio').val('');
	$('#add').removeClass('d-none');
	$('#save').addClass('d-none');
}

// Модалка для редактирования пользователя
function EditModal(id, login, password, fio) {
	$('#ModalTitle').text('Редактирование пользователя Id=' + id);
	$('#input-login').val(login);
	$('#input-password').val(password);
	$('#input-fio').val(fio);
	$('#add').addClass('d-none');
	$('#save').removeClass('d-none');
}