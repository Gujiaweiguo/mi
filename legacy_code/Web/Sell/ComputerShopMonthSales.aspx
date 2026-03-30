<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComputerShopMonthSales.aspx.cs" Inherits="Sell_ComputerShopMonthSales" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml"  >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../JavaScript/Common.js"></script>
    <script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
    <script language="javascript" type="text/javascript" src="../JavaScript/setPeriod.js" charset="gb2312"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=strFresh %>,sell/ComputerShopMonthSales.aspx");
	        loadTitle();
	    }
	    function BtnUp( p )
        {
	        var t = String(p)
	        var l = t.substring(3,15); 
	        document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
        }
        function BtnOver( p )
        {
	        var t = String(p)
	        var l = t.substring(3,15); 
	        document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
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
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Menu_ShopMonthSales %>"></asp:Label>
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
                                                        <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>" CssClass="labelStyle"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:DropDownList ID="ddlStore" runat="server" CssClass="ipt160px" Width="162px">
                                                        </asp:DropDownList></td>
                                                    <td style="width: 44px">
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr class="bodyTbl">
                                                    <td style="width: 89px" class="lable">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_InvDate %>"></asp:Label></td>
                                                    <td style="width: 218px">
                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                    <td style="width: 44px" class="lable" align="right">
                                                        </td>
                                                    <td>
                                                        </td>
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
                                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Text="<%$ Resources:BaseInfo,User_btnOk %> "  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="btnSave_Click"/>&nbsp;
                                                        <asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> "  onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);" OnClick="BtnCancel_Click"/></td>
                                                    <td align="right" class="lable" style="width: 44px">
                                                    </td>
                                                    <td>
                                                        </td>
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
