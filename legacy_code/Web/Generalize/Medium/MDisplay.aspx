<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MDisplay.aspx.cs" Inherits="Generalize_Medium_MDisplay" %>

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
            if(isEmpty(document.all.txtDisplayNm.value))
            {
                parent.document.all.txtWroMessage.value =('<%= errorMes %>');
                document.all.txtDisplayNm.focus();
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
                                        ID="Label1" runat="server" Text='<%$ Resources:BaseInfo,MDisplay_lblMDisplay %>' Height="12pt" Width="218px"></asp:Label></td>
                              
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
                                             <td valign="top" width="29%">
                                                 <table align="center" border="0" cellpadding="0" cellspacing="0" width="95%">
                                                     <tr>
                                                         <td height="25">
                                                             &nbsp;</td>
                                                     </tr>
                                                     <tr>
                                                         <td style="text-align: center">
                                                 <asp:GridView ID="gvMDisplay" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                     BorderStyle="Inset" BorderWidth="1px" CellPadding="3" PageSize="11" Width="240px" OnSelectedIndexChanged="gvMDisplay_SelectedIndexChanged">
                                                     <Columns>
                                                         <asp:BoundField DataField="MDisplayID">
                                                             <FooterStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <ItemStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="DisplayNm" HeaderText="<%$ Resources:BaseInfo,MDisplay_lblDisplayNm %>">
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                             <ItemStyle BorderColor="#E1E0B2" />
                                                         </asp:BoundField>
                                                         <asp:BoundField DataField="Intention" HeaderText="<%$ Resources:BaseInfo,MDisplay_lblintention %>">
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
                                             <td style="vertical-align: top; text-align: center; width: 71%; height: 440px;" valign="top">
                                                 <table border="0" cellpadding="0" cellspacing="0" width="90%">
                                                     <tr>
                                                         <td style="width: 96px">
                                                             &nbsp;</td>
                                                         <td height="25" style="width: 172px">
                                                             &nbsp;</td>
                                                     </tr>
                                                     <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="lblDisplayNm" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MDisplay_lblDisplayNm %>" Width="75px"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left;">
                                                                         <asp:TextBox ID="txtDisplayNm" runat="server" CssClass="ipt160px" MaxLength="32"
                                                                              Width="170px"></asp:TextBox></td>
                                                     </tr>
                                                     <tr>
                                                         <td align="right" style="width: 96px">
                                                             <asp:Label ID="Label27" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuilding %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:DropDownList ID="DDownListBuilding" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDownListBuilding_SelectedIndexChanged"
                                                                 Width="175px">
                                                             </asp:DropDownList></td>
                                                     </tr>
                                                     <tr>
                                                         <td align="right" style="width: 96px">
                                                             <asp:Label ID="Label28" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:DropDownList ID="DDownListFloors" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DDownListFloors_SelectedIndexChanged"
                                                                 Width="175px">
                                                             </asp:DropDownList></td>
                                                     </tr>
                                                      <tr>
                                                         <td align="right" style="width: 96px">
                                                             <asp:Label ID="Label29" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationName %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:DropDownList ID="DDownListLocation" runat="server"
                                                                 Width="175px">
                                                             </asp:DropDownList></td>
                                                     </tr>
                                                      <tr>
                                                         <td align="right" style="width: 96px">
                                                             <asp:Label ID="Label57" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaName %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:DropDownList ID="DDownListAreaName" runat="server" Width="175px">
                                                             </asp:DropDownList></td>
                                                     </tr>
                                                      <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="lblLocationDesc" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MDisplay_lblLocationDesc %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:TextBox ID="txtLocationDesc" runat="server" CssClass="ipt160px" Height="40px" MaxLength="32"
                                                                 Width="170px"></asp:TextBox></td>
                                                     </tr>
                                                      <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="lblintention" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MDisplay_lblintention %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:TextBox ID="txtintention" runat="server" CssClass="ipt160px" MaxLength="32" Width="170px"></asp:TextBox></td>
                                                     </tr>
                                                      <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Radio_lblEstCosts %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:TextBox ID="txtEstCosts" runat="server" CssClass="ipt160px" MaxLength="32" Width="170px"></asp:TextBox></td>
                                                     </tr>
                                                      <tr>
                                                         <td align="right" style="width: 96px">
                                                             <asp:Label ID="lblSDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_SDate %>"
                                                                 Width="51px"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" MaxLength="32"
                                                                 onclick="calendar()" Width="170px"></asp:TextBox></td>
                                                     </tr>
                                                      <tr>
                                                         <td align="right" style="width: 96px">
                                                             <asp:Label ID="lblEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LblDate_EndDate %>"
                                                                 Width="57px"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" MaxLength="32" onclick="calendar()"
                                                                 Width="170px"></asp:TextBox></td>
                                                     </tr>
                                                      <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="lblMcompany" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MDisplay_lblMcompany %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:TextBox ID="txtMcompany" runat="server" CssClass="ipt160px" MaxLength="32" Width="170px"></asp:TextBox></td>
                                                     </tr>
                                                     
                                                     <tr>
                                                         <td align="right" style="width: 96px">
                                                                         <asp:Label ID="lblDisplayDesc" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,MDisplay_lblDisplayDesc %>"></asp:Label>&nbsp;</td>
                                                         <td height="25" style="width: 172px; text-align: left">
                                                             <asp:TextBox ID="txtDisplayDesc" runat="server" CssClass="ipt160px" MaxLength="32" Width="170px" Height="40px"></asp:TextBox></td>
                                                     </tr>
                                                     <tr>
                                                         <td colspan="2" rowspan="2" style="text-align: left; height: 5px;">
                                                             </td>
                                                     </tr>
                                                     <tr>
                                                     </tr>
                                                     <tr>
                                                         <td colspan="2" style="vertical-align: top; height: 10px">
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
                                                 </table>
                                                 <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                                                     Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="70px" /><asp:Button ID="btnEdit"
                                                         runat="server" CssClass="buttonEdit" Enabled="False" Height="30px" OnClick="btnEdit_Click"
                                                         Text="<%$ Resources:BaseInfo,User_btnChang %>" Width="70px" /><asp:Button ID="btnCel"
                                                             runat="server" CssClass="buttonClear" OnClick="btnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" /></td>
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
