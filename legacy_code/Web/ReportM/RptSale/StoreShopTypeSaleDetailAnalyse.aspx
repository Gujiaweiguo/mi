<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreShopTypeSaleDetailAnalyse.aspx.cs" Inherits="ReportM_RptSale_StoreShopTypeSaleDetailAnalyse" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%=baseInfo%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
    </style>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript" src="../../JavaScript/setPeriod.js" charset="gb2312"></script>
        <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("刷新,ReportM/RptSale/StoreShopTypeSaleDetailAnalyse.aspx");
	        loadTitle();
	    }
	</script>
</head>
<body style="margin:0px"  onload ="Load();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1200">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_StoreShopTypeSaleDetailAnalyse %>"></asp:Label>
                                                    </td>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                        <img class="imageRightBack"/>
                                                    </td>
                                                </tr>
                                                <tr style="height:1px">
                                                    <td colspan="3" style="background-color:White; height:1px">
                                                    </td>
                                                </tr>
                                            </table>
                                            <table style="width:100%" class="tdBackColor">
                                                <tr style="height:10px">
                                                    <td style="width: 89px" align="right">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        
                                                        &nbsp;</td>
                                                    <td style="width: 14px">
                                                    </td>
                                                    <td style="width: 1055px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label15" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaTitle %>"> </asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlArea" runat="server" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 14px">
                                                        &nbsp;</td>
                                                    <td style="width: 1055px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label16" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_City %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlCity" runat="server" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 14px">
                                                        </td>
                                                    <td style="width: 1055px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="labShopType" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlShopType" runat="server" BackColor="White" 
                                                            Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 14px">
                                                        &nbsp;</td>
                                                    <td style="width: 1055px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="labShopType0" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_Year %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlYear" runat="server" BackColor="White" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 14px">
                                                        &nbsp;</td>
                                                    <td style="width: 1055px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="labShopType1" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_lblSalesMonth %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 258px">
                                                        <asp:DropDownList ID="ddlMonth" runat="server" BackColor="White" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 14px">
                                                        &nbsp;</td>
                                                    <td style="width: 1005px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label17" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,RPT_lblGroupBy %>"></asp:Label>
                                                        </td>
                                                    <td style="width: 218px">
                                                        <asp:CheckBox ID="ckbArea" runat="server" Font-Size="Small" 
                                                            Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaTitle %>" />
                                                        <asp:CheckBox ID="ckbCity" runat="server" Font-Size="Small" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_City %>" />
                                                        <asp:CheckBox ID="ckbShopType" runat="server" Font-Size="Small" 
                                                            Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>" />
                                                    </td>
                                                    <td style="width: 14px">
                                                    </td>
                                                    <td style="width: 1055px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 21px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 21px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" 
                                                            onclick="btnQuery_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                                        <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" 
                                                            OnClick="BtnCancel_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %> " />
                                                        </td>
                                                    <td style="width: 14px; height: 21px;">
                                                        </td>
                                                    <td style="height: 21px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 21px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 21px;">
                                                        </td>
                                                    <td style="width: 14px; height: 21px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 21px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 22px;">
                                                        </td>
                                                    <td style="width: 218px; height: 22px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 14px; height: 22px;">
                                                    </td>
                                                    <td style="height: 22px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 14px">
                                                        &nbsp;</td>
                                                    <td style="width: 1055px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 26px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 26px;">
                                                        </td>
                                                    <td style="width: 14px; height: 26px;">
                                                    </td>
                                                    <td style="height: 26px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 15px;">
                                                        </td>
                                                    <td style="width: 14px; height: 15px;">
                                                    </td>
                                                    <td style="height: 15px; width: 1055px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 28px;">
                                                        </td>
                                                    <td style="width: 218px; height: 28px;">
                                                        </td>
                                                    <td style="width: 14px; height: 28px;">
                                                    </td>
                                                    <td style="height: 28px; width: 1055px;">
                                                        </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;
    </form>
</body>
</html>

