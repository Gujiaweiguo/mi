<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ServerResource.aspx.cs" Inherits="PosSystem_ServerResource" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_PosServerVindicate")%></title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>	
	<script type="text/javascript" src="../JavaScript/TreeShow.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
<script type="text/javascript">
function tree()
{
	    addTabTool("<%= (String)GetGlobalResourceObject("BaseInfo", "Menu_PosServerVindicate")%>,PosSystem/ServerResource.aspx");
	    treearray();
	    loadTitle();    
}
    //text控件文本验证
    function check()
    {
        if(isEmpty(document.all.txtResourceCode.value))  
        {
            parent.document.all.txtWroMessage.value="数据源编码不能为空!";
            document.all.txtResourceCode.focus();
            return false;					
        }
        
        if(!isIP(document.all.txtIP.value))  
        {
            parent.document.all.txtWroMessage.value="IP地址格式错误!";
            document.all.txtIP.focus();
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

    <style type="text/css">
        .style1
        {
            height: 6px;
            width: 157px;
        }
        .style2
        {
            height: 26px;
            width: 157px;
        }
        .style3
        {
            height: 24px;
            width: 157px;
        }
    </style>

</head>
<body onload='tree();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="depttxt" runat="server" EnableViewState="False" />
                <asp:HiddenField ID="selectdeptid" runat="server" />
                <asp:HiddenField ID="deptid" runat="server"  />
                <table border="0" cellpadding="0" cellspacing="0" style="height: 430px; width:98%" >
                    <tr>
                        <td style="width: 35%; height: 401px; vertical-align: top;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 255px; vertical-align:top;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" style="width: 266px; height: 27px;">
                                        <img alt="" class="imageLeftBack" /><asp:Label ID="labAreaVindicate" runat="server" CssClass="lblTitle"
                                            Text="<%$ Resources:BaseInfo,Store_BusinessItemTree %>"></asp:Label></td>
                                    <td class="tdTopRightBackColor" valign="top" style="height: 27px">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="2" style="height: 1px" class="tdTopBackColor">
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td class="tdBackColor" colspan="2" rowspan="10" style="height: 340px; text-align: center"
                                        valign="top">
                                        
                                        <table style="height: 440px">
                                                  <tr>
                                        <td style="height: 275px; width: 258px;">
                                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                            Font-Size="Medium" Height="330px" HorizontalAlign="Left" ScrollBars="Auto" Width="240px">
                                            <table>
                                                <tr>
                                                    <td style="width: 203px; height: 225px" valign="top" id="treeview">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        </td>
                                        </tr>
                                            <tr>
                                                <td style="height: 20px; width: 258px;" valign="middle"><table border="0" cellpadding="0" cellspacing="0" style="width: 231px">
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
                                            <td style=" vertical-align:top; height: 42px; width: 258px; text-align:right;">
                                                      <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" Width="12px" 
                                                          onclick="treeClick_Click" />&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                            </tr>
                                        </table>
                        </td>
                                </tr>
                            </table>
                        </td>
                        <td style="height: 401px; width: 4%;">
                        </td>
                        <td style="width: 55%; height: 401px; vertical-align: top;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 288px; vertical-align:top;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" valign="top">
                                        <img alt="" class="imageLeftBack" /><asp:Label
                                                ID="labAreaTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Store_POSServerResourceVindicate %>"></asp:Label><a
                                                    style="font-size: 18px"></a></td>
                                    <td class="tdTopRightBackColor" valign="top">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="2" class="tdTopBackColor">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="2" rowspan="10" style="height: 365px; text-align: center"
                                        valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" 
                                            style="width: 5px; height: 440px">
                                            <tr>
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td style="width: 194px; height: 8px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style2">
                                                    <asp:Label ID="labResourceCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_ServerResourceCode %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 26px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:TextBox ID="txtResourceCode" runat="server" CssClass="ipt160px" 
                                                        MaxLength="8" Width="146px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right; " class="style1">
                                                    </td>
                                                <td style="height: 6px; width: 5px;">
                                                    </td>
                                                <td style="width: 194px; text-align: left; height: 6px;">
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="labResourceName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_ServerResourceName %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 24px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:TextBox ID="txtResourceName" runat="server" CssClass="ipt160px" 
                                                        MaxLength="16" Width="146px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style1">
                                                    </td>
                                                <td style="height: 6px; width: 5px;">
                                                    </td>
                                                <td style="width: 194px; height: 6px; text-align: left">
                                                    </td>
                                            </tr>
                                            
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblIP" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_ServerResourceIP %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 24px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:TextBox ID="txtIP" runat="server" CssClass="ipt160px" MaxLength="15" 
                                                        Width="146px"></asp:TextBox></td>
                                            </tr>                                            
                                            
                                                                                        <tr>
                                                <td style="text-align: right" class="style1">
                                                    </td>
                                                <td style="height: 6px; width: 5px;">
                                                    </td>
                                                <td style="width: 194px; height: 6px; text-align: left">
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblPort" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_Port %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 24px; width: 5px;">
                                                   </td>
                                                <td style="width: 194px; text-align: left" rowspan="1">
                                                    <asp:TextBox ID="txtPort" runat="server" CssClass="ipt160px" MaxLength="4"
                                                        Width="146px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblDBName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_DBName %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    <asp:TextBox ID="txtDBName" runat="server" CssClass="ipt160px" MaxLength="16" 
                                                        Width="146px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblLoginNm" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserLoginCode %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    <asp:TextBox ID="txtLoginNm" runat="server" CssClass="ipt160px" MaxLength="8" 
                                                        Width="146px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblPwd" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblPassword %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    <asp:TextBox ID="txtPwd" runat="server" CssClass="ipt160px" MaxLength="32" 
                                                        Width="146px"></asp:TextBox></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserStatusStr %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    <asp:DropDownList ID="cmbStatus" runat="server" CssClass="cmb160px" 
                                                        Width="153px">
                                                    </asp:DropDownList></td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaSize_lblNote %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="64" 
                                                        Width="146px"></asp:TextBox></td>
                                            </tr>
                                                    <tr>
                                                        <td colspan="3" style="height: 22px; text-align: center">
                                                            &nbsp;<table border="0" cellpadding="0" cellspacing="0" style="width: 100%" id="TABLE1" onclick="return TABLE1_onclick()">
                                                                <tr>
                                                                    <td style="width: 100%; height: 1px; background-color: #738495; position: relative; top: 15px;">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%; height: 1px; background-color: #ffffff; position: relative;
                                                                        top: 15px;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                            </tr>
                                              <tr>
                                                  <td colspan="3" style="height: 10px; text-align: left">
                                                  </td>
                                            </tr>                      
                                              <tr>
                                                  <td colspan="3" style="height: 30px; text-align: right; position: relative; top: 15px;">
                                                      &nbsp;<asp:Button ID="btnSave"
                                                                  runat="server" CssClass="buttonSave" 
                                                          Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Enabled="False" 
                                                          onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                          onmouseup="BtnUp(this.id);" onclick="btnSave_Click"/>&nbsp;<asp:Button
                                                                      ID="btnCancel" runat="server" CssClass="buttonCancel"
                                                                      onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                                                      Text="<%$ Resources:BaseInfo,User_btnCancel %>" 
                                                          onclick="btnCancel_Click" />
                                                  </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="height: 30px; text-align: right">
                                                   </td>
                                            </tr>
                                        </table>
                                      </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidAreaNotSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_AreaNotSelect %>" />
        <asp:HiddenField ID="hidAddArea" runat="server" Value="<%$ Resources:BaseInfo,Dept_TitleAdd %>" />
        <asp:HiddenField ID="hidlblUnitit" runat="server" Value="<%$ Resources:BaseInfo,Menu_PosServerVindicate %>" />
    </form>
</body>
</html>