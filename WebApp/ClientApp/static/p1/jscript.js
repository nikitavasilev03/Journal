'use strict';
let message = 'Тоже учишь JS?';
alert(message);
let user_name = prompt('тебя как звать?', '');
hiing(user_name);
let user = 'Зубенко Михаил Петрович';
alert(`А я ${user}`);
// let isGreater = 4 > 1;
// alert(`4 > 1 = ${isGreater}`);
// символ "n" в конце означает, что это BigInt
//const bigInt = 1234567890123456789012345678901234567890 n;
// alert(Number("123")); // 123
// alert(Number("123z")); // NaN (ошибка чтения числа в "z")
// alert(Number(true)); // 1
// alert(Number(false)); // 0
// alert(`${'1' + 2} ${1 + 2}`)

// let ans = prompt("хотишь кекс?", "да");
// alert(`ты сказал ${ans}, но ты всёравно не в моём вкусе :)`)
// ans = confirm("дрочешь?");
// let res = ans ? 'а я бачив и всим роскажу :)' : 'пиздежом попахивает';
// alert(res);

alert('ищем минимум');
min();
alert('возводим в степань');
alert(pow());

function hiing(u_n) {
    alert(`нормального человека ${u_n} не назовут :)`);
}

function getNumber() {
    let x = prompt('Введи число:', '')
    return x;
}

function min(a = getNumber(), b = getNumber()) {

    if (a > b || a == b) {
        alert(`min = ${b}`);
    } else
        alert(`min = ${a}`);
}

function pow(a = getNumber(), b = getNumber()) {

    for (let i = 0; i < b; i++) {
        a = a * a;
    }
    return a;
}