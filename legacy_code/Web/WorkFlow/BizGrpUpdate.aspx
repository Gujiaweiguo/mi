<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BizGrpUpdate.aspx.cs" Inherits="WorkFlow_BizGrpUpdate"  %>



<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <table border="0" style="width: 100%; height: 1px">
            <tr>
                <td style="width: 699px; height: 50px">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left" rowspan="2" style="width: 40px" valign="middle">
                                <img alt="" height="32" src="../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
                            <td align="left" class="workAreaMainTitle" style="height: 20px" valign="middle">
                                <asp:Label ID="lblBizGrp" runat="server" Text='<%$ Resources:BaseInfo,WrkFlw_lblBizGrp %>' ToolTip="为系统增加新的业务组定义"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="left" class="workAreaMainTitleMemo" valign="middle">
                            </td>
                        </tr>
                    </table>
                    <hr style="width: 100%; size: 1" />
                </td>
            </tr>
            <tr>
                <td style="width: 699px; height: 337px" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 74px">
                        <tr>
                            <td align="left" style="width: 50%; height: 75px" valign="top">
                                <fieldset>
                                    <legend class="workAreaFieldsetTitle">基础信息修改</legend>
                                    <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                        <tr>
                                            <td style="width: 644px; height: 80px">
                                                <table border="0" width="100%">
                                                    <tr>
                                                        <td align="center" style="width: 63px; height: 25px">
                                                            <asp:Label ID="lblBizGrpName" runat="server" Text='<%$ Resources:BaseInfo,WrkFlw_lblBizGrpName %>' CssClass="label"></asp:Label></td>
                                                        <td align="left" style="width: 518px; color: #000000; height: 25px">
                                                            <asp:TextBox ID="txtBizGrpName" runat="server" CssClass="ipt160px"
                                                                MaxLength="32"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                                                    runat="server" ControlToValidate="txtBizGrpName" ErrorMessage="*" Width="4px"></asp:RequiredFieldValidator>
                                                            </td>
                                                    </tr>
                                                    <tr style="color: #000000">
                                                        <td align="center" style="width: 63px; height: 22px">
                                                            <asp:Label ID="lblBizGrpStatus" runat="server" Text='<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>' CssClass="label"></asp:Label></td>
                                                        <td align="left" style="width: 518px; height: 22px">
                                                            <asp:DropDownList ID="cmbBizGrpStatus" runat="server" BackColor="White" CssClass="ipt160px">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                </table>
                                                <div>
                                                    &nbsp;</div>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="修改" CssClass="btnOn50px" /></fieldset>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>