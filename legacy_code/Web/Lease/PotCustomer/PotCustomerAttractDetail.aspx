<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PotCustomerAttractDetail.aspx.cs" Inherits="Lease_PotCustomer_PotCustomerAttractDetail"  %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "PotCustomer_CustAttractQuery")%></title>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
		<script type="text/javascript">
		var tabbar ;
	    function styletabbar()
	    {
	       
	        tabbar=new dhtmlXTabBar("a_tabbar","top");
            tabbar.setImagePath("../../App_Themes/Tabbar/imgs/");
            
            tabbar.setSkinColors("#FCFBFC","#F4F3EE","#e1e0b2");
         
            tabbar.addTab("PotCustomer_a",document.getElementById("hidContractAuth").value,"80px");
            tabbar.addTab("PotCustomer_b",document.getElementById("hidConcessionAuth").value,"80px");
            
            tabbar.setTabActive("PotCustomer_a");
			
            tabbar.setContent("PotCustomer_a","a1");
            tabbar.setContent("PotCustomer_b","a2");
            
            addTabTool('<%= (String)GetGlobalResourceObject("BaseInfo", "Menu_BackHome")%>'+",Lease/PotCustomer/PotCustomerAttractQuery.aspx");
	        loadTitle();
	    }
	     function styletabbar_atv()
	    {
	   
	        if(  typeof(a_tabbar)=="undefined")
	        {
	        }
	        else
	        { 
	          if (a_tabbar.innerHTML=="")
	          {
	            tabbar=new dhtmlXTabBar("a_tabbar","top");
                tabbar.setImagePath("../../App_Themes/Tabbar/imgs/");
                
                tabbar.setSkinColors("#FCFBFC","#F4F3EE","#e1e0b2");
             
                tabbar.addTab("PotCustomer_a",document.getElementById("hidContractAuth").value,"80px");
                tabbar.addTab("PotCustomer_b",document.getElementById("hidConcessionAuth").value,"80px");
                
         
                tabbar.setTabActive("PotCustomer_a");
    			
                tabbar.setContent("PotCustomer_a","a1");
                tabbar.setContent("PotCustomer_b","a2");
                }
              }
        }
        </script>
</head>
<body onload="styletabbar()" topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
 <asp:ScriptManager id="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center" style="width:100%; ">
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <ContentTemplate  >
            <table border="0" cellpadding="0" cellspacing="0" style="height: 430px;width:100%; vertical-align:top">
                    <td style="width:30%; vertical-align: top;">
                    <table border="0" cellpadding="0" cellspacing="0" style=" height: 430px;vertical-align: top; width:100%">
                        
                        <tr>
                          <td style="vertical-align:top; height: 22px; width: 100%; background-color: #e1e0b2;" >          
                                 <table border="0" cellpadding="0" cellspacing="0" style="height: 22px; width:100%;   ">              
                                    <tr>
                                        <td class="tdTopRightBackColor"    valign="top" style="width: 8px; height:22px;  text-align:left" >
                                            <img alt="" class="imageLeftBack" style=" text-align:left; height: 22px;"  />
                                            </td>
                                            <td class="tdTopRightBackColor" style="height: 22px; text-align: left;">
                                        <asp:Label ID="labUserTree" runat="server" CssClass="lblTitle" Text='<%$ Resources:BaseInfo,PotCustomer_MenuBasic%>'></asp:Label></td>
                                      
                                        <td class="tdTopRightBackColor"   valign="top" style="width: 20px; height: 22px;">
                                            <img class="imageRightBack" style="width: 7px; height: 22px" />
                                            </td>
                                    </tr>                       
                                </table>               
                        </td>
                        </tr>
                        <tr>  
                             <td colspan="2" style="height: 300px; background-color: #e1e0b2; vertical-align:top; text-align:center;">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">   
                                    <tr>
                                        <td style="width:15px;"></td>
                                        <td align="center" style="width:100px">
                                            <asp:Label ID="lblCustCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                        <td align="left" >
                                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px"
                                                MaxLength="16" ReadOnly="True" Width="160px"></asp:TextBox></td>
                                        <td style="width:5px;"> </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15px; height: 56px" >
                                        </td>
                                        <td style="height: 56px" align="center" >
                                            <asp:Label ID="lblCustName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                        <td style="height: 56px" align="left" >
                                            <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px"
                                                MaxLength="64" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                        <td style="width: 25px; height: 56px" >
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15px" >
                                        </td>
                                        <td style="height: 36px" align="center">
                                            <asp:Label ID="lblCustShortName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                                        <td align="left" >
                                            <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px" MaxLength="32" Width="160px" ReadOnly="True"></asp:TextBox></td>
                                        <td style="width: 5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width:15px">
                                        </td>
                                        <td style="height: 36px" align="center">
                                            <asp:Label ID="lblCustType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustType %>"></asp:Label></td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlCustType" runat="server" CssClass="ipt160px" Enabled="False" Width="160px">
                                            </asp:DropDownList></td>
                                        <td style="width: 5px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5px">
                                        </td>
                                        <td valign="top" colspan="2" align="center" style="height:20px">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" >
                                    <tr >
                                        <td style="width: 180px; height: 1px; background-color: #738495;" align="center">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 180px; height: 1px; background-color: #ffffff;" align="center">
                                        </td>
                                    </tr>
                                </table>
                                            </td>
                                        <td style="width: 5px" valign="top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5px">
                                        </td>
                                        <td colspan="2" valign="top" align="center">
                                            <asp:GridView ID="GrdCust" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="95%"
                                                OnPageIndexChanging="GrdCust_PageIndexChanging" Width="90%" OnRowDataBound="GrdCust_RowDataBound" PageSize="6" OnSelectedIndexChanged="GrdCust_SelectedIndexChanged">
                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                <Columns>
                                                    <asp:BoundField DataField="CustId">
                                                        <ItemStyle CssClass="hidden" />
                                                        <HeaderStyle CssClass="hidden" />
                                                        <FooterStyle CssClass="hidden" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ShopSort" HeaderText="<%$ Resources:BaseInfo,PotCustomer_CustAttractTime %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="StoreName" HeaderText="<%$ Resources:BaseInfo,Store_StoreName %>">
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" ShowSelectButton="True">
                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" HorizontalAlign="Left" />
                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                    </asp:CommandField>
                                                </Columns>
                                                <PagerTemplate>
                                                    <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page"
                                                        Font-Size="Smaller" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">首页</asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev"
                                                        CommandName="Page" Font-Size="Smaller" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page"
                                                        Font-Size="Smaller" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton>
                                                    <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                                                        Font-Size="Smaller" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">尾页</asp:LinkButton>
                                                    <asp:TextBox ID="txtNewPageIndex" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>'
                                                        Width="20px"></asp:TextBox>/
                                                    <asp:Label ID="LabelPageCount" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                                                    <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-1"
                                                        CommandName="Page" Font-Size="Smaller" Text="GO"></asp:LinkButton>
                                                </PagerTemplate>
                                                <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
                                                <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                            </asp:GridView>
                                        </td>
                                        <td style="width: 5px" valign="top">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5px" valign="top">
                                        </td>
                                        <td valign="top">
                                        </td>
                                        <td valign="top">
                                        </td>
                                        <td style="width: 5px" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td> 
                    <td style="width: 1.5%; height: 430px">
                    </td>
                    <td colspan="3" style="width: 68%; height: 430px; vertical-align:top;">
                           <table border="0" cellpadding="0" cellspacing="0" style="height: 300px; width:100%;">
                               <tr>
                                <td colspan="8" valign="top" style="height: 22px; vertical-align: top; background-color: #e1e0b2;" rowspan="">
                                   <table border="0" cellpadding="0" cellspacing="0"style="height: 22px; width: 100%;">
                                        <tr>
                                          <td valign="top" style="height: 27px ; text-align:left; width: 8px;" class="tdTopRightBackColor">
                                            <img alt="" class="imageLeftBack" style="height: 22px" />
                                            </TD>
                                            <td style="height: 27px;WIDTH:378px; text-align: left;" class="tdTopRightBackColor">
                                                <asp:Label ID="labUserDefine" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,PotCustomer_CustAttractInfo %>"></asp:Label></td>
                                           <td style="height: 27px ; text-align:RIGHT; width: 3px;" class="tdTopRightBackColor">
                                               &nbsp;</td>
                                        </tr>
                                    </table>
                                   </td>
                            </tr>
                            <tr>
                                <td class="tdBackColor" colspan="8" style="height: 5px; text-align: center;">
                                </td>
                            </tr>              
                            <tr>
                                <td class="tdBackColor" colspan="8" style=" height: 229px; text-align:left;" valign="top">
                                    <div id="a_tabbar" style="width: 95%; height: 349px">
                                    </div>
                                    <div id="a2">
                                    <table>
                                    <tr>
                                        <td style="vertical-align: middle; width: 100px; height: 35px; text-align: right">
                                            <asp:Label ID="lblBizMode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_BizMode %>"></asp:Label></td>
                                        <td style="height: 25px; text-align: center">
                                            <asp:DropDownList ID="ddlBizMode" runat="server" CssClass="cmb160px" Enabled="False">
                                            </asp:DropDownList></td>
                                        <td style="height: 25px; text-align: center">
                                        </td>
                                    <td style="height: 25px; text-align:center;">
                                        <asp:Label ID="lblShopStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
                                    <td style="height: 25px;">
                                        <asp:TextBox ID="txtShopStartDate" runat="server" CssClass="ipt160px" onclick="calendar()" ReadOnly="True"></asp:TextBox></td>
                                    </tr>
                                        <tr>
                                            <td style="vertical-align: middle; width: 100px; height: 35px; text-align: right">
                                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopTypeTitle %>"></asp:Label></td>
                                            <td style="height: 25px; text-align: center">
                                                <asp:DropDownList ID="ddlShopType" runat="server" CssClass="cmb160px" Enabled="False">
                                                </asp:DropDownList></td>
                                            <td style="height: 25px; text-align: right">
                                            </td>
                                            <td style="height: 25px; text-align: right">
                                                <asp:Label ID="lblShopEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopEndDate %>"></asp:Label></td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtShopEndDate" runat="server" CssClass="ipt160px" onclick="calendar()" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: middle; width: 100px; height: 35px; text-align: right">
                                                <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_BusinessItem %>"></asp:Label></td>
                                            <td style="height: 25px; text-align: center">
                                                <asp:DropDownList ID="ddlBusinessItem" runat="server" CssClass="cmb160px" Enabled="False">
                                                </asp:DropDownList></td>
                                            <td style="height: 25px; text-align: right">
                                            </td>
                                            <td style="height: 25px; text-align: right">
                                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_IntentUnits %>"></asp:Label></td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtUnits" runat="server" CssClass="ipt160px" Height="32px" TextMode="MultiLine"
                                                    Width="160px" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: middle; width: 100px; height: 35px; text-align: right">
                                                <asp:Label ID="lblRentalPrice" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblRentalPrice %>"></asp:Label></td>
                                            <td style="height: 25px; text-align: center">
                                                <asp:TextBox ID="txtRentalPrice" runat="server" CssClass="ipt160px" MaxLength="9" ReadOnly="True"></asp:TextBox></td>
                                            <td style="height: 25px; text-align: right">
                                            </td>
                                            <td style="height: 25px; text-align: right">
                                                <asp:Label ID="lblPotShopName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"></asp:Label></td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtPotShopName" runat="server" CssClass="ipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: middle; height: 35px; text-align: right">
                                                <asp:Label ID="lblRentArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblRentArea %>"></asp:Label></td>
                                            <td style="height: 25px; text-align: center">
                                                <asp:TextBox ID="txtRentArea" runat="server" CssClass="ipt160px" MaxLength="13" ReadOnly="True"></asp:TextBox></td>
                                            <td style="width: 60px; height: 25px; text-align: right">
                                            </td>
                                            <td style="height: 25px; text-align: right">
                                                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PotShopHighReg %>"></asp:Label></td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtHighReg" runat="server" CssClass="ipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: middle; height: 35px; text-align: right">
                                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustRentInc %>"></asp:Label></td>
                                            <td style="height: 25px; text-align: center">
                                                <asp:TextBox ID="txtRentInc" runat="server" CssClass="ipt160px" MaxLength="13" ReadOnly="True"></asp:TextBox></td>
                                            <td style="height: 25px; text-align: right">
                                            </td>
                                            <td style="height: 25px; text-align: right">
                                                <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PotShopLoadReg %>"></asp:Label></td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtLoadReg" runat="server" CssClass="ipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 35px; text-align: right">
                                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustTakeOutRate %>"></asp:Label></td>
                                            <td style="width: 100px; height: 25px; text-align: center">
                                                <asp:TextBox ID="txtPcent" runat="server" CssClass="ipt160px" MaxLength="13" ReadOnly="True"></asp:TextBox></td>
                                            <td style="height: 25px; text-align: right">
                                            </td>
                                            <td style="height: 25px; text-align: right">
                                                <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PotShopWaterReg %>"></asp:Label></td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtWaterReg" runat="server" CssClass="ipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: middle; height: 35px; text-align: right">
                                                <asp:Label ID="lblMainBrand" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblMainBrand %>"></asp:Label></td>
                                            <td style="width: 100px; height: 25px; text-align: center">
                                                <asp:TextBox ID="txtMainBrand" runat="server" CssClass="ipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                                            <td style="height: 25px; text-align: right">
                                            </td>
                                            <td style="height: 25px; text-align: right">
                                                <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PotShopPowerReg %>"></asp:Label></td>
                                            <td style="height: 25px">
                                                <asp:TextBox ID="txtPowerReg" runat="server" CssClass="ipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                        </div>
                                            <div id="a1" style="width: 388px; height: 164px">
                                                <table>
                                                    <tr>
                                                        <td rowspan="9" style="vertical-align: top; width: 20px; text-align: center">
                                                            &nbsp;</td>
                                                        <td style="width: 100px; text-align:center; vertical-align:top;" rowspan="9">
                                                            <asp:GridView ID="GrdCustPalaverInfo" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                BackColor="White" BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Font-Size="12pt"
                                                                Font-Strikeout="False" Height="90%" OnPageIndexChanging="GrdCustPalaverInfo_PageIndexChanging"
                                                                Width="240px" OnRowDataBound="GrdCustPalaverInfo_RowDataBound" OnSelectedIndexChanged="GrdCustPalaverInfo_SelectedIndexChanged">
                                                                <Columns>
                                                                    <asp:BoundField DataField="PalaverID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PalaverTime" HeaderText="<%$ Resources:BaseInfo,PotShop_lblPalaverTime %>">
                                                                        <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="PalaverAim" HeaderText="<%$ Resources:BaseInfo,PotCustomer_CustMostlyTitle %>">
                                                                        <ItemStyle BorderColor="#E1E0B2" Font-Size="10.5pt" HorizontalAlign="Center" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>
                                                                    <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_lblUserQuery %>" SelectText="<%$ Resources:BaseInfo,User_lblUserQuery %>"
                                                                        ShowSelectButton="True">
                                                                        <ItemStyle BorderColor="#E1E0B2" HorizontalAlign="Center" />
                                                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:CommandField>
                                                                </Columns>
                                                                <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                                <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                                <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Right" />
                                                                <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                                                <PagerTemplate>
                                                                    <asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page"
                                                                        Font-Size="Smaller" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">首页</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev"
                                                                        CommandName="Page" Font-Size="Smaller" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>">上一页</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page"
                                                                        Font-Size="Smaller" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">下一页</asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page"
                                                                        Font-Size="Smaller" Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>">尾页</asp:LinkButton>
                                                                    <asp:TextBox ID="txtNewPageIndex" runat="server" Text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>'
                                                                        Width="20px"></asp:TextBox>/
                                                                    <asp:Label ID="LabelPageCount" runat="server" Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label>
                                                                    <asp:LinkButton ID="btnGo" runat="server" CausesValidation="False" CommandArgument="-1"
                                                                        CommandName="Page" Font-Size="Smaller" Text="GO"></asp:LinkButton>
                                                                </PagerTemplate>
                                                                <PagerSettings Mode="NextPreviousFirstLast" />
                                                            </asp:GridView>
                                                            &nbsp;</td>
                                                        <td rowspan="9" style="vertical-align: top; width: 100px; text-align: center">
                                                            &nbsp;
                                                        </td>
                                                        <td rowspan="9" style="vertical-align: top; width: 100px; text-align: center">
                                                            &nbsp;</td>
                                                        <td rowspan="9" style="vertical-align: top; width: 100px; text-align: center">
                                                            &nbsp;</td>
                                                        <td rowspan="9" style="vertical-align: top; width: 100px; text-align: center">
                                                            &nbsp;</td>
                                                        <td rowspan="9" style="vertical-align: top; width: 100px; text-align: center">
                                                            &nbsp;
                                                        </td>
                                                        <td align="right" style="height: 30px">
                                                            &nbsp;<asp:Label ID="lblPalaverTime" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustAttractProcessType %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlProcessTypeId" runat="server" CssClass="ipt160px" Enabled="False">
                                                            </asp:DropDownList>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 30px">
                                                            <asp:Label ID="lblPalaverUser" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustPalaverRound %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtPalaverRound" runat="server" CssClass="ipt160px" MaxLength="16"
                                                                ReadOnly="True"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 30px">
                                                            <asp:Label ID="lblPalaverAim" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_PalaverType %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtPalaverPlace" runat="server" CssClass="ipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 30px">
                                                            <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustContactorName %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtContactorName" runat="server" CssClass="ipt160px" MaxLength="16" ReadOnly="True"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 30px">
                                                            <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustMostlyTitle %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtPalaverAim" runat="server" CssClass="ipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 30px">
                                                            <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_PalaverAndOther %>"
                                                                Width="89px"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtPalaverContent" runat="server" CssClass="ipt160px" Height="37px"
                                                                MaxLength="512" TextMode="MultiLine" ReadOnly="True"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 30px">
                                                            <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustPalaverResult %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtPalaverResult" runat="server" CssClass="ipt160px" MaxLength="256" ReadOnly="True"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 30px">
                                                            <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_CustUnSolved %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtUnSolved" runat="server" CssClass="ipt160px" MaxLength="256" ReadOnly="True"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" style="height: 30px">
                                                            <asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblNote %>"></asp:Label></td>
                                                        <td>
                                                            <asp:TextBox ID="txtNode" runat="server" CssClass="ipt160px" MaxLength="128" ReadOnly="True"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                            </div>
                                    <div style="width: 387px; height: 40px;text-align:center;">
                                    <table>
                                    <tr>
                                    <td style="  text-align:right; vertical-align:top; height:45px; width: 310px; margin-left: 100px;">
                                    </td>
                                    </tr>
                                    </table>
                                        </div>
                                </td>
                            </tr>
                        </table>
                    </td>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
        <asp:HiddenField ID="hidSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidSelect %>" />
        <asp:HiddenField ID="hidCondition" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidCondition %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />   
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidConcessionAuth" runat="server" Value="<%$ Resources:BaseInfo,Hidden_Shop %>" />
        <asp:HiddenField ID="hidOther" runat="server" Value="<%$ Resources:BaseInfo,Dept_Other %>" />
        <asp:HiddenField ID="hidContractAuth" runat="server" Value="<%$ Resources:BaseInfo,PotShop_lblPalaverNode %>" />
        <asp:HiddenField ID="hidTradeAuth" runat="server" Value="<%$ Resources:BaseInfo,Dept_lblTradeAuth %>" />
        <asp:HiddenField ID="hidUserBeing" runat="server" Value="<%$ Resources:BaseInfo,Hidden_DeptCodeBeing %>" />
        <asp:HiddenField ID="hidNotSelect" runat="server" Value="You can't select this department type!" />
    </form>
</body>
    
</html>