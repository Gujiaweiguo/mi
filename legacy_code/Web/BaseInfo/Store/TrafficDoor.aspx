<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrafficDoor.aspx.cs" Inherits="BaseInfo_Store_TrafficDoor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_BuildingEntryManage")%></title>
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
            addTabTool("<%=baseInfo %>,BaseInfo/Store/TrafficDoor.aspx");
            loadTitle();
        }
        function CheckData()
    {
        if(isEmpty(document.all.txtEntryName.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(string)GetGlobalResourceObject("BaseInfo", "Hidden_hidMessage") %>';
            document.all.txtEntryName.focus();
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Menu_BuildingEntryManage %>' Height="12pt" Width="218px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style=" height: 27px; text-align: right;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                            <tr class="headLine">
                            <td colspan="3" style="height: 0px"></td>
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
                                                 <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>" CssClass="labelStyle" Width="92px"></asp:Label></td>
                                             <td><asp:DropDownList ID="ddlStore" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                                             </asp:DropDownList></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblSelBuildingID %>" CssClass="labelStyle" Width="87px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:DropDownList ID="ddlBuilding" runat="server" Width="165px">
                                                 </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Traffic_TrafficDoorName %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtEntryName" runat="server" CssClass="ipt160px" MaxLength="10"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_Status %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:DropDownList ID="ddlStatus" runat="server" Width="165px">
                                                 </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ChargeType_lblNote %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="10"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 </td>
                                             <td class="baseInput" style="width: 497px">
                                                 </td>
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
                                                 <asp:GridView ID="gvTrafficEntry" runat="server" AutoGenerateColumns="False"  BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                                     Width="96%" BackColor="White" OnSelectedIndexChanged="gvTrafficEntry_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="gvTrafficEntry_PageIndexChanging">
                                                     <Columns>
                                                        <asp:BoundField DataField="doorid">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="deptname" HeaderText="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="buildingname" HeaderText="<%$ Resources:BaseInfo,RentableArea_lblSelBuildingID %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="doorname" HeaderText="<%$ Resources:BaseInfo,Traffic_TrafficDoorName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="doorstatus" HeaderText="<%$ Resources:BaseInfo,Rpt_Status %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="note" HeaderText="<%$ Resources:BaseInfo,ChargeType_lblNote %>" >
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
