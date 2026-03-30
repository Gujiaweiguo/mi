<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Palaver.aspx.cs" Inherits="Lease_PotCustomer_Palaver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotShop_lblPalaverNode")%></title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
  <%--  <script type="text/javascript"  src="../../JavaScript/setday.js"></script>--%>
    <script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
     <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
		function Load()
		{
		    loadTitle();
//		    document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
		}
		function InputValidator()
        {
             if(isEmpty(document.all.txtPalaverAim.value))
            {
                parent.document.all.txtWroMessage.value =('<%= strError %>');
                document.all.txtPalaverAim.focus();
                return false;
            }
        }
		</script>
</head>
<body style="margin-top:0; margin-left:0" onload="Load();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div style=" text-align:right">
        <table id="TABLE0" border="0" cellpadding="0" cellspacing="0" style="height: 24px;
            width: 100%; text-align: center;">
            <tr>
                <td class="tdTopBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopBackColor">
                   <%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustPalaverNote")%>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 410px; text-align:right;">
            <tr>
                <td class="tdBackColor" colspan="3" style="width: 40%; height: 320px; text-align:right" valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 311px; text-align:right" width="255">
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td style="width: 191px; height: 10px" align="left">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 163px; text-align: center">
                                    <tr>
                                        <td style="width: 324px; position: relative; top: 6px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 324px; position: relative; top: 6px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdBackColor" style="height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px; text-align: center"
                                valign="bottom">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 1px">
                            </td>
                            <td class="tdBackColor" style="width: 5px; height: 1px">
                            </td>
                            <td style="width: 191px; height: 10px">
                            </td>
                            <td class="tdBackColor" style="height: 1px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                <asp:Label ID="lblPalaverTime" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustAttractProcessType %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px" align="left">
                                <asp:DropDownList ID="ddlProcessTypeId" runat="server" CssClass="ipt160px">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                <asp:Label ID="lblPalaverUser" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustPalaverRound %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px" align="left">
                                <asp:TextBox ID="txtPalaverRound" runat="server" CssClass="ipt160px" MaxLength="16" ReadOnly="True"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                <asp:Label ID="lblPalaverAim" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PalaverType %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px" align="left">
                                <asp:TextBox ID="txtPalaverPlace" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustContactorName %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px" align="left">
                                <asp:TextBox ID="txtContactorName" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustMostlyTitle %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px">
                                <asp:TextBox ID="txtPalaverAim" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox>&nbsp;<img id="ImgPalaverAim" src="../../App_Themes/Main/Images/must.gif" style="width: 16px; height: 16px" /></td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 280px; height: 22px; text-align: right">
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_PalaverAndOther %>" Width="89px"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px" align="left">
                                <asp:TextBox ID="txtPalaverContent" runat="server" CssClass="ipt160px" MaxLength="512" Height="47px" TextMode="MultiLine"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustPalaverResult %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px" align="left">
                                <asp:TextBox ID="txtPalaverResult" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustUnSolved %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px" align="left">
                                <asp:TextBox ID="txtUnSolved" runat="server" CssClass="ipt160px" MaxLength="256"></asp:TextBox>&nbsp;</td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>"></asp:Label>&nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px" align="left">
                                <asp:TextBox ID="txtNode" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 255px; height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="width: 260px; height: 22px; text-align: right">
                                &nbsp;</td>
                            <td class="tdBackColor" style="width: 5px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="width: 191px; height: 22px">
                            </td>
                            <td class="tdBackColor" style="height: 22px">
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="tdBackColor" colspan="4" style="width: 100%; height: 22px; text-align: right">
                                <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>"
                                     />
                                <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butUpdate %>"
                                     />
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel"  OnClick="btnCancel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    Text="<%$ Resources:BaseInfo,User_btnCancel %>"  />
                                &nbsp; &nbsp; &nbsp;</td>
                        </tr>
                    </table>
                </td>
                
                <td class="tdBackColor" style="height: 330px; width: 50%;" valign="top" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" style="height: 311px" width="280">
                        <tr>
                            <td class="tdBackColor" style="width: 280px; height: 22px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 271px; text-align: center">
                                    <tr>
                                        <td style="width: 324px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 324px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="vertical-align: top; width: 280px; height: 347px;
                                text-align: center" valign="top" rowspan="3">
                                <asp:GridView ID="GrdCustPalaverInfo" runat="server" AutoGenerateColumns="False"
                                    BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Font-Size="12pt"
                                    Font-Strikeout="False" Height="90%" OnRowDataBound="GrdCustPalaverInfo_RowDataBound"
                                    OnSelectedIndexChanged="GrdCustPalaverInfo_SelectedIndexChanged"
                                    Width="277px" AllowPaging="True" OnPageIndexChanging="gvShopBrand_OnPageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="PalaverID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PalaverTime" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverTime %>">
                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="PalaverAim" HeaderText="<%$ Resources:BaseInfo,PotCustomer_CustMostlyTitle %>">
                                            <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                        </asp:BoundField>
                                        <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_lblUserQuery %>" SelectText="<%$ Resources:BaseInfo,User_lblUserQuery %>"
                                            ShowSelectButton="True">
                                            <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" />
                                            <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                        </asp:CommandField>
                                    </Columns>
                                    <FooterStyle BackColor="Red" ForeColor="#000066" />
                                    <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
                                    <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
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
                                &nbsp;&nbsp;</td>
                        </tr>
                        <tr>
                        </tr>
                        <tr>
                        </tr>
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
