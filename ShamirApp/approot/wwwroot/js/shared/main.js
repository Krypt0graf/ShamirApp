$(document).ready(function(){
    // тут тупо чилим :D
}); 
$('#addtextblock').click(function(){ /* создаем блок для добавления текста */ 
    addblocktext();
});
$('#addimageblock').click(function(){ /* создаем блок для добавления изображений */ 
    addblockimage();
});
$('#addvideoblock').click(function(){ /* создаем блок для добавления кода видео */ 
    addblockvideo();
});
/* Функция установки сегодняшней даты в inputdate */
function setdatenow(){
    var date = new Date();
    $('#date').attr('value', date.getFullYear() + '-' 
    + ((date.getMonth() + 1) + '').padStart(2, "0") + '-' 
    + (date.getDate() + '').padStart(2, "0"));
}
/* Функция добавления блока заголовка новости */
function addblockheader(text = 'Заголовок новости'){
    $('#newcontainer').append($('<h4 class="border p-2 mb-2" contenteditable="true">' + text + '</h4>'));
}
/* Функция добавления блока текста */
function addblocktext(text = 'Новый блок текста - добавьте ваш текст сюда'){
    var textblock = $(
        '<div type="text" class="border mb-2">' +
            '<div class="w-100" style="display: flex; justify-content: flex-end;">' +
                '<button title="Удалить этот блок" onclick="deleteblock(this);" class="btn btn-danger m-1 mr-0">✕</button>' +
            '</div>' +
            '<p class="p-2 m-1 mb-0" contenteditable="true">' + text + '</p>' +
        '</div>');
    $('#newcontainer').append(textblock);
}
/* Функция добавления блока изображения */
function addblockimage(url = 'adding.png', description = 'Добавьте подпись к рисунку'){
    var imageblock = $(
        '<div type="image" class="border mb-2">' + 
            '<div class="w-100" style="display: flex; justify-content: flex-end;">' +
                '<button title="Удалить этот блок" onclick="deleteblock(this);" class="btn btn-danger m-1 mr-0">✕</button>' +
            '</div>' +
            '<div class="m-2 row justify-content-around">' +
                '<div>' +
                    '<div title="Нажмите чтобы добавить\\изменить изображение" onclick="addimage(this)" style="display: flex; justify-content: center; cursor: pointer;">' +
                        '<input type="file" style="display: none;">' +
                        '<img class="img-fluid" src="' + url + '">' +
                    '</div>' +
                    '<p contenteditable="true" class="p-1 mt-2 text-center">' + description + '</p>' +
                '</div>' +
            '</div>' +
        '</div>');
    imageblock.find('input[type=file]').bind('change', function(){ // находим в элементе инпут-файл
        var img = $(this).next();// добавляем на него обработчик на выбор файла
        var reader = new FileReader();
        reader.onload = function (e) {
            img.attr('src', e.target.result);// создаем превьюшку изображения
        };
        reader.readAsDataURL(this.files[0]);
    });
    $('#newcontainer').append(imageblock);// добавляем его на страницу
}
/* Функция добавления блока видео */
function addblockvideo(src = ''){
    var textblock = $(
        '<div type="video" class="border mb-2">' + 
            '<div class="w-100" style="display: flex; justify-content: flex-end;">' + 
                '<button title="Удалить этот блок" onclick="deleteblock(this);" class="btn btn-danger m-1 mr-0">✕</button>' + 
            '</div>' + 
            '<div class="w-100" style="display: flex;">' + 
                '<textarea class="m-2 w-100 p-2" rows="5" style="resize: vertical;" placeholder="Это поле для вставки кода видео">' + src + '</textarea>' + 
            '</div>' + 
        '</div>');
    $('#newcontainer').append(textblock);
}
/* Функция удаления блока из разметки */
function deleteblock(e) {
    var block = $(e).parent().parent(); // находим родителя от кликнутой кнопки
    block.remove(); // удаляем
};
/* Функция имитацияя нажатия на fileinput для выбора нового изображения */
function addimage(elem){
    $(elem).children()[0].click(); // делаем клик по инпут-файлу
}
/* Функция очистки модального окна от блоков */
function clearmodal(){
    $('#newcontainer').empty(); // Очищаем модалку
}
/* Функция для кнопки 'Новая новость' */
function addnew(){
    $('#newcontainer').empty(); // Очищаем модалку
    addblockheader(); // Добавляем дефолт заголовок
    $('#modalnew').modal('show'); // Показываем модалку
}
/* Фукция редактирования новости */
function editnew(id_new){
    clearmodal();
    addblockheader('тестовый заголовок редактирования новости');
    addblocktext('текстовый тескт');
    addblockimage('null', '123321');
    addblockvideo('srcsrcsrc');

    $('#modalnew').modal('show'); // Показываем модалку
    /*$.ajax({
        url: '/index.php',
        method: 'post',
        dataType: 'html',
        data: {text: 'Текст'},
        success: function(data){
            alert(data);
        },
    });*/
}
function cycle(){
    $('#newcontainer').children().each(function(i, block){
        if(i == 0) return;
        console.log($(block).attr('type'));
    });
}