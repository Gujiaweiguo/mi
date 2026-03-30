<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MediaInfo.aspx.cs" Inherits="Sell_MediaInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
        <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />   
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"> </script>
	<script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript">
	function Load()
    {
        treearray();
        addTabTool("<%=strFresh %>,Sell/MediaInfo.aspx");
        loadTitle();
    }
    
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
    var imgurl;
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
                imgurl=treenode[3];
            }
          
            t.add(id, pid, name, "", imgurl, true);
            
        }
    }
            t.opt.sort='no';
            t.opt.enbScroll=true;
            t.opt.height="300px";
            t.opt.width="235px";
            t.opt.trg="mainFrame";
            t.opt.oneExp=true;
            t.opt.oneClick=true;
            
            t.render("treeview");
            
            t.treeOnClick = ev_click;   
            
            t.expandAll();
            if(document.form1.selectdeptid.value!="")
            {
                t.expandNode(document.form1.selectdeptid.value);
                t.selectNodeById(document.form1.selectdeptid.value);
            }
}
 function CheckIsNull()
{
     if(isInteger(document.all.txtMediaNo.value)==false)
    {
        alert("please input number.");   
        document.all.txtMediaNo.focus();         
        return false;
    }  
     
}
function CheckISNum()
{
     if(isInteger(document.all.txtMediaMNo.value)==false)
    {
        alert("please input number.");   
        document.all.txtMediaMNo.focus();         
        return false;
    }
}

function ev_click(e, id)
{
    document.form1.deptid.value=id;
    document.form1.Button1.click();
    

     
} 
function BtnUp( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
}
function BtnOver( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
}
    </script>
