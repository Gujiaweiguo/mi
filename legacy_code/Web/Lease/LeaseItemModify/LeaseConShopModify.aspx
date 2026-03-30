<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaseConShopModify.aspx.cs" Inherits="Lease_LeaseItemModify_LeaseConShopModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopBaseInfo")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        
          <!--
            talbe.baseShop tr{ height:50px;}
            table.baseShop tr.headLine{ height:1px; }
            table.baseShop tr.bodyLine{ height:1px;}
        -->
       
        
    </style>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
     <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script type="text/javascript">
	    function Load()
	    {
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
	    
	    function selectShopBrand()
		{
			strreturnval=window.showModalDialog('../../Lease/SelectShopBrand.aspx','window','dialogWidth=350px;dialogHeight=380px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
			    var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');
                objImgBtn1.click();
            }
			else
				return;	
		}
	        
	     //输入验证
      function InputValidator(sForm)
        {
            
            if(document.getElementById("txtStartDate").value > document.getElementById("txtEndDate").value)
            {
                alert('终止日期大于开始日期!');
                return false;
            }
        }
	</script>
</head>
<body onload="Load()" style="margin:0px">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
       <table class="baseShop" border="0" cellpadding="0" cellspacing="0" style="height: 1px" width="100%">
       <tr>
                <td class="tdTopBackColor" style="width: 100%; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" src="" />
                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblShopBaseInfo %>" Width="344px"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 100%; height: 25px; text-align:right;" valign="top">
                    <img class="imageRightBack" alt="img"  src=""/></td>
            </tr>
            <tr class="headLine"> 
                <td class="tdBackColor" style="background-color:White;" colspan="4">
                </td>
             </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" style="width:100%; height:357px" >
            <tr >
            <td class="tdBackColor" valign="top" style="width:30%; height: 349px;">
                <table width="100%">
                    <tr style="height:25px">
                        <td colspan="3" style="text-align: center">
                            <table style="width:182px;" border="0" cellpadding="0" cellspacing="0">
                                <tr  class="bodyLine">
                                    <td style="height: 1px; background-color: #738495;" >
                                    </td>
                                </tr>
                                <tr  class="bodyLine">
                                    <td style="height: 1px; background-color: #ffffff;" >
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: center">
                            <asp:GridView ID="gvShop" runat="server" AutoGenerateColumns="False" BackColor="White"
                                Width="181px" OnSelectedIndexChanged="gvShop_SelectedIndexChanged" BorderStyle="Inset">
                                <Columns>
                                    <asp:BoundField DataField="ShopId">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopName" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopTypeName" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopType %>">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                    </asp:BoundField>
                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                        <ItemStyle BorderColor="#E1E0B2" />
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
          <td class="tdBackColor" valign="top" style="text-align: right; width:30%; height: 349px;">
                 <table class="tdBackColor" style="width: 238px" >
                       <tr style="height:15px">
                           <td style="width: 2239px" ></td>
                           <td valign="bottom" style="width: 329px">
                               <table style="width:136px;" border="0" cellpadding="0" cellspacing="0">
                                   <tr  class="bodyLine">
                                       <td style="height: 1px; background-color: #738495;" >
                                       </td>
                                   </tr>
                                   <tr  class="bodyLine">
                                       <td style="height: 1px; background-color: #ffffff;" >
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 2239px; height: 30px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>"></asp:Label></td>
                           <td style="width: 329px; height: 30px;" class="tdBackColor">
                               <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" Width="128px" MaxLength="16"></asp:TextBox></td>
                       </tr>
                       <tr><td style="TEXT-ALIGN: right; width: 2239px;" class="tdBackColor"><asp:Label id="Label50" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 21px" class="tdBackColor"><asp:TextBox id="txtShopName" runat="server" CssClass="ipt160px" Width="128px" MaxLength="64"></asp:TextBox></td></tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label53" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label></td>
                           <td style="" class="tdBackColor">
                               <asp:DropDownList ID="DDownListShopType" runat="server" Width="133px">
                               </asp:DropDownList></td>
                       </tr>
                     <tr>
                         <td class="tdBackColor" style="width: 2239px; text-align: right">
                            <asp:Label id="Label18" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>" CssClass="labelStyle"></asp:Label></td>
                         <td class="tdBackColor">
                            <asp:TextBox id="txtRentArea" runat="server" CssClass="ipt160px" Width="128px"></asp:TextBox></td>
                     </tr>
                       <tr><td style="TEXT-ALIGN: right; width: 2239px;" class="tdBackColor"><asp:Label id="Label57" runat="server" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaName %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 23px" class="tdBackColor">
                           <asp:DropDownList ID="DDownListAreaName" runat="server" Width="134px">
                           </asp:DropDownList></td></tr>
                       <tr><td style="TEXT-ALIGN: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label60" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 21px" class="tdBackColor">
                               <asp:TextBox ID="txtStartDate" onclick="calendar()" runat="server" CssClass="ipt160px" Width="129px"></asp:TextBox></td></tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label61" runat="server" Text="<%$ Resources:BaseInfo,LblDate_EndDate %>"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:TextBox ID="txtEndDate" onclick="calendar()" runat="server" CssClass="ipt160px" Width="129px"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label63" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:TextBox ID="txtContactName" runat="server" CssClass="ipt160px" Width="131px" MaxLength="32"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label ID="Label64" runat="server" Text="<%$ Resources:BaseInfo,Dept_lblTel %>" CssClass="labelStyle"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:TextBox ID="txtContactTel" runat="server" CssClass="ipt160px" Width="131px" MaxLength="16"></asp:TextBox></td>
                       </tr>
            </table>
            </td>
            <td class="tdBackColor" valign="top" style="width:40%; height: 349px;">
              <table class="tdBackColor">
                       <tr style="height:15px">
                           <td style="width: 932px;">
                           </td>
                           <td  valign="bottom" style="width: 347px">
                               <table style="width:134px;" border="0" cellpadding="0" cellspacing="0">
                                   <tr class="bodyLine">
                                       <td style="height:1px; background-color: #738495;" class="tdBackColor">
                                       </td>
                                   </tr>
                                   <tr class="bodyLine">
                                       <td style=" height: 1px; background-color: #ffffff;" class="tdBackColor">
                                       </td>
                                   </tr>
                               </table>
                           </td>
                       </tr>
                  <tr>
                      <td class="tdBackColor" style="text-align: right">
                          <asp:Label id="Label56" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblMainBrand %>" CssClass="labelStyle"></asp:Label></td>
                      <td class="tdBackColor">
                          <asp:TextBox ID="txtShopBrand" runat="server" Width="129px" CssClass="ipt160px"></asp:TextBox></td>
                  </tr>
                       <tr>
                           <td style="text-align: right" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label27" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblBuilding %>"></asp:Label></td>
                           <td  class="tdBackColor">
                               <asp:DropDownList ID="DDownListBuilding" runat="server" Width="133px" OnSelectedIndexChanged="DDownListBuilding_SelectedIndexChanged" AutoPostBack="True">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style="TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label28" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>" CssClass="labelStyle"></asp:Label></td><td style="HEIGHT: 21px; width: 347px;" class="tdBackColor"><asp:DropDownList id="DDownListFloors" runat="server" Width="133px" AutoPostBack="True" OnSelectedIndexChanged="DDownListFloors_SelectedIndexChanged">
                               </asp:DropDownList></td></tr>
                       <tr>
                           <td style=" text-align:right;" class="tdBackColor">
                               <asp:Label ID="Label29" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationName %>"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:DropDownList ID="DDownListLocation" runat="server" Width="133px" OnSelectedIndexChanged="DDownListLocation_SelectedIndexChanged" AutoPostBack="True">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style=" TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label30" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblSelectUnit %>" CssClass="labelStyle"></asp:Label></td><td style="HEIGHT: 23px; width: 347px;" class="tdBackColor"><asp:DropDownList id="DDownListUnit" runat="server" Width="133px">
                               </asp:DropDownList></td></tr>
                       <tr style="height:50px"><td style="TEXT-ALIGN: right" class="tdBackColor"></td><td class="tdBackColor">
                               <asp:Button ID="IBtnUnitsDel" runat="server" CssClass="buttonClear" Height="31px"
                                   OnClick="IBtnUnitsDel_Click" Text="<%$ Resources:BaseInfo,Btn_Del %>" Width="70px" />
                               <asp:Button ID="IBtnUnitsAdd" runat="server" CssClass="buttonSave" Height="31px"
                                   OnClick="IBtnUnitsAdd_Click" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="64px" />
                           </td></tr>
                       <tr>
                           <td style="text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label34" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblNoeUnitCollect %>"></asp:Label></td>
                           <td class="tdBackColor" rowspan="3" style="width: 347px">
                               <asp:ListBox ID="ListBoxUnits" runat="server" Width="137px" Height="94px"></asp:ListBox></td>
                       </tr>
                       <tr>
                           <td style=" text-align: right;" class="tdBackColor">
                               <asp:Label ID="Label19" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblComm %>"></asp:Label></td>
                       </tr>
                       <tr>
                           <td class="tdBackColor" style="text-align: right; height: 31px;">
                           </td>
                       </tr>
            </table>
            </td>
            </tr>
                <tr style="height:3px">
                    <td class="tdBackColor" style="width: 30%;" valign="top">
                    </td>
                    <td class="tdBackColor" colspan="2" style="text-align: center;" valign="top">
                        <table style="width:95%;" border="0" cellpadding="0" cellspacing="0">
                            <tr  class="bodyLine">
                                <td style="height: 1px; background-color: #738495;" >
                                </td>
                            </tr>
                            <tr  class="bodyLine">
                                <td style="height: 1px; background-color: #ffffff;" >
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="height:45px" valign="top">
                    <td class="tdBackColor" style="width: 30%; height: 45px;" valign="top">
                    </td>
                    <td class="tdBackColor" style="width: 30%; text-align: right; height: 45px;" valign="top">
                    </td>
                    <td class="tdBackColor" style="width: 40%; text-align: right; padding-right:20px; height: 45px;" valign="top"><asp:Button ID="btnShopAdd" runat="server" CssClass="buttonSave" Height="31px"
                                   OnClick="btnShopAdd_Click" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="64px" />
                        <asp:Button ID="btnShopDel" runat="server" CssClass="buttonClear" Height="31px"
                                   OnClick="btnShopDel_Click" Text="<%$ Resources:BaseInfo,Btn_Del %>" Width="70px" />&nbsp;
                               <asp:Button ID="btnShopSave" runat="server" CssClass="buttonCancel" Height="33px" OnClick="btnShopSave_Click"
                                   Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Width="74px" /></td>
                </tr>
            </table>
    </div>
            <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
             <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
