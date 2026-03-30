<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustomerAuditing.aspx.cs" Inherits="Lease_PotCustomer_PotCustomerAuditing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_Basic")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        
	        var str= document.getElementById("PotCustomer_Basic").value + ",Lease/PotCustomer/PotCustomerAuditing.aspx~" + 
	        document.getElementById("PotCustomer_ClientCard").value + ",Lease/PotCustomer/CustLicenseAutiding.aspx~"+ 
	        
	        document.getElementById("PotCustomer_lblTradeBrand").value + ",Lease/PotCustomer/PotCustBrand.aspx?look=yes~"+
	        document.getElementById("PotCustomer_ManageSurvey").value + ",Lease/PotCustomer/PotCustOprInfo.aspx?look=yes~"+ 
	        document.getElementById("Hidden_Shop").value +  ",Lease/PotCustomer/PotShopNew.aspx?look=yes~" +
	         //document.getElementById("PotShop_lblPalaverNode").value +",Lease/PotCustomer/CustPalaverAutiding.aspx";谈判记录
	        document.getElementById("PotShop_lblPalaverNode").value +",Lease/PotCustomer/Palaver.aspx?look=yes";
	        addTabTool(str);
	        loadTitle();
	    }
	    function refurbishTree()
        {
             parent.frames(0).location.reload();
             window.opener=null;
             window.close();
        }
        
         function ShowMessage()
        {
            var wFlwID = document.getElementById("HidenWrkID").value;
            var vID = document.getElementById("HidenVouchID").value;
        	strreturnval=window.showModalDialog('../NodeMessage.aspx?wrkFlwID='+encodeURI(wFlwID)+'&voucherID='+encodeURI(vID),'window','dialogWidth=600px;dialogHeight=320px'); 
        }
        function ReturnDefault()
        {
            window.parent.mainFrame.location.href="../../Disktop1.aspx";
        }
    </script>
	
</head>
<body style="margin-top:0; margin-left:0" onload="Load()">

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
                    <%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustTitle") %>
                </td>
            </tr>
        </table>

        <table id="TABLE1" border="0" cellpadding="0" cellspacing="0" style="height: 405px; width:100%; text-align: center;">
            <tr>
                <td class="tdBackColor" colspan="2" style="height: 330px; text-align: right"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height:330px;width:255px;">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 6px; height: 1px">
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 170px">
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
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                                &nbsp;</td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" MaxLength="16" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="vertical-align: middle; width: 49px; height: 22px;
                                text-align: center">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 2px; text-align: right">
                            </td>
                            <td class="tdBackColor" style="width: 6px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 49px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="cmbCustType" runat="server" CssClass="ipt160px" Enabled="False">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 6px; height: 1px">
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 170px">
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
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblLegalRep" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRep %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtLegalRep" runat="server" CssClass="Enabledipt160px" MaxLength="16" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRepTitle %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtLegalRepTitle" runat="server" CssClass="Enabledipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblRegCap" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCap %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px; text-align: left;">
                                <asp:TextBox ID="txtRegCap" runat="server" CssClass="Enabledipt160px" MaxLength="16"
                                    ReadOnly="True" Style="ime-mode: disabled" Width="76px">0</asp:TextBox><asp:DropDownList
                                        ID="DDownListCurrencyType" runat="server" Enabled="False" Width="75px">
                                    </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblRegAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegAddr %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtRegAddr" runat="server" CssClass="Enabledipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 6px; height: 1px">
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 170px">
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
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px">
                                <asp:Label ID="lblRegCode" runat="server" CssClass="labelStyle" Height="18px" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCode %>"
                                    Width="88px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtRegCode" runat="server" CssClass="Enabledipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblTaxCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTaxCode %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtTaxCode" runat="server" CssClass="Enabledipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblBankName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="Enabledipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblBankAcct" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtBankAcct" runat="server" CssClass="ipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblOfficeAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblOfficeAddr %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                                                <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr2" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                                                <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr3" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                    </table>
                    <br />
                </td>
                <td class="tdBackColor" style=" height: 330px; text-align: left;" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 100px" width="280">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 170px">
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
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblPostAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblPostAddr %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostAddr" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostAddr2" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostAddr3" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblPostCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostCode %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostCode" runat="server" CssClass="Enabledipt160px" MaxLength="16" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblWeb" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblWeb %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtWeb" runat="server" CssClass="Enabledipt160px" MaxLength="128" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 5px">
                            </td>
                            <td class="tdBackColor" style="width: 30px; height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 170px">
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
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblContactorName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustCreditLevel %>"
                                    Width="86px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="ddlCreditLevel" runat="server" CssClass="ipt160px" Enabled="False">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustSource %>"
                                    Width="86px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="ddlSourceType" runat="server" CssClass="ipt160px" Enabled="False">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="labAttract" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContractAuditing_labAttract %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="cmbCommOper" runat="server" CssClass="ipt160px" Enabled="False">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                                                                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 14px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 14px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 14px">
                                </td>
                            <td class="tdBackColor" style="width: 30px; height: 14px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:Button ID="btnPeople" runat="server" CssClass="buttonLinkMan" Height="31px" OnClick="btnPeople_Click"
                                    Text="<%$ Resources:BaseInfo,PotCustomer_LinkmanVindicate %>" Width="92px" /></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 5px">
                            </td>
                            <td class="tdBackColor" style="width: 30px; height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                </td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                                                <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                </td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 20px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align: right">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 250px">
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
                            <td colspan="4" style="height: 44px; text-align: right">
                                <asp:Button ID="btnOk" runat="server" CssClass="buttonOk" OnClick="butConsent_Click"
                                    Text="<%$ Resources:BaseInfo,CustPalaver_butConsent %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear" OnClick="butOverrule_Click"
                                    Text="<%$ Resources:BaseInfo,CustPalaver_butOverrule %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                 <asp:Button ID="btnMessage" runat="server" CssClass="buttonMessage" Text="<%$ Resources:BaseInfo,WrkFlwEntity_btnMessage %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                    </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
                &nbsp;&nbsp;
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="PotCustomer_Basic" runat="server" Value="<%$ Resources:BaseInfo,PotCustomer_Basic %>" />
        <asp:HiddenField ID="Hidden_Shop" runat="server"  Value="<%$ Resources:BaseInfo,Hidden_Shop %>"/>
        <asp:HiddenField ID="PotShop_lblPalaverNode" runat="server"  Value="<%$ Resources:BaseInfo,PotShop_lblPalaverNode %>"/>
        <asp:HiddenField ID="PotCustomer_ClientCard" runat="server" Value="<%$ Resources:BaseInfo,PotCustomer_ClientCard %>" />
        <asp:HiddenField ID="PotCustomer_ManageSurvey" runat="server" Value="<%$ Resources:BaseInfo,PotCustomer_ManageSurvey %>" /><!--经营概况-->
        <asp:HiddenField ID="PotCustomer_lblTradeBrand" runat="server" Value="<%$ Resources:BaseInfo,PotCustomer_lblTradeBrand %>" /><!--经营品牌-->
         <asp:HiddenField ID="HidenWrkID" runat="server">
            </asp:HiddenField>
            <asp:HiddenField ID="HidenVouchID" runat="server">
            </asp:HiddenField>
    </form>
</body>
</html>
