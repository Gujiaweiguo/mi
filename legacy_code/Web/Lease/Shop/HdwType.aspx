<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HdwType.aspx.cs" Inherits="Lease_Shop_HdwType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "LeaseHdw_lblHdwTypeModi")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
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
      <script type="text/javascript">
         
	     function Load()
        {
            addTabTool("<%=baseInfo %>,Lease/Shop/HdwType.aspx");
            loadTitle();
//            document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
        }
        function CheckData()
    {
        if(isEmpty(document.all.txtHdwTypeName.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
            document.all.txtHdwTypeName.focus();
            return false;					
        }
        
    }
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,LeaseHdw_lblHdwTypeModi %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwTypeName %>" CssClass="labelStyle" Width="92px"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtHdwTypeName" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,LeaseHdw_lblHdwType %>" CssClass="labelStyle" Width="87px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:DropDownList ID="dropHdwClass" runat="server" Width="163px">
                                                 </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseHdw_lblHdwTypeStatus %>"></asp:Label></td>
                                             <td>
                                                 <asp:DropDownList ID="dropHdwTypeStatus" runat="server" Width="165px">
                                                 </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ChargeType_lblNote %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" Width="160px" MaxLength="64"></asp:TextBox></td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td class="baseLable">
                                             </td>
                                             <td colspan="3">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="4" style="text-align: center">
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
                                             <td colspan="4" style="text-align: center">
                                                 <asp:GridView ID="gvChargeType" runat="server" AutoGenerateColumns="False"  BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                                     Width="96%" BackColor="White" OnSelectedIndexChanged="gvChargeType_SelectedIndexChanged" OnRowDataBound="gvChargeType_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvChargeType_PageIndexChanging">
                                                     <RowStyle Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                     <Columns>
                                                         <asp:BoundField DataField="HdwTypeID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="HdwTypeName" HeaderText="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwTypeName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="HdwClassName" HeaderText="<%$ Resources:BaseInfo,LeaseHdw_lblHdwType %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="HdwTypeStatusName" HeaderText="<%$ Resources:BaseInfo,LeaseHdw_lblHdwTypeStatus %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Note" HeaderText="<%$ Resources:BaseInfo,ChargeType_lblNote %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:CommandField>
                                                          <asp:BoundField DataField="HdwClass">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                          <asp:BoundField DataField="HdwTypeStatus">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                     </Columns>
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
                                             <td align="right" colspan="4">
                                                 &nbsp;&nbsp; &nbsp; &nbsp; 
                                                 <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"  OnClick="btnSave_Click"
                                                     Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />
                                                 <asp:Button ID="btnEdit" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                         runat="server" CssClass="buttonEdit"  OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>"
                                                          />
                                                 <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                             Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                                                 &nbsp; &nbsp; &nbsp;&nbsp;
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
