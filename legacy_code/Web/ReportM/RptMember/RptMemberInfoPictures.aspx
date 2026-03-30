<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptMemberInfoPictures.aspx.cs" Inherits="ReportM_RptMember_RptMemberInfoPictures" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--
/// 编写人：hesijian
/// 编写时间：2009年7月15日
-->
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
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
	        addTabTool("<%=baseInfo %>,ReportM/RptMember/RptMemberInfoPictures.aspx");
	        loadTitle();
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo, Rpt_MemberInfoPicture %>"></asp:Label>
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
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_MemberDescType %>" ></asp:Label></td>
                                                    <td style="width: 218px">
                                                       <asp:DropDownList ID="txtMemberDesc" runat="server" Width="165px" ></asp:DropDownList></td>
                                                    <td style="width: 44px" class="lable">
                                                        <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Rpt_CheckType %>" Width="73px" ></asp:Label></td>
                                                    <td>
                                                        <asp:RadioButton ID="Rdo1" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Rpt_MembNumbers %>" Checked="true" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_JoinGroupDate %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                     <asp:RadioButton ID="Rdo2" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Associator_MemberBonus %>"  />
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_LeaveGroupDate %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="Rdo3" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Associator_MemberChangeNum %>" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                    </td>
                                                    <td style="width: 218px">
                                                     </td>
                                                    <td style="width: 44px" class="lable" align="right">
                                                     </td>
                                                    <td>
                                                        <asp:RadioButton ID="Rdo4" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Associator_MemberChangeSum %>" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                    </td>
                                                    <td style="width: 218px">
                                                        </td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        <asp:RadioButton ID="RadioButton8" runat="server" GroupName="b" Visible="False" /></td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td class="lable" style="width: 89px">
                                                       </td>
                                                    <td style="width: 218px">
                                                     <asp:Button ID="Button1" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click"/>
                                                        &nbsp;<asp:Button ID="Button2" runat="server" CssClass="buttonCancel" 
                                                            Text="<%$ Resources:BaseInfo,User_btnCancel %> " OnClick="btnCancel_Click"/>
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
                                                    <td style="width: 89px" class="lable">
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
                                                        &nbsp;
                                                    </td>
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
