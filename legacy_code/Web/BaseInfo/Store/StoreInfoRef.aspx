<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreInfoRef.aspx.cs" Inherits="Store_Store" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseinfo%></title>
   <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript">
 function Load()
{
    addTabTool("<%=PotCustomer_Basic %>,BaseInfo/Store/StoreInfoRef.aspx?look=<%=isbro%>~<%=Store_CardInfo %>,BaseInfo/Store/StoreLicense.aspx?look=<%=isbro%>~<%=Store_ItemBackdrop %>,BaseInfo/Store/StoreHistory.aspx?look=<%=isbro%>~<%=Store_FloorThing%>,BaseInfo/Store/FloorInfo.aspx?look=<%=isbro%>");
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
            t.opt.height="260px";
            t.opt.width="235px";
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
            /*document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
            */
            addTabTool("null");
	        loadTitle();
}



function ev_click(e, id)
{
	
    document.form1.deptid.value=id;
    document.form1.treeClick.click();
     
} 
function CheckIsNull()
{
        if(isEmpty(document.all.txtStoreCode.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "Store_ItemCode")%>'+document.getElementById("hidMessage").value;
            document.all.txtStoreCode.focus();
            return false;					
        }
        if(isEmpty(document.all.txtStoreName.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "Store_StoreName")%>'+document.getElementById("hidMessage").value;
            document.all.txtStoreName.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtStoreShortName.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "Store_StoreShortName")%>'+document.getElementById("hidMessage").value;
            document.all.txtStoreShortName.focus();
            return false;					
        }
        if(isInteger(document.all.txtGroundParking.value)==false)
        {
            alert("please input number.");
            document.all.txtGroundParking.focus();
            return false;
        }
        if(isInteger(document.all.txtUnderParking.value)==false)
        {
            alert("please input number.");   
            document.all.txtUnderParking.focus();         
            return false;
        }        
         if(isPostCode(document.all.txtOfficeZip.value)==false)
        {
            alert("邮政编码格式错误.");   
            document.all.txtOfficeZip.focus();         
            return false;
        }      
         if(isDigit(document.all.txtOfficeTel.value)==false)
        {
            alert("please input number.");   
            document.all.txtOfficeTel.focus();         
            return false;
        }      
         if(isDigit(document.all.txtOfficeTel2.value)==false)
        {
            alert("please input number.");   
            document.all.txtOfficeTel2.focus();         
            return false;
        } 
         if(isDigit(document.all.txtPropertyTel.value)==false)
        {
            alert("please input number.");   
            document.all.txtPropertyTel.focus();         
            return false;
        } 
}
var xmlHttp;
function createXMLHttpRequest()
{
    if(window.ActiveXObject)
    {
        xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    else
    {
        xmlHttp = new XMLHttpRequest();
    }
}
function AJAXCheckStoreCode()
{
    createXMLHttpRequest();
    var getCode = document.getElementById("txtStoreCode").value;
    var url = "checkStoreCodeHandler.ashx?code="+getCode;
   xmlHttp.open("POST",url,true);
   xmlHttp.onreadystatechange=resultCheckCode;
   xmlHttp.send(null);
}
function resultCheckCode()
{
    if(xmlHttp.readystate==4)
    {
        if(xmlHttp.status==200)
        {
            var txtGetResponse=xmlHttp.responseText;
            if(txtGetResponse=="0")
            {
                parent.document.all.txtWroMessage.value = "not register。";
            }
            else
            {
                parent.document.all.txtWroMessage.value = "have register。";
                document.getElementById("txtStoreCode").value = "";
                document.getElementById("txtStoreCode").focus();
            }
        }
    }
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
<body  onload='treearray();Load();' topmargin=0 leftmargin=0>
     <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server"></asp:ScriptManager>
    <div align="right"  style="width:100%; vertical-align:top; ">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            
        <table border="0" cellpadding="0" cellspacing="0" style="height: 400px; width:100%; vertical-align:top">
            <tr>                
                 <td style="width:33%; height: 400px; " valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style=" height: 400px;">
                        
                        <tr>
                          <td style="vertical-align:top; height: 22px; width: 100%;" >          
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">              
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,Store_BusinessItemBrowse %>'></asp:Label></td>
                                      
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
                                          <asp:Panel ID="Panel1" runat="server" BorderStyle="Inset" BorderWidth="1px" Height="280px" ScrollBars="Auto" Width="260px" BackColor="White">
                                              <table>
                                                    <tr>
                                                        <td valign="top" id ="treeview" style="height: 259px; width: 207px; text-align:left;">                                    
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
                <td style="width: 0.5%; height: 400px">
                </td>
                <td style=" height: 400px;" valign="top" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 400px; width:97%; " >
                        
                        <tr>
                              <td  style=" height: 22px; width: 100%; background-color: #e1e0b2;" colspan="5" valign="top">     
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%; ">
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label
                                            ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Store_BusinessItemBasicInfoMaintenance %>"></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>
                                </table>          
                            </td>
                        </tr>
                        <tr>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right">                            
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle"　Text="<%$ Resources:BaseInfo,Store_ItemCode %>" ></asp:Label>&nbsp;</td>
                            <td style="height: 23px; background-color: #e1e0b2">                            
                                <asp:TextBox ID="txtStoreCode" runat="server" Width="140px" CssClass="ipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td align="left" style="height: 23px; background-color: #e1e0b2">
                                </td>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_UnderParking %>"></asp:Label>&nbsp;</td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">                                                        
                                <asp:TextBox ID="txtUnderParking" runat="server" Width="140px" 
                                    CssClass="ipt160px" MaxLength="8"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right">                            
                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreName %>"></asp:Label>&nbsp;</td>
                            <td style="height: 23px; background-color: #e1e0b2">                                                         
                                <asp:TextBox ID="txtStoreName" runat="server" Width="140px" CssClass="ipt160px" ReadOnly="True"></asp:TextBox><span></span></td>
                             <td style=" height: 23px; background-color: #e1e0b2">
                                 <img id="ImgCustName" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right">
                                <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblOfficeAddr %>"></asp:Label>&nbsp;</td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2"> 
                                <asp:TextBox ID="txtOfficeAddr" runat="server" Width="140px" 
                                    CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right">                            
                                <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreShortName %>"></asp:Label>&nbsp;</td>
                            <td style="height: 23px; background-color: #e1e0b2"> 
                                <asp:TextBox ID="txtStoreShortName" runat="server" Width="140px" 
                                    CssClass="ipt160px" MaxLength="8"></asp:TextBox></td>
                             <td style="height: 23px; background-color: #e1e0b2">
                                 <img id="ImgCustShortName" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> &nbsp;</td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2"> 
                                <asp:TextBox ID="txtOfficeAddr2" runat="server" CssClass="ipt160px" 
                                    Width="140px" MaxLength="32"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreType %>"></asp:Label>&nbsp;</td>
                            <td style="height: 23px; background-color: #e1e0b2"> 
                                <asp:DropDownList ID="txtStoreType" CssClass="ipt160px" runat="server" Width="140px">
                                </asp:DropDownList></td>
                             <td style=" height: 23px; background-color: #e1e0b2">
                                 </td>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> &nbsp;</td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtOfficeAddr3" runat="server" CssClass="ipt160px" 
                                    Width="140px" MaxLength="32"></asp:TextBox></td>
                        </tr>
                         <tr>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreAddr %>"></asp:Label>&nbsp;</td>
                            <td style="height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtStoreAddr" runat="server" Width="140px" CssClass="ipt160px" 
                                    MaxLength="32"></asp:TextBox></td>
                             <td style=" height: 23px; background-color: #e1e0b2">
                                 </td>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label6" runat="server"  CssClass="labelStyle"　Text="<%$ Resources:BaseInfo,Dept_lblPostCode %>"></asp:Label>&nbsp;</td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2"> 
                                <asp:TextBox ID="txtOfficeZip" runat="server" Width="140px" CssClass="ipt160px" 
                                    MaxLength="6"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreAmbit %>"></asp:Label>&nbsp;</td>
                            <td style="height: 23px; background-color: #e1e0b2" >                            
                                <asp:TextBox ID="txtStoreAmbit" runat="server" Width="140px" 
                                    CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                            <td style=" height: 23px; background-color: #e1e0b2">
                                </td>
                             <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeTel %>"></asp:Label>&nbsp;</td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2" >                            
                                <asp:TextBox ID="txtOfficeTel" runat="server" Width="140px" CssClass="ipt160px" 
                                    MaxLength="20"></asp:TextBox></td>
                                   
                        </tr>
                        <tr>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreManageType %>"></asp:Label>&nbsp;</td>
                            <td style="height: 23px; background-color: #e1e0b2"><asp:DropDownList ID="ddlStoreManageType" CssClass="ipt160px" runat="server" Width="140px">
                            </asp:DropDownList></td>
                            <td style=" height: 23px; background-color: #e1e0b2">
                                </td>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MiInfoVindicate_labAttractTel %>"></asp:Label>&nbsp;</td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2" >                            
                                <asp:TextBox ID="txtOfficeTel2" runat="server" Width="140px" 
                                    CssClass="ipt160px" MaxLength="20"></asp:TextBox></td>
                                 
                        </tr>
                        <tr>
                            <td align="right" style="height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_GroundParking %>"></asp:Label>&nbsp;</td>
                            <td style="height: 23px; background-color: #e1e0b2">
                               <asp:TextBox ID="txtGroundParking" runat="server" Width="140px" 
                                    CssClass="ipt160px" MaxLength="8"></asp:TextBox></td>
                            <td style="height: 23px; background-color: #e1e0b2">
                            </td>
                            <td align="right" style="height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MiInfoVindicate_labContentTel %>"></asp:Label>&nbsp;</td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:TextBox ID="txtPropertyTel" runat="server" Width="140px" 
                                    CssClass="ipt160px" MaxLength="20"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style=" height: 22px; background-color: #e1e0b2;" align="right"> 
                                <asp:Label ID="Label15" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,sort %>"></asp:Label>&nbsp;
                            </td>
                            <td style="height: 10px; background-color: #e1e0b2; " >
                                <asp:TextBox ID="txtOrder" runat="server" CssClass="ipt160px" MaxLength="8" 
                                    Width="140px"></asp:TextBox>
                                </td>
                            <td style="height: 23px; background-color: #e1e0b2">
                            </td>
                            <td align="right" style="height: 22px; background-color: #e1e0b2">
                                <asp:Label ID="lblStatus" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,Dept_lblStatus %>" Width="60px"></asp:Label>
                            </td>
                            <td style="width: 114px; height: 23px; background-color: #e1e0b2">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="ipt160px" 
                                    Width="140px">
                                </asp:DropDownList>
                            </td>    
                        </tr>
                        <tr>
                            <td colspan="5" style="width: 98%; height: 5px; background-color: #e1e0b2; text-align: center" valign="middle">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 98%;text-align: center;">
                                    <tr>
                                        <td style="width: 98%; height: 1px; background-color: #738495;position: relative;top: 16px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 98%; height: 1px; background-color: #ffffff;position: relative;top: 16px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="5" style="height: 62px; background-color: #e1e0b2" valign="middle">
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnCancel"  runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div><asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
    </form>
</body>
</html>