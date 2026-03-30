<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PosInfoQuery.aspx.cs" Inherits="Sell_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
        <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript">
	function Load()
    {
        addTabTool("<%=baseInfo %>,Sell/PosInfoQuery.aspx");
        loadTitle();
    }
    
     function ShowShopTree()
        {
        	strreturnval=window.showModalDialog('../Lease/Shop/SelectShop.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
			     var objImgBtn1 = document.getElementById('<%= LinkButton2.ClientID %>');
                objImgBtn1.click();
            }
			else
			{
				return;	
			}  
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
<body style="margin:0px" onload ="Load()">
    <form id="form1" runat="server" >
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>

            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 401px">
                <tr>
                    <td align="left" class="tdTopRightBackColor" style="vertical-align: top; width: 356px;
                        height: 22px; text-align: left">
                        <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                        <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_ReceiptQuery %>"
                            Width="295px"></asp:Label></td>
                    <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                    </td>
                    <td class="tdTopRightBackColor" style="vertical-align: top; height: 22px;
                        text-align: right; width: 115px;" valign="top">
                        <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                </tr>
                <tr>
                    <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px" valign="top">
                        <table border="0" cellpadding="0" cellspacing="0" style="height: 54px;">
                            <tr>
                                <td colspan="4" style="vertical-align: top; width: 136px; height: 10px; text-align: right">
                                </td>
                                <td colspan="9" style="vertical-align: top; height: 10px; text-align: left">
                                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="1" style="vertical-align: top; height: 12px; text-align: left">
                                </td>
                                <td colspan="13" style="vertical-align: top; height: 12px; text-align: left" align="left">
                                    &nbsp;<asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label>
                                    <asp:TextBox ID="ddlShopCode" runat="server" Width="360px" OnClick="ShowShopTree();">
                                    </asp:TextBox>&nbsp;&nbsp;
                                    <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                        Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="1" style="vertical-align: top; height: 23px; text-align: left">
                                </td>
                                <td align="center" colspan="13" style="vertical-align: top; height: 23px; text-align: left">
                                    &nbsp;<asp:Label ID="Label28" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblDate %>"></asp:Label>
                                    <asp:TextBox ID="txtBizdate" runat="server" CssClass="ipt160px" Width="117px" onclick="calendar()"></asp:TextBox><asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Rpt_POSId %>"></asp:Label><asp:TextBox ID="txtPosId" runat="server" Width="45px" CssClass="ipt160px"></asp:TextBox><asp:Label ID="Label6" runat="server" Text="<%$ Resources:ReportInfo,RptSale_BatchId %>"></asp:Label><asp:TextBox ID="txtBatchId" runat="server" Width="42px" CssClass="ipt160px"></asp:TextBox>
                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:ReportInfo,RptSale_ReceiptID %>"></asp:Label>
                                    <asp:TextBox ID="txtReceiptId" runat="server" Width="65px" CssClass="ipt160px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right" colspan="4" style="vertical-align: top; width: 136px; height: 23px;
                                    text-align: right">
                                </td>
                                <td align="right" colspan="9" style="vertical-align: top; height: 23px; text-align: left">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="13" style="vertical-align: top; text-align: left; height: 20px;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 787px; text-align: center; height: 1px;">
                                        <tr>
                                            <td style="left: 15px; width: 324px; position: relative; height: 1px; background-color: #738495">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="13" style="vertical-align: top; height: 20px; text-align: left">
                        <table border="0" cellpadding="0" cellspacing="0" style="height: 357px; width: 639px;">
                            <tr>
                                <td colspan="4" rowspan="8" style="vertical-align: top; width: 399px; text-align: left; left: 40px; position: relative;">
                                    <table border="0" cellpadding="0" cellspacing="0" style="height: 203px; width: 200px;">
                                        <tr>
                                            <td colspan="5" style="vertical-align: top; height: 21px; text-align: center">
                                               <asp:GridView ID="GrdReceiptInfo" runat="server" AutoGenerateColumns="False"
                                                    BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" 
                                                    Font-Strikeout="False" Height="162px" PageSize="20" Width="251px" OnSelectedIndexChanged="GrdReceiptInfo_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="GrdReceiptInfo_PageIndexChanging">
                                                    <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                    <RowStyle Font-Overline="False" ForeColor="Black" Height="10px" />
                                                    <Columns>
                                                        <asp:BoundField DataField="PosId" HeaderText="<%$ Resources:BaseInfo,Rpt_POSId %>">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2"  HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="BatchId" HeaderText="<%$ Resources:ReportInfo,RptSale_BatchId %>">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="ReceiptId" HeaderText="<%$ Resources:ReportInfo,RptSale_ReceiptId %>">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2"  HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_lblUserQuery %>" SelectText="<%$ Resources:BaseInfo,User_lblUserQuery %>"
                                                            ShowSelectButton="True">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" />
                                                        </asp:CommandField>
                                                    </Columns>
                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
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
                                            <td colspan="4" style="vertical-align: top; width: 69px; height: 21px; text-align: left">
                                            </td>
                                            <td colspan="1" style="vertical-align: top; width: 69px; height: 21px; text-align: left">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" style="vertical-align: top; width: 69px; height: 21px; text-align: left">
                                                </td>
                                            <td colspan="1" style="vertical-align: top; width: 69px; height: 21px; text-align: left">
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
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" rowspan="5" style="vertical-align: top; text-align: left; width: 489px; left: 70px; position: relative;"><fieldset>
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:ReportInfo,RptSale_FHeaderInfo %>"></asp:Label>
                                    </legend>
                                    <table style="width: 431px; height: 16px">
                                        <tr>
                                            <td style="width: 95px; height: 10px">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:ReportInfo,RptSale_SalesMan %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 106px; height: 10px">
                                                <asp:TextBox ID="txtSalesMan" runat="server" Width="98px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td style="height: 10px">
                                                <asp:Label ID="Label17" runat="server" Text="<%$ Resources:ReportInfo,RptSale_RReceiptId %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="height: 10px">
                                                <asp:TextBox ID="txtRReceipt" runat="server" Width="72px" CssClass="ipt160px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 95px; height: 10px">
                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:ReportInfo,RptSale_UserID %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 106px; height: 10px">
                                                <asp:TextBox ID="txtUserId" runat="server" Width="98px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td style="height: 10px">
                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:ReportInfo,RptSale_TransType %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="height: 10px">
                                                <asp:TextBox ID="txtTransType" runat="server" Width="98px" CssClass="ipt160px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 95px; height: 10px">
                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:ReportInfo,RptSale_MReceiptId %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 106px; height: 10px">
                                                <asp:TextBox ID="txtMReceipt" runat="server" Width="98px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td style="height: 10px">
                                                <asp:Label ID="Label27" runat="server" Text="<%$ Resources:ReportInfo,RptSale_TransTime %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="height: 10px">
                                                <asp:TextBox ID="txtTransTime" runat="server" Width="98px" CssClass="ipt160px"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                                    <br />
                                <fieldset >
                                <legend style="text-align: left">
                                 <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="<%$ Resources:ReportInfo,RptSale_FFooterInfo %>"></asp:Label>
                                </legend>
                                    <table style="width: 427px; height: 86px">
                                        <tr>
                                            <td style="width: 73px; height: 26px">
                                                <asp:Label ID="Label8" runat="server" Text="<%$ Resources:ReportInfo,RptSale_PaidAmt %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 93px; height: 26px">
                                                <asp:TextBox ID="txtTotalAmt" runat="server" Width="94px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td style="height: 26px">
                                                <asp:Label ID="Label12" runat="server" Text="<%$ Resources:ReportInfo,RptSale_Tax %>"
                                                    Width="48px"></asp:Label></td>
                                            <td colspan="3" style="height: 26px">
                                                <asp:TextBox ID="txtTax" runat="server" Width="94px" CssClass="ipt160px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 73px">
                                                <asp:Label ID="Label9" runat="server" Text="<%$ Resources:ReportInfo,RptSale_ReceiptDisc %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 93px">
                                                <asp:TextBox ID="txtReceiptDisc" runat="server" Width="94px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="Label13" runat="server" Text="<%$ Resources:ReportInfo,RptSale_MiscTax %>"
                                                    Width="48px"></asp:Label></td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtMiscTax" runat="server" Width="94px" CssClass="ipt160px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 73px">
                                                <asp:Label ID="Label10" runat="server" Text="<%$ Resources:ReportInfo,RptSale_SurCharge %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 93px">
                                                <asp:TextBox ID="txtSurCharge" runat="server" Width="94px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:ReportInfo,RptSale_TaxType %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 158px">
                                                <asp:TextBox ID="txtTaxType" runat="server" Width="33px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td style="width: 158px">
                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:ReportInfo,RptSale_ExemptTax %>"
                                                    Width="48px"></asp:Label></td>
                                            <td colspan="2" style="width: 94px">
                                                <asp:TextBox ID="txtExemptTax" runat="server" Width="56px" CssClass="ipt160px"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 73px">
                                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources:ReportInfo,RptSale_DueAmt %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 93px">
                                                <asp:TextBox ID="txtDueAmt" runat="server" Width="94px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td>
                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:ReportInfo,RptSale_MIscCharge %>"
                                                    Width="48px"></asp:Label></td>
                                            <td colspan="3">
                                                <asp:TextBox ID="txtMIscCharge" runat="server" Width="94px" CssClass="ipt160px"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                   </fieldset>
                                <fieldset>
                                    <legend style="text-align: left">
                                        <asp:Label ID="Label18" runat="server" CssClass="labelStyle" Text="<%$ Resources:ReportInfo,RptSale_FBonusInfo %>"></asp:Label>
                                    </legend>
                                    <table style="width: 431px; height: 16px">
                                        <tr>
                                            <td style="width: 95px; height: 10px">
                                                <asp:Label ID="Label19" runat="server" Text="<%$ Resources:BaseInfo,Tab_IssueCard %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="width: 106px; height: 10px">
                                                <asp:TextBox ID="TextBox9" runat="server" Width="98px" CssClass="ipt160px"></asp:TextBox></td>
                                            <td style="height: 10px">
                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:BaseInfo,Tab_integral %>"
                                                    Width="48px"></asp:Label></td>
                                            <td style="height: 10px">
                                                <asp:TextBox ID="TextBox10" runat="server" Width="72px" CssClass="ipt160px"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                                    <fieldset>
                                        <legend style="text-align: left">
                                            <asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="<%$ Resources:ReportInfo,RptSale_FMediaInfo %>"></asp:Label>
                                        </legend>
                                        <table style="width: 431px; height: 16px">
                                            <tr>
                                                <td colspan="4" style="height: 10px">
                                                <asp:GridView ID="GrdMedia" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                    BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Font-Size="12pt" Font-Strikeout="False"
                                                    Height="1px" PageSize="12" Width="432px">
                                                    <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                    <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                    <Columns>
                                                        <asp:BoundField DataField="MediaCd" HeaderText="<%$ Resources:BaseInfo,Rpt_MediaMDesc %>">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Amountt" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Remark1" HeaderText="<%$ Resources:BaseInfo,Associator_CardID %>">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Remark2" HeaderText="<%$ Resources:BaseInfo,Rpt_MediaMDesc %>">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                         <asp:BoundField DataField="Remark3" HeaderText="<%$ Resources:BaseInfo,BankCard_BankEFTID %>">
                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                                        </asp:BoundField>
      
                                                    </Columns>
                                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                                </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                <fieldset>
                                        <legend style="text-align: left">
                                            <asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="<%$ Resources:ReportInfo,RptSale_FDetailInfo %>"></asp:Label>
                                        </legend>
                                        <table style="width: 431px; height: 16px">
                            <tr>
                                <td colspan="4" style="height: 10px">
                                    <asp:GridView ID="GrdDetail" runat="server" AutoGenerateColumns="False" 
                                        BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" 
                                        Font-Size="12pt" Font-Strikeout="False" Height="1px" PageSize="12" 
                                        Width="433px">
                                        <FooterStyle BackColor="Red" ForeColor="#000066" />
                                        <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" 
                                            Height="10px" />
                                        <Columns>
                                            <asp:BoundField DataField="SkuCd" 
                                                HeaderText="<%$ Resources:ReportInfo,RptSale_SkuDesc %>">
                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                    CssClass="gridviewtitle" />
                                                <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NewPrice" 
                                                HeaderText="<%$ Resources:ReportInfo,RptSale_NewPrice %>">
                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                    CssClass="gridviewtitle" />
                                                <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Qty" 
                                                HeaderText="<%$ Resources:ReportInfo,RptSale_TrNum %>">
                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                    CssClass="gridviewtitle" />
                                                <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ItemDisc" 
                                                HeaderText="<%$ Resources:ReportInfo,RptSale_ItemDisc %>">
                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                    CssClass="gridviewtitle" />
                                                <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AllocDisc" 
                                                HeaderText="<%$ Resources:ReportInfo,RptSale_AllocDisc %>">
                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" 
                                                    CssClass="gridviewtitle" />
                                                <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            </asp:BoundField>
                                        </Columns>
                                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                        <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                        <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                    </asp:GridView>
                                </td>
                                <caption>
                                    &nbsp;
                                    <div>
                                    </div>
                                    <div>
                                    </div>
                                </caption>
                            </tr>
                                </td>
                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                <input id="allvalue" runat="server" style="width: 25px" type="hidden" />                                
                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton>
                            </tr>
                        </table>
                                </td>
            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
