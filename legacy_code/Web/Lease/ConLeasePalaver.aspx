<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConLeasePalaver.aspx.cs" enableEventValidation="false" Inherits="Lease_ConLeasePalaver" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    
        <style type="text/css">
        <!--
        
        table.tblBase tr{ height:30px; }
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        table.tblBase tr.colLine{ height:10px; }
        
        table.tblBase td.baseLable{ padding-right:10px;text-align:right;}
        table.tblBase td.baseInput{ align:left;padding-right:20px }
        
        div#chooseDiv { position: absolute; top: 405px; left: 36px; height: 55px;
            width: 320px; overflow: auto;}
        -->
        
         <!--
        table.tblLeft tr{ height:15px;}
        table.tblLeft tr.headLine{ height:1px; }
        table.tblLeft tr.colLine{ height:10px; }
        table.tblLeft tr.bodyLine{ height:1px; }
        table.tblLeft tr.topLine{ height:25px; }
        table.tblLeft tr.gridView{ height:200px; }
        table.tblLeft td.leftLable{ padding-right:5px;text-align:right;}
        
        table.tblFast tr{ height:28px;}
        -->
        
        <!--
            talbe.tblLease tr{ height:30px;}
            table.tblLease tr.bodyLine{ height:1px;}
        -->
        
          <!--
            talbe.baseShop tr{ height:50px;}
            table.baseShop tr.headLine{ height:1px; }
            table.baseShop tr.bodyLine{ height:1px;}
            
            div#btn{ position: absolute; top: 407px; left: 477px; height: overflow: auto;}
        -->
        
        
    </style>
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	<script type="text/javascript">
	<!--    
	    
	    function chooseCard(id)
	    {
	        if(id==0){
		        document.getElementById("BaseBargain").style.display ="block";
		        document.getElementById("Expression").style.display ="none";
		        document.getElementById("LeaseItem").style.display ="none";
		        document.getElementById("ShopBaseInfo").style.display ="none";
		        
		        document.getElementById("BaseBargain").style.position="absolute";
		        document.getElementById("BaseBargain").style.top ="20px";
		        document.getElementById("Expression").style.position="absolute";
		        document.getElementById("Expression").style.top ="20px";
		        document.getElementById("LeaseItem").style.position="absolute";
		        document.getElementById("LeaseItem").style.top ="20px";
		        document.getElementById("ShopBaseInfo").style.position="absolute";
		        document.getElementById("ShopBaseInfo").style.top ="20px";
		        
		        
		        document.getElementById("tab4").className="tab";
		        document.getElementById("tab3").className="tab";
		        document.getElementById("tab2").className="tab";
		        document.getElementById("tab1").className="selectedtab";
	        }
	        if(id==1){	
		        document.getElementById("BaseBargain").style.display ="none";
		        document.getElementById("Expression").style.display ="block";
		        document.getElementById("LeaseItem").style.display ="none";
		        document.getElementById("ShopBaseInfo").style.display ="none";
		        
		         document.getElementById("BaseBargain").style.position="absolute";
		        document.getElementById("BaseBargain").style.top ="20px";
		        document.getElementById("Expression").style.position="absolute";
		        document.getElementById("Expression").style.top ="20px";
		        document.getElementById("LeaseItem").style.position="absolute";
		        document.getElementById("LeaseItem").style.top ="20px";
		        document.getElementById("ShopBaseInfo").style.position="absolute";
		        document.getElementById("ShopBaseInfo").style.top ="20px";
		        
		        document.getElementById("tab4").className="tab";
		        document.getElementById("tab3").className="tab";
		        document.getElementById("tab2").className="selectedtab";
		        document.getElementById("tab1").className="tab";
	        }
	        if(id==2){	
		        document.getElementById("BaseBargain").style.display ="none";
		        document.getElementById("Expression").style.display ="none";
		        document.getElementById("LeaseItem").style.display ="block";
		        document.getElementById("ShopBaseInfo").style.display ="none";
		        
		        document.getElementById("BaseBargain").style.position="absolute";
		        document.getElementById("BaseBargain").style.top ="20px";
		        document.getElementById("Expression").style.position="absolute";
		        document.getElementById("Expression").style.top ="20px";
		        document.getElementById("LeaseItem").style.position="absolute";
		        document.getElementById("LeaseItem").style.top ="20px";
		        document.getElementById("ShopBaseInfo").style.position="absolute";
		        document.getElementById("ShopBaseInfo").style.top ="20px";
		        
		        document.getElementById("tab4").className="tab";
		        document.getElementById("tab3").className="selectedtab";
		        document.getElementById("tab2").className="tab";
		        document.getElementById("tab1").className="tab";
		       
	        }
	        if(id==3){	
		        document.getElementById("BaseBargain").style.display ="none";
		        document.getElementById("Expression").style.display ="none";
		        document.getElementById("LeaseItem").style.display ="none";
		        document.getElementById("ShopBaseInfo").style.display ="block";
		        
		        document.getElementById("BaseBargain").style.position="absolute";
		        document.getElementById("BaseBargain").style.top ="20px";
		        document.getElementById("Expression").style.position="absolute";
		        document.getElementById("Expression").style.top ="20px";
		        document.getElementById("LeaseItem").style.position="absolute";
		        document.getElementById("LeaseItem").style.top ="20px";
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
        
        function ListBoxValidator(sForm)
        {
            if(isEmpty(document.all.listBoxRemark.value))  
            {
                alert("请输入驳回意见!");
                document.all.listBoxRemark.focus();
                return false;					
            }
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
                    <div id="BaseBargain">
    <table border="0" cellpadding="0" cellspacing="0" style="height: 386px; width: 710px;">
        <tr>
            <td style="width: 434px; height: 24px; text-align: left;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,LeaseBargain_lblPostil %>" Width="335px"></asp:Label></td>
            <td style="width: 562px; height: 24px;" class="tdTopRightBackColor" align="left"></td>
            <td style="width: 7px; height: 24px;" class="tdTopRightBackColor" valign="top">
                <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
        </tr>
        <tr>
            <td class="tdBackColor" colspan="3" style="width: 710px; height: 339px;"
                    valign="top">
                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    
                    <tr class="headLine">
                        <td style="background-color:White; height:1" colspan="4"　width="710px">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:710px; height:10px;" colspan="4" class="tdBackColor">
                        </td>
                    </tr>
                    
                    <tr>
                    <!--  *********left
                    -->
                    <td width="50%" valign="top">
                    
                <table class="tblBase" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:8px">
                        <td style="width:118px;">
                        </td>
                        <td style="width:164px;">
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #738495;" >
                                    </td>
                                </tr>
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #FFFFFF;">
                                    </td>
                                </tr>
                            </table>
                        </td>
    </tr>
    
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
    </tr>
      <tr style="height:20px">
    <td colspan="2"></td>
    </tr>
        <tr style="height:8px">
            <td>
            </td>
            <td style="width:164px;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #738495;">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #FFFFFF;">
                        </td>
                    </tr>
                </table>
            </td>
    </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"></asp:Label></td>
        <td style="height:18px;">
            <asp:DropDownList ID="cmbTradeID" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
            </asp:DropDownList></td>
    </tr>
        <tr>
            <td class="baseLable">
                <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label></td>
            <td>
                <asp:TextBox ID="cmbContractStatus" runat="server" CssClass="ipt160px" ReadOnly="True" BackColor="#F5F5F4"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="baseLable" style="height: 19px">
            <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label></td>
            <td style="height: 19px">
            <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="baseLable" style="height: 28px">
            <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label></td>
            <td style="height: 28px">
            <asp:TextBox ID="txtRefID" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
        </tr>
    <tr>
        <td class="baseLable" style="height: 28px">
            <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
        <td style="height: 28px">
            <asp:TextBox ID="txtConStartDate" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
        <tr>
            <td class="baseLable">
                <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtConEndDate" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
        </tr>
                </table>
                    </td>
                    
                    <!--  *********right
                    -->
                    <td width="50%"  valign="top">
                    
                <table class="tblBase" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:8px">
                        <td style="width:136px;">
                        </td>
                        <td class="baseInput">
                            <table width="165px" border="0" cellpadding="0" cellspacing="0">
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #738495;">
                                    </td>
                                </tr>
                                <tr class="bodyLine">
                                    <td style="height: 1px; background-color: #FFFFFF;" >
                                    </td>
                                </tr>
                              </table>
                       </td>
                    </tr>
    <tr>
        <td class="baseLable" style="width: 136px">
            <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>"></asp:Label></td>
        <td class="baseInput">
            <asp:TextBox ID="txtChargeStart" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
                    <tr>
                        <td class="baseLable" style="width: 136px">
                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblNorentDays %>"  CssClass="labelStyle"></asp:Label>
                        </td>
                        <td class="baseInput">
                            <asp:TextBox ID="txtNorentDays" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
                    </tr>
    <tr>
        <td class="baseLable" style="width: 136px">
            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labEConURL %>" CssClass="labelStyle"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtBargain" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
      <tr style="height:20px">
    <td colspan="2"></td>
    </tr>
        <tr style="height:8px">
            <td style="width: 136px">
            </td>
            <td class="baseInput">
                <table width="165px" border="0" cellpadding="0" cellspacing="0">
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #738495;">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #FFFFFF;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
                   
                    <tr>
                        <td class="baseLable" style="width: 136px">
                        <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>"></asp:Label>
                        </td>
                        <td >
                            <asp:DropDownList ID="DDownListTerm" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="baseLable" style="width: 136px">
                        <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>" Width="99px"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="DDownListPenalty" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                </asp:DropDownList></td>
                    </tr>
                    <tr style="height:10px">
    <td colspan="2"></td>
    </tr>
        <tr style="height:5px">
            <td style="width: 136px; height: 5px;">
            </td>
            <td class="baseInput" style="height: 5px">
                <table width="165px" border="0" cellpadding="0" cellspacing="0">
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #738495;">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #FFFFFF;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
              <tr style="height: 8px">
                        <td class="baseLable" style="width: 136px;">
                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblCommOper %>" CssClass="labelStyle"></asp:Label>
                        </td>
                        <td class="baseInput" style="height: 8px">
                            <asp:TextBox ID="txtCommOper" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
                    </tr>       
    <tr style="height:10px">
    <td colspan="2"></td>
    </tr>
        <tr style="height:5px">
            <td style="width: 136px">
            </td>
            <td class="baseInput">
                <table width="165px" border="0" cellpadding="0" cellspacing="0">
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #738495;">
                        </td>
                    </tr>
                    <tr class="bodyLine">
                        <td style="height: 1px; background-color: #FFFFFF;">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    <tr>
        <td class="baseLable" style="width: 136px">
            <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"></asp:Label></td>
        <td rowspan="2" valign="top">
            <asp:TextBox ID="listBoxRemark" runat="server" Height="50px" CssClass="ipt160px"></asp:TextBox></td>
    </tr>
        <tr>
            <td style="width: 136px">
            </td>
        </tr>
                </table>
                    
                    </td>
                    
                    </tr>
                    </table>
                    
            </td>
        </tr>
 </table>
 
  </div>
  <div id="Expression">
 <table border="0" cellpadding="0" cellspacing="0" width="710px">
            <tr>
            <!--********************left**********************-->
                <td class="tdBackColor" colspan="2" style="text-align:center;width:320px; height: 370px;" valign="top">
        
                <table class="tblLeft" width="100%" border="0" cellpadding="0" cellspacing="0" >
     <tr class="topLine">
       <td class="tdTopBackColor" style="width: 293px;" colspan="3">
                    <img alt="" class="imageLeftBack" /><asp:Label ID="Label4" runat="server"
                        Text="<%$ Resources:BaseInfo,Formula_lblContent %>" Width="199px"></asp:Label></td>
       <td class="tdTopBackColor" valign="top">
                    <img  align="right" class="imageRightBack" style="width: 6px;"/>
                </td> 
    </tr>
    <tr class="headLine" >
        <td style="background-color:White;" colspan="4"></td> 
    </tr>
    <tr class="colLine">
        <td colspan="4" valign="middle">
            <table border="0" cellpadding="0" cellspacing="0" width="280">
                <tr class="bodyLine">
                    <td style="width:280px; height:1px; background-color:#738495;" class="tdBackColor"></td>
                </tr>
                <tr class="bodyLine">
                    <td style="width:280px; height:1px; background-color:#FFFFFF;" class="tdBackColor"></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="leftLable" align="left" style="padding-left:5px">
            <asp:Label ID="labChargeTypeID" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>" Width="57px"></asp:Label></td>   
        <td align="left">
            <asp:DropDownList ID="cmbChargeTypeID" runat="server" CssClass="textBoxStyle" Width="76px" BackColor="#F5F5F4" Enabled="False">
            </asp:DropDownList></td> 
        <td class="leftLable" style="width: 74px">
            <asp:Label ID="labFormulaType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFormulaType %>" Width="60px"></asp:Label></td> 
        <td align="left" width="100" style="padding-right:5px">
            <asp:DropDownList ID="cmbFormulaType" runat="server" CssClass="textBoxStyle" Width="76px" BackColor="#F5F5F4" Enabled="False">
            </asp:DropDownList></td> 
    </tr>
    <tr>
        <td class="leftLable">
            <asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td> 
        <td align="left">
            <asp:TextBox ID="txtBeginDate"　onclick="calendar()" runat="server" Width="70px" BackColor="#F5F5F4" ReadOnly="True" Enabled="False"></asp:TextBox></td> 
        <td class="leftLable" style="width: 74px" >
            <asp:Label ID="labEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labEndDate %>"></asp:Label></td> 
        <td align="left" width="100">
            <asp:TextBox ID="txtOverDate" onclick="calendar()" runat="server" CssClass="textBoxStyle" Width="70px" BackColor="#F5F5F4" ReadOnly="True" Enabled="False"></asp:TextBox></td> 
    </tr>
    <tr>
        <td class="leftLable">
            <asp:Label ID="labBaseAmt" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labBaseAmt %>"></asp:Label></td> 
        <td colspan="3" align="left">
            <asp:TextBox ID="txtBaseAmt" runat="server" CssClass="textBoxStyle" Width="70px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td> 
    </tr>
    <tr height="20">
        <td colspan="4" valign="middle">
            <table border="0" cellpadding="0" cellspacing="0" width="280">
                <tr class="bodyLine">
                    <td style="width: 280px;background-color: #738495;" class="tdBackColor">
                    </td>
                </tr>
                <tr class="bodyLine">
                    <td style="width: 280px;background-color: #FFFFFF;" class="tdBackColor">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr class="gridView">
        <td style="width:300px; text-align:center;" colspan="7" class="tdBackColor" valign="top">
            <asp:GridView ID="GVType" runat="server" AutoGenerateColumns="False" Height="150px"
                Width="280px" OnRowCommand="GVType_RowCommand" OnRowDataBound="GVType_RowDataBound" BackColor="White" BorderColor="#E1E0B2">
                <Columns>
                    <asp:BoundField DataField="FormulaID" >
                        <ItemStyle CssClass="hidden" Font-Size="12px" />
                        <HeaderStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ChargeTypeName" HeaderText="<%$ Resources:BaseInfo,ConLease_labChargeTypeID %>" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FStartDate" HeaderText="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>" DataFormatString="{0:D}" HtmlEncode="False" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FEndDate" HeaderText="<%$ Resources:BaseInfo,ConLease_labEndDate %>" DataFormatString="{0:D}" HtmlEncode="False" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FormulaTypeName" HeaderText="<%$ Resources:BaseInfo,ConLease_labFormulaType %>" >
                        <HeaderStyle CssClass="gridviewtitle" Font-Size="12px" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle Font-Size="12px" BorderColor="#E1E0B2" />
                    </asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
            </asp:GridView>
        </td> 
    </tr>
    <tr height="10px">
        <td colspan="4" valign="middle">
            <table border="0" cellpadding="0" cellspacing="0" width="280px">
                <tr class="bodyLine">
                    <td style="background-color: #738495;" class="tdBackColor">
                    </td>
                </tr>
                <tr class="bodyLine">
                    <td style="background-color: #FFFFFF;" class="tdBackColor">
                    </td>
                </tr>
            </table>
        </td> 
    </tr>
    <tr>
        <td style="width:70px;text-align:center;">
            </td> 
        <td style="width:70px;text-align:center;">
            </td> 
        <td style="width:74px;text-align:center;">
            </td>
        <td style="width:70px; text-align:center;">
            </td> 
    </tr>
</table>
                    <input id="Hidden1" runat="server" type="hidden" style="width: 16px; height: 1px;" />
                    <input id="Hidden2" runat="server" type="hidden" style="width: 8px; height: 1px;" />&nbsp;
                    <asp:TextBox ID="TextBox1" runat="server" AutoPostBack="True" Visible="False" Width="12px" Height="1px"></asp:TextBox>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Height="1px" Width="1px"></asp:LinkButton></td>
                <!--*****************middle***********************
                -->
                <td style="width:10px; height: 370px; padding-left:6px"></td> 
                <!--************************right********************-->
                <td style="text-align: center; width:420px; height: 370px;" valign="top">
        <table border="0" cellpadding="0" cellspacing="0"
            width="100%" class="tdBackColor">
            <tr class="topLine">
                <td class="tdTopBackColor" style="width: 393px;">
                    <img alt="" class="imageLeftBack" /><asp:Label ID="Label43" runat="server"
                        Text="<%$ Resources:BaseInfo,Formula_lblContent %>" Width="293px"></asp:Label></td>
                <td class="tdTopRightBackColor" style="width: 7px;; text-align: right">
                    <img class="imageRightBack" />
                </td>
            </tr>
            <tr class="headLine">
                <td colspan="2" style="background-color: white;" >
                </td>
            </tr>
            <tr height="100">
                <td colspan="2">
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 390px; height: 60px;">
                                    <legend style="text-align: left">
                                        <asp:Label ID="labFastnessHire" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFastnessHire %>"></asp:Label>
                                    </legend>
                                    <table class="tblFast" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="5"></td>
                                            <td style="width: 107px;" colspan=2>
                                                <asp:RadioButton ID="rabMonthHire" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMonthHire %>"
                                                    TextAlign="Left" CssClass="labelStyle" GroupName="b" Width="65px" Enabled="False" />
                                                </td>
                                            <td width="240" style="padding-left:25px" colspan=3 align="left">
                                                <asp:RadioButton ID="rabDayHire" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabDayHire %>"
                                                    TextAlign="Left" CssClass="labelStyle" GroupName="b" Enabled="False" /></td>
                                        </tr>
                                        <tr>
                                            <td width="10"></td>
                                            <td style="width: 60px;" align="right">
                                                <asp:Label ID="lblRentArea" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>"
                                                    Width="60px"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 50px;">
                                                <asp:TextBox ID="txtArea" runat="server" CssClass="textBoxStyle" Width="50px" ReadOnly="True" BackColor="#F5F5F4"></asp:TextBox></td>
                                            <td style="padding-left:5px;">
                                                <asp:Label ID="labUnitHire" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labUnitHire %>"
                                                    Width="60px"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 50px;">
                                                <asp:TextBox ID="txtUnitHire" runat="server" CssClass="textBoxStyle" Width="50px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                                            <td style="padding-left:5px;">
                                                <asp:Label ID="labFixedRental" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labFixedRental %>"
                                                    Width="45px"></asp:Label></td>
                                            <td class="tdBackColor" style="width: 85px;" align="left">
                                                <asp:TextBox ID="txtFixedRental" runat="server" CssClass="textBoxStyle" Width="85px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                                        </tr>
                                    </table>
                                </fieldset>
                </td>
            </tr>
            <tr>
                <td colspan="2" valign="top" style="height: 257px">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="tdBackColor">
                        <tr>
                            <td valign="top" style="padding-left:10px; padding-right:10px">
                                <fieldset style="border-right: #c0c0c0 1px solid; border-top: #c0c0c0 1px solid;
                                    border-left: #c0c0c0 1px solid; border-bottom: #c0c0c0 1px solid; width: 390px;">
                                    <legend style="text-align: left">
                                    <asp:Label ID="Label33" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConLease_labDeductAndKeep %>"></asp:Label>
                                    </legend>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="50%" valign="top">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%" style=" padding-left:5px;">
                                                    <tr height="30">
                                                        <td class="tdBackColor" colspan="4" align="left">
                                                            <asp:RadioButton ID="rabFastness" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabFastness %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" Enabled="False" />
                                                            <asp:RadioButton ID="rabMonopole" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMonopole %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" Enabled="False" />
                                                            <asp:RadioButton ID="rabMultilevel" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabMultilevel %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="a" Enabled="False" /></td>
                                                    </tr>
                                                    <tr height="130" >
                                                        <td class="tdBackColor" colspan="4" style="text-align: center"
                                                            valign="top">
                                                            <asp:GridView ID="GVDeductMoney" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" OnRowDataBound="GVDeductMoney_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="FormulaID" Visible="False" />
                                                                    <asp:BoundField DataField="SalesTo" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Pcent" HeaderText="<%$ Resources:BaseInfo,ConLease_labDistill %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
                                                            </asp:GridView></td>
                                                    </tr>
                                                    <tr >
                                                        <td align="left" style=" padding-left:5px;width:10px; padding-right:3px;">
                                                            </td>
                                                        <td style="width: 57px">
                                                            </td>
                                                        <td style="width: 45px">
                                                            </td>
                                                        <td width="38" align="left">
                                                            </td>
                                                    </tr>
                                                    <tr >
                                                        <td style="height: 9px;" colspan="4">
                                                            <input id="HiddenDeduct1" runat="server" style="width: 3px; height: 4px" type="hidden" />
                                                            <input id="HiddenDeduct2" runat="server" style="width: 7px; height: 5px" type="hidden" />
                                                            <asp:Button ID="btnDeduct" runat="server" Height="4px" Text="Button" Width="17px" Visible="False" OnClick="btnDeduct_Click" /></td>
                                                    </tr>
                                                    <tr class="colLine">
                                                        <td colspan="4" align="center">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr class="bodyLine">
                                                                    <td style="width: 175px; height: 1px; background-color: #738495;">
                                                                    </td>
                                                                </tr>
                                                                <tr class="bodyLine">
                                                                    <td style="width: 175px; height: 1px; background-color: #FFFFFF;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" width="50%" colspan=2>
                                                            &nbsp;</td>
                                                        <td align="center" style="width: 45px">
                                                            &nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <!--*********************-->
                                            <td width="50%" valign="top">
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr height="30">
                                                        <td class="tdBackColor" colspan="3">
                                                            <asp:RadioButton ID="rabFastness2" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabFastness %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="c" Enabled="False" />
                                                            <asp:RadioButton ID="rabLevel" runat="server" Text="<%$ Resources:BaseInfo,ConLease_rabLevel %>"
                                                                TextAlign="Left" CssClass="labelStyle" GroupName="c" Enabled="False" /></td>
                                                    </tr>
                                                    <tr height="130">
                                                        <td class="tdBackColor" colspan="3" style=" text-align: center"
                                                            valign="top">
                                                            <asp:GridView ID="GVKeepMin" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#E1E0B2" OnRowDataBound="GVKeepMin_RowDataBound">
                                                                <Columns>
                                                                    <asp:BoundField DataField="FormulaID" Visible="False" />
                                                                    <asp:BoundField DataField="SalesTo" HeaderText="<%$ Resources:BaseInfo,ConLease_labSellCount %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="MinSum" HeaderText="<%$ Resources:BaseInfo,ConLease_LalKeepMin %>" >
                                                                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                                                        <ItemStyle BorderColor="#E1E0B2" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <HeaderStyle BackColor="#E1E0B2" Height="10px" Font-Bold="False" Font-Size="50px"  />
                                                            </asp:GridView></td>
                                                    </tr>
                                                    <tr>
                                                        <td >
                                                            </td>
                                                        <td >
                                                            </td>
                                                        <td>
                                                            </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 9px;" colspan="5" class="tdBackColor">
                                                            <input id="HiddenKeepMin1" style="width: 2px; height: 4px" type="hidden" runat="server" />
                                                            <input id="HiddenKeepMin2" style="width: 4px; height: 5px" type="hidden" runat="server" />
                                                            <asp:Button ID="btnKeepMin" runat="server" Height="4px" Text="Button" Width="19px" Visible="False" OnClick="btnKeepMin_Click" /></td>
                                                    </tr>
                                                    <tr class="colLine">
                                                        <td colspan="5" align="center">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr class="bodyLine">
                                                                    <td style="width: 175px; height: 1px; background-color: #738495;">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 175px; height: 1px; background-color: #FFFFFF;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%" colspan="2" >       </td>
                                                        <td width="50%">
                                                           </td>
                                                      
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        
                                                    <tr height="8"></tr>
                                    </table>
                                </fieldset>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
                </td>
            </tr>
        </table>
    </div>
       
        <div id="LeaseItem">

     <table border="0" cellpadding="0" cellspacing="0" style="height: 25px" width="710">
            <tr>
                <td class="tdTopBackColor" style="width: 445px; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" />
                    <asp:Label ID="Label49" runat="server" Text="<%$ Resources:BaseInfo,LeaseBargain_lblPostil %>" Width="367px"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 628px; height: 25px; text-align:right;" valign="top">
                    <img class="imageRightBack" /></td>
            </tr>
            </table>
            
            <table class="tblLease" style="width:710px; height:359px" border="0" cellpadding="0" cellspacing="0">
                <tr class="headLine">
                    <td class="tdBackColor" style="background-color:White;" colspan="4">
                    </td>
                </tr>
                <tr>
                    <td class="tdBackColor" style="width: 118px; text-align: right" colspan="4">
                    </td>
                </tr>
             <tr class="bodyLine">
                 <td class="tdBackColor" style="width: 163px; height: 19px; text-align: right">
                     </td>
                 <td class="tdBackColor" style="width: 182px; height: 19px" valign="bottom">
                 <table style="width:165px;" border="0" cellpadding="0" cellspacing="0">
                     <tr class="bodyLine">
                         <td
                              style="height: 1px; background-color: #738495">
                         </td>
                     </tr>
                     <tr class="bodyLine">
                         <td style="height: 1px; background-color: #ffffff"></td>
                     </tr>
                 </table>
                 </td>
                 <td class="tdBackColor" style="width: 163px; height:20px">
                 </td>
                 <td class="tdBackColor" style="width: 165px" valign="bottom" align="left">
                     <table style="width:165px;" border="0" cellpadding="0" cellspacing="0">
                         <tr class="bodyLine">
                             <td 
                                  style="height: 1px; background-color: #738495">
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
                    <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label36" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labBillCycle %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor">
                     <asp:DropDownList ID="DDownListBillCycle" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                    <td class="tdBackColor" align="right" style="padding-right:5px">
                     <asp:Label ID="Label47" runat="server" Text="<%$ Resources:BaseInfo,ConLease_lblTaxFrank %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" >
                        <asp:TextBox ID="txtTaxRate" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                </tr >
                <tr >
                    <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label41" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labCurrencyType %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" style="width: 182px; height: 23px">
                     <asp:DropDownList ID="DDownListCurrencyType" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                    <td class="tdBackColor" align="right" style="padding-right:5px">
                     <asp:Label ID="Label48" runat="server" Text="<%$ Resources:BaseInfo,ConLease_InvoiceType %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" style="width: 262px">
                     <asp:DropDownList ID="DDownListTaxType" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                </tr>
                <tr >
                    <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label37" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labSettleMode %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" style="width: 182px; height: 23px">
                     <asp:DropDownList ID="DDownListSettleMode" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                    <td class="tdBackColor" style="width: 107px; text-align: right">
                    </td>
                    <td class="tdBackColor" style="width: 262px">
                    </td>
                </tr>
                <tr >
                    <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label38" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labMonthSettleDays %>" CssClass="labelStyle"></asp:Label></td>
                    <td class="tdBackColor" style="width: 182px; height: 23px">
                        <asp:TextBox ID="txtMonthSettleDays" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                    <td class="tdBackColor" style="width: 107px; text-align: right">
                    </td>
                    <td class="tdBackColor" style="width: 262px">
                    </td>
                </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label39" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labYesNoMin %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 23px;">
                     <asp:DropDownList ID="DDownListIfPrepay" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList>
                 </td>
                 <td class="tdBackColor" style="width: 107px; text-align: right">
                 </td>
                 <td class="tdBackColor" style="width: 262px">
                 </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label40" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labFirstAccountMonth %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 24px;">
                     <asp:TextBox ID="txtBalanceMonth" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                 <td class="tdBackColor" style="width: 107px; text-align: right; height: 24px;">
                     </td>
                 <td class="tdBackColor" rowspan="4" valign="top" style="width: 262px">
                     </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="width: 163px; height: 15px; text-align: right">
                     </td>
                 <td class="tdBackColor" style="width: 182px; height: 19px" valign="bottom">
                 <table style="width:165px;" border="0" cellpadding="0" cellspacing="0">
                     <tr class="bodyLine">
                         <td
                              style="height: 1px; background-color: #738495">
                         </td>
                     </tr>
                     <tr class="bodyLine">
                         <td style="height: 1px; background-color: #ffffff"></td>
                     </tr>
                 </table>
                 </td>
                 <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                 </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label44" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPayTypeID %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 19px">
                     <asp:DropDownList ID="DDownListPayTypeId" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                     </asp:DropDownList></td>
                 <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                 </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px;" align="right">
                     <asp:Label ID="Label45" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labLatePayInt %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 19px">
                     <asp:TextBox ID="txtLatePayInt" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                 <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                 </td>
             </tr>
             <tr >
                 <td class="tdBackColor" style="padding-right:5px; width: 163px; height: 26px;" align="right">
                     <asp:Label ID="Label46" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labIntDay %>" CssClass="labelStyle"></asp:Label></td>
                 <td class="tdBackColor" style="width: 182px; height: 26px">
                     <asp:TextBox ID="txtIntDay" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
                 <td class="tdBackColor" style="width: 107px; height: 26px; text-align: right">
                 </td>
                 <td class="tdBackColor" style="height: 26px; width: 262px;">
                 </td>
             </tr>
                <tr >
                    <td class="tdBackColor" colspan="2" style="height: 19px">
                        </td>
                    <td class="tdBackColor" style="width: 107px; height: 19px; text-align: right">
                    </td>
                    <td class="tdBackColor" style="height: 19px; width: 262px;">
                    </td>
                </tr>
            </table>
         

        </div>
     <div id="ShopBaseInfo" >
  
          <table class="baseShop" border="0" cellpadding="0" cellspacing="0" style="height: 25px" width="710">
            <tr>
                <td class="tdTopBackColor" style="width: 430px; height: 25px" valign="top">
                    <img alt="" class="imageLeftBack" />
                    <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,LeaseBargain_lblPostil %>" Width="320px"></asp:Label></td>
                <td class="tdTopRightBackColor" colspan="2" style="width: 628px; height: 25px; text-align:right;" valign="top">
                    <img class="imageRightBack" /></td>
            </tr>
            <tr class="headLine"> 
                <td class="tdBackColor" style="background-color:White;" colspan="4">
                </td>
             </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" style="width:710px; height:358px" >
            <tr>
             <td class="tdBackColor" valign="top" style="width:30%">
                <table width="100%">
                    <tr style="height:25px">
                        <td colspan="3" style="text-align: center; width: 197px;">
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
                        <td colspan="3" style="text-align: center; width: 197px;">
                            <asp:GridView ID="gvShop" runat="server" AutoGenerateColumns="False" BackColor="White"
                                Width="181px" OnSelectedIndexChanged="gvShop_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="ShopId">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopName" HeaderText="商铺名称">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ShopType" HeaderText="商铺类型">
                                        <ItemStyle BorderColor="#E1E0B2" />
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                    </asp:BoundField>
                                    <asp:CommandField HeaderText="选择" ShowSelectButton="True">
                                        <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                                        <ItemStyle BorderColor="#E1E0B2" />
                                    </asp:CommandField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
          <td width="40%" class="tdBackColor" valign="top">
                 <table class="tdBackColor" >
                       <tr height="20">
                           <td style="width: 3945px;" ></td>
                           <td valign="bottom" style="width: 345px" align="right">
                               <table style="width:126px;" border="0" cellpadding="0" cellspacing="0">
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
                           <td style="width: 3945px; text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label14" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labShopCode %>"></asp:Label></td>
                           <td style="width: 345px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True" Width="128px"></asp:TextBox></td>
                       </tr>
                       <tr><td style="WIDTH: 3945px;  TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label50" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 345px; HEIGHT: 21px; text-align: right;" class="tdBackColor"><asp:TextBox id="txtShopName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True" Width="128px"></asp:TextBox></td></tr>
                       <tr>
                           <td style="width:3945px;  text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label53" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopType %>"></asp:Label></td>
                           <td style="width:345px; text-align: right;" class="tdBackColor">
                               <asp:DropDownList ID="DDownListShopType" runat="server" Width="132px" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style="WIDTH: 3945px; TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label56" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblMainBrand %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 345px; HEIGHT: 10px; text-align: right;" class="tdBackColor"><asp:DropDownList id="DDownListBrand" runat="server" Width="132px" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td></tr>
                     <tr>
                         <td class="tdBackColor" style="width: 3945px;text-align: right">
                            <asp:Label id="Label16" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblRentArea %>" CssClass="labelStyle"></asp:Label></td>
                         <td class="tdBackColor" style="width: 345px; text-align: right;">
                            <asp:TextBox id="txtRentArea" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True" Width="128px"></asp:TextBox></td>
                     </tr>
                       <tr><td style="WIDTH: 3945px;TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label57" runat="server" Text="<%$ Resources:BaseInfo,AreaVindicate_labAreaName %>" CssClass="labelStyle"></asp:Label></td><td style="WIDTH: 345px; HEIGHT: 23px; text-align: right;" class="tdBackColor">
                           <asp:DropDownList ID="DDownListAreaName" runat="server" Width="132px" BackColor="#F5F5F4" Enabled="False">
                           </asp:DropDownList></td></tr>
                       <tr><td style="WIDTH: 3945px;TEXT-ALIGN: right" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label60" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"></asp:Label></td><td style="WIDTH: 345px; HEIGHT: 21px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtStartDate" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False" Width="128px"></asp:TextBox></td></tr>
                       <tr>
                           <td style="width: 3945px;text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label61" runat="server" Text="<%$ Resources:BaseInfo,ConLease_labEndDate %>"></asp:Label></td>
                           <td style="width: 345px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtEndDate" onclick="calendar()" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False" Width="128px"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="width: 3945px; text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label63" runat="server" Text="<%$ Resources:BaseInfo,PotCustomer_lblContactorName %>"></asp:Label></td>
                           <td style="width: 345px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtContactName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False" Width="128px"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td style="width: 3945px; text-align: right" class="tdBackColor">
                               <asp:Label ID="Label64" runat="server" Text="<%$ Resources:BaseInfo,Dept_lblTel %>" CssClass="labelStyle"></asp:Label></td>
                           <td style="width: 345px; text-align: right;" class="tdBackColor">
                               <asp:TextBox ID="txtContactTel" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" Enabled="False" Width="128px"></asp:TextBox></td>
                       </tr>
                       <tr>
                           <td class="tdBackColor" style="width: 3945px; text-align: right">
                               </td>
                           <td class="tdBackColor" style="width: 345px;">
                               </td>
                       </tr>
                       <tr>
                           <td class="tdBackColor" colspan="2" >
                               </td>
                       </tr>
            </table>
            </td>
            <td width="30%" class="tdBackColor" valign="top">
              <table class="tdBackColor" style="width: 283px">
                       <tr height="20">
                           <td style="width: 98px;">
                           </td>
                           <td  valign="bottom">
                               <table style="width:165px;" border="0" cellpadding="0" cellspacing="0">
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
                           <td style="width: 98px;text-align: right" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label27" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblBuilding %>"></asp:Label></td>
                           <td class="tdBackColor">
                               <asp:DropDownList ID="DDownListBuilding" runat="server" Width="165px" OnSelectedIndexChanged="DDownListBuilding_SelectedIndexChanged" AutoPostBack="True" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td>
                       </tr>
                       <tr><td style="WIDTH: 98px;TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label28" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblFloorName %>" CssClass="labelStyle"></asp:Label></td><td style="HEIGHT: 21px" class="tdBackColor"><asp:DropDownList id="DDownListFloors" runat="server" Width="165px" AutoPostBack="True" OnSelectedIndexChanged="DDownListFloors_SelectedIndexChanged" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td></tr>
                       <tr>
                           <td style="width:98px;text-align:right;" class="tdBackColor">
                               <asp:Label ID="Label29" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblLocationName %>"></asp:Label></td>
                           <td style="height:21px;" class="tdBackColor">
                               <asp:DropDownList ID="DDownListLocation" runat="server" Width="165px" OnSelectedIndexChanged="DDownListLocation_SelectedIndexChanged" AutoPostBack="True" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td>
                       </tr>
                  <tr height = "30px">
                      <td style="width: 98px;">
                      </td>
                      <td valign="bottom">
                                <table style="width:166px;" border="0" cellpadding="0" cellspacing="0">
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
                       <tr><td style="WIDTH: 98px;TEXT-ALIGN: right" class="tdBackColor"><asp:Label id="Label30" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblSelectUnit %>" CssClass="labelStyle"></asp:Label></td><td style="HEIGHT: 23px" class="tdBackColor"><asp:DropDownList id="DDownListUnit" runat="server" Width="165px" BackColor="#F5F5F4" Enabled="False">
                               </asp:DropDownList></td></tr>
                       <tr><td style="WIDTH: 98px; TEXT-ALIGN: right" class="tdBackColor"></td><td style="HEIGHT: 21px; padding-left:20px" class="tdBackColor" align="left">
                               <asp:Button ID="IBtnUnitsDel" runat="server" CssClass="buttonClear" Height="31px"
                                   OnClick="IBtnUnitsDel_Click" Text="<%$ Resources:BaseInfo,Btn_Del %>" Width="70px" Enabled="False" />
                               <asp:Button ID="IBtnUnitsAdd" runat="server" CssClass="buttonSave" Height="31px"
                                   OnClick="IBtnUnitsAdd_Click" Text="<%$ Resources:BaseInfo,Dept_TitleAdd %>" Width="64px" Enabled="False" />
                           </td></tr>
                       <tr>
                           <td style="width: 98px; text-align: right;" class="tdBackColor">
                               <asp:Label CssClass="labelStyle" ID="Label34" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblNoeUnitCollect %>"></asp:Label></td>
                           <td  rowspan="4">
                               <asp:ListBox ID="ListBoxUnits" runat="server" Width="165px" Enabled="False" Height="130px"></asp:ListBox></td>
                       </tr>
                       <tr>
                           <td style="width: 98px;text-align: right;" >
                               <asp:Label ID="Label19" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblComm %>"></asp:Label></td>
                       </tr>
                       <tr>
                           <td  style="width: 98px; text-align: right">
                           </td>
                       </tr>
                       <tr>
                           <td style="width: 98px; text-align: right">
                           </td>
                       </tr>
                       <tr>
                           <td style="width:98px;  text-align: right;" class="tdBackColor">
                           </td>
                           <td class="tdBackColor">
                           </td>
                       </tr>
            </table>
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
                                Text="<%$ Resources:BaseInfo,Lease_NewLineBtnPutIn %>" Width="93px" Visible="False" />
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
                    <td class="selectedtab" id="tab1" onclick="chooseCard(0);" style="height: 21px; width: 80px;">
                        <span id="tabpage1"><asp:Label ID="Label22" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblContractBase %>"></asp:Label></span></td>
                    <td class="tab" id="tab2" onclick="chooseCard(1);" style="height: 21px; width: 80px;">
                        <span id="Span1"><asp:Label ID="Label23" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblFormul %>"></asp:Label></span></td>
                    <td class="tab" id="tab3" onclick="chooseCard(2);" style="height: 21px; width: 80px;">
                        <span id="Span2"><asp:Label ID="Label24" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblLeaseItem %>"></asp:Label></span></td>
                    <td class="tab" id="tab4" onclick="chooseCard(3);" style="height: 21px; width: 80px;">
                        <span id="Span3"><asp:Label ID="Label25" runat="server" Text="<%$ Resources:BaseInfo,Tab_lblShopBaseInfo %>"></asp:Label></span></td>
                </tr>
            </table>
        </div>
        &nbsp;&nbsp;
    </form>
</body>
</html>
