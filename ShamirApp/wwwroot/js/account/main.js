$("#formvote").submit(function (event) {
    event.preventDefault();

    let radios = $('#formvote input[type="radio"]');

    let map = new Map();

    for (let radio of radios) {
        let name = $(radio).attr('name');
        let id = $(radio).attr('id');
        map.set(name, !$(`#${id}`).prop('checked'));
    }

    console.log(map);
});

// f(x) = ax² + bx + c 
function gen_func(c, max_a = 50, max_b = 50) {
    let _a = getRandomInt(max_a);
    let _b = getRandomInt(max_b);
    return { a: _a, b: _b, c: c,
        f: `f(x) = ${_a}x² + ${_b}x + ${c}`
    };
}

function get_random_points(func, count = 3, range = 50) {
    let points = [];
    for (var i = 0; i < 3; i++) {
        let x = 0;
        while (x <= 1) {
            x = getRandomInt(range);
        }
        let fx = func.a * x * x + func.b * x + func.c;
        points.push({x: x, y: fx});
    }
    console.log(func.f);
    return points;
}

function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}