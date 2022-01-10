function ParseRoversNavigation(rover, xsize, ysize, yLength) {
    var mycanvas = document.getElementById('roverplateau');
    console.log(xsize, ysize);
    var ctx = mycanvas.getContext('2d');
    ctx.beginPath();
    var previous = rover.startPosition;
    var last = null;
    var count = 1;

    rover.navigatedPostions.map((navigatedPostions) => {
        if (last != null && count == 1) {
            ctx.fillText("(Final:" + last.x + "," + last.y + ")", (last.x * xsize) + 30, ((yLength - last.y) * ysize) + 30);

        }
        console.log(count)
        ctx.moveTo((previous.x * xsize) + xsize / 2, ((yLength - previous.y) * ysize) + ysize / 2);
        ctx.fillText("(" + previous.x + "," + previous.y + ")-" + count, (navigatedPostions.x * xsize) + xsize / 3, (yLength - navigatedPostions.y) * ysize + ysize / 3);
        count += 1;
        ctx.lineTo((navigatedPostions.x * xsize) + xsize / 2, ((yLength - navigatedPostions.y) * ysize) + ysize / 2);
        last = previous = navigatedPostions;

    });
    ctx.lineWidth = 5;
    ctx.strokeStyle = rover.color;
    ctx.stroke();
    //ctx.beginPath();
    //ctx.moveTo((0 * xsize) + 25, (0 * ysize) + 25);
    //ctx.fillText('S', 25, 25)
    //ctx.lineTo((0 * xsize) + 25, (1 * ysize) + 25);
    //ctx.lineWidth = 5;
    //ctx.strokeStyle = 'yellow'
    //ctx.stroke();
}

function DrawConnectors(json, xSize, ySize) {
    var yLength = json.plateau.gridYColum;
    json.rovers.map((rover) => ParseRoversNavigation(rover, xSize, ySize, yLength))

}


function DrawGridLines(mycanvas, x, y) {

    var a = 500 / x;
    var b = 500 / y;
    console.log(a);
    console.log(b);
    for (var i = 0; i <= (x + 1); i++) {
        var ctx = mycanvas.getContext('2d');
        ctx.moveTo(i * a, 0);
        ctx.lineTo(i * a, y * b);
    }
    for (var i = 0; i <= (y + 1); i++) {
        var ctx = mycanvas.getContext('2d');
        ctx.moveTo(0, i * b);
        ctx.lineTo(x * a, i * b);
    }


    ctx.lineWidth = 5;
    ctx.strokeStyle = 'green'
    ctx.stroke();

}