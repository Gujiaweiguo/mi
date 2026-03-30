<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InterestAgainPrint.aspx.cs" Inherits="Lease_ChargeAccount_InterestAgainPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_InterestPrint")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
        
        table.tblBase tr.rowHeight{ height:28px;}
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        
        td.baseLable{ padding-right:10px;text-align:right; width:136px}
        td.baseInput{ align:left;padding-right:20px }
        -->
      </style>
      <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
      <script language="javascript" type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
      <script type="text/javascript">
      <!--	    
	     function Load()
        {
            addTabTool("<%=strFresh %>,Lease/ChargeAccount/InterestAgainPrint.aspx");
            loadTitle();
//            document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
        }
	    -->  
      </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 27px; width: 100%;">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                <td class="tdTopRightBackColor" style=" height: 27px; text-align:left;">
                                    <asp:Label
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Menu_InterestPrint %>' Height="12pt" Width="218px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style=" height: 27px; text-align: right;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                            <tr class="headLine">
                            <td colspan="3"></td>
                            </tr>
                             <tr style="height: 1px">
                                 <td colspan="3" class="tdBackColor">
                                     <table style="width: 100%" >
                                         <tr style="height:10px">
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td style="width: 497px">
                                             </td>
                                             <td style="width: 497px">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"
                                                     Width="50px"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_RptType %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:DropDownList ID="dropRptType" runat="server" Width="164px">
                                                 </asp:DropDownList></td>
                                             <td class="baseInput" style="width: 497px">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"
                                                     Width="50px"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtAccDate" runat="server" CssClass="ipt160px" MaxLength="32" onclick="calendar()"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"
                                                     Width="50px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtAccEndDate" runat="server" CssClass="ipt160px" MaxLength="32"
                                                     onclick="calendar()"></asp:TextBox></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                     Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td class="baseLable">
                                             </td>
                                             <td colspan="4">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="5" style="text-align: center">
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
                                             <td colspan="5" style="text-align: center">
                                                 <asp:GridView ID="gvCharge" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" Width="96%" OnSelectedIndexChanged="gvCharge_SelectedIndexChanged" OnRowDataBound="gvCharge_RowDataBound" AllowPaging="True" CellPadding="3" OnPageIndexChanging="gvCharge_OnPageIndexChanging">
                                                     <RowStyle Font-Size="10pt" ForeColor="Black" Height="10px" />
                                            <Columns>
                                                <asp:BoundField DataField="InvID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvCode" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                                    <ItemStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ContractCode" HeaderText="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>">
                                                    <ItemStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvDate" HeaderText="<%$ Resources:BaseInfo,Rpt_InvDate %>" DataFormatString="{0:d}" HtmlEncode="False" >
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustCode" HeaderText="<%$ Resources:BaseInfo,LeaseAreaType_CustCode %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="TaxCode" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblTaxCode %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BankName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblBankName %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BankAcct" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblBankAcct %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:CommandField SelectText="<%$ Resources:BaseInfo,btn_lblPrint %>" ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,btn_lblPrint %>">
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:CommandField>
                                            </Columns>
                                            <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
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
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO"  Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />

                                        </asp:GridView>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 </td>
                                             <td>
                                                 &nbsp;</td>
                                             <td style="text-align: right" colspan="3">
                                                 <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnPrint_Click"
                                                     Text="<%$ Resources:BaseInfo,btn_lblPrint %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                 &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td colspan="2" style="text-align: right">
                                             </td>
                                             <td colspan="1" style="text-align: right">
                                             </td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                        
                        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
