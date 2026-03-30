<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptMemberInformation.aspx.cs" Inherits="ReportM_RptMember_RptMemberInformation" %>

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
	        addTabTool("<%=baseInfo %>,ReportM/RptBase/RptUnitInfo.aspx");
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_Title_lblUnitInfo %>"></asp:Label>
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
                                                        <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitStatus %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 218px"><asp:RadioButton ID="rdoLeaseOut" runat="server" GroupName="a" Text="<%$ Resources:BaseInfo,RentableArea_LeaseOut %>" CssClass="labelStyle"/></td>
                                                    <td style="width: 44px">
                                                        <asp:Label ID="Label13" runat="server" Text="排序顺序" Width="73px" Visible="False"></asp:Label></td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton4" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px"><asp:RadioButton ID="rdoNoLeaseOut" runat="server" GroupName="a" Text="<%$ Resources:BaseInfo,RentableArea_NoLeaseOut %>" CssClass="labelStyle" /></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton5" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        </td>
                                                    <td style="width: 218px"><asp:RadioButton ID="rdoBlankOut" runat="server" GroupName="a" Text="<%$ Resources:BaseInfo,RentableArea_BlankOut %>" CssClass="labelStyle"/></td>
                                                    <td style="width: 44px">
                                                        </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton6" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        </td>
                                                    <td style="width: 218px"><asp:RadioButton ID="rdoall" runat="server" GroupName="a" Text="<%$ Resources:BaseInfo, Rpt_UnitAll %>" CssClass="labelStyle"/></td>
                                                    <td style="width: 44px" class="lable" align="right">
                                                        </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton7" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitCode %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtUnitCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton8" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlBuildingName" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton9" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlFloorName" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton10" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaName %>"></asp:Label></td>
                                                    <td style="width: 218px"><asp:DropDownList ID="ddlAreaName" runat="server" Width="165px">
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton11" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationName %>" Visible="False"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        &nbsp;<asp:DropDownList ID="ddlLocationName" runat="server" Width="165px" Visible="False">
                                                    </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnOK" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click"  /><asp:Button ID="BtnCel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> "  Width="80px"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px">
                                                        &nbsp;
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
