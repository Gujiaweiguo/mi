<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CurrencyType.aspx.cs" Inherits="Lease_CurrencyParam_CurrencyType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Lease_lblCurrencyType")%></title>
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
      <script language="javascript" type="text/javascript" src="../../JavaScript/Common.js"></script>
      <script type="text/javascript">
      <!--	    
	     function Load()
        {
            addTabTool("<%=baseInfo %>,Lease/CurrencyParam/CurrencyType.aspx");
            loadTitle();
//            document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
        }
        
         //输入验证
        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtCurTypeName.value))
            {
//                alert('<%= enterInfo %>');
                parent.document.all.txtWroMessage.value='<%= (String)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage")%>';
                document.all.txtCurTypeName.focus();
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Lease_lblCurrencyType %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblCurTypeName %>" CssClass="labelStyle" Width="92px"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtCurTypeName" runat="server" CssClass="ipt160px" MaxLength="8"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblCurTypeStatus %>" CssClass="labelStyle" Width="87px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:DropDownList ID="dropCurTypeStatus" runat="server" Width="165px">
                                                 </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblIsLocal %>"></asp:Label></td>
                                             <td>
                                                 <asp:DropDownList ID="dropIsLocal" runat="server" Width="163px">
                                                 </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ChargeType_lblNote %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="64" Width="158px"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 </td>
                                             <td class="baseLable" style="text-align: left" colspan="3">
                                                 </td>
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
                                                 <asp:GridView ID="gvCurrencyType" runat="server" AutoGenerateColumns="False"  BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                                     Width="96%" BackColor="White" OnSelectedIndexChanged="gvCurrencyType_SelectedIndexChanged" OnRowDataBound="gvCurrencyType_RowDataBound" AllowSorting="True" AllowPaging="True" OnPageIndexChanging="gvCurrencyType_PageIndexChanging">
                                                     <RowStyle Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                     <Columns>
                                                         <asp:BoundField DataField="CurTypeID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="CurTypeName" HeaderText="<%$ Resources:BaseInfo,Lease_lblCurTypeName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="curTypeStatusName" HeaderText="<%$ Resources:BaseInfo,Lease_lblCurTypeStatus %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="isLocalName" HeaderText="<%$ Resources:BaseInfo,Lease_lblIsLocal %>" >
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
                                                           <asp:BoundField DataField="CurTypeStatus">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                           <asp:BoundField DataField="IsLocal">
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
                                                 &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave"  OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                     Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" />
                                                 <asp:Button ID="btnEdit" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                         runat="server" CssClass="buttonEdit"  OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" />
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
