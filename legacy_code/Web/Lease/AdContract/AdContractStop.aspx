<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdContractStop.aspx.cs" Inherits="Lease_AdContract_AdContractStop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Menu_AdContractStopAdd")%></title>
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
        addTabTool("<%=titleName%>,Lease/AdContract/AdContractStop.aspx~<%=leaseItem %>,Lease/LeaseItemPalaver.aspx~<%=shopInfo %>,Lease/AdContract/AdBoardListPalaver.aspx~<%=espression %>,Lease/AdContract/AdBoardExpressionPalaver.aspx");
	    loadTitle();
    }
    
    function InputValidator(sForm)
    {   
        if((document.getElementById("txtConEndDate").value < document.getElementById("txtStopDate").value))
        {
            parent.document.all.txtWroMessage.value =('<%= dateError %>');
            return false;
        }    
         if((document.getElementById("txtConStartDate").value > document.getElementById("txtStopDate").value))
        {
            parent.document.all.txtWroMessage.value =('<%= dateError %>');
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
    
    function BillOfDocumentDelete()
    {
        return window.confirm('<%= billOfDocumentDelete %>');
    }
    function ReturnDefault()
    {
        window.parent.mainFrame.location.href="../../Disktop.aspx";
    }
    function ShowMessage()
    {
        var wFlwID = document.getElementById("HidenWrkID").value;
        var vID = document.getElementById("HidenVouchID").value;
	    window.showModalDialog('../../lease/NodeMessage.aspx?wrkFlwID='+encodeURI(wFlwID)+'&voucherID='+encodeURI(vID),'window','dialogWidth=600px;dialogHeight=320px'); 
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
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblBasicContractInfo %>"></asp:Label></td>
                                        <td align="left" class="tdTopRightBackColor" style="width: 562px; height: 22px">
                                        </td>
                                        <td class="tdTopRightBackColor" style="width: 7px; height: 22px" valign="top">
                                            <img align="right" class="imageRightBack" src="" style="width: 7px; height: 22px;" /></td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="3" style="width: 100%;" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr class="headLine">
                                                    <td colspan="4" style="height: 1px; background-color: white" width="100%">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="4" style="width: 100%; height: 15px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <!--  *********left
                    -->
                                                    <td valign="top" width="50%" style="width: 50%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" width="100%">
                                                            <tr style="height: 5px">
                                                                <td style="width: 20%">
                                                                </td>
                                                                <td style="width: 30%">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 60%">
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
                                                                    <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px"></asp:TextBox>
                                                                    <asp:Button ID="btnQueryContract" runat="server" CssClass="buttonQueryHelp" OnClick="btnQueryContract_Click" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 10px">
                                                                    </td>
                                                                <td style="height: 10px">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 60%">
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
                                                                <td class="baseLable" style="height: 10px">
                                                                    <asp:Label ID="Label14" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Contract_ManageCompany %>"></asp:Label></td>
                                                                <td style="height: 10px">
                                                                    <asp:DropDownList ID="ddlSubs" runat="server" BackColor="#F5F5F4" Width="165px">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                            <td style="text-align: right">
                                                                    <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractStatus %>"></asp:Label>
                                                                                &nbsp;&nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="cmbContractStatus" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td style="text-align: right; height: 5px;">
                                                                    <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labContractCode %>"></asp:Label>
                                                                    &nbsp;&nbsp;
                                                                </td>
                                                                <td style="height: 5px;">
                                                                    <asp:TextBox ID="txtContractCode" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 28px">
                                                                    <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labRefID %>"></asp:Label></td>
                                                                <td style="height: 28px">
                                                                    <asp:TextBox ID="txtRefID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px">
                                                                    <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtConStartDate" runat="server" CssClass="Enabledipt160px"  ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px">
                                                                    <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtConEndDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True" ></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>"
                                                                        Width="99px"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:DropDownList ID="DDownListPenalty" runat="server" Enabled="False" Width="165px">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:DropDownList ID="DDownListTerm" runat="server" Enabled="False" Width="165px">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label54" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labAdditionalItem %>"></asp:Label></td>
                                                                <td rowspan="3" valign="top">
                                                                    <asp:TextBox ID="listBoxAddItem" runat="server" CssClass="ipt160px" Height="75px"
                                                                        TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    </td>
                                                            </tr>
                                                        </table>
                                                                    <asp:Label ID="Label32" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labEConURL %>" Visible="False"></asp:Label>
                                                                    <asp:TextBox ID="txtBargain" runat="server" CssClass="ipt160px" Visible="False"></asp:TextBox></td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; width: 50%;" width="50%">
                                                        <table border="0" cellpadding="0" cellspacing="0" class="tblBase" style="width: 100%">
                                                            <tr style="height: 5px">
                                                                <td style="width: 15%; height: 5px;">
                                                                </td>
                                                                <td class="baseInput" style="width: 35%; height: 5px;">
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
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label13" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,AdContract_lblStopDate %>" Width="98px"></asp:Label></td>
                                                                <td style="width: 30%;">
                                                                    <asp:TextBox ID="txtStopDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConTerminateBill_lblTerReason %>"></asp:Label></td>
                                                                <td class="baseInput" rowspan="2">
                                                                    <asp:TextBox ID="txtTerReason" runat="server" CssClass="ipt160px" Height="74px" TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 28px">
                                                                    </td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 8px">
                                                                <td>
                                                                </td>
                                                                <td style="width: 206px">
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
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label59" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNote %>"></asp:Label></td>
                                                                <td valign="top">
                                                                    <asp:TextBox ID="txtNode" runat="server" CssClass="ipt160px" Height="58px"
                                                                        TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                            <tr style="height: 20px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td>
                                                                </td>
                                                                <td>
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
                                                                <td colspan="2" rowspan="2" valign="top">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                            </tr>
                                                            <tr style="height: 15px">
                                                                <td colspan="2">
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 5px">
                                                                <td>
                                                                </td>
                                                                <td rowspan="1" valign="top">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" rowspan="1" style="height: 25px" valign="top">
                                                                    &nbsp;<asp:Button ID="btnTempSave" runat="server" CssClass="buttonCancel" Enabled="False" OnClick="btnTempSave_Click" Text="<%$ Resources:BaseInfo,Lease_NewLineBtnTemp %>"
                                                                         />
                                                                    <asp:Button ID="btnOverTime" runat="server" CssClass="buttonSave" Enabled="False" OnClick="btnOverTime_Click" Text="<%$ Resources:BaseInfo,Lease_NewLineBtnPutIn %>"
                                                                         />
                                                                    <asp:Button ID="btnBalnkOut" runat="server" CssClass="buttonClear" Enabled="False" OnClick="btnBalnkOut_Click" Text="<%$ Resources:BaseInfo,ConLease_butDel %>"
                                                                         />
                                                                    <asp:Button ID="btnMessage" runat="server" CssClass="buttonMessage" Text="<%$ Resources:BaseInfo,WrkFlwEntity_btnMessage %>" Enabled="False" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <asp:HiddenField ID="HidenWrkID" runat="server">
            </asp:HiddenField>
             <asp:HiddenField ID="HidenVouchID" runat="server">
            </asp:HiddenField>
                        </ContentTemplate>
                    </asp:UpdatePanel>
        &nbsp; &nbsp;
        </div>
    </form>
</body>
</html>
