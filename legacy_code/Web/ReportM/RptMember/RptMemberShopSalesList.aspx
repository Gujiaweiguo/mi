<!--
编写人：何思键  English Name : Bruce
编写时间：2009年3月25日
-->
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptMemberShopSalesList.aspx.cs" Inherits="Report_MemberShopSalesList" ResponseEncoding="GB2312" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=baseInfo %></title>
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
            addTabTool("<%=baseInfo %>,ReportM/RptMember/RptMemberShopSalesList.aspx");
            loadTitle(); 
        }
    
    </script>
</head>
<body style="margin:0px" onload="Load();">
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
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_MemberShopSalesList %>">
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
                                       <td style="width: 1%">
                                       </td>
                                       <td style="width: 15%">
                                       </td>
                                       <td style="width: 35%">
                                       </td>
                                       <td style="width: 21%">
                                       </td>
                                       <td>
                                       </td>
                                    </tr>
                                    <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                            <asp:Label ID="label3" runat="server" Text="<%$ Resources:ReportInfo,Rpt_BuildingName %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:DropDownList ID="txtBuilding" runat="server" width="78%"></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" align="right">
                                                            <asp:Label ID="label4" runat="server" Text="排序" CssClass="labelStyle" Font-size="10pt" ></asp:Label>
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoChangeNumber" runat="server" GroupName="b" Text="<%$ Resources:ReportInfo,RptSale_TrNum %>" Checked="true" CssClass="labelStyle"></asp:RadioButton>
                                                        </td>
                                                    </tr>
<!---->
                                                      <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                            <asp:Label ID="label5" runat="server" Text="<%$ Resources:ReportInfo,Rpt_FloorName %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:DropDownList ID="txtFloor" runat="server" width="78%"></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" >
                                                            
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoBizBonus" runat="server" GroupName="b" Text="<%$ Resources:ReportInfo,RptInv_CostAmt %>"  CssClass="labelStyle"></asp:RadioButton>
                                                        </td>
                                                    </tr>
<!---->
                                                
                                                      <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                            <asp:Label ID="label6" runat="server" Text="<%$ Resources:ReportInfo,RptSale_Trade2Name %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:DropDownList ID="txtBizStyle" runat="server" width="78%"></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" >
                                                            
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoMemberChangeNum" runat="server" GroupName="b" Text="会员交易笔数"  CssClass="labelStyle"></asp:RadioButton>
                                                        </td>
                                                    </tr>
<!---->
                                                       <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                            <asp:Label ID="label7" runat="server" Text="<%$ Resources:ReportInfo,Rpt_AreaName %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:DropDownList ID="txtArea" runat="server" width="78%"></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" >
                                                            
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoMemberBizBonus" runat="server" GroupName="b" Text="会员交易金额"  CssClass="labelStyle"></asp:RadioButton>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 22px"></tr>
                                                    
                                                    
<!---->
                                                     <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                             <asp:Label ID="label10" runat="server" Text="<%$ Resources:BaseInfo,Rpt_SalesDate %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">                                                     
                                                              <asp:TextBox ID="txtFirstDate" runat="server" width="37%" OnClick="calendar();"></asp:TextBox>
                                                            <asp:TextBox ID="txtSecondDate" runat="server" width="37%" OnClick="calendar();"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" align="right">
                                                            <asp:Label ID="label9" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>" CssClass="labelStyle" Font-size="10pt"></asp:Label>
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoGetAll" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,Rpt_rdoAll %>"  CssClass="labelStyle" Checked="true"></asp:RadioButton>
                                                        </td>
                                                    </tr> 
<!---->
                                                     
                                                     <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                           
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                           
                                                           
                                                        </td>
                                                        <td style="width: 21%;height: 22px" align="right">
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoSalePOS" runat="server" GroupName="c"   Text="<%$ Resources:BaseInfo,DataSource_POS %>"  CssClass="labelStyle" ></asp:RadioButton>
                                                        </td>
                                                    </tr> 
<!---->       
                                                     <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                        </td>
                                                        <td style="width: 21%;height: 22px" align="right">
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoImportSale" runat="server" GroupName="c"  Text="<%$ Resources:BaseInfo,DataSource_Put %>"  CssClass="labelStyle" ></asp:RadioButton>
                                                        </td>
                                                    </tr> 
<!---->                                             
                                                     <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                        </td>
                                                        <td style="width: 21%;height: 22px" align="right">
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoInput" runat="server" GroupName="c"  Text="<%$ Resources:BaseInfo,DataSource_Manual %>"  CssClass="labelStyle" ></asp:RadioButton>
                                                        </td>
                                                    </tr> 
                                                      <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
                                                    </td>
                                                    <td>
                                                    </td>
                                                   </tr>
                                                     <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
                                                    </td>
                                                    <td>
                                                    </td>
                                                   </tr>
                                                     <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
                                                    </td>
                                                    <td>
                                                    </td>
                                                   </tr>
                                                     <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
                                                    </td>
                                                    <td>
                                                    </td>
                                                   </tr>
                                                     <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
                                                    </td>
                                                    <td>
                                                    </td>
                                                   </tr>
                                                     <tr class="bodyTb1">
                                                        <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                        </td>
                                                        <td colspan="2" style="width: 21%;height: 22px" align="center">
                                                        <asp:Button ID="btnOK" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="BtnOK_Click"  />&nbsp;&nbsp;&nbsp;<asp:Button ID="BtnCel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> "  OnClick="BtnCel_Click"/>
                                                        </td>
                                                    </tr> 
                                                     <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
                                                    </td>
                                                    <td>
                                                    </td>
                                                   </tr>
                                                   <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
                                                    </td>
                                                    <td>
                                                    </td>
                                                   </tr>
                                                    <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
                                                    </td>
                                                    <td>
                                                    </td>
                                                   </tr>
                                                    <tr style="height:10px">
                                                    <td style="width: 1%">
                                                    </td>
                                                    <td style="width: 15%">
                                                    </td>
                                                    <td style="width: 35%">
                                                        
                                                    </td>
                                                    <td style="width: 21%">
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
