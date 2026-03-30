<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceBancthPrint.aspx.cs" Inherits="Lease_ChargeAccount_InvoiceBancthPrint" ResponseEncoding="gb2312" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Invoice_BacthPrint")%></title>
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
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
      <script language="javascript" type="text/javascript" src="../../JavaScript/Calendar.js"></script>
      <script type="text/javascript">
      <!--	    
	     function Load()
        {
            addTabTool("<%=strFresh %>,Lease/ChargeAccount/InvoiceBancthPrint.aspx");
            loadTitle();
//            document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Invoice_BacthPrint %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>" CssClass="labelStyle" Width="92px"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" MaxLength="32" Width="159px" onclick="calendar()"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Rpt_EDate %>" CssClass="labelStyle" Width="87px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" MaxLength="32" onclick="calendar()"></asp:TextBox></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                     Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_RptType %>"></asp:Label></td>
                                             <td>
                                                 <asp:DropDownList ID="dropRptType" runat="server" Width="164px">
                                                 </asp:DropDownList></td>
                                             <td class="baseLable">
                                             </td>
                                             <td class="baseInput" style="width: 497px">
                                             </td>
                                             <td class="baseInput" style="width: 497px">
                                             </td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td class="baseLable">
                                             </td>
                                             <td colspan="3">
                                             </td>
                                             <td colspan="1">
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
                                                 <asp:GridView ID="gvChargeType" runat="server" AutoGenerateColumns="False"  BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                                     Width="100%" BackColor="White" OnSelectedIndexChanged="gvChargeType_SelectedIndexChanged" OnRowDataBound="gvChargeType_RowDataBound" AllowPaging="True" PageSize="8" OnPageIndexChanging="GrdCust_OnPageIndexChanging">
                                                     <Columns>
                                                         <asp:BoundField DataField="CreateTime" HeaderText="<%$ Resources:BaseInfo,Rpt_InvDate %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="BancthID" HeaderText="<%$ Resources:BaseInfo,Rpt_BatchId %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,btn_lblPrint %>" SelectText="<%$ Resources:BaseInfo,btn_lblPrint %>" >
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:CommandField>
                                                     </Columns>
                                                     <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="right" />
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
                                             <td>
                                                 </td>
                                             <td>
                                                 &nbsp;</td>
                                             <td style="text-align: right" colspan="2">
                                                 <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False"
                                                     OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Visible="False"/>
                                                 <asp:Button ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False"
                                                     OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" Visible="False"/></td>
                                             <td colspan="1" style="text-align: right">
                                             </td>
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
