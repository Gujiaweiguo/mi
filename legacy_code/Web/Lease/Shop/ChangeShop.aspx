<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChangeShop.aspx.cs" Inherits="Lease_Shop_ChangeShop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
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
                    <div id="Div1">
                        <table border="0" cellpadding="0" cellspacing="0" class="baseShop" style="height: 25px"
                            width="710">
                            <tr>
                                <td class="tdTopBackColor" style="width: 120px; height: 25px" valign="top">
                                    <img alt="" class="imageLeftBack" />
                                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,LeaseBargain_lblWrite %>"
                                        Width="98px"></asp:Label></td>
                                <td class="tdTopRightBackColor" colspan="2" style="width: 628px; height: 25px; text-align: right"
                                    valign="top">
                                    <img class="imageRightBack" /></td>
                            </tr>
                            <tr class="headLine">
                                <td class="tdBackColor" colspan="4" style="background-color: white">
                                </td>
                            </tr>
                        </table>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 710px; height: 357px">
                            <tr>
                                <td class="tdBackColor" style="width: 30%" valign="top">
                                    <table width="100%">
                                        <tr style="height: 25px">
                                            <td colspan="3" style="text-align: center">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 182px">
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
                                            <td colspan="3" style="text-align: center">
                                                <asp:GridView ID="gvShop" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    OnSelectedIndexChanged="gvShop_SelectedIndexChanged" Width="181px" Height="260px">
                                                    <Columns>
                                                        <asp:BoundField DataField="ShopId">
                                                            <ItemStyle CssClass="hidden" />
                                                            <HeaderStyle CssClass="hidden" />
                                                            <FooterStyle CssClass="hidden" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ShopName" HeaderText="商铺名称">
                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ShopType" HeaderText="商铺类型">
                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                        </asp:BoundField>
                                                        <asp:CommandField HeaderText="选择" ShowSelectButton="True">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                        </asp:CommandField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="tdBackColor" style="width: 30%; text-align: right" valign="top">
                                    <table class="tdBackColor" style="width: 238px">
                                        <tr style="height: 15px">
                                            <td style="width: 2239px">
                                            </td>
                                            <td style="width: 329px" valign="bottom">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 136px">
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
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 329px">
                                                <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt130px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label50" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 329px; height: 21px">
                                                <asp:TextBox ID="txtShopName" runat="server" CssClass="ipt130px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label53" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:DropDownList ID="DDownListShopType" runat="server" Width="133px">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label18" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:TextBox ID="txtRentArea" runat="server" CssClass="ipt130px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label57" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaName %>"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 329px; height: 23px">
                                                <asp:DropDownList ID="DDownListAreaName" runat="server" Width="134px">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label60" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 329px; height: 21px">
                                                <asp:TextBox ID="txtStartDate" runat="server" BackColor="#F5F5F4" CssClass="Enabledipt130px"
                                                    onclick="calendar()" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label61" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LblDate_EndDate %>"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:TextBox ID="txtEndDate" runat="server" BackColor="#F5F5F4" CssClass="Enabledipt130px"
                                                    onclick="calendar()" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label63" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:TextBox ID="txtContactName" runat="server" CssClass="ipt130px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="width: 2239px; text-align: right">
                                                <asp:Label ID="Label64" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblTel %>"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:TextBox ID="txtContactTel" runat="server" CssClass="ipt130px"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </td>
                                <td class="tdBackColor" style="width: 40%" valign="top">
                                    <table class="tdBackColor">
                                        <tr style="height: 15px">
                                            <td style="width: 932px">
                                            </td>
                                            <td style="width: 347px" valign="bottom">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 134px">
                                                    <tr class="bodyLine">
                                                        <td class="tdBackColor" style="height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr class="bodyLine">
                                                        <td class="tdBackColor" style="height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="text-align: right; width: 932px;">
                                                <asp:Label ID="Label56" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblMainBrand %>"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:DropDownList ID="DDownListBrand" runat="server" Width="134px" OnSelectedIndexChanged="DDownListBrand_SelectedIndexChanged">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="text-align: right; width: 932px;">
                                                <asp:Label ID="Label27" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuilding %>"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:DropDownList ID="DDownListBuilding" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDownListBuilding_SelectedIndexChanged"
                                                    Width="133px">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="text-align: right; width: 932px;">
                                                <asp:Label ID="Label28" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 347px; height: 21px">
                                                <asp:DropDownList ID="DDownListFloors" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDownListFloors_SelectedIndexChanged"
                                                    Width="133px">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="text-align: right; width: 932px;">
                                                <asp:Label ID="Label29" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationName %>"></asp:Label></td>
                                            <td class="tdBackColor">
                                                <asp:DropDownList ID="DDownListLocation" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDownListLocation_SelectedIndexChanged"
                                                    Width="133px">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="text-align: right; width: 932px;">
                                                <asp:Label ID="Label30" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblSelectUnit %>"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 347px; height: 23px">
                                                <asp:DropDownList ID="DDownListUnit" runat="server" Width="133px" OnSelectedIndexChanged="DDownListUnit_SelectedIndexChanged">
                                                </asp:DropDownList></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="text-align: right; width: 932px;">
                                            </td>
                                            <td class="tdBackColor">
                                                <asp:Button ID="IBtnUnitsDel" runat="server" CssClass="buttonClear" Height="31px"
                                                    OnClick="IBtnUnitsDel_Click" Text="<%$ Resources:BaseInfo,Btn_Del %>" />
                                                <asp:Button ID="IBtnUnitsAdd" runat="server" CssClass="buttonSave" Height="31px"
                                                    OnClick="IBtnUnitsAdd_Click" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="text-align: right; width: 932px;">
                                                <asp:Label ID="Label34" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblNoeUnitCollect %>"></asp:Label></td>
                                            <td class="tdBackColor" rowspan="3" style="width: 347px">
                                                <asp:ListBox ID="ListBoxUnits" runat="server" Height="94px" Width="137px" CssClass="OpenColor"></asp:ListBox></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="text-align: right; width: 932px;">
                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblComm %>" CssClass="labelStyle" Width="72px"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td class="tdBackColor" style="height: 31px; text-align: right; width: 932px;">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height: 15px">
                                <td class="tdBackColor" style="width: 30%" valign="top">
                                </td>
                                <td class="tdBackColor" colspan="2" style="text-align: center" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 95%">
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
                            <tr style="height: 35px" valign="top">
                                <td class="tdBackColor" style="width: 30%" valign="top">
                                </td>
                                <td class="tdBackColor" style="width: 30%; text-align: right" valign="top">
                                </td>
                                <td class="tdBackColor" style="padding-right: 20px; width: 40%; text-align: right"
                                    valign="top">
                                    <asp:Button ID="btnShopSave" runat="server" CssClass="buttonCancel" Height="30px"
                                        OnClick="btnShopSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" /></td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="width: 60px; height: 401px; text-align: center" valign="top">
                    <img height="401" src="../../images/shuxian.jpg" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
