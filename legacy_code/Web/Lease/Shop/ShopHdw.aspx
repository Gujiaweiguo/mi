<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopHdw.aspx.cs" Inherits="Lease_Shop_ShopHdw" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_lblShopHdwModi")%></title>
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
            addTabTool("<%=strFresh %>,Lease/Shop/ShopHdw.aspx");
            loadTitle();
//            document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
        }
	    
	    
	     function ShowTree()
        {
        	strreturnval=window.showModalDialog('SelectShop.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
			     var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');
                objImgBtn1.click();
            }
			else
			{
				return;	
			}  
        }
        function CheckData()
        {
            if(isEmpty(document.all.txtShopCode.value))
            {
                parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
                document.all.txtShopCode.focus();
                ShowTree();
                return false;
            }
            
            if(isEmpty(document.all.txtHdwCode.value))
            {
                parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
                document.all.txtHdwCode.focus();
                return false;
            }
            
            if(isEmpty(document.all.txtHdwName.value))  
            {
                parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
                document.all.txtHdwName.focus();
                return false;					
            }
            if(isEmpty(document.all.txtHdwQty.value))
            {
                parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
                document.all.txtHdwQty.focus();
                return false;
            }
           if(isDigit(document.all.txtHdwQty.value)==false)
            {
                parent.document.all.txtWroMessage.value='请输入整数。';
                document.all.txtHdwQty.select();
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Menu_lblShopHdwModi %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                             <td><asp:DropDownList ID="dropHdwTypeID" runat="server" Width="163px">
                                             </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>" CssClass="labelStyle" Width="87px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwCode %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtHdwCode" runat="server" CssClass="ipt160px" Width="160px"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwName %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtHdwName" runat="server" CssClass="ipt160px" Width="160px"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwQty %>"></asp:Label></td>
                                             <td class="baseLable">
                                                 <asp:TextBox ID="txtHdwQty" runat="server" CssClass="ipt160px" Width="160px"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwUnit %>"></asp:Label></td>
                                             <td style="width: 497px" >
                                                 <asp:TextBox ID="txtHdwUnit" runat="server" CssClass="ipt160px" Width="160px"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwStd %>"></asp:Label></td>
                                             <td class="baseLable">
                                                 <asp:TextBox ID="txtHdwStd" runat="server" CssClass="ipt160px" Width="160px"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwCond %>"></asp:Label></td>
                                             <td style="width: 497px"><asp:DropDownList ID="dropHdwCond" runat="server" Width="163px">
                                             </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseShopHdw_lblOwner %>"></asp:Label></td>
                                             <td class="baseLable"><asp:DropDownList ID="dropOwner" runat="server" Width="163px">
                                             </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ChargeType_lblNote %>"></asp:Label></td>
                                             <td style="width: 497px">
                                                 <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" Width="160px"></asp:TextBox></td>
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
                                                     position: relative; top: 6px">
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
                                                     Width="100%" BackColor="White" OnSelectedIndexChanged="gvChargeType_SelectedIndexChanged" OnRowDataBound="gvChargeType_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvChargeType_PageIndexChanging" PageSize="6">
                                                     <RowStyle Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                     <Columns>
                                                         <asp:BoundField DataField="HdwID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="HdwCode" HeaderText="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwCode %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="HdwName" HeaderText="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="ShopCode" HeaderText="<%$ Resources:BaseInfo,Lease_lblShopCode %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="HdwQty" HeaderText="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwQty %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="HdwStd" HeaderText="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwStd %>">
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="HdwCondName" HeaderText="<%$ Resources:BaseInfo,LeaseShopHdw_lblHdwCond %>">
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="OwnerName" HeaderText="<%$ Resources:BaseInfo,LeaseShopHdw_lblOwner %>">
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
                                             <td>
                                                 </td>
                                             <td>
                                                 <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" Height="31px"
                                                     OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>" Width="71px" Visible="False" />
                                                 <asp:Button ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" Height="30px"
                                                     OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Width="73px" Visible="False" /></td>
                                             <td style="text-align: center" colspan="2">
                                                 <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd"  OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                     Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" />
                                                 <asp:Button ID="btnEdit"
                                                         runat="server" CssClass="buttonEdit"  OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                          />
                                                 <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
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
                        <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
             <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>&nbsp;
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
