<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConUnionPalaver.aspx.cs" Inherits="Lease_ConUnionPalaver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
     <style type="text/css">
        <!--
        
        table.tblBase tr{ height:29px; }
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        table.tblBase tr.nullLine{ height:10px; }
        
        table.tblBase td.baseLable{ padding-right:10px;text-align:right;}
        table.tblBase td.baseInput{ align:left;padding-right:20px }
        
        div#chooseDiv { position: absolute; top: 398px; left: 36px; height: 55px;
            width: 320px; overflow: auto;}
        -->
               
        <!--
        
        table.tblFormulaH tr{ height:20px; }
        table.tblFormulaH tr.headLine{ height:1px; }
        table.tblFormulaH tr.bodyLine{ height:1px; }
        table.tblFormulaH tr.colLine{ height:10px; }
        
        -->
        
         <!--
        
        table.tblUnionLease tr{ height:25px; }
        table.tblUnionLease tr.headLine{ height:1px; }
        table.tblUnionLease tr.bodyLine{ height:1px; }
        table.tblUnionLease tr.colLine{ height:10px; }
        
        -->
        
        <--
            table.tblBaseShop tr{ height:25px; }
            table.tblBaseShop tr.headLine{ height:1px; }
            table.tblBaseShop tr.bodyLine{ height:1px; }
            table.tblBaseShop tr.colLine{ height:10px; }
            
            div#btn{ position: absolute; top: 400px; left: 475px; height: overflow: auto;}
        -->
       </style>
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript">
	<!--
	    function chooseCard(id)
	    {
	        if(id==0){
		        document.getElementById("PotCustomerList").style.display ="block";
		        document.getElementById("PotCustomer").style.display ="none";
		        document.getElementById("PotCustLicense").style.display ="none";
		        document.getElementById("ShopBaseInfo").style.display ="none";
		        
		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="20px";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="20px";
		        document.getElementById("PotCustLicense").style.position="absolute";
		        document.getElementById("PotCustLicense").style.top ="20px";
		        document.getElementById("ShopBaseInfo").style.position="absolute";
		        document.getElementById("ShopBaseInfo").style.top ="20px";
		     
		        document.getElementById("tab4").className="tab";
		        document.getElementById("tab3").className="tab";
		        document.getElementById("tab2").className="tab";
		        document.getElementById("tab1").className="selectedtab";
	        }
	        if(id==1){	
		        document.getElementById("PotCustomerList").style.display ="none";
		        document.getElementById("PotCustomer").style.display ="block";
		        document.getElementById("PotCustLicense").style.display ="none";
		        document.getElementById("ShopBaseInfo").style.display ="none";
		        
		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="20px";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="20px";
		        document.getElementById("PotCustLicense").style.position="absolute";
		        document.getElementById("PotCustLicense").style.top ="20px";
		        document.getElementById("ShopBaseInfo").style.position="absolute";
		        document.getElementById("ShopBaseInfo").style.top ="20px";
		        
		        document.getElementById("tab4").className="tab";
		        document.getElementById("tab3").className="tab";
		        document.getElementById("tab2").className="selectedtab";
		        document.getElementById("tab1").className="tab";
	        }
	        if(id==2){	
		        document.getElementById("PotCustomerList").style.display ="none";
		        document.getElementById("PotCustomer").style.display ="none";
		        document.getElementById("PotCustLicense").style.display ="block";
		        document.getElementById("ShopBaseInfo").style.display ="none";
		        
		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="20px";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="20px";
		        document.getElementById("PotCustLicense").style.position="absolute";
		        document.getElementById("PotCustLicense").style.top ="20px";
		        document.getElementById("ShopBaseInfo").style.position="absolute";
		        document.getElementById("ShopBaseInfo").style.top ="20px";
		        
		        document.getElementById("tab4").className="tab";
		        document.getElementById("tab3").className="selectedtab";
		        document.getElementById("tab2").className="tab";
		        document.getElementById("tab1").className="tab";
		       
	        }
	        if(id==3){
		        document.getElementById("PotCustomerList").style.display ="none";
		        document.getElementById("PotCustomer").style.display ="none";
		        document.getElementById("PotCustLicense").style.display ="none";
		        document.getElementById("ShopBaseInfo").style.display ="block";
		        
		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="20px";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="20px";
		        document.getElementById("PotCustLicense").style.position="absolute";
		        document.getElementById("PotCustLicense").style.top ="20px";
		        document.getElementById("ShopBaseInfo").style.position="absolute";
		        document.getElementById("ShopBaseInfo").style.top ="20px";
		        
		        document.getElementById("tab4").className="selectedtab";
		        document.getElementById("tab3").className="tab";
		        document.getElementById("tab2").className="tab";
		        document.getElementById("tab1").className="tab";
	        }
	    }
	    
	    
	    function ClickEvent(d)
        {					
			window.document.all("Hidden1").value = d; 
             if ((window.document.all("Hidden1").value != "undefined") && (window.document.all("Hidden1").value != "") && (window.document.all("Hidden1").value != "&nbsp;"))
             {
				var objImgBtn1 = document.getElementById('<%= LinkButton1.ClientID %>');  
                objImgBtn1.click(); 
             }
			 else
				return;         
        }
        
        function ClickEvent1(d)
        {
            window.document.all("Hidden3").value = d; 
             if ((window.document.all("Hidden3").value != "undefined") && (window.document.all("Hidden3").value != "") && (window.document.all("Hidden3").value != "&nbsp;"))
             {
				var objImgBtn1 = document.getElementById('<%= LinkButton2.ClientID %>');  
                objImgBtn1.click(); 
             }
			 else
				return;        
        }
	   
	    
	-->
    </script>
