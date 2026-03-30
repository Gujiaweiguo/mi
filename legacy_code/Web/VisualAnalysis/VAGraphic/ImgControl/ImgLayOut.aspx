<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ImgLayOut.aspx.cs" Inherits="VisualAnalysis_VAMenu_ImgControl_ImgLayOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="../../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <title>测试</title>
        <script  type="text/javascript" language="javascript">
Number.prototype.NaN0=function(){return isNaN(this)?0:this;}
var iMouseDown  = false;
var dragObject  = null;
var curTarget   = null;

function makeDraggable(item){
	if(!item) return;
	item.onmousedown = function(ev){
		dragObject  = this;
		mouseOffset = getMouseOffset(this, ev);
		return false;
	}
}

function getMouseOffset(target, ev){
	ev = ev || window.event;

	var docPos    = getPosition(target);
	var mousePos  = mouseCoords(ev);

	return {x:mousePos.x - docPos.x, y:mousePos.y - docPos.y};
}

function getPosition(e){
	var left = 0;
	var top  = 0;
	while (e.offsetParent){
		left += e.offsetLeft + (e.currentStyle?(parseInt(e.currentStyle.borderLeftWidth)).NaN0():0);
		top  += e.offsetTop  + (e.currentStyle?(parseInt(e.currentStyle.borderTopWidth)).NaN0():0);
		e     = e.offsetParent;
	}

	left += e.offsetLeft + (e.currentStyle?(parseInt(e.currentStyle.borderLeftWidth)).NaN0():0);
	top  += e.offsetTop  + (e.currentStyle?(parseInt(e.currentStyle.borderTopWidth)).NaN0():0);

	return {x:left, y:top};

}

function mouseCoords(ev){
	if(ev.pageX || ev.pageY){
		return {x:ev.pageX, y:ev.pageY};
	}
	return {
		x:ev.clientX + document.body.scrollLeft - document.body.clientLeft,
		y:ev.clientY + document.body.scrollTop  - document.body.clientTop
	};
}

function mouseDown(ev){
	ev         = ev || window.event;
	var target = ev.target || ev.srcElement;

	if(target.onmousedown || target.getAttribute('DragObj')){
		return false;
	}
}

function mouseUp(ev){

	dragObject = null;

	iMouseDown = false;
}


function mouseMove(ev){
	ev         = ev || window.event;


	/*
	We are setting target to whatever item the mouse is currently on

	Firefox uses event.target here, MSIE uses event.srcElement
	*/
	var target   = ev.target || ev.srcElement;
	var mousePos = mouseCoords(ev);

	if(dragObject){
		dragObject.style.position = 'absolute';
		dragObject.style.top      = mousePos.y - mouseOffset.y;
		dragObject.style.left     = mousePos.x - mouseOffset.x;

        
               
       var xid = dragObject.id.lastIndexOf('_'); 
       var txtxID = dragObject.id.substring(0,xid)+"_txtx"
       var txtyID = dragObject.id.substring(0,xid)+"_txty"
       document.getElementById(txtxID).value=mousePos.x - mouseOffset.x-parseInt(document.getElementById('img1').style.left,0);
       document.getElementById(txtyID).value=mousePos.y - mouseOffset.y-parseInt(document.getElementById('img1').style.top,0);
       
       var name = dragObject.id.substring(0,xid)+"_txtname"
       document.getElementById('txtunitcode').value=document.getElementById(name).value
       document.getElementById('txtunitx').value=mousePos.x - mouseOffset.x-parseInt(document.getElementById('img1').style.left,0);
       document.getElementById('txtunity').value=mousePos.y - mouseOffset.y-parseInt(document.getElementById('img1').style.top,0);


	}

	// track the current mouse state so we can compare against it next time
	lMouseState = iMouseDown;

	// this prevents items on the page from being highlighted while dragging
	if(curTarget || dragObject) return false;
}

document.onmousemove = mouseMove;
document.onmousedown = mouseDown;
document.onmouseup   = mouseUp;
window.onload = function() {
for (i = 0; i <= 99; i++)
{
if(i<10)
{
makeDraggable(document.getElementById('gvImages_ctl0'+i+'_img'));
}
else
{
makeDraggable(document.getElementById('gvImages_ctl'+i+'_img'));
}
}
makeDraggable(document.getElementById('DragImage5'));
}


        
</script>
</head>
<body style="margin:0px">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <table style="width: 1186px; height: 481px">
            <tr>
                <td colspan="3" rowspan="3" style="width: 727px; height: 489px">
                    <img  id="img1" runat="server" src="" style="left: 10px; position: absolute; top: 50px ; z-index:-1" />
                    &nbsp;&nbsp;
                    <table style="width: 793px; height: 495px">
                        <tr>
                            <td colspan="6" rowspan="3" style="height: 480px">
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td colspan="6" rowspan="1" style="height: 1px">
                                &nbsp;
                                <table class="tdBackColor" style="left: 10px; width: 150px; position: absolute; top: 10px; height: 30px;">
                                    <tr>
                                        <td rowspan="3">
                                <asp:TextBox ID="txtunitcode" runat="server" Width="90px" ></asp:TextBox></td>
                                        <td rowspan="3">
                                <asp:TextBox ID="txtunitx" runat="server" Width="50px"></asp:TextBox></td>
                                        <td rowspan="3">
                                <asp:TextBox ID="txtunity" runat="server" Width="50px"></asp:TextBox></td>
                                        <td rowspan="3" style="vertical-align: middle; text-align: center">
                                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="保存" CssClass="buttonSave" /></td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                    </tr>
                                </table>
                    <table style="width: 1px; height: 1px">
                        <tr>
                            <td colspan="3" rowspan="3" style="height: 1px; width: 1px;">
                            <asp:Panel ID="Panel1" runat="server" Height="1px"  Width="1px" ScrollBars="Vertical">
                                <asp:GridView ShowHeader="false" ID="gvImages" runat="server" Height="1px" Width="1px" AutoGenerateColumns="False">
                                <Columns>
                                <asp:BoundField HeaderText = "Image Name" DataField="unitid" />
                                <asp:TemplateField HeaderText="Image"> 
                                 <ItemTemplate>  
                                 <asp:Image ID="img" name='<%#  Eval("unitid") %>' runat="server"  style="left: 10px; position: absolute; top: 20px" ImageUrl='<%#  Eval("map") %>'/>  
                                 </ItemTemplate>   
                                 </asp:TemplateField>
                                 <asp:TemplateField >
                                            <ItemTemplate>
                                               <asp:TextBox ID="txtx"  runat="server"   Text='<%#  Eval("x") %>' Font-Size="9pt" Width="30px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      <asp:TemplateField>
                                            <ItemTemplate>
                                               <asp:TextBox ID="txty"  runat="server"  Text='<%#  Eval("y") %>' Font-Size="9pt" Width="30px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <ItemTemplate>
                                               <asp:TextBox ID="txtname"  runat="server"   Text='<%#  Eval("unitcode") %>' Font-Size="9pt" Width="30px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField >
                                            <ItemTemplate>
                                               <asp:TextBox ID="unitid"  runat="server"   Text='<%#  Eval("unitid") %>' Font-Size="9pt" Width="30px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                </Columns>
                                </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                        </tr>
                    </table>
                            </td>
                        </tr>
                    </table>
                    </td>
            </tr>
            <tr>
            </tr>
            <tr>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
