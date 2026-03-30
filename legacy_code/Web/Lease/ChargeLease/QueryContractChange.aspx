<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryContractChange.aspx.cs" Inherits="Lease_ChangeLease_QueryContractChange" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Lease_ExpressionMod")%></title>
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
        addTabTool("<%=titleName%>,Lease/ChangeLease/QueryContractChange.aspx~<%=changeLease%>,Lease/ExpressionPalaver.aspx~<%=changeLeaseNow%>,Lease/ChangeLease/ChangeExpression.aspx");
	    loadTitle();
    }
    
    function InputValidator(sForm)
    {
        if(document.getElementById("txtConEndDate").value > document.getElementById("txtNewConStartDate").value)
        {
            parent.document.all.txtWroMessage.value='<%= dateError %>';
            return false;
        }
        
        if(document.getElementById("txtNewConStartDate").value > document.getElementById("txtNewConEndDate").value)
        {
            parent.document.all.txtWroMessage.value='<%= dateError %>';
            return false;
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
    <div id="DIV1" >
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div id="showLeaseBargain" style="text-align:center">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 452px">
                                    <tr>
                                        <td align="left" class="tdTopRightBackColor" style="width: 557px; height: 22px; text-align: left">
                                            <img class="imageLeftBack" src="" style="width: 7px; height: 22px;" />
                                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources:BaseInfo,Lease_Addrentalformula %>" Width="291px"></asp:Label></td>
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
                                                    <td valign="top" width="50%" style="width: 100%; text-align:left">
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
                                                                <td style="height: 35px">
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
                                                                    <asp:Label ID="Label53" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Contract_ManageCompany %>"></asp:Label>
                                                                </td>
                                                                <td style="height: 28px">
                                                                    <asp:DropDownList ID="ddlSubs" runat="server" BackColor="#F5F5F4" 
                                                                        Width="165px" Enabled="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 28px">
                                                                    <asp:Label ID="Label4" runat="server" CssClass="labelStyle" 
                                                                        Text="<%$ Resources:BaseInfo,LeaseholdContract_labTradeID %>"></asp:Label>
                                                                </td>
                                                                <td style="height: 28px">
                                                                    <asp:TextBox ID="txtTradeID" runat="server" Enabled="False"
                                                                        CssClass="Enabledipt160px"></asp:TextBox>
                                                                </td>
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
                                                                    <asp:TextBox ID="txtRefID" runat="server" CssClass="Enabledipt160px" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable" style="height: 30px">
                                                                    <asp:Label ID="Label54" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Contract_ContractType %>"></asp:Label>
                                                                </td>
                                                                <td style="height: 30px">
                                                                    <asp:DropDownList ID="ddlContractType" runat="server" BackColor="#F5F5F4" 
                                                                        Width="165px" Enabled="False">
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConStartDate %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtConStartDate" runat="server" CssClass="Enabledipt160px"  ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labConEndDate %>"></asp:Label></td>
                                                                <td style="height: 30px">
                                                                    <asp:TextBox ID="txtConEndDate" runat="server" CssClass="Enabledipt160px" ReadOnly="True" ></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label52" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labPenalty %>"
                                                                        Width="99px"></asp:Label></td>
                                                                <td>
                                                                    <asp:DropDownList ID="DDownListPenalty" runat="server" Width="165px" Enabled="False">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseholdContract_labNotice %>"></asp:Label></td>
                                                                <td>
                                                                    <asp:DropDownList ID="DDownListTerm" runat="server" Width="165px" Enabled="False">
                                                                    </asp:DropDownList></td>
                                                            </tr>
                                                            <tr>
                                                                <td class="baseLable">
                                                                    &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <!--  *********right
                    -->
                                                    <td style="vertical-align: top; width: 100%; text-align:left" width="50%">
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
                                                                    <asp:Label ID="lblModReason" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,ConFormulaMod_ModReason %>"></asp:Label></td>
                                                                <td style="height: 30px; width: 206px;">
                                                                    <asp:TextBox ID="txtModReason" runat="server" CssClass="ipt160px" Height="72px"
                                                                        TextMode="MultiLine"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" style="text-align: center">
                                                                    <br />
                                                                    <br />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" OnClick="btnAdd_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                                        Text="<%$ Resources:BaseInfo,Lease_NewLineBtnTemp %>" />
                                                                    <asp:Button ID="btnPutIn" runat="server" CssClass="buttonPutIn" OnClick="butAuditing_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"
                                                                        Text="<%$ Resources:BaseInfo,Lease_NewLineBtnPutIn %>" Enabled="False" />
                                                                    <asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear" Enabled="False" OnClick="btnBalnkOut_Click" Text="<%$ Resources:BaseInfo,ConLease_butDel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="Expressionshow">
                                           
                                        </td>
                                    </tr>
                                </table>
                                 &nbsp;</div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
        
    </form>
</body>
</html>
