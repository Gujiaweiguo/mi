<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RentLevel.aspx.cs" Inherits="RentableArea_AreaSize_RentLevel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
        <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <table id="showmain" border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle"
                    style="height: 445px">
                    <tr height="15">
                        <td colspan="8">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 95px; height: 401px; text-align: center" valign="top">
                            <img height="401" src="../../images/shuxian.jpg" />
                        </td>
                        <td colspan="5" style="vertical-align: top; width: 572px; height: 401px">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 553px; height: 370px">
                                        <tr>
                                            <td class="tdTopBackColor" style="vertical-align: middle; width: 290px; height: 25px;
                                                text-align: left" valign="top">
                                                <img alt="" class="imageLeftBack" />
                                                <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelAreaLevel %>"></asp:Label></td>
                                            <td class="tdTopRightBackColor" colspan="2" style="width: 538px; height: 25px; text-align: right"
                                                valign="top">
                                                <img alt="" class="imageRightBack" /></td>
                                        </tr>
                                        <tr>
                                            <td colspan="8" style="width: 533px; height: 1px; background-color: white">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" colspan="3" style="vertical-align: top; width: 535px; height: 330px;
                                                text-align: center" valign="top">
                                                <table style="width: 552px">
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="width: 495px; height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblAreaLeveCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblAreaLeveCode %>"
                                                                Width="75px"></asp:Label>&nbsp;</td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtAreaSizeCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblAreaLeveName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblAreaLeveName %>"
                                                                Width="58px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtAreaSizeName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblAreaLeveStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblAreaLeveStatus %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:DropDownList ID="cmbAreaSizeStatus" runat="server" CssClass="cmb160px" Width="165px">
                                                            </asp:DropDownList></td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="width: 495px; height: 12px; text-align: center">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="left: 12px; width: 526px;
                                                                position: relative">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8" style="height: 192px" valign="top">
                                                            &nbsp;<asp:GridView ID="GrdVewCustType" runat="server" AutoGenerateColumns="False"
                                                                BackColor="White" BorderColor="#E1E0B2" OnRowDataBound="GrdVewCustType_RowDataBound"
                                                                OnSelectedIndexChanged="GrdVewCustType_SelectedIndexChanged" PageSize="9" Width="539px">
                                                                <Columns>
                                                                    <asp:BoundField DataField="RentLevelID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RentLevelCode" HeaderText="<%$ Resources:BaseInfo,RentableArea_lblAreaLeveCode %>">
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RentLevelDesc" HeaderText="<%$ Resources:BaseInfo,RentableArea_lblAreaLeveName %>">
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RentLevelStatusName" HeaderText="<%$ Resources:BaseInfo,RentableArea_lblAreaLeveStatus %>">
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Note" HeaderText="<%$ Resources:BaseInfo,User_lblNote %>">
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" SelectText="<%$ Resources:BaseInfo,User_btnChang %>"
                                                                        ShowSelectButton="True">
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:CommandField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="3" style="height: 53px">
                                                            &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px"
                                                                OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="70px" />
                                                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Height="30px" OnClick="btnEdit_Click"
                                                                Text="<%$ Resources:BaseInfo,User_btnChang %>" Width="70px" />&nbsp;
                                                            <asp:Button ID="btnCel" runat="server" CssClass="buttonClear" OnClick="btnCel_Click"
                                                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
                                                        <td class="tdBackColor" style="height: 53px">
                                                        </td>
                                                        <td class="tdBackColor" colspan="4" style="left: 30px; vertical-align: middle; width: 270px;
                                                            height: 53px; text-align: left">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 257px">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="left: 3px; width: 160px; position: relative; top: -5px; height: 1px; background-color: #738495">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="left: 3px; width: 160px; position: relative; top: -5px; height: 1px; background-color: #ffffff">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="height: 30px; text-align: right">
                                                                            <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                                                                Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNext" runat="server"
                                                                                    CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" /></td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 60px; height: 401px; text-align: center" valign="top">
                            <img height="401" src="../../images/shuxian.jpg" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    
    </div>
    </form>
</body>
</html>
