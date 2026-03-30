<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GenerateInitialFees.aspx.cs" Inherits="Lease_AuditingLease_GenerateInitialFees" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Login.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    function BtnUp( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
    }
    function BtnOver( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
    }
    </script>
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
                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblLeaseAccount %>" Height="12pt" Width="218px"></asp:Label>
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
                <td style="height: 34px">
                </td>
                <td align="right" style="height: 34px">
                    <br />
                    <asp:Button id="btnSave" onclick="btnFirstCharge_Click" runat="server" Text="<%$ Resources:BaseInfo,Lease_FirstInvoice %>" CssClass="buttonSave" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                    <asp:Button ID="btnOk" runat="server" CssClass="buttonOk" OnClick="btnOK_Click"
                        Text="<%$ Resources:BaseInfo,User_btnOk %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                        <asp:Button ID="BtnCancel" runat="server"
                            CssClass="buttonCancel" OnClick="BtnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/><br />
                    &nbsp;&nbsp;
                </td>
                <td colspan="2" style="height: 34px">
                </td>
            </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
