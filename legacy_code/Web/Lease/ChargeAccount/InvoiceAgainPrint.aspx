<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceAgainPrint.aspx.cs" Inherits="Lease_ChargeAccount_InvoiceAgainPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_Billreprint")%></title>
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
	        addTabTool("<%=strFresh %>,Lease/ChargeAccount/InvoiceAgainPrint.aspx");
	        loadTitle();
	    }
       -->
    </script>
   
</head>
<body onload="Load()" style="margin:0px">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>        
                        <div>
                            <table style="width: 100%" border="0" cellpadding="0" cellspacing="0" class="tdBackColor">
                                <tr>
                                    <td colspan="5">
                                        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 11px; text-align:left; height: 28px;" class="tdTopRightBackColor">
                                                 <img class="imageLeftBack" />
                                                </td>
                                                <td style="width: 574px;text-align:left; height: 28px;" class="tdTopRightBackColor">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_Billreprint %>"></asp:Label>
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
                                    <td style="width: 114px" >
                                    </td>
                                    <td style="width: 203px">
                                    </td>
                                    <td style="width: 88px">
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 114px; height: 15px;">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"
                                            Width="50px"></asp:Label></td>
                                    <td style="width: 203px; height: 15px;" >
                                        <asp:TextBox ID="txtContractID" runat="server" CssClass="ipt160px" Width="157px"></asp:TextBox></td>
                                    <td class="lable" style="width: 88px; height: 15px;">
                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Lease_RptType %>" CssClass="labelStyle"></asp:Label>&nbsp;</td>
                                    <td style="height: 15px">
                                        &nbsp;<asp:DropDownList ID="dropRptType" runat="server" Width="164px">
                                        </asp:DropDownList></td>
                                    <td style="height: 15px">
                                        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="<%$ Resources:BaseInfo,User_lblQuery %>" CssClass="buttonQuery" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 114px; height: 15px">
                                    </td>
                                    <td style="width: 203px; height: 15px">
                                        <asp:CheckBox ID="cbType" runat="server" Text="<%$ Resources:BaseInfo,Invoice_lblcbType %>" Font-Size="9pt" /></td>
                                    <td class="lable" style="width: 88px; height: 15px">
                                    </td>
                                    <td style="height: 15px">
                                    </td>
                                    <td style="height: 15px">
                                    </td>
                                </tr>

                                <tr>
                                    <td class="lable" style="width: 114px;" />
                                    <td class="lable" colspan="5" style="text-align: left; height: 28px;">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 593px">
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
                                    <td class="lable" style="width: 114px">
                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                    <td style="width: 203px">
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                    <td class="lable" colspan="3" rowspan="5" style="vertical-align: top; text-align: center">
                                        <asp:GridView ID="gvCharge" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" OnSelectedIndexChanged="gvCharge_SelectedIndexChanged" Width="85%" AllowPaging="True" OnPageIndexChanging="gvShopBrand_OnPageIndexChanging">
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
                                                <asp:BoundField DataField="InvPayAmtL" HeaderText="<%$ Resources:BaseInfo,Account_lblTotalMoney %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="hidden" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvStatus" HeaderText="<%$ Resources:BaseInfo,User_lblUserStatusStr %>">
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="hidden" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustName" HeaderText="CustName">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ContractTypeID" HeaderImageUrl="ContractTypeID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <ItemStyle BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:CommandField>
                                                <asp:BoundField DataField="ContractID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
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
                                    <td class="lable" style="width: 114px" >
                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label></td>
                                    <td style="width: 203px">
                                        <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 114px" >
                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>"></asp:Label></td>
                                    <td style="width: 203px">
                                        <asp:TextBox ID="txtInvoiceID" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 114px" >
                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_InvDate %>"></asp:Label></td>
                                    <td style="width: 203px">
                                        <asp:TextBox ID="txtBuildDate" onclick="calendar()" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 114px; height: 28px;" >
                                        <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Account_lblInvoiceStutas %>"></asp:Label></td>
                                    <td style="width: 203px; height: 28px;">
                                        <asp:TextBox ID="txtPrintStatus" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                </tr>
                                <tr style="height:50px">
                                    <td class="lable" style="width: 114px" >
                                        </td>
                                    <td style="width: 203px">
                                        &nbsp;<asp:Button ID="btnSave" runat="server" OnClick="btnPrint_Click" Text="<%$ Resources:BaseInfo,btn_lblPrint %>" CssClass="buttonSave" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />
                                        <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:BaseInfo,User_btnCancel %>" CssClass="buttonCancel" OnClick="btnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />
                                        </td>
                                    <td class="lable" style="width: 88px">
                                        </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lable" style="width: 114px" >
                                    </td>
                                    <td style="width: 203px">
                                    </td>
                                    <td class="lable" style="width: 88px">
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div> 
                        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
