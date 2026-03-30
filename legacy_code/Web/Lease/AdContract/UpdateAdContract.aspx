<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UpdateAdContract.aspx.cs" Inherits="Lease_AdContract_UpdateAdContract" %>

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
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool("<%=baseInfo %>,Lease/AdContract/UpdateAdContract.aspx~<%=leaseItem %>,Lease/LeaseItem.aspx~<%=shopInfo %>,Lease/AdContract/AdBoardList.aspx~<%=espression %>,Lease/AdContract/AdBoardExpression.aspx");
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
                parent.document.all.txtWroMessage.value=('<%= conLeaseTradeID %>');
                return false;
            }
            
             if(isEmpty(document.all.txtContractCode.value))
            {
                parent.document.all.txtWroMessage.value=('<%= contractCode %>');
                return false;
            }
            
            if(isEmpty(document.all.txtConStartDate.value))
            {
                parent.document.all.txtWroMessage.value=('<%= contractBeginDate %>');
                return false;
            }
            
            if(isEmpty(document.all.txtConEndDate.value))
            {
                parent.document.all.txtWroMessage.value=('<%= contractEndDate %>');
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
        
        	strreturnval=window.showModalDialog('TradeRelation/TradeRelationSelect.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("hidTradeID").value = strreturnval;
			if ((window.document.all("hidTradeID").value != "undefined") && (window.document.all("hidTradeID").value != ""))
			{
			    var btnBindDealType = document.getElementById('<%= btnBindDealType.ClientID %>');
                btnBindDealType.click();
            }
			else
				return;	
//        //document.form1.btnBindDealType.click();
//            window.open('TradeRelation/TradeRelationSelect.aspx?', 'b', 'height=450, width=180, top=120, left=262, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no');
            
        }
        
        function BillOfDocumentDelete()
        {
            return window.confirm('<%= billOfDocumentDelete %>');
        }
    </script>
</head>
<body style="margin:0px" onload="Load()">
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
         <div id="BaseBargain">
    <table border="0" cellpadding="0" cellspacing="0" style="height: 386px; width: 100%;">
        <tr>
            <td style="width: 430px; height: 24px; text-align: left;" class="tdTopRightBackColor" align="left">
                <img class="imageLeftBack" src="" style="width: 7px"  />
                <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblBasicContractInfo %>" Width="314px"></asp:Label></td>
            <td style="width: 562px; height: 24px;" class="tdTopRightBackColor" align="left"></td>
            <td style="width: 7px; height: 24px;" class="tdTopRightBackColor" valign="top">
                <img alt="imgRightBack" class="imageRightBack" src="" style="width: 7px;" align="right" /></td>
        </tr>
        <tr>
            <td class="tdBackColor" colspan="3" style="width: 100%; height: 339px;"
                    valign="top">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%" id="TABLE1">
                    
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
                    <td width="50%" valign="top">
                    
                <table class="tblBase" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:5px">
                        <td style="width:20%;">
                        </td>
                        <td style="width:30%;">
                            <table width="165" border="0" cellpadding="0" cellspacing="0">
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
            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" MaxLength="16" ReadOnly="True"></asp:TextBox>
            </td>
    </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCustName" runat="server" CssClass="ipt160px" MaxLength="64" ReadOnly="True"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtCustShortName" runat="server" CssClass="ipt160px" MaxLength="32" ReadOnly="True"></asp:TextBox>
            </td>
    </tr>
        <tr style="height:20px">
            <td colspan=2></td>
        </tr>
        <tr style="height:5px" >
            <td>
            </td>
            <td>
                <table width="165" border="0" cellpadding="0" cellspacing="0">
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
                    <tr style="height: 5px">
                        <td class="baseLable">
                            <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Contract_ManageCompany %>"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddlSubs" runat="server" BackColor="#F5F5F4" Width="165px">
                            </asp:DropDownList></td>
                    </tr>
        <tr>
            <td class="baseLable">
                <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label></td>
            <td>
                <asp:TextBox ID="cmbContractStatus" runat="server" CssClass="ipt160px" ReadOnly="True"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="baseLable" style="height: 19px">
            <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label></td>
            <td style="height: 19px">
            <asp:TextBox ID="txtContractCode" runat="server" CssClass="ipt160px" MaxLength="32"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="baseLable">
            <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label></td>
            <td>
            <asp:TextBox ID="txtRefID" runat="server" CssClass="ipt160px" MaxLength="16"></asp:TextBox></td>
        </tr>
    <tr>
        <td class="baseLable">
            <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
        <td>
            <asp:TextBox ID="txtConStartDate" onclick="calendarExt(GetNorentDays)" runat="server" CssClass="ipt160px"></asp:TextBox></td>
    </tr>
        <tr>
            <td class="baseLable">
                <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtConEndDate" onclick="calendar()" runat="server" CssClass="ipt160px"></asp:TextBox></td>
        </tr>
                    <tr>
                        <td class="baseLable">
                <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>" Width="99px"></asp:Label></td>
                        <td>
                <asp:DropDownList ID="DDownListPenalty" runat="server" Width="165px">
                </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td class="baseLable">
            <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>"></asp:Label></td>
                        <td>
            <asp:DropDownList ID="DDownListTerm" runat="server" Width="165px">
            </asp:DropDownList></td>
                    </tr>
                </table>
                    </td>
                    
                    <!--  *********right
                    -->
                    <td width="50%">
                    
                <table class="tblBase" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height:5px">
                        <td style="width:20%;">
                        </td>
                        <td class="baseInput" style="width: 30%">
                            <table width="164" border="0" cellpadding="0" cellspacing="0">
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
            <td>
            </td>
            <td>
                <table width="165" border="0" cellpadding="0" cellspacing="0">
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
            <td style="height: 5px">
            </td>
            <td style="height: 5px;">
                <table width="165" border="0" cellpadding="0" cellspacing="0">
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
            <td style="height: 28px">
            </td>
        </tr>
        <tr style="height:15px">
            <td colspan="2"></td>
        </tr>
                    <tr style="height:5px">
                        <td>
                            <asp:Button ID="btnBindDealType" runat="server" CssClass="hidden" Height="1px" Text="Button"
                                Width="1px" OnClick="btnBindDealType_Click" />
                            <asp:TextBox ID="hidTradeID" runat="server" CssClass="hidden" Width="1px"></asp:TextBox></td>
                        <td rowspan="1" valign="top"><table border="0" cellpadding="0" cellspacing="0" style="width: 81%">
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
                            <asp:DropDownList ID="cmbCommOper" runat="server" CssClass="ipt160px">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2" rowspan="1" valign="top" style="margin-right:30px; height: 28px;">
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />
                            &nbsp; &nbsp;
                        </td>
                    </tr>
                    <!--  *********
                    -->   
  <input id="Hidden_txtNorentDays" runat="server" type="hidden"/>
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
    </div>
    
   <%-- <input type="button" value="删除cookie" onclick="delCookie('Info')" id="Button1" />--%>
    </form>
    
</body>
</html>
