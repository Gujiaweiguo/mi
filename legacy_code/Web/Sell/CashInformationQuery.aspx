<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CashInformationQuery.aspx.cs" Inherits="Sell_CashInformationQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>    
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../JavaScript/Common.js"></script>
    <link href="../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
    <script type="text/javascript">
     function Load()
    {
        addTabTool("<%=baseInfo %>,sell/CashInformationQuery.aspx");
        loadTitle();
//        document.getElementById("lblTotalNum").style.display="none";
//        document.getElementById("lblCurrent").style.display="none";
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
                                                <asp:Label ID="labCustomer" runat="server" Text="<%$ Resources:BaseInfo,CashInformation_Query %>" Width="134px" Height="22px"></asp:Label></td>
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
                                                        <td class="tdBackColor" colspan="8" style="height: 8px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 173px; height: 30px; text-align: right">
                                                            &nbsp;<asp:Label ID="lblCustTypeCode" runat="server" CssClass="labelStyle" 
                                                                Width="70px" Text="<%$ Resources:BaseInfo,Lease_lblPayInDate %>"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 8px; height: 30px">
                                                            <asp:TextBox ID="txtBizDate" runat="server" CssClass="ipt160px" MaxLength="32"  onclick="calendar()"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="width: 98px; height: 30px; text-align: left">
                                                            &nbsp;</td>
                                                        <td class="tdBackColor" style="height: 30px; width: 105px;">
                                                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" 
                                                                Text="<%$ Resources:BaseInfo,User_lblQuery %>" /></td>
                                                        <td class="tdBackColor" style="width: 94px; height: 30px; text-align: right">
                                                           <asp:Button ID="btnAdd" runat="server" CssClass="buttonAdd" Height="31px"
                                                                 Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" OnClick="btnAdd_Click"  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />
                                                            </td>
                                                        <td class="tdBackColor" style="width: 7px; height: 30px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 100px; height: 30px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 173px; height: 10px; text-align: right">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 8px; height: 10px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 98px; height: 10px; text-align: left">
                                                            </td>
                                                        <td class="tdBackColor" style="height: 10px; width: 105px;">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 94px; height: 10px; text-align: right">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 7px; height: 10px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 127px; height: 10px; text-align: left">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 100px; height: 10px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="height: 12px; text-align: center; padding-left:10px; padding-right:10px">
                                                            <table border="0" cellpadding="0" cellspacing="0" style="left: 12px; width:100%;
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
                                                            &nbsp;<asp:GridView ID="GrdCashInfo" runat="server" AutoGenerateColumns="False"
                                                                BackColor="White" BorderColor="#E1E0B2"  PageSize="10" Width="98%" OnSelectedIndexChanged="GrdCashInfo_SelectedIndexChanged" >
                                                                <Columns>
                                                                     <asp:BoundField DataField="UserID" HeaderText="<%$ Resources:BaseInfo,Rpt_CasherCode  %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="TPUsrNm" HeaderText="<%$ Resources:BaseInfo,Rpt_CasherName %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="BizDate" HeaderText="<%$ Resources:BaseInfo,Rpt_GetTotalDate %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Amountt" HeaderText="<%$ Resources:ReportInfo,RptSale_LocalAmt %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PayAmt" HeaderText="<%$ Resources:BaseInfo,CashInformation_PayAmt %>" >
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField SelectText="<%$ Resources:BaseInfo,User_btnChang %>" ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" >
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:CommandField>
                                                                </Columns>
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
                                                        <td class="tdBackColor" colspan="3" style="left: 30px; vertical-align: middle; height: 5px;
                                                            text-align: left">
                                                        </td>
                                                        <td class="tdBackColor" style="left: 30px; vertical-align: middle; height: 5px; text-align: left; width: 105px;">
                                                        </td>
                                                        <td class="tdBackColor" colspan="4" style="left: 30px; vertical-align: middle; height: 5px;
                                                            text-align: left">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="3" style="height: 53px">
                                                            &nbsp; &nbsp; &nbsp;
                                                        </td>
                                                        <td class="tdBackColor" style="height: 53px; width: 105px;">
                                                        </td>
                                                        <td class="tdBackColor" colspan="4" style="left: 30px; vertical-align: middle; width: 270px;
                                                            height: 53px; text-align: left">
                                                            &nbsp;<asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %>" OnClick="btnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
        &nbsp;
        &nbsp;&nbsp;
    </form>
</body>
</html>