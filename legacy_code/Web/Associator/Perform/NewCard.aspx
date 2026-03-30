<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewCard.aspx.cs" Inherits="Associator_Perform_NewCard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>发行新会员卡</title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
    function Load()
	    {
	        addTabTool("发行新会员卡,Associator/Perform/NewCard.aspx");
	        loadTitle();
	    }
	    </script>
</head>
<body style="margin-top:0; margin-left:0" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <div>
     <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr >
                    <td>
                        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
                            width: 100%; text-align: center;" >
                            <tr>
                                <td class="tdTopBackColor" style="width: 5px">
                                    <img alt="" class="imageLeftBack" />
                                </td>
                                <td class="tdTopBackColor">
                                    发行新会员卡
                                </td>
                            </tr>
                        </table>
                    </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; height: 329px; text-align: center" class="tdBackColor" valign="top"
                            colspan="3">
                            <table style="width: 100%; height: 380px" cellspacing="0" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td style="width: 495px; height: 5px" class="tdBackColor" colspan="8">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="8" style="height: 22px; width: 100%; text-align: center;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 12px; text-align: center" class="tdBackColor" colspan="8">
                                            <table style="left: 12px; width: 90%; position: relative" cellspacing="0" cellpadding="0"
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
                                            <table style="width: 549px; height: 260px;">
                                                <tbody>
                                                    <tr>
                                                        <td style="left: 7px; vertical-align: top; width: 100%; position: relative; text-align: center">
                                                        </td>
                                                           <td style="left: 7px; vertical-align: top; width: 100%; position: relative; text-align: center">
                                                            <asp:GridView ID="GrdCust" runat="server" BorderWidth="1px" BorderStyle="Inset" 
                                                                   CellPadding="3" BackColor="White" Width="560px" Height="258px"
                                                                AutoGenerateColumns="False" OnSelectedIndexChanged="GrdCust_SelectedIndexChanged"
                                                                OnRowDataBound="GrdCust_RowDataBound" AllowPaging="True" 
                                                                   onpageindexchanging="GrdCust_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:BoundField DataField="MembID">
                                                                        <HeaderStyle CssClass="hidden" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" CssClass="hidden" />
                                                                        <ControlStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MembCode" HeaderText="会员号">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MemberName" HeaderText="会员名">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="HomePhone" HeaderText="家庭电话">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MobilPhone" HeaderText="移动电话">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DateJoint" HeaderText="入会日期" DataFormatString="{0:d}" HtmlEncode="False">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField HeaderText="选择" ShowSelectButton="True">
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
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 55px; height: 12px" class="tdBackColor">
                                        </td>
                                        <td style="height: 12px" class="tdBackColor" colspan="7">
                                            <asp:Label ID="lblTotalNum" runat="server"></asp:Label><asp:Label ID="lblCurrent"
                                                runat="server" ForeColor="Red">1</asp:Label><asp:Button ID="btnBack" runat="server"
                                                    CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click" 
                                                Text="<%$ Resources:BaseInfo,Button_back %>" Visible="False" />
                                            <asp:Button
                                                        ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click"
                                                        Text="<%$ Resources:BaseInfo,Button_next %>" Visible="False" /></td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
            </table>
    </div>
    </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