</head>
<body style="margin:0px" onload ="Load()">
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
              <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 4px">
                <tr>
                    <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 356px;
                        height: 22px; text-align: left">
                        <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_MediaInfo %>"
                            Width="295px"></asp:Label></td>
                    <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                    </td>
                    <td class="tdTopRightBackColor" style="vertical-align: top; height: 22px;
                        text-align: right; width: 115px;" valign="top">
                        <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                </tr>

                </table> 
                <table style="width: 100%" class="tdBackColor">

                    <tr>
                        <td colspan="1" rowspan="2" valign ="top" >
                        <asp:Panel ID="Panel3" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                        Font-Size="Medium" Height="300px" HorizontalAlign="Left"  Width="240px">
                                    <table style="height: 100%">
                                        <tr>
                                    <td valign="top" id ="treeview" style="height: 100%; width: 90%;">
                                             
                                         
                                    </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                            <table style="width: 236px">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="bt1Add" runat="server" CssClass="buttonAdd" EnableTheming="True" OnClick="btnDisplay_Click" Text="<%$ Resources:BaseInfo,MediaInfo_Display %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="bt2Add" runat="server" CssClass="buttonAdd" EnableTheming="True" OnClick="btnDisplayM_Click" Text="<%$ Resources:BaseInfo,MediaInfo_AddMember %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                        &nbsp; &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="left" style="height: 15%;width: 100%" valign ="top">
                          <asp:Panel ID="Panel2" runat="server" Height="90%" Width="90%">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 15%; height: 14px;">
                                    </td>
                                    <td style="width: 30%; height: 14px;" align ="center" >
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_MediaNo %>"></asp:Label></td>
                                    <td align ="left" style="height: 14px" >
                                        <asp:TextBox ID="txtMediaNo" runat="server" CssClass="ipt160px" Width="30%"></asp:TextBox>
                                        </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 15%; height: 14px">
                                    </td>
                                    <td align="right" colspan="2" style="height: 14px">
                                        <asp:Label ID="lblErr1" runat="server" Width="70%" Visible="False"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 15%; height: 13px;">
                                    </td>
                                    <td align="center" style="width: 30%; height: 13px;">
                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_MediaDesc %>"></asp:Label></td>
                                    <td align="left" style="height: 13px;">
                                        <asp:TextBox ID="txtMediaDesc" runat="server" CssClass="ipt160px" Width="50%"></asp:TextBox>
                                        <asp:CheckBox ID="ChkAuto" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_ChangAuto %>" CssClass="labelStyle"/></td>
                                </tr>
                                <tr>
                                    <td align="right" style="height: 5px" colspan="3">
                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" OnClick="btnAdd_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit"
                                Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>" OnClick="btnEdit_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="BtnBlankOut" runat="server" CssClass="buttonClear" 
                                Text="<%$ Resources:BaseInfo,Btn_Del %>" OnClick="BtnDel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnQuit_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30%">
                            <asp:Panel ID="Panel1" runat="server" Height="90%" Width="90%" Visible="False" valign ="top">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 10%; height: 30px">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label16" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_MediaMNo %>"></asp:Label></td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtMediaMNo" runat="server" CssClass="ipt160px" Width="50%"></asp:TextBox>
                                        </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; height: 14px">
                                    </td>
                                    <td align="right" colspan="3" style="height: 14px">
                                        <asp:Label ID="lblErr2" runat="server" Width="70%" Visible="False"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; height: 30px">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_MediaMDesc %>"></asp:Label></td>
                                    <td colspan="2" align ="left" >
                                        <asp:TextBox ID="txtMediaMDesc" runat="server" CssClass="ipt160px" Width="50%"></asp:TextBox>
                                        <asp:CheckBox ID="ChkAutoM" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_ChangAuto %>" CssClass="labelStyle"/></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; height: 30px;">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_MediaNo %>" ></asp:Label>&nbsp;</td>
                                    <td colspan="2" align ="left" >
                                        <asp:DropDownList ID="ddlMedia" runat="server" Width="70%" >
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; height: 17px;">
                                    </td>
                                    <td style="height: 17px;width: 20%" >
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_CMediaNo %>"></asp:Label></td>
                                    <td colspan="2" style="height: 17px;" align ="left" >
                                        <asp:DropDownList ID="ddlMediaC" runat="server" Width="70%" >
                                    </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_CMediaMNo %>"></asp:Label></td>
                                    <td colspan="2" align ="left" >
                                        <asp:DropDownList ID="ddlMediaMC" runat="server" Width="70%" >
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                   <td style="width: 20%">
                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_RMediaNo %>"></asp:Label></td>
                                    <td colspan="2" align ="left" >
                                        <asp:DropDownList ID="ddlMediaR" runat="server" Width="70%" >
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_RMediaMNo %>"></asp:Label></td>
                                    <td colspan="2" align ="left" >
                                        <asp:DropDownList ID="ddlMediaMR" runat="server" Width="70%" >
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_ComTenant%>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtComTenant" runat="server" CssClass="ipt160px" Width="100%"></asp:TextBox></td>
                                    <td colspan="1" style="width: 30%">
                                        <asp:RadioButton ID="rdoD" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_CardTypeD%>" CssClass="labelStyle" GroupName="a"/></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_ComMall%>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtComMall" runat="server" CssClass="ipt160px" Width="100%"></asp:TextBox></td>
                                    <td colspan="1" style="width: 30%">
                                        <asp:RadioButton ID="rdoF" runat="server" Text="<%$ Resources:BaseInfo,MediaInfo_CardTypeF%>" CssClass="labelStyle" GroupName="a"/></td>
                                </tr>
                                <tr>
                                    <td colspan="4" align ="right" style="width: 80%">
                                        <asp:Button ID="bt1Save" runat="server" CssClass="buttonSave" 
                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" OnClick="btnAddM_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="bt1Edit" runat="server" CssClass="buttonEdit"
                                Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>" OnClick="btnEditM_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="Bt1BlankOut" runat="server" CssClass="buttonClear" 
                                Text="<%$ Resources:BaseInfo,Btn_Del %>" OnClick="BtnDelM_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="bt1Cancel" runat="server" CssClass="buttonCancel" 
                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnQuitM_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            <asp:HiddenField ID="depttxt" runat="server" />
            <asp:HiddenField ID="selectdeptid" runat="server" />
            <asp:HiddenField ID="deptid" runat="server"  />
            <asp:Button ID="Button1" runat="server" CssClass="buttonHidden" OnClick="Button1_Click" Width="1px" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
