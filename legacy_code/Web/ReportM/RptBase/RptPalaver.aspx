<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptPalaver.aspx.cs" Inherits="ReportM_RptBase_RptPalaver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
	        addTabTool("<%=Fresh %>,ReportM/RptBase/RptPalaver.aspx");
	        loadTitle();
	    }
	    function BtnUp( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
}
function BtnOver( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
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
                                                    <td style="width:5px; height: 28px;" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left; height: 28px;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_CustPalaver %>"></asp:Label>
                                                    </td>
                                                    <td style="width:5px; height: 28px;" class="tdTopRightBackColor">
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
                                                    <td style="width: 134px">
                                                    </td>
                                                    <td style="width: 604px">
                                                        
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td style="width: 252px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 134px; text-align: right;" class="lable">
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 604px; text-align: left;"><asp:DropDownList ID="ddlStoreName" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlStoreName_SelectedIndexChanged" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                        </td>
                                                    <td style="width: 252px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 134px; text-align: right;">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Hidden_hidCustID %>"></asp:Label></td>
                                                    <td style="width: 604px; text-align: left;">
                                                        <asp:TextBox ID="txtPotCustID" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td style="width: 252px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 134px; text-align: right;" class="lable">
                                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Hidden_hidCustName %>"></asp:Label></td>
                                                    <td style="width: 604px; text-align: left;">
                                                        <asp:TextBox ID="txtPotCustName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                                    <td style="width: 44px; height: 28px;">
                                                        </td>
                                                    <td style="width: 252px; height: 28px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 134px; text-align: right;" class="lable">
                                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustAttractProcessType %>"></asp:Label></td>
                                                    <td style="width: 604px; text-align: left;">
                                                        <asp:DropDownList ID="ddlProcessTypeName" runat="server" Width="165px" AutoPostBack="True" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px" class="lable" align="right">
                                                        </td>
                                                    <td style="width: 252px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 134px; text-align: right;">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblCommOper %>"></asp:Label></td>
                                                    <td style="width: 604px; text-align: left;">
                                                        <asp:TextBox ID="txtCommOper" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td style="width: 252px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 134px; text-align: right;">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPalaverTime %>"></asp:Label></td>
                                                    <td style="width: 604px; text-align: left;">
                                                        <asp:TextBox ID="txtPalaverTime" runat="server" CssClass="ipt160px"  onclick="calendar()"　></asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td style="width: 252px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 134px; text-align: right">
                                                    </td>
                                                    <td style="width: 604px; text-align: left">
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td style="width: 252px">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 134px; text-align: right;" class="lable">
                                                        </td>
                                                    <td style="width: 604px; text-align: left;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                        <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="BtnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td style="width: 252px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 134px; text-align: right;" class="lable">
                                                        </td>
                                                    <td style="width: 604px; text-align: left;">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td style="width: 252px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 134px">
                                                        </td>
                                                    <td style="width: 604px">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td style="width: 252px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 134px; height: 28px; text-align: right;">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>" Visible="False"></asp:Label></td>
                                                    <td style="width: 604px; height: 28px;" align="left">
                                                        <asp:DropDownList ID="ddlBuildingName" runat="server" Width="165px" AutoPostBack="True" Visible="False" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 28px;">
                                                    </td>
                                                    <td style="width: 252px; height: 28px;">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 134px; height: 28px; text-align: right">
                                                    <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_IntentUnits %>" Visible="False"></asp:Label></td>
                                                    <td style="width: 604px; height: 28px">
                                                        <asp:TextBox ID="txtIntentUnits" runat="server" CssClass="ipt160px" MaxLength="16" Visible="False" ></asp:TextBox></td>
                                                    <td style="width: 44px; height: 28px">
                                                    </td>
                                                    <td style="width: 252px; height: 28px">
                                                    </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

