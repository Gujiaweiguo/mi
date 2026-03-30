<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptShopSKUInfo.aspx.cs" Inherits="ReportM_RptSale_RptShopSKUInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Rpt_ShopSKUInfo") %></title>
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
    <script type="text/javascript" src="../../JavaScript/setPeriod.js" charset="gb2312"></script>
        <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptSale/RptShopSKUInfo.aspx");
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_ShopSKUInfo %>"></asp:Label>
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
                                                    <td class="lable" style="width: 89px; height: 22px"><!--项目名称-->
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 22px">
                                                        <asp:DropDownList ID="ddlStoreName" runat="server" Width="165px"  OnSelectedIndexChanged="ddlBizproject_SelectedIndexChanged" AutoPostBack="True">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 22px">
                                                  </td>
                                                    <td style="height: 22px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 22px;" class="lable"><!--大楼名称-->
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 218px; height: 22px;"><asp:DropDownList ID="ddlBuildingName" runat="server" Width="165px" OnSelectedIndexChanged="ddlBuildingName_SelectedIndexChanged" AutoPostBack="True">
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 22px;">
                                                        </td>
                                                    <td style="height: 22px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 17px;"><!--楼层名称-->
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 17px;"><asp:DropDownList ID="ddlFloorName" 
                                                            runat="server" Width="165px" AutoPostBack="True" 
                                                            onselectedindexchanged="ddlFloorName_SelectedIndexChanged" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 17px;">
                                                    </td>
                                                    <td style="height: 17px">
                                                       </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 16px;" class="lable"><!--经营方式-->
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 16px;"><asp:DropDownList ID="ddlCustCode" runat="server" Width="165px" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 16px;">
                                                        </td>
                                                    <td style="height: 16px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable"><!--商铺类型-->
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_SkuClass %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 15px;">
                                                        <asp:DropDownList ID="ddlSkuClass" runat="server" Width="165px" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 15px;" class="lable" align="right">
                                                    </td>
                                                    <td style="height: 15px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;"><!--商铺名称-->
                                                        </td>
                                                    <td style="width: 218px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                    </td>
                                                    <td style="height: 7px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 12px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 12px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px; height: 12px;">
                                                    </td>
                                                    <td style="height: 12px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 10px;">
                                                        </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />
                                                        &nbsp; <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> "  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="BtnCel_Click"/></td>
                                                    <td align="right" class="lable" style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        </td>
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
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        </td>
                                                    <td style="width: 218px; height: 4px;">
                                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_EDate %>" Visible="False"></asp:Label><asp:TextBox ID="txtBizEDate" runat="server" CssClass="ipt160px" Height="20px" onclick="calendar()" Visible="False"></asp:TextBox></td>
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
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
