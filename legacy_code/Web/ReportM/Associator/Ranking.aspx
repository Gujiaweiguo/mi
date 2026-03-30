<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ranking.aspx.cs" Inherits="ReportM_Associator_Ranking" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
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
        addTabTool("<%=baseInfo %>,ReportM/Associator/Ranking.aspx");
        loadTitle();
	}
	
		     //验证数字类型
        function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110)
            {
		        window.event.returnValue =true;
	        }else
	        {
		        window.event.returnValue =false;
	        }
	    }
	
function TABLE1_onclick() {

}

	</script>
</head>
<body style="margin-top:0; margin-left:0"; onload='Load()'>
    <form id="form1" runat="server">
    <div>
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
                                <asp:Label ID="Label1" runat="server" Text="积分点和购买额排序"></asp:Label>
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
                        <tr>
                            <td align="left" valign="bottom">
                                <table border="0" cellpadding="0" cellspacing="0" height="430" width="688">
                                    <tr>
                                        <td align="left" height="40" valign="middle">
                                            <table border="0" cellpadding="0" cellspacing="0" height="40" width="380">
                                                <tr>
                                                    <td align="center" width="100">
                                                        <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="日期范围"></asp:Label></td>
                                                    <td align="right" width="152">
                                                        <asp:TextBox ID="txtStartBizTime" runat="server" CssClass="ipt160px" Height="18px"
                                                            onclick="calendar()" Width="79px"></asp:TextBox>
                                                    </td>
                                                    <td align="center" width="128">
                                                        &nbsp;<asp:TextBox ID="txtEndBizTime" runat="server" CssClass="ipt160px" Height="18px"
                                                            onclick="calendar()" Width="78px"></asp:TextBox></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" valign="top" style="height: 232px">
                                            <table border="0" cellpadding="0" cellspacing="0" height="224" width="640" id="TABLE1" onclick="return TABLE1_onclick()">
                                                <tr>
                                                    <td align="left" height="28" valign="middle" style="width: 73px">
                                                        排序方式</td>
                                                    <td style="width: 85px">
                                                        &nbsp;</td>
                                                    <td style="width: 94px">
                                                        &nbsp;</td>
                                                    <td style="width: 172px">
                                                        &nbsp;</td>
                                                    <td width="128">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" height="28" valign="middle" style="width: 73px">
                                                        <asp:RadioButton ID="RadioButton1" runat="server" Text="购买总额前排序" Width="107px" Checked="True" CssClass="labelStyle" GroupName="Ranking" /></td>
                                                    <td style="width: 85px">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" style="width: 94px">
                                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="选项:总额大于"></asp:Label></td>
                                                    <td align="right" valign="middle" style="width: 172px">
                                                        <asp:TextBox ID="txtSum" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="middle" style="height: 28px; width: 73px;"><asp:RadioButton ID="RadioButton2" runat="server" Text="积分前排序" Width="82px" CssClass="labelStyle" GroupName="Ranking" /></td>
                                                    <td style="height: 28px; width: 85px;">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" style="height: 28px; width: 94px;">
                                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="选项:积分大于"></asp:Label></td>
                                                    <td align="right" valign="middle" style="height: 28px; width: 172px;">
                                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                                                    <td style="height: 28px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="middle" style="height: 28px; width: 73px;"><asp:RadioButton ID="RadioButton3" runat="server" Text="购买频次前排序" Width="107px" CssClass="labelStyle" GroupName="Ranking" /></td>
                                                    <td style="height: 28px; width: 85px;">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" style="width: 94px; height: 28px">
                                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="选项:数量大于"></asp:Label></td>
                                                    <td align="right" valign="middle" style="height: 28px; width: 172px;">
                                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                                                    <td style="height: 28px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" height="28" valign="middle" style="width: 73px"><asp:RadioButton ID="RadioButton4" runat="server" Text="购买总额排序" Width="93px" CssClass="labelStyle" GroupName="Ranking" /></td>
                                                    <td style="width: 85px">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" style="width: 94px">
                                                        <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="选项:总额小于"></asp:Label></td>
                                                    <td align="right" valign="middle" style="width: 172px">
                                                        <asp:TextBox ID="TextBox3" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" valign="middle" style="height: 28px; width: 73px;"><asp:RadioButton ID="RadioButton5" runat="server" Text="积分后排序" Width="81px" CssClass="labelStyle" GroupName="Ranking" /></td>
                                                    <td style="height: 28px; width: 85px;">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" style="width: 94px; height: 28px">
                                                        <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="选项:积分小于"></asp:Label></td>
                                                    <td align="right" valign="middle" style="height: 28px; width: 172px;">
                                                        <asp:TextBox ID="TextBox4" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                                                    <td style="height: 28px">
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="right" height="28" valign="middle" style="width: 73px"><asp:RadioButton ID="RadioButton6" runat="server" Text="购买频次后排序" Width="105px" CssClass="labelStyle" GroupName="Ranking" /></td>
                                                    <td style="width: 85px">
                                                        &nbsp;</td>
                                                    <td align="left" valign="middle" style="width: 94px">
                                                        <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="选项:数量小于"></asp:Label></td>
                                                    <td align="right" valign="middle" style="width: 172px">
                                                        <asp:TextBox ID="TextBox5" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox></td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="center" height="28" valign="middle" style="width: 73px">
                                                        </td>
                                                    <td style="width: 85px">
                                                        &nbsp;</td>
                                                    <td style="width: 94px" align="left">
                                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="排序数量" ></asp:Label></td>
                                                    <td align="right" valign="middle" style="width: 172px">
                                                        <asp:TextBox ID="TextBox6" runat="server" CssClass="ipt160px" Height="18px" Width="164px"></asp:TextBox>
                                                        </td>
                                                    <td>
                                                        &nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" height="95" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" height="80" width="292">
                                                <tr>
                                                    <td width="29">
                                                        &nbsp;</td>
                                                    <td align="left" height="20" valign="bottom" width="133">
                                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="分类标准"></asp:Label></td>
                                                    <td align="left" valign="bottom" style="width: 130px">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td align="left" height="30" valign="middle"><asp:RadioButton ID="RadioButton7" runat="server" Text="降序" Width="100px" Checked="True" CssClass="labelStyle" GroupName="order" /></td>
                                                    <td align="left" valign="middle" style="width: 130px">
                                                        </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        &nbsp;</td>
                                                    <td align="left" height="30" valign="middle"><asp:RadioButton ID="RadioButton8" runat="server" Text="升序" Width="100px" CssClass="labelStyle" GroupName="order" /></td>
                                                    <td align="left" valign="middle" style="width: 130px">
                                                        </td>
                                                </tr>
                                            </table>
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                            <asp:Button ID="btnCancel" runat="server" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                    CssClass="buttonCancel" OnClick="BtnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %> "
                                                    /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
        </asp:UpdatePanel>    
        </div>

    </form>
</body>
</html>
