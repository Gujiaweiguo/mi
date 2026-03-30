function my_renderer(value, record, columnObj, grid, colNo, rowNo) {
    var no = record[columnObj.fieldIndex];
    var color = "000000";
    return "<span style=\"color:" + color + ";\">" + no + "</span>";
}

function my_Numrenderer(value, record, columnObj, grid, colNo, rowNo) {
    var no = record[columnObj.fieldIndex];
    if (no <= 0) {
        var color = "ff0000";
        return "<span style=\"color:" + color + ";\">" + no + "</span>";
    } else if (no > 0) {
        var color = "000000";
        return "<span style=\"color:" + color + ";\">" + no + "</span>";
    }
}

function my_hdRenderer(header, colObj, grid) {
    var color = "666666";
    return "<span style=\"color:" + color + ";\" font:24px;>" + String(header) + "</span>";
}

function my_Imgrenderer(value, record, columnObj, grid, colNo, rowNo) {
    var no = record[columnObj.fieldIndex];
    return "<img style=\"padding-top:4px;\" src=\"" + no.toLowerCase() + "\">";
}