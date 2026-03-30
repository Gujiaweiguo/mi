
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptContractAlert.aspx.cs" Inherits="Report_ContractAlert"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=baseInfo %></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
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
            addTabTool("<%=fresh %>,ReportM/RptBase/RptContractAlert.aspx");
	        loadTitle();
        }
        
        
        function CheckAll(sForm)
        {
        
           var v1= document.all.txtLeaseStart.value;
           var v2= document.all.txtLeaseEnd.value;
           if(v1 == "" && v2 =="")
           {
                window.alert("请选择终止日期范围！");
                return false;
           }
           return true;
        }
        function BtnUp( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
}
function BtnOver( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
}
    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                               <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                   <tr>
                                       <td style="width:5px" class="tdTopRightBackColor">
                                            <img class="imageLeftBack" />
                                       </td>
                                       <td class="tdTopRightBackColor" style="text-align:left;">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo, Menu_ContractAlert %>">
                                            </asp:Label>
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
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
<!--合同号-->
                                   <tr class="bodyTbl">
                                       <td class="lable" style="width: 15%">
                                           <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                            </asp:Label></td>
                                       <td style="width: 40%">
                                           <asp:DropDownList ID="ddlBizproject" runat="server" AutoPostBack="True" Width="165px">
                                           </asp:DropDownList></td>
                                       <td style="width: 25%">
                                       </td>
                                       <td style="width: 20%">
                                       </td>
                                   </tr>
                                    <tr class="bodyTbl">
                                        <td style="width:15%" class="lable">
                                            <asp:Label ID="label9" runat="server" CssClass="labelStyle" 
                                                Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Label>
                                        </td>
                                        <td style="width:40%">
                                            <asp:TextBox ID="txtLeaseStart" runat="server" onClick="calendar()"></asp:TextBox>
                                            &nbsp; 到</td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--客户号-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            &nbsp;</td>
                                        <td style="width:40%">
                                            <asp:TextBox ID="txtLeaseEnd" runat="server"  onClick="calendar()"></asp:TextBox></td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--客户名称-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            &nbsp;</td>
                                        <td style="width:40%">
                                            &nbsp;</td>
                                        <td style="width:25%">
                                            &nbsp;</td>
                                        <td style="width:20%">
                                            &nbsp;</td>
                                    </tr>
                                    <tr class="bodyTb1">
                                        <td class="lable" style="width:15%">
                                            &nbsp;</td>
                                        <td style="width:40%">
                                            &nbsp;<asp:Button ID="BtnQuery" runat="server" CssClass="buttonQuery" 
                                                OnClick="BtnOK_Click" onmouseout="BtnUp(this.id);" 
                                                onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                            &nbsp;<asp:Button ID="BtnCancel" runat="server" CssClass="buttonCancel" 
                                                OnClick="BtnCel_Click" onmouseout="BtnUp(this.id);" 
                                                onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);" 
                                                Text="<%$ Resources:BaseInfo,User_btnCancel %> " />
                                        </td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--经营方式-->
<!--所有行业-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            <asp:Label ID="label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Dept_rbtnAllVocation %>" Visible="False"></asp:Label>&nbsp;</td>
                                        <td style="width:40%">
                                            &nbsp;</td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--经营类别-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            &nbsp;</td>
                                        <td style="width:40%">
                                            <asp:DropDownList ID="txtBizStyle" runat="server" width="161px" Visible="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--终约期限-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            <asp:Label ID="label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>" Visible="False"></asp:Label>
                                        </td>
                                        <td style="width:40%">
                                            <asp:DropDownList ID="txtLease" runat="server" width="161px" Visible="False">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
<!--终止日期-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" class="lable">
                                            &nbsp;</td>
                                        <td style="width:40%">
                                            &nbsp;
                                        </td>
                                        <td style="width:25%">
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
<!--按钮-->
                                    <tr class="bodyTb1">
                                        <td style="width:15%" >
                                        </td>
                                        <td style="width:40%" colspan="2" align="left">
                                            &nbsp;
                                        </td>
                                        <td style="width:20%">
                                        </td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%">
                                            <asp:Label ID="label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>" Visible="False"></asp:Label><asp:TextBox ID="txtContractID" runat="server" CssClass="ipt160px" MaxLength="32" Visible="False"></asp:TextBox><asp:Label ID="label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_CustCode %>" Visible="False"></asp:Label><asp:TextBox ID="txtCustomerID" runat="server" CssClass="ipt160px" MaxLength="16" Visible="False"></asp:TextBox><asp:Label ID="label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>" Visible="False"></asp:Label><asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" MaxLength="32" Visible="False"></asp:TextBox><asp:Label ID="label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_BizMode %>" Visible="False"></asp:Label><asp:DropDownList ID="txtBizType" runat="server" width="161px" Visible="False">
                                            </asp:DropDownList></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                     <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                                   <tr style="height:10px">
                                        <td style="width:15%"></td>
                                        <td style="width:40%"></td>
                                        <td style="width:25%"></td>
                                        <td style="width:20%"></td>
                                    </tr>
                               </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
