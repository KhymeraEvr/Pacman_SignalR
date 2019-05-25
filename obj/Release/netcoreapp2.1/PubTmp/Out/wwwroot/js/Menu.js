const FONT_NAME = 'Pixel';
//Subway Ticker
window.onload = function () {
    var canvas = document.getElementById("MenuCanvas");
    var ctx = canvas.getContext('2d');
    ctx.rect(0, 0, canvas.width, canvas.height)
    ctx.fillStyle = "#161616";
    ctx.fill();
    ctx.font = "60px Subway Ticker";
    ctx.fillStyle = "#ffe030";
    ctx.fillText("Pacman", canvas.width / 2 - 120, canvas.height / 2 - 60);
}

