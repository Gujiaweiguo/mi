<!--
/// 编写人:何思键  English Name:Bruce
/// 编写时间:2009年3月31日
-->
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptStoreInvDetail.aspx.cs" Inherits="Rpt_StoreInvDetail" ResponseEncoding="utf-16"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=pageTitle%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/setPeriod.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>    
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptInv/RptStoreInvDetail.aspx");
	        loadTitle();
	    }
	    function FormulaValidator()
        {
            if(isEmpty(document.all.txtPeriod.value))  
            {
                parent.document.all.txtWroMessage.value='记账月不能为空';
                document.all.txtPeriod.focus();
                return false;					
            }
        }
	</script>
</head>
<body style="margin:0px" onload="Load();">
    <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1200"></asp:ScriptManager>
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                   <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width:5px" class="tdTopRightBackColor">
                             <img class="imageLeftBack" />
                            </td>
                            <td class="tdTopRightBackColor" style="text-align:left;">
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_StoreInvDetail %>"></asp:Label>
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
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>">
                                  </asp:Label>
                            </td>
                            <td style="width: 218px">
                                <asp:DropDownList ID="txtBizItem" runat="server" Height="20px" width="160px">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 44px">
                                <asp:Label ID="Label13" runat="server" CssClass="labelStyle" 
                                    Text="<%$ Resources:BaseInfo,RPT_lblRptType %>" Width="73px"></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButton ID="RadioButton4" runat="server" CssClass="labelStyle" 
                                    GroupName="b" Text="<%$ Resources:BaseInfo,RPT_lblABA %>" />
                            </td>
                        </tr>
                        <tr class="bodyTbl">
                             <td style="width: 89px; " class="lable" >
                                  <asp:Label ID="Label3" runat="server" CssClass="labelStyle" 
                                      Text="<%$ Resources:BaseInfo,Account_lblAccountMon %>">
                                  </asp:Label>
                             </td>
                             <td style="width: 218px" >                                                  
                                  <asp:TextBox ID="txtPeriod" runat="server" CssClass="ipt160px" 
                                       onClick="calendar();"></asp:TextBox>
                             </td>
                             <td style="width: 44px"  >
                             </td>
                             <td >
                                 <asp:RadioButton ID="RadioButton5" runat="server" CssClass="labelStyle" 
                                     GroupName="b" Text="<%$ Resources:BaseInfo,RPT_lblCBA %>" />
                             </td>
                        </tr>
                        <tr class="bodyTbl">
                             <td style="width: 89px;" class="lable" >
                                  &nbsp;</td>
                             <td style="width: 218px"  >                                                  &nbsp;</td>
                             <td style="width: 44px">
                             </td>
                             <td >
                             </td>
                        </tr>
                        <tr style="height:10px">
                             <td style="width: 89px; ">
                             </td>
                             <td style="width: 218px; ">
                             </td>
                             <td style="width: 44px; ">
                             </td>
                             <td >
                             </td>
                        </tr>
                        <tr class="bodyTb1">
                             <td style="width: 89px; " class="lable">
                             </td>
                             <td style="width: 218px" >
                                <asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click" Text="<%$ Resources:BaseInfo,User_lblQuery %> " onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel" OnClick="BtnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                             </td>
                             <td style="width: 44px">
                             </td>
                             <td >
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
