<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChargeAccountAdBoard.aspx.cs" Inherits="Lease_ChargeAccount_ChargeAccountAdBoard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Lease_lblAdBoardAccount")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
        //输入验证
        <!--
        function InputValidator(sForm)
        {
            if(isEmpty(document.all.txtContractID.value))
            {
                alert('<%= emptyStr %>');
                document.all.txtConStartDate.focus();
                return false;
            }          
           
       }
       
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,Lease/ChargeAccount/ChargeAccountAdBoard.aspx");
	        loadTitle();
	    }
	    
	    function CheckAll()
	    {   
            for(var i=0;i<document.forms[0].length;i++)
			{
				if (document.forms[0][i].type=="checkbox" && document.forms[0][i].name!="ckcbSelect")
				{
					document.forms[0][i].checked=document.all["ckcbSelect"].checked;
				}
			}
			return;
	    }
       -->
    </script>
</head>
<body onload="Load()" style="margin:0px">
    <form id="form1" runat="server">
       <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>--%>
                            <div>
                                <table class="mainTbl" border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr style="height: 28px">
                                        <td valign="top">
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblAdBoardAccount %>" Width="560px"></asp:Label>
                                                    </td>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                        <img class="imageRightBack"/>
                                                    </td>
                                                </tr>
                                                <tr style="height:1px">
                                                    <td colspan="3" style="background-color:White; height:1px">
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width:100%" class="tdBackColor">
                                                <tr style="height:10px">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        
                                                    </td>
                                                    <td style="width: 43px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtContractID" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                    <td style="width: 43px">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %>" Width="79px" OnClick="btnQuery_Click" /></td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr style="height:10px">
                                                    <td style="text-align: center;" colspan="4">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 593px">
                                                            <tr style="height:1px">
                                                                <td style="width: 160px; height: 1px; background-color: #738495">
                                                                </td>
                                                            </tr>
                                                            <tr style="height:1px">
                                                                <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                                                    <td style="width: 43px">
                                                        </td>
                                                    <td>
                                                        <asp:RadioButton ID="RBtnPrint" runat="server" Width="91px" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_RadioAccountPrint %>" GroupName="b" />
                                                        <asp:RadioButton ID="RBtnNoPrint" runat="server" Width="66px" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_RadioNoPrint %>" GroupName="b" Checked="True" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_ContractType %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="cmbContractType" runat="server" Width="165px" Enabled="False">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 43px" class="lable" align="right">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_ContractAccountIng %>" Width="106px"></asp:Label></td>
                                                    <td>
                                                        <asp:TextBox ID="txtAccountContractID" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 28px;">
                                                    </td>
                                                    <td style="width: 218px; height: 28px;">
                                                        <asp:RadioButton ID="RBtnAllContract" runat="server" Width="86px" CssClass="labelStyle"  Text="<%$ Resources:BaseInfo,Lease_RadioAllContract %>" Checked="True" GroupName="a" AutoPostBack="True" OnCheckedChanged="RBtnAllContract_CheckedChanged" />
                                                        <asp:RadioButton ID="RBtnOneContract" runat="server" Width="84px"  CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_RadioOneContract %>" GroupName="a" AutoPostBack="True" OnCheckedChanged="RBtnOneContract_CheckedChanged" /></td>
                                                    <td style="width: 43px; height: 28px;">
                                                    </td>
                                                    <td style="height: 28px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td colspan="4" style="height: 28px; text-align: center">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 593px">
                                                            <tr style="height:1px">
                                                                <td style="width: 160px; height: 1px; background-color: #738495">
                                                                </td>
                                                            </tr>
                                                            <tr style="height:1px">
                                                                <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblConChargeType %>"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox ID="ckcbSelect" runat="server" Text="<%$ Resources:BaseInfo,ckck_selectAll %>" Font-Size="9pt" onclick="CheckAll()" Checked="True"/>
                                                        </td>
                                                    <td colspan="3">
                                                        <asp:CheckBoxList ID="chklChargeType" runat="server" DataTextField="ChargeTypeName"
                                                            DataValueField="ChargeTypeID" Font-Size="9pt" Height="9px" RepeatDirection="Horizontal" RepeatColumns="5">
                                                        </asp:CheckBoxList></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" colspan="4" style="text-align: center">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 593px">
                                                            <tr style="height:1px">
                                                                <td style="width: 160px; height: 1px; background-color: #738495">
                                                                </td>
                                                            </tr>
                                                            <tr style="height:1px">
                                                                <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblChargeDate %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtChargeDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 43px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr style="height:40px">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:Button ID="btnOK" runat="server" CssClass="buttonSave" Text="<%$ Resources:BaseInfo,User_btnOk %> " OnClick="btnOK_Click"/>
                                                        <asp:Button ID="BtnCel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="BtnCel_Click" Width="80px"/></td>
                                                    <td style="width: 43px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 43px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                    </td>
                                                    <td style="width: 43px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                      <%--  </ContentTemplate>
                    </asp:UpdatePanel>--%>
    </form>
</body>
</html>