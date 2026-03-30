<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustPalaver.aspx.cs" Inherits="CustPalaver" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript">
    	function chooseCard(id)
	    {
	        if(id==0)
	        {
		        document.getElementById("PotCustomerList").style.visibility ="visible";
		        document.getElementById("PotCustomer").style.visibility ="hidden";
		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="10px";
		        document.getElementById("chooseDiv").style.position="absolute";
		        document.getElementById("chooseDiv").style.top ="410px";
		        document.getElementById("chooseDiv").style.left ="95px";
		        document.getElementById("tab2").className="tab";
		        document.getElementById("tab1").className="selectedtab";
	        }
	        if(id==1)
	        {
		        document.getElementById("PotCustomerList").style.visibility ="hidden";
		        document.getElementById("PotCustomer").style.visibility ="visible";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="10px";
                document.getElementById("tab1").className="tab";
		        document.getElementById("tab2").className="selectedtab";
		        document.getElementById("lblTotalNum").style.display="none";
                document.getElementById("lblCurrent").style.display="none";
	        }
	    }
	    
	    	    //text控件文本验证
    function OverruleValidator(sForm)
    {
        if(isEmpty(document.all.txtVoucherMemo.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtVoucherMemo.focus();
            return false;					
        }
    }
    </script>
</head>
<body onload='chooseCard(0);' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="chooseDiv" style="left: 448px; overflow: auto; width: 272px; position: absolute;
            top: 824px; height: 55px">
            <table>
                <tr>
                    <td id="tab1" class="treecard" onclick="chooseCard(0);" style="height: 21px">
                        <span id="tabpage1">
                            <asp:Label ID="hidShop" runat="server" Text="<%$ Resources:BaseInfo,Hidden_Shop %>"
                                Width="63px"></asp:Label></span></td>
                    <td id="tab2" class="tab" onclick="chooseCard(1);" style="height: 21px">
                        <span id="tabpage2">
                            <asp:Label ID="hidPalaverNode" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPalaverNode %>"
                                Width="61px"></asp:Label></span></td>
                </tr>
            </table>
        </div>
        <table id="showmain" border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle"
            style="height: 445px">
            <tr height="15">
                <td colspan="8">
                </td>
            </tr>
            <tr>
                <td style="width: 95px; height: 401px; text-align: center" valign="top">
                    <img height="401" src="../../images/shuxian.jpg" />
                </td>
                <td colspan="5" style="vertical-align: top; width: 572px; height: 401px">
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div id="PotCustomerList" style="width: 100px; height: 100px">
    
            <table border="0" cellpadding="0" cellspacing="0" style="height: 405px; width: 535px;">
            <tr>
                <td class="tdTopBackColor" style="width: 430px; height: 22px; vertical-align: middle;" valign="top">
                    <img alt="" class="imageLeftBack" style="height: 22px" /><asp:Label ID="lblVoucherTitle" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblVoucherTitle %>"
                        Width="152px"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 528px; height: 22px" valign="top">
                    <img class="imageRightBack" style="height: 22px" /></td>
            </tr>
            <tr>
                <td class="tdBackColor" colspan="2" style="width: 266px; height: 280px; text-align: left"
                    valign="top">
    
    <table border="0" cellpadding="0" cellspacing="0" style="width:266px; height:280px;">
        <tr>
            <td style="width:266px; height:1px; background-color:White;" colspan="4" ></td>
        </tr>
        <tr>
            <td style="width:266px; height:5px;" colspan="4" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:266px; height:10px;" colspan="4">
                <table border="0" cellpadding="0" cellspacing="0" style="left: 99px; width: 166px;
                    position: relative; top: 0px; text-align: center">
                    <tr>
                        <td style="width: 240px; height: 1px; background-color: #738495">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width:96px; height:22px; text-align:right;" class="tdBackColor">
            <asp:Label ID="lblCustCode" runat="server" Text='<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor">
            <asp:TextBox ID="txtCustCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
            <asp:Label ID="lblCustName" runat="server" Text='<%$ Resources:BaseInfo,PotCustomer_lblCustName %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor">
            <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
            <asp:Label ID="lblCustShortName" runat="server" Text='<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor">
            <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:266px; height:5px;" colspan="4" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:266px; height:10px;" colspan="4"><table border="0" cellpadding="0" cellspacing="0" style="left: 99px; width: 166px;
                    position: relative; top: 0px; text-align: center">
                <tr>
                    <td style="width: 240px; height: 1px; background-color: #738495">
                    </td>
                </tr>
                <tr>
                    <td style="width: 240px; height: 1px; background-color: #ffffff">
                    </td>
                </tr>
            </table>
            </td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
            <asp:Label ID="lblShopType" runat="server" Text='<%$ Resources:BaseInfo,PotShop_lblShopType %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor">
            <asp:DropDownList ID="cmbShopType" runat="server" CssClass="cmb160px" Enabled="False">
            </asp:DropDownList></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
                    <asp:Label ID="lblPotShopName" runat="server" Text='<%$ Resources:BaseInfo,PotShop_lblPotShopName %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor">
            <asp:TextBox ID="txtPotShopName" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
            <asp:Label ID="lblMainBrand" runat="server" Text='<%$ Resources:BaseInfo,PotShop_lblMainBrand %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor">
            <asp:TextBox ID="txtMainBrand" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:266px; height:5px;" colspan="4" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:266px; height:10px;" colspan="4"><table border="0" cellpadding="0" cellspacing="0" style="left: 99px; width: 166px;
                    position: relative; top: 0px; text-align: center">
                <tr>
                    <td style="width: 240px; height: 1px; background-color: #738495">
                    </td>
                </tr>
                <tr>
                    <td style="width: 240px; height: 1px; background-color: #ffffff">
                    </td>
                </tr>
            </table>
            </td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
            <asp:Label ID="Label7" runat="server" Text='<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor"><asp:TextBox ID="txtShopStartDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
            <asp:Label ID="Label8" runat="server" Text='<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor"><asp:TextBox ID="txtShopEndDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td colspan="4" rowspan="2" style="vertical-align: top; width: 266px; height: 15px;
                text-align: left">
                &nbsp;<table border="0" cellpadding="0" cellspacing="0" style="left: 99px; width: 166px;
                    position: relative; top: -7px; text-align: center">
                <tr>
                    <td style="width: 240px; height: 1px; background-color: #738495; left: 99px;">
                    </td>
                </tr>
                <tr>
                    <td style="width: 240px; height: 1px; background-color: #ffffff; left: 99px;">
                    </td>
                </tr>
            </table>
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
            <asp:Label ID="labArea" runat="server" Text='<%$ Resources:BaseInfo,RentableArea_lblArea %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor"><asp:DropDownList ID="cmbArea" runat="server" CssClass="cmb160px" Enabled="False">
            </asp:DropDownList></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;" class="tdBackColor"><asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text='<%$ Resources:BaseInfo,PotShop_lblRentalPrice %>' Width="97px"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor"><asp:TextBox ID="txtRentalPrice" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        <tr>
            <td style="width:96px; height:22px;text-align:right;" class="tdBackColor">
            <asp:Label ID="Label11" runat="server" Text='<%$ Resources:BaseInfo,PotShop_lblRentArea %>' CssClass="labelStyle"></asp:Label></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
            <td style="width:160px; height:22px;" class="tdBackColor"><asp:TextBox ID="txtRentArea" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
            <td style="width:5px; height:22px;" class="tdBackColor"></td>
        </tr>
        </table>
                   
                </td>
                <td style="width:269px; height:280px; text-align: center;" class="tdBackColor" valign="top">
        
           <table style="width:269px; height:280px;" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="width:269px; height:1px; background-color:White;" colspan="4"></td>
            </tr>
            <tr>
                <td style="width:269px; height:5px; " colspan="4" class="tdBackColor"></td>
            </tr>
            <tr>
                <td style="width:85px; height:1px;" class="tdBackColor"></td>
                <td style="width:5px; height:1px;" class="tdBackColor"></td>
                <td style="width:149px; height:15px;" class="tdBackColor"><table border="0" cellpadding="0" cellspacing="0" style="left: 104px; width: 153px; top: 0px; text-align: center">
                    <tr>
                        <td style="width: 240px; height: 1px; background-color: #738495">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                        </td>
                    </tr>
                </table>
                </td>
                <td style="width:30px; height:1px;" class="tdBackColor"></td>
            </tr>
            <tr>
                <td style="width:85px; height:100px; text-align:right;" class="tdBackColor" valign="top">
                <asp:Label ID="lblNote" runat="server" Text='<%$ Resources:BaseInfo,User_lblNote %>' CssClass="labelStyle"></asp:Label>
                </td>
                <td style="width:5px; height:100px;" class="tdBackColor"></td>
                <td style="width:149px; height:100px;" class="tdBackColor">
              <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Height="100px" Width="150px" ReadOnly="True" CssClass="EnabledColor"></asp:TextBox></td>
              <td style="width:30px; height:100px;" class="tdBackColor"></td>
            </tr>
               <tr>
                   <td class="tdBackColor" style="width: 85px; height: 15px;">
                   </td>
                   <td class="tdBackColor" style="width: 5px; height: 15px;">
                   </td>
                   <td class="tdBackColor" style="width: 149px; height: 15px; vertical-align: middle; text-align: center;">
                       <table border="0" cellpadding="0" cellspacing="0" style="left: 104px; width: 153px;
                           top: 0px; text-align: center">
                           <tr>
                               <td style="width: 240px; height: 1px; background-color: #738495">
                               </td>
                           </tr>
                           <tr>
                               <td style="width: 240px; height: 1px; background-color: #ffffff">
                               </td>
                           </tr>
                       </table>
                   </td>
                   <td class="tdBackColor" style="width: 30px; height: 15px;">
                   </td>
               </tr>
            <tr>
                <td style="width:85px; height:25px; text-align:right; vertical-align: middle;" class="tdBackColor" valign="top">
                    <asp:Label ID="lblCommOper" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContractAuditing_labAttract %>"></asp:Label></td>
                <td style="width:5px; height:25px;" class="tdBackColor"></td>
                <td style="width:149px; height:25px;" class="tdBackColor" >
                    <asp:TextBox ID="txtCommOper" runat="server" CssClass="Enabledipt160px" ReadOnly="True"
                        Width="150px"></asp:TextBox></td>
             <td style="width:30px; height:25px;" class="tdBackColor"></td>
            </tr>
               
            
            <tr>
                <td style="width:269px; height:10px; " colspan="4" class="tdBackColor"></td>
            </tr>
            <tr>
                <td style="width:85px; height:1px;" class="tdBackColor"></td>
                <td style="width:5px; height:1px;" class="tdBackColor"></td>
                <td style="width:149px; height:15px;" class="tdBackColor"><table border="0" cellpadding="0" cellspacing="0" style="left: 104px; width: 153px; top: 0px; text-align: center">
                    <tr>
                        <td style="width: 240px; height: 1px; background-color: #738495">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 240px; height: 1px; background-color: #ffffff">
                        </td>
                    </tr>
                </table>
                </td>
                <td style="width:30px; height:1px;" class="tdBackColor"></td>
            </tr>
            <tr>
                <td style="width:85px; height:100px; text-align:right;" class="tdBackColor" valign="top">
                <asp:Label ID="labPalaverNote" runat="server" Text='<%$ Resources:BaseInfo,CustPalaver_labPalaverNote %>'  CssClass="labelStyle" Width="66px" ></asp:Label>
                </td>
                <td style="width:5px; height:100px;" class="tdBackColor"></td>
                <td style="width:149px; height:100px;" class="tdBackColor" >
            <asp:TextBox ID="txtVoucherMemo" runat="server" TextMode="MultiLine" Height="100px" Width="150px" CssClass="OpenColor" MaxLength="512"></asp:TextBox></td>
             <td style="width:30px; height:100px;" class="tdBackColor"></td>
            </tr>
            <tr>
               <td style="width:85px; height:22px;" class="tdBackColor"></td>
               <td style="width:154px; height:22px; text-align:right;" class="tdBackColor" colspan="2">
                   <asp:Label ID="Label9" runat="server" Text='<%$ Resources:BaseInfo,LeaseholdContractAuditing_labOverruleMust %>' CssClass="labelStyle"></asp:Label>
                  </td>
                  <td style="width:30px; height:22px;" class="tdBackColor"></td>
            </tr>
        </table>
        
    <asp:Button ID="butOverrule" runat="server" Text='<%$ Resources:BaseInfo,CustPalaver_butOverrule %>'  OnClick="butOverrule_Click" CssClass="buttonClear" />&nbsp;
                    <asp:Button ID="butConsent" runat="server" Text='<%$ Resources:BaseInfo,CustPalaver_butConsent %>' OnClick="butConsent_Click" CssClass="buttonSave" /><asp:Button ID="butLeadPalaver" runat="server" Text='<%$ Resources:BaseInfo,CustPalaver_butLeadPalaver %> ' OnClick="butLeadPalaver_Click" CssClass="buttonCancel" Width="99px"/></td>
            </tr>
        </table>
                </div>
                <div id="PotCustomer" style="width: 100px; height: 100px">
    <table border="0" cellpadding="0" cellspacing="0" style="width:535px; height: 405px">
      <tr>
                        <td style="vertical-align:top; width: 247px; height: 22px;" colspan="2" >
                        
                         <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width: 535px;   ">
                        
                        
                            <tr>
                                <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                    <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                    </td>
                                    <td class="tdTopRightBackColor" style="width: 545px; height: 22px; text-align:left;">
                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblVoucherTitle %>"
                                            Width="152px"></asp:Label></td>
                              
                                <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px; text-align:right;">
                                    <img class="imageRightBack" style="width: 7px; height: 22px" />
                                    </td>
                            </tr>
                        
                        </table>
                        
                        
                        </td>
      </tr>
                                                      <tr>
                                                    <td colspan="2" style="height: 1px; background-color: white">
                                                    </td>
                                                </tr>
      <tr>
        <td style="height: 5px; width: 485px;" class="tdBackColor" colspan="2"></td>
      </tr>
      <tr>
          <td class="tdBackColor" colspan="2" style="vertical-align: middle; height: 11px;
              text-align: center">
              <table border="0" cellpadding="0" cellspacing="0" style="left: 104px; width: 522px; top: 0px; text-align: center">
            <tr>
                <td style="width: 240px; height: 1px; background-color: #738495">
                </td>
            </tr>
            <tr>
                <td style="width: 240px; height: 1px; background-color: #ffffff">
                </td>
            </tr>
        </table>
          </td>
      </tr>
      <tr>
        <td style="height: 150px; width: 485px; left: 5px; vertical-align: middle; text-align: center;" colspan="2" class="tdBackColor" align="center" valign="middle">
            <table>
            <tr>
            <td style="vertical-align: top; text-align: center">
               <asp:GridView ID="GrdCustPalaverInfo" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GrdCustPalaverInfo_SelectedIndexChanged" BackColor="White"  CellPadding="3" BorderStyle="Inset" BorderWidth="1px" Width="526px" PageSize="6" OnRowDataBound="GrdCustPalaverInfo_RowDataBound" Height="153px">
                <Columns>
                    <asp:BoundField DataField="PalaverID" FooterText="PalaverID">
                        <ItemStyle CssClass="hidden" />
                        <HeaderStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PalaverTime" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverTime %>" >
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PalaverName" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverUser %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PalaverAim" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverAim %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:CommandField ShowSelectButton="True" SelectText="<%$ Resources:BaseInfo,User_lblUserQuery %>" >
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                    </asp:CommandField>
                </Columns>
                    <FooterStyle BackColor="Red" ForeColor="#000066"/>
                    <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                    <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                    <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left" />
                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False"  />
            </asp:GridView>
            </td>
            </tr>
            </table>

            </td>
      </tr>
      <tr>
        <td style="height: 35px; width: 485px; text-align:right; vertical-align: bottom;"  colspan="2" class="tdBackColor">
            <asp:Label ID="lblCurrent" runat="server"
                        ForeColor="Red">1</asp:Label><asp:Label ID="lblTotalNum" runat="server"></asp:Label>&nbsp;<asp:Button
                            ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                            Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNext" runat="server"
                                CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" />
            &nbsp;</td>
      </tr>
      <tr>
        <td style="height: 12px; width: 315px;" class="tdBackColor">      
        <asp:Label ID="lblPalaverNode" runat="server" Text='<%$ Resources:BaseInfo,PotShop_lblPalaverNode %>'  Width="89px" CssClass="labelStyle"></asp:Label></td>
        <td style="height: 12px; width: 2688px; text-align:center; vertical-align: bottom;" valign="bottom" class="tdBackColor"><table border="0" cellpadding="0" cellspacing="0" style="left: 104px; width: 436px; top: 0px; text-align: center">
            <tr>
                <td style="width: 240px; height: 1px; background-color: #738495">
                </td>
            </tr>
            <tr>
                <td style="width: 240px; height: 1px; background-color: #ffffff">
                </td>
            </tr>
        </table>
        </td>
      </tr>
      <tr>
        <td style="height: 150px; width: 485px; vertical-align: middle; text-align: left;" colspan="2" class="tdBackColor">
        <table style="width: 531px">
        <tr>
        <td style="left: 5px; position: relative; height: 131px" colspan="2">
                <asp:TextBox ID="txtPalaverContent" runat="server" Height="104px" TextMode="MultiLine"
                Width="508px" CssClass="EnabledColor" ReadOnly="True"></asp:TextBox>
        </td>
        </tr>
        </table>

                </td>
      </tr>
    </table>
    
    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
                    &nbsp;
                </td>
                <td style="width: 60px; height: 401px; text-align: center" valign="top">
                    <img height="401" src="../../images/shuxian.jpg" />
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidCustLicense" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidCustLicense %>"/>
        <asp:HiddenField ID="hidCustContact" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidCustContact %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidInitiativeOverrule" runat="server" Value="<%$ Resources:BaseInfo,WrkFlwEntity_lblInitiativeOverrule %>" />
    </form>
</body>
</html>
