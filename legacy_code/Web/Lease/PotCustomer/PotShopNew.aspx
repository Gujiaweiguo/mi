<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotShopNew.aspx.cs" Inherits="Lease_PotCustomer_PotShopNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Hidden_Shop")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    
    <script type="text/javascript">
    /*验证数字类型*/
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
	     //意向单元
        function ShowUnitTree()
        {
			//window.open('UnitSelect.aspx','window','Width=237px,Height=420px');
			strreturnval=window.showModalDialog('../Brand/UnitSelect.aspx?deptid='+document.all.ddlBusinessItem.value + '&startDate=' +document.all.txtShopStartDate.value + '&endDate=' + document.all.txtShopEndDate.value,'window','dialogWidth=240px;dialogHeight=435px');
            window.document.all("txtHidValue").value = strreturnval;
			if ((window.document.all("txtHidValue").value != "undefined") && (window.document.all("txtHidValue").value != ""))
            {
                var arr = strreturnval.split("|");
                window.document.all("txtUnitID").value = arr[0];//单元iD
                window.document.all("txtUnits").value = arr[1];//单元Code
                window.document.all("txtStore").value = arr[2];//楼、层、方位 的编号
            }
        }
        //输入验证
        function InputValidator()
        {
             if(isEmpty(document.all.txtRentalPrice.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtRentalPrice.focus();
                return false;
            }
            
             if(isEmpty(document.all.txtRentArea.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtRentArea.focus();
                return false;
            }
             if(isEmpty(document.all.txtRentInc.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtRentInc.focus();
                return false;
            }
             if(isEmpty(document.all.txtPcent.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtPcent.focus();
                return false;
            }
             if(isEmpty(document.all.txtMainBrand.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtMainBrand.focus();
                return false;
            }
             if(isEmpty(document.all.txtShopStartDate.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtShopStartDate.focus();
                return false;
            }
             if(isEmpty(document.all.txtShopEndDate.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtShopEndDate.focus();
                return false;
            }
            if(isEmpty(document.all.txtUnits.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtUnits.focus();
                return false;
            }
            if(isEmpty(document.all.txtPotShopName.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtPotShopName.focus();
                return false;
            }
           if(LicenseBoxValidator(document.all.txtShopStartDate,document.all.txtShopEndDate,'<%= strErrorTime %>')==false)
           {
                return false;
           }
        }
    </script>
</head>
<body style="margin-top:0; margin-left:0" onload="loadTitle();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "Title_PotShop")%>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 410px">
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 50%; height: 281px; text-align: right"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 255px; height: 231px">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 9px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 10px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 164px; text-align: center">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCreateUserID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_CustCode %>"
                                    Width="50px"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px" align="left">
                                <asp:TextBox ID="txtCreateUserID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"
                                    Width="63px"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px" align="left">
                                <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"
                                    Width="58px"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px" align="left">
                                <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 27px; text-align: right">
                                <asp:Label ID="lblBizMode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_BizMode %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 27px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 27px" align="left">
                                <asp:DropDownList ID="cmbBizMode" runat="server" CssClass="cmb160px" Width="163px">
                                </asp:DropDownList><font color=red></font></td>
                            <td class="tdBackColor" style="width: 5px; height: 27px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 28px; text-align: right">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopTypeTitle %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 28px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 28px" align="left">
                                <asp:DropDownList ID="ddlShopType" runat="server" CssClass="cmb160px" Width="164px">
                            </asp:DropDownList><font color=red></font></td>
                            <td class="tdBackColor" style="width: 5px; height: 28px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                               <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px" align="left">
                                <asp:DropDownList ID="ddlBusinessItem" runat="server" CssClass="cmb160px" Width="164px">
                                </asp:DropDownList><font color=red></font></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 160px; height: 22px;text-align: right">
                                <asp:Label ID="lblRentalPrice" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblRentalPrice %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px">
                                <asp:TextBox ID="txtRentalPrice" runat="server" CssClass="ipt160px" MaxLength="9"></asp:TextBox>&nbsp;<img id="ImgRentalPrice" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" />
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px">
                                <asp:Label ID="lblRentArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblRentArea %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px; text-align: center">
                                <asp:TextBox ID="txtRentArea" runat="server" CssClass="ipt160px" MaxLength="13"></asp:TextBox>&nbsp;<img id="imgRentArea" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" />
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustRentInc %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px">
                                <asp:TextBox ID="txtRentInc" runat="server" CssClass="ipt160px" MaxLength="13"></asp:TextBox>&nbsp;<img id="imgRentInc" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustTakeOutRate %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px">
                                <asp:TextBox ID="txtPcent" runat="server" CssClass="ipt160px" MaxLength="13"></asp:TextBox>&nbsp;<img id="imgPcent" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblMainBrand" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblMainBrand %>"></asp:Label>&nbsp;&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 190px; height: 22px">
                                <asp:TextBox ID="txtMainBrand" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox>&nbsp;<img id="imgMainBrand" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        
                    </table>
                </td>
                <td class="tdBackColor" style="width: 280px; height: 281px" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 280px; height: 231px">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 9px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 10px; text-align: right">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 10px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 10px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 164px; text-align: center">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 19px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblShopStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"
                                    Width="72px"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" colspan="2" style="height: 22px">
                                <asp:TextBox ID="txtShopStartDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox>&nbsp;<img id="imgShopStartDate" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblShopEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" colspan="2" style="height: 22px">
                                <asp:TextBox ID="txtShopEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox>&nbsp;<img id="imgShopEndDate" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_IntentUnits %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" colspan="2" style="height: 22px">
                               <asp:TextBox ID="txtUnits" runat="server" CssClass="ipt160px" Height="22px" TextMode="MultiLine" Width="160px"
                                    ></asp:TextBox>&nbsp;<img id="imgUnits" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 10px; text-align: right">
                                <asp:Label ID="lblPotShopName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 10px">
                            </td>
                            <td class="tdBackColor" colspan="2" style="height: 10px">
                                <asp:TextBox ID="txtPotShopName" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox>&nbsp;<img id="imgPotShopName" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 11px; text-align: right">
                                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PotShopHighReg %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 11px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 11px">
                               <asp:TextBox ID="txtHighReg" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 11px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 12px; text-align: right">
                                <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PotShopLoadReg %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 12px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 12px">
                                <asp:TextBox ID="txtLoadReg" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 12px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PotShopWaterReg %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 22px">
                                <asp:TextBox ID="txtWaterReg" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                         <tr>
                            <td class="tdBackColor" style="width: 96px; height: 8px; text-align: right">
                                <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PotShopPowerReg %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 8px">
                            </td>
                            <td class="tdBackColor" style="width: 172px; height: 8px">
                                <asp:TextBox ID="txtPowerReg" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox><font color=red></font></td>
                            <td class="tdBackColor" style="width: 19px; height: 8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="height: 22px; text-align: right" valign="bottom" colspan="4">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 59%; right: 21px; position: relative; top: 0px;">
                        <tr>
                            <td style="width: 160px; height: 1px; background-color: #738495" >
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160px; height: 1px; background-color: #ffffff">
                            </td>
                        </tr>
                    </table>
                                &nbsp; &nbsp;&nbsp; &nbsp;<font color=red></font></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="height: 22px; text-align: right" colspan="4">
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    OnClick="btnAdd_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />&nbsp;
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                     OnClick="btnQuit_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" 
                                     />
                                &nbsp;&nbsp; &nbsp; &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="4" style="width: 100%; height: 49px; text-align: center">
                    <table style="width: 508px">
                        <tr>
                            <td style="text-align: right">
                                &nbsp;
                                <asp:LinkButton ID="btnDellBrand" runat="server" OnClick="btnDellBrand_Click" Width="0px"></asp:LinkButton>
                                    <asp:TextBox ID="hidUnitID" runat="server" CssClass="hidden"  Width="1px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                                <asp:Label ID="lblArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblArea %>" Visible="False"></asp:Label>
                                <asp:DropDownList ID="cmbArea" runat="server" CssClass="cmb160px" Visible="False">
                                </asp:DropDownList>
                                <asp:Label ID="lblCommOper" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContractAuditing_labAttract %>" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtCommOper" runat="server" CssClass="Enabledipt160px" ReadOnly="True"
                                    Width="150px" Visible="False"></asp:TextBox>
                                <asp:Label ID="lblNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>" Visible="False"></asp:Label>
                                <asp:TextBox ID="txtNode" runat="server" CssClass="OpenColor" Height="27px" MaxLength="128"
                                    TextMode="MultiLine" Width="160px" Visible="False"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="3" style="height: 3px; text-align: right">
                </td>
            </tr>
        </table>
    
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
                    <asp:TextBox ID="txtUnitID" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
        <asp:TextBox ID="txtStore" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
        <asp:TextBox ID="txtHidValue" runat="server" CssClass="hidden" Width="0px"></asp:TextBox>
    </form>
</body>
</html>
