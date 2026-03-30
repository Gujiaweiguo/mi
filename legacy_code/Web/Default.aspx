<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script>
        //删除cookies
        function delCookie(name)
        {
        getCookie(name);
       /*var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var cval=getCookie(name);
        if(cval!=null) document.cookie= name + "="+cval+";expires="+exp.toGMTString();
        */
        }
        //读取cookies
function getCookie(name)
{
debugger;
var arr,reg=new RegExp("(^| )"+name+"=([^;]*)(;|$)");
if(arr=document.cookie.match(reg)) return unescape(arr[2]);
else return null;
}

function getCookie() 
{
debugger;
var c_name='Custumer';
  if (document.cookie.length>0)
    { 
	c_start=document.cookie.indexOf(c_name + "=")
	if (c_start!=-1)
	{ 
	  c_start=c_start + c_name.length+1 
	  c_end=document.cookie.indexOf(";",c_start)
	  if (c_end==-1) c_end=document.cookie.length
	  document.getElementById("txtEnter").value =  unescape(document.cookie.substring(c_start,c_end))
	} 
    }
  return null
}

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="DIV1">
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
            <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="save" /></div>
    </form>
</body>

</html>
