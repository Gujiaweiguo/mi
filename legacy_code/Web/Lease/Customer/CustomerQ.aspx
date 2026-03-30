<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerQ.aspx.cs" Inherits="Lease_Customer_CustomerQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
     <title><%= (String)GetGlobalResourceObject("BaseInfo", "Customer_labBusinessManQuery")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
        <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
//	    	document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
	        addTabTool("<%= strFresh %>,Lease/Customer/CustomerQ.aspx");
	        loadTitle();
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
    </script>
</head>
<body topmargin=0 leftmargin=0 onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="showmain" border="0" cellpadding="0" cellspacing="0"
            style="height: 445px; width:100%">
            
            <tr>
                <td style="vertical-align: top; width: 100%; height: 401px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
            <div id="PotCustomerList">
            <table style="width: 100%; height: 405px" cellspacing="0" cellpadding="0" border="0">
                <tbody>
                    <tr>
                        <td style="height: 25px; vertical-align: middle; text-align: left;" class="tdTopBackColor" valign="top">
                            <img class="imageLeftBack" alt="" />
                            <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Customer_labBusinessManQuery %>" Width="163px"></asp:Label></td>
                        <td style=" height: 25px; text-align: right" class="tdTopRightBackColor"
                            valign="top" colspan="2">
                            <img class="imageRightBack" alt="" /></td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 329px; text-align: center" class="tdBackColor" valign="top"
                            colspan="3">
                            <table style="width: 100%; height: 380px" cellspacing="0" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td style=" height: 1px; background-color: white" colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=" height: 5px" class="tdBackColor" colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=" height: 22px; text-align: right;" class="tdBackColor">
                                            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblQueryCondition %>"></asp:Label></td>
                                        <td style=" height: 22px" class="tdBackColor">
                                        </td>
                                        <td style="height: 22px; text-align: left" class="tdBackColor">
                                            <asp:DropDownList ID="cmbCustTypeq" runat="server" Width="133px" AutoPostBack="True" OnSelectedIndexChanged="cmbCustTypeq_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                        <td style="height: 22px" class="tdBackColor">
                                        </td>
                                        <td style=" height: 22px; text-align: right" class="tdBackColor">
                                            </td>
                                        <td style=" height: 22px" class="tdBackColor">
                                        </td>
                                        <td style="height: 22px; text-align: right" class="tdBackColor">
                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="textstyle"></asp:TextBox>
                                            <asp:DropDownList ID="dropCondit" runat="server" Width="122px">
                                            </asp:DropDownList></td>
                                        <td style=" height: 22px" class="tdBackColor">
                                            <asp:Button ID="btnQuery" OnClick="btnQuery_Click" runat="server" CssClass="buttonQuery"
                                                Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 12px; text-align: center; padding-right:20px" class="tdBackColor" colspan="8">
                                            <table style="left: 12px; width: 100%; position: relative" cellspacing="0" cellpadding="0"
                                                border="0">
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
                                        <td style="width: 100%; height: 260px; text-align: center; vertical-align: top;" class="tdBackColor" colspan="8">
                                            <table style="width: 100%; height: 260px;">
                                                <tbody>
                                                    <tr>
                                                        <td style="left: 7px; vertical-align: top; width: 100%; position: relative; text-align: center; padding-right:20px">
                                                            <asp:GridView ID="GrdCust" runat="server" BorderWidth="1px" BorderStyle="Inset" CellPadding="3" BackColor="White" Width="100%" Height="258px"
                                                                AutoGenerateColumns="False" OnSelectedIndexChanged="GrdCust_SelectedIndexChanged"
                                                                OnRowDataBound="GrdCust_RowDataBound" AllowPaging="True" PageSize="8" OnPageIndexChanging="GrdCust_OnPageIndexChanging">
                                                                <Columns>
                                                                    <asp:BoundField DataField="CustID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CustCode" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CustName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CustShortName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="CustTypeName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_ADDArchives %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ContactMan" HeaderText="<%$ Resources:BaseInfo,PotCustomer_Contact %>">
                                                                        <HeaderStyle CssClass="hidden" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2"　CssClass="hidden" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:CommandField>
                                                                </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
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
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                       
                                        <td colspan="8" style="vertical-align: top; width:100%; height: 44px; left: 0px; text-align:center;" class="tdBackColor"  >
                                            &nbsp;
                                            <table style="width: 98%;" cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495; left: 0px;top:-5px; position: relative;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff; left: 0px; top:-5px; position: relative;">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                                Text="<%$ Resources:BaseInfo,Button_back %>" Visible="False" /><asp:Button ID="btnNext" runat="server"
                                                    CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Visible="False" />
                                        
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
