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
        data: {
            idform: $('#formid').attr('formid'),
            info: JSON.stringify(info),
        },
        success: function (data) {
            let json = JSON.parse(data);

            if (json.status !== 200) {
                alert('Вы не авторизованы! Авторизуйтесь снова');
                return;
            }

            if (json.result === 1)
                alert('ok');
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

function get_random_points(func, count = 3, range = 50) {
    let points = [];
    for (var i = 0; i < 3; i++) {
        let x = 0;
        while (x <= 1 || points.indexOf(x) != -1) {
            x = getRandomInt(range);
        }
        let fx = func.a * x * x + func.b * x + func.c;
        points.push({x: x, y: fx});
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