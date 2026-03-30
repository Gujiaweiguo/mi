<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustomerBaseInfoUpdate.aspx.cs" Inherits="Lease_PotCustomer_PotCustomerBaseInfoUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_labCustomerUptate")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool("<%=PotCustomer_Basic %>,Lease/PotCustomer/PotCustomerBaseInfoUpdate.aspx~<%=PotCustomer_ClientCard %>,Lease/PotCustomer/PotCustLicenseUpdate.aspx~<%=PotCustomer_TitlePalaver %>,Lease/PotCustomer/PalaverModi.aspx");
	        loadTitle();
	    }
	</script>
</head>
<body style="margin:0px" onload='Load()'>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <div style="width:100%">
                <table id="TABLE1" border="0" cellpadding="0" cellspacing="0" style="width: 114%;
                    height: 405px">
                    <tr>
                        <td class="tdTopBackColor" style="vertical-align: middle; width: 1674px; height: 25px;
                            text-align: left" valign="top">
                            <img alt="" class="imageLeftBack" />
                            <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_labCustomerUptate %>"
                                Width="293px"></asp:Label></td>
                        <td class="tdTopRightBackColor" colspan="2" style="width: 528px; height: 25px" valign="top">
                            <img class="imageRightBack" /></td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="2" style="width: 50%; height: 320px; text-align: left"
                            valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 310px" width="100%">
                                <tr>
                                    <td colspan="4" style="height: 1px; background-color: white">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="left: 100px; vertical-align: bottom; width: 100%;
                                        height: 1px; text-align: left">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 165px">
                                            <tr>
                                                <td style="left: 90px; width: 166px; position: relative; top: 3px; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="left: 90px; width: 166px; position: relative; top: 3px; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblCustCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblCustType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:DropDownList ID="cmbCustType" runat="server" Width="164px">
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="vertical-align: bottom; height: 1px; text-align: left">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="vertical-align: middle; width: 100%; height: 5px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 166px">
                                            <tr>
                                                <td style="left: 90px; width: 166px; position: relative; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="left: 90px; width: 166px; position: relative; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblLegalRep" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRep %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtLegalRep" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRepTitle %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtLegalRepTitle" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblRegCap" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCap %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtRegCap" runat="server" CssClass="ipt160px" MaxLength="19" Width="81px"></asp:TextBox>
                                        <asp:DropDownList ID="DDownListCurrencyType" runat="server" Width="75px">
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblRegAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegAddr %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtRegAddr" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="height: 1px; text-align: center">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 100%; height: 5px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: middle;
                                            width: 166px">
                                            <tr>
                                                <td style="left: 90px; width: 166px; position: relative; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="left: 90px; width: 166px; position: relative; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblRegCode" runat="server" CssClass="labelStyle" Height="18px" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCode %>"
                                            Width="88px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtRegCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblTaxCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTaxCode %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtTaxCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblBankName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtBankName" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblBankAcct" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 204px; height: 22px">
                                        <asp:TextBox ID="txtBankAcct" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 22px; width: 17px;">
                                    </td>
                                </tr>
                                                                                                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblOfficeAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeAddr %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                                                <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeAddr2 %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr2" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                                                <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeAddr3 %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr3" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                            </table>
                        </td>
                        <td class="tdBackColor" style="width: 50%; height: 320px" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 100px" width="100%">
                                <tr>
                                    <td colspan="4" style="width: 280px; height: 1px; background-color: white">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 280px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 1px">
                                    </td>
                                    <td class="tdBackColor" style="width: 84px; height: 1px">
                                    </td>
                                    <td colspan="2" style="width: 100%">
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
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblPostAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostAddr %>" Width="91px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtPostAddr" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostAddr2 %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostAddr2" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 29px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostAddr3 %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostAddr3" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 29px; height: 22px">
                            </td>
                        </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblPostCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostCode %>" Width="91px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtPostCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblWeb" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblWeb %>" Width="91px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtWeb" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 1px">
                                    </td>
                                    <td class="tdBackColor" style="width: 84px; height: 1px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 5px">
                                    </td>
                                    <td class="tdBackColor" style="width: 29px; height: 1px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 1px">
                                    </td>
                                    <td class="tdBackColor" style="width: 84px; height: 1px">
                                    </td>
                                    <td colspan="2" style="width: 100%">
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
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblContactorName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"
                                            Width="86px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtContactorName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblTitle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTitle %>"
                                            Width="83px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtTitle" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblOfficeTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeTel %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtOfficeTel" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblMobileTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblMobileTel %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtMobileTel" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right">
                                        <asp:Label ID="lblEMail" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblEMail %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtEMail" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 5px; text-align: right;">
                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelUnitStatus %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 5px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 5px">
                                        <asp:DropDownList ID="cmbCustTypeStatus" runat="server" BackColor="White" CssClass="cmb160px"
                                            Width="160px">
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="width: 29px; height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 1px">
                                    </td>
                                    <td class="tdBackColor" style="width: 84px; height: 1px">
                                    </td>
                                    <td colspan="2" style="width: 100%">
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
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="labAttract" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContractAuditing_labAttract %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 84px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:DropDownList ID="cmbCommOper" runat="server" CssClass="ipt160px">
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="width: 29px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 20px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="width: 100%; text-align: left">
                                        <table border="0" cellpadding="0" cellspacing="0" style="left: 10px; width: 243px">
                                            <tr>
                                                <td style="left: 17px; width: 160px; position: relative; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="left: 17px; width: 160px; position: relative; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 44px; text-align: left">
                                        <table>
                                            <tr>
                                                <td style="height: 17px">
                                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Height="31px" OnClick="btnCancel_Click"
                                                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" Width="70px" /><asp:Button ID="btnSave"
                                                            runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>"
                                                            Width="78px" /></td>
                                                <td style="width: 30px; height: 17px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
                    <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
         <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
    </form>
</body>
</html>
