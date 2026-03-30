<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UnionAgainPrint.aspx.cs" Inherits="Lease_ChargeAccount_UnionAgainPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_UnionBillreprint")%></title>
     <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
           
            tr{height:15px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
    <script type="text/javascript" src="../../JavaScript/Common.js" language="javascript"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
        
        <!--
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,Lease/ChargeAccount/UnionAgainPrint.aspx");
	        loadTitle();
	    }
       -->
    </script>
   
</head>
<body onload="Load()" style="margin:0px">
    <form id="form1" runat="server">
                        <div>
                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                            </asp:ScriptManager>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                            <table style="width: 100%" border="0" cellpadding="0" cellspacing="0" class="tdBackColor">
                                <tr>
                                    <td colspan="5">
                                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 11px; text-align:left; height: 28px;" class="tdTopRightBackColor">
                                                 <img class="imageLeftBack" />
                                                </td>
                                                <td style="width: 574px;text-align:left; height: 28px;" class="tdTopRightBackColor">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_UnionBillreprint %>"></asp:Label>
                                                </td>
                                                <td class="tdTopRightBackColor" style="height: 28px">
                                                <img class="imageRightBack"/>
                                                </td>
                                            </tr>
                                            <tr style="height:1px">
                                                <td colspan="3" style="background-color:White; height:1px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style="height:10px">
                                    <td style="width: 116px" >
                                    </td>
                                    <td style="width: 203px">
                                    </td>
                                    <td style="width: 88px">
                                    </td>
                                    <td style="width: 5px">
                                    </td>
                                    <td style="width: 75px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 116px; height: 15px;">
                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"
                                            Width="50px"></asp:Label></td>
                                    <td style="width: 203px; height: 15px;" >
                                        <asp:TextBox ID="txtContractID" runat="server" CssClass="ipt160px" Width="157px"></asp:TextBox></td>
                                    <td class="lable" style="width: 88px; height: 15px;">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_RptType %>"
                                            Width="59px"></asp:Label></td>
                                    <td style="height: 15px; width: 5px;">
                                        <asp:DropDownList ID="dropRptType" runat="server" Width="164px">
                                        </asp:DropDownList></td>
                                    <td style="height: 15px; width: 75px;">
                                        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="<%$ Resources:BaseInfo,User_lblQuery %>" CssClass="buttonQuery" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 116px; height: 15px">
                                    </td>
                                    <td style="width: 203px; height: 15px">
                                        <asp:CheckBox ID="cbType" runat="server" Font-Size="9pt" Text="<%$ Resources:BaseInfo,Invoice_lblcbType %>" /></td>
                                    <td class="lable" style="width: 88px; height: 15px">
                                    </td>
                                    <td style="width: 5px; height: 15px">
                                    </td>
                                    <td style="width: 75px; height: 15px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lable" colspan="5" style="text-align: center; height: 28px;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr style="height: 1px">
                                                <td style="width: 160px; height: 1px; background-color: #738495">
                                                </td>
                                            </tr>
                                            <tr style="height: 1px">
                                                <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lable" align="center" colspan="5" rowspan="7" style="padding-left:10px; padding-right:10px; text-align: center;">
                                        <asp:GridView ID="gvCharge" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" Width="100%" OnRowDataBound="gvCharge_RowDataBound" OnSelectedIndexChanged="gvCharge_SelectedIndexChanged" OnPageIndexChanging="gvCharge_PageIndexChanging" AllowPaging="True" CellPadding="4">
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
                                            <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="Small"/> 
                                              </PagerTemplate>         
                                            <PagerSettings Mode="NextPreviousFirstLast"  />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 116px; height: 21px;" >
                                    </td>
                                    <td style="width: 203px; height: 21px;">
                                    </td>
                                    <td class="lable" style="width: 88px; height: 21px;">
                                    </td>
                                    <td style="width: 5px; height: 21px;">
                                    </td>
                                    <td style="width: 75px; height: 21px;">
                                    </td>
                                </tr>
                            </table>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                        </div> 
    </form>
</body>
</html>
