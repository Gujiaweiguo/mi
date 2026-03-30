<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectShopBrand.aspx.cs" Inherits="Lease_SelectShopBrand" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblBrandSelect")%></title>
    <base target="_self"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
 
      function clickok()
        {
            window.document.all("Hide_return").value = document.all("brandID").value+","+document.all("brandName").value;
	        returnall = document.getElementById("Hide_return").value;
	        window.returnValue=returnall;
	        window.close();		
	        return true;
        }
        function BtnUp( p )
        {
            var t = String(p)
            var l = t.substring(3,15); 
            document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
        }
        function BtnOver( p )
        {
            var t = String(p)
            var l = t.substring(3,15); 
            document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
        }
    </script>
</head>
<body style="margin:0px">
    <form id="form1" runat="server">
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 27px;">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                <td class="tdTopRightBackColor" style="height: 27px; text-align:left;">
                                    <asp:Label
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,ShopBrand_lblBrandSelect %>' Height="12pt"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="height: 27px; text-align: right;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                            <tr class="headLine">
                            <td colspan="3"></td>
                            </tr>
                             <tr style="height: 1px">
                                 <td colspan="3" class="tdBackColor">
                                     <table style="width: 231px" >
                                         <tr style="height:10px">
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td style="width: 61px">
                                             </td>
                                         </tr>
                                         <tr style="height: 10px">
                                             <td>
                                             </td>
                                             <td>
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandName %>" Width="62px"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtShopBrand" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                             <td style="width: 61px">
                                                 <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %>" OnClick="btnQuery_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                         </tr>
                                         <tr style="height: 10px">
                                             <td colspan="4">
                                                 <table border="0" cellpadding="0" cellspacing="0" style="left: 0px; width: 100%;
                                                     position: relative; top: 0px">
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
                                             <td colspan="4" style="text-align: center; padding-left:10px; padding-right:10px">
                                                 <asp:GridView ID="gvShopBrand" runat="server" AutoGenerateColumns="False" BorderStyle="Inset"  CellPadding="3"
                                                     BackColor="White" OnSelectedIndexChanged="gvShopBrand_SelectedIndexChanged" Width="288px" PageSize="7">
                                                     <Columns>
                                                         <asp:BoundField DataField="BrandID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="BrandName" HeaderText="<%$ Resources:BaseInfo,ShopBrand_lblBrandName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:CommandField>
                                                     </Columns>
                                                 </asp:GridView>
                                             </td>
                                         </tr>
                                         <tr class="rowHeight">
                                             <td colspan="4" style="text-align: center" align="right">
                                                 <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                                     Text="<%$ Resources:BaseInfo,Button_back %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                 <asp:Button ID="btnNext" runat="server"
                                                         CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                        
                        </table>
        <asp:HiddenField ID="brandID" runat="server" />
        <asp:HiddenField ID="brandName" runat="server" />
        <input id="Hide_return" type="hidden" runat="server"/>
    </form>
</body>
</html>
