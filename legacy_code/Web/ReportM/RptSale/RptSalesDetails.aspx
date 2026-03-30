<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptSalesDetails.aspx.cs" Inherits="RptBaseMenu_RptSalesDetails" ResponseEncoding="utf-16" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
        <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=Fresh %>,ReportM/RptSale/RptSalesDetails.aspx");
	        loadTitle();
	    }
	</script>

</head>
<body style="margin:0px" onload ="Load();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_lblSalesDetails %>"></asp:Label>
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
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 22px">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 22px">
                                                        <asp:DropDownList ID="ddlStoreName" runat="server" Width="165px" AutoPostBack="True"
                                                            onselectedindexchanged="ddlStoreName_SelectedIndexChanged" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 22px">
                                                        <asp:Label ID="Label13" runat="server" Width="73px" Height="17px" Text="<%$ Resources:BaseInfo,Rpt_Sumtype %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="height: 22px">
                                                        <asp:RadioButton ID="rdoShopSum" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Rpt_ShopSum %>" Checked="True" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 22px;" class="lable">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 218px; height: 22px;"><asp:DropDownList ID="ddlBuildingName" 
                                                            runat="server" Width="165px" AutoPostBack="True" 
                                                            OnSelectedIndexChanged="ddlBuildingName_SelectedIndexChanged" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 22px;">
                                                        </td>
                                                    <td style="height: 22px">
                                                        <asp:RadioButton ID="rdoShopDetail" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Rpt_ShopDetail %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 17px;">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 17px;"><asp:DropDownList ID="ddlFloorName" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlFloorName_SelectedIndexChanged" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 17px;">
                                                    </td>
                                                    <td style="height: 17px">
                                                        <asp:RadioButton ID="rdoSKUSum" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Rpt_SKUSum %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 16px;" class="lable">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_BizMode %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 16px;"><asp:DropDownList ID="ddlBizMode" runat="server" Width="165px" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 16px;">
                                                        </td>
                                                    <td style="height: 16px">
                                                        <asp:RadioButton ID="rdoSaleDetail" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Rpt_SaleDetail %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblAreaName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 15px;">
                                                        <asp:DropDownList ID="ddlAreaName" runat="server" Width="165px" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 15px;" class="lable" align="right">
                                                    <asp:Label ID="Label5" runat="server" Width="73px" Height="17px" Text="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="height: 15px">
                                                        <asp:RadioButton ID="RB1" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,Rpt_rdoAll %>" Checked="True" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 7px;">
                                                        <asp:DropDownList ID="ddlShopCode" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                    </td>
                                                    <td style="height: 7px">
                                                        <asp:RadioButton ID="RB2" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,DataSource_POS %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 12px;">
                                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_SDate %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 12px;">
                                                        <asp:TextBox ID="txtBizSDate" runat="server" CssClass="ipt160px" Height="17px" onclick="calendar()"></asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 44px; height: 12px;">
                                                    </td>
                                                    <td style="height: 12px">
                                                        <asp:RadioButton ID="RB3" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,DataSource_Put %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 10px;">
                                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_EDate %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 10px;">
                                                        <asp:TextBox ID="txtBizEDate" runat="server" CssClass="ipt160px" Height="17px" onclick="calendar()"></asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        <asp:RadioButton ID="RB4" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,DataSource_Manual %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 16px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 16px;"></td>
                                                    <td style="width: 44px; height: 16px;">
                                                    </td>
                                                    <td style="height: 16px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 10px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />
                                                        <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> "  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="BtnCel_Click"/></td>
                                                    <td style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        <asp:RadioButton ID="RadioButton12" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        </td>
                                                    <td style="width: 218px; height: 4px;">
                                                        </td>
                                                    <td style="width: 44px; height: 4px;">
                                                    </td>
                                                    <td style="height: 4px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
