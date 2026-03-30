<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptMallSalesSum.aspx.cs" Inherits="RptBaseMenu_RptMallSalesSum" ResponseEncoding="utf-16" %>

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
	        addTabTool("<%=Fresh %>,ReportM/RptSale/RptMallSalesSum.aspx");
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_lblMallSalesSum %>"></asp:Label>
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
                                                <tr style="height: 10px">
                                                    <td style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr style="height:10px">
                                                    <td style="width: 89px; text-align: right;">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                            </asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlBizproject" runat="server"
                                                            Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                        <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Height="17px" 
                                                            Text="<%$ Resources:BaseInfo,Lease_RptType %>" Width="73px"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RB1" runat="server" Checked="True" CssClass="labelStyle" 
                                                            GroupName="a" Text="<%$ Resources:BaseInfo,Lease_RptDay %>" 
                                                            AutoPostBack="True" oncheckedchanged="RB1_CheckedChanged" />
                                                    </td>
                                                </tr>
                                                <tr style="height:10px">
                                                    <td style="width: 89px; text-align: right;">
                                                        <asp:Label ID="Label14" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_SalesMonth %>" Visible="False"></asp:Label>
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_SDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtBizSDate" runat="server" CssClass="ipt160px" 
                                                            onclick="calendar()" Width="160px"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        <asp:RadioButton ID="RB2" runat="server" CssClass="labelStyle" GroupName="a" 
                                                            Text="<%$ Resources:BaseInfo,Lease_RptMonth %>" AutoPostBack="True" 
                                                            oncheckedchanged="RB2_CheckedChanged" />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 22px;" class="lable">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" 
                                                            Text="<%$ Resources:BaseInfo,Rpt_EDate %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px; height: 22px;">
                                                        <asp:TextBox ID="txtBizEDate" runat="server" CssClass="ipt160px" 
                                                            onclick="calendar()"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px; height: 22px;">
                                                        </td>
                                                    <td style="height: 22px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 14px;">
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" 
                                                            OnClick="btnOK_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" 
                                                            OnClick="btnCancel_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %> " />
                                                    </td>
                                                    <td style="width: 44px; height: 14px;">
                                                    </td>
                                                    <td style="height: 14px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 14px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 14px;">
                                                        </td>
                                                    <td style="height: 14px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="height: 14px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="height: 14px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="height: 14px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="height: 14px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="height: 14px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 14px;">
                                                        &nbsp;</td>
                                                    <td style="height: 14px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 13px;" class="lable">
                                                        </td>
                                                    <td style="width: 218px; height: 13px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 13px;" class="lable" align="right">
                                                        </td>
                                                    <td style="height: 13px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 13px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 13px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px; height: 13px;">
                                                        &nbsp;</td>
                                                    <td style="height: 13px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px; height: 13px;">
                                                        &nbsp;</td>
                                                    <td style="width: 218px; height: 13px;">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px; height: 13px;">
                                                        &nbsp;</td>
                                                    <td style="height: 13px">
                                                        &nbsp;</td>
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
