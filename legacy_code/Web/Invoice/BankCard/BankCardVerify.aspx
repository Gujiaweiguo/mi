<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BankCardVerify.aspx.cs" Inherits="Invoice_BankCard_BankCardVerify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "BankCard_BankCardVerify")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>

    <script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>

    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>

    <script type="text/javascript" src="../../JavaScript/Common.js"> </script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
    function Load()
    {
        addTabTool("<%=baseInfo %>,Invoice/BankCard/BankCardVerify.aspx");
        loadTitle();
    }
    </script>
</head>
<body topmargin="0" leftmargin="0" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                        <div id="showLeaseBargain">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 401px; width:100%;
                                text-align: center;">
                                <tr>
                                    <td class="tdTopRightBackColor" style="width: 404px; height: 22px; vertical-align: top;
                                        text-align: left;">
                                        <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,BankCard_BankCardVerify %>" Width="319px"></asp:Label></td>
                                    <td class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                    </td>
                                    <td class="tdTopRightBackColor" style="width: 7px; height: 22px; vertical-align: top;
                                        text-align: right;" valign="top">
                                        <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px; text-align: center;"
                                        valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" style="height: 380px; width: 631px;">
                                            <tr class="headLine">
                                                <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdBackColor" colspan="4" style="width: 635px; height: 20px; vertical-align: middle;
                                                    top: 20px; text-align: center;">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 652px">
                                                        <tr style="height: 1px">
                                                            <td style="width: 160px; height: 1px; background-color: #738495">
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 1px">
                                                            <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px; text-align: center; vertical-align: top; width: 537px;" colspan="2">
                                                    <table border="0" cellpadding="0" cellspacing="0" class="payPutOut" style="width: 95%;
                                                        height: 269px;">
                                                        <tr class="rowHeight">
                                                            <td class="baseLable" style="width: 131px; height: 30px">
                                                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,BankCard_BankTransTimeBagin %>"
                                                                    Width="86px"></asp:Label></td>
                                                            <td style="width: 158px; height: 30px">
                                                                <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                            <td class="baseLable" style="width: 16px; height: 30px; text-align: right;">
                                                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,BankCard_BankTransTimeEnd %>"
                                                                    Width="84px"></asp:Label></td>
                                                            <td style="width: 191px; height: 30px">
                                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                            <td style="height: 30px; width: 73px;">
                                                                </td>
                                                        </tr>
                                                        <tr class="rowHeight">
                                                            <td class="baseLable" style="width: 131px; height: 30px">
                                                            </td>
                                                            <td style="width: 158px; height: 30px">
                                                                <asp:RadioButtonList ID="RadioBank" runat="server"
                                                                    RepeatDirection="Horizontal" AutoPostBack="True">
                                                                    <asp:ListItem Selected="True" Value="014">兴业银行</asp:ListItem>
                                                                    <asp:ListItem Value="851">建设银行</asp:ListItem>
                                                                </asp:RadioButtonList></td>
                                                            <td class="baseLable" style="width: 16px; height: 30px; text-align: right">
                                                            </td>
                                                            <td style="width: 191px; height: 30px">
                                                                <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                                    Text="<%$ Resources:BaseInfo,BankCardVerify_BtnBegin %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                            <td style="width: 73px; height: 30px">
                                                            </td>
                                                        </tr>
                                                        <tr class="rowHeight">
                                                            <td align="center" colspan="5" style="text-align: center; vertical-align: middle;
                                                                height: 15px;">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 652px">
                                                                    <tr style="height: 1px">
                                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 1px">
                                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="5" rowspan="10" style="padding-right: 15px; height: 54px; text-align: center">
                                                                <table style="width: 101%">
                                                                    <tr>
                                                                        <td style="text-align: center">
                                                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,BankCard_btnTransmitBank %>"></asp:Label></td>
                                                                        <td>
                                                                        </td>
                                                                        <td style="text-align: center">
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,BankCard_btnTransmitPos %>"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 315px; height: 275px">
                                                                            <asp:GridView ID="GrdBank" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="258px" Width="307px" OnRowDataBound="GrdBank_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdBank_PageIndexChanging" PageSize="9">
                                                                                <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="BankEFTID" HeaderText="<%$ Resources:BaseInfo,BankCard_BankEFTID %>">
                                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="BankCardID" HeaderText="<%$ Resources:BaseInfo,BankCard_BankCard %>">
                                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="BankTransTime" HeaderText="<%$ Resources:BaseInfo,BankCard_BankTransTime %>">
                                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="BankAmt" HeaderText="<%$ Resources:BaseInfo,BankCard_BankAmt %>">
                                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                    </asp:BoundField>
                                                                                </Columns>
                                                                                <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                                                <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                        <td style="width: 9px; height: 275px">
                                                                        </td>
                                                                        <td style="height: 275px; text-align: left">
                                                                            <asp:GridView ID="GrdPOS" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                                BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="258px" Width="307px" OnRowDataBound="GrdPOS_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdPOS_PageIndexChanging">
                                                                                <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="EFTID" HeaderText="<%$ Resources:BaseInfo,BankCard_BankEFTID %>">
                                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="CardID" HeaderText="<%$ Resources:BaseInfo,BankCard_BankCard %>">
                                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="BizDate" HeaderText="<%$ Resources:BaseInfo,BankCard_BankTransTime %>" DataFormatString="{0:d}" HtmlEncode="False">
                                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                    </asp:BoundField>
                                                                                    <asp:BoundField DataField="Amt" HeaderText="<%$ Resources:BaseInfo,BankCard_BankAmt %>">
                                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                    </asp:BoundField>
                                                                                </Columns>
                                                                                <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                                                <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,BankCard_Disposal %>"></asp:Label><asp:Label ID="lblLog" runat="server" Text=""></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                        </tr>
                                                        <tr class="rowHeight">
                                                        </tr>
                                                        <tr class="rowHeight">
                                                        </tr>
                                                        <tr class="rowHeight">
                                                        </tr>
                                                        <tr class="rowHeight">
                                                        </tr>
                                                        <tr class="rowHeight">
                                                        </tr>
                                                        <tr class="rowHeight">
                                                        </tr>
                                                        <tr class="rowHeight">
                                                        </tr>
                                                        <tr style="height: 45px">
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
    </form>
</body>
</html>
