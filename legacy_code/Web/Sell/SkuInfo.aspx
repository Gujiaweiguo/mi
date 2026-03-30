<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SkuInfo.aspx.cs" Inherits="Sell_SkuInfo" %>

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
        addTabTool("<%=strFresh %>,Sell/SkuInfo.aspx");
        loadTitle();
    }
    function check()
    {
        if(isEmpty(document.all.txtQuery.value))  
        {
            parent.document.all.txtWroMessage.value="请选择商铺。";
            document.all.txtQuery.focus();
            return false;					
        }
        if(!isDigitDot(document.all.txtPrice.value))  
        {
            parent.document.all.txtWroMessage.value="请输入数字格式。";
            document.all.txtPrice.focus();
            return false;					
        }
        if(!isDigitDot(document.all.txtPriceMin.value))  
        {
            parent.document.all.txtWroMessage.value="请输入数字格式。";
            document.all.txtPriceMin.focus();
            return false;					
        }
        if(!isDigitDot(document.all.txtStock.value))  
        {
            parent.document.all.txtWroMessage.value="请输入数字格式。";
            document.all.txtStock.focus();
            return false;					
        }
        if(!isDigitDot(document.all.txtPriceMax.value))  
        {
            parent.document.all.txtWroMessage.value="请输入数字格式。";
            document.all.txtPriceMax.focus();
            return false;					
        }
        if(!isDigitDot(document.all.txtBonusPre.value))  
        {
            parent.document.all.txtWroMessage.value="请输入数字格式。";
            document.all.txtBonusPre.focus();
            return false;					
        }
        if(!isDigitDot(document.all.txtPcent.value))  
        {
            parent.document.all.txtWroMessage.value="请输入数字格式。";
            document.all.txtPcent.focus();
            return false;					
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
        
         function ShowShopTree()
        {
        	strreturnval=window.showModalDialog('../Lease/Shop/SelectShop.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
			     var objImgBtn1 = document.getElementById('<%= LinkButton2.ClientID %>');
                objImgBtn1.click();
            }
			else
			{
				return;	
			}  
        }
        
        function selectShopBrand()
		{
			strreturnval=window.showModalDialog('../Lease/Brand/BrandSelect.aspx','window','dialogWidth=237px;dialogHeight=400px');
			window.document.all("brand").value = strreturnval;
			if ((window.document.all("brand").value != "undefined") && (window.document.all("brand").value != ""))
			{
//			document.getElementById("txtBrand").value=window.document.all("brand").value;
			    var objImgBtn1 = document.getElementById('<%= LinkButton3.ClientID %>');
                objImgBtn1.click();
            }
			else
				return;	
		}
		
		function ShowDeptId()
		{
			strreturnval=window.showModalDialog('showProductID.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
			    //			    document.getElementById("ddlDeptId").value=window.document.all("allvalue").value;
			    var objImgBtn1 = document.getElementById('<%= LinkButton4.ClientID %>');
			    objImgBtn1.click();
            }
			else
				return;	
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
	    function text()
	    {
	        var key=window.event.keyCode;
	        if ((key>=48&&key<=57)||(key==08)) 
            {
                window.event.returnValue =true;
            }
            else
            {
            window.event.returnValue =false;
            }
	    
	    }
    </script>
</head>
<body style="margin:0px" onload ="Load()">
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
              <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 4px">
                <tr>
                    <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 356px;
                        height: 22px; text-align: left">
                        <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_SkuInfo %>"
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
                        <td style="height: 17px; width: 100%;" rowspan="1">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label21" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                                    <td style="width: 25%; height: 32px">
                                        <asp:TextBox ID="txtQuery" runat="server" CssClass="ipt160px" Width="100%" ></asp:TextBox></td>
                                    <td colspan="1" style="width: 15%; height: 32px; text-align: right">
                                        <asp:Label ID="Label20" runat="server" Text="<%$ Resources:BaseInfo,Rpt_SkuId %>"></asp:Label></td>
                                    <td colspan="1" style="width: 30%; height: 32px; text-align: left">
                                        <asp:DropDownList ID="dropSkuID" runat="server" Width="83%" AutoPostBack="True" 
                                            OnSelectedIndexChanged="dropSkuID_SelectedIndexChanged">
                                        </asp:DropDownList></td>
                                </tr>
                                <tr>
                                    <td style="height: 10px" colspan="4">
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
                                    <td style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label16" runat="server" 
                                            Text="<%$ Resources:BaseInfo,Rpt_SkuId_last %>"></asp:Label></td>
                                    <td style="width: 25%; height: 32px">
                                        <asp:TextBox ID="txtSkuId" runat="server" CssClass="ipt160px" MaxLength="3" 
                                            Width="100%" onkeydown="text()"></asp:TextBox>
                                        </td>
                                    <td colspan="1" style="width: 15%; height: 32px; text-align: right">
                                        <asp:Label ID="Label4" runat="server" 
                                            Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label>
                                    </td>
                                    <td colspan="1" style="width: 30%; height: 32px; text-align: left">
                                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" 
                                            ReadOnly="True" Width="83%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; height: 32px; text-align: right">
                                        <asp:Label ID="Label17" runat="server" Text="<%$ Resources:BaseInfo,Rpt_SkuDesc %>"></asp:Label></td>
                                    <td style="width: 25%; height: 32px">
                                        <asp:TextBox ID="txtSkuDesc" runat="server" CssClass="ipt160px" Width="100%"></asp:TextBox></td>
                                    <td colspan="1" style="width: 15%; height: 32px; text-align: right">
                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Brand%>"></asp:Label>
                                    </td>
                                    <td colspan="1" style="width: 30%; height: 32px; text-align: left">
                                        <asp:TextBox ID="txtBrand" runat="server" CssClass="ipt160px" Width="83%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Rpt_DeptId %>" ></asp:Label></td>
                                    <td style="width: 25%; height: 32px;">
                                        <asp:TextBox ID="ddlDeptId" runat="server" Width="100%" 
                                            onclick="ShowDeptId();" CssClass="ipt160px"></asp:TextBox></td>
                                    <td colspan="1" style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Spec %>"></asp:Label></td>
                                    <td colspan="1" style="width: 30%; height: 32px; text-align: left;">
                                        <asp:TextBox ID="txtSpec" runat="server" CssClass="ipt160px" Width="83%" 
                                            MaxLength="10"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Pcode %>"></asp:Label></td>
                                   <td style="width: 25%">
                                        <asp:TextBox ID="txtPcode" runat="server" CssClass="ipt160px" Width="100%" 
                                            MaxLength="10"></asp:TextBox></td>
                                    <td colspan="1" style="width: 15%; text-align: right;">
                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Unit %>"></asp:Label></td>
                                    <td colspan="1" style="width: 30%">
                                        <asp:TextBox ID="txtUnit" runat="server" CssClass="ipt160px" Width="83%" 
                                            MaxLength="5"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,Rpt_color %>"></asp:Label></td>
                                    <td style="height: 30px">
                                        <asp:TextBox ID="txtColor" runat="server" CssClass="ipt160px" Width="100%" 
                                            MaxLength="10"></asp:TextBox></td>
                                    <td colspan="1" style="width: 15%; height: 30px; text-align: right">
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Produce %>"></asp:Label></td>
                                    <td colspan="1" style="height: 30px">
                                        <asp:TextBox ID="txtProduct" runat="server" CssClass="ipt160px" Width="83%" 
                                            MaxLength="10"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label9" runat="server" 
                                            Text="<%$ Resources:BaseInfo,Rpt_Price %>"></asp:Label>
                                    </td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="ipt160px" Width="100%" 
                                            MaxLength="12"></asp:TextBox>
                                    </td>
                                    <td colspan="1" style="width: 15%; text-align: right;">
                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Level %>"></asp:Label></td>
                                    <td colspan="1" style="width: 30%">
                                        <asp:TextBox ID="txtLevel" runat="server" CssClass="ipt160px" Width="83%" 
                                            MaxLength="4"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label11" runat="server" 
                                            Text="<%$ Resources:BaseInfo,Rpt_dPriceMin %>"></asp:Label>
                                    </td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txtPriceMin" runat="server" CssClass="ipt160px" Width="100%" 
                                            MaxLength="12"></asp:TextBox>
                                    </td>
                                    <td colspan="1" style="width: 15%; text-align: right;">
                                        <asp:Label ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Component %>"></asp:Label></td>
                                    <td colspan="1" style="width: 30%">
                                        <asp:TextBox ID="txtComponent" runat="server" CssClass="ipt160px" Width="83%" 
                                            MaxLength="10"></asp:TextBox></td>
                                </tr>
                                <tr>
                                   <td style="width: 15%; height: 32px; text-align: right;">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Stock%>"></asp:Label>
                                    </td>
                                    <td style="width: 25%">
                                        <asp:TextBox ID="txtStock" runat="server" CssClass="ipt160px" Width="100%" 
                                            MaxLength="12"></asp:TextBox>
                                    </td>
                                    <td colspan="1" style="width: 15%; text-align: right;">
                                        <asp:Label ID="Label15" runat="server" Text="<%$ Resources:BaseInfo,Rpt_dPriceMax %>"></asp:Label></td>
                                    <td colspan="1" style="width: 30%">
                                        <asp:TextBox ID="txtPriceMax" runat="server" CssClass="ipt160px" Width="83%" 
                                            MaxLength="12"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 15%; height: 32px; text-align: right;">
                                       <asp:Label ID="Label19" runat="server" Text="<%$ Resources:BaseInfo,SkuInfo_SkuStatus %>"></asp:Label></td></td>
                                    <td style="width: 25%">
                                       <asp:DropDownList ID="SkuStatus" runat ="server" Width="100%"></asp:DropDownList></td>
                                    <td colspan="1" style="width: 15%; text-align: right;">
                                       <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,Rpt_BonusGpPer %>"></asp:Label></td><td colspan="1" style="width: 30%">
                                        <asp:TextBox ID="txtBonusPre" runat="server" CssClass="ipt160px" Width="83%" 
                                            MaxLength="10"></asp:TextBox></td></tr><tr>
                                    <td style="width: 15%; text-align: right;">
                                        <asp:Label ID="Label32" runat="server" 
                                            Text="<%$ Resources:BaseInfo,Rpt_isDiscountCode%>"></asp:Label></td><td style="width: 25%">
                                        <asp:RadioButton ID="RadoY" runat="server" CssClass="labelStyle" GroupName="a" 
                                            Text="<%$ Resources:BaseInfo,Associator_rdoYes %>" />
                                        <asp:RadioButton ID="RadoN" runat="server" CssClass="labelStyle" GroupName="a" 
                                            Text="<%$ Resources:BaseInfo,Associator_rdoNo %>" />
                                        <asp:CheckBox ID="chkSkuLocked" runat="server" 
                                            Text="<%$ Resources:BaseInfo,SkuMaster_lblSkuLocked %>" Width="104px" />
                                    </td>
                                    <td colspan="1" style="width: 15%; text-align: right;">
                                        <asp:Label ID="Label18" runat="server" Text="<%$ Resources:BaseInfo,Rpt_DiscountPcentRate %>"></asp:Label></td><td colspan="1" style="width: 30%">
                                        <asp:TextBox ID="txtPcent" runat="server" CssClass="ipt160px" Width="83%" 
                                            MaxLength="5"></asp:TextBox></td></tr><tr>
                                    <td colspan="2" style="width: 45%; height: 6px">
                                        </td>
                                    <td colspan="1" style="width: 15%; height: 6px;">
                                        </td>
                                    <td colspan="1" style="width: 30%; height: 6px;">
                                        </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px" align="right">
                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" 
                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" OnClick="btnAdd_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit"
                         Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>" OnClick="btnEdit_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                            &nbsp;
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnQuit_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                            &nbsp;&nbsp;&nbsp;<asp:Button ID="BtnBlankOut" runat="server" CssClass="buttonClear" 
                                Text="<%$ Resources:BaseInfo,Btn_Del %>" OnClick="BtnDel_Click" 
                                onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                onmouseup="BtnUp(this.id);" Visible="False"/>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                        </td>
                    </tr>
                </table>
                <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
             <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton><asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton><input id="brand" style="width: 24px" type="hidden" runat="server" />
                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="LinkButton3_Click"></asp:LinkButton><asp:LinkButton ID="LinkButton4" runat="server" OnClick="LinkButton4_Click"></asp:LinkButton></ContentTemplate></asp:UpdatePanel></form></body></html>