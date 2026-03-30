<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManualReceipt.aspx.cs" Inherits="Sell_ManualReceipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
        <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript">
	function Load()
    {
        addTabTool("<%=strFresh %>,Sell/ManualReceipt.aspx");
        loadTitle();
    }
 	     //验证数字类型
        function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110)
            {
		        window.event.returnValue =true;
	        }else
	        {
		        window.event.returnValue =false;
	        }
	    }    
     function ShowTree()
        {
        	strreturnval=window.showModalDialog('../Lease/Shop/SelectShop.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
			     var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');
                objImgBtn1.click();
            }
			else
			{
				return;	
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
function CheckData() {
    if (isEmpty(document.all.txtTransId.value)) {
        parent.document.all.txtWroMessage.value = '<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
        document.all.txtTransId.focus();
        return false;
    }
    if (isEmpty(document.all.txtPosId.value)) {
        parent.document.all.txtWroMessage.value = '<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
        document.all.txtPosId.focus();
        return false;
    }
    if (isEmpty(document.all.txtBizDate.value)) {
        parent.document.all.txtWroMessage.value = '<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
        document.all.txtBizDate.focus();
        return false;
    }
    if (isEmpty(document.all.txtSalesAmt.value)) {
        parent.document.all.txtWroMessage.value = '<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
        document.all.txtSalesAmt.focus();
        return false;
    }
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
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_ManualReceipt %>"
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
                        <td align="left" colspan="2" style="height: 17px">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 10%">
                                    </td>
                                    <td align="left" style="width: 20%">
                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,Rpt_TransId %>"></asp:Label></td>
                                    <td align="left">
                                        <asp:TextBox ID="txtTransId" runat="server" CssClass="ipt160px" Width="37%" 
                                            MaxLength="4"></asp:TextBox>&nbsp;&nbsp;
                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                            Text="<%$ Resources:BaseInfo,User_lblQuery %>" 
                                            onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                            onmouseup="BtnUp(this.id);" Visible="False"/></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 10%">
                                    </td>
                                    <td style="width: 20%" align ="left" >
                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
                                    <td align ="left" >
                                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" Width="37%"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 10%">
                                    </td>
                                    <td style="width: 20%" align ="left" >
                                        <asp:Label ID="Label1" runat="server"  Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                                    <td align ="left">
                                        <asp:TextBox ID="txtShopName" runat="server" CssClass="ipt160px" Width="37%"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 17px">
                            <table border="0" cellpadding="0" cellspacing="0" style="width:98%; height: 1px;
                                text-align: center">
                                <tr>
                                    <td style="left: 15px; width: 324px; position: relative; height: 1px; background-color: #738495">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 17px">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 10%; height: 30px;">
                                    </td>
                                    <td style="width: 20%; height: 30px;">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblDate %>" ></asp:Label></td>
                                    <td style="height: 30px">
                                        <asp:TextBox ID="txtBizDate" runat="server" Width="37%" CssClass="ipt160px" onclick="calendar()"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Rpt_POSId %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtPosId" runat="server" CssClass="ipt160px" Width="37%" 
                                            MaxLength="4"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,Rpt_BatchId %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtBatchId" runat="server" CssClass="ipt160px" Width="37%"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%; height: 24px;">
                                    </td>
                                    <td style="width: 20%; height: 24px;">
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,Rpt_SkuId %>"></asp:Label></td>
                                    <td style="height: 24px"><asp:DropDownList ID="ddlSkuId" runat="server" Width="38%">
                                    </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label9" runat="server" Text="<%$ Resources:BaseInfo,Rpt_MediaMDesc %>"></asp:Label></td>
                                    <td><asp:DropDownList ID="ddlMediaID" runat="server" Width="38%" Enabled="False">
                                    </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%">
                                    </td>
                                    <td style="width: 20%">
                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:BaseInfo,Rpt_SalesAmt %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtSalesAmt" runat="server" CssClass="ipt160px" Width="37%"></asp:TextBox></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 30%">
                        </td>
                        <td style="height: 17px" align="left">
                            &nbsp;&nbsp; &nbsp;&nbsp; &nbsp; &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" OnClick="btnAdd_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                            &nbsp;&nbsp;<asp:Button ID="BtnBlankOut" runat="server" CssClass="buttonClear" 
                                Text="<%$ Resources:BaseInfo,Btn_Del %>" OnClick="BtnDel_Click" 
                                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                onmouseup="BtnUp(this.id);" Visible="False"/>&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnQuit_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                            <br />
                            &nbsp;
                        </td>
                    </tr>
                </table>
                 <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
             <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
