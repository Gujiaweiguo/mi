<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptADpqReport.aspx.cs" Inherits="ReportM_RptBase_RptADpqReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_ADpqReport")%></title>
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
	        addTabTool("<%=fresh %>,ReportM/RptBase/RptADpqReport.aspx");
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
                            <asp:Label ID="Label1" runat="server" Style="position: static" Text="<%$ Resources:BaseInfo, Menu_ADpqReport %>"></asp:Label>
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
                        <td style="width: 44px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_StoreName %>"></asp:Label></td>
                        <td style="width: 218px">
                            <asp:DropDownList ID="ddlStore" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="ddlStore_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblAdBoardCode %>"></asp:Label></td>
                        <td style="width: 218px"><asp:DropDownList ID="ddlAdBoard" runat="server" Width="165px">
                        </asp:DropDownList></td>
                        <td style="width: 44px">
                            </td>
                        <td>
                            </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo, AdContract_lblAdType %>"></asp:Label></td>
                        <td style="width: 218px"><asp:DropDownList ID="ddlAdType" runat="server" Width="165px">
                        </asp:DropDownList></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                            </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                            </td>
                        <td style="width: 218px">
                            </td>
                        <td style="width: 44px">
                        </td>
                        <td>
                            </td>
                    </tr>
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 89px">
                                                        </td>
                        <td style="width: 218px">
                            <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                            <asp:Button ID="BtnCancel" runat="server" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                    CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="BtnCancel_Click" /></td>
                        <td style="width: 44px">
                        </td>
                        <td>
                            &nbsp;</td>
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
