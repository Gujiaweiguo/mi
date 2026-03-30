<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustBrandSelect.aspx.cs" Inherits="Lease_PotCustomer_PotCustBrandSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
         <title><%= (String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblBrandQuery")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
		<script type="text/javascript">
	    function Load()
	    {
	        addTabTool(document.getElementById("hidcon").value + ",Lease/PotCustomer/PotCustBrandSelect.aspx");
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
        function ShowMessage(wFlwID,vID)
        {
            //var wFlwID = document.getElementById("HidenWrkID").value;
            //var vID = document.getElementById("HidenVouchID").value;
	        window.showModalDialog('../WrkFlwMessage.aspx?wrkFlwID='+encodeURI(wFlwID)+'&voucherID='+encodeURI(vID),'window','dialogWidth=600px;dialogHeight=320px');
        }
        //获得品牌
        function ShowBrandTree()
        {
        
        	strreturnval=window.showModalDialog('../Brand/BrandSelect.aspx','window','dialogWidth=337px;dialogHeight=420px');
			window.document.all("hidBrandID").value = strreturnval;
			if ((window.document.all("hidBrandID").value != "undefined") && (window.document.all("hidBrandID").value != ""))
			{
			    var btnDellBrand = document.getElementById('<%= btnDellBrand.ClientID %>');
                btnDellBrand.click();
            }
			else
			{
				return;	
			}  
        }
    </script>
</head>
<body  style="margin-top:0; margin-left:0" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
    <table style="width: 100%" cellpadding="0" cellspacing="0">
<%--                    <tr>
                        <td style="width: 826px; height: 25px; vertical-align: middle; text-align: left;" class="tdTopBackColor" valign="top">
                            <img class="imageLeftBack" alt="" />
                            <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_labCustomerQuery %>" Width="420px"></asp:Label></td>
                        <td style="width: 538px; height: 25px; text-align: right" class="tdTopRightBackColor"
                            valign="top" colspan="2">
                            <img class="imageRightBack" alt="" /></td>
                    </tr>--%>
                    <tr >
                    <td>
                        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
                            width: 100%; text-align: center;" >
                            <tr>
                                <td class="tdTopBackColor" style="width: 5px">
                                    <img alt="" class="imageLeftBack" />
                                </td>
                                <td class="tdTopBackColor">
                                    <%= (String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblBrandQuery")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 329px; text-align: center" class="tdBackColor" valign="top"
                            colspan="3">
                            <table style="width: 100%; height: 380px" cellspacing="0" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td style="width: 495px; height: 5px" class="tdBackColor" colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="8" style="height: 22px; width: 100%; text-align: center;">
                                        <table style="width: 100%">
                                        <tr>
                                        <td style="width: 20%; height: 22px">
                                            &nbsp;<asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandName %>" Width="80px"></asp:Label></td>
                                        <td style="width: 15%; height: 22px">
                                            <asp:TextBox ID="txtBrandName" runat="server" CssClass="ipt160px"></asp:TextBox>
                                            </td>
                                        <td style="width: 5%; height: 22px" align="right">
                                            <asp:LinkButton ID="btnDellBrand" runat="server" OnClick="btnDellBrand_Click" 
                                                Width="0px"></asp:LinkButton>
                                            </td>
                                       <td style="width: 5%; height: 22px" align="left">
                                           &nbsp;</td>
                                        <td style="width: 5%; height: 22px" align="right">
                                            &nbsp;</td>
                                            <td style="width: 20%; height: 22px" align="left">
                                                &nbsp;</td>
                                        <td style="width: 20%; height: 22px">
                                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" 
                                                OnClick="btnQuery_Click" TabIndex="1" 
                                                Text="<%$ Resources:BaseInfo,User_lblQuery %>" />
                                            </td>
                                            <td style="width: 20%; height: 22px">
                                                &nbsp;</td>
                                            <td style="width: 20%; height: 22px">
                                                &nbsp;</td>
                                        <td style="width: 25%; height: 22px">
                                            &nbsp;</td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 12px; text-align: center" class="tdBackColor" colspan="8">
                                            <table style="left: 0px; width: 98%; position: relative; text-align:center;" cellspacing="0" cellpadding="0"
                                                border="0">
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
                                        <td style="width: 100%; height: 260px; text-align: center; vertical-align: top;" class="tdBackColor" colspan="8">
                                            <table style="width: 100%; height: 260px;">
                                                <tbody>
                                                    <tr>
                                                        <td style="left: 0px; vertical-align: top; width: 100%; position: relative; text-align: center">
                                                            <asp:GridView ID="GrdWrk" runat="server" BorderWidth="1px" BorderStyle="Inset" CellPadding="3" BackColor="White" Width="98%" Height="258px"
                                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" OnPageIndexChanging="GrdWrk_PageIndexChanging" OnRowDataBound="GrdWrk_RowDataBound" OnSelectedIndexChanging="GrdWrk_SelectedIndexChanging">
                                                                <Columns>
                                                                    <asp:BoundField DataField="CustID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    
                                                                    <asp:BoundField DataField="CustCode" HeaderText="<%$ Resources:BaseInfo,LeaseAreaType_CustCode %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CustName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="BrandName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblTradeBrand %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TradeName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblTradeType %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="OperateName" HeaderText="<%$ Resources:BaseInfo,PotShop_BizMode %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle　BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="AvgAmt" HeaderText="<%$ Resources:BaseInfo,Rpt_SalesPer %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:CommandField>
                                                                </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
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
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        
                                        
                                        <td style="vertical-align: top; width: 98%; height: 44px; left: 0px; text-align: center;" class="tdBackColor"
                                            colspan="8">
                                            <table style="width: 98%" cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495;  position: relative;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff;  position: relative;">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        <table>
                                        <tr>
                                        <td style="left: 0px; position: relative; height: 37px; width: 6px;">
                                            
                                        </td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
            </table><asp:TextBox ID="hidBrandID" runat="server" CssClass="hidden"
                        Width="1px"></asp:TextBox>
    </div>
         <asp:HiddenField ID="HidenWrkID" runat="server">
            </asp:HiddenField>
             <asp:HiddenField ID="HidenVouchID" runat="server">
            </asp:HiddenField>
        <asp:HiddenField ID="hidcon" runat="server" Value="<%$ Resources:BaseInfo,ShopBrand_lblBrandQuery %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

