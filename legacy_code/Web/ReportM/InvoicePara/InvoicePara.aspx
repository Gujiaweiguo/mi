<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoicePara.aspx.cs" Inherits="ReportM_InvoicePara_InvoicePara" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "InvoicePara_Title")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
 
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script type="text/javascript" src="../../JavaScript/TreeShow.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
      <!--	    
	     function Load()
        {
            addTabTool("<%=baseInfo %>,ReportM/InvoicePara/InvoicePara.aspx");
            loadTitle();
//            document.getElementById("lblTotalNum").style.display="none";
//            document.getElementById("lblCurrent").style.display="none";
        }
	    -->
      </script>
</head>
<body topmargin=0 leftmargin=0 onload='Load()' style="width:97%">
    <form id="form1" runat="server" style="width:100%">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
            <ContentTemplate>
           <table border="0" cellpadding="0" cellspacing="0" style="height: 301px; width: 100%;">
            <tr>
                <td class="tdTopBackColor" style="width: 470px; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" />
                    <asp:Label ID="labBuildingTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,InvoicePara_Title %>"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 493; height: 25px" valign="top">
                    <img class="imageRightBack" /></td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="3" style="width: 100%; height: 228px; text-align:center;"
                    valign="top">
                 
                 <table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td valign="top" style="height: 485px"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
      
      <tr>
        <td colspan="3" align="right" style="height: 15px"><hr /></td>
        </tr>
        <tr>
            <td rowspan="11" style="left: 5px; width: 35%; position: relative">
            <asp:GridView ID="gvInvoicePara" runat="server" AutoGenerateColumns="False"
                BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" CssClass="labelStyle"
                Height="470px" OnSelectedIndexChanged="gvInvoicePara_SelectedIndexChanged" PageSize="15"
                Width="100%" OnRowDataBound="gvInvoicePara_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvInvoicePara_OnPageIndexChanging">
                <Columns>                    
                    <asp:BoundField DataField="InvoiceParaID">
                        <FooterStyle CssClass="hidden" />
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SubsName" HeaderText="<%$ Resources:BaseInfo,MakePoolVoucher_lblSubs %>">
                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                        <ItemStyle BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="InvHeader" HeaderText="<%$ Resources:BaseInfo,InvoicePara_ReportTitle %>">
                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                        <ItemStyle BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ParaStatusName" HeaderText="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>">
                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                        <ItemStyle BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                        <ItemStyle BorderColor="#E1E0B2" />
                    </asp:CommandField>
                </Columns>
                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Right" />
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

            </asp:GridView>             &nbsp;</td>
            <td align="right" style="width: 76px">
                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MakePoolVoucher_lblSubs %>"
                    Width="75px"></asp:Label></td>
            <td align="center" height="30" style="width: 301px; text-align: left">
                <asp:DropDownList ID="ddlSubs" runat="server" CssClass="ipt160px" Width="300px">
                </asp:DropDownList></td>
        </tr>
      <tr>
        <td align="right" style="width: 76px"><span class="STYLE1">
            <asp:Label ID="lblInvParaTitle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportName %>"
                Width="70px"></asp:Label></span></td>
        <td height="30" align="center" style="width: 301px; text-align: left;">
            <asp:TextBox ID="txtInvHeader" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right" style="width: 76px"><span class="STYLE1">
            <asp:Label ID="lblInvSubhead" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportSubhead %>"></asp:Label></span></td>
        <td height="30" align="center" style="width: 301px; text-align: left;">
            <asp:TextBox ID="txtInvSubhead" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right" style="width: 76px; height:5px"></td>
        <td align="left" style="width: 301px; vertical-align: top; text-align: left;" rowspan="5"><table width="310" border="0" align="center" cellpadding="0" cellspacing="0" style="width: 301px">
            <tr>
                <td height="30" align="left" style="width: 301px; text-align: left">
                    <asp:TextBox ID="txtInvH1" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left" style="width: 301px; text-align: left">
                    <asp:TextBox ID="txtInvH2" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" style="width: 301px; text-align: left">
                    <asp:TextBox ID="txtInvH3" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left" style="width: 301px; text-align: left">
                    <asp:TextBox ID="txtInvH4" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" style="width: 301px; height: 30px">
                    <asp:TextBox ID="txtInvH5" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
        </table>
        </td>
      </tr>
      <tr>
        <td align="right" style="height: 113px; width: 76px;" valign="top" rowspan="4"><span class="STYLE1"></span><span class="STYLE1"></span><span class="STYLE1"></span><span class="STYLE1">
            <asp:Label ID="lblinvH" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportTitle %>"></asp:Label>
            </span></td>
      </tr>
      <tr>
      </tr>
      <tr>
      </tr>
      <tr>
      </tr>
      <tr>
        <td align="right" valign="top" class="STYLE1" style="vertical-align:top; width: 76px;">
            <asp:Label ID="lblInvF" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportRemark %>"></asp:Label></td>
        <td height="30" style="width: 301px; text-align: left;" valign="top"><table border="0" align="center" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
              <td height="30" align="left">
                  <asp:TextBox ID="txtInvF1" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left">
                    <asp:TextBox ID="txtInvF2" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left">
                    <asp:TextBox ID="txtInvF3" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left">
                    <asp:TextBox ID="txtInvF4" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left">
                    <asp:TextBox ID="txtInvF5" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
              <td height="30" align="left">
                  <asp:TextBox ID="txtInvF6" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
            <tr>
              <td align="left" style="height: 30px">
                  <asp:TextBox ID="txtInvF7" runat="server" CssClass="ipt160px" Width="300px" MaxLength="64"></asp:TextBox></td>
            </tr>
        </table></td>
      </tr>
      <tr>
        <td align="right" valign="middle" style="width: 76px"><span class="STYLE1">
            <asp:Label ID="lblInvoiceParaDesc" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportBewrite %>"></asp:Label></span></td>
        <td height="30" align="left" style="width: 301px">
            <asp:TextBox ID="txtInvoiceParaDesc" runat="server" CssClass="ipt160px" Width="300px" MaxLength="32"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right" valign="middle" class="STYLE1" style="width: 76px">
            <asp:Label ID="lblStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                Width="50px"></asp:Label></td>
        <td height="30" align="left" style="width: 301px; text-align: left;">
            <asp:DropDownList ID="cmbStatus" runat="server" Width="165px">
            </asp:DropDownList>&nbsp;
            <asp:CheckBox ID="chkDefault" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Invoice_chkDefault %>" /></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td style="height: 5px"><hr />
        &nbsp;</td>
  </tr>
</table>
                 </td>
                
            </tr>
            <tr>
                <td style="width:478px; height:40px; text-align:right;"  class="tdBackColor" valign="top">
                </td>
                <td style="width:10px; height:40px; text-align:right;"  class="tdBackColor" valign="top">
                    </td>
                <td style="width:300px; height:40px; text-align:center;"  class="tdBackColor" valign="top">
                    <asp:Button ID="btnSave" runat="server" CssClass="buttonSave"  OnClick="btnSave_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                        Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>"  />&nbsp;
                    <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Enabled="False" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                        OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>"  />&nbsp;
                    <asp:Button
                            ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="btnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" /></td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </form>
</body>
</html>