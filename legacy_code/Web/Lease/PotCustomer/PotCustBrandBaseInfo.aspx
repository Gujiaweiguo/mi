<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustBrandBaseInfo.aspx.cs" Inherits="Lease_PotCustomer_CustBrand" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustTradeBrand")%></title>
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
	    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
		<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
		<script type="text/javascript">
		function Load()
		{
		    loadTitle();
		}
		/*获取二级经营类别*/
        function ShowTradeTree()
        {
        
        	strreturnval=window.showModalDialog('../TradeRelation/TradeRelationSelect.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("hidTradeID").value = strreturnval;
			if ((window.document.all("hidTradeID").value != "undefined") && (window.document.all("hidTradeID").value != ""))
			{
			    var btnBindDealType = document.getElementById('<%= btnBindDealType.ClientID %>');
                btnBindDealType.click();
            }
			else
			{
				return;	
			}  
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
        //text控件文本验证
    function allTextBoxValidator()
    {
        if(isEmpty(document.all.txtTrade.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "LeaseholdContract_labTradeID")%>'+document.getElementById("hidMessage").value;
            document.all.txtTrade.focus();
            return false;					
        }
        if(isEmpty(document.all.txtBrandName.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "ShopBrand_lblBrandName")%>'+document.getElementById("hidMessage").value;
            document.all.txtBrandName.focus();
            return false;					
        }
        if(isInteger(document.all.txtAvgAmt.value)==false)
        {
            alert("please input number.");
            document.all.txtAvgAmt.focus();
            return false;
        }
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
<body style="margin-top:0; margin-left:0" onload="Load();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustTradeBrand")%>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 405px">
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 50%; height: 320px; text-align: right"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 221px; width: 305px;">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px" valign="bottom">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 322px; height: 5px; text-align: center"
                                valign="bottom">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 267px">
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #738495" align="left">
                                        <asp:TextBox ID="hidTradeID" runat="server" CssClass="hidden"
                        Width="1px"></asp:TextBox><asp:TextBox ID="hidBrandID" runat="server" CssClass="hidden"
                        Width="1px"></asp:TextBox></td>
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
                            <td class="tdBackColor" style="height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 10px; height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px" valign="bottom">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"
                                    Width="69px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:TextBox ID="txtTrade" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                            <td class="tdBackColor" style=" height: 22px">
                            <img id="ImgTrade" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ShopBrand_lblBrandName%>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:TextBox ID="txtBrandName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            <img id="ImgBrandName" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" runat="server"/></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBrandTradeType %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:DropDownList ID="ddlOperateTypeId" runat="server" CssClass="cmb160px">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustSex %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:DropDownList ID="ddlSex" runat="server" CssClass="cmb160px">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustAge %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:TextBox ID="txtConsumerAge" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustomerLayer %>"></asp:Label>
                            </td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:DropDownList ID="ddlClientLevel" runat="server" CssClass="cmb160px">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblMostlyWarePrice %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:TextBox ID="txtAvgAmt" runat="server" CssClass="ipt160px" ></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PriceRange %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:TextBox ID="txtPriceRange" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 3px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px" align="left">
                                <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" ></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 322px; height: 22px; text-align: center; vertical-align:bottom;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 267px">
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #738495" >
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
                            <td class="tdBackColor" colspan="6" style="width: 300px; height: 50px; text-align:center; vertical-align:bottom;" >
                                <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" OnClick="btnAdd_Click"
                                    Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Enabled="False"
                                    OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnQuit_Click"
                                    Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:LinkButton ID="btnDellBrand" runat="server" OnClick="btnDellBrand_Click" Width="0px"></asp:LinkButton>
                                <asp:LinkButton ID="btnBindDealType" runat="server" OnClick="btnBindDealType_Click" CssClass="hidden" 
                                    Width="1px"></asp:LinkButton></td>
                        </tr>
                    </table>
                </td>
                <td class="tdBackColor" style="width: 280px; height: 330px" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 286px; height: 311px">
                        <tr>
                            <td class="tdBackColor" style="width: 322px; height: 22px; text-align:center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 275px">
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
                            <td class="tdBackColor" style="vertical-align: top; width: 302px; height: 254px;
                                text-align: center" valign="top">
                                <asp:GridView ID="GrdCustBrand" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="239px"
                                    OnSelectedIndexChanged="GrdCustBrand_SelectedIndexChanged" Width="271px" OnRowDataBound="GrdCustBrand_RowDataBound" PageSize="11" AllowPaging="True" OnPageIndexChanging="GrdCustBrand_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="CustBrandId">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TradeName" HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="BrandName" HeaderText="<%$ Resources:BaseInfo,ShopBrand_lblBrandName %>">
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
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                    <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
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
                            <td class="tdBackColor" style="width: 302px; height: 33px; text-align: center; vertical-align: middle;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
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
            </tr>
        </table>
    
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
    </form>
</body>
</html>
