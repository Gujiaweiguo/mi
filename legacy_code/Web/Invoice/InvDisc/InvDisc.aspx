<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvDisc.aspx.cs" Inherits="Invoice_InvDisc_InvDisc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "InvDiscDet_InvDiscDe")%></title>
        <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/setPeriod.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	function hidden()
	{
		document.getElementById("lblTotalNum").style.display="none";
        document.getElementById("lblCurrent").style.display="none";
        var str= document.getElementById("InvDiscDet_InvDiscDe").value + ",Invoice/InvDisc/InvDisc.aspx";
        
        addTabTool(str);
        loadTitle();
    }
    function BillOfDocumentDelete()
    {
        return window.confirm('<%= billOfDocumentDelete %>');
    }
	</script>
</head>
<body onload='hidden();' topmargin=0 leftmargin=0 >
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
                            <div id="showLeaseBargain" style="width:98%">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 420px; width: 100%; text-align:center">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 833px; height: 22px; text-align: left">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,InvDiscDet_InvDiscDe %>" Width="281px"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                    <tr style="width:100%">
                                        <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px; text-align: center;" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 45px;">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 635px; height: 15px">
                                                    <table style="width: 652px">
                                                    <tr>
                                                    <td style="width: 87px; height: 32px; text-align: right">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingCode %>"></asp:Label></td>
                                                    <td style="width: 3px; height: 32px">
                                                        &nbsp;</td>
                                                    <td style="height: 32px; width: 169px;">
                                                        <asp:DropDownList ID="cmbBuildingID" runat="server" AutoPostBack="True" Width="165px" OnSelectedIndexChanged="cmbBuildingID_SelectedIndexChanged">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 5px; text-align: right; height: 32px;">
                                                        </td>
                                                    <td style="width: 73px; height: 32px; text-align: right">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelFloorID %>"></asp:Label></td>
                                                    <td style="width: 3px; height: 32px">
                                                        &nbsp;</td>
                                                    <td style="height: 32px; width: 169px;">
                                                        <asp:DropDownList ID="cmbFloorID" runat="server" Width="165px" Enabled="False">
                                                        </asp:DropDownList></td>
                                                    <td colspan="3" style="height: 32px">
                                                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                            Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                    </tr>
                                                    
                                                    <tr>
                                                    <td style="width: 87px; height: 17px; text-align: right">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"></asp:Label></td>
                                                    <td style="width: 3px; height: 17px">
                                                        &nbsp;</td>
                                                    <td style="height: 17px; width: 169px;">
                                                        <asp:DropDownList ID="cmbTradeID" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 5px; text-align: right;">
                                                        </td>
                                                   <td style="width: 73px; height: 17px; text-align: right">
                                                                    <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvAdj_KeepAccountsMth %>"
                                                                        Width="62px"></asp:Label></td>
                                                    <td style="width: 3px; height: 17px">
                                                        &nbsp;</td>
                                                    <td style="height: 17px; width: 169px;">
                                                        <asp:TextBox ID="txtInvPeriod" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                    <td colspan="3">
                                                        <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="true" Visible="False"  /></td>
                                                    </tr> 
                                                    
                                                    
                                                    <tr>
                                                    <td style="width: 87px; height: 17px; text-align: right">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ChargeType_lblChargeTypeName %>"></asp:Label></td>
                                                    <td style="width: 3px; height: 17px">
                                                        &nbsp;</td>
                                                    <td style="height: 17px; width: 169px;">
                                                        <asp:DropDownList ID="cmbChargeType" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                    </tr>                                                   
                                                    </table>
                                                    
                                                        </td>
                                                </tr>
                                                <tr>
                                                <td style="height: 5px; text-align: center; width:100%" colspan="2">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                                        <tr class="bodyLine">
                                                            <td style="height: 1px; background-color: #738495">
                                                            </td>
                                                        </tr>
                                                        <tr class="bodyLine">
                                                            <td style="height: 1px; background-color: #ffffff">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 50%; height: 295px;">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 100%; height: 275px; text-align:left">
                                                            <tr style="height: 5px">
                                                                <td style="width: 120px; height: 5px;">
                                                                </td>
                                                                <td style="width: 168px; height: 5px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 120px; height: 26px;">
                                                                    &nbsp;<asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>" Width="72px"></asp:Label></td>
                                                                <td style="height: 26px; width: 168px;">
                                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 26px; text-align: center; width: 120px;">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>" Width="76px"></asp:Label></td>
                                                                <td style="height: 26px; width: 168px;">
                                                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td style="width: 120px; height: 30px; text-align: center;">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>" Width="76px"></asp:Label></td>
                                                                <td style="width: 168px; vertical-align: middle; height: 30px;">
                                                                    <asp:TextBox ID="txtContractID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 28px; text-align: center; width: 120px;">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"
                                                                        Width="79px"></asp:Label></td>
                                                                <td style="height: 28px; width: 168px;">
                                                                    <asp:TextBox ID="txtShopName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 10px; text-align: right; width: 120px;">
                                                                    &nbsp;</td>
                                                                <td style="height: 10px; width: 168px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #738495">
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #ffffff">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 120px; height: 26px;">
                                                                    <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvDiscDet_DiscDetRatio %>" Width="81px"></asp:Label></td>
                                                                <td style="height: 26px; width: 168px;">
                                                                    <asp:TextBox ID="txtDiscRate" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 120px; height: 65px;" valign="middle">
                                                                    <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvDiscDet_DiscDetCausation %>" Width="81px"></asp:Label></td>
                                                                <td style="width: 168px;" rowspan="3" valign="middle">
                                                                    <asp:TextBox ID="txtDiscReason" runat="server" CssClass="ipt160px" Height="32px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" rowspan="2" style="vertical-align: middle; width: 120px; text-align: right; height: 11px;">
                                                                </td>
                                                            </tr>
                                                            </tr>
                                                                                                                        <tr>
                                                                <td class="baseLable" style="text-align: right; vertical-align: middle; height: 20px;" colspan="2">
                                                                    <br />
                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"
                                                                    OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Visible="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                        <asp:Button ID="btnPutIn" runat="server" CssClass="buttonPutIn" OnClick="butAuditing_Click"
                                                            Text="<%$ Resources:BaseInfo,ConLease_butAddAll %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                                <asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear" Enabled="False" OnClick="btnBalnkOut_Click" Text="<%$ Resources:BaseInfo,ConLease_butDel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        </td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; text-align: center; height: 295px;" width="100%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 100%; height: 134px" id="">
                                                            <tr style="height: 5px">
                                                                <td class="baseInput" colspan="2" style="height: 5px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseInput" colspan="2" rowspan="3" style="vertical-align: top; text-align: center; height: 110px; width:100%">
                                                                    <asp:GridView ID="GrdVewInvoiceDetail" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                        BorderStyle="Inset" BorderWidth="1px" Height="235px" Width="100%" PageSize="9" OnRowDataBound="GrdVewInvoiceDetail_RowDataBound" OnSelectedIndexChanged="GrdVewInvoiceDetail_SelectedIndexChanged">
                                                                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="InvDetailID">
                                                                            <ItemStyle CssClass="hidden" />
                                                                            <HeaderStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                            </ItemTemplate>
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField DataField="invCode" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvDate" HeaderText="<%$ Resources:BaseInfo,Rpt_InvDate %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvActPayAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayAmt %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvPaidAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_InvPaidAmt %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />                                                                        
                                                                        </asp:CommandField>
                                                                        <asp:BoundField DataField="InvExRate">
                                                                            <ItemStyle CssClass="hidden" />
                                                                            <HeaderStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                        <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                                        <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                        <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left" />
                                                                        <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                            <td style="width: 1px; height: 119px;">
                                                            </td>
                                                            </tr>

                                                        </table>
                                                        <br />
                                                        <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" OnClick="btnBack_Click"
                                                            Text="<%$ Resources:BaseInfo,Button_back %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                        <asp:Button ID="btnNext" runat="server"
                                                                CssClass="buttonNext" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/><asp:Label
                                                                    ID="lblTotalNum" runat="server" CssClass="hidden" Height="9px" Width="62px"></asp:Label><asp:Label
                                                                        ID="lblCurrent" runat="server" CssClass="hidden" ForeColor="Red" Height="9px"
                                                                        Width="1px">1</asp:Label></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                 <asp:HiddenField ID="InvDiscDet_InvDiscDe" runat="server" Value="<%$ Resources:BaseInfo,InvDiscDet_InvDiscDe %>"/>
                            </div>
    
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>