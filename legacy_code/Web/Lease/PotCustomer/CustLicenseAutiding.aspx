<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustLicenseAutiding.aspx.cs" Inherits="Lease_PotCustomer_CustLicenseAutiding" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ClientCard")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
		<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
		<script type="text/javascript">
		function Load()
		{
		    loadTitle();
//		    document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
		}
		</script>
</head>
<body style="margin-top:0; margin-left:0" onload="Load();">
    <form id="form1" runat="server">
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "Title_CustLicense")%>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 405px">
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 50%; height: 320px; text-align: right"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 216px" width="255">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px" valign="bottom">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px; text-align: center"
                                valign="bottom">
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
                            <td class="tdBackColor" style="width: 84px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 4px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px" valign="bottom">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseCode %>"
                                    Width="69px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtLicenseID" runat="server" CssClass="Enabledipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtLicenseName" runat="server" CssClass="Enabledipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseType %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:DropDownList ID="cmbLicenseType" runat="server" CssClass="ipt160px" Enabled="False">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtLicenseBeginDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtLicenseEndDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 22px; text-align: center">
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
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 23px; text-align: center">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="tdBackColor" style="width: 280px; height: 330px" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 286px; height: 311px">
                        <tr>
                            <td class="tdBackColor" style="width: 302px; height: 22px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 275px; left: 5px; position: relative; top: 0px;">
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
                            <td class="tdBackColor" style="vertical-align: top; width: 302px; height: 306px;
                                text-align: center" valign="top" rowspan="2">
                                <asp:GridView ID="GrdCustLicense" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="95%" OnRowDataBound="GrdCustLicense_RowDataBound"
                                    OnSelectedIndexChanged="GrdCustLicense_SelectedIndexChanged" Width="271px" AllowPaging="True" OnPageIndexChanging="GrdCustLicense_OnPageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="CustLicenseID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustLicenseName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseName %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustLicenseStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="CustLicenseEndDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:CommandField>
                                    </Columns>
                                    <FooterStyle BackColor="Red" ForeColor="#000066" />
                                    <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
                                    <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                    <PagerTemplate>                                                   
<asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Small">首页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Small">上一页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Small">下一页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Small">尾页</asp:LinkButton> 
<asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
<asp:Label ID="LabelPageCount" runat="server" 
 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />

                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="vertical-align: top; width: 302px; height: 34px; text-align: right">
                                &nbsp; &nbsp; &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
