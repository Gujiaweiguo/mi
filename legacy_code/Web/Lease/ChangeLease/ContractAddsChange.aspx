<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractAddsChange.aspx.cs" Inherits="Lease_ChangeLease_ContractAddsChange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
<script language="javascript" type="text/javascript">
// <!CDATA[

function Text2_onclick() {

}

// ]]>
</script>

    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 564px; height: 445px" border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle">
                    <tr>
                        <td rowspan="1" style="width: 31px; height: 18px;">
                        </td>
                        <td rowspan="1" style="height: 18px; width: 8px;">
                        </td>
                        <td rowspan="1" style="width: 31px; height: 18px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="60" rowspan="18" style="width: 60px" valign="middle">
                            <img class="compartLink" style="height: 378px" /></td>
                        <td colspan="20" style="height: 10px">
                            <table style="width: 583px; height: 12px;" border="0" cellpadding="0" cellspacing="0" class="tdTopRightBackColor">
                                <tr>
                                    <td colspan="1" rowspan="3" style="width: 9px; height: 7px" align="left" valign="top">
                                        <img alt="" class="imageLeftBack" style="width: 7px; height: 7px; text-align: left" /></td>
                                    <td colspan="3" rowspan="3" style="height: 7px; width: 500px;" align="left" valign="middle">
                                        <asp:Label ID="Label2" runat="server" Font-Size="12pt" ForeColor="White" Height="12px"
                                            Width="385px">合同相关条款修改</asp:Label></td>
                                    <td align="right" colspan="1" rowspan="3" style="width: 19px; height: 7px" valign="top">
                                        <img class="imageRightBack" height="7" style="width: 7px" /></td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                            </table>
                        </td>
                        <td align="center" colspan="100" rowspan="17" style="width: 100px; height: 42px"
                            valign="middle">
                            <img class="compartLink" style="height: 378px" /></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="height: 1px">
                        </td>
                        <td colspan="16" style="width: 20px; height: 1px">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="4" rowspan="1" style="height: 10px" valign="bottom">
                            <table style="width: 565px; height: 1px">
                                <tr>
                                    <td style="width: 109px"><table style="width: 100px; height: 1px">
                                        <tr>
                                            <td colspan="3" rowspan="3">
                                            </td>
                                        </tr>
                                        <tr>
                                        </tr>
                                        <tr>
                                        </tr>
                                    </table>
                                    </td>
                                    <td style="width: 149px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 144px">
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
                                    <td style="width: 4px">
                                        <table style="width: 140px; height: 1px">
                                            <tr>
                                                <td colspan="3" rowspan="3">
                                                </td>
                                            </tr>
                                            <tr>
                                            </tr>
                                            <tr>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="width: 148px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 145px">
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
                            </table>
                        </td>
                        <td class="tdBackColor" colspan="16" rowspan="1" style="width: 20px; height: 10px"
                            valign="bottom">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="1" style="width: 139px; height: 24px;" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblAttr" runat="server" Width="55px" Text="<%$ Resources:BaseInfo,Lease_lblCommOper %>"></asp:Label></td>
                        <td rowspan="1" style="width: 88px; height: 24px;" class="tdBackColor">
                            <asp:TextBox ID="txtAttr" runat="server"></asp:TextBox></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 196px; height: 24px;"
                            valign="middle">
                            <asp:Label ID="lblMonthSettleDays" runat="server" Width="87px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labMonthSettleDays %>"></asp:Label></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 158px;
                            height: 24px" valign="middle">
                            <asp:TextBox ID="txtMonthSettleDays" runat="server"></asp:TextBox></td>
                        <td colspan="16" rowspan="14" style="width: 20px;" class="tdBackColor">
                            </td>
                    </tr>
                    <tr>
                        <td rowspan="1" style="width: 139px; height: 24px;" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblCustomCode" runat="server" Width="55px" Text = "<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                        <td colspan="1" rowspan="1" style="width: 88px; height: 24px;" class="tdBackColor">
                            <asp:TextBox ID="txtCustomCode" runat="server"></asp:TextBox></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 196px; height: 24px;"
                            valign="middle">
                            <asp:Label ID="lblYesNoMin" runat="server" Height="21px" Width="89px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labYesNoMin %>"></asp:Label></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 158px; height: 24px;"
                            valign="middle">
                            <asp:DropDownList ID="ddltYesNoMin" runat="server" Width="157px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td rowspan="1" style="width: 139px; height: 24px;" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblCustomName" runat="server"  Width="55px" Text = "<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                        <td colspan="1" rowspan="1" style="width: 88px; height: 24px;" class="tdBackColor">
                            <asp:TextBox ID="txtCustomName" runat="server"></asp:TextBox></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 196px; height: 24px;"
                            valign="middle">
                            <asp:Label ID="lblFirstAccountMonth" runat="server" Height="21px" Width="89px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labFirstAccountMonth %>"></asp:Label></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 158px; height: 24px;"
                            valign="middle">
                            <asp:TextBox ID="txtFirstAccountMonth" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td rowspan="1" style="width: 139px" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblCustomShortName" runat="server" Width="55px" Text = "<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                        <td colspan="1" rowspan="1" style="width: 88px" class="tdBackColor">
                            <asp:TextBox ID="txtCustomShortName" runat="server"></asp:TextBox></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 196px"
                            valign="middle">
                            <asp:Label ID="lblLatePayInt" runat="server" Width="92px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labLatePayInt %>"></asp:Label></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 158px"
                            valign="middle">
                            <asp:TextBox ID="txtLatePayInt" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="center" class="tdBackColor" rowspan="1" style="width: 158px; height: 5px;" valign="middle">
                        </td>
                        <td class="tdBackColor" colspan="1" rowspan="1" style="width: 158px; height: 5px;" align="center" valign="middle">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 144px">
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
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 158px; height: 5px;"
                            valign="middle">
                        </td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 158px; height: 5px;"
                            valign="middle">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="1" style="width: 139px" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblRefID" runat="server" Width="73px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label></td>
                        <td colspan="1" rowspan="1" style="width: 88px" class="tdBackColor">
                            <asp:TextBox ID="txtRefID" runat="server"></asp:TextBox></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 196px"
                            valign="middle">
                            <asp:Label ID="lblIntDay" runat="server" Height="22px"  Width="92px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labIntDay %>"></asp:Label></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 158px"
                            valign="middle">
                            <asp:TextBox ID="txtIntDay" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td rowspan="1" style="width: 139px; height: 24px;" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblConStartDate" runat="server" Width="78px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
                        <td colspan="1" rowspan="1" style="width: 88px; height: 24px;" class="tdBackColor">
                            <asp:TextBox ID="txtConStartDate" runat="server"></asp:TextBox></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 196px; height: 24px;"
                            valign="middle">
                            <asp:Label ID="lblNote" runat="server" Height="22px"  Width="81px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"></asp:Label></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="3" style="width: 158px"
                            valign="middle">
                            <asp:TextBox ID="lbNote" runat="server" Height="61px"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td rowspan="1" style="width: 139px; height: 24px;" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblConEndDate" runat="server"  Width="78px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
                        <td colspan="1" rowspan="1" style="width: 88px; height: 24px;" class="tdBackColor">
                            <asp:TextBox ID="txtConEndDate" runat="server"></asp:TextBox></td>
                        <td class="tdBackColor" colspan="1" rowspan="1" style="width: 196px; height: 24px;">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="1" style="width: 139px; height: 26px" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblChargeStartD" runat="server" Width="79px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>"></asp:Label></td>
                        <td colspan="1" rowspan="1" style="width: 88px; height: 26px" class="tdBackColor">
                            <asp:TextBox ID="txtChargeStartD" runat="server"></asp:TextBox></td>
                        <td class="tdBackColor" colspan="1" rowspan="1" style="width: 196px; height: 26px">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="tdBackColor" rowspan="1" style="width: 139px; height: 5px"
                            valign="middle">
                        </td>
                        <td class="tdBackColor" colspan="1" rowspan="1" style="width: 88px; height: 5px">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 144px">
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
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 196px;
                            height: 5px">
                        </td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="1" style="width: 158px;
                            height: 5px">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="3" style="width: 139px; height: 24px;" align="center" class="tdBackColor" valign="middle">
                            <asp:Label ID="lblBillCycle" runat="server" Width="55px" Text = "<%$ Resources:BaseInfo,LeaseholdContract_labBillCycle %>"></asp:Label></td>
                        <td colspan="1" rowspan="3" style="width: 88px; height: 24px;" class="tdBackColor">
                            <asp:DropDownList ID="ddltBillCycle" runat="server" Width="156px">
                            </asp:DropDownList></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="3" style="width: 196px; height: 24px;">
                            <asp:Button ID="btnPrev" runat="server" CssClass="buttonEdit" Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>" OnClick="btnPrev_Click" /></td>
                        <td align="center" class="tdBackColor" colspan="1" rowspan="3" style="width: 158px;
                            height: 24px">
                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %>" Enabled="False" /></td>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                    </tr>
                    <tr>
                        <td align="right" class="tdBackColor" style="width: 139px; height: 18px;" valign="middle">
                        </td>
                        <td class="tdBackColor" colspan="1" style="width: 88px; height: 18px;">
                        </td>
                        <td align="center" class="tdBackColor" colspan="1" style="width: 196px; height: 18px;">
                        </td>
                        <td align="center" class="tdBackColor" colspan="1" style="width: 158px; height: 18px">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" rowspan="1" style="height: 10px">
                        </td>
                        <td colspan="16" rowspan="1" style="width: 20px;">
                        </td>
                        <td colspan="1" rowspan="1" style="width: 20px">
                        </td>
                    </tr>                    
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
    </form>
</body>
</html>
