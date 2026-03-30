<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ContractType.aspx.cs" Inherits="Lease_AdContract_ContractType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Contract_ContractTypeVindicate")%></title>    
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
     function Load()
    {
        addTabTool("<%=strBaseInfo %>,Lease/AdContract/ContractType.aspx");
        loadTitle();
    }
    //text控件文本验证
    function allTextBoxValidator()
    {
        if(isEmpty(document.all.txtCode.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtCode.focus();
            return false;					
        }
        if(isEmpty(document.all.txtName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
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
                                                <asp:Label ID="labCustomer" runat="server" Text="<%$ Resources:BaseInfo,Contract_ContractTypeVindicate %>" Width="376px"></asp:Label></td>
                                            <td class="tdTopRightBackColor" colspan="2" style=" height: 25px; text-align: right"
                                                valign="top">
                                                <img alt="" class="imageRightBack" /></td>
                                        </tr>
                                                                                            <tr>
                                                        <td colspan="8" style="height: 1px; background-color: white">
                                                        </td>
                                                    </tr>
                                        <tr>
                                            <td class="tdBackColor" colspan="3" style="width: 100%; height: 330px; text-align: center; vertical-align: top;"
                                                valign="top">
                                                
                                                <table style="width: 100%">

                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style=" height: 30px; text-align: right">
                                                            <asp:Label ID="lblCustTypeCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,CustType_lblCustTypeCode %>"
                                                                Width="70px"></asp:Label></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtCode" runat="server" CssClass="ipt160px" MaxLength="8"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px; width: 3px;">
                                                        </td>
                                                        <td class="tdBackColor" style=" height: 30px; text-align: right">
                                                            <asp:Label ID="lblCustTypeName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,CustType_lblCustTypeName %>"
                                                                Width="70px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtName" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblBizGrpNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="height: 30px; width: 3px;">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblBizGrpStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>"
                                                                Width="91px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" BackColor="White" CssClass="cmb160px" Width="165px">
                                                            </asp:DropDownList></td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="height: 12px; width:100%; text-align: center; padding-left:0px; padding-right:0px">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width:98%;
                                                                position: relative">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="width:160px; height: 1px; background-color: #738495">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width:160px; height: 1px; background-color: #ffffff">
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8" style="height: 192px;padding-left:10px; padding-right:10px" valign="top">
                                                            &nbsp;<asp:GridView ID="GrdVewType" runat="server" AutoGenerateColumns="False"
                                                                BackColor="White" BorderColor="#E1E0B2" OnSelectedIndexChanged="GrdVewType_SelectedIndexChanged"
                                                                PageSize="9" Width="100%" OnRowDataBound="GrdVewType_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdVewType_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ContractTypeID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ContractTypeCode" HeaderText="<%$ Resources:BaseInfo,CustType_lblCustTypeCode %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ContractTypeName" HeaderText="<%$ Resources:BaseInfo,CustType_lblCustTypeName %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Note" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ContractTypeStatus" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField  ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
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
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="left: 30px; vertical-align: middle; width: 100%;
                                                            height: 53px; text-align: left">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width:98%">
                                                                <tbody>
                                                                    <tr>
                                                                        <td style="left: 3px; width: 160px; height: 1px; background-color: #738495; position: relative; top: -5px;">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="left: 3px; width: 160px; height: 1px; background-color: #ffffff; position: relative; top: -5px;">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                    <td style="height: 30px;" align="right" >
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"
                                                                OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit"
                                                                OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                                        &nbsp;&nbsp;
                                                                    </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                           </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
                    <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
                    <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
                    <asp:HiddenField ID="hidUpdateLost" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdateLost %>" />
                    <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
                    <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
                    <asp:HiddenField ID="hidRoleCodeBeing" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidRoleCodeBeing %>" />
    </form>
</body>
</html>

