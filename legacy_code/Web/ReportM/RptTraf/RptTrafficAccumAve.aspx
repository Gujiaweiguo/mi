<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptTrafficAccumAve.aspx.cs" Inherits="ReportM_RptTraf_RptTrafficAccumAve" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%=strBaseInfo %></title>
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
	        addTabTool("<%=strFresh %>,ReportM/RptTraf/RptTrafficAccumAve.aspx");
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_TrafficAccumAve %>"></asp:Label>
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
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>"
                                                            Width="50px"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlStoreName" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                        </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_Selected %>"></asp:Label><asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Year %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                        <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="BtnCel_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 25px;">
                                                        </td>
                                                    <td style="width: 218px; height: 25px;">
                                                        </td>
                                                    <td style="width: 44px; height: 25px;">
                                                    </td>
                                                    <td style="height: 25px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 21px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 21px;">
                                                        </td>
                                                    <td style="width: 44px; height: 21px;">
                                                        </td>
                                                    <td style="height: 21px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 21px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 21px;">
                                                        </td>
                                                    <td style="width: 44px; height: 21px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 21px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 22px;">
                                                        </td>
                                                    <td style="width: 218px; height: 22px;">
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 22px;">
                                                    </td>
                                                    <td style="height: 22px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 26px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 26px;">
                                                        </td>
                                                    <td style="width: 44px; height: 26px;">
                                                    </td>
                                                    <td style="height: 26px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 15px;">
                                                        </td>
                                                    <td style="width: 44px; height: 15px;">
                                                    </td>
                                                    <td style="height: 15px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 28px;">
                                                        </td>
                                                    <td style="width: 218px; height: 28px;">
                                                        </td>
                                                    <td style="width: 44px; height: 28px;">
                                                    </td>
                                                    <td style="height: 28px">
                                                        </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>