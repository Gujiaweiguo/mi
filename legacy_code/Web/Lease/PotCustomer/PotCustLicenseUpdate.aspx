<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustLicenseUpdate.aspx.cs" Inherits="Lease_PotCustomer_PotCustLicenseUpdate" ResponseEncoding="gb2312" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Title_CustLicense")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
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
        function CheckNull()
        {
            if(isEmpty(document.all.txtLicenseName.value))  
            {
                parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "PotCustomer_lblCustLicenseName")%>'+document.getElementById("hidMessage").value;
                document.all.txtLicenseName.focus();
                return false;
            }
        }
	</script>
</head>
<body onload='Load()' style="margin:0px">
    <form id="form1" runat="server">
    
        <table id="showmain" border="0" cellpadding="0" cellspacing="0"
            style="width:100%">
            <tr>
                <td style="vertical-align: top; width: 100%;">
       
             <div style="width: 100%; height: 100%" id="PotCustLicense">
                <table border="0" cellpadding="0" cellspacing="0" style="height: 405px; width:100%;">
                    <tr>
                        <td class="tdTopBackColor" style="height: 25px; vertical-align: middle; text-align: left; width: 509px;" valign="top">
                            <img alt="" class="imageLeftBack" /><asp:Label ID="Label5" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Title_CustLicense %>" Width="344px"></asp:Label></td>
                        <td class="tdTopRightBackColor" colspan="2" style=" height: 25px" valign="top">
                            <img class="imageRightBack" /></td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="2" style="width: 100%; height: 330px; text-align: center"
                            valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 216px; width: 100%;">
                                 <tr>
                                    <td style="width: 280px; height: 1px; background-color: white" colspan="4">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="5" style="width: 100%; height: 22px; text-align: right; padding-left:100px" >
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; text-align:right">
                                            <tr>
                                                <td style="width: 100%; height: 1px; background-color: #738495;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%; height: 1px; background-color: #ffffff;">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px; text-align: center; vertical-align: top;"
                                        valign="bottom">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style=" height: 22px; text-align: right; width: 212px;">
                                        <asp:Label ID="lblCustLicenseCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseCode %>"
                                            Width="69px"></asp:Label></td>
                                    <td class="tdBackColor" style="height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="height: 22px">
                                        <asp:TextBox ID="txtLicenseID" runat="server" CssClass="ipt160px" MaxLength="8"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 50px; height: 22px">
                                        </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style=" height: 22px; text-align: right; width: 212px;">
                                        <asp:Label ID="lblCustLicenseName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseName %>"></asp:Label></td>
                                    <td class="tdBackColor" style="height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="height: 22px">
                                        <asp:TextBox ID="txtLicenseName" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 13px; height: 22px" align="left">
                                        <img id="ImgCustName" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right; width: 212px;">
                                        <asp:Label ID="lblCustLicenseType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseType %>"></asp:Label></td>
                                    <td class="tdBackColor" style="height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="height: 22px">
                                        <asp:DropDownList ID="cmbLicenseType" runat="server" Width="163px" >
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="width: 13px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="height: 22px; text-align: right; width: 212px;">
                                        <asp:Label ID="lblCustLicenseStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
                                    <td class="tdBackColor" style="height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="height: 22px">
                                        <asp:TextBox ID="txtLicenseBeginDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 13px; height: 22px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style=" height: 22px; text-align: right; width: 212px;">
                                        <asp:Label ID="lblCustLicenseEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Label></td>
                                    <td class="tdBackColor" style="height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="height: 22px">
                                        <asp:TextBox ID="txtLicenseEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 13px; height: 22px">
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 10px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style=" height: 22px; text-align: right; width: 212px;">
                                       <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_ClientCard %>"></asp:Label><asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_Update %>"></asp:Label></td>
                                    <td class="tdBackColor" style="height: 22px">
                                    </td>
                                    <td class="tdBackColor" style="height: 22px">
                                        <asp:FileUpload ID="FileUpload1" runat="server" class="ipt160px" /></td>
                                    <td class="tdBackColor" style="width: 33px; height: 22px">
                                        <asp:LinkButton ID="btnLook" runat="server" 
                                            Text="<%$ Resources:BaseInfo,User_lblUserQuery %>" Enabled="False" 
                                            onclick="btnLook_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="tdBackColor" colspan="4" style="width: 255px; height: 22px; text-align: center">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="5" style="width: 100%; height: 22px; text-align: right; padding-left:100px">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                            <tr>
                                                <td style="width: 100%; height: 1px; background-color: #738495;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%; height: 1px; background-color: #ffffff;">
                                                </td>
                                            </tr>
                                        </table>
                                      </td>
                                </tr>
                            </table>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" OnClick="btnAdd_Click"
                                Text="<%$ Resources:BaseInfo,PotCustomer_butAdd %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="btnEdit"
                                    runat="server" CssClass="buttonEdit" OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel"
                                        OnClick="btnQuit_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                        <td class="tdBackColor" style="width: 100%; height: 351px" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 311px" width="100%">
                                <tr>
                                    <td style="width: 100%; height: 1px; background-color: white">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 100%; height: 22px; text-align: center">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 267px">
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
                                    <td class="tdBackColor" style="width:100%; height: 254px; text-align: center; padding-left:20px; padding-right:20px" valign="top">
                                        <asp:GridView ID="GrdCustLicense" runat="server" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="285px"
                                            OnRowDataBound="GrdCustLicense_RowDataBound" OnSelectedIndexChanged="GrdCustLicense_SelectedIndexChanged"
                                            PageSize="11" Width="260px" AllowPaging="True" OnPageIndexChanging="GrdCustLicense_PageIndexChanging">
                                            <Columns>
                                                <asp:BoundField DataField="CustLicenseID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                    <FooterStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustLicenseName" HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseName %>">
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustLicenseStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>">
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="CustLicenseEndDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>">
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                </asp:BoundField>
                                                <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" ShowSelectButton="True">
                                                    <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
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
                                    <td class="tdBackColor" style="width: 100%; height: 22px; text-align: center">
                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 267px">
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
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
             
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidWrite" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidWrite %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
    </form>
</body>
</html>
