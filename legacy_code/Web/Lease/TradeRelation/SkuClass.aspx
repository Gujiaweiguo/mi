<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SkuClass.aspx.cs" Inherits="Lease_TradeRelation_SkuClass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "RentableArea_SkuClassVindicate")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script type="text/javascript" src="../../JavaScript/TreeShow.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<SCRIPT LANGUAGE="Javascript"  src="../../JavaScript/ColorPicker2.js"></SCRIPT>
<SCRIPT LANGUAGE="JavaScript">
var cp = new ColorPicker('window'); 
var cp2 = new ColorPicker(); 
</SCRIPT>
	<script type="text/javascript">
	function Load()
	{
	    treearray();
	    addTabTool("<%=baseInfo %>,Lease/TradeRelation/SkuClass.aspx");
        loadTitle();
	}
	function CheckData()
    {
        if(isEmpty(document.all.txtTradePCode.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtTradePCode.focus();
            return false;					
        }
        if(isEmpty(document.all.txtTradePName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtTradePName.focus();
            return false;					
        }
    }
    function dw()
    {
//        document.all.txtTradePCode.value="";
        document.all.txtTradePCode.select();
//        document.all.txtTradePCode.focus();
    }

    function colordiv()
    {
    cp2.select(document.forms[0].txtbgcolor,'txtbgcolor'); 
    window.document.all("hidbgcolor").value =document.getElementById("txtbgcolor").value;
    }

    
	</script>
</head>
<body onload='Load();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField id="depttxt" runat="server"></asp:HiddenField>
            <asp:HiddenField id="deptid" runat="server" ></asp:HiddenField>
                <asp:HiddenField ID="selectdeptid" runat="server" />
                <asp:HiddenField ID="hidbgcolor" runat="server" />
                <div style="width:100%; text-align:center;">
        <table border="0" cellpadding="0" cellspacing="0" style="height: 430px" >
            <tr>
                <td style="width: 49%; height: 402px; vertical-align: top; text-align: right;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 330px; width: 100%;" >
                        <tr>
                            <td style="vertical-align:top; height: 22px;"  >
                            
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"     valign="top" style="height:27px;  text-align:left; width: 19px;" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  / >
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left; ">
                                <asp:Label ID="labBuildingTitle" runat="server" CssClass="lblTitle"  Text="<%$ Resources:BaseInfo,RentableArea_SkuClassTable %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                                </td>
                            
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 342px; text-align: center; width:100%;"
                                valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 380px">
                                
                                
                          <tr>
                                <td style="height:8px">
                        
                                </td>
                                </tr>
                                
                                
                                
                                
                                
                                <tr>
                                <td style="height:5px; text-align: center;">
                                </td>
                                </tr>
                                    <tr>
                                        <td colspan="4" rowspan="9" style="height: 207px; text-align: center; width: 185px;" valign="top">
                         <table border="0" cellpadding="0" cellspacing="0" style="width: 272px; height: 338px">
                         <tr>
                         <td style="height:10px">
                         
                         </td>
                         </tr>
                        <tr>
                        
                            <td colspan="2" style="vertical-align: top; height: 350px; background-color: #e1e0b2;
                                text-align: center; top: 20px; padding-left:40px;" width="100%">
                                <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                    Height="285px" ScrollBars="Auto" Width="100%">
                                    <table style="width: 216px; height: 263px">
                                        <tr>
                                            <td id="treeview" style="width: 100%; height: 240px; text-align: left; vertical-align: top;" valign="top">
                                            </td>
                                        </tr>
                                    </table>
                                    </asp:Panel>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-left:40px" align="center">
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #738495; position: relative; top: 13px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 160px; height: 1px; background-color: #ffffff; position: relative; top: 13px;">
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                        <td style="text-align:right; position: relative; top: 25px; height: 31px;">
                                         &nbsp;
                                        </td>
                                        </tr>
                                    </table>
                               
                               </td>
                        </tr>
                    </table>
     
                            </td>
                                  </tr>
                             
                                </table>
                                         <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" 
                                    Width="1px" OnClick="treeClick_Click" Height="1px" /></td>
                        </tr>
                   
                    </table>
                </td>
                <td style="height: 402px; width: 3%;">
                </td>
                <td style="height: 402px; width: 49%; vertical-align:top;">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 367px; width: 100%; ">
                        <tr>
                         <td style="vertical-align:top; height: 22px;" >
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 100%;   ">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,RentableArea_SkuClassVindicate %>"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 27px;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                        </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" rowspan="10" style="height: 380px; text-align: center; width: 400px;"
                                valign="top"><table border="0" cellpadding="0" cellspacing="0" style="height: 300px; width: 290px; ">
                                    <tr>
                                        <td class="tdBackColor" colspan="4" rowspan="10" style="height: 370px; text-align: center; width: 391px;"
                                valign="top">
                <table style="height: 370px">
                <tr>
                <td style="width: 100%; height: 335px; vertical-align: top;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 360px;">
                                    <tr>
                                        <td colspan="3" style="height:16px;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #738495">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblTradePCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_SkuClassCode %>"
                                                Width="91px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 155px; text-align: left;">
                                            <asp:TextBox ID="txtTradePCode" runat="server" CssClass="textstyle" ReadOnly="True" MaxLength="32"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblTradePName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_SkuClassName %>" Width="90px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 155px; text-align: left;">
                        <asp:TextBox ID="txtTradePName" runat="server" CssClass="textstyle" ReadOnly="True" MaxLength="32"></asp:TextBox></td>
                                    </tr>
                                    
                                 <tr>
                                        <td style="width: 61px; height: 30px; text-align: right">
                                            <asp:Label ID="lblTradePSatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_SkuClassStatus %>"
                                                Width="90px"></asp:Label></td>
                                        <td style="height: 30px; width: 13px;">
                                           </td>
                                        <td style="height: 30px; width: 155px; text-align: left;">
                                            <asp:DropDownList ID="cmbTradePStatus" runat="server" CssClass="textstyle" Enabled="False">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="height: 9px; text-align: left" colspan="3">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" style="height: 30px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 201px">
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 30px; text-align: right" colspan="3">
                                         <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                         Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" OnClick="btnAdd_Click" Enabled="False" />&nbsp;
                                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                Text="<%$ Resources:BaseInfo,User_btnChang %>" Enabled="False" />
                                            &nbsp;<asp:Button ID="btnSave" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" 
                                                        runat="server" CssClass="buttonSave" OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Enabled="False" />&nbsp;
                                            <asp:Button ID="btnCAncel" runat="server" CssClass="buttonCancel" OnClick="btnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                                            &nbsp; &nbsp;
                                        </td>
                                    </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="height: 30px">
                                                                                </td>
                                    </tr>
                                    
                                                                        <tr>
                                                                            <td colspan="3">
                                                                            </td>
                                    </tr>
                                                                        <tr>
                                                                            <td colspan="3" style="height: 15px">
                                                                            </td>
                                    </tr>
                                                                                                            <tr>
                                                                                                                <td colspan="3" style="height: 17px; text-align:right;">
                                                                                                                </td>
                                    </tr>
                                </table>
                </td>
                </tr>
                </table>
                                </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
           
          
                </td>
            </tr>
            <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        </table>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <SCRIPT LANGUAGE="JavaScript">cp.writeDiv()</SCRIPT>
</body>
</html>