</head>
<body onload='chooseCard(0);'>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table id="showmain" border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle"
            style="height: 445px">
            <tr height="15">
                <td colspan="8">
                </td>
            </tr>
            <tr>
                <td style="width: 25px; height: 401px; text-align: center" valign="top">
                    <img height="401" src="../images/shuxian.jpg" />
                </td>
                <td colspan="5" style="vertical-align: top; width: 572px; height: 401px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                         <div id="PotCustomerList">
        <table class="tblBase" border="0" cellpadding="0" cellspacing="0" style="height: 380px; width: 709px;">
            <tr>
                <td style="width: 5px; height: 25px;" class="tdTopRightBackColor" valign="top">
                    <img src="" class="imageLeftBack"  align="left"/></td>
                <td style="width: 635px; height: 25px; text-align: left;" class="tdTopRightBackColor">
                    <asp:Label ID="Label28" runat="server" Text="<%$ Resources:BaseInfo,Union_lblPalaver %>"></asp:Label></td>
                <td style="height: 4px; text-align: right" class="tdTopRightBackColor" valign="top">
                    <img src="" class="imageRightBack" /></td>
                
            </tr>
            <tr>
            <td width="50%" class="tdBackColor" colspan="2"  valign="top">
             <table border="0" cellpadding="0" cellspacing="0">
                        <tr class="headLine">
                            <td colspan="2" style="height: 1px; background-color: white">
                            </td>
                        </tr>
                        <tr class="nullLine">
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr class="nullLine">
                            <td>
                            </td>
                            <td valign="bottom">
                                <table border="0" cellpadding="0" cellspacing="0" width="165">
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #738495;">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #ffffff;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                            <td class="tdBackColor" style=" width: 362px;">
                                <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 362px;">
                                <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 362px;">
                                <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr class="nullLine">
                            <td colspan="2" style="width: 635px;">
                            </td>
                        </tr>
                        <tr class="nullLine">
                            <td style="width: 498px" >
                            </td>
                            <td valign="bottom" style="width: 362px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 165px">
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 362px;">
                                <asp:DropDownList ID="cmbTradeID" runat="server" Width="162px">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 362px;">
                                <asp:TextBox ID="cmbContractStatus" runat="server" CssClass="ipt160px" ReadOnly="True"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 362px;">
                                <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label></td>
                            <td class="tdBackColor">
                                <asp:TextBox ID="txtRefID" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 362px;">
                                <asp:TextBox ID="txtConStartDate" onclick="calendar()" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 498px;">
                                <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 362px;">
                                <asp:TextBox ID="txtConEndDate" onclick="calendar()" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                 <tr>
                     <td class="baseLable" style="width: 498px; text-align: right">
                                <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>"></asp:Label></td>
                     <td class="tdBackColor" style="width: 362px">
                                <asp:DropDownList ID="cmbNotice" runat="server" Width="165px">
                                </asp:DropDownList></td>
                 </tr>
                    </table>
            </td>
            <td width="50%" class="tdBackColor" colspan="3" valign="top">
                              <table border="0" cellpadding="0" cellspacing="0" style="height: 280px">
                        <tr class="headLine">
                            <td colspan="2" style="height: 1px; background-color: white">
                            </td>
                        </tr>
                        <tr class="nullLine">
                            <td colspan="2" >
                            </td>
                        </tr>
                        <tr class="nullLine">
                            <td style="width: 152px" >
                            </td>
                            <td  valign="bottom" style="width: 237px">
                                <table border="0" cellpadding="0" cellspacing="0"  width="165">
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 152px;">
                                <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>"></asp:Label></td>
                            <td class="tdBackColor" style=" width: 237px;">
                                <asp:TextBox ID="txtChargeStart" onclick="calendar()" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 152px;">
                                <asp:Label ID="Label32" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblNorentDays %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 237px;">
                                <asp:TextBox ID="txtNorentDays" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 152px;">
                                <asp:Label ID="Label41" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labEConURL %>"></asp:Label></td>
                            <td class="tdBackColor" style="width: 237px;">
                                <asp:TextBox ID="txtEBargain" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                        </tr>
                        <tr class="nullLine">
                            <td colspan="2">
                            </td>
                        </tr>
                        <tr class="nullLine">
                            <td style="width: 152px">
                            </td>
                            <td valign="bottom" style="width: 237px">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 165px" >
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 152px;">
                                <asp:Label ID="Label46" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>"></asp:Label></td>
                            <td class="tdBackColor" rowspan="1" style="width: 237px" valign="top">
                                <asp:DropDownList ID="cmbPenalty" runat="server" Width="165px">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 152px;">
                                <asp:Label ID="Label48" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labAdditionalItem %>"></asp:Label></td>
                            <td class="tdBackColor" rowspan="3" style="width: 237px" valign="top">
                                <asp:TextBox ID="listBoxAddItem" runat="server" Height="60px" Width="161px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 152px;">
                            </td>
                        </tr>
                        <tr style="height:20px">
                            <td class="tdBackColor" style="text-align: right; width: 152px;">
                            </td>
                        </tr>
                        <tr>
                            <td class="baseLable" style="text-align: right; width: 152px;" >
                                <asp:Label ID="Label53" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"></asp:Label></td>
                            <td class="tdBackColor" rowspan="2" style="width: 237px" valign="top">
                                <asp:TextBox ID="listBoxRemark" runat="server" Height="40px" Width="161px"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style="text-align: right; width: 152px;">
                            </td>
                        </tr>
                                  <tr style="height:3px">
                                      <td class="tdBackColor" style="width: 152px; text-align: right">
                                      </td>
                                      <td class="tdBackColor" rowspan="1" style="width: 237px" valign="top">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 165px" >
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #738495">
                                    </td>
                                </tr>
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #ffffff">
                                    </td>
                                </tr>
                            </table>
                                      </td>
                                  </tr>
                                  <tr>
                                      <td class="baseLable" style="width: 152px; text-align: right">
                                          <asp:Label ID="Label33" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblCommOper %>"></asp:Label></td>
                                      <td class="tdBackColor" rowspan="1" style="width: 237px" valign="top">
                                          <asp:TextBox ID="txtCommOper" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                  </tr>
                    </table>
            </td>
            </tr>
        </table>
        </div>
        <div id="PotCustomer" >
        <table border="0" cellpadding="0" cellspacing="0" style="width: 709px; height: 380px">
        <tr>
            <td style="width: 350px;height:380px" class="tdBackColor" valign="top">
                <table class="tblFormulaH" border="0" cellpadding="0" cellspacing="0" style="width: 350px" >
                    <tr>
                        <td class="tdTopBackColor" style="width: 310px;" colspan="3" height="29">
                            <img alt="" class="imageLeftBack" /><asp:Label ID="Label12" runat="server"
                                Text="<%$ Resources:BaseInfo,Formula_lblContent %>"></asp:Label></td>
                        <td class="tdTopBackColor" valign="top" height="29">
                            <img  align="right" class="imageRightBack"/>
                        </td> 
                    </tr>
                    
                    <tr class="headLine">
                    <td style="height: 1px; background-color: white" colspan="4" width="317"></td>
                    </tr>
                        <tr class="colLine">
                            <td colspan="4" align="center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 338px">
                                    <tr class="bodyLine">
                                        <td  style=" height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td class="tdBackColor" style="height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="colLine"><td colspan="4"></td></tr>
                        <tr>
                            <td style="text-align: right; width: 85px;">
                                <asp:Label ID="labChargeTypeID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>"
                                    Width="70px"></asp:Label></td>
                            <td class="tdBackColor" style="width:70">
                                <asp:DropDownList ID="cmbChargeTypeID" runat="server" CssClass="textBoxStyle" Width="76px">
                                </asp:DropDownList></td>
                            <td class="tdBackColor" style="width: 60px; text-align: right">
                                <asp:Label ID="labFormulaType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFormulaType %>"
                                    Width="70px"></asp:Label></td>
                            <td class="tdBackColor" style=" width: 90px;">
                                <asp:DropDownList ID="cmbFormulaType" runat="server" CssClass="textBoxStyle" Width="76px">
                                </asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style=" text-align: right; width: 85px;" align="right">
                                <asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td>
                            <td class="tdBackColor">
                                <asp:TextBox ID="txtStartDate" onclick="calendar()" runat="server" CssClass="textBoxStyle" Width="70px"></asp:TextBox></td>
                            <td class="tdBackColor" style=" width: 60px;text-align: right">
                                <asp:Label ID="labEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labEndDate %>"></asp:Label></td>
                            <td class="tdBackColor" style=" width: 77px;">
                                <asp:TextBox ID="txtEndDate" onclick="calendar()" runat="server" CssClass="textBoxStyle" Width="70px"></asp:TextBox></td>
                        </tr>
                        <tr class="colLine"><td colspan="4"></td></tr>
                        <tr class="colLine">
                            <td colspan="4" align="center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 335px">
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr class="gridView">
                            <td class="tdBackColor" colspan="8" style="text-align: center"
                                valign="top">
                                <asp:GridView ID="GVConFormulaH" runat="server" AutoGenerateColumns="False" Height="200px"
                                    Width="323px" OnRowDataBound="GVConFormulaH_RowDataBound" OnRowCommand="GVConFormulaH_RowCommand" BackColor="White" BorderColor="#E1E0B2">
                                    <Columns>
                                        <asp:BoundField DataField="FormulaID">
                                            <ItemStyle CssClass="hidden" />
                                            <HeaderStyle CssClass="hidden" />
                                            <FooterStyle CssClass="hidden" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ChargeTypeName" HeaderText="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>" >
                                            <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BorderColor="#E1E0B2" />
                                            <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>" DataFormatString="{0:D}" HtmlEncode="False" >
                                            <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BorderColor="#E1E0B2" />
                                            <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FEndDate" HeaderText="<%$ Resources:BaseInfo,ConLease_labEndDate %>" DataFormatString="{0:D}" HtmlEncode="False" >
                                            <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BorderColor="#E1E0B2" />
                                            <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FormulaTypeName" HeaderText="<%$ Resources:BaseInfo,ConLease_labFormulaType %>">
                                            <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BorderColor="#E1E0B2" />
                                            <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                                        </asp:BoundField>
                                    </Columns>
                                    <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
                                    <RowStyle Height="10px" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 300px">
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" style=" text-align: center; width: 85px;">
                                </td>
                            <td class="tdBackColor" style=" text-align: center">
                                </td>
                            <td class="tdBackColor" style="text-align: center; width: 71px;">
                                </td>
                            <td class="tdBackColor" style="text-align: center; width: 77px;">
                                </td>
                        </tr>
                         <tr class="colLine">
                    <td class="tdBackColor" style="text-align: left; width: 115px; height: 10px;"
                        valign="top" colspan="4">
                       
                        <input id="Hidden1" runat="server" style="width: 5px; height: 6px;" type="hidden" />
                        <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" Visible="False" Width="1px" Height="1px"></asp:TextBox>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton></td>
                </tr>
                    </table>
               
            </td>
            <td style="width: 10px"></td>
            <td>
                <table id="Table2" border="0" cellpadding="0" cellspacing="0" style="width: 350px;height:380px" class="tdBackColor">
                     <tr>
                        <td class="tdTopBackColor" style="width: 310px;" colspan="3" valign="top">
                            <img alt="" class="imageLeftBack" /><asp:Label ID="Label14" runat="server"
                                Text="<%$ Resources:BaseInfo,Formula_lblContent %>"></asp:Label></td>
                        <td class="tdTopBackColor" valign="top">
                            <img  align="right" class="imageRightBack"/>
                        </td> 
                    </tr>
                    
                    <tr class="headLine">
                    <td style="height: 1px; background-color: white" colspan="4" width="317"></td>
                    </tr>
                     <tr height="10"><td ></td></tr>
                        <tr>
                            <td  colspan="2" style="width: 317px; padding-left:10px" valign="top">
                                            <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                                border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:300px; height: 1px;">
                                                <legend style="text-align: left">
                                                    <asp:Label ID="Label29" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_KeepMinSet %>"></asp:Label>
                                                </legend>
                                                <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 295px;
                                                    height: 33px">
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 69px; height: 22px">
                                                            <asp:Label ID="labMinSumSell" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labMinSumSell %>"
                                                                Width="76px"></asp:Label></td>
                                                        <td class="tdBackColor" style="width: 50px; height: 22px">
                                                            <asp:TextBox ID="txtMinSumSell" runat="server" CssClass="textBoxStyle" Width="63px"></asp:TextBox></td>
                                                        <td class="tdBackColor" style="width: 65px; height: 22px">
                                                            <asp:Label ID="labBalanceDeduct" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labBalanceDeduct %>"
                                                                Width="69px"></asp:Label></td>
                                                        <td colspan="4" style="width: 55px; height: 22px">
                                                            <asp:TextBox ID="txtBalanceDeduct" runat="server" CssClass="textBoxStyle" Width="50px"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="2" style="width: 299px; text-align: center; height:198px; padding-left:10px"  valign="top">
                                            <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                                border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width:300px; height:204px;">
                                                <legend style="text-align: left">
                                                    <asp:Label ID="Label45" runat="server" Text="<%$ Resources:BaseInfo,Lease_FormulaPSet %>" CssClass="labelStyle"></asp:Label></legend>
                                                <table border="0" cellpadding="0" cellspacing="0" class="tdBackColor" style="width: 295px; height:151px;">
                                                    <tr>
                                                        <td class="tdBackColor" colspan="3" style="width: 100px; height: 22px">
                                                            <asp:RadioButton ID="rabFastness" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabFastness %>"
                                                                TextAlign="Left" GroupName="a" CssClass="labelStyle" Checked="True" />&nbsp;</td>
                                                        <td class="tdBackColor" colspan="3" style="width: 100px; height: 22px">
                                                            <asp:RadioButton ID="rabMonopole" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMonopole %>"
                                                                TextAlign="Left" GroupName="a" CssClass="labelStyle" /></td>
                                                        <td class="tdBackColor" colspan="2" style="width: 95px; height: 22px">
                                                            <asp:RadioButton ID="rabMultilevel" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMultilevel %>"
                                                                TextAlign="Left" GroupName="a" CssClass="labelStyle" /></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="width: 295px; height: 5px">
                                                        </td>
                                                    </tr>
                                                    <tr class="gridView">
                                                        <td class="tdBackColor" colspan="8" style="width: 295px; text-align: center">
                                                            <asp:GridView ID="GVDeductMoney" runat="server" AutoGenerateColumns="False" Height="148px"
                                                                Width="290px" BackColor="White" BorderColor="#E1E0B2" OnRowDataBound="GVDeductMoney_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="FormulaID">
                                                                        <ItemStyle CssClass="hidden" />
                                                                        <HeaderStyle CssClass="hidden" />
                                                                        <FooterStyle CssClass="hidden" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="SalesTo" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Pcent" HeaderText="<%$ Resources:BaseInfo,Rpt_SkuPercent %>">
                                                                        <HeaderStyle CssClass="gridviewtitle" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="width: 295px; height: 10px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" style="width: 75px; height: 22px">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 50px; height: 22px">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 75px; height: 22px">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 5px; height: 22px">
                                                        </td>
                                                        <td class="tdBackColor" style="width: 40px; height: 22px">
                                                            </td>
                                                        <td class="tdBackColor" style="width: 40px; height: 22px">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="8" style="width: 295px; height: 6px; text-align:center;">
                                                        <hr style=" height:2px; width:280px;" />
                                                            <input id="Hidden3" runat="server" style="width: 3px; height: 8px" type="hidden" />&nbsp;
                                                            <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdBackColor" colspan="4" style="width: 158px; height: 10px;" align="center">
                                                            </td>
                                                        <td class="tdBackColor" colspan="4" style="width: 158px; height: 10px;" align="left">
                                                            </td>
                                                   </tr>
                                                </table>
                                            </fieldset>
                            </td>
                        </tr>
                    </table>
               </td>
            </tr>
        </table>
        </div>
           <div id="PotCustLicense">
        <table class="tdBackColor" border="0" cellpadding="0" cellspacing="0" style="width: 709px;">
            <tr height="29">
                <td style="width: 5px; height: 29px;" class="tdTopRightBackColor" valign="top">
                    <img src="" class="imageLeftBack"  align="left"/></td>
                <td style="width: 635px; height: 29px; text-align: left;" class="tdTopRightBackColor">
                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:BaseInfo,Union_lblPalaver %>"></asp:Label></td>
                <td style="height: 29px; text-align: right" class="tdTopRightBackColor" valign="top">
                    <img src="" class="imageRightBack" /></td>
            </tr>
            <tr class="headLine">
                            <td colspan="3" style="height: 1px; background-color: white" >
                            </td>
            </tr>
         </table>
        <table class="tblUnionLease" border="0" cellpadding="0" cellspacing="0"  style="height: 350px; width: 709px;">
            <tr class="colLine">
                <td class="tdBackColor">
                </td>
                <td class="tdBackColor" valign="bottom">
                     <table border="0" cellpadding="0" cellspacing="0" width="165">
                        <tr class="bodyLine">
                            <td style="height: 1px; background-color: #738495;">
                            </td>
                        </tr>
                        <tr class="bodyLine">
                            <td style="height: 1px; background-color: #ffffff;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" class="tdBackColor" width="20%" style="padding-right:5px">
                    <asp:Label ID="Label17" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labBillCycle %>"
                        Width="60px"></asp:Label></td>
                <td class="tdBackColor" width="80%">
                    <asp:DropDownList ID="DDownListBillCycle" runat="server" Width="165px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right" class="tdBackColor" width="20%" style="padding-right:5px">
                    <asp:Label ID="Label19" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labCurrencyType %>"
                        Width="70px"></asp:Label></td>
                <td class="tdBackColor" width="80%">
                    <asp:DropDownList ID="DDownListCurrencyType" runat="server" Width="165px">
                </asp:DropDownList></td>
            </tr>
            <tr>
                <td class="tdBackColor" width="20%" align="right" style="padding-right:5px">
                    <asp:Label ID="Label22" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labAccountCycle %>"
                        Width="72px"></asp:Label></td>
                <td class="tdBackColor" width="80%">
                    <asp:DropDownList ID="DDownListAccountCycle" runat="server" Width="165px">
                    </asp:DropDownList></td>
            </tr>
            <tr class="colLine">
                <td class="tdBackColor">
                </td>
                <td class="tdBackColor" valign="bottom">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td style="height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" width="20%"align="right" style="padding-right:5px">
                    <asp:Label ID="Label26" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConUnionRentInc %>"
                        Width="80px"></asp:Label></td>
                <td class="tdBackColor" width="80%">
                    <asp:TextBox ID="txtRentInc" runat="server" CssClass="ipt160px"></asp:TextBox></td>
            </tr>
            <tr class="colLine">
                <td class="tdBackColor" style="width: 1px">
                </td>
                <td class="tdBackColor" valign="bottom">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 160px">
                                    <tr class="bodyLine">
                                        <td class="tdBackColor" style="width: 160px; height: 1px; background-color: #738495">
                                        </td>
                                    </tr>
                                    <tr class="bodyLine">
                                        <td class="tdBackColor" style="width: 160px; height: 1px; background-color: #ffffff">
                                        </td>
                                    </tr>
                                </table>
                </td>
            </tr>
            <tr>
                <td class="tdBackColor" width="20%" align="right" style="padding-right:5px">
                    <asp:Label ID="Label27" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_lblTaxFrank %>"
                        Width="70px"></asp:Label></td>
                <td class="tdBackColor" width="80%">
                    <asp:TextBox ID="txtTaxRate" runat="server" CssClass="ipt160px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="tdBackColor" width="20%" align="right" style="padding-right:5px">
                    <asp:Label ID="Label30" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_InvoiceType %>"
                        Width="70px"></asp:Label></td>
                <td class="tdBackColor" width="80%">
                    <asp:DropDownList ID="DDownListTaxType" runat="server" Width="165px">
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                </td>
                <td class="tdBackColor" width="80%">
                </td>
            </tr>
            <tr>
                <td align="right" class="tdBackColor" style="padding-right: 5px" width="20%">
                </td>
                <td class="tdBackColor" width="80%">
                </td>
            </tr>
           </table>
        </div>
             <div id="ShopBaseInfo" >
  
          <table class="baseShop" border="0" cellpadding="0" cellspacing="0" style="height: 25px" width="709">
            <tr>
                <td class="tdTopBackColor" style="width: 120px; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" />
                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:BaseInfo,Union_lblPalaver %>" Width="98px"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 628px; height: 25px; text-align:right;" valign="top">
                    <img class="imageRightBack" /></td>
            </tr>
            <tr class="headLine"> 
                <td class="tdBackColor" style="background-color:White;" colspan="4">
                </td>
             </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" style="width:710px; height:352px" >
            <tr >
            <td class="tdBackColor" valign="top" style="width:30%">
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
                                    <asp:BoundField DataField="ShopTypeID" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopType %>">
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
          <td class="tdBackColor" valign="top" style="text-align: right; width:30%">
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
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label20" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>"></asp:Label></td>
                           <td style="width: 329px;" class="tdBackColor">
                               <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" Width="128px"></asp:TextBox></td>
                       </tr>
                       <tr><td style="TEXT-ALIGN: right; width: 2239px;" class="tdBackColor"><asp:Label id="Label50" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 21px" class="tdBackColor"><asp:TextBox id="txtShopName" runat="server" CssClass="ipt160px" Width="128px"></asp:TextBox></td></tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label21" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label></td>
                           <td style="" class="tdBackColor">
                               <asp:DropDownList ID="DDownListShopType" runat="server" Width="133px">
                               </asp:DropDownList></td>
                       </tr>
                     <tr>
                         <td class="tdBackColor" style="width: 2239px; text-align: right">
                            <asp:Label id="Label23" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>" CssClass="labelStyle"></asp:Label></td>
                         <td class="tdBackColor">
                             &nbsp;<asp:TextBox id="txtRentArea" runat="server" CssClass="ipt160px" Width="128px"></asp:TextBox></td>
                     </tr>
                       <tr><td style="TEXT-ALIGN: right; width: 2239px;" class="tdBackColor"><asp:Label id="Label57" runat="server" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaName %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 23px" class="tdBackColor">
                           <asp:DropDownList ID="DDownListAreaName" runat="server" Width="134px">
                           </asp:DropDownList></td></tr>
                       <tr><td style="TEXT-ALIGN: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label60" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td><td style="WIDTH: 329px; HEIGHT: 21px" class="tdBackColor">
                               <asp:TextBox ID="txtShopStartDate" onclick="calendar()" runat="server" CssClass="ipt160px" Width="129px"></asp:TextBox></td></tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label61" runat="server" Text="<%$ Resources:BaseInfo,LblDate_EndDate %>"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:TextBox ID="txtShopEndDate" onclick="calendar()" runat="server" CssClass="ipt160px" Width="129px"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label63" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:TextBox ID="txtContactName" runat="server" CssClass="ipt160px" Width="131px"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="text-align: right; width: 2239px;" class="tdBackColor">
                               <asp:Label ID="Label64" runat="server" Text="<%$ Resources:BaseInfo,Dept_lblTel %>" CssClass="labelStyle"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:TextBox ID="txtContactTel" runat="server" CssClass="ipt160px" Width="131px"></asp:TextBox></td>
                       </tr>
            </table>
            </td>
            <td class="tdBackColor" valign="top" style="width:40%">
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
                          <asp:DropDownList id="DDownListBrand" runat="server" Width="134px">
                               </asp:DropDownList></td>
                  </tr>
                       <tr>
                           <td style="text-align: right" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label24" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblBuilding %>"></asp:Label></td>
                           <td  class="tdBackColor">
                               <asp:DropDownList ID="DDownListBuilding" runat="server" Width="133px" OnSelectedIndexChanged="DDownListBuilding_SelectedIndexChanged" AutoPostBack="True">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style="TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label25" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>" CssClass="labelStyle"></asp:Label></td><td style="HEIGHT: 21px; width: 347px;" class="tdBackColor"><asp:DropDownList id="DDownListFloors" runat="server" Width="133px" AutoPostBack="True" OnSelectedIndexChanged="DDownListFloors_SelectedIndexChanged">
                               </asp:DropDownList></td></tr>
                       <tr>
                           <td style=" text-align:right;" class="tdBackColor">
                               <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationName %>"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:DropDownList ID="DDownListLocation" runat="server" Width="133px" OnSelectedIndexChanged="DDownListLocation_SelectedIndexChanged" AutoPostBack="True">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style=" TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label31" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblSelectUnit %>" CssClass="labelStyle"></asp:Label></td><td style="HEIGHT: 23px; width: 347px;" class="tdBackColor"><asp:DropDownList id="DDownListUnit" runat="server" Width="133px">
                               </asp:DropDownList></td></tr>
                       <tr><td style="TEXT-ALIGN: right" class="tdBackColor"></td><td class="tdBackColor">
                               <asp:Button ID="IBtnUnitsDel" runat="server" CssClass="buttonClear" Height="31px"
                                   OnClick="IBtnUnitsDel_Click" Text="<%$ Resources:BaseInfo,Btn_Del %>" Width="69px" Enabled="False" />
                               <asp:Button ID="IBtnUnitsAdd" runat="server" CssClass="buttonSave" Height="31px"
                                   OnClick="IBtnUnitsAdd_Click" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="64px" Enabled="False" />
                           </td></tr>
                       <tr>
                           <td style="text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label34" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblNoeUnitCollect %>"></asp:Label></td>
                           <td class="tdBackColor" rowspan="3" style="width: 347px">
                               <asp:ListBox ID="ListBoxUnits" runat="server" Width="137px" Height="94px"></asp:ListBox></td>
                       </tr>
                       <tr>
                           <td style=" text-align: right;" class="tdBackColor">
                               <asp:Label ID="Label35" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblComm %>"></asp:Label></td>
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
                    <td class="tdBackColor" style="width: 30%" valign="top">
                    </td>
                    <td class="tdBackColor" style="width: 30%; text-align: right" valign="top">
                    </td>
                    <td class="tdBackColor" style="width: 40%; text-align: right; padding-right:20px;" valign="top">
                               </td>
                </tr>
            </table>
                 
           
       </div>
       
        <div id="btn">
                        <table style="width:270px; height:40px" border="0" cellpadding="0" cellspacing="0" >
                            <tr>
                                <td style="width:16px; background-image: url(../images/left.gif);"></td>
                                <td style="width:725px; background-color: #e1e0b2;">
                                   <asp:Button ID="btnDispose" runat="server" CssClass="buttonClear" Height="32px"
                        OnClick="btnDispose_Click" Text="<%$ Resources:BaseInfo,CustPalaver_butOverrule %>" Width="70px" /><asp:Button
                            ID="btnTempSave" runat="server" CssClass="buttonCancel" Height="31px" OnClick="btnTempSave_Click"
                            Text="<%$ Resources:BaseInfo,CustPalaver_butConsent %>" Width="69px" /><asp:Button
                                ID="btnPutIn" runat="server" CssClass="buttonSave" Height="33px" OnClick="btnPutIn_Click"
                                Text="<%$ Resources:BaseInfo,PotCustomer_btnExamine %>" Width="93px" Visible="False" />
                        </td>
                                <td style="width:16px; background-image: url(../images/right.gif);"></td>
                            </tr>
                        </table>
                    </div>      
  
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                <td style="width: 60px; height: 401px; text-align: center" valign="top">
                    </td>
            </tr>
        </table>
          <div id="chooseDiv">
            <table>
                <tr>
                    <td class="selectedtab" id="tab1" onclick="chooseCard(0);" style="height: 21px; width:100px;">
                        <span id="tabpage1"><asp:Label ID="Label40" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblContractBase %>"></asp:Label></span></td>
                    <td class="tab" id="tab2" onclick="chooseCard(1);" style="height: 21px; width: 100px;">
                        <span id="Span1"><asp:Label ID="Label42" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblFormul %>"></asp:Label></span></td>
                    <td class="tab" id="tab3" onclick="chooseCard(2);" style="height: 21px; width: 100px;">
                        <span id="Span2"><asp:Label ID="Label43" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblUnionItem %>"></asp:Label></span></td>
                    <td class="tab" id="tab4" onclick="chooseCard(3);" style="height: 21px; width: 100px;">
                        <span id="Span3"><asp:Label ID="Label47" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblShopBaseInfo %>"></asp:Label></span></td>
                </tr>
            </table>
        </div>
         <br />
    </form>
</body>
</html>
