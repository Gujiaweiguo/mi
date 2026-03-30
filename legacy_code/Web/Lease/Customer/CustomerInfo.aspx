<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerInfo.aspx.cs" Inherits="Lease_Customer_CustomerInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title><%= (String)GetGlobalResourceObject("BaseInfo", "Customer_labCustomerQuery")%></title>
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
	        addTabTool("<%=PotCustomer_Basic %>,Lease/Customer/CustomerInfo.aspx~<%=PotCustomer_ClientCard %>,Lease/Customer/CustLicense.aspx~<%=PotCustomer_lblTradeBrand %>,Lease/Customer/CustBrand.aspx?browse=yes~<%=Customer_OprInfo %>,Lease/Customer/CustOprBaseInfo.aspx?browse=yes");
	        loadTitle();
	    }
	    function ShowContact()
        {
        	window.showModalDialog('CustContact.aspx?look=yes','window','dialogWidth=700px;dialogHeight=460px');
        }
    </script>
</head>
<body onload='Load()' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="showmain" border="0" cellpadding="0" cellspacing="0"
            style="height: 445px; width:100%">
           
            <tr>
                <td style="vertical-align: top; width: 99%; height: 401px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="PotCustomer">
             <table border="0" cellpadding="0" cellspacing="0" style="height: 405px; width:100%;" width="535" id="TABLE1" >
                        <tr>
                            <td class="tdTopBackColor" style="height: 25px; vertical-align: middle; text-align: left;" valign="top">
                                <img alt="" class="imageLeftBack" />
                                <asp:Label ID="Label4" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Customer_labCustomerQuery %>" Width="229px"></asp:Label></td>
                            <td class="tdTopRightBackColor" colspan="2" style="height: 25px" valign="top">
                                <img class="imageRightBack" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="2" style="width: 50%; height: 320px; text-align: left"
                                valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 310px" width="255">
                                    <tr>
                                        <td colspan="4" style="height: 1px; background-color: white">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="height: 5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="height: 1px; text-align: left; vertical-align: bottom;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 166px">
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #738495; left: 90px; position: relative; top: 3px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #ffffff; left: 90px; position: relative; top: 3px;">
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
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblCustCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                            </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblCustType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:DropDownList ID="cmbCustType" runat="server" Width="164px" Enabled="False" >
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                     <tr>
                                       <td style="width:255px; height:5px;" class="tdBackColor" colspan="4"></td>
                                     </tr>
                                    <tr>
                                        <td class="tdBackColor" style="height: 1px; text-align: center;" colspan="4">
                                        </td>
                                    </tr>
                                                                         <tr>
                                       <td style="width:255px; height:5px; vertical-align: middle;" class="tdBackColor" colspan="4">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 166px; left: 90px;">
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #738495; left: 90px; position: relative;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #ffffff; left: 90px; position: relative;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                     </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblLegalRep" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRep %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtLegalRep" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRepTitle %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtLegalRepTitle" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblRegCap" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCap %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtRegCap" runat="server" CssClass="Enabledipt160px" ReadOnly="True" Width="72px"></asp:TextBox>
                                            <asp:DropDownList ID="DDownListCurrencyType" runat="server" Enabled="False" Width="75px">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblRegAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegAddr %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtRegAddr" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                   <tr>
                                       <td style="width:255px; height:5px;" class="tdBackColor" colspan="4"></td>
                                     </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="height: 1px; text-align: left; vertical-align: bottom;">
                                        </td>
                                    </tr>
                                     <tr>
                                       <td style="width:255px; height:5px; vertical-align: middle;" class="tdBackColor" colspan="4">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 166px; left: 90px;">
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #738495; left: 90px; position: relative;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 166px; height: 1px; background-color: #ffffff; left: 90px; position: relative;">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                     </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px">
                                            <asp:Label ID="lblRegCode" runat="server" CssClass="labelStyle" Height="18px" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCode %>"
                                                Width="88px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtRegCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblTaxCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTaxCode %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtTaxCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblBankName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtBankName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblBankAcct" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtBankAcct" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 4px; height: 22px">
                                        </td>
                                    </tr>
                            <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblOfficeAddr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblOfficeAddr %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr" runat="server" CssClass="Enabledipt160px" MaxLength="256" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                                                <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr2" runat="server" CssClass="Enabledipt160px" MaxLength="256" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                                                <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeAddr3" runat="server" CssClass="Enabledipt160px" MaxLength="256" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                                </table>
                                <br />
                            </td>
                            <td class="tdBackColor" style="width:50%; height: 320px" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 100px" width="280">
                                    <tr>
                                        <td colspan="4" style="width: 280px; height: 1px; background-color: white">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="width: 280px; height: 10px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 1px">
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
                                <asp:TextBox ID="txtPostAddr" runat="server" CssClass="Enabledipt160px" MaxLength="256" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                                                <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostAddr2" runat="server" CssClass="Enabledipt160px" MaxLength="256" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostAddr3" runat="server" CssClass="Enabledipt160px" MaxLength="256" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblPostCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostCode %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtPostCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="lblWeb" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblWeb %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtWeb" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 5px">
                                        </td>
                                        <td class="tdBackColor" style="width: 30px; height: 1px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 1px">
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
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
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
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            <asp:Label ID="labAttract" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContractAuditing_labAttract %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            <asp:TextBox ID="txtCommOper" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 11px; text-align: right">
                                            </td>
                                        <td class="tdBackColor" style="width: 5px; height: 11px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 11px">
                                            </td>
                                        <td class="tdBackColor" style="width: 30px; height: 11px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            </td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            &nbsp;<asp:Button ID="btnPeople" runat="server" CssClass="buttonLinkMan" Height="31px"
                                                OnClick="btnPeople_Click" Text="<%$ Resources:BaseInfo,PotCustomer_LinkmanVindicate %>"
                                                Width="92px" /></td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                    				    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 5px">
                                        </td>
                                        <td class="tdBackColor" style="width: 30px; height: 1px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 1px">
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 1px">
                                        </td>
                                        <td colspan="2">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="width: 255px; height: 4px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                            </td>
                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                        <td class="tdBackColor" style="width: 160px; height: 22px">
                                            </td>
                                        <td class="tdBackColor" style="width: 30px; height: 22px">
                                        </td>
                                    </tr>
                                </table>
                                </table>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
