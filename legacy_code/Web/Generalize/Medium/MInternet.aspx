<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MInternet.aspx.cs" Inherits="Generalize_Medium_MInternet" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Master_lblAnPRecord")%></title>
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
            loadTitle();
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        
        //输入验证
        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtEstCosts.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtEstCosts.focus();
                return false;
            }
             if(isEmpty(document.all.txtStartDate.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtStartDate.focus();
                return false;
            }            
             if(isEmpty(document.all.txtEndDate.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtEndDate.focus();
                return false;
            }            
            
            if(document.getElementById("rdoDaily").checked == true)
            {
                if(isEmpty(document.all.txtDaily.value))
                {
                    parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                    document.all.txtDaily.focus();
                    return false;
                }
            }
            
            if(document.getElementById("rdoWeekly").checked == true )
            {
                var i = 0;
                if(document.getElementById("chkMon").checked == true )
                {
                    i = 1;
                }
                if(document.getElementById("chkTue").checked == true )
                {
                    i = 1;
                }
                if(document.getElementById("chkWed").checked == true )
                {
                    i = 1;
                }
                if(document.getElementById("chkThu").checked == true )
                {
                    i = 1;
                }
                if(document.getElementById("chkFri").checked == true )
                {
                    i = 1;
                }
                if(document.getElementById("chkSat").checked == true )
                {
                    i = 1;
                }
                if(document.getElementById("chkSun").checked == true )
                {
                    i = 1;
                }
                if(i == 0)
                {
                    parent.document.all.txtWroMessage.value =('<%= onlySelected %>');
                    return false;
                }
            }
            
            if(document.getElementById("rdoMthly").checked == true )
            {             
                if(isEmpty(document.all.txtMthlyFr.value))
                {
                    parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                    document.all.txtMthlyFr.focus()
                    return false;
                }
                if(isEmpty(document.all.txtMthlyTo.value))
                {
                    parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                    document.all.txtMthlyTo.focus()
                    return false;
                }
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,Internet_lblInternet %>' Height="12pt" Width="218px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style=" height: 27px; text-align: right; width: 22px;">
                                    </td>
                            </tr>
                            <tr class="headLine">
                            <td colspan="3"></td>
                            </tr>
                             <tr style="height: 1px">
                                 <td colspan="3" class="tdBackColor" style="text-align: left">
                                     <table align="center" border="0" cellpadding="0" cellspacing="0" width="90%">
                                         <tr>
                                             <td height="326" valign="top" width="29%">
                                                 <table align="center" border="0" cellpadding="0" cellspacing="0" width="95%">
                                                     <tr>
                                                         <td height="25">
                                                             &nbsp;</td>
                                                     </tr>
                                                     <tr>
                                                         <td style="text-align: center">
                                                 <asp:GridView ID="gvMInternet" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                     BorderStyle="Inset" BorderWidth="1px" CellPadding="3" PageSize="11" Width="240px" OnSelectedIndexChanged="gvMInternet_SelectedIndexChanged">
                                                     <Columns>
                                                         <asp:BoundField DataField="MInternetID">
                                                             <FooterStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <ItemStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="InternetNm" HeaderText="<%$ Resources:BaseInfo,Internet_lblInternetNm %>">
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="ContentsNm" HeaderText="<%$ Resources:BaseInfo,Radio_lblContentsNm %>">
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="EstCosts" HeaderText="<%$ Resources:BaseInfo,Radio_lblEstCosts %>">
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
                                                     </tr>
                                                     <tr>
                                                     <td style="height: 20px" colspan="3">
                                                     </td>
                                                     </tr>
                                                     <tr>
                                                         <td>
                                                             <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                 <tr>
                                                                     <td align="center">
                                                                         <asp:Label
                                                             ID="lblTotalNum" runat="server" Height="1px" Width="32px"></asp:Label><asp:Label
                                                                 ID="lblCurrent" runat="server" ForeColor="Red" Height="1px" Width="1px">1</asp:Label></td>
                                                                     <td align="center">
                                                 <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" Height="31px"
                                                     OnClick="btnBack_Click" Text="<%$ Resources:BaseInfo,Button_back %>" Width="71px" /></td>
                                                                     <td align="center">
                                                                         <asp:Button
                                                         ID="btnNext" runat="server" CssClass="buttonNext" Enabled="False" Height="30px"
                                                         OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" Width="73px" /></td>
                                                                 </tr>
                                                             </table>
                                                         </td>
                                                     </tr>
                                                 </table>
                                             </td>
                                             <td style="vertical-align: top; text-align: center; width: 71%;" valign="top">
                                                 <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                                     <tr>
                                                         <td style="width: 96px">
                                                             &nbsp;</td>
                                                         <td height="25" style="width: 172px">
                                                             &nbsp;</td>
                                                     </tr>
                                                     <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="lblPrintsNm" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Internet_lblInternetNm %>" Width="74px"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px">
                                                            
                                                            <asp:DropDownList ID="cmbInternet" runat="server" Width="200px">
                                                                         </asp:DropDownList>
                                                         </td>
                                                     </tr>
                                                     <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="lblContentsNm" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Radio_lblContentsNm %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                                         <asp:DropDownList ID="cmbContentsNm" runat="server" Width="200px">
                                                                         </asp:DropDownList></td>
                                                     </tr>
                                                     <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="lblEstCosts" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Radio_lblEstCosts %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                                         <asp:TextBox ID="txtEstCosts" runat="server" CssClass="ipt160px" MaxLength="32"
                                                                              Width="195px"></asp:TextBox></td>
                                                     </tr>
                                                     <tr>
                                                         <td colspan="2" rowspan="2" style="text-align: left">
                                                             &nbsp;&nbsp;</td>
                                                     </tr>
                                                     <tr>
                                                     </tr>
                                                     <tr>
                                                         <td colspan="2" style="vertical-align: bottom; height: 25px">
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
                                                         <td colspan="2">
                                                             <table border="0" cellpadding="0" cellspacing="0" style="width: 345px">
                                                                 <tr>
                                                                     <td colspan="2" height="25" style="text-align: left">
                                                                         &nbsp;<table border="0" cellpadding="0" cellspacing="0" style="width: 286px">
                                                                             <tr>
                                                                                 <td align="right" width="24%">
                                                                                     <asp:Label ID="lblSDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_SDate %>" Width="51px"></asp:Label></td>
                                                                                 <td align="center" width="21%">
                                                                                     &nbsp;<asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" MaxLength="32"
                                                                                         onclick="calendar()" Width="80px"></asp:TextBox></td>
                                                                                 <td align="center" style="width: 21%">
                                                                                     <asp:Label ID="lblEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LblDate_EndDate %>" Width="57px"></asp:Label></td>
                                                                                 <td align="right" width="22%">
                                                                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" MaxLength="32"
                                                                                         onclick="calendar()" Width="80px"></asp:TextBox>&nbsp;</td>
                                                                             </tr>
                                                                         </table>
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td colspan="2" height="25" style="height: 10px; text-align: left">
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td width="100" style="text-align: right">
                                                                         &nbsp;<asp:RadioButton ID="rdoDaily" runat="server" Text="<%$ Resources:BaseInfo,Internet_optDaily %>" Checked="True" GroupName="every" OnCheckedChanged="rdoDaily_CheckedChanged" AutoPostBack="True" Width="61px" /></td>
                                                                     <td height="25" style="text-align: left">
                                                                      <table>
                                                                         <tr>
                                                                         <td style="width: 3px">
                                                                             <asp:TextBox ID="txtDaily" runat="server" CssClass="ipt160px" MaxLength="32" 
                                                                                 Width="80px"></asp:TextBox></td>
                                                                         <td style="width: 3px">
                                                                             <asp:Label ID="lblDay" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblEfficiencyDay %>"></asp:Label></td>
                                                                         </tr>
                                                                         </table>
                                                                        
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td width="100" style="text-align: right">
                                                                         <asp:RadioButton ID="rdoWeekly" runat="server" Text="<%$ Resources:BaseInfo,Internet_optWeekly %>" GroupName="every" AutoPostBack="True" OnCheckedChanged="rdoWeekly_CheckedChanged" Width="58px" /></td>
                                                                     <td height="25" style="text-align: left">
                                                                         
                                                                     <table>
                                                                         <tr>
                                                                             <td style="height: 19px">
                                                                                      <asp:CheckBox ID="chkMon" runat="server" Text="<%$ Resources:BaseInfo,Prints_chkMon %>" Enabled="False" Width="60px" /></td>
                                                                             <td style="width: 6px; height: 19px">
                                                                             </td>
                                                                             <td style="height: 19px">
                                                                                      <asp:CheckBox ID="chkTue" runat="server" Text="<%$ Resources:BaseInfo,Prints_chkTue %>" Enabled="False" Width="60px" /></td>
                                                                         <td style="width: 6px; height: 19px">
                                                                         </td>
                                                                         <td style="width: 3px; height: 19px">
                                                                                      <asp:CheckBox ID="chkWed" runat="server" Text="<%$ Resources:BaseInfo,Prints_chkWed %>" Enabled="False" Width="60px" /></td>
                                                                         <td style="width: 7px; height: 19px">
                                                                         </td>
                                                                         <td style="width: 5px; height: 19px">
                                                                                      <asp:CheckBox ID="chkThu" runat="server" Text="<%$ Resources:BaseInfo,Prints_chkThu %>" Enabled="False" Width="60px" /></td>
                                                                         </tr>
                                                                         </table>
                                                                         
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td width="100">
                                                                         &nbsp;</td>
                                                                     <td height="25" style="text-align: left">
                                                                         <table>
                                                                         <tr>
                                                                         <td style="width: 2px; height: 19px">
                                                                                      <asp:CheckBox ID="chkFri" runat="server" Text="<%$ Resources:BaseInfo,Prints_chkFri %>" Enabled="False" Width="60px" /></td>
                                                                             <td style="width: 6px; height: 19px">
                                                                             </td>
                                                                             <td style="width: 3px; height: 19px">
                                                                                      <asp:CheckBox ID="chkSat" runat="server" Text="<%$ Resources:BaseInfo,Prints_chkSat %>" Enabled="False" Width="60px" /></td>
                                                                             <td style="width: 6px; height: 19px">
                                                                             </td>
                                                                             <td colspan="3" style="height: 19px">
                                                                                      <asp:CheckBox ID="chkSun" runat="server" Text="<%$ Resources:BaseInfo,Prints_chkSun %>" Enabled="False" Width="60px" /></td>
                                                                         </tr>
                                                                         </table>
                                                                         
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td width="100" style="text-align: right">
                                                                         &nbsp;<asp:RadioButton ID="rdoMthly" runat="server" Text="<%$ Resources:BaseInfo,Internet_optMthly %>" GroupName="every" OnCheckedChanged="rdoMthly_CheckedChanged" AutoPostBack="True" Width="46px" /></td>
                                                                     <td style="text-align: left">
                                                                                  <table>
                                                                                   <tr>
                                                                                  <td style="width: 3px">
                                                                                      <asp:TextBox ID="txtMthlyFr" runat="server" CssClass="ipt160px" MaxLength="32"
                                                                                          Width="29px" Enabled="False"></asp:TextBox></td>
                                                                                  <td style="width: 3px">
                                                                                      <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="-- " Width="21px"></asp:Label></td>
                                                                                 <td style="width: 3px">
                                                                                     <asp:TextBox ID="txtMthlyTo" runat="server" CssClass="ipt160px" MaxLength="32"
                                                                                         Width="29px" Enabled="False"></asp:TextBox></td> 
                                                                                  </tr>
                                                                                  
                                                                                  </table>
                                                                         
                                                                     </td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td width="100" style="height: 31px">
                                                                         &nbsp;</td>
                                                                     <td style="height: 31px">
                                                                         <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                                                                             Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="70px" /><asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Enabled="False" Height="30px"
                                                                             OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" Width="70px" /><asp:Button ID="btnCel" runat="server" CssClass="buttonClear" OnClick="btnCel_Click"
                                                                             Text="<%$ Resources:BaseInfo,User_btnCancel %>" />&nbsp;</td>
                                                                 </tr>
                                                                 <tr>
                                                                     <td width="100">
                                                                         </td>
                                                                     <td height="25">
                                                                         &nbsp;</td>
                                                                 </tr>
                                                             </table>
                                                         </td>
                                                     </tr>
                                                 </table>
                                             </td>
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
