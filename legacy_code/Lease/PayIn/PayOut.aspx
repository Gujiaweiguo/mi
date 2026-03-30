<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayOut.aspx.cs" Inherits="Lease_PayIn_PayOut" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Inv_lblPayOut")%></title>
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
        
        table.payPutOut tr.rowHeight{ height:28px;}
        
        table.payPutOut tr.headLine{ height:1px; }
        table.payPutOut tr.bodyLine{ height:1px; }
        
        td.baseLable{ padding-right:10px;text-align:right;}
        td.baseInput{ align:left;padding-right:20px }
        -->
      </style>
      <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
      <script type="text/javascript" src="../../JavaScript/Common.js"> </script>
      <script type="text/javascript">
      <!--

        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtPayOutAmt.value))
            {
                parent.document.all.txtWroMessage.value =('<%= msg1 %>');
                return false;
            }
           
        }
            
                 //验证数字类型
        function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110)
            {
		        window.event.returnValue =true;
	        }else
	        {
		        window.event.returnValue =false;
	        }
	    } 
	    
	     function Load()
        {
            addTabTool("<%=baseInfo %>,Lease/PayIn/PayOut.aspx");
            loadTitle();
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
                <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 2px; text-align:left" class="tdTopRightBackColor">
                        <img alt="" class="imageLeftBack" style=" text-align:left"  />
                        </td>
                        <td class="tdTopRightBackColor" style="text-align:left">
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Inv_lblPayOut %>" Height="12pt" Width="342px"></asp:Label>
                        </td>
                        <td class="tdTopRightBackColor">
                        <img class="imageRightBack" style="width: 7px; height: 22px" />
                        </td>
                    </tr>
                    <tr >
                        <td style="width: 2px">
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="tdBackColor">
                            <table style="width:100%"  border="0" cellpadding="0" cellspacing="0" class="payPutOut">
                                <tr style="height:10px">
                                    <td style="width: 142px">
                                    </td>
                                    <td style="width: 114px">
                                    </td>
                                    <td style="width: 69px">
                                    </td>
                                    <td style="width: 191px">
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtPayInCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td style="width: 69px" class="baseLable">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
                                    <td style="width: 191px">
                                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td>
                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                            Text="<%$ Resources:BaseInfo,User_lblQuery %>" /></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td align="center" colspan="5">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 692px">
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
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False"></asp:TextBox></td>
                                    <td style="width: 69px">
                                    </td>
                                    <td colspan="2" rowspan="4" valign="top" style="text-align: right; padding-right:15px">
                                        <asp:GridView ID="gdvwPayIn" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#E1E0B2" Width="310px" PageSize="3" OnSelectedIndexChanged="gdvwPayIn_SelectedIndexChanged">
                                            <Columns>
                                                <asp:BoundField DataField="PayInID" HeaderText="PayInID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayInDate" HeaderText="<%$ Resources:BaseInfo,Lease_lblPayInCode %>" DataFormatString="{0:d}" HtmlEncode="False">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayInAmt" HeaderText="<%$ Resources:BaseInfo,Lease_lblPayInAmt %>">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayOutAmtSum" HeaderText="<%$ Resources:BaseInfo,Inv_lblPayOutSum %>">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ShopID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                            Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNext" runat="server"
                                                CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" /></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False"></asp:TextBox></td>
                                    <td style="width: 69px">
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtShopCodeF" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                            Enabled="False"></asp:TextBox></td>
                                    <td style="width: 69px">
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px; height: 31px;" class="baseLable">
                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblPayInAmt %>"></asp:Label></td>
                                    <td style="width: 114px; height: 31px">
                                        <asp:TextBox ID="txtPayInAmt" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                                    <td style="width: 69px; height: 31px">
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px; height: 28px;" class="baseLable">
                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Invoice_lblSurBalType %>"></asp:Label></td>
                                    <td style="width: 114px; height: 28px;">
                                        <asp:DropDownList ID="dropPayOutType" runat="server" Width="167px" Enabled="False">
                                        </asp:DropDownList></td>
                                    <td style="width: 69px; height: 28px;">
                                    </td>
                                    <td style="text-align: right; padding-right:15px" colspan="2" rowspan="5" valign="top">
                                        <asp:GridView ID="gdvwInvHeader" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#E1E0B2" Width="312px" PageSize="4">
                                            <Columns>
                                                <asp:BoundField DataField="InvID" HeaderText="InvID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvCode" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvDate" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvDate %>" DataFormatString="{0:d}" HtmlEncode="False">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="NoInvPayAmtL" HeaderText="<%$ Resources:BaseInfo,Rpt_AdvAmt %>">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        </td>
                                </tr>
                                <tr>
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Inv_lblPayOutAmt %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtPayOutAmt" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                    <td style="width: 69px">
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        </td>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                            Text="<%$ Resources:BaseInfo,User_btnOk %>" Width="63px" /><asp:Button ID="btnCel" runat="server"
                                                CssClass="buttonClear" OnClick="btnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" Width="63px" /></td>
                                    <td style="width: 69px">
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td class="baseLable" style="width: 142px">
                                        </td>
                                    <td style="width: 114px">
                                        </td>
                                    <td style="width: 69px">
                                    </td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        </td>
                                    <td style="width: 114px">
                                        </td>
                                    <td style="width: 69px">
                                    </td>
                                </tr>
                                <tr style="height:45px">
                                    <td style="width: 142px">
                                    </td>
                                    <td style="width: 144px">
                                        </td>
                                    <td style="width: 69px">
                                    </td>
                                    <td style="text-align: right; padding-right:15px" colspan="2"><asp:Button ID="btnBackT" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBackT_Click1"
                                            Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNextT" runat="server"
                                                CssClass="buttonNext" Enabled="False" OnClick="btnNextT_Click" Text="<%$ Resources:BaseInfo,Button_next %>" /></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px">
                                    </td>
                                    <td style="width: 114px">
                                    </td>
                                    <td style="width: 69px">
                                    </td>
                                    <td style="width: 191px">
                                    </td>
                                    <td>
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
