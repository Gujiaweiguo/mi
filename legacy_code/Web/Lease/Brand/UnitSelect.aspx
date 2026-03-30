<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnitSelect.aspx.cs" Inherits="Lease_TradeRelation_TradeRelationSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server" >

    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_IntentUnits")%></title>
    
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
        <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/main/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    var tabbar ;
function treearray()
{
     var t = new NlsTree('MyTree');
    var treestr =document.getElementById("depttxt").value;
     var nodeid="";
     var nodeValue = "";//名称
    var treearr = new Array();
    var n=0;
    var id;
    var pid;
    var name;
    var chkstrtus;
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
                        chkstrtus=treenode[3];
                    }
                    if(chkstrtus==undefined)
                    {
                        t.add(id, pid, name, "", "", true,false);
                    }
                    if(chkstrtus==0)
                    {
                        t.add(id, pid, name, "", "", true,false);
                    }
                    else if(chkstrtus==1)
                    {
                        t.add(id, pid, name, "", "", true,true);
                        nodeid+=id+ ',';
                    }
                }
            }
            
            document.form1.deptid.value = nodeid;
           
            t.opt.sort='no';
            t.opt.enbScroll=true;
            t.opt.height="280px";
            t.opt.width="170px";
            t.opt.trg="mainFrame";
            t.opt.oneExp=true;
            t.opt.check = true;
            t.opt.checkIncSub = true; //default is true.
            t.opt.checkParents = true; //default is false.
            t.opt.checkOnLeaf = true; //default is false
            t.treeOnCheck = enableCheckbox;
            
            t.render("treeview");  
            t.collapseAll();
	    
}
function enableCheckbox(id,v)
{
    var str;
    str=document.form1.deptid.value;
    if(str.indexOf(id+',',0)!=-1)
    {
        document.form1.deptid.value =str.replace(id+',',"");
    }
    else
    {
        str+=id+',';
        document.form1.deptid.value =str;
    }
} 
function ReturnValue()
{
 var a = document.form1.deptid.value+"|"+document.form1.txtUnitCode.value+"|"+document.form1.txtStore.value;
    window.returnValue = a;
    window.close();
}
     function BtnUp( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
    }
    function BtnOver( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
    }
    </script>
</head>
<body onload='treearray();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <table border="0" cellpadding="0" cellspacing="0" style="height: 32px; width: 230px;">
            <tr>
                <td class="tdTopRightBackColor" style="height: 27px; text-align: left; vertical-align: top;" valign="top">
                    <img alt="" class="imageLeftBack" style="text-align: left" />
                    <asp:Label ID="Label1" runat="server" Height="12pt" Text="<%$ Resources:BaseInfo,PotCustomer_IntentUnits %>"></asp:Label></td>
                <td class="tdTopRightBackColor" style="height: 27px; text-align: left">
                    </td>
                <td class="tdTopRightBackColor" style="height: 27px; text-align: right" valign="top">
                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                </td>
            </tr>
            <tr class="headLine">
                <td colspan="3">
                </td>
            </tr>
            <tr style="height: 1px">
                <td class="tdBackColor" colspan="3">
                    <table style="width: 231px">
                        <tr style="height: 10px">
                        </tr>
                        <tr style="height: 10px">
                            <td style="height: 2px">
                                <table border="0" cellpadding="0" cellspacing="0" style="left: 0px; width: 100%;
                                    position: relative; top: 0px">
                                    <tbody>
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #738495">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #ffffff">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                </td>
                        </tr>
                        <tr>
                            <td align="right" rowspan="2" 
                                style="text-align: center; vertical-align: top; height: 300px;">
        <table>
            <tr>
                <td style="height: 290px; text-align: left;">
                    <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                        Height="290px" ScrollBars="Auto" Width="180px">
                        <table>
                            <tr>
                                <td id="treeview" style="height: 280px; width: 159px;" valign="top">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
                                &nbsp;
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                    Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                        </tr>
                        <tr class="rowHeight">
                        </tr>
                    </table>
                    </td>
            </tr>
        </table>
        <asp:HiddenField ID="depttxt" runat="server" />
        <asp:HiddenField ID="deptid" runat="server" />
        <asp:HiddenField ID="deptValue" runat="server" />
        <asp:HiddenField ID="selectdeptid" runat="server" />
                    <asp:TextBox ID="txtUnitID" runat="server" Width="0px" CssClass="hidden"></asp:TextBox><asp:TextBox ID="txtUnitCode" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="txtStore" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
