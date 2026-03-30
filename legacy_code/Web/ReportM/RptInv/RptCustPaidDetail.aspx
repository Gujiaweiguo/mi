<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptCustPaidDetail.aspx.cs" Inherits="RptBaseMenu_RptCustPaidDetail" ResponseEncoding="utf-16" %>

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
    <script type="text/javascript" src="../../JavaScript/setPeriod.js" charset="gb2312"></script>
        <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptInv/RptCustPaidDetail.aspx");
	        loadTitle();
	    }
	</script>
</head>
<body style="margin:0px"  onload ="Load();">
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_lblCustPaidDetail %>"></asp:Label>
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
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                            </asp:Label>
                                                        </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlBizproject" runat="server" AutoPostBack="true" 
                                                            Width="165px">
                                                        </asp:DropDownList>
                                                        </td>
                                                    <td style="width: 44px">
                                                        <asp:Label ID="Label13" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,RPT_lblRptType %>" Width="73px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton4" runat="server" CssClass="labelStyle" 
                                                            GroupName="b" Text="<%$ Resources:BaseInfo,RPT_lblABA %>" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>" Width="50px"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtContractID" runat="server" CssClass="ipt160px" 
                                                            MaxLength="32" Width="160px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton5" runat="server" CssClass="labelStyle" 
                                                            GroupName="b" Text="<%$ Resources:BaseInfo,RPT_lblCBA %>" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label14" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_invStartDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtInvStartDate" runat="server" CssClass="ipt160px" 
                                                            onclick="calendar()"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label15" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_invEndDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtInvEndDate" runat="server" CssClass="ipt160px" 
                                                            onclick="calendar()"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 25px;">
                                                        </td>
                                                    <td style="width: 218px; height: 25px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click"  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="btnCancel_Click"/></td>
                                                    <td style="width: 44px; height: 25px;">
                                                    </td>
                                                    <td style="height: 25px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 21px;" class="lable">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>" Visible="False"></asp:Label></td>
                                                    <td style="width: 218px; height: 21px;">
                                                        <asp:DropDownList ID="ddlBuildingName" runat="server" Width="165px" Visible="False" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 21px;">
                                                        </td>
                                                    <td style="height: 21px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 21px;" class="lable">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>" Visible="False"></asp:Label></td>
                                                    <td style="width: 218px; height: 21px;">
                                                        <asp:DropDownList ID="ddlFloorName" runat="server" Width="165px" Visible="False" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 21px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 21px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 22px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 22px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px; height: 22px;">
                                                    </td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 28px;">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>" Visible="False"></asp:Label></td>
                                                    <td style="width: 218px; height: 28px;">
                                                        <asp:DropDownList ID="ddlContractStatus" runat="server" Width="165px" Visible="False" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 28px;">
                                                        &nbsp;</td>
                                                    <td style="height: 28px">
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
