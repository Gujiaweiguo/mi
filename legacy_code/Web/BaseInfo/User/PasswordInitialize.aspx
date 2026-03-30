<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PasswordInitialize.aspx.cs" Inherits="BaseInfo_User_PasswordInitialize" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
         <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_PasswordInitialize")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
		<script type="text/javascript">
	    function Load()
	    {
	        addTabTool("<%=strFresh %>,BaseInfo/User/PasswordInitialize.aspx");
	        loadTitle();
//	        document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
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
<body  style="margin-top:0; margin-left:0" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
    <table style="width: 100%" cellpadding="0" cellspacing="0">
<%--                    <tr>
                        <td style="width: 826px; height: 25px; vertical-align: middle; text-align: left;" class="tdTopBackColor" valign="top">
                            <img class="imageLeftBack" alt="" />
                            <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_labCustomerQuery %>" Width="420px"></asp:Label></td>
                        <td style="width: 538px; height: 25px; text-align: right" class="tdTopRightBackColor"
                            valign="top" colspan="2">
                            <img class="imageRightBack" alt="" /></td>
                    </tr>--%>
                    <tr >
                    <td>
                        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
                            width: 100%; text-align: center;" >
                            <tr>
                                <td class="tdTopBackColor" style="width: 5px">
                                    <img alt="" class="imageLeftBack" />
                                </td>
                                <td class="tdTopBackColor">
                                    <%= (String)GetGlobalResourceObject("BaseInfo", "Menu_PasswordInitialize")%>
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
                                        <table style="width: 100%">
                                        <tr>
                                        <td style="width: 15%; height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 15%; height: 22px" align="right">
                                            <asp:Label ID="lblUserCode" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,User_lblUserLoginCode %>" Width="84px"></asp:Label>
                                            </td>
                                        <td style="width: 5%; height: 22px" align="left">
                                            <asp:TextBox ID="txtCode" runat="server"></asp:TextBox>
                                            </td>
                                       <td style="width: 5%; height: 22px">
                                            &nbsp;</td>
                                        <td style="width: 20%; height: 22px" align="right">
                                            <asp:Label ID="lblUserName" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,User_lblUserName %>" Width="84px"></asp:Label>
                                            </td>
                                        <td style="width: 20%; height: 22px" align="left">
                                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
                                            </td>
                                        <td style="width: 25%; height: 22px">
                                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                Text="<%$ Resources:BaseInfo,User_lblQuery %>" TabIndex="1" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100%; height: 12px; text-align: center" class="tdBackColor" colspan="8">
                                            <table style="left: 0px; width: 98%; position: relative; text-align:center;" cellspacing="0" cellpadding="0"
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
                                                        <td style="left: 0px; vertical-align: top; width: 100%; position: relative; text-align: center">
                                                            <asp:GridView ID="GrdUser" runat="server" BorderWidth="1px" BorderStyle="Inset" 
                                                                CellPadding="3" BackColor="White" Width="98%" Height="258px"
                                                                AutoGenerateColumns="False" AllowPaging="True" PageSize="8" 
                                                                OnPageIndexChanging="GrdUser_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:BoundField DataField="UserID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="UserCode" HeaderText="<%$ Resources:BaseInfo,User_lblUserLoginCode %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="UserName" HeaderText="<%$ Resources:BaseInfo,User_lblUserName %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="DeptName" HeaderText="<%$ Resources:BaseInfo,Dept_lblDeptName %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="RoleName" HeaderText="<%$ Resources:BaseInfo,Role_lblRoleName %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Left" />
                                                                    </asp:BoundField>
                                                                   
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
                                       
                                        
                                        
                                        <td style="vertical-align: top; width: 98%; height: 44px; left: 0px; text-align: center;" class="tdBackColor"
                                            colspan="8">
                                            <table style="width: 98%" cellspacing="0" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495;  position: relative;">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff;  position: relative;">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        <table>
                                        <tr>
                                        <td style="left: 0px; position: relative; height: 37px; width: 6px;">
                                            &nbsp;<asp:Button ID="btnDefault" runat="server" Text="密码初始化" Enabled="False" 
                                                onclick="btnDefault_Click" />
&nbsp;</td>
                                        </tr>
                                        </table>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
            </table>
            
    </div>
        <asp:HiddenField ID="hidcon" runat="server" Value="<%$ Resources:BaseInfo,Menu_LeaseQuery %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
