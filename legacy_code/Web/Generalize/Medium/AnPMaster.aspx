<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AnPMaster.aspx.cs" Inherits="Generalize_Medium_AnPMaster" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
        
        table.tblBase tr.rowHeight{ height:28px;}
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        
        td.baseLable{ padding-right:10px;text-align:right; width:136px}
        td.baseInput{ align:left;padding-right:20px }
        -->
      </style>
      <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
      <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
      <script type="text/javascript" src="../../JavaScript/Common.js"></script>
      <script type="text/javascript">
      <!--	    
	     function Load()
        {
            addTabTool("<%=baseInfo %>,Generalize/Medium/AnPMaster.aspx");
            loadTitle();
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        
        //输入验证
        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtAnPNm.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtAnPNm.focus();
                return false;
            }
             if(isEmpty(document.all.txtStartDay.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtStartDay.focus();
                return false;
            }            
             if(isEmpty(document.all.txtEndDay.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtEndDay.focus();
                return false;
            }            
             if(isEmpty(document.all.txtSales.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtSales.focus();
                return false;
            }             
             if(isEmpty(document.all.txtTargetPeopletime.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtTargetPeopletime.focus()
                return false;
            }            
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
	    -->
      </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 27px; width: 100%;">
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="height:27px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left"  />
                                    </td>
                                <td class="tdTopRightBackColor" style=" height: 27px; text-align:left;">
                                    <asp:Label
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Master_lblAnPRecord %>' Height="12pt" Width="218px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style=" height: 27px; text-align: right;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                            <tr class="headLine">
                            <td colspan="3"></td>
                            </tr>
                             <tr style="height: 1px">
                                 <td colspan="3" class="tdBackColor" style="text-align: left">
                                     <table style="width: 100%" >
                                         <tr style="height:10px">
                                         <td style="width: 3px"></td>
                                             <td style="width: 100px; height: 10px">
                                             </td>
                                             <td colspan="2" style="height: 10px">
                                             </td>
                                         </tr>
                                         <tr>
                                             <td rowspan="7" style="width: 3px; vertical-align: top; text-align: center;">
                                                 <asp:GridView ID="gvAnPMaster" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                     BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="341px" OnSelectedIndexChanged="gvAnPMaster_SelectedIndexChanged"
                                                     PageSize="12" Width="230px">
                                                     <Columns>
                                                         <asp:BoundField DataField="AnpID">
                                                             <FooterStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <ItemStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="AnPNm" HeaderText="<%$ Resources:BaseInfo,Master_lblAnPNm %>">
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="ThemeNm" HeaderText="<%$ Resources:BaseInfo,Master_lblAnPTheme %>">
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:BoundField>
                                                         <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:CommandField>
                                                     </Columns>
                                                 </asp:GridView>
                               
                                             </td>
                                             <td class="baseLable" style="height: 24px; width: 100px;">
                                                 <asp:Label ID="lblAnPNm" runat="server" Text="<%$ Resources:BaseInfo,Master_lblAnPNm %>" CssClass="labelStyle" Width="92px"></asp:Label></td>
                                             <td class="baseInput" colspan="2" style="height: 24px">
                                                 <asp:TextBox ID="txtAnPNm" runat="server" CssClass="ipt160px" MaxLength="32"
                                                     Width="240px"></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable" style="width: 100px">
                                                 <asp:Label ID="lblAnPTheme" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Master_lblAnPTheme %>"></asp:Label></td>
                                             <td class="baseInput" colspan="2">
                                                 <asp:DropDownList ID="cmbThemeName" runat="server" Width="245px">
                                                 </asp:DropDownList></td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable" style="height: 30px; width: 100px;">
                                                 <asp:Label ID="lblStart" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Master_lblStart %>"></asp:Label></td>
                                             <td style="height: 30px; width: 80px;">
                                                 <asp:TextBox ID="txtStartDay" runat="server" CssClass="ipt160px" MaxLength="32" Width="80px" onclick="calendar()"></asp:TextBox></td>
                                             <td class="baseLable" style="height: 30px; text-align: left;">
                                                 <table>
                                                     <tr>
                                                         <td style="width: 44px; height: 24px">
                                                             <asp:DropDownList ID="cmbStartHour" runat="server" Width="41px">
                                                             </asp:DropDownList></td>
                                                         <td style="width: 3px; height: 24px">
                                                             :</td>
                                                         <td style="width: 3px; height: 24px">
                                                             <asp:DropDownList ID="cmbStartMinute" runat="server" Width="41px">
                                                             </asp:DropDownList></td>
                                                         <td style="width: 3px; height: 24px">
                                                             :</td>
                                                         <td style="width: 3px; height: 24px">
                                                             <asp:DropDownList ID="cmbStartSecond" runat="server" Width="41px">
                                                             </asp:DropDownList></td>
                                                     </tr>
                                                 </table>
                                                 </td>
                                         </tr>
                                         <tr>
                                             <td class="baseLable" style="width: 100px">
                                                 <asp:Label ID="lblEnd" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Master_lblEnd %>"></asp:Label></td>
                                             <td style="width: 80px">
                                                 <asp:TextBox ID="txtEndDay" runat="server" CssClass="ipt160px" MaxLength="32" Width="80px" onclick="calendar()"></asp:TextBox></td>
                                             <td class="baseLable" style="text-align: left;"><table>
                                                 <tr>
                                                     <td style="width: 44px; height: 24px">
                                                         <asp:DropDownList ID="cmbEndHour" runat="server" Width="41px">
                                                         </asp:DropDownList></td>
                                                     <td style="width: 3px; height: 24px">
                                                         :</td>
                                                     <td style="width: 3px; height: 24px">
                                                         <asp:DropDownList ID="cmbEndMinute" runat="server" Width="41px">
                                                         </asp:DropDownList></td>
                                                     <td style="width: 3px; height: 24px">
                                                         :</td>
                                                     <td style="width: 3px; height: 24px">
                                                         <asp:DropDownList ID="cmbEndSecond" runat="server" Width="41px">
                                                         </asp:DropDownList></td>
                                                 </tr>
                                             </table>
                                                 </td>
                                         </tr>
                                        <tr>
                                             <td class="baseLable" style="width: 100px">
                                                 <asp:Label ID="lblSales" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Master_lblSales %>"></asp:Label></td>
                                            <td class="baseInput" colspan="2">
                                                 <asp:TextBox ID="txtSales" runat="server" CssClass="ipt160px" MaxLength="32" Width="240px" ></asp:TextBox></td>
                                         </tr>
                                        <tr>
                                             <td class="baseLable" style="width: 100px">
                                                 <asp:Label ID="lblTargetPeopletime" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Master_lblTargetPeopletime %>"></asp:Label></td>
                                            <td class="baseInput" colspan="2">
                                                 <asp:TextBox ID="txtTargetPeopletime" runat="server" CssClass="ipt160px" MaxLength="32" Width="240px" ></asp:TextBox></td>
                                         </tr>
                                        <tr>
                                             <td class="baseLable" style="width: 100px">
                                                 <asp:Label ID="lblNote" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaSize_lblNote %>"></asp:Label></td>
                                            <td class="baseInput" colspan="2">
                                                 <asp:TextBox ID="txtRemark" runat="server" CssClass="ipt160px" MaxLength="32" Width="240px" Height="120px" TextMode="MultiLine" ></asp:TextBox></td>
                                         </tr>
                                         <tr>
                                             <td colspan="6">
                                                 <table border="0" cellpadding="0" cellspacing="0" style="left: 0px; width: 100%;
                                                     position: relative; top: 0px">
                                                     <tbody>
                                                         <tr>
                                                             <td style="width: 160px; height: 1px; background-color: #738495">
                                                             </td>
                                                         </tr>
                                                         <tr>
                                                             <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                             </td>
                                                         </tr>
                                                     </tbody>
                                                 </table>
                                             </td>
                                         </tr>
                                         <tr>
                                         <td style="width: 3px">
                                             &nbsp;
                                         </td>
                                             <td colspan="4" style="text-align: right">
                                                 &nbsp;
                                             </td>
                                             <td style="width: 101px">
                                                 &nbsp;
                                             </td>
                                         </tr>
                                         <tr>
                                         <td style="width: 3px; vertical-align: middle; text-align: center;">
                                         <table>
                                         <tr>
                                         <td>
                                             <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False"
                                                     Height="31px" OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>"
                                                     Width="71px" /></td>
                                         <td>
                                                 <asp:Button ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" Height="30px"
                                                     OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Width="73px" /></td>
                                         </tr>
                                         </table>
                                             </td>
                                             <td style="width: 100px">
                                                 <asp:Label ID="lblTotalNum" runat="server" Height="1px" Width="32px"></asp:Label><asp:Label
                                                     ID="lblCurrent" runat="server" ForeColor="Red" Height="1px" Width="1px">1</asp:Label></td>
                                             <td style="width: 80px">
                                                 </td>
                                             <td style="text-align: right" colspan="2">
                                                 <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                                                     Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="70px" /><asp:Button ID="btnEdit"
                                                         runat="server" CssClass="buttonEdit" Enabled="False" Height="30px" OnClick="btnEdit_Click"
                                                         Text="<%$ Resources:BaseInfo,User_btnChang %>" Width="70px" /><asp:Button ID="btnCel"
                                                             runat="server" CssClass="buttonClear" OnClick="btnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
                                                <td style="width: 101px"></td>
                                         </tr>
                                         <tr style="height:10px">
                                         <td style="width: 3px"></td>
                                             <td style="height: 10px; width: 100px;">
                                             </td>
                                             <td style="width: 80px; height: 10px;">
                                             </td>
                                             <td colspan="2" style="text-align: right; height: 10px;">
                                             </td>
                                             <td style="width: 101px; height: 10px;"></td>
                                         </tr>
                                     </table>
                                 </td>
                             </tr>
                        </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
