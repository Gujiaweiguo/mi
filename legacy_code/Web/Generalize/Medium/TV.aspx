<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TV.aspx.cs" Inherits="Generalize_Medium_TV" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "TV_lblTV")%></title>
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
      <script type="text/javascript">
      <!--	    
	     function Load()
        {
            addTabTool("<%=baseInfo %>,Generalize/Medium/TV.aspx");
            loadTitle();
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,TV_lblTV %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                             <td class="baseLable" style="height: 24px">
                                                 <asp:Label ID="lblPrintsNm" runat="server" Text="<%$ Resources:BaseInfo,TV_lblTVNm %>" CssClass="labelStyle" Width="92px"></asp:Label></td>
                                             <td style="height: 24px">
                                                 <asp:TextBox ID="txtTVNm" runat="server" CssClass="ipt160px" MaxLength="32"
                                                     Width="156px"></asp:TextBox></td>
                                             <td class="baseLable" style="height: 24px">
                                                 <asp:Label ID="lblAddr" runat="server" Text="<%$ Resources:BaseInfo,Prints_lblAddr %>" CssClass="labelStyle" Width="87px"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px; height: 24px;">
                                                 <asp:TextBox ID="txtAddr" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="lblOffPhone" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Prints_lblOffPhone %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtOffPhone" runat="server" CssClass="ipt160px" MaxLength="32"
                                                     Width="156px"></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="lblFax" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblFax %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtFax" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="lblWeb" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Prints_lblWeb %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtWeb" runat="server" CssClass="ipt160px" MaxLength="32" Width="156px" ></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="lblTitle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorDuty %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtTitle" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="lblUserName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"></asp:Label></td>
                                             <td>
                                                 <asp:TextBox ID="txtUserName" runat="server" CssClass="ipt160px" MaxLength="32" Width="156px" ></asp:TextBox></td>
                                             <td class="baseLable">
                                                 <asp:Label ID="lblMobileTel" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_MobileTel %>"></asp:Label></td>
                                             <td class="baseInput" style="width: 497px">
                                                 <asp:TextBox ID="txtMobileTel" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable">
                                                 <asp:Label ID="lblStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                                                     Width="90px"></asp:Label></td>
                                             <td class="baseLable" style="text-align: left" colspan="3">
                                                 <asp:DropDownList ID="cmbStatus" runat="server" CssClass="cmb160px" Width="160px">
                                                 </asp:DropDownList></td>
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
                                                 <asp:GridView ID="gvTV" runat="server" AutoGenerateColumns="False"  BorderStyle="Inset" BorderWidth="1px" CellPadding="3"
                                                     Width="100%" BackColor="White" OnSelectedIndexChanged="gvTV_SelectedIndexChanged" PageSize="7">
                                                     <Columns>
                                                        <asp:BoundField DataField="TVID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="TVNm" HeaderText="<%$ Resources:BaseInfo,TV_lblTVNm %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="ContactNm" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Title" HeaderText="<%$ Resources:BaseInfo,Associator_AssociatorDuty %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Phone" HeaderText="<%$ Resources:BaseInfo,Rpt_MobileTel %>" >
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                         </asp:BoundField>
                                                         <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:CommandField>
                                                     </Columns>
                                                 </asp:GridView>
                                             </td>
                                         </tr>
                                         <tr>
                                             <td>
                                                 <asp:Label ID="lblTotalNum" runat="server" Height="1px" Width="32px"></asp:Label><asp:Label
                                                     ID="lblCurrent" runat="server" ForeColor="Red" Height="1px" Width="1px">1</asp:Label></td>
                                             <td>
                                                 <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" Height="31px"
                                                     OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>" Width="71px" />
                                                 <asp:Button ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" Height="30px"
                                                     OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Width="73px" /></td>
                                             <td style="text-align: right" colspan="2">
                                                 <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                                                     Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="70px" /><asp:Button ID="btnEdit"
                                                         runat="server" CssClass="buttonEdit" Height="30px" OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>"
                                                         Width="70px" Enabled="False" /><asp:Button ID="btnCel" runat="server" CssClass="buttonClear" OnClick="btnCel_Click"
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
