<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OldMembChargeCard.aspx.cs" Inherits="Associator_Perform_OldMembChargeCard" ResponseEncoding="gb2312"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
      <title>旧会员发卡</title>
    <link href="../../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        <!--
        table.tbIntegral tr{ height:20px; }
        
        table.tbCard tr{ height:10px; }
        
        table.tbIntegral tr.headLine{ height:1px; }
        table.tbIntegral tr.bodyLine{ height:1px; }
        
        table.tbIntegral td.baseLable{ padding-right:5px;text-align:right;}
        table.tbIntegral td.baseInput{ align:left;padding-right:20px }
         .style1
         {
             width: 20%;
             height: 35px;
         }
         .style2
         {
             width: 30%;
             height: 35px;
         }
         .style3
         {
             width: 10%;
             height: 35px;
         }
         .style4
         {
             width: 40%;
             height: 35px;
         }
         .style5
         {
             width: 20%;
             height: 25px;
         }
         .style6
         {
             width: 30%;
             height: 25px;
         }
         .style7
         {
             width: 10%;
             height: 25px;
         }
         .style8
         {
             width: 40%;
             height: 25px;
         }
        --> 
    </style>  
    <script src="../../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../../JavaScript/Calendar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool("旧会员发卡,Associator/Perform/OldMembChargeCard.aspx");
	        loadTitle();
	    }
	    
	      //输入验证
        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtCardID.value))
            {
                parent.document.all.txtWroMessage.value =('请输入新会员卡卡号！');
                return false;
            }
        }
    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <div>
        <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                 <td style="height: 24px; text-align: left; width: 854px;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="旧会员发卡"></asp:Label></td>
            <td style="height: 24px; width: 663px;" class="tdTopRightBackColor" align="left"></td>
            <td style=" height: 24px;" class="tdTopRightBackColor" valign="top">
                <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
            </tr>
            <tr class="tdBackColor" style="height:20px">
                <td style="width: 854px">
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
             <tr class="tdBackColor">
                 <td colspan="2" style="padding-left:10px; padding-right:10px">
                     <table border="0" cellpadding="0" cellspacing="0" style="width: 117%">
                         <tr>
                             <td style="height: 1px; background-color: #738495">
                             </td>
                         </tr>
                         <tr>
                             <td style="height: 1px; background-color: #ffffff">
                             </td>
                         </tr>
                     </table>
                 </td>
                 <td>
                 </td>
             </tr>
            <tr class="tdBackColor">
                <td style="width: 854px; height: 21px;">
                </td>
                <td style="height: 21px">
                </td>
                <td style="height: 21px">
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="height: 21px;" colspan="3">
                    <table style="width:100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:20%; text-align:right;padding-right:5px">
                                <asp:Label ID="Label1" runat="server" Text="旧卡号"></asp:Label>
                            </td>
                            <td style="width:30%">
                                <asp:TextBox ID="txtOldCard" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                            <td style="width:10%">
                                <asp:Button ID="Button2" runat="server" CssClass="buttonQuery" OnClick="btnQuery_Click"
                                    TabIndex="1" Text="<%$ Resources:BaseInfo,User_lblQuery %>" /></td>
                            <td style="width:40%">
                                <asp:RadioButtonList ID="rBtnStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="L">丢失</asp:ListItem>
                                    <asp:ListItem Value="D">损坏</asp:ListItem>
                                </asp:RadioButtonList></td>
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:right; padding-right:5px">
                                <asp:Label ID="Label2" runat="server" Text="发行日期"></asp:Label>
                            </td>
                            <td style="width:30%">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width:10%; text-align: right; padding-right:5px">
                                <asp:Label ID="Label4" runat="server" Text="卡类型"></asp:Label></td>
                            <td style="width:40%">
                                <asp:TextBox ID="txtCardType" runat="server" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width:20%; text-align:right; height: 16px; padding-right:5px">
                                <asp:Label ID="Label3" runat="server" Text="期满日期"></asp:Label>
                            </td>
                            <td style="width:30%; height: 16px;">
                                <asp:TextBox ID="txtPassDate" runat="server" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width:10%; height: 16px; text-align: right; padding-right:5px">
                                <asp:Label ID="Label5" runat="server" Text="卡状态"></asp:Label></td>
                            <td style="width:40%; height: 16px;">
                                <asp:TextBox ID="txtCardStatus" runat="server" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right;padding-right:5px">
                                <asp:Label ID="Label6" runat="server" Text="会员号"></asp:Label></td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtMembID" runat="server" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width: 10%; text-align: right;padding-right:5px">
                                <asp:Label ID="Label8" runat="server" Text="卡级别"></asp:Label></td>
                            <td style="width: 40%">
                                <asp:TextBox ID="txtCardClass" runat="server" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right;padding-right:5px">
                                <asp:Label ID="Label7" runat="server" Text="会员名"></asp:Label></td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtMembName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                            <td style="width: 10%; text-align: right;padding-right:5px">
                                <asp:Label ID="Label9" runat="server" Text="移动电话"></asp:Label></td>
                            <td style="width: 40%">
                                <asp:TextBox ID="txtTel" runat="server" CssClass="ipt160px" BackColor="#F5F5F4"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="width: 20%; height: 15px; text-align: right">
                            </td>
                            <td style="width: 30%; height: 15px">
                            </td>
                            <td style="width: 10%; height: 15px">
                            </td>
                            <td style="width: 40%; height: 15px">
                            </td>
                        </tr>
                        <tr class="tdBackColor">
                         <td colspan="4" style="padding-left:10px; padding-right:10px">
                             <table border="0" cellpadding="0" cellspacing="0" style="width: 117%">
                                 <tr>
                                     <td style="height: 1px; background-color: #738495">
                                     </td>
                                 </tr>
                                 <tr>
                                     <td style="height: 1px; background-color: #ffffff">
                                     </td>
                                 </tr>
                             </table>
                         </td>
                       </tr>
                       <tr style="height:15px">
                            <td colspan="4" style="width: 20%; text-align: right">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right;padding-right:5px">
                                <asp:Label ID="Label12" runat="server" Text="新卡号"></asp:Label></td>
                            <td style="width: 30%">
                                <asp:TextBox ID="txtCardID" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                            <td style="width: 10%; text-align: right;padding-right:5px">
                                <asp:Label ID="Label13" runat="server" Text="发卡日期"></asp:Label></td>
                            <td style="width: 40%">
                                <asp:TextBox ID="txtDate" runat="server" CssClass="ipt160px"　onclick="calendar()"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: right;padding-right:5px" class="style5">
                                </td>
                            <td class="style6">
                                </td>
                            <td style="text-align: right;padding-right:5px" class="style7">
                                </td>
                            <td class="style8">
                                </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right">
                                </td>
                            <td style="width: 30%">
                                </td>
                            <td style="width: 10%">
                                </td>
                            <td style="width: 40%">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="BtnSave" runat="server" CssClass="buttonSave" OnClick="btnSubmit_Click"
                                    Text="<%$ Resources:BaseInfo,User_btnOk %>" /></td>
                        </tr>
                        <tr>
                            <td class="style1" style="text-align: right">
                            </td>
                            <td class="style2">
                            </td>
                            <td class="style3">
                            </td>
                            <td class="style4">
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 20%; text-align: right">
                                &nbsp;</td>
                            <td style="width: 30%">
                                &nbsp;</td>
                            <td style="width: 10%">
                                &nbsp;</td>
                            <td style="width: 40%">
                                &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class="tdBackColor">
                <td style="height: 21px;" colspan="3">
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

