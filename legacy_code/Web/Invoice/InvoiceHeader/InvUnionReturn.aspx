<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvUnionReturn.aspx.cs" Inherits="Invoice_InvUnionReturn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
        <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	function txtSurAmtSum()
	{
	    document.getElementById("txtSurAmt").value = document.getElementById("txtInvPayExRate").value * document.getElementById("txtInvPaidAmtSum").value;
	}
    function Load()
    {
        addTabTool("<%=baseInfo %>,Invoice/InvoiceHeader/InvoiceHeader.aspx");
        loadTitle();
    }
    </script>
</head>
<body onload="Load()" topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="showLeaseBargain">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 100%">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 577px; height: 22px; text-align: left">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Menu_InvUnionReturn %>" Width="330px"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 45px;">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 635px; height: 15px">
                                                    <table style="width: 652px">
                                                    <tr>
                                                    <td style="width: 72px; height: 17px; text-align: right">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"
                                                                        Width="50px"></asp:Label>
                                                    </td>
                                                    <td style="width: 5px; height: 17px">
                                                        &nbsp;</td>
                                                    <td style="height: 17px; width: 169px;">
                                                        &nbsp;<asp:TextBox ID="txtContractID" runat="server" CssClass="ipt160px"
                                                                        Width="157px"></asp:TextBox></td>
                                                    <td style="width: 61px; text-align: right;">
                                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>"></asp:Label></td>
                                                        <td style="width: 4px"></td>
                                                    <td style="height: 35px; width: 178px;">
                                                        &nbsp;<asp:DropDownList ID="dropInvCode" runat="server" Width="164px" OnSelectedIndexChanged="dropInvCode_SelectedIndexChanged" AutoPostBack="True">
                                                        </asp:DropDownList></td>
                                                            <td>
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                            Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                    </tr>
                                                    </table>
                                                    
                                                        </td>
                                                </tr>
                                                <tr>
                                                <td style="height: 5px; text-align: center" colspan="2">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
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
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 21%;">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 97%; height: 275px">
                                                            <tr style="height: 5px">
                                                                <td style="width: 105px; height: 5px;">
                                                                </td>
                                                                <td style="width: 156px; height: 5px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                                                <td style="height: 35px; width: 156px;">
                                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" Width="119px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                                                <td style="height: 35px; width: 156px;">
                                                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True" Width="120px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblTaxCode %>"
                                                                        Width="50px"></asp:Label></td>
                                                                <td style="height: 35px; width: 156px;">
                                                                    <asp:TextBox ID="txtTaxCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"
                                                                        Width="120px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: center; width: 105px;">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>"
                                                                        Width="50px"></asp:Label></td>
                                                                <td style="height: 35px; width: 156px;">
                                                                    <asp:TextBox ID="txtBank" runat="server" CssClass="Enabledipt160px" ReadOnly="True"
                                                                        Width="120px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 105px; height: 35px;">
                                                                    <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>"
                                                                        Width="50px"></asp:Label></td>
                                                                <td style="height: 35px; width: 156px;">
                                                                    <asp:TextBox ID="txtBankAcct" runat="server" CssClass="Enabledipt160px" ReadOnly="True"
                                                                        Width="120px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 105px; height: 35px;">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Invoice_InvUnionReturnTotalMoney %>"
                                                                        Width="50px"></asp:Label></td>
                                                                <td style="height: 35px; width: 156px;">
                                                                    <asp:TextBox ID="txtTotalMoney" runat="server" CssClass="Enabledipt160px" ReadOnly="True"
                                                                        Width="120px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: center; width: 105px; vertical-align: middle; height: 35px;">
                                                                    </td>
                                                                <td style="width: 156px; height: 35px;">
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="height: 35px" align="right">
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnAdd_Click"
                                                                        Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                                    <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnQuit_Click"
                                                                        Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top;" width="50%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 99%; height: 31px" id="">
                                                            <tr style="height: 5px">
                                                                <td style="width: 155px; height: 5px;">
                                                                </td>
                                                                <td class="baseInput" style="height: 5px; width: 200px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseInput" colspan="2" rowspan="3" style="vertical-align: top; text-align: center; height: 135px;">
                                                                    <asp:GridView ID="gridInvJVDetail" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                        BorderStyle="Inset" BorderWidth="1px" Height="120px" Width="437px" PageSize="5" OnRowDataBound="GrdVewInvoiceHeader_RowDataBound" ShowFooter="True" AllowPaging="True" OnPageIndexChanging="gridInvJVDetail_OnPageIndexChanging">
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  /><FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                        <Columns>
                                                                            <asp:BoundField DataField="invJVDetailID">
                                                                                <ItemStyle CssClass="hidden" />
                                                                                <HeaderStyle CssClass="hidden" />
                                                                                <FooterStyle CssClass="hidden" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="ChargeTypeName" HeaderText="<%$ Resources:BaseInfo,Inv_lblChargeTypeName %>" FooterText="<%$ Resources:BaseInfo,Invoice_InvUnionTicket %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="InvStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="InvEndDate" HeaderText="<%$ Resources:BaseInfo,Rpt_EDate %>" FooterText="<%$ Resources:BaseInfo,Invoice_InvUnionPayment %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="invPcent" HeaderText="<%$ Resources:BaseInfo,ConLease_labDistill %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="invSalesAmt" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>" FooterText="<%$ Resources:BaseInfo,ConLease_lblTaxFrank %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="InvPayAmt" HeaderText="<%$ Resources:BaseInfo,Invoice_InvUnionMoneyP %>" FooterText="<%$ Resources:BaseInfo,Invoice_InvUnionTaxMoney %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="InvActPayAmt" HeaderText="<%$ Resources:BaseInfo,Invoice_InvUnionReturnMoney %>">
                                                                                <ItemStyle BorderColor="#E1E0B2" />
                                                                                <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                                <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                            </asp:BoundField>

                                                                        </Columns>
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
                                                            <td style="width: 1px">
                                                            </td>
                                                            </tr>

                                                            <tr style="height: 8px">
                                                                <td colspan="2" style="height: 9px" align="right">
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="height: 5px" valign="top">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2" style="height: 20px; vertical-align: top; text-align: center;"><asp:GridView ID="gridInvDetail" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                        BorderStyle="Inset" BorderWidth="1px" Height="120px" Width="438px" PageSize="5" OnRowDataBound="GrdVewInvoiceDetail_RowDataBound" ShowFooter="True" AllowPaging="True" OnPageIndexChanging="gridInvDetail_OnPageIndexChanging">
                                                                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="InvDetailID">
                                                                            <ItemStyle CssClass="hidden" />
                                                                            <HeaderStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ChargeTypeName" HeaderText="<%$ Resources:BaseInfo,Inv_lblChargeTypeName %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>" FooterText="<%$ Resources:BaseInfo,Invoice_InvUnionDeductMoney %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvEndDate" HeaderText="<%$ Resources:BaseInfo,Rpt_EDate %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvActPayAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblPayment %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                            <FooterStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                    </Columns>
                                                                    <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
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
                                                            <tr style="height: 5px">
                                                                <td colspan="2" style="height: 15px" align="right">
                                                                    <br />
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 27px; text-align: right;" colspan="2">
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
    </form>
</body>
</html>
