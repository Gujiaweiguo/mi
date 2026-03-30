<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InvoiceAdPara.aspx.cs" Inherits="ReportM_InvoicePara_InvoiceAdPara" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "InvoiceAdPara_Title")%></title>
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
            addTabTool("<%=baseInfo %>,ReportM/InvoicePara/InvoiceAdPara.aspx");
            loadTitle();
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
	    -->
      </script>
</head>
<body topmargin=0 leftmargin=0 onload='Load()'>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
           <table border="0" cellpadding="0" cellspacing="0" style="height: 301px; width: 100%;" width="500">
            <tr>
                <td class="tdTopBackColor" style="width: 478px; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" />
                    <asp:Label ID="labBuildingTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,InvoiceAdPara_Title %>"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 493; height: 25px" valign="top">
                    <img class="imageRightBack" /></td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="3" style="width: 100%; height: 230px; text-align:center;"
                    valign="top">
                 
                 <table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td valign="top"><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
      
      <tr>
        <td height="30" colspan="3" align="right"><hr /></td>
        </tr>
      <tr>
        <td rowspan="10" align="right" style="width: 22%; vertical-align: top;">
            <table>
            <tr>
            <td style="width: 10px">
            </td>
            <td>
            <asp:GridView ID="gvInvoicePara" runat="server" AutoGenerateColumns="False"
                BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" CssClass="labelStyle"
                Height="444px" OnSelectedIndexChanged="gvInvoicePara_SelectedIndexChanged" PageSize="15"
                Width="170px" OnRowDataBound="gvInvoicePara_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="InvoiceAdParaID">
                        <FooterStyle CssClass="hidden" />
                        <HeaderStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
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
            </asp:GridView>
            </td>
            <td style="width: 10px">
            </td>
            </tr>
            <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" Height="31px"
                    OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>" Width="71px" /><asp:Button
                        ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" Height="30px"
                        OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Width="73px" /></td>
            <td>
            </td>
            </tr>
            </table>
            <asp:Label ID="lblTotalNum" runat="server" Height="1px" Width="32px"></asp:Label><asp:Label
                ID="lblCurrent" runat="server" ForeColor="Red" Height="1px" Width="1px">1</asp:Label></td>
        <td align="right"><span class="STYLE1">
            <asp:Label ID="lblInvParaTitle" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportName %>"
                Width="75px"></asp:Label></span></td>
        <td height="30" align="center" style="width: 470px; text-align: left;">
            <asp:TextBox ID="txtInvHeader" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right"><span class="STYLE1">
            <asp:Label ID="lblInvSubhead" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportSubhead %>"></asp:Label>&nbsp;</span></td>
        <td height="30" align="center" style="width: 470px; text-align: left;">
            <asp:TextBox ID="txtInvSubhead" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right"><span class="STYLE1"></span></td>
        <td align="center" style="width: 470px; vertical-align: top; text-align: left;" rowspan="5"><table width="460" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td height="30" align="center" style="width: 460px; text-align: left">
                    <asp:TextBox ID="txtInvH1" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="center" style="width: 460px; text-align: left">
                    <asp:TextBox ID="txtInvH2" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="center" style="width: 460px; text-align: left">
                    <asp:TextBox ID="txtInvH3" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="center" style="width: 460px; text-align: left">
                    <asp:TextBox ID="txtInvH4" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
                <td align="left" style="width: 460px; height: 30px">
                    <asp:TextBox ID="txtInvH5" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
        </table>
        </td>
      </tr>
      <tr>
        <td align="right" style="height: 113px" rowspan="4"><span class="STYLE1"></span><span class="STYLE1"></span><span class="STYLE1"></span><span class="STYLE1">
            <asp:Label ID="lblinvH" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportTitle %>"></asp:Label>&nbsp;
        </span></td>
      </tr>
      <tr>
      </tr>
      <tr>
      </tr>
      <tr>
      </tr>
      <tr>
        <td align="right" valign="top" class="STYLE1" style="vertical-align: middle">
            <asp:Label ID="lblInvF" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportRemark %>"></asp:Label>&nbsp;</td>
        <td height="30" style="width: 470px; text-align: left;"><table border="0" align="center" cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
              <td height="30" align="left">
                  <asp:TextBox ID="txtInvF1" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left">
                    <asp:TextBox ID="txtInvF2" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left">
                    <asp:TextBox ID="txtInvF3" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left">
                    <asp:TextBox ID="txtInvF4" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
                <td height="30" align="left">
                    <asp:TextBox ID="txtInvF5" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
              <td height="30" align="left">
                  <asp:TextBox ID="txtInvF6" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
            <tr>
              <td align="left" style="height: 30px">
                  <asp:TextBox ID="txtInvF7" runat="server" CssClass="ipt160px" Width="440px" MaxLength="128"></asp:TextBox></td>
            </tr>
        </table></td>
      </tr>
      <tr>
        <td align="right" valign="middle"><span class="STYLE1">
            <asp:Label ID="lblInvoiceParaDesc" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,InvoicePara_ReportBewrite %>"></asp:Label></span></td>
        <td height="30" align="left" style="width: 470px">
            <asp:TextBox ID="txtInvoiceParaDesc" runat="server" CssClass="ipt160px" Width="440px" MaxLength="64"></asp:TextBox></td>
      </tr>
      <tr>
        <td align="right" valign="middle" class="STYLE1">
            <asp:Label ID="lblStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationStatus %>"
                Width="50px"></asp:Label></td>
        <td height="30" align="left" style="width: 470px; text-align: left;">
            <asp:DropDownList ID="cmbStatus" runat="server" Width="165px">
            </asp:DropDownList>&nbsp;
            <asp:CheckBox ID="chkDefault" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Invoice_chkDefault %>" /></td>
      </tr>
    </table></td>
  </tr>
  <tr>
    <td style="height: 23px"><hr />
        &nbsp;</td>
  </tr>
</table>
                 </td>
                
            </tr>
            <tr>
                <td style="width:478px; height:36px; text-align:right;"  class="tdBackColor" valign="top">
                </td>
                <td style="width:10px; height:36px; text-align:right;"  class="tdBackColor" valign="top">
                    <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                        Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="70px" /></td>
                <td style="width:190px; height:36px; text-align:center;"  class="tdBackColor" valign="top">
                    <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Enabled="False" Height="30px"
                        OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" Width="70px" /><asp:Button
                            ID="btnCel" runat="server" CssClass="buttonClear" OnClick="btnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
            </tr>
        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </form>
</body>
</html>
