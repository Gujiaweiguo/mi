<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotShopAutiding.aspx.cs" Inherits="Lease_PotCustomer_PotShopAutiding" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Title_PotShop")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
</head>
<body style="margin-top:0; margin-left:0" onload="loadTitle();">
    <form id="form1" runat="server">
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "Title_PotShop")%>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 410px">
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 50%; height: 281px; text-align: right"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 255px; height: 231px">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 9px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 10px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 164px; text-align: center">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCreateUserID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"
                                    Width="50px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCreateUserID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"
                                    Width="63px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"
                                    Width="58px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblContactorName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"
                                    Width="86px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtContactorName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblOfficeTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblOfficeTel %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtOfficeTel" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblMobileTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblMobileTel %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtMobileTel" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="3" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 10px; text-align: center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 168px; text-align: center">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 2px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblBizMode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_BizMode %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="cmbBizMode" runat="server" CssClass="cmb160px" Enabled="False">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblShopType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="cmbShopType" runat="server" CssClass="cmb160px" Enabled="False">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblPotShopName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtPotShopName" runat="server" CssClass="Enabledipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 81px; height: 22px; text-align: right">
                                <asp:Label ID="lblMainBrand" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblMainBrand %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtMainBrand" runat="server" CssClass="Enabledipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="tdBackColor" style="width: 280px; height: 281px" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 280px; height: 231px">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 9px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 10px; text-align: right">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 10px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 10px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 164px; text-align: center">
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="width: 19px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblShopStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"
                                    Width="72px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtShopStartDate" runat="server" CssClass="Enabledipt160px"  ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblShopEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtShopEndDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 14px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblArea %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="cmbArea" runat="server" CssClass="cmb160px" Enabled="False">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblRentalPrice" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblRentalPrice %>"
                                    Width="96px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtRentalPrice" runat="server" CssClass="Enabledipt160px" MaxLength="9" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblRentArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblRentArea %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtRentArea" runat="server" CssClass="Enabledipt160px" MaxLength="13" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 280px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" style="width: 280px; height: 10px; text-align: center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 168px; text-align: center">
                                    <tr>
                                        <td style="left: 42px; width: 324px; position: relative; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="left: 42px; width: 324px; position: relative; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 60px; text-align: right" valign="top">
                                <asp:Label ID="lblNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 60px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 60px" valign="top">
                                <asp:TextBox ID="txtNode" runat="server" CssClass="Enabledipt160px" Height="84px" MaxLength="128"
                                    TextMode="MultiLine" Width="160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 60px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="vertical-align: middle; width: 280px;
                                height: 10px; text-align: center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 168px; text-align: center">
                                    <tr>
                                        <td style="left: 42px; width: 324px; position: relative; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="left: 42px; width: 324px; position: relative; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 96px; height: 22px; text-align: right">
                                <asp:Label ID="lblCommOper" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContractAuditing_labAttract %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtCommOper" runat="server" CssClass="Enabledipt160px" ReadOnly="True"
                                    Width="150px"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 19px; height: 22px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="3" style="width: 535px; height: 49px; text-align: center">
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="3" style="height: 3px; text-align: right">
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>