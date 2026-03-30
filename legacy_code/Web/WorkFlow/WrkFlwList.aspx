<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WrkFlwList.aspx.cs" Inherits="WorkFlow_WrkFlwList"  %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "WrkFlw_QueryList")%></title>
        <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
   <script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
function showline()
{
    parent.document.all.txtWroMessage.value = "";
    addTabTool("<%=baseInfo %>,WorkFlow/WrkFlwList.aspx");
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
<body onload='showline();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 400px; width: 100%; vertical-align:top;">
                        <tr  style="width:100%; vertical-align:top; height:20px;">
                           <td style="vertical-align:top;">
                                <table border="0" cellpadding="0" cellspacing="0" style=" width: 100%; vertical-align:top;">
                                    <tr style="vertical-align:top;">
                                        <td class="tdTopRightBackColor" valign="top" style=" height: 22px; text-align: left">
                                            <img alt="" class="imageLeftBack" style="text-align: left; height: 22px;" />
                                        </td>
                                        <td class="tdTopRightBackColor" style=" height: 22px; text-align: left;">
                                            <asp:Label ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,WrkFlw_QueryList %>'
                                                Height="12pt" Width="536px"></asp:Label></td>
                                        <td class="tdTopRightBackColor" valign="top" style=" height: 22px; text-align: right;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                </table>
                            </td> 
                        </tr>
                        <tr style="vertical-align:top;">
                            <td style="vertical-align: top; text-align: center; height: 303px; width:100%;" class="tdBackColor" colspan="3">
                            <table style="vertical-align: top; text-align: center; height: 359px; width:100%;">
                                <tr style="vertical-align:top;">
                                    <td style="width: 100%; height: 12px; text-align: center; vertical-align:top;" class="tdBackColor" colspan="8">
                                        <table style="left: 0px; width: 98%; position: relative; text-align:center; vertical-align:top;" cellspacing="0" cellpadding="0"
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
                            <tr style="vertical-align:top;">
                                 <td style="width: 100%; height: 200px;" >                            
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                    OnRowDataBound="GridView1_RowDataBound" Width="98%" OnRowEditing="GridView1_RowEditing"
                                    BackColor="White" BorderStyle="Inset" AllowPaging="True" OnPageIndexChanging="GridView1_OnPageIndexChanging" CellPadding="4">
                                    <Columns>
                                        <asp:BoundField DataField="WrkFlwID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WrkFlwName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblWorkFlow %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="VoucherTypeID" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblVoucherTypeID %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="InitVoucherName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblInitVoucher %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Efficiency" HeaderText="<%$ Resources:BaseInfo,WrkFlw_Efficiency %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="TraceDays" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblTraceDay %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="IfTransitName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblTransit %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ProcessClass" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblNodeProcessClass %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="WrkFlwStatusName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_Activity %>">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2"/>
                                            <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" />
                                        </asp:CommandField>
                                    </Columns>
                                                <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" Font-Size="Small" />
                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                <PagerTemplate>                                                   
                                                <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
                                                Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="X-Small">首页</asp:LinkButton> 

                                                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
                                                Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="X-Small">上一页</asp:LinkButton> 

                                                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
                                                Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="X-Small">下一页</asp:LinkButton> 

                                                <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
                                                Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="X-Small">尾页</asp:LinkButton> 
                                                <asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
                                                <asp:Label ID="LabelPageCount" runat="server" 
                                                Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="X-Small"/> 
                                                </PagerTemplate>         
                                                <PagerSettings Mode="NextPreviousFirstLast"  />

                                                </asp:GridView>
                            
                            </td>
                            </tr>
                            </table>
                            <table style="width: 98%" cellspacing="0" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #738495;top:-10px;  position: relative;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #ffffff; top:-10px; position: relative;">
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            </td>
                        </tr>
                    </table>
    </form>
</body>
</html>





