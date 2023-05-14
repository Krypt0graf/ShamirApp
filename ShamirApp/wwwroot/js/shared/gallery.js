/* Функция имитацияя нажатия на fileinput для выбора нового изображения */
function addimage(elem){
    $(elem).children()[0].click(); // делаем клик по инпут-файлу
}
$('#files').on('change', function(){ // находим в элементе инпут-файл
    $('#addingfiles').empty();
    $('#addingfiles').append('<p>Выбранные файлы:</p>');
    for (let i = 0; i < this.files.length; i++) {
        $('#addingfiles').append('<b>' + this.files[i].name + ' (' + mathsize(this.files[i].size) + ')</b><br>');
      }
});
function clearmodal(){
    $('#phototype').find('option')[0].selected = true
    $('#addingfiles').empty();
    $('#files').val('');
}
function openmodal(){
    clearmodal();
    $('#modal').modal('show');
}
function mathsize(bytes){
    if((t = bytes) < 1024)
        return t + " bytes";
    if((t = bytes / 1024) < 1024)
        return t.toFixed(2) + " Kb";
    if((t = bytes / 1024 / 1024) < 1024)
        return t.toFixed(2) + " Mb";
    else
        return (t / 1024).toFixed(2) + " Gb";
}
$('.trash').on('click', function(){
    confirm('Удалить нахОй эту фотку?');
});