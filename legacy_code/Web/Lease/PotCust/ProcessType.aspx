<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ProcessType.aspx.cs" Inherits="Lease_PotCust_ProcessType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_ProcessType")%></title>    
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
     function Load()
    {
        addTabTool("<%=strBaseInfo %>,Lease/PotCust/ProcessType.aspx");
        loadTitle();
    }
       //text控件文本验证
    function allTextBoxValidator()
    {
        if(isEmpty(document.all.txtProcessCode.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtProcessCode.focus();
            return false;					
        }
        if(isEmpty(document.all.txtProcessName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtProcessName.focus();
            return false;					
        }
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
                                                <asp:Label ID="labCustomer" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_ProcessType %>" Width="376px"></asp:Label></td>
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
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblCustTypeName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,CustType_lblCustTypeCode %>"
                                                                Width="70px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtProcessCode" runat="server" CssClass="ipt160px" 
                                                                MaxLength="8"></asp:TextBox>
                                                        </td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblCustTypeName0" runat="server" CssClass="labelStyle" 
                                                                Text="<%$ Resources:BaseInfo,CustType_lblCustTypeName %>" Width="70px"></asp:Label>
                                                        </td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtProcessName" runat="server" CssClass="ipt160px" 
                                                                MaxLength="8"></asp:TextBox>
                                                        </td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblBizGrpStatus" runat="server" CssClass="labelStyle" 
                                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>" Width="91px"></asp:Label>
                                                        </td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                            <asp:DropDownList ID="cmbTypeStatus" runat="server" BackColor="White" 
                                                                CssClass="cmb160px" Width="165px">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td class="tdBackColor" style="height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                            <asp:Label ID="lblBizGrpNote" runat="server" CssClass="labelStyle" 
                                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>"></asp:Label>
                                                            </td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox>
                                                            </td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="height: 12px; width:100%; text-align: center; padding-left:0px; padding-right:0px">
                                                            <table border="0" cellpadding="0" cellspacing="0" style=" width:98%;
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
                                                            &nbsp;<asp:GridView ID="GrdBrandOperateType" runat="server" AutoGenerateColumns="False"
                                                                BackColor="White" BorderColor="#E1E0B2" OnSelectedIndexChanged="GrdBrandOperateType_SelectedIndexChanged"
                                                                PageSize="15" Width="100%" OnRowDataBound="GrdBrandOperateType_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdBrandOperateType_PageIndexChanging">
                                                                <Columns>
                                                                    <asp:BoundField DataField="ProcessTypeId">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ProcessTypeCode" HeaderText="<%$ Resources:BaseInfo,CustType_lblCustTypeCode %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="ProcessTypeName" HeaderText="<%$ Resources:BaseInfo,CustType_lblCustTypeName %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Note" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Status" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField  ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" >
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:CommandField>
                                                                </Columns>
                                                                 <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                <RowStyle ForeColor="Black" Height="15px" Font-Overline="False" Font-Size="10pt" />
                                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
                                                                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
                                                                <PagerTemplate>                                                   
                                                                <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
                                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" 
                                                                        Font-Size="Small">首页</asp:LinkButton> 

                                                                <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
                                                                 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" 
                                                                        Font-Size="Small">上一页</asp:LinkButton> 

                                                                <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
                                                                 
                                                                        Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" 
                                                                        Font-Size="Small">下一页</asp:LinkButton> 

                                                                <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
                                                                 
                                                                        Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" 
                                                                        Font-Size="Small">尾页</asp:LinkButton> 
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
                                                        <td class="tdBackColor" colspan="8" style=" vertical-align: middle; width:100%;
                                                            height: 53px; text-align: left">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 98%">
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
                                                                    <td style="height: 30px; text-align: right">
                                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                                OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,DeptTree_labDeptAdd %>"
                                                                />
                                                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                                OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>"  />
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />&nbsp;
                                                                        &nbsp;
                                                                        
                                                                        <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                                                            Text="<%$ Resources:BaseInfo,Button_back %>" Visible="False" /><asp:Button ID="btnNext" runat="server"
                                                                                CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Visible="False" /></td>
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
