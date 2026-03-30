<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DepBal.aspx.cs" Inherits="Invoice_DepBal_DepBal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Inv_lblDepBal")%></title>
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <style type="text/css">
        <!--
        
        table.payPutOut tr.rowHeight{ height:28px;}
        
        table.payPutOut tr.headLine{ height:1px; }
        table.payPutOut tr.bodyLine{ height:1px; }
        
        td.baseLable{ padding-right:10px;text-align:right;}
        td.baseInput{ align:left;padding-right:20px }
        -->
      </style>
      <script type="text/javascript">
      <!--
            function GetDepAmt()
            {
                if((document.getElementById("txtDepAmt").value != "undefined")&&(document.getElementById("txtDepAmt").value != "")&&(document.getElementById("txtPayOutAmtSum").value != "undefined")&&(document.getElementById("txtPayOutAmtSum").value != ""))
                {
                    document.getElementById("txtDepBalAmt").value = document.getElementById("txtDepAmt").value - document.getElementById("txtPayOutAmtSum").value
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
            addTabTool("<%=strFresh %>,Invoice/DepBal/DepBal.aspx");
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
                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Inv_lblDepBal %>" Height="12pt" Width="218px"></asp:Label>
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
                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td style="width: 69px" class="baseLable">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
                                    <td style="width: 191px">
                                        <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td>
                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                            Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" />
                                    <td align="left" colspan="5">
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
                                    <td style="width:5%;"></td>
                                </tr>
                                <tr>
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False"></asp:TextBox></td>
                                    <td align="center" colspan="3" rowspan="5" style="padding-right: 15px" valign="top">
                                        <asp:GridView ID="gdvwInvPayDetail" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#E1E0B2" Width="85%" PageSize="4" OnSelectedIndexChanged="gdvwInvPayDetail_SelectedIndexChanged" OnRowDataBound="gdvwInvPayDetail_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvShopBrand_OnPageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="InvPayDetID" HeaderText="InvPayDetID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvPayID" HeaderText="<%$ Resources:BaseInfo,Inv_lblInvPayID %>">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="InvPaidAmt" HeaderText="<%$ Resources:BaseInfo,Inv_lblDepAmL %>">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="PayOutAmtSum" HeaderText="<%$ Resources:BaseInfo,Inv_lblPayOutSum %>">
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>">
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                    <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" CssClass="gridviewtitle" />
                                                </asp:CommandField>
                                                <asp:BoundField DataField="CustName">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ContractCode">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ShopID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="ChargeTypeID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                </asp:BoundField>
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
                                        &nbsp;<br />
                                        &nbsp;&nbsp;<br />
                                        &nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td class="baseLable" style="width: 142px">
                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtContractCode" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                            Enabled="False"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                                    <td style="width: 114px"><asp:DropDownList ID="dropShopName" runat="server" Width="167px" BackColor="#F5F5F4" Enabled="False">
                                    </asp:DropDownList></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Inv_lblInvPayID %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtInvPayID" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td class="baseLable" style="width: 142px">
                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Inv_lblChargeTypeName %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtChargeTypeName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px; height: 28px;" class="baseLable">
                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Inv_lblDepAmL %>"></asp:Label></td>
                                    <td style="width: 114px; height: 28px;">
                                        <asp:TextBox ID="txtDepAmt" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                                    <td align="center" colspan="3" rowspan="4" style="padding-right: 15px" valign="top">
                                        <asp:GridView ID="gdvwInvHeader" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderColor="#E1E0B2" Width="85%" PageSize="4" AllowPaging="True" OnPageIndexChanging="gvShopBrand_OnPageIndexChanging">
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
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Invoice_lblTotalSurBalAmt %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtPayOutAmtSum" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                            ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td style="width: 142px" class="baseLable">
                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Invoice_lblSurBalType %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:DropDownList ID="dropDepBalType" runat="server" Width="167px">
                                    </asp:DropDownList></td>
                                </tr>
                                <tr class="rowHeight">
                                    <td class="baseLable" style="width: 142px">
                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Inv_lblPayOutAmt %>"></asp:Label></td>
                                    <td style="width: 114px">
                                        <asp:TextBox ID="txtDepBalAmt" runat="server" CssClass="ipt160px" style="ime-mode:disabled;"></asp:TextBox></td>
                                </tr>
                                <tr style="height:45px">
                                    <td style="width: 142px; height: 45px;">
                                    </td>
                                    <td style="width: 144px; height: 45px;">
                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                            Text="<%$ Resources:BaseInfo,User_btnOk %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;
                                        <asp:Button ID="btnCancel" runat="server"
                                                CssClass="buttonCancel" OnClick="btnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                    <td style="width: 69px; height: 45px;">
                                    </td>
                                    <td style="text-align: right; padding-right:15px; height: 45px;" colspan="2">
                                        &nbsp;
                                    </td>
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
