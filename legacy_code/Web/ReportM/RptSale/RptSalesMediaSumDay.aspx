<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptSalesMediaSumDay.aspx.cs" Inherits="ReportM_RptSale_RptSalesMediaSumDay" %>

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
	        addTabTool("<%=Fresh %>,ReportM/RptSale/RptSalesMediaSumDay.aspx");
	        loadTitle();
	    }
	</script>

</head>
<body style="margin:0px"	onload ="Load();">
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Mneu_SalesMediaSumDay %>"></asp:Label>
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
                                                    <td style="width: 89px; height: 22px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 22px;">
                                                        </td>
                                                    <td style="width: 44px; height: 22px;">
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>" CssClass="labelStyle" Width="73px"  Height="17px"></asp:Label></td>
                                                    <td style="height: 22px">
                                                        <asp:RadioButton ID="RB1" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,Rpt_rdoAll %>" Checked="True" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 17px;">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 17px;"><asp:DropDownList ID="ddlShopCode" runat="server" Width="165px">
                                                    </asp:DropDownList></td>
                                                    <td style="width: 44px; height: 17px;">
                                                    </td>
                                                    <td style="height: 17px">
                                                        <asp:RadioButton ID="RB2" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,DataSource_POS %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 16px;" class="lable">
                                                        <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_SDate %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 16px;">
                                                        <asp:TextBox ID="txtStartBizTime" runat="server" CssClass="ipt160px" Height="18px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 44px; height: 16px;">
                                                        </td>
                                                    <td style="height: 16px">
                                                        <asp:RadioButton ID="RB3" runat="server" GroupName="c"  Text="<%$ Resources:BaseInfo,DataSource_Put %>" CssClass="labelStyle"/></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 15px;" class="lable">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_EDate %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 15px;">
                                                        <asp:TextBox ID="txtEndBizTime" runat="server" CssClass="ipt160px" Height="19px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 44px; height: 15px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 15px">
                                                       <asp:RadioButton ID="RB4" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,DataSource_Manual %>" CssClass="labelStyle"/> </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 7px;">
                                                        </td>
                                                    <td style="width: 218px; height: 7px;">
                                                        <asp:RadioButton ID="RBtnDetail" runat="server"  Text="<%$ Resources:BaseInfo,Rpt_ShopDetail %>" AutoPostBack="True" Checked="True" Font-Size="9pt" OnCheckedChanged="RBtnDetail_CheckedChanged"/>
                                                        <asp:RadioButton ID="RBtnTotal" runat="server"  Text="<%$ Resources:BaseInfo,Rpt_ShopSum %>" AutoPostBack="True" Font-Size="9pt" OnCheckedChanged="RBtnTotal_CheckedChanged"/></td>
                                                    <td align="right" class="lable" style="width: 44px; height: 7px;">
                                                    </td>
                                                    <td style="height: 7px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 12px;">
                                                        </td>
                                                    <td style="width: 218px; height: 12px;">
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 12px;">
                                                    </td>
                                                    <td style="height: 12px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 10px;">
                                                        </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 16px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 16px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="btnCancel_Click"/></td>
                                                    <td style="width: 44px; height: 16px;">
                                                    </td>
                                                    <td style="height: 16px">
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 10px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 10px;">
                                                        </td>
                                                    <td style="width: 44px; height: 10px;">
                                                    </td>
                                                    <td style="height: 10px">
                                                        </td>
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
                                                    <td class="lable" style="width: 89px; height: 4px;">
                                                        </td>
                                                    <td style="width: 218px; height: 4px;">
                                                        </td>
                                                    <td style="width: 44px; height: 4px;">
                                                    </td>
                                                    <td style="height: 4px">
                                                        </td>
                                                </tr>
                                            </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

