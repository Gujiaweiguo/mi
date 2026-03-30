<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdBoardTypeDefend.aspx.cs" Inherits="Lease_AdContract_AdBoardTypeDefend" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "AdContract_lblAdTypeDefend")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
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
            addTabTool("<%=baseInfo %>,Lease/AdContract/AdBoardTypeDefend.aspx");
            loadTitle();
        }
         //输入验证
        function InputValidator(sForm)
        {
            if(isEmpty(document.all.txtAdBordCode.value))
            {
                alert('<%= enterInfo %>');
                document.all.txtAdBordCode.focus();
                return false;
            }
            if(isEmpty(document.all.txtAdBordName.value))
            {
                alert('<%= enterInfo %>');
                document.all.txtAdBordName.focus();
                return false;
            }
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,AdContract_lblAdTypeDefend %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,CustType_lblCustTypeCode %>" CssClass="labelStyle"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtAdBordCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,CustType_lblCustTypeName %>" Width="80px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtAdBordName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeStatus %>"
                                                     Width="80px"></asp:Label></td>
                                             <td>
                                                 <asp:DropDownList ID="ddlStatus" runat="server" Width="163px">
                                                 </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 </td>
                                             <td class="baseInput" style="width: 497px">
                                                 </td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 </td>
                                             <td>
                                                 </td>
                                             <td class="baseLable">
                                                 </td>
                                             <td class="baseInput" style="width: 497px">
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
                                                 <asp:GridView ID="gvAdBoard" runat="server" AutoGenerateColumns="False"  BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                                     Width="100%" BackColor="White" OnRowDataBound="gvAdBoard_RowDataBound" OnSelectedIndexChanged="gvAdBoard_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvAdBoard_PageIndexChanging">
                                                     <Columns>
                                                         <asp:BoundField DataField="AdBoardTypeID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardTypeCode" HeaderText="<%$ Resources:BaseInfo,CustType_lblCustTypeCode %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardTypeName" HeaderText="<%$ Resources:BaseInfo,CustType_lblCustTypeName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         
                                                         <asp:BoundField DataField="AdBoardTypeStatus" HeaderText="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeStatus %>" >
                                                             <ItemStyle CssClass="hidden"/>
                                                             <HeaderStyle CssClass="hidden"/>
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AdBoardTypeStatus" HeaderText="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeStatus %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         
                                                         <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:CommandField>
                                                     </Columns>
                                                     <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
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
                                             <td>
                                                 </td>
                                             <td>
                                                 &nbsp;</td>
                                             <td style="text-align: right" colspan="2">
                                                 <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                                                     Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" /><asp:Button ID="btnEdit"
                                                         runat="server" CssClass="buttonEdit" Height="30px" OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>"
                                                          /><asp:Button ID="btnCel" runat="server" CssClass="buttonClear" OnClick="btnCel_Click"
                                                             Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
                                         </tr>
                                         <tr style="height:10px">
                                             <td>
                                             </td>
                                             <td>
                                             </td>
                                             <td colspan="2" style="text-align: right">
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

