<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptUser.aspx.cs" Inherits="ReportM_RptBase_DeptUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%=baseInfo%></title>
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
	        addTabTool("刷新,ReportM/RptBase/DeptUser.aspx");
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
                            <div>
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_DeptUser %>"></asp:Label>
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
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_lblDeptName %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlProjuct" CssClass="ipt160px" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserStatusStr %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="cmbUserState" CssClass="ipt160px" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserName %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtName" CssClass="ipt160px" runat="server"></asp:TextBox>
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        <asp:Button ID="btnOK0" runat="server" CssClass="buttonQuery" 
                                                            OnClick="btnOK_Click" Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                                        &nbsp;<asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" 
                                                            OnClick="BtnCel_Click" onmouseout="BtnUp(this.id);" 
                                                            onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %> " />
                                                    </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td style="width: 44px" class="lable" align="right">
                                                        </td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        &nbsp;</td>
                                                    <td style="width: 218px">
                                                        &nbsp;</td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                        &nbsp;</td>
                                                    <td>
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
                                                    <td style="width: 89px; height: 28px; text-align: right;">
                                                        </td>
                                                    <td style="width: 218px; height: 28px;">
                                                        &nbsp;</td>
                                                    <td style="width: 44px; height: 28px;">
                                                    </td>
                                                    <td style="height: 28px">
                                                        </td>
                                                </tr>
                                            </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
    </form>
</body>
</html>
