<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustLicense.aspx.cs" Inherits="Lease_PotCustomer_PotCustLicense" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_labCustomerQuery")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        loadTitle();
	    }
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
<body onload='Load()' style="margin:0px">
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="showmain" border="0" cellpadding="0" cellspacing="0"
            style="width:100%">
            <tr>
                <td style="vertical-align: top; width: 100%;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                 <div id="PotCustLicense">
                <table border="0" cellpadding="0" cellspacing="0" style="height: 405px; width: 100%;">
                    <tr>
                        <td class="tdTopBackColor" style="width: 266px; height: 25px; vertical-align: middle; text-align: left;" valign="top">
                            <img alt="" class="imageLeftBack" />
                            <asp:Label ID="Label5" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_labCustomerQuery %>" Width="254px"></asp:Label></td>
                        <td class="tdTopRightBackColor" colspan="2" style="width: 528px; height: 25px" valign="top">
                            <img class="imageRightBack" /></td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="2" style="width: 100%; height: 351px; text-align: left"
                            valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 311px" width="100%">
                                <tr>
                                    <td style="width: 280px; height: 1px; background-color: white">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 100%; height: 22px; text-align: center;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 267px">
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
                                    <td class="tdBackColor" style="width: 100%; height: 254px; text-align: center;" valign="top">
                                        <asp:GridView id="GrdCustLicense" runat="server" Width="273px" AutoGenerateColumns="False" OnSelectedIndexChanged="GrdCustLicense_SelectedIndexChanged" OnRowDataBound="GrdCustLicense_RowDataBound" Height="285px" BackColor="White" CellPadding="3"
                  BorderStyle="Inset" BorderWidth="1px" PageSize="11" AllowPaging="True" OnPageIndexChanging="GrdCustLicense_PageIndexChanging">
                       <Columns>
                          <asp:BoundField DataField="CustLicenseID">
                            <ItemStyle CssClass="hidden"  />
                            <HeaderStyle CssClass="hidden"  />
                            <FooterStyle CssClass="hidden"  />
                            </asp:BoundField>
                           
                           <asp:BoundField DataField="CustLicenseName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseName %>">
                               <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                               <ItemStyle BorderColor="#E1E0B2" />
                           </asp:BoundField>
                           <asp:BoundField DataField="CustLicenseStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>">
                               <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                               <ItemStyle BorderColor="#E1E0B2" />
                           </asp:BoundField>
                           <asp:BoundField DataField="CustLicenseEndDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>">
                               <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                               <ItemStyle BorderColor="#E1E0B2" />
                           </asp:BoundField>
                           <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" ShowSelectButton="True">
                               <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                               <ItemStyle BorderColor="#E1E0B2" />
                           </asp:CommandField>
                       </Columns>
                        <FooterStyle BackColor="Red" ForeColor="#000066"/>
                        <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                        <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                        <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                        <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                        <PagerTemplate>                                                   
                        <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
                         Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">首页</asp:LinkButton> 

                        <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
                         Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton> 

                        <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
                         Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton> 

                        <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
                         Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">尾页</asp:LinkButton> 
                        <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                        <asp:Label ID="LabelPageCount" runat="server" 
                         Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                        <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" /> 
                          </PagerTemplate>         
                        <PagerSettings Mode="NextPreviousFirstLast"  />
                    </asp:GridView> 
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 100%; height: 22px; text-align: center;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 267px">
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
                            </table>
                            </td>
                        <td class="tdBackColor" style="width: 100%; height: 351px; padding-right:50px" valign="top">
                          <table border="0" cellpadding="0" cellspacing="0" style="height: 216px; width: 100%;">
                                                          <tr>
                                    <td style="width: 280px; height: 1px; background-color: white" colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 15px; vertical-align: top; text-align: center;" valign="bottom">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 243px">
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #738495; left: 10px; position: relative; top: 10px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 160px; height: 1px; background-color: #ffffff; left: 10px; position: relative; top: 10px;">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px; text-align: center"
                                        valign="bottom">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseCode %>"
                                            Width="69px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtLicenseID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                        </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseName %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtLicenseName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseType %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:DropDownList ID="cmbLicenseType" runat="server" Width="166px" Enabled="False" >
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
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
                                    <td class="tdBackColor" style="width: 85px; height: 22px; text-align: right">
                                        <asp:Label ID="lblCustLicenseEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:TextBox ID="txtLicenseEndDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 105px; height: 22px; text-align: right">
                                        <asp:Label ID="Label1" runat="server" CssClass="labelStyle" 
                                            Text="<%$ Resources:BaseInfo,PotCustomer_ClientCard %>"></asp:Label>
                                    </td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="width: 160px; height: 22px">
                                        <asp:LinkButton ID="btnLook" runat="server" Enabled="False" 
                                            onclick="btnLook_Click" Text="<%$ Resources:BaseInfo,User_lblUserQuery %>"></asp:LinkButton>
                                    </td>
                                    <td class="tdBackColor" style="width: 5px; height: 22px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
     </ContentTemplate>
        </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
