let debug = true;
let map = new Map();

$("#formvote").submit(function (event) {
    event.preventDefault();

    let radios = $('#formvote input[type="radio"]');

    for (let radio of radios) {
        let name = $(radio).attr('name');
        let id = $(radio).attr('id');
        map.set(name, !$(`#${id}`).prop('checked'));
    }

    let info = [];

    map.forEach((value, key) => {
        let func = gen_func(value ? 1 : -1);
        let points = get_random_points(func);
        logdebug(func.f);
        logdebug(points);
        info.push({
            id: +key.split('-')[1],
            points: points
        });
    });

    logdebug(info);

    $.ajax({
        url: '/Account/SendVote',
        method: 'post',
        //async: false,
        data: {
            idform: $('#formid').attr('formid'),
            info: JSON.stringify(info),
        },
        success: function (data) {
            let json = JSON.parse(data);

            if (json.result > 0)
                swal('Успешно', 'Ваши ответы приняты', 'success')
                .then(() => {
                    window.location.replace("/Account");
                });
            else
                swal('Ошибка', 'Ошибка при сохранении результатов!', 'error')
                .then(() => {
                    window.location.replace("/Account");
                });       
        }
    });
});
 
function gen_func(c, max_a = 50, max_b = 50) {
    let _a = getRandomInt(max_a);
    let _b = getRandomInt(max_b);
    return {
        a: _a, b: _b, c: c,
        f: `f(x) = ${_a}x² + ${_b}x + ${c}`
    };
}

function get_random_points(func) {
    let points = [];
    let xs = [5, 8, 13];
    for (var i = 0; i < xs.length; i++) {
        let fx = func.a * xs[i] * xs[i] + func.b * xs[i] + func.c;
        points.push({ x: xs[i], y: fx});
    }
    return points;
}

function getRandomInt(max) {
    let x = 0;
    while (x === 0) {
        x = Math.floor(Math.random() * max);
    }
    return x;
}
function logdebug(msg) {
    if (debug)
        console.log(msg)
}