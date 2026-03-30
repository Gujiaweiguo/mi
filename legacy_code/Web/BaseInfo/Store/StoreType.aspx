<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreType.aspx.cs" Inherits="BaseInfo_Store_StoreType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Store_StoreType")%></title>    
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
     function Load()
    {
        addTabTool("<%=baseInfo %>,BaseInfo/Store/StoreType.aspx");
        loadTitle();
    }
    function CheckData()
    {
        if(isEmpty(document.all.txtStoreTypeCode.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtStoreTypeCode.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtStoreTypeName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtStoreTypeName.focus();
            return false;					
        }
    }
     function BtnUp( p )
{
	var t = String(p)
	var l = t.substring(3,10); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
}
function BtnOver( p )
{
	var t = String(p)
	var l = t.substring(3,10); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
}
    </script>
</head>
<body topmargin=0 leftmargin=0 onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 370px">
                                        <tr>
                                            <td class="tdTopBackColor" style="vertical-align: middle; height: 25px;
                                                text-align: left" valign="top">
                                                <img alt="" class="imageLeftBack" />
                                                <asp:Label ID="labStoreType" runat="server" 
                                                    Text="<%$ Resources:BaseInfo,Store_StoreType %>" Width="376px"></asp:Label></td>
                                            <td class="tdTopRightBackColor" style=" height: 25px; text-align: right"
                                                valign="top">
                                                <img alt="" class="imageRightBack" /></td>
                                        </tr>
                                                                                            <tr>
                                                        <td colspan="2" style="height: 1px; background-color: white">
                                                        </td>
                                                    </tr>
                                        <tr>
                                            <td class="tdBackColor" colspan="2" style="width: 100%; height: 321px; text-align: center; vertical-align: top;"
                                                valign="top">
                                                
                                                <table style="width: 100%">

                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            &nbsp;<asp:Label ID="lblStoreTypeCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,CustType_lblCustTypeCode %>"
                                                                Width="70px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtStoreTypeCode" runat="server" CssClass="ipt160px" 
                                                                MaxLength="8"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblStoreTypeName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,CustType_lblCustTypeName %>"
                                                                Width="70px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtStoreTypeName" runat="server" CssClass="ipt160px" 
                                                                MaxLength="8"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            &nbsp;</td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            &nbsp;</td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            &nbsp;</td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            &nbsp;</td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr><td colspan="8" style="text-align: center;">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width:100%; position: relative; top:19px; left: 0px;">
                                                                  <tr>
                                                                        <td style="width:160px; height: 1px; background-color: #738495">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:160px; height: 1px; background-color: #ffffff">
                                                                        </td>
                                                                    </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8" style="height: 192px;" valign="top">
                                                            &nbsp;<asp:GridView ID="GrdVewStoreType" runat="server" AutoGenerateColumns="False"
                                                                BackColor="White" 
                                                                OnSelectedIndexChanged="GrdVewStoreType_SelectedIndexChanged" Width="96%" 
                                                                AllowPaging="True" BorderStyle="Inset" 
                                                                BorderWidth="1px" CellPadding="3" 
                                                                onpageindexchanging="GrdVewStoreType_PageIndexChanging" 
                                                                onrowdatabound="GrdVewStoreType_RowDataBound">
                                                                <RowStyle Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="StoreTypeID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="StoreTypeCode" HeaderText="<%$ Resources:BaseInfo,CustType_lblCustTypeCode %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="StoreTypeName" HeaderText="<%$ Resources:BaseInfo,CustType_lblCustTypeName %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField SelectText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
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
                                                        <td class="tdBackColor" colspan="8" style="height: 1px" align="right">
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>"
                                                                OnClick="btnSave_Click"  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" 
                                                                 />
                                                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Text="<%$ Resources:BaseInfo,User_btnChang %>"
                                                                OnClick="btnEdit_Click"  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                                            onmouseup="BtnUp(this.id);" Enabled="False"  />
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel"  onmouseover="BtnOver(this.id);" Text="<%$ Resources:BaseInfo,User_btnCancel %>"  onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="btnCel_Click" />
                                                            &nbsp; &nbsp; &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
            <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
                                </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
    </form>
</body>
</html>
