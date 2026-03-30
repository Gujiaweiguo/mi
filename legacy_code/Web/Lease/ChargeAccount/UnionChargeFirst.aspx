<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnionChargeFirst.aspx.cs" Inherits="Lease_ChargeAccount_UnionChargeFirst" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Login.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin:0px">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%" border="0" cellpadding="0" cellspacing="0" class="tdBackColor">
            <tr>
                <td style="width: 8px; text-align:left" class="tdTopRightBackColor">
                <img alt="" class="imageLeftBack" style=" text-align:left"  />
                </td>
                <td class="tdTopRightBackColor" style="text-align:left;">
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Lease_UnionCount %>" Height="12pt" Width="218px"></asp:Label>
                </td>
                <td class="tdTopRightBackColor">
                    &nbsp;</td>
                <td class="tdTopRightBackColor">
                <img class="imageRightBack" style="width: 7px; height: 22px" />
                </td>
                
            </tr>
            <tr style="height:15px">
                <td colspan=4></td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="3">
        <asp:GridView id="GVCust" runat="server" BorderColor="#E1E0B2" BackColor="White" AutoGenerateColumns="False" Width="100%">
            <Columns>
                <asp:BoundField HeaderText="<%$ Resources:BaseInfo,Account_lblChargeType %>" DataField="ChargeTypeName">
                    <HeaderStyle BackColor="#E1E0B2"  />
                    <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>" DataField="InvStartDate" >
                    <ItemStyle BorderColor="#E1E0B2"  />
                    <HeaderStyle BackColor="#E1E0B2"  />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labChargeEndDate %>" DataField="InvEndDate" >
                    <ItemStyle BorderColor="#E1E0B2"  />
                    <HeaderStyle BackColor="#E1E0B2"  />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:BaseInfo,Account_lblChargeMoney %>" DataField="InvActPayAmtL" >
                    <ItemStyle BorderColor="#E1E0B2"  />
                    <HeaderStyle BackColor="#E1E0B2"  />
                </asp:BoundField>
            </Columns>
        </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td align="right" style="position:relative; top:8px;">
                    <asp:Button id="btnFirstCharge" onclick="btnFirstCharge_Click" runat="server" 
                        Text="<%$ Resources:BaseInfo,Lease_FirstInvoice %>" CssClass="buttonSave"></asp:Button>
                    &nbsp;<asp:Button ID="btnOK" runat="server" CssClass="buttonCancel" OnClick="btnOK_Click"
                        Text="<%$ Resources:BaseInfo,User_btnOk %> " />&nbsp; <asp:Button ID="BtnCel" runat="server"
                            CssClass="buttonClear" OnClick="BtnCel_Click" 
                        Text="<%$ Resources:BaseInfo,User_btnCancel %> " /></td>
                <td colspan="2">
                </td>
            </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td align="right">
                        &nbsp;</td>
                    <td colspan="2">
                        &nbsp;</td>
                </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
