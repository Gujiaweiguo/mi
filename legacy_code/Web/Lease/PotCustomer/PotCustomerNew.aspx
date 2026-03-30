<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustomerNew.aspx.cs" Inherits="Lease_PotCustomer_PotCustomerNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_hiddenInformation")%></title>
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
	        var str= document.getElementById("PotCustomer_Basic").value + ",Lease/PotCustomer/PotCustomerNew.aspx~" + 
	        document.getElementById("PotCustomer_ClientCard").value + ",Lease/PotCustomer/CustLicense.aspx~"+ 
	        document.getElementById("PotCustomer_lblTradeBrand").value + ",Lease/PotCustomer/PotCustBrand.aspx~"+ 
	        document.getElementById("PotCustomer_ManageSurvey").value + ",Lease/PotCustomer/PotCustOprInfo.aspx~"+ 
	         document.getElementById("Hidden_Shop").value + 
	        ",Lease/PotCustomer/PotShopNew.aspx~" + 
	        document.getElementById("PotShop_lblPalaverNode").value +",Lease/PotCustomer/Palaver.aspx";
	        addTabTool(str);
	        loadTitle();
       
	    }
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
	    
	     //输入验证
        function InputValidator(sForm)
        {
//             if(isEmpty(document.all.txtCustCode.value))
//            {
//                parent.document.all.txtWroMessage.value =('The Code is  Empty');
//                document.all.txtCustCode.focus();
//                return false;
//            }
             if(isEmpty(document.all.txtCustName.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtCustName.focus();
                return false;
            }
            
             if(isEmpty(document.all.txtCustShortName.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtCustShortName.focus();
                return false;
            }
            
        }
        
        function BillOfDocumentDelete()
        {
            return window.confirm('<%= billOfDocumentDelete %>');
        }
        
        function ShowMessage()
        {
            var wFlwID = document.getElementById("HidenWrkID").value;
            var vID = document.getElementById("HidenVouchID").value;
        	strreturnval=window.showModalDialog('../NodeMessage.aspx?wrkFlwID='+encodeURI(wFlwID)+'&voucherID='+encodeURI(vID),'window','dialogWidth=600px;dialogHeight=320px'); 
        }
        
        function ShowContact()
        {
           
        	//strreturnval=window.showModalDialog('PotCustContact.aspx','window','dialogWidth=650px;dialogHeight=500px');dialogWidth=240px;dialogHeight=435px
        	//window.open('PotCustContact.aspx?look=no','window','Width=800px,Height=400px,location=no,status=no,resize=no');
        	strreturnval=window.showModalDialog('PotCustContact.aspx?look=no','window','dialogWidth=700px;dialogHeight=460px');
        }
        function conf()
        {
            if(confirm("该数据已经存在,是否调用该数据？")==true)
            {
                document.getElementById("btnSearchData").click();
            }
        }
        function ReturnDefault()
        {
            window.parent.mainFrame.location.href="../../Disktop1.aspx";
        }
    </script>
	
</head>
<body style="margin-top:0; margin-left:0" onload="Load();">

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
                            <td colspan="2" align="left">
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
                                <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" MaxLength="16" OnTextChanged="txtCustCode_TextChanged" AutoPostBack="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="vertical-align: middle; width: 49px; height: 22px;
                                text-align: center">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" MaxLength="64" AutoPostBack="True" OnTextChanged="txtCustName_TextChanged"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                                &nbsp;<img id="ImgCustName" src="../../App_Themes/Main/Images/must.gif" style="width: 16px;
                                    height: 16px" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                                &nbsp;<img id="ImgCustShortName" src="../../App_Themes/Main/Images/must.gif" style="width: 16px;
                                    height: 16px" /></td>
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
                                <asp:DropDownList ID="cmbCustType" runat="server" CssClass="ipt160px">
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
                            <td colspan="2" align="left">
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
                                <asp:TextBox ID="txtLegalRep" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 21px; text-align: right">
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblLegalRepTitle %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 21px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 21px">
                                <asp:TextBox ID="txtLegalRepTitle" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 21px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblRegCap" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblRegCap %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px; text-align: left;">
                                <asp:TextBox ID="txtRegCap" runat="server" CssClass="ipt160px" MaxLength="16" Style="ime-mode: disabled" Width="76px">0</asp:TextBox>
                                <asp:DropDownList ID="DDownListCurrencyType" runat="server" Width="75px">
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
                                <asp:TextBox ID="txtRegAddr" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
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
                            <td colspan="2" align="left">
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
                                <asp:TextBox ID="txtRegCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblTaxCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTaxCode %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtTaxCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblBankName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtBankName" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 49px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblBankAcct" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtBankAcct" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
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
                    <asp:LinkButton ID="btnSearchData" runat="server" OnClick="btnSearchData_Click" Width="0px"></asp:LinkButton><br />
                    </td>
                <td class="tdBackColor" style=" height: 330px; text-align: left;" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 100px; width: 296px;">
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
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblPostCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblPostCode %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPostCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                <asp:Label ID="lblWeb" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblWeb %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtWeb" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
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
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: center">
                                <asp:Label ID="lblContactorName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustCreditLevel %>"
                                    Width="86px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px"><asp:DropDownList ID="ddlCreditLevel" runat="server" CssClass="ipt160px">
                            </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: left">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustSource %>"
                                    Width="86px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="ddlSourceType" runat="server" CssClass="ipt160px">
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
                                <asp:DropDownList ID="cmbCommOper" runat="server" CssClass="ipt160px">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 10px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 10px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 10px">
                                
                                </td>
                            <td class="tdBackColor" style="width: 30px; height: 10px">
                            </td>
                        </tr>
                                                                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                
                                <asp:Button ID="btnPeople" runat="server" CssClass="buttonLinkMan" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                 Text="<%$ Resources:BaseInfo,PotCustomer_LinkmanVindicate %>" Width="91px" OnClick="btnPeople_Click" /></td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
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
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                </td>
                            <td class="tdBackColor" style="width: 6px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                </td>
                            <td class="tdBackColor" style="width: 30px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 20px" align="center">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="4" style="text-align:center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
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
                               <table>
                                    <tr>
                                        <td style="height: 35px">
                                            <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                OnClick="btnTempSave_Click" Text="<%$ Resources:BaseInfo,Lease_NewLineBtnTemp %>"
                                                 />
                                            </td>
                                        <td style="width: 24px; height: 35px;">
                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel"  OnClick="btnCancel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                Text="<%$ Resources:BaseInfo,User_btnCancel %>"  /></td>
                                                <td style="width: 30px; height: 35px;">
                                            <asp:Button ID="btnPutIn" runat="server" CssClass="buttonPutIn"  OnClick="btnPutIn_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                Text="<%$ Resources:BaseInfo,Lease_NewLineBtnPutIn %>" /></td>
                                                <td style="width: 30px; height: 35px;">
                                                    <asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                         OnClick="btnBalnkOut_Click" Text="<%$ Resources:BaseInfo,ConLease_butDel %>"
                                                         /></td>
                                                        <td style="width: 30px; height: 35px;">
                                                    <asp:Button ID="btnMessage" runat="server" CssClass="buttonMessage" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                        Text="<%$ Resources:BaseInfo,WrkFlwEntity_btnMessage %>"
                                                          OnClick="btnMessage_Click"  /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
        <asp:HiddenField ID="PotCustomer_Basic" runat="server" Value="<%$ Resources:BaseInfo,PotCustomer_Basic %>" />
        <asp:HiddenField ID="PotCustomer_ClientCard" runat="server" Value="<%$ Resources:BaseInfo,PotCustomer_ClientCard %>" />
        <asp:HiddenField ID="Hidden_Shop" runat="server"  Value="<%$ Resources:BaseInfo,Hidden_Shop %>"/>
        <asp:HiddenField ID="PotShop_lblPalaverNode" runat="server"  Value="<%$ Resources:BaseInfo,PotShop_lblPalaverNode %>"/>
        <asp:HiddenField ID="PotCustomer_lblTradeBrand" runat="server" Value="<%$ Resources:BaseInfo,PotCustomer_lblTradeBrand %>" /><!--经营品牌-->
        <asp:HiddenField ID="PotCustomer_ManageSurvey" runat="server" Value="<%$ Resources:BaseInfo,PotCustomer_ManageSurvey %>" /><!--经营概况-->
        <asp:HiddenField ID="hidCustomerID" runat="server" />
            <asp:HiddenField ID="HidenWrkID" runat="server">
            </asp:HiddenField>
            <asp:HiddenField ID="HidenVouchID" runat="server">
            </asp:HiddenField>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
