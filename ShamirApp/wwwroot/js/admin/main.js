
let CurrentId = 0;

// Кнопка новый пользователь
$("#newUser").on("click", function () {
	DefaultModal();
    $('#usermodal').modal('show'); // Показываем модалку
})

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
				let json = JSON.parse(data);

				/*if (json.status !== 200) {
					alert('Вы не авторизованы! Авторизуйтесь снова');
					return;
				}*/

				if (json.id === 0)
					swal('Ошибка', 'Пользователь не добавлен - некорректные данные!', 'error');
				else if (json.id === -1)
					swal('Ошибка', 'Пользователь не добавлен - скорее всего, пользователь с такими данными уже существует', 'error');
				else {
					AddNewUserRow(json.id, login, password, fio);
					swal('Успешно', 'Новый пользователь добавлен', 'success');
				}	
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
				let json = JSON.parse(data);

				/*if (json.status !== 200) {
					alert('Вы не авторизованы! Авторизуйтесь снова');
					return;
				}*/

				if (json.rows === 0)
					swal('Ошибка', 'Произошла ошибка - кажется, такого пользователя нет', 'error');
				else if (json.rows === -1)
					swal('Ошибка', 'Пользователь не отредактирован - ошибка при сохранении изменений, проверьте данные', 'error');
				else {
					EditUserRow(CurrentId, login, password, fio);
					swal('Успешно', 'Данные обновлены', 'success');
				}
				$('#usermodal').modal('hide');
			}
		});
	}
})

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
				let json = JSON.parse(data);

				/*if (json.status !== 200) {
					alert('Вы не авторизованы! Авторизуйтесь снова');
					return;
				}*/

				if (json.rows === 0)
					swal('Ошибка', 'Пользователь не удален - некорректные данные', 'error');
				else if (json.rows === -1)
					swal('Ошибка', 'Пользователь не удален - ошибка при сохранении изменений, проверьте данные', 'error');
				else {
					DeleteUserRow(id);
					swal('Успешно', 'Пользователь удален', 'success');
				}
					
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

// удаление строки из таблицы пользователей
function DeleteUserRow(id) {
	row = findrow(id, $('#t_users > tbody > tr'));
	$(row).remove();
}

// Поиск строки по первому столбцу
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

/*---------------------------------------------------------------------------------------*/

// Кнопка новая анкета
$("#newForm").on("click", function () {
	DefaultFormModal();
	$('#modalNewForm').modal('show'); // Показываем модалку
})

$('#addtextblock').click(function (event) { /* создаем блок для добавления текста */
	event.preventDefault();
	addblocktext();
});

/* Функция добавления блока текста */
function addblocktext(text = '') {
	let placeholder = 'Добавьте сюда текст вопроса';
	var textblock = $(
		'<div type="0" class="border mb-2" style="border-radius: 0.25rem;">' +
			'<div class="w-100" style="display: flex; justify-content: flex-end;">' +
				'<button title="Удалить этот блок" onclick="deleteblock(this);" class="btn btn-danger mr-0">✕</button>' +
			'</div>' +
		'<textarea placeholder="' + placeholder + '" style="width:100%; outline:none; border:none;" required>' + text + '</textarea>' +
		'</div>');
	$('#newcontainer').append(textblock);
}

/* Функция удаления блока из разметки */
function deleteblock(e) {
	var block = $(e).parent().parent(); // находим родителя от кликнутой кнопки
	block.remove(); // удаляем
};

$("#modalNewForm").submit(function (event) {
	let button = $(event.originalEvent.submitter).attr('id');
	event.preventDefault();

	let title = $('#TitleForm').val();
	let areas = $('#newcontainer').find('textarea');
	let qs = [];
	for (let area of areas) {
		let text = $(area).val();
		qs.push(text);
	}
	if (title.length === 0) {
		alert('Кажется, не заполнено название анкеты');
		return;
	}
	if (qs.length === 0) {
		alert('Для добавления анкеты, нужно добавить хотя бы один вопрос');
		return;
	}

	if (button === "add") {
		$.ajax({
			url: '/Admin/AddNewForm',
			method: 'post',
			data: {
				title: title,
				qs: qs
			},
			success: function (data) {
				let json = JSON.parse(data);

				/*if (json.status !== 200) {
					alert('Вы не авторизованы! Авторизуйтесь снова');
					return;
				}*/

				if (json.id === 0)
					swal('Ошибка', 'Анкета не добавлена - некорректные данные', 'error');
				else
					swal('Успешно', 'Анкета добавлена: Id = ' + json.id + ', Количество вопросов ' + json.count, 'success');
					//AddNewUserRow(id, login, password, fio);
				$('#usermodal').modal('hide');
			}
		});
	} else {
		//edit
	}

	


});

