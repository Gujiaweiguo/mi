<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptShopInfo.aspx.cs" Inherits="ReportM_Shop_ShopUserRpt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Rpt_ShopInfo")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptBase/RptShopInfo.aspx");
	        loadTitle();
	    }
    </script>
    
</head>
<body style="margin:0px" onload="Load();">
    <form id="form1" runat="server">
         <table id="showmain" border="0" cellpadding="0" cellspacing="0"
            style="width: 100%;">
            <tr>
                <td style="vertical-align: top; width: 100%; height: 401px" valign="top">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="width:100%">
                                <table class="mainTbl" border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr style="height: 28px">
                                        <td valign="top">
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo, Rpt_ShopInfo %>"></asp:Label>
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
                                                    <td style="width: 266px">
                                                        
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Store_StoreName %>"></asp:Label></td>
                                                    <td style="width: 266px">
                                                        <asp:DropDownList ID="ddlStoreName" runat="server" Width="165px" AutoPostBack="True"
                                                            onselectedindexchanged="ddlStoreName_SelectedIndexChanged" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label2" runat="server" 
                                                            Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>" 
                                                            CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 266px">
                                                        <asp:DropDownList ID="ddlBuildingName" 
                                                            runat="server" Width="165px" AutoPostBack="True" 
                                                            OnSelectedIndexChanged="ddlBuildingName_SelectedIndexChanged" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label14" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label></td>
                                                    <td style="width: 266px">
                                                        <asp:DropDownList ID="ddlFloorName" runat="server" Width="165px" 
                                                            AutoPostBack="True" 
                                                            OnSelectedIndexChanged="ddlFloorName_SelectedIndexChanged" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                                                    <td style="width: 266px">
                                                        <asp:DropDownList ID="ddlShopCode" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>"></asp:Label></td>
                                                    <td style="width: 266px">
                                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        &nbsp;</td>
                                                    <td style="width: 266px">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                        <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="BtnCel_Click"/></td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 266px">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        &nbsp;</td>
                                                    <td style="width: 266px">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        </td>
                                                    <td style="width: 266px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        </td>
                                                    <td style="width: 266px">
                                                        &nbsp;
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 266px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 266px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 266px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 266px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                       </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
             </tr>
           </table>  
    </form>
</body>
</html>
