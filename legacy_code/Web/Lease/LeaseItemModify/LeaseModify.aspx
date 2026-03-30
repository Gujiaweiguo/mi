<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LeaseModify.aspx.cs" Inherits="Lease_LeaseItemModify_LeaseModify" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "LeaseModify_Title")%></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/longCss/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
    <style type="text/css">
        <!--
        
        table.tblBase tr{ height:28px; }
        
        table.tblBase tr.headLine{ height:1px; }
        table.tblBase tr.bodyLine{ height:1px; }
        
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
    
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
	<script type="text/javascript" src="../../JavaScript/Common.js"> </script>
	
	<script type="text/javascript">
	
	<!--    
    function Load()
    {
        addTabTool("<%=baseInfo %>,Lease/LeaseItemModify/LeaseModify.aspx~<%=leaseItem %>,Lease/LeaseItemModify/LeaseItemModify.aspx~<%=shopInfo %>,Lease/LeaseItemModify/LeaseConShopModify.aspx");
	    loadTitle();
    }
    
    function InputValidator(sForm)
    {
             if(isEmpty(document.all.txtTradeID.value))
            {
                parent.document.all.txtWroMessage.value =('<%= conLeaseTradeID %>');
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

//	/*验证数字类型*/
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
	-->
    </script>
</head>
<body onload='Load();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <div id="DIV1">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="showLeaseBargain">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 452px">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 167px; height: 22px; text-align: left">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px;" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblBasic %>"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px;" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 100%;" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 635px">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 635px; height: 15px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" width="50%" style="width: 100%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" width="100%">
                                                            <tr style="height: 5px">
                                                                <td style="width: 118px">
                                                                </td>
                                                                <td style="width: 164px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdBoard_lblContractID %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px"></asp:TextBox>
                                                                    <asp:Button ID="btnQueryContract" runat="server" CssClass="buttonQueryHelp" OnClick="btnQueryContract_Click" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 35px">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtCustName" runat="server" CssClass="Enabledipt160px"
                                                                        ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="txtCustShortName" runat="server" CssClass="Enabledipt160px"
                                                                        ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td>
                                                                </td>
                                                                <td style="width: 164px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <td class="baseLable" style="height: 28px">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"></asp:Label></td>
                                                                <td style="height: 28px">
                                                                    <asp:TextBox ID="txtTradeID" runat="server" CssClass="ipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px">
                                                                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="cmbContractStatus" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtContractCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px">
                                                                    <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtRefID" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtConStartDate" runat="server" CssClass="ipt160px" onclick="calendarExt(GetNorentDays)"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtConEndDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>"
                                                                        Width="99px"></asp:Label></td>
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
                                                    <td style="vertical-align: top; width: 100%;" width="50%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 316px">
                                                            <tr style="height: 5px">
                                                                <td style="width: 136px; height: 5px;">
                                                                </td>
                                                                <td class="baseInput" style="width: 206px; height: 5px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="164">
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
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labChargeStartDate %>"></asp:Label></td>
                                                                <td style="height: 30px; width: 206px;">
                                                                    <asp:TextBox ID="txtChargeStart" runat="server" CssClass="ipt160px" onclick="calendarExt(GetNorentDays)"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Lease_lblNorentDays %>"></asp:Label></td>
                                                                <td class="baseInput" style="height: 30px; width: 206px;">
                                                                    <asp:TextBox ID="txtNorentDays" runat="server" BackColor="#F5F5F4" CssClass="ipt160px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label32" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labEConURL %>"></asp:Label></td>
                                                                <td class="baseInput" style="height: 30px; width: 206px;">
                                                                    <asp:TextBox ID="txtBargain" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 8px">
                                                                <td>
                                                                </td>
                                                                <td style="width: 206px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label54" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labAdditionalItem %>"></asp:Label></td>
                                                                <td valign="top" style="width: 206px">
                                                                    <asp:TextBox ID="listBoxAddItem" runat="server" CssClass="ipt160px" Height="72px"
                                                                        TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td style="height: 5px">
                                                                </td>
                                                                <td style="width: 206px; height: 5px;">
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
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
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"></asp:Label></td>
                                                                <td rowspan="2" valign="top" style="width: 206px">
                                                                    <asp:TextBox ID="listBoxRemark" runat="server" CssClass="ipt160px" Height="58px"
                                                                        TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="height: 28px">
                                                                    <asp:Button ID="btnBindDealType" runat="server" CssClass="hidden" Height="1px" OnClick="btnBindDealType_Click"
                                                                        Text="Button" Width="1px" /><asp:TextBox ID="hidTradeID" runat="server" CssClass="hidden"
                                                                            Width="1px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 15px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td>
                                                                </td>
                                                                <td rowspan="1" valign="top" style="width: 206px">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" rowspan="1" style="height: 28px" valign="top">
                                                                    &nbsp;<asp:Button ID="btnTempSave" runat="server" CssClass="buttonCancel" Enabled="False"
                                                                        Height="31px" OnClick="btnTempSave_Click" Text="<%$ Resources:BaseInfo,Lease_NewLineBtnTemp %>"
                                                                        Width="80px" />
                                                                    <asp:Button ID="btnOverTime" runat="server" CssClass="buttonSave" Enabled="False"
                                                                        Height="33px" OnClick="btnOverTime_Click" Text="<%$ Resources:BaseInfo,Lease_NewLineBtnPutIn %>"
                                                                        Width="80px" />
                                                                    <asp:Button ID="btnBalnkOut" runat="server" CssClass="buttonClear" Enabled="False"
                                                                        Height="32px" OnClick="btnBalnkOut_Click" Text="<%$ Resources:BaseInfo,ConLease_butDel %>"
                                                                        Width="70px" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
        <input id="Hidden_txtNorentDays" runat="server" type="hidden" /></div>
    </form>
</body>
</html>
