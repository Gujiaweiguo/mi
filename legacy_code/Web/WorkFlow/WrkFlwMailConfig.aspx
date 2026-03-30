<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WrkFlwMailConfig.aspx.cs" Inherits="WorkFlow_WrkFlwMailConfig" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "WrkFlw_MailConfigSet")%></title>
	<link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    
    <script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	
    <script type="text/javascript">
    function deptidnull()
    {
        parent.document.all.txtWroMessage.value =document.getElementById("hidSelect").value;
        treearray();
    }
    function usercodebeing()
    {
        parent.document.all.txtWroMessage.value =document.getElementById("hidUserCodeBeing").value;
        treearray();
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
                document.getElementById("lblTotalNum").style.display="none";
                document.getElementById("lblCurrent").style.display="none";
                
                addTabTool("null");
	        loadTitle();
    }



        function ev_click(e, id)
        {
            document.form1.deptid.value=id;
            document.form1.treeClick.click();  
            
        } 
        function showlineIns()
        {
            parent.document.all.txtWroMessage.value = document.getElementById("hidAdd").value;
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        function showlineError()
        {
            parent.document.all.txtWroMessage.value = document.getElementById("hidInsert").value;
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        	//text控件文本验证
    function EmailValidator(sForm)
    {
        if(isEmpty(document.all.txtTitle.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtTitle.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtContent.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtContent.focus();
            return false;					
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
    </script>
</head>
<body onload='treearray();'>
    <form id="form1" runat="server">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager><div align="center" style="width:80%; ">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    
                        <ContentTemplate>
                        <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
                        <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                        <asp:HiddenField ID="selectdeptid" runat="server" />
                                <table border="0" cellpadding="0" cellspacing="0" style=" text-align:center;width: 100%; ">
                                        <tr>
                                            <td class="tdTopBackColor" style="vertical-align: middle; height: 25px;
                                                text-align: left" valign="top">
                                                <img alt="" class="imageLeftBack" />
                                                <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,WrkFlw_EmailSetUp %>"></asp:Label></td>
                                            <td class="tdTopRightBackColor" colspan="2" style="height: 25px; text-align: right"
                                                valign="top">
                                                <img alt="" class="imageRightBack" /></td>
                                       </tr>
                                        <tr >
                                            <td class="tdBackColor" colspan="3" valign="top" style="vertical-align: middle; 
                                                text-align: left; height: 402px;" >
                                                <table cellpadding="0" cellspacing="0" >
                                                    <tr style=" vertical-align:top; height: 350px;">
                                                        <td style="width: 210px; vertical-align:top; height: 330px;">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 203px; height: 338px" valign="top">
                                                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                                                            Height="320px" ScrollBars="Auto" Width="200px">
                                                                            <table>
                                                                                <tr>
                                                                                    <td id="treeview" style="height: 300px; width: 170px;" valign="top">
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td style="width: 2%; height: 330px;">
                                                            </td>
                                                        <td style="width: 100%;  vertical-align:top; height: 330px;">
                                                            <table style="width:100%;">
                                                                <tr>
                                                                    <td >
                                                                        <asp:Label ID="lblBizGrpID" runat="server" CssClass="labelStyle" ForeColor="Black"
                                                                            Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpID %>" Width="56px"></asp:Label></td>
                                                                    <td >
                                                                        <asp:DropDownList ID="ddlBizGrpID" runat="server" BackColor="White" CssClass="cmb120px">
                                                                        </asp:DropDownList></td>
                                                                    <td>
                                                                        <asp:Label ID="lblStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_MailConfigStatus %>"
                                                                            Width="60px"></asp:Label></td>
                                                                    <td>
                                                                        
                                                                        <asp:DropDownList ID="ddlEmailState" runat="server" CssClass="ipt160px" Width="110px">
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td >
                                                                        <asp:Label ID="Label1" runat="server" CssClass="labelStyle" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_EmailTitle %>"
                                                                            Width="56px"></asp:Label></td>
                                                                    <td colspan="3">
                                                                        <asp:TextBox ID="txtTitle" runat="server" Width="97%"></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" ForeColor="Black" Text="<%$ Resources:BaseInfo,WrkFlw_EmailContent %>"
                                                                            Width="56px"></asp:Label></td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="4" style="height: 63px">
                                                                        <asp:TextBox ID="txtContent" runat="server" Height="242" TextMode="MultiLine" Width="97%"></asp:TextBox></td>
                                                                </tr>
                                                            </table>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr style="height:20px;">
                                                        <td  align="right" style="width: 210px; vertical-align:top;">
                                                            <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" EnableTheming="True" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                                 OnClick="btnAdd_Click" Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>"
                                                                />&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;</td>
                                                        <td ><asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" OnClick="treeClick_Click" Width="1px" />
                                                        </td>
                                                        <td align="right" style="vertical-align:top;">
                                                            <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>"  />&nbsp;&nbsp; &nbsp;
                                                            &nbsp;&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                <asp:HiddenField ID="hidBizCode" runat="server"  Value="<%$ Resources:BaseInfo,WrkFlw_lblBizCode %>"/>
                <asp:HiddenField ID="hidBizGrpName" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpName %>" />
                <asp:HiddenField ID="hidBizNote" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>" />
                <asp:HiddenField ID="hidBizGrpStatus" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>" />
                <asp:HiddenField ID="hidChang" runat="server" Value="<%$ Resources:BaseInfo,User_btnChang %>" />
                <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
                <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
                <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
                <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
                <asp:HiddenField ID="hidMessageError" runat="server" Value="<%$ Resources:BaseInfo,BizGrp_MessageError %>" />
        <asp:Label ID="lblCurrent" runat="server" ForeColor="Red" Height="9px" Width="0px">1</asp:Label><asp:Label
            ID="lblTotalNum" runat="server" Height="9px" Width="0px"></asp:Label></div>
    </form>
</body>
</html>
