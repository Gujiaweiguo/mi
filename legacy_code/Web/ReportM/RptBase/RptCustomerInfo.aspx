<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptCustomerInfo.aspx.cs" Inherits="RptBaseMenu_RptCustomerInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Rpt_lblCustInfo")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>    
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript">
    function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptBase/RptCustomerInfo.aspx");
	        loadTitle();
	    }
    function aa()
    {
    
        parent.document.all.mainFrame.scrolling="NO";
    }
    </script>
    
</head>
<body style="margin:0px" onload="Load();">
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo, Rpt_lblCustInfo %>"></asp:Label>
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
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                            </asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlBizproject" runat="server" AutoPostBack="true" 
                                                            Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 28px;" class="lable">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                                    <td style="width: 218px; height: 28px;">
                                                        <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
                                                    <td style="width: 44px; height: 28px;">
                                                        <asp:Label ID="Label13" runat="server" Text="排序顺序" Width="73px" Visible="False"></asp:Label></td>
                                                    <td style="height: 28px">
                                                        <asp:RadioButton ID="RadioButton4" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
                                                    <td style="width: 44px">
                                                        </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlCustTypeID" runat="server" Width="165px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                        </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton6" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustLicenseType %>" Visible="False"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlCustLicenseType" runat="server" Width="165px" Visible="False">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px" class="lable" align="right">
                                                        </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton7" runat="server" GroupName="b" Visible="False" /></td>
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
                                                        <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="btnOK_Click"/>
                                                        <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="btnCancel_Click" /></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton8" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton9" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton10" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton11" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        </td>
                                                    <td style="width: 218px">
                                                        &nbsp;
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_LicenseStartDate %>" Visible="False"></asp:Label><asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" onclick="calendar()" Visible="False"></asp:TextBox><asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_LicenseEndDate %>" Visible="False"></asp:Label><asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" onclick="calendar()" Visible="False"></asp:TextBox></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px; height: 28px;">
                                                    </td>
                                                    <td style="width: 218px; height: 28px;">
                                                        &nbsp;</td>
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
