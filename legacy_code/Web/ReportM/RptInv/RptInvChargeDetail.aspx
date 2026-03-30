<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptInvChargeDetail.aspx.cs" Inherits="ReportM_RptInv_RptInvChargeDetail" %>

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
	        addTabTool("<%=baseInfo %>,ReportM/RptInv/RptInvChargeDetail.aspx");
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_ContractExpensesDetail %>"></asp:Label>
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
                                                    <td style="width: 89px; height: 10px;">
                                                    </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        
                                                    </td>
                                                    <td style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                    </td>
                                                </tr>
                                                <tr style="height:10px">
                                                    <td align="right" style="width: 89px; height: 10px;">
                                                        <asp:Label ID="Label15" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,MakePoolVoucher_lblSubs %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        <asp:DropDownList ID="ddlSubs" runat="server" Width="165px">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px; height: 10px;">
                                                        &nbsp;</td>
                                                    <td style="height: 10px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 22px;" class="lable">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                            </asp:Label></td>
                                                    <td style="width: 218px; height: 22px;">
                                                        <asp:DropDownList ID="ddlBizproject" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlBizproject_SelectedIndexChanged"
                                                            Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 22px;">
                                                        &nbsp;</td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 17px;">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 17px;">
                                                        <asp:DropDownList ID="ddlBuildingName" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlBuildingName_SelectedIndexChanged" >
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 17px;">
                                                    </td>
                                                    <td style="height: 17px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 16px">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 16px">
                                                        <asp:DropDownList ID="ddlFloorName" runat="server" Width="165px" >
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 16px">
                                                    </td>
                                                    <td style="height: 16px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 15px;">
                                                        <asp:TextBox ID="txtConractCode" runat="server" CssClass="ipt160px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px; height: 15px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 15px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 7px;">
                                                        <asp:DropDownList ID="ddlContractStatus" runat="server" Width="165px" >
                                                        </asp:DropDownList></td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                    </td>
                                                    <td style="height: 7px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 7px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" 
                                                            OnClick="btnOK_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                                            OnClick="btnCancel_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %> " />
                                                    </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="height: 7px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                        &nbsp;</td>
                                                    <td style="height: 7px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        </td>
                                                    <td style="width: 218px; height: 4px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 4px;">
                                                    </td>
                                                    <td style="height: 4px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; text-align: right;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">&nbsp;</td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; text-align: right;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; text-align: right;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

