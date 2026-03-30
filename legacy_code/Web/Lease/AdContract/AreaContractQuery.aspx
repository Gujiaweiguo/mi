<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AreaContractQuery.aspx.cs" Inherits="Lease_AdContract_AreaContractQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
        <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <asp:HiddenField ID="depttxt" runat="server" />
                    <asp:HiddenField ID="deptid" runat="server" />
                    <asp:HiddenField ID="selectdeptid" runat="server" />
                    <table border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle" style="height: 445px">
                        <tr height="5">
                            <td colspan="8" style="height: 15px">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 60px; height: 401px; text-align: center" valign="bottom">
                                <img height="401" src="../../images/shuxian.jpg" />
                            </td>
                            <td style="vertical-align: top; width: 310px; height: 401px; text-align: right">
                                <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: top; height: 255px"
                                    width="280">
                                    <tr>
                                        <td class="tdTopBackColor" colspan="2" style="width: 266px; height: 27px">
                                            <img alt="" class="imageLeftBack" /><asp:Label ID="labUnitTitle" runat="server" CssClass="lblTitle"
                                                Text="<%$ Resources:BaseInfo,RentableArea_labUnitTitle %>"></asp:Label></td>
                                        <td class="tdTopRightBackColor" colspan="2" style="height: 27px" valign="top">
                                            &nbsp;<img class="imageRightBack" /></td>
                                    </tr>
                                    <tr height="1">
                                        <td colspan="5" style="height: 1px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" rowspan="10" style="height: 360px; text-align: center"
                                            valign="top">
                                            <table style="height: 300px">
                                                <tr>
                                                    <td style="height: 10px">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 253px">
                                                            <tr>
                                                                <td style="width: 160px; position: relative; top: 3px; height: 1px; background-color: #738495">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 160px; position: relative; top: 3px; height: 1px; background-color: #ffffff">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:RadioButton ID="rbtnNoLeaseOut" runat="server" AutoPostBack="True" Checked="True"
                                                            CssClass="rbtn" Font-Size="10.5pt" GroupName="area" OnCheckedChanged="rbtnNoLeaseOut_CheckedChanged"
                                                            Text="<%$ Resources:BaseInfo,LeaseAreaType_NoStop %>" />
                                                        <asp:RadioButton ID="rbtnLeaseOut" runat="server" AutoPostBack="True" CssClass="rbtn"
                                                            Font-Size="10.5pt" GroupName="area" OnCheckedChanged="rbtnLeaseOut_CheckedChanged"
                                                            Text="<%$ Resources:BaseInfo,LeaseAreaType_Stop %>" /></td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 275px">
                                                        <asp:Panel ID="Panel1" runat="server" BackColor="White" BorderStyle="Inset" BorderWidth="1px"
                                                            Font-Size="Medium" Height="245px" HorizontalAlign="Left" ScrollBars="Auto" Width="260px">
                                                            <table>
                                                                <tr>
                                                                    <td id="treeview" style="width: 207px; height: 219px" valign="top">
                                                                        &nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 20px">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 253px">
                                                            <tr>
                                                                <td style="width: 160px; position: relative; top: 3px; height: 1px; background-color: #738495">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 160px; position: relative; top: 3px; height: 1px; background-color: #ffffff">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Button ID="treeClick" runat="server" CssClass="buttonHidden" Height="1px" OnClick="treeClick_Click"
                                                Width="24px" /></td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 5px; height: 401px">
                            </td>
                            <td colspan="3" style="vertical-align: top; width: 315px; height: 401px">
                                <table border="0" cellpadding="0" cellspacing="0" style="vertical-align: top; height: 288px"
                                    width="280">
                                    <tr>
                                        <td style="vertical-align: top; width: 247px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 279px; height: 22px">
                                                <tr>
                                                    <td class="tdTopRightBackColor" style="width: 8px; height: 27px; text-align: left"
                                                        valign="top">
                                                        <img alt="" class="imageLeftBack" style="text-align: left" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="vertical-align: bottom; width: 251px; height: 27px;
                                                        text-align: left">
                                                        <asp:Label ID="lblUnitit" runat="server" Height="12pt" Text="<%$ Resources:BaseInfo,RentableArea_lblUnit %>"
                                                            Width="218px"></asp:Label></td>
                                                    <td class="tdTopRightBackColor" style="width: 20px; height: 27px; text-align: right"
                                                        valign="top">
                                                        <img class="imageRightBack" style="width: 7px; height: 22px" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr height="1">
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" rowspan="10" style="height: 341px; text-align: center"
                                            valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 265px; height: 273px">
                                                <tr>
                                                    <td colspan="3" style="vertical-align: middle; height: 20px; text-align: center">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 243px">
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
                                                    <td style="width: 224px; height: 25px; text-align: right">
                                                        <asp:Label ID="lblUnitCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitCode %>"></asp:Label>&nbsp;</td>
                                                    <td style="height: 25px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:TextBox ID="txtUnitCode" runat="server" CssClass="Enabledipt150px" ReadOnly="True"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 26px; text-align: right">
                                                        <asp:Label ID="lblAreaType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_AreaType %>"
                                                            Width="71px"></asp:Label>&nbsp;</td>
                                                    <td style="height: 26px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 26px; text-align: left">
                                                        <asp:DropDownList ID="cmbAreaType" runat="server" CssClass="cmb160px" Enabled="False"
                                                            Width="155px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 22px; text-align: right">
                                                        <asp:Label ID="lblAdContractCode" runat="server" CssClass="labelStyle" Height="18px"
                                                            Text="<%$ Resources:BaseInfo,AdContract_lblAdContractCode %>" Width="88px"></asp:Label>&nbsp;</td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:TextBox ID="txtAdContractCode" runat="server" CssClass="Enabledipt150px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 24px; text-align: right">
                                                        <asp:Label ID="lblStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblStartDate %>"
                                                            Width="85px"></asp:Label>&nbsp;</td>
                                                    <td style="height: 24px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="Enabledipt150px" onclick="calendar()"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 26px; text-align: right">
                                                        <asp:Label ID="lblEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblEndDate %>"
                                                            Width="88px"></asp:Label></td>
                                                    <td style="height: 26px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 26px; text-align: left">
                                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="Enabledipt150px" onclick="calendar()"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 22px; text-align: right">
                                                        <asp:Label ID="lblTotalMoney" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblTotalMoney %>"
                                                            Width="86px"></asp:Label></td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:TextBox ID="txtTotalMoney" runat="server" CssClass="Enabledipt150px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 25px; text-align: right">
                                                        <asp:Label ID="lblPrepayment" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblPrepayment %>"
                                                            Width="85px"></asp:Label></td>
                                                    <td style="height: 25px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:TextBox ID="txtPrepayment" runat="server" CssClass="Enabledipt150px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 22px; text-align: right">
                                                        <asp:Label ID="lblPayingCycle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblPayingCycle %>"></asp:Label></td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:DropDownList ID="cmbPayingCycle" runat="server" CssClass="cmb150px" Enabled="False"
                                                            Width="155px">
                                                        </asp:DropDownList></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 22px; text-align: right">
                                                        <asp:Label ID="lblPaymentPerCycle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblPaymentPerCycle %>"></asp:Label></td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:TextBox ID="txtPaymentPerCycle" runat="server" CssClass="Enabledipt150px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 25px; text-align: right">
                                                        <asp:Label ID="lblLastPaymentDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblLastPaymentDate %>"
                                                            Width="84px"></asp:Label></td>
                                                    <td style="height: 25px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:TextBox ID="txtLastPaymentDate" runat="server" CssClass="Enabledipt150px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 224px; height: 22px; text-align: right">
                                                        <asp:Label ID="lblAreaMaker" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_lblAreaMaker %>"
                                                            Width="86px"></asp:Label></td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                    <td style="width: 195px; height: 25px; text-align: left">
                                                        <asp:TextBox ID="txtAreaMaker" runat="server" CssClass="Enabledipt150px"></asp:TextBox></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="vertical-align: top; height: 22px; text-align: center">
                                                        &nbsp;<table border="0" cellpadding="0" cellspacing="0" style="width: 243px">
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
                                                    <td colspan="3" style="height: 15px; text-align: right">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3" style="height: 10px; text-align: left">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 60px; height: 401px; text-align: center" valign="middle">
                                <img height="401" src="../../images/shuxian.jpg" /></td>
                        </tr>
                    </table>
                </div>
                <asp:HiddenField ID="hidSelectLocation" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelectLocation %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
