<!--
/**
 * 
 * 编写人：何思键 ; English Name : bruce
 * 
 * 编写时间:2009年4月20日
 * 
 * 更新类型：Add,Modify(增加及修改)
 * 
 * 
 * **/
-->
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptMemberShopBonusList.aspx.cs" Inherits="Rpt_MemberShop_BonusList" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
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
        function Load(){
            addTabTool("<%=baseInfo %>,ReportM/RptMember/RptMemberShopBonusList.aspx");
            loadTitle();        
        }
        
        function InputValidator(sForm){
            var t1 =document.all.txtFirstNm.value;
            var t2 =document.all.txtSecondNm.value;
            if(isNaN(t1)|| isNaN(t2)){
                window.alert("请输入有效数据");
                this.focus();
                return false; 
            }
        }
     
     </script>
</head>
<body style="margin:0px" onload ="Load();">
    <form id="form3" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager3" runat="server">
    </asp:ScriptManager>
     <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
                                         <table style="width:102%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px; height: 28px;" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left; width: 234px; height: 28px;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Rpt_MemberShopBonusList %>"></asp:Label>
                                                    </td>
                                                    <td style="width:5px; height: 28px;" class="tdTopRightBackColor">
                                                        <img class="imageRightBack" style="width: 1px; height: 24px"/>
                                                    </td>
                                                </tr>
                                                <tr style="height:1px">
                                                    <td colspan="3" style="background-color:White; height:1px">
                                                    </td>
                                                </tr>
                                            </table>
                                             <table style="width:102%" class="tdBackColor">
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
                                                        <td style="width: 15%;height: 22px; text-align:right"><!--大楼-->
                                                            <asp:Label ID="label3" runat="server" Text="<%$ Resources:BaseInfo,Building %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:DropDownList ID="BuildingName" runat="server" width="78%" ></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" align="right">
                                                            <asp:Label ID="label4" runat="server" Text="<%$ Resources:BaseInfo,sort %>" CssClass="labelStyle" Font-size="10pt" ></asp:Label>
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoShopCode" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Rdo_ShopCode %>" Checked="true" CssClass="labelStyle"></asp:RadioButton>
                                                        </td>
                                                    </tr>
<!---->
                                                      <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                            <asp:Label ID="label5" runat="server" Text="<%$ Resources:BaseInfo,Floors %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:DropDownList ID="FloorName" runat="server" width="78%" ></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" >
                                                            
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoChangeNm" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Associator_MemberChangeNum2 %>"  CssClass="labelStyle"></asp:RadioButton>
                                                        </td>
                                                    </tr>
<!---->
                                                
                                                      <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                            <asp:Label ID="label6" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:DropDownList ID="BizStyle" runat="server" width="78%" ></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" >
                                                            
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoSaleNm" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Associator_MemberChangeSum %>"  CssClass="labelStyle"></asp:RadioButton>
                                                        </td>
                                                    </tr>
<!---->
                                                       <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                            <asp:Label ID="label7" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblAreaName %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:DropDownList ID="AreaName" runat="server" width="78%" ></asp:DropDownList>
                                                        </td>
                                                        <td style="width: 21%;height: 22px" >
                                                            
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoGetNm" runat="server" GroupName="b" Text="<%$ Resources:BaseInfo,Associator_MemberBonus %>"  CssClass="labelStyle"></asp:RadioButton>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 22px"></tr>
                                                    
                                                    
<!---->
                                                     <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                            <asp:Label ID="label8" runat="server" Text="<%$ Resources:BaseInfo,Associator_BonusArea %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:TextBox ID="txtFirstNm" runat="server" width="35%" CssClass="ipt160px" ></asp:TextBox>&nbsp;
                                                            <asp:TextBox ID="txtSecondNm" runat="server" width="35%" CssClass="ipt160px" ></asp:TextBox>
                                                           
                                                        </td>
                                                        <td style="width: 21%;height: 22px" align="right">
                                                            <asp:Label ID="label9" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblPayInDataSource %>" CssClass="labelStyle" Font-size="10pt" ></asp:Label>
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
                                                            <asp:Label ID="label10" runat="server" Text="<%$ Resources:BaseInfo,Rpt_SalesDate %>" CssClass="labelStyle" ></asp:Label>
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                            <asp:TextBox ID="txtFirstDate" runat="server" width="35%" OnClick="calendar();" CssClass="ipt160px"></asp:TextBox>&nbsp;
                                                            <asp:TextBox ID="txtSecondDate" runat="server" width="35%" OnClick="calendar();" CssClass="ipt160px"></asp:TextBox>
                                                           
                                                        </td>
                                                        <td style="width: 21%;height: 22px" align="right">
                                                        </td>
                                                        <td style="height: 22px; ">
                                                            <asp:RadioButton ID="txtRdoSalePOS" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,DataSource_POS %>"  CssClass="labelStyle" ></asp:RadioButton>
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
                                                            <asp:RadioButton ID="txtRdoImportSale" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,DataSource_Put %>"  CssClass="labelStyle" ></asp:RadioButton>
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
                                                            <asp:RadioButton ID="txtRdoInput" runat="server" GroupName="c" Text="<%$ Resources:BaseInfo,DataSource_Manual %>"  CssClass="labelStyle" ></asp:RadioButton>
                                                        </td>
                                                    </tr> 
                                                    <tr style="height: 22px"></tr>
                                                    <tr style="height: 22px"></tr>
                                                    <tr style="height: 22px"></tr>
<!---->
                                                     <tr class="bodyTb1">
                                                         <td style="width: 1%">
                                                        </td>
                                                        <td style="width: 15%;height: 22px; text-align:right">
                                                        </td>
                                                        <td style="width: 35%;height: 22px" align="left">
                                                        </td>
                                                        <td colspan="2" style="width: 21%;height: 22px" align="center">
                                                        <asp:Button ID="btnOK" runat="server" CssClass="buttonQuery" Text="<%$ Resources:BaseInfo,User_lblQuery %> " OnClick="btnOK_Click"  />&nbsp;&nbsp;&nbsp;<asp:Button ID="BtnCel" runat="server" CssClass="buttonCancel" Text="<%$ Resources:BaseInfo,User_btnCancel %> "  OnClick="btnCel_Click"/>
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
        </div>
    </form>
</body>
</html>
