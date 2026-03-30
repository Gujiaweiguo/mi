<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustContact.aspx.cs" Inherits="Lease_PotCustomer_PotCustContact" EnableEventValidation="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ContactVindicate")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
<script type="text/javascript">
  //text控件文本验证
    function allTextBoxValidator()
    {
        if(isEmpty(document.all.txtName.value))  
        {
            alert('Name is Empty!');
            document.all.txtName.focus();
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
</script>
<base target="_self">  
</head>
<body style="margin-top:0; margin-left:0">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                    <%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ContactVindicate")%>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 405px">
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 50%; height: 320px; text-align: right"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 221px; width: 305px;">
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px" valign="bottom">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 100%; height: 5px; text-align: center"
                                valign="bottom">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 4px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px" valign="bottom">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserName %>"
                                    Width="69px"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtName" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorDuty%>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtDuty" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_TakeChargeArea %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtChargeArea" runat="server" CssClass="ipt160px" 
                                    MaxLength="16"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorOfficeTel %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtphone" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height:8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblFax %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtFax" runat="server" CssClass="ipt160px" MaxLength="15"></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="lblCustLicenseStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblMobileTel %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtmobil" runat="server" CssClass="ipt160px" MaxLength="16" ></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 8px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 84px; height: 22px; text-align: right">
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorEmail %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 4px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 160px; height: 22px">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="ipt160px" MaxLength="64" ></asp:TextBox></td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 100%; height: 22px; text-align: center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 80%">
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 23px; text-align: center">
                                <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" OnClick="btnAdd_Click"
                                    Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Enabled="False"
                                    OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnQuit_Click"
                                    Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                        </tr>
                    </table>
                </td>
                <td class="tdBackColor" style="width: 280px; height: 330px" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 286px; height: 311px">
                        <tr>
                            <td class="tdBackColor" style="width: 302px; height: 22px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 275px">
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="vertical-align: top; width: 302px; height: 254px;
                                text-align: center" valign="top">
                                <asp:GridView ID="GrdCustBrand" runat="server" AutoGenerateColumns="False" BackColor="White"
                                    BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="297px"
                                    OnSelectedIndexChanged="GrdCustBrand_SelectedIndexChanged" Width="271px" OnRowDataBound="GrdCustBrand_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdCustBrand_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="CustContactId">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ContactorName" HeaderText="<%$ Resources:BaseInfo,User_lblUserName %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ManageArea" HeaderText="<%$ Resources:BaseInfo,PotCustomer_TakeChargeArea %>">
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                            <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                         <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                             <ItemStyle BorderColor="#E1E0B2" />
                                        </asp:CommandField>
                                    </Columns>
                                    <FooterStyle BackColor="Red" ForeColor="#000066" />
                                    <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
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
                            <td class="tdBackColor" style="width: 302px; height: 22px; text-align: center;">
                            </td>
                        </tr>
                    </table>
                    </td>
            </tr>
        </table>
    
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
    </form>
</body>
</html>

