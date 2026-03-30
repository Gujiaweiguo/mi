<%@ Page language="c#" codePage="936"%>
<html>
<head>
<title>设置背景色</title>
<meta http-equiv="Pragma" content="no-cache"/>
<meta http-equiv="Window-target" content="_top"/>
<meta http-equiv="Content-Language" content="zh-cn"/>
<meta http-equiv="Content-Type" content="text/html; charset=gb2312"/>

    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script type="text/javascript" src="../../JavaScript/TreeShow.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>

<style type="text/css">
body,a,table,div,span,td,th,input,select{font:9pt;font-family: "宋体", Verdana, Arial, Helvetica, sans-serif;}
</style>
<body bgcolor=menu topmargin="0" leftmargin="0" style="background-color:#E1E0B2">
<script >
var SelRGB = '#000000';
var DrRGB = '#000000';
var SelGRAY = '120';

var hexch = new Array('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F');

function ToHex(n){
  var h, l;
  n = Math.round(n);
  l = n % 16;
  h = Math.floor((n / 16)) % 16;
  return (hexch[h] + hexch[l]);
}

function DoColor(c, l){
  var r, g, b;
  r = '0x' + c.substring(1, 3);
  g = '0x' + c.substring(3, 5);
  b = '0x' + c.substring(5, 7);

  if(l > 120) {
    l = l - 120;
    r = (r * (120 - l) + 255 * l) / 120;
    g = (g * (120 - l) + 255 * l) / 120;
    b = (b * (120 - l) + 255 * l) / 120;
  } else {
    r = (r * l) / 120;
    g = (g * l) / 120;
    b = (b * l) / 120;
  }
  return '#' + ToHex(r) + ToHex(g) + ToHex(b);
}

function EndColor(){
  var i;
  if(DrRGB != SelRGB) {
    DrRGB = SelRGB;
    for(i = 0; i <= 30; i ++)
      GrayTable.rows(i).bgColor = DoColor(SelRGB, 240 - i * 8);
  }
  SelColor.value = DoColor(RGB.innerText, GRAY.innerText);
  ShowColor.bgColor = SelColor.value;
}
</script></head>
<table border="1" cellspacing="0" cellpadding="0" width="260" align="center">
  <tr>
    <td> <table id=ColorTable border="0" cellspacing="0" cellpadding="0" style='cursor:hand'
onclick		='SelRGB = event.srcElement.bgColor;		EndColor();'
onmouseover	='RGB.innerText = event.srcElement.bgColor;	EndColor();'
onmouseout	='RGB.innerText = SelRGB;			EndColor();'
>
        <script>
function wc(r, g, b, n) {
  r = ((r * 16 + r) * 3 * (15 - n) + 0x80 * n) / 15;
  g = ((g * 16 + g) * 3 * (15 - n) + 0x80 * n) / 15;
  b = ((b * 16 + b) * 3 * (15 - n) + 0x80 * n) / 15;
  document.write('<td bgcolor=#' + ToHex(r) + ToHex(g) + ToHex(b) + ' height=8 width=8></td>');
}

var cnum = new Array(1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0);

for(i = 0; i < 16; i ++) {
  document.write('<tr>');
  for(j = 0; j < 30; j ++) {
    n1 = j % 5;
    n2 = Math.floor(j / 5) * 3;
    n3 = n2 + 3;
    wc((cnum[n3] * n1 + cnum[n2] * (5 - n1)),
    (cnum[n3 + 1] * n1 + cnum[n2 + 1] * (5 - n1)),
    (cnum[n3 + 2] * n1 + cnum[n2 + 2] * (5 - n1)), i);
  }
  document.writeln('</tr>');
}
</script>
      </table></td>
    <td valign=bottom> <table ID=GrayTable border="0" cellspacing="0" cellpadding="0" height=100% style='cursor:hand'
onclick		='SelGRAY = event.srcElement.title;		EndColor();'
onmouseover	='GRAY.innerText = event.srcElement.title;	EndColor();'
onmouseout	='GRAY.innerText = SelGRAY;			EndColor();'
>
        <script>
  for(i = 255; i >= 0; i -= 8.5)
     document.write('<tr bgcolor=#' + ToHex(i) + ToHex(i) + ToHex(i) + '><td title=' + Math.floor(i * 16 / 17) + ' height=4 width=20></td></tr>');
</script>
      </table></td>
  </tr>
</table>
<table border="1" cellspacing="10" cellpadding="0" width="260" align="center">
  <tr>
    <td rowspan="2" align="center"> <table ID=ShowColor border="0" cellspacing="0" cellpadding="0" width="80" height="30">
        <tr>
          <td></td>
        </tr>
      </table>
      <input class=text type=text size=7 ID=SelColor> </td>
    <td rowspan="2" width="80" style="line-height:16pt"> 基色: <span ID=RGB></span><br>
      亮度: <span ID=GRAY>120</span> </td>
    <td> <input class=button type=submit onclick='window.returnValue = SelColor.value;window.close();' value="确　定" CssClass="buttonSave"> 
    </td>
  </tr>
</table>
</body>
</html>