// Редактирование анкеты
function GetInfo(e) {
	var row = $(e).parent().parent();
	let id = $(row).children('td')[0].innerHTML;
	let title = $(row).children('td')[1].innerHTML;
	let questions = [];
	$.ajax({
		url: '/Admin/GetQuestions',
		method: 'get',
		async: false,
		data: {
			idform: id,
		},
		success: function (data) {
			json = JSON.parse(data);
			questions = json;
			/*if (json.status !== 200) {
				alert('Вы не авторизованы! Авторизуйтесь снова');
				return;
			}*/

			//console.log(qs[0]);
		}
	});
	
	InfoFormModal(id, title, questions);
	$('#modalNewForm').modal('show'); // Показываем модалку
}

// Модалка для редактирования анкеты
function InfoFormModal(id, title, qs) {
	$('#modalFormTitle').text('Редактирование анкеты Id=' + id);
	$('#TitleForm').val(title);
	$('#add').addClass('d-none');
	$('#newcontainer').empty();
	for (let q of qs) {
		addblocktext(q.Item2);
	}
}
function DefaultFormModal() {
	$('#modalFormTitle').text('Редактор анкеты');
	$('#TitleForm').val('');
	$('#add').removeClass('d-none');
	$('#newcontainer').empty();
}
function DeleteForm(e) {
	var row = $(e).parent().parent();
	let id = $(row).children('td')[0].innerHTML;
	CurrentId = id;
	if (confirm("Удалить анкету Id=" + id)) {
		$.ajax({
			url: '/Admin/DeleteForm',
			method: 'delete',
			data: {
				id: id
			},
			success: function (data) {
				let json = JSON.parse(data);

				/*if (json.status !== 200) {
					alert('Вы не авторизованы! Авторизуйтесь снова');
					return;
				}*/

				if (json.rows === 0)
					swal('Ошибка', 'Анкета не удалена - некорректные данные', 'error');
				else if (json.rows === -1)
					swal('Ошибка', 'Анкета не удалена - ошибка при сохранении изменений, проверьте данные', 'error');
				else {
					DeleteFormRow(id);
					swal('Успешно', 'Анкета удалена', 'success');
				}
					
			}
		});
	}
}
// удаление строки из таблицы пользователей
function DeleteFormRow(id) {
	row = findrow(id, $('#t_forms > tbody > tr'));
	$(row).remove();
}

function GetResultForm(e) {
	var row = $(e).parent().parent();
	let id = $(row).children('td')[0].innerHTML;
	let title = $(row).children('td')[1].innerHTML;
	$.ajax({
		url: '/Admin/GetResult',
		method: 'get',
		async: false,
		data: {
			idform: id,
		},
		success: function (data) {
			json = JSON.parse(data);
			let countVotes = json.CountVotes;
			let results = json.Results; // IdQuestion Text Value
			Showresults(title, countVotes, results);
		}
	});
}
function Showresults(title, countVotes, results) {
	DefaultResultForm();
	$('#resultsmodal_title').text(title);
	$('#resultsmodal_count').text(`Количество участников: ${countVotes}`);
	if (results.length > 0) {
		$('#resultsmodal_head').removeClass('d-none');
		for (let res of results) {
			let block = $(
				'<tr>' +
				'<td>' + res.IdQuestion + '</td>' +
				'<td>' + res.Text + '</td>' +
				'<td>' + res.Value + '</td>' +
				'</tr>');
			$('#resultsmodal_body').append(block);
		}
	}
	$('#resultsmodal').modal('show');
}
function DefaultResultForm() {
	$('#resultsmodal_title').text('');
	$('#resultsmodal_count').text('');
	$('#resultsmodal_head').addClass('d-none');
	$('#resultsmodal_body').empty();
}