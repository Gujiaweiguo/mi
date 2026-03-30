<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnitAttrOperate.aspx.cs" Inherits="VisualAnalysis_UnitAttrOperate" EnableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseinfo %></title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    
    <script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    
    <script src="../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
	<script type="text/javascript"  src="../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	
   <script type="text/javascript" language="javascript">
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
        t.opt.height="360px";
        t.opt.width="195px";
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
    }
    function ev_click(e, id)
    {
        document.form1.deptid.value=id;
        document.form1.selectdeptid.value=id;
        document.form1.treeClick.click();
        parent.document.all.txtWroMessage.value = ""; 
    }
    function ShowTitle()
    {
        parent.document.all.txtWroMessage.value = "";
        addTabTool("<%=baseinfo %>,VisualAnalysis/UnitAttrOperate.aspx");
        loadTitle();
    }
    function NumberTest()
    {
        if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) 
            || (window.event.keyCode == 13) &&(window.event.keyCode == 46)
            || (window.event.keyCode == 45)))
            {
                window.event.keyCode = 0 ;
            }
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
    function CheckConfirm()
    {
        if(confirm('图片已经存在，是否覆盖')==false)
        {
            return;
            //document.form1.btnSaveFile.click();
        }
    }
   </script>
</head>
<body onload='treearray();ShowTitle();' topmargin=0 leftmargin=0>
     <form id="form1" runat="server">
    <div align="center" style="width:100%; ">
    <asp:ScriptManager id="ScriptManager1" runat="server" ></asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
        <table border="0" cellpadding="0" cellspacing="0" style="height: 450px; width:100%; vertical-align:top" >
            <tr>                
                 <td style="width:33%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style=" height: 450px;vertical-align: top;">
                        
                        <tr>
                          <td style="vertical-align:top; height: 22px; width: 100%;" >          
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;">              
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,ShopXml_UnitAttrVindicate%>'></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>                       
                                </table>               
                        </td>
                        </tr>
                        <tr>  
                             <td colspan="2" style="height: 300px; background-color: #e1e0b2; vertical-align:top; text-align:center;">
                                <table>   
                                    <tr>
                                        <td style="width:5px;" valign="top">   </td>
                                        <td style="height: 290px" valign="top">
                                          <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px" Height="370px" ScrollBars="Auto" Width="220px" BackColor="White">
                                              <table>
                                                    <tr>
                                                        <td valign="top" id ="treeview" style="height: 250px; width: 160px; text-align:left;">                                    
                                                        </td>
                                                    </tr>
                                                </table>                
                                            </asp:Panel>
                                        </td>
                                        <td style="width:5px;" valign="top"> </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 200px;" >
                                    <tr >
                                        <td style="width: 180px; height: 1px; background-color: #738495; position: relative; top: 10px;" align="center">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px; height: 1px; background-color: #ffffff; position: relative;top: 10px;" align="center">
                                        </td>
                                    </tr>
                                </table>
                                <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden"  Width="1px" OnClick="treeClick_Click" />
                            </td>
                        </tr>
                    </table>
                </td>           
                <td style="width: 1%;">
                </td>
                <td style="height:400px; width: 66%; vertical-align:top;" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 450px; width:98%; vertical-align:top; " >
                        
                        <tr>
                            <td colspan="8" style="height: 22px; background-color: #e1e0b2" valign="top">
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%; ">
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        </td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            </td>
                                    </tr>
                                </table>          
                            </td>
                        </tr>
                        <tr>
                            <td style=" width:200px;  background-color: #e1e0b2; height: 12px; text-align: right;" align="right">                           
                                <asp:Label ID="lblFloorName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ShopXml_X %>"
                                    Width="57px"></asp:Label>&nbsp;</td>
                            <td style="width: 145px; height: 12px; background-color: #e1e0b2; text-align: left">
                                <asp:TextBox ID="txtX"  runat="server"  CssClass="ipt160px" Width="120px" MaxLength="64" OnKeyPress="NumberTest()"></asp:TextBox></td>
                            <td style="height: 12px; background-color: #e1e0b2; text-align: right">
                                <asp:Label ID="lblFloorStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ShopXml_Y %>"
                                    Width="57px"></asp:Label>&nbsp;</td>
                            <td style="width:145px;  background-color: #e1e0b2; text-align: left; height: 12px;"> 
                                <asp:TextBox ID="txtY" runat="server" CssClass="ipt160px" Width="120px" OnKeyPress="NumberTest()"></asp:TextBox></td>
                            <td colspan="2" rowspan="5" style="background-color: #e1e0b2; text-align: left; width: 140px;" valign="top" id="td1">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="width: 200px; height: 21px; background-color: #e1e0b2; text-align: right">
                                <asp:Label ID="lblFloorCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ShopXml_ShopMap %>"
                                    Width="59px"></asp:Label>&nbsp;</td>
                            <td colspan="3" style="height: 21px; background-color: #e1e0b2; text-align: left">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                <asp:Button ID="btnUp" runat="server" OnClick="btnUp_Click" Text="上传" /></td>
                        </tr>
                        <tr >
                            <td align="right" style="vertical-align:middle width: 197px; background-color: #e1e0b2; width: 200px; height: 36px; text-align: right;">
                                &nbsp;</td>
                            <td style="height: 36px; background-color: #e1e0b2; text-align: left">
                                </td>
                            <td style="width: 200px; height: 36px; background-color: #e1e0b2; text-align: right">
                                </td>
                            <td style="vertical-align:middle width: 145px; background-color: #e1e0b2; text-align: left; height: 36px;">
                                </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="vertical-align: middle; height: 12px; background-color: #e1e0b2;
                                text-align: center">
                                &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave%>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Enabled="False"/>
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:LinkButton ID="btnSaveFile" runat="server" Width="0px" OnClick="btnSaveFile_Click"></asp:LinkButton>
                                <asp:Label ID="lblmap" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                             <td align="center" colspan="1" style="vertical-align: top; width: 94px; height: 206px;
                                 background-color: #e1e0b2">
                                </td>
                            <td style=" background-color: #e1e0b2; height: 206px; vertical-align: top;" align="center" colspan="2">                            &nbsp; &nbsp; &nbsp; &nbsp;
                                &nbsp; &nbsp; &nbsp;<asp:Image ID="Image1" runat="server" Height="100%" Width="80%" Visible="False" />
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                &nbsp; &nbsp; &nbsp;&nbsp;
                            </td>
                             <td align="center" colspan="1" style="vertical-align: top; height: 206px; background-color: #e1e0b2">
                             </td>
                        </tr>
                    </table>
                    </td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
         <asp:Label ID="lblCurrent" Visible="false" runat="server" ForeColor="Red" Height="9px">1</asp:Label>
         <asp:Label ID="lblTotalNum" runat="server" Height="9px"></asp:Label>
         <asp:LinkButton ID="btn" runat="server" OnClick="btn_Click"></asp:LinkButton>
    </form>
</body>
</html>
