<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceDetailCancel.aspx.cs" Inherits="Invoice_InvoiceDetailCancel_InvoiceDetailCancel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "InvCancel_lblInvCancel")%></title>
        <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	function Load()
	{
        var str= document.getElementById("InvCancel_lblInvCancel").value + ",Invoice/InvoiceDetailCancel/InvoiceDetailCancel.aspx";
        addTabTool(str);
        loadTitle();
    }
    
    function BillOfDocumentDelete()
    {
        return window.confirm('<%= billOfDocumentDelete %>');
    }
	</script>
</head>
<body topmargin=0 leftmargin=0 onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div style="width:100%">
                                <table border="0" cellpadding="0" cellspacing="0" style="height: 420px; width: 100%; text-align:right">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 536px; height: 22px; text-align: left">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,InvCancel_lblInvCancel %>" Width="329px"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 655px; height: 339px; text-align: center;" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="height: 45px; width: 100%;">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="3" style="height: 15px; width:100%; text-align:left">
                                                        <table >
                                                            <tr>
                                                                <td style="width: 99px; height: 17px; text-align: right"  >&nbsp;
                                                                    <asp:Label ID="Label3" Width="53px" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>"></asp:Label></td>

                                                                <td style="height: 17px; width: 168px; ">
                                                                    <asp:TextBox ID="txtInvCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                                 <td colspan="3" align="left">
                                                                    &nbsp;<asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                                                    Text="<%$ Resources:BaseInfo,User_lblQuery %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                             </tr>
                                                        </table>                                                    
                                                    </td>
                                                </tr>
                                                <tr style="width:100%">
                                                <td style="height: 5px; text-align: center; width:100%" colspan="4">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 99%">
                                                        <tr class="bodyLine">
                                                            <td style="height: 1px; background-color: #738495">
                                                            </td>
                                                        </tr>
                                                        <tr class="bodyLine">
                                                            <td style="height: 1px; background-color: #ffffff">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" style="width: 43%; text-align:center">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 100%; height: 275px">
                                                            <tr style="height: 5px">
                                                                <td style="width: 99px; height: 5px;">
                                                                </td>
                                                                <td style="width: 168px; height: 5px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align:right; width: 99px;">
                                                                    &nbsp;<asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>" Width="53px"></asp:Label>
                                                                </td>
                                                                <td style="height: 30px; width: 168px;">
                                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px; text-align: right; width: 99px;">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label>
                                                                </td>
                                                                <td style="height: 30px; width: 168px;">
                                                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td style="width: 99px; height: 30px; text-align: right;">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>" Width="58px"></asp:Label></td>
                                                                <td style="width: 168px; vertical-align: middle; height: 30px;">
                                                                    <asp:TextBox ID="txtContractID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 28px; text-align: right; width: 99px;">
                                                                    <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvAdj_KeepAccountsMth %>"
                                                                        Width="62px"></asp:Label></td>
                                                                <td style="height: 28px; width: 168px;">
                                                                    <asp:TextBox ID="txtKeepAccountsMth" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px; text-align: right; width: 99px;">
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                                <td style="height: 30px; width: 168px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #738495">
                                                                            </td>
                                                                        </tr>
                                                                        <tr class="bodyLine">
                                                                            <td style="height: 1px; background-color: #ffffff">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="width: 99px; height: 31px; text-align: right">
                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,AreaSize_lblNote %>"></asp:Label></td>
                                                                <td rowspan="1" style="width: 168px">
                                                                    <asp:TextBox ID="txtNote" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right; width: 99px; height: 31px;">
                                                                    <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvCancel_InvCancel %>"></asp:Label></td>
                                                                <td style="width: 168px;" rowspan="3">
                                                                    <asp:TextBox ID="txtAdjReason" runat="server" CssClass="ipt160px" Height="85px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right; width: 99px; vertical-align: middle;">
                                                                    &nbsp; &nbsp; &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="text-align: right; width: 99px; height: 22px;">
                                                                    &nbsp; &nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <br />
                                                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"
                                                            OnClick="btnTempSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                        <asp:Button ID="btnPutIn" runat="server" CssClass="buttonPutIn" OnClick="butAuditing_Click"
                                                            Text="<%$ Resources:BaseInfo,ConLease_butAddAll %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                        <asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear" Enabled="False" OnClick="btnBalnkOut_Click" Text="<%$ Resources:BaseInfo,ConLease_butDel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; text-align:right" width="40%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 99%; text-align:center; height: 31px" id="">
                                                            <tr style="height: 5px">
                                                                <td style="width: 154px; height: 5px;">
                                                                </td>
                                                                <td class="baseInput" style="height: 5px; width: 200px;">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseInput" colspan="2" rowspan="3" style="vertical-align: top; text-align: center; height: 110px; width: 100%;">
                                                                    <asp:GridView ID="GrdVewInvoiceDetail" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                                        BorderStyle="Inset" BorderWidth="1px" Height="274px" Width="380px" PageSize="9" AllowPaging="True" OnPageIndexChanging="gvShopBrand_OnPageIndexChanging">
                                                                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                                                                    <Columns>
                                                                        <asp:BoundField DataField="InvDetailID">
                                                                            <ItemStyle CssClass="hidden" />
                                                                            <HeaderStyle CssClass="hidden" />
                                                                            <FooterStyle CssClass="hidden" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvID" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvID %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="ChargeTypeName" HeaderText="<%$ Resources:BaseInfo,ChargeType_lblChargeTypeName %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="InvActPayAmt" HeaderText="<%$ Resources:BaseInfo,InvoiceHeader_lblInvPayAmt %>">
                                                                            <ItemStyle BorderColor="#E1E0B2" />
                                                                            <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        </asp:BoundField>
                                                                    </Columns>
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
                                                            <tr>

                                                            </tr>
                                                            <tr>
                                                            <td style="width: 1px; height: 139px;">
                                                            </td>
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 28px; width: 154px;">
                                                                </td>
                                                                <td rowspan="1" style="height: 28px; width: 200px;" valign="top">
                                                                    <br />
                                                                    &nbsp;
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
        <asp:HiddenField ID="InvCancel_lblInvCancel" runat="server" Value="<%$ Resources:BaseInfo,InvCancel_lblInvCancel %>" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>



