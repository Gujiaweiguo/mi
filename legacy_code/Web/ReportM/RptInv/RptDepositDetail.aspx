<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptDepositDetail.aspx.cs" Inherits="ReportM_RptInv_RptDepositDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_RptDepositDetail")%></title>
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
	        addTabTool("<%=fresh %>,ReportM/RptInv/RptDepositDetail.aspx");
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
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="tdTopRightBackColor" style="width: 5px">
                            <img class="imageLeftBack" />
                        </td>
                        <td class="tdTopRightBackColor" style="text-align: left">
                            <asp:Label ID="Label1" runat="server" Style="position: static" Text="<%$ Resources:BaseInfo, Menu_RptDepositDetail %>"></asp:Label>
                        </td>
                        <td class="tdTopRightBackColor" style="width: 5px">
                            <img class="imageRightBack" />
                        </td>
                    </tr>
                    <tr style="height: 1px">
                        <td colspan="3" style="height: 1px; background-color: white">
                        </td>
                    </tr>
                </table>
                <table class="tdBackColor" style="width: 100%">
                    <tr style="height: 10px">
                        <td style="width: 89px">
                        </td>
                        <td style="width: 218px">
                        </td>
                        <td style="width: 65px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaTitle %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:DropDownList ID="ddlArea" runat="server" Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddlArea_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td style="width: 65px">
                            <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_RptType %>"></asp:Label></td>
                        <td>
                            <asp:RadioButton ID="rbtSum" runat="server" Text="<%$ Resources:BaseInfo,Rpt_StoreRentInfoDetail %>" Checked="True" CssClass="labelStyle" GroupName="aa"/></td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_City %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:DropDownList ID="ddlCity" runat="server" Width="160px" AutoPostBack="True" OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td style="width: 65px">
                        </td>
                        <td>
                            <asp:RadioButton ID="rbtDetail" runat="server" Text="<%$ Resources:BaseInfo,Rpt_StoreRentInfo %>" CssClass="labelStyle" GroupName="aa"/></td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:DropDownList ID="DDlStore" runat="server" Width="160px">
                            </asp:DropDownList></td>
                        <td style="width: 65px">
                        </td>
                        <td>
                            </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Contract_ManageCompany %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:DropDownList ID="ddlSubs" runat="server" Width="160px">
                            </asp:DropDownList></td>
                        <td align="right" class="lable" style="width: 65px">
                        </td>
                        <td>
                            </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px; height: 28px;">
                            </td>
                        <td style="width: 218px; height: 28px;">
                            </td>
                        <td align="right" class="lable" style="width: 65px; height: 28px;">
                        </td>
                        <td style="height: 28px">
                            </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                                                        </td>
                        <td style="width: 218px">
                                                        </td>
                        <td style="width: 65px">
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                            <asp:Button ID="BtnCancel" runat="server" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="BtnCancel_Click" /></td>
                        <td align="right" class="lable" style="width: 65px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                                                        &nbsp;
                            </td>
                        <td style="width: 65px">
                        </td>
                        <td>
                        </td>
                    </tr>
                     <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                                                        &nbsp;
                            </td>
                        <td style="width: 65px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                                                        &nbsp;
                            </td>
                        <td style="width: 65px">
                        </td>
                        <td>
                        </td>
                    </tr>
                                        <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                                                        &nbsp;
                            </td>
                        <td style="width: 65px">
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
