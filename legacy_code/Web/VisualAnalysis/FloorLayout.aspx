<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FloorLayout.aspx.cs" Inherits="VisualAnalysis_FloorLayout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script type="text/jscript">
    var tabbar ;
function treearray()
{
    var t = new NlsTree('MyTree');
    var treestr =document.getElementById("depttxt").value;
    var treearr = new Array();
    var n=0;
    var id;
    var pid;
    var name;
    var num = treestr.split("^");
    for(var i=0;i<num.length-1;i++)
    {
        if(num[i]!="")
        {
       
           var treenode = num[i].split("|");
            for(var j=0;j<treenode.length;j++)
            {
                id=treenode[0];
                pid=treenode[1];
                name=treenode[2];
            }
            t.add(id, pid, name, "", "", true);
        }
    }
    t.opt.sort='no';
    t.opt.enbScroll=true;
    t.opt.height="330px";
    t.opt.width="170px";
    t.opt.trg="mainFrame";
    t.opt.oneExp=true;
    t.opt.oneClick=true;
    
    t.render("treeview");
    
    t.treeOnClick = ev_click;   
    t.collapseAll();
    if(document.form1.selectdeptid.value!="")
    {
        t.expandNode(document.form1.selectdeptid.value);
        t.selectNodeById(document.form1.selectdeptid.value);
    }
    addTabTool("null");
    loadTitle();
}
function ev_click(e, id)
{
    document.form1.deptid.value=id;
    document.form1.selectdeptid.value=id;
    document.form1.Button1.click();
} 
    </script>
</head>
<body onload='treearray();' style=" margin:0px">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 959px; height: 488px;background-color: #e1e0b2; ">
                    <tr>
                        <td style="width: 138px">
                        </td>
                        <td rowspan="5" style="width: 245px">
                            <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px" Height="340px" ScrollBars="Auto" Width="200px" BackColor="White">
                              <table>
                                    <tr>
                                        <td valign="top" id ="treeview" style="height: 250px; width: 160px; text-align:left;">                                    
                                        </td>
                                    </tr>
                                </table>                
                            </asp:Panel>    
                        </td>
                        <td colspan="2" rowspan="7">
                                    </td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                            <asp:TextBox ID="TextBox1" runat="server" Visible="False"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                        </td>
                        <td style="width: 245px">
                            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Width="0px" />
                            </td>
                    </tr>
                    <tr>
                        <td style="width: 138px">
                        </td>
                        <td style="width: 245px">
                        </td>
                    </tr>
                </table>
                <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
