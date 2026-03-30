<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopBaseInfoPalaver.aspx.cs" Inherits="Lease_ShopBaseInfoPalaver" ResponseEncoding="gb2312"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopInfo")%></title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
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
     <style type="text/css">
        
        <!--
            talbe.baseShop tr{ height:50px;}
            table.baseShop tr.headLine{ height:1px; }
            table.baseShop tr.bodyLine{ height:1px;}
        -->
        
        
    </style>  
    <script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
     <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" language="javascript"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"> </script>
</head>
<body  style="margin:0px">
    <form id="form1" runat="server">
     <div id="ShopBaseInfo" >
  
          <table class="baseShop" border="0" cellpadding="0" cellspacing="0" style="height: 25px" width="100%">
            <tr>
                <td class="tdTopBackColor" style="width: 430px; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" />
                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblShopInfo %>" Width="320px"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="height: 25px; text-align:right;" valign="top">
                    <img class="imageRightBack" /></td>
            </tr>
            <tr class="headLine"> 
                <td class="tdBackColor" style="background-color:White;" colspan="4">
                </td>
             </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" style="width:100%; height:400px" >
            <tr>
             <td class="tdBackColor" valign="top" style="width:60%">
                <table width="100%">
                    <tr style="height:25px">
                        <td colspan="3" style="text-align: center; width: 90%;">
                            <table style="width:90%;" border="0" cellpadding="0" cellspacing="0">
                                <tr  class="bodyLine">
                                    <td style="height: 1px; background-color: #738495;" >
                                    </td>
                                </tr>
                                <tr  class="bodyLine">
                                    <td style="height: 1px; background-color: #ffffff;" >
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="text-align:right; width: 100%;">
                        <td colspan="3" style="text-align: center; width: 100%;">
                            <asp:GridView ID="gvShop" runat="server" AutoGenerateColumns="False" BackColor="White"
                                Width="90%" OnSelectedIndexChanged="gvShop_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="ShopId">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopName" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopTypeName" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopType %>">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                    </asp:BoundField>
                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        <ItemStyle BorderColor="#E1E0B2" />
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
          <td width="40%" class="tdBackColor" valign="top">
                 <table class="tdBackColor" >
                       <tr height="20">
                           <td style="width: 3945px;" ></td>
                           <td valign="bottom" style="width: 345px" align="right">
                               <table style="width:126px;" border="0" cellpadding="0" cellspacing="0">
                                   <tr  class="bodyLine">
                                       <td style="height: 1px; background-color: #738495;" >
                                       </td>
                                   </tr>
                                   <tr  class="bodyLine">
                                       <td style="height: 1px; background-color: #ffffff;" >
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                       <tr>
                           <td style="width: 3945px; text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>"></asp:Label></td>
                           <td style="width: 345px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True" Width="128px"></asp:TextBox></td>
                       </tr>
                       <tr><td style="WIDTH: 3945px;  TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label50" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 345px; HEIGHT: 21px; text-align: right;" class="tdBackColor"><asp:TextBox id="txtShopName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True" Width="128px"></asp:TextBox></td></tr>
                       <tr>
                           <td style="width:3945px;  text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label53" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label></td>
                           <td style="width:345px; text-align: right;" class="tdBackColor">
                               <asp:DropDownList ID="DDownListShopType" runat="server" Width="132px" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style="WIDTH: 3945px; TEXT-ALIGN: right" class="tdBackColor">
                            <asp:Label id="Label16" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 345px; HEIGHT: 10px; text-align: right;" class="tdBackColor">
                            <asp:TextBox id="txtRentArea" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True" Width="128px"></asp:TextBox></td></tr>
                     <tr>
                         <td class="tdBackColor" style="width: 3945px;text-align: right">
                            <asp:Label id="Label57" runat="server" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaName %>" CssClass="labelStyle"></asp:Label></td>
                         <td class="tdBackColor" style="width: 345px; text-align: right;">
                           <asp:DropDownList ID="DDownListAreaName" runat="server" Width="132px" BackColor="#F5F5F4" Enabled="False">
                           </asp:DropDownList></td>
                     </tr>
                       <tr><td style="WIDTH: 3945px;TEXT-ALIGN: right" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label60" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td><td style="WIDTH: 345px; HEIGHT: 23px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtStartDate" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False" Width="128px"></asp:TextBox></td></tr>
                       <tr><td style="WIDTH: 3945px;TEXT-ALIGN: right" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label61" runat="server" Text="<%$ Resources:BaseInfo,ConLease_labEndDate %>"></asp:Label></td><td style="WIDTH: 345px; HEIGHT: 21px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtEndDate" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False" Width="128px"></asp:TextBox></td></tr>
                       <tr>
                           <td style="width: 3945px;text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label63" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"></asp:Label></td>
                           <td style="width: 345px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtContactName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False" Width="128px"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="width: 3945px; text-align: right;" class="tdBackColor">
                               <asp:Label ID="Label64" runat="server" Text="<%$ Resources:BaseInfo,Dept_lblTel %>" CssClass="labelStyle"></asp:Label></td>
                           <td style="width: 345px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtContactTel" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False" Width="128px"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="width: 3945px; text-align: right" class="tdBackColor">
                               </td>
                           <td style="width: 345px; text-align: right;" class="tdBackColor">
                               </td>
                       </tr>
                       <tr>
                           <td class="tdBackColor" style="width: 3945px; text-align: right">
                               </td>
                           <td class="tdBackColor" style="width: 345px;">
                               </td>
                       </tr>
                       <tr>
                           <td class="tdBackColor" colspan="2" >
                               </td>
                       </tr>
            </table>
            </td>
            <td width="30%" class="tdBackColor" valign="top">
              <table class="tdBackColor" style="width: 283px">
                       <tr height="20">
                           <td style="width: 98px;">
                           </td>
                           <td  valign="bottom">
                               <table style="width:165px;" border="0" cellpadding="0" cellspacing="0">
                                   <tr class="bodyLine">
                                       <td style="height:1px; background-color: #738495;" class="tdBackColor">
                                       </td>
                                   </tr>
                                   <tr class="bodyLine">
                                       <td style=" height: 1px; background-color: #ffffff;" class="tdBackColor">
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                       <tr>
                           <td style="width: 98px;text-align: right" class="tdBackColor">
                               <asp:Label id="Label56" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblMainBrand %>" CssClass="labelStyle"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:DropDownList id="DDownListBrand" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td>
                       </tr>
                  <tr>
                      <td class="tdBackColor" style="width: 98px; text-align: right">
                               <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>"></asp:Label></td>
                      <td class="tdBackColor">
                               <asp:DropDownList ID="ddlStore" runat="server" Width="165px" AutoPostBack="True" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td>
                  </tr>
                       <tr><td style="WIDTH: 98px;TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label30" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblSelectUnit %>" CssClass="labelStyle"></asp:Label></td><td style="HEIGHT: 21px" class="tdBackColor">
                           <asp:TextBox ID="txtUnits" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                               Enabled="False" Height="22px" Width="160px"></asp:TextBox></td></tr>
                       <tr>
                           <td style="width:98px;text-align:right;" class="tdBackColor" valign="bottom">
                               <asp:Label CssClass="labelStyle" ID="Label34" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblNoeUnitCollect %>"></asp:Label></td>
                           <td class="tdBackColor" rowspan="2">
                               <asp:ListBox ID="ListBoxUnits" runat="server" Width="165px" Enabled="False" Height="110px"></asp:ListBox></td>
                       </tr>
                  <tr height = "30px">
                      <td style="width: 98px;" align="right" valign="top">
                               <asp:Label ID="Label19" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblComm %>"></asp:Label></td>
                  </tr>
                       <tr><td style="WIDTH: 98px;TEXT-ALIGN: right" class="tdBackColor"></td><td style="HEIGHT: 23px" class="tdBackColor"></td></tr>
                       <tr><td style="WIDTH: 98px; TEXT-ALIGN: right" class="tdBackColor"></td><td style="HEIGHT: 21px; padding-left:20px" class="tdBackColor" align="left">
                           &nbsp;
                       </td></tr>
                       <tr>
                           <td style="width: 98px; text-align: right;" class="tdBackColor">
                               </td>
                           <td  rowspan="4">
                               </td>
                       </tr>
                       <tr>
                           <td style="width: 98px;text-align: right;" >
                               </td>
                       </tr>
                       <tr>
                           <td  style="width: 98px; text-align: right">
                           </td>
                       </tr>
                       <tr>
                           <td style="width: 98px; text-align: right; height: 35px;">
                           </td>
                       </tr>
                       <tr>
                           <td style="width:98px;  text-align: right;" class="tdBackColor">
                           </td>
                           <td class="tdBackColor">
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
