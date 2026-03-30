<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopType.aspx.cs" Inherits="Lease_Shop_ShopType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
    
        function showline()
        {
            parent.document.all.txtWroMessage.value = "";
             addTabTool("<%=baseInfo %>,Lease/Shop/ShopType.aspx");
             loadTitle();
        }
        function showlineIns()
        {
            parent.document.all.txtWroMessage.value = document.getElementById("hidAdd").value;
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        function showlineError()
        {
            parent.document.all.txtWroMessage.value = document.getElementById("hidInsert").value;
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        	//text控件文本验证
    function ShopTypeCode(sForm)
    {
        if(isEmpty(document.all.txtShopTypeCode.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopTypeCode")%>'+document.getElementById("hidMessage").value;
            document.all.txtShopTypeCode.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtShopTypeName.value))  
        {
            parent.document.all.txtWroMessage.value='<%=(String)GetGlobalResourceObject("BaseInfo", "PotShop_lblShopTypeName")%>'+document.getElementById("hidMessage").value;
            document.all.txtShopTypeName.focus();
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
</head>
<body onload='showline();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 400px">
                                        <tr>
                                            <td class="tdTopBackColor" style="vertical-align: middle; height: 25px;
                                                text-align: left" valign="top">
                                                <img alt="" class="imageLeftBack" />
                                                <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotShop_lblShopTypeTitle %>"></asp:Label></td>
                                            <td class="tdTopRightBackColor" colspan="2" style="height: 25px; text-align: right"
                                                valign="top">
                                                <img alt="" class="imageRightBack" /></td>
                                        </tr>
                                                                                            <tr>
                                                        <td colspan="8" style="width:100%; height: 1px; background-color: white">
                                                        </td>
                                                    </tr>
                                        <tr>
                                            <td class="tdBackColor" colspan="3" style="width: 100%; height: 330px; text-align: center; vertical-align: top;"
                                                valign="top">
                                                
                                                <table style="width: 100%">

                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="width: 100%; height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblShopTypeCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopTypeCode %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtShopTypeCode" runat="server" CssClass="textstyle" MaxLength="16"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblShopTypeName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopTypeName %>"
                                                                Width="87px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtShopTypeName" runat="server" CssClass="textstyle" MaxLength="32"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblBizGrpNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtNote" runat="server" CssClass="textstyle" MaxLength="128"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblBizGrpStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>"
                                                                Width="91px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:DropDownList ID="cmbShopTypeStatus" runat="server" BackColor="White" CssClass="cmb160px"
                                                                Width="124px">
                                                            </asp:DropDownList></td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="width: 100%; height: 12px; text-align: center">
                                                            <table border="0" cellpadding="0" cellspacing="0" style=" width: 96%; text-align:center;
                                                                ">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width: 160px; height: 1px;  position: relative; top: -5px;background-color: #738495">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 160px; height: 1px;  position: relative; top: -5px;background-color: #ffffff">
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8" style="width:100%;">
                                                            <asp:GridView ID="GrdShopType" runat="server" AutoGenerateColumns="False" BackColor="White" BorderStyle="Inset"  CellPadding="3" Height="202px"
                                                                OnRowDataBound="GrdWrkGrp_RowDataBound" OnSelectedIndexChanged="GrdWrkGrp_SelectedIndexChanged"
                                                                Width="96%" BorderWidth="1px" AllowPaging="True" OnPageIndexChanging="GrdShopType_OnPageIndexChanging" PageSize="8">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ShopTypeID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ShopTypeCode" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopTypeCode %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ShopTypeName" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopTypeName %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ShopTypeStatusName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Note" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
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
                                                <asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO"  Font-Size="Small"/> 
                                                  </PagerTemplate>         
                                                <PagerSettings Mode="NextPreviousFirstLast"  />

                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr style="width:100%;">  
                                                        <td class="tdBackColor" colspan="20" style="left: 0px; vertical-align:bottom; width: 100%;
                                                            height: 10px; text-align:center">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 96%;text-align:center;">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="left: 0px; width: 160px; height: 1px; background-color: #738495; position: relative; top: -5px;">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="left: 0px; width: 160px; height: 1px; background-color: #ffffff; position: relative; top: -5px;">
                                                                        </td>
                                                                    </tr>
                                                                    
                                                                </tbody>
                                                            </table>
                                                           
                                                         </td>
                                                    </tr>
                                                    <tr style="width:20px; vertical-align:bottom;">
                                                        <td colspan="8" style="text-align: right" valign="bottom">
                                                                <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd"
                                                                    OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butAdd %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                                &nbsp;<asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Enabled="False"
                                                                    OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                            &nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCancel_Click"
                                                                onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                                                Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                                                            &nbsp;&nbsp;
                                                        </td>
                                                        </tr>
                                                    
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
    
    </div>
        <asp:HiddenField ID="hidBizCode" runat="server"  Value="<%$ Resources:BaseInfo,WrkFlw_lblBizCode %>"/>
        <asp:HiddenField ID="hidBizGrpName" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpName %>" />
        <asp:HiddenField ID="hidBizNote" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>" />
        <asp:HiddenField ID="hidBizGrpStatus" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>" />
        <asp:HiddenField ID="hidChang" runat="server" Value="<%$ Resources:BaseInfo,User_btnChang %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidMessageError" runat="server" Value="<%$ Resources:BaseInfo,BizGrp_MessageError %>" />
       <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
    </form>
</body>
</html>
