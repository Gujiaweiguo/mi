<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CurExRate.aspx.cs" Inherits="Lease_CurrencyParam_CurExRate" ResponseEncoding="gb2312" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Lease_lblCurExRate")%></title>
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
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
            addTabTool("<%=baseInfo %>,Lease/CurrencyParam/CurExRate.aspx");
            loadTitle();
//            document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
        }
        function CheckData()
    {
        if(isEmpty(document.all.txtExRate.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
            document.all.txtExRate.focus();
            return false;					
        }
        if(isInteger(document.all.txtExRate.value)==false)  
        {
            parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Lease_lblExRateError") %>';
            document.all.txtExRate.select();
            return false;					
        }
        if(isEmpty(document.all.txtDate.value))  
        {
            parent.document.all.txtWroMessage.value='汇率时间不能为空。';
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Lease_lblCurExRate %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                         <tr style="height:5px">
                                             <td style="height: 5px">
                                             </td>
                                             <td style="height: 5px">
                                             </td>
                                             <td style="height: 5px">
                                             </td>
                                             <td style="width: 497px; height: 5px;">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblCurTypeID %>" CssClass="labelStyle" Width="92px"></asp:Label></td>
                                             <td><asp:DropDownList ID="dropCurTypeID" runat="server" Width="165px">
                                             </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblToCurTypeID %>" CssClass="labelStyle" Width="87px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:DropDownList ID="DropToCurTypeID" runat="server" Width="165px">
                                                 </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblStatus %>"></asp:Label></td>
                                             <td>
                                                 <asp:DropDownList ID="dropStatus" runat="server" Width="165px">
                                                 </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblExRate %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtExRate" runat="server" CssClass="ipt160px" MaxLength="12"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblExRateDate %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtDate" runat="server" CssClass="ipt160px" MaxLength="32" onclick="calendar()" ReadOnly="False"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ChargeType_lblNote %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
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
                                                 <asp:GridView ID="gvCurExRate" runat="server" AutoGenerateColumns="False"  BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                                     Width="96%" BackColor="White" OnSelectedIndexChanged="gvCurExRate_SelectedIndexChanged" OnRowDataBound="gvCurExRate_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvCurExRate_PageIndexChanging">
                                                     <Columns>
                                                        <asp:BoundField DataField="ExRateID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="CurTypeName" HeaderText="<%$ Resources:BaseInfo,Lease_lblCurTypeID %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="ToCurTypeName" HeaderText="<%$ Resources:BaseInfo,Lease_lblToCurTypeID %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="StatusName" HeaderText="<%$ Resources:BaseInfo,Lease_lblStatus %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="ExRate" HeaderText="<%$ Resources:BaseInfo,Lease_lblExRate %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="ExRateDate" HeaderText="<%$ Resources:BaseInfo,Lease_lblExRateDate %>" DataFormatString="{0:d}" HtmlEncode="False" >
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
                                             <td align="right" colspan="4" rowspan="2">
                                                 &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave"  OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                     Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>"  />
                                                 <asp:Button ID="btnEdit"
                                                         runat="server" CssClass="buttonEdit"  OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                       />
                                                 <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                             Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                                                 &nbsp; &nbsp; &nbsp;&nbsp;
                                                 </td>
                                         </tr>
                                         <tr style="height:4px">
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
