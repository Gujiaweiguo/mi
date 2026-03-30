<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConUnion.aspx.cs" Inherits="Lease_LeaseConUnion_ConUnion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasicContractInfo")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
      <style type="text/css">
        <!--
        
        table.tblBase tr{ height:28px; }
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        
        table.tblBase td.baseLable{ padding-right:5px;text-align:right;}
        table.tblBase td.baseInput{ align:left;padding-right:20px }
        
        -->
    
        
        
    </style>  
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js" charset="gb2312"> </script>
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool('<%= (String)GetGlobalResourceObject("BaseInfo", "Lease_lblBasicContractInfo")%>'+",Lease/LeaseConUnion/ConUnion.aspx~"+'<%= (String)GetGlobalResourceObject("BaseInfo", "Tab_lblLeaseBalanceItem")%>'+",Lease/LeaseConUnion/ConUnionItem.aspx~"+'<%= (String)GetGlobalResourceObject("BaseInfo", "Tab_lblShopInfo")%>'+",Lease/ShopBaseInfo.aspx~<%=espression %>,Lease/Expression.aspx");
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
	    
	     //输入验证
        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtTradeID.value))
            {
                parent.document.all.txtWroMessage.value('<%= conLeaseTradeID %>');
                return false;
            }
            
             if(isEmpty(document.all.txtContractCode.value))
            {
                parent.document.all.txtWroMessage.value =('<%= contractCode %>');
                return false;
            }
            
            if(isEmpty(document.all.txtConStartDate.value))
            {
                parent.document.all.txtWroMessage.value =('<%= contractBeginDate %>');
                return false;
            }
            
            if(isEmpty(document.all.txtConEndDate.value))
            {
                parent.document.all.txtWroMessage.value =('<%= contractEndDate %>');
                return false;
            }
            
            if(document.getElementById("txtConStartDate").value > document.getElementById("txtConEndDate").value)
            {
                parent.document.all.txtWroMessage.value =('<%= beginEndDate %>');
                return false;
            }
            
            if((document.getElementById("txtConStartDate").value > document.getElementById("txtChargeStart").value) || (document.getElementById("txtConEndDate").value < document.getElementById("txtChargeStart").value))
            {
                parent.document.all.txtWroMessage.value =('<%= beginChargeDate %>');
                return false;
            }
        }  
        
        //计算免租期
        function GetNorentDays()
        {
            if(((document.getElementById("txtChargeStart").value != "undefined")&&(document.getElementById("txtChargeStart").value != ""))||((document.getElementById("txtConStartDate").value != "undefined")&&(document.getElementById("txtConStartDate").value != "")))
            //if((document.getElementById("txtChargeStart").value != "undefined")&&(document.getElementById("txtChargeStart").value != ""))
		        { 
		           var day = dayselapsed(document.getElementById("txtChargeStart").value,document.getElementById("txtConStartDate").value);
		           document.getElementById("Hidden_txtNorentDays").value = day;
		           document.getElementById("txtNorentDays").value = day;
		           document.getElementById("txtNorentDays").readOnly = true;
		        }
        } 
        //计算两个日期中的间隔天数
        function dayselapsed(date1,date2) 
        {
            var dob = new Date(date1.substring(0,4),date1.substring(5,7)-1,date1.substring(8,10));
            var dop = new Date(date2.substring(0,4),date2.substring(5,7)-1,date2.substring(8,10));
            
            var difference = Date.UTC(dob.getYear(),dob.getMonth(),dob.getDate(),0,0,0) 
                           - Date.UTC(dop.getYear(),dop.getMonth(),dop.getDate(),0,0,0);
                           
            return difference/1000/60/60/24;
        }
        
        /*获取二级经营类别*/
        function ShowTree()
        {
        
        	strreturnval=window.showModalDialog('../TradeRelation/TradeRelationSelect.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("hidTradeID").value = strreturnval;
			if ((window.document.all("hidTradeID").value != "undefined") && (window.document.all("hidTradeID").value != ""))
			{
			    var btnBindDealType = document.getElementById('<%= btnBindDealType.ClientID %>');
                btnBindDealType.click();
            }
			else
			{
				return;	
			}  
        }
        
        function BillOfDocumentDelete()
        {
            return window.confirm('<%= billOfDocumentDelete %>');
        }
        
          function ShowMessage()
        {
            var wFlwID = document.getElementById("HidenWrkID").value;
            var vID = document.getElementById("HidenVouchID").value;
        	strreturnval=window.showModalDialog('../NodeMessage.aspx?wrkFlwID='+encodeURI(wFlwID)+'&voucherID='+encodeURI(vID),'window','dialogWidth=600px;dialogHeight=320px'); 
        }
                function ReturnDefault()
    {
        window.parent.mainFrame.location.href="../../Disktop1.aspx";
    }
    </script>
</head>
<body onload="Load()" style="margin:0px" >
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
     <table border="0" cellpadding="0" cellspacing="0" style="height: 470px; width: 100%;">
     <tr>
            <td style="width: 100%; height: 24px; text-align: left;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblBasicContractInfo %>" Width="287px"></asp:Label></td>
            <td style="width: 100%; height: 24px;" class="tdTopRightBackColor" align="left"></td>
            <td style="width: 7px; height: 24px;" class="tdTopRightBackColor" valign="top">
                <img class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
        </tr>
        <tr>
            <td class="tdBackColor" colspan="3" style="width: 100%; height: 339px;"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    
                    <tr class="headLine">
                        <td style="background-color:White; height:1" colspan="4" width="100%">
                        </td>
                    </tr>
                    <tr>
                        <td style="width:635px; height:15px;" colspan="4" class="tdBackColor">
                        </td>
                    </tr>
                     <tr>
                    <!--  *********left
                    -->
                    <td valign="top" style="width: 42%">
                    
                <table class="tblBase" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:5px">
                        <td style="width:109px;">
                        </td>
                        <td style="width:139px;">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 117%">
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
        <td style="width: 139px">
            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
        <td style="width: 139px">
            <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" BackColor="#F5F5F4" ReadOnly="True"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
        <td style="width: 139px">
            <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px" ReadOnly="True" BackColor="#F5F5F4"></asp:TextBox></td>
    </tr>
        <tr style="height:20px">
            <td colspan=2></td>
        </tr>
        <tr style="height:5px" >
            <td style="width: 109px" >
            </td>
            <td style="width:139px;">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 117%">
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
            <asp:Label ID="Label60" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Contract_ManageCompany %>"></asp:Label>
        </td>
        <td style="height:18px; width: 139px;">
            <asp:DropDownList ID="ddlSubs" runat="server" BackColor="#F5F5F4" Width="165px">
            </asp:DropDownList>
        </td>
    </tr>
                    <tr>
                        <td class="baseLable">
                            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" 
                                Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"></asp:Label>
                        </td>
                        <td style="height:18px; width: 139px;">
                            <asp:TextBox ID="txtTradeID" runat="server" CssClass="ipt160px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
        <tr>
            <td class="baseLable">
                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label></td>
            <td style="width: 139px">
                <asp:TextBox ID="cmbContractStatus" runat="server" CssClass="ipt160px" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="baseLable">
            <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label></td>
            <td style="height: 19px; width: 139px;">
            <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="baseLable" >
            <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label></td>
            <td style="width: 139px">
            <asp:TextBox ID="txtRefID" runat="server" CssClass="ipt160px" style="ime-mode:disabled;" MaxLength="16"></asp:TextBox></td>
        </tr>
                    <tr>
                        <td class="baseLable">
                            <asp:Label ID="Label61" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Contract_ContractType %>"></asp:Label>
                        </td>
                        <td style="width: 139px">
                            <asp:DropDownList ID="ddlContractType" runat="server" BackColor="#F5F5F4" Width="165px">
                            </asp:DropDownList>
                        </td>
                    </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
        <td style="width: 139px">
            <asp:TextBox ID="txtConStartDate" onclick="calendarExt(GetNorentDays)" runat="server" CssClass="ipt160px"></asp:TextBox></td>
    </tr>
        <tr>
            <td class="baseLable">
                <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
            <td style="width: 139px">
                <asp:TextBox ID="txtConEndDate" onclick="calendar()" runat="server" CssClass="ipt160px"></asp:TextBox></td>
        </tr>
                    <tr>
                        <td class="baseLable">
                <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>" Width="99px"></asp:Label></td>
                        <td style="width: 139px">
                <asp:DropDownList ID="DDownListPenalty" runat="server" Width="165px">
                </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="baseLable">
            <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>"></asp:Label></td>
                        <td style="width: 139px">
            <asp:DropDownList ID="DDownListTerm" runat="server" Width="165px">
            </asp:DropDownList></td>
                    </tr>
                </table>
                    </td>
                    
                    <!--  *********right
                    -->
                    <td style="width: 46%">
                    
                <table class="tblBase" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:5px">
                        <td style="width:78px;">
                        </td>
                        <td class="baseInput">
                            <table width="164px" border="0" cellpadding="0" cellspacing="0">
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
        <td class="baseLable">
            <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtChargeStart" onclick="calendarExt(GetNorentDays)" runat="server" CssClass="ipt160px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="baseLable">
                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblNorentDays %>" CssClass="labelStyle"></asp:Label></td>
        <td class="baseInput">
                            <asp:TextBox ID="txtNorentDays" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"></asp:TextBox></td>
    </tr>
                    <tr>
                        <td class="baseLable">
            <asp:Label ID="Label32" runat="server" Text="<%$ Resources:BaseInfo,LeaseholdContract_labEConURL %>" CssClass="labelStyle"></asp:Label></td>
                        <td class="baseInput">
            <asp:TextBox ID="txtBargain" runat="server" CssClass="ipt160px" MaxLength="128"></asp:TextBox></td>
                    </tr>
    <tr style="height:20px">
            <td colspan=2></td>
        </tr>
        <tr style="height:8px" >
            <td style="width: 78px">
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
            <asp:Label ID="Label54" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labAdditionalItem %>"></asp:Label></td>
        <td valign="top">
            <asp:TextBox ID="listBoxAddItem" runat="server" Height="72px" CssClass="ipt160px" TextMode="MultiLine"></asp:TextBox></td>
    </tr>
    <tr style="height:20px">
            <td colspan="2" style="height: 20px"></td>
        </tr>
        <tr style="height:5px" >
            <td style="width: 78px">
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
            <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"></asp:Label></td>
        <td rowspan="2" valign="top">
            <asp:TextBox ID="listBoxRemark" runat="server" Height="58px" CssClass="ipt160px" TextMode="MultiLine"></asp:TextBox></td>
            </tr>
        <tr>
            <td style="height: 28px; width: 78px;">
                <asp:Button ID="btnBindDealType" runat="server" CssClass="hidden" Height="1px" OnClick="btnBindDealType_Click"
                    Text="Button" Width="1px" /><asp:TextBox ID="hidTradeID" runat="server" CssClass="hidden"
                        Width="1px"></asp:TextBox></td>
        </tr>
        <tr style="height:15px">
            <td colspan="2"></td>
        </tr>
                    <tr style="height:5px">
                        <td style="width: 78px">
                        </td>
                        <td rowspan="1" valign="top" style="width:164px"><table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
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
                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblCommOper %>" CssClass="labelStyle"></asp:Label></td>
                        <td rowspan="1" valign="top">
                            <asp:TextBox ID="txtCommOper" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"
                                ReadOnly="True"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td class="baseLable">
                        </td>
                        <td rowspan="1" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2" rowspan="1" valign="top"
                            style="padding-right:20px">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnTempSave" runat="server" CssClass="buttonCancel"
                                        Text="<%$ Resources:BaseInfo,Lease_NewLineBtnTemp %>" 
                                OnClick="btnTempSave_Click" />&nbsp;
                            <asp:Button ID="btnPutIn" runat="server" CssClass="buttonSave" OnClick="btnPutIn_Click"
                                Text="<%$ Resources:BaseInfo,Lease_NewLineBtnPutIn %>" Enabled="False" />&nbsp;
                            <asp:Button ID="btnBalnkOut" runat="server" CssClass="buttonClear" 
                                Enabled="False" OnClick="btnBalnkOut_Click" 
                                Text="<%$ Resources:BaseInfo,ConLease_butDel %>" />&nbsp;
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />
                                <asp:Button ID="btnMessage" runat="server" CssClass="buttonMessage" 
                                Text="<%$ Resources:BaseInfo,WrkFlwEntity_btnMessage %>" />
                                </td>
                    </tr>
                    <!--  *********
                    -->          
                    <asp:HiddenField ID="HidenWrkID" runat="server">
            </asp:HiddenField>
            <asp:HiddenField ID="HidenVouchID" runat="server">
            </asp:HiddenField>
         </td> 
    </tr>
                   
                </table>
                    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <input id="Hidden_txtNorentDays" runat="server" type="hidden"/>
    </form>
</body>
</html>
