<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Fitment.aspx.cs" Inherits="Shop_ShopFitment_Fitment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript"  src="../../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
  <%--  <script type="text/javascript"  src="../../JavaScript/setday.js"></script>--%>
	<script type="text/javascript" src="../../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
     <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript">
    
    //验证数字类型
    function textleave(form1)
    {
        if(!isInteger(document.all.txtRentalPrice.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidIntError").value;
            document.all.txtRentalPrice.focus();
            return false;					
        }
        if(!isInteger(document.all.txtRentArea.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidIntError").value;
            document.all.txtRentArea.focus();
            return false;					
        }
    }
    
    
	    function chooseCard(id)
	    {
	    	document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
	        if(id==0){
		        document.getElementById("PotCustomerList").style.visibility ="visible";
		        document.getElementById("PotCustomer").style.visibility ="hidden";
		        document.getElementById("PotCustomerList").style.position="absolute";
		        document.getElementById("PotCustomerList").style.top ="20px";
		        document.getElementById("showmain").style.height ="450px";
		        document.getElementById("chooseDiv").style.position="absolute";
		        document.getElementById("chooseDiv").style.top ="417px";
		        document.getElementById("chooseDiv").style.left ="95px"
		        document.getElementById("tab2").className="tab";
		        document.getElementById("tab1").className="selectedtab";
		        
	        }
	        if(id==1){
		        document.getElementById("PotCustomerList").style.visibility ="hidden";
		        document.getElementById("PotCustomer").style.visibility ="visible";
		        document.getElementById("PotCustomer").style.position="absolute";
		        document.getElementById("PotCustomer").style.top ="20px";
                document.getElementById("tab1").className="tab";
		        document.getElementById("tab2").className="selectedtab";
	        }
	    }
//	    function PalaverShowTime()
//	    {
//	        calendar(document.form1.txtPalaverTime);
//	    }
//	    function StartShowTime()
//	    {
//	        calendar(document.form1.txtShopStartDate);
//	    }
//	    function EndShowTime()
//	    {
//	        calendar(document.form1.txtShopEndDate);
//	    }
	    
	//text控件文本验证
    function CustomrtBoxValidator(sForm)
    {
        if(isEmpty(document.all.txtPotShopName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtPotShopName.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtMainBrand.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtMainBrand.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtShopStartDate.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtShopStartDate.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtShopEndDate.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtShopEndDate.focus();
            return false;					
        } 
        
        if(isEmpty(document.all.txtRentalPrice.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtRentalPrice.focus();
            return false;					
        }
        
         if(isEmpty(document.all.txtRentArea.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtRentArea.focus();
            return false;					
        } 
        
        var beginvalue=document.getElementById("txtShopStartDate").value;
        var endvalue=document.getElementById("txtShopEndDate").value;

        if(beginvalue!="")
        {
              var begin=new Array();
              var end=new Array();
              begin=beginvalue.split("-");
              end=endvalue.split("-");
              var beginnum=begin[0]+begin[1]+begin[2];
              var endnum=end[0]+end[1]+end[2];

            if(endnum<beginnum)
            {
                parent.document.all.txtWroMessage.value=document.getElementById("hidDateTime").value;
                document.all.txtShopEndDate.focus();
                return false;	
            }
        }
    }
    
    	//text控件文本验证
    function PalaverBoxValidator(sForm)
    {
        if(isEmpty(document.all.txtPalaverTime.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtPalaverTime.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtPalaverUser.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtPalaverUser.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtPalaverAim.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtPalaverAim.focus();
            return false;					
        }
    }
    </script>
</head>
<body onload='chooseCard(0);' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <table border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle" style="height: 445px" id="showmain">
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
            <div style="width: 239px; height: 103px" id="PotCustomerList">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 553px; height: 400px">
                    <tr>
                        <td class="tdTopBackColor" style="vertical-align: middle; width: 290px; height: 25px;
                            text-align: left" valign="top">
                            <img alt="" class="imageLeftBack" />
                            <asp:Label ID="labCustomer" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,BizGrp_Title %>"></asp:Label></td>
                        <td class="tdTopRightBackColor" colspan="2" style="width: 538px; height: 25px; text-align: right"
                            valign="top">
                            <img alt="" class="imageRightBack" /></td>
                    </tr>
                    <tr>
                        <td colspan="8" style="width: 533px; height: 1px; background-color: white">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="3" style="vertical-align: top; width: 535px; height: 330px;
                            text-align: center" valign="top">
                            <table style="width: 552px">
                                <tr>
                                    <td class="tdBackColor" colspan="8" style="width: 495px; height: 5px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 241px; height: 30px; text-align: right">
                                        <asp:Label ID="lblBizGrpCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustCode %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 8px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 184px; height: 30px; text-align: left">
                                        <asp:TextBox ID="txtPotCode" runat="server" CssClass="textstyle"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                        <asp:Label ID="lblBizGrpName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>"
                                            Width="75px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 7px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 129px; height: 30px; text-align: left">
                                        <asp:TextBox ID="txtPotName" runat="server" CssClass="textstyle"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 100px; height: 30px">
                                        <asp:Button ID="btnCustCode" runat="server" CssClass="buttonQueryHelp" OnClick="btnCustCode_Click" /></td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 241px; height: 30px; text-align: right">
                                        <asp:Label ID="lblBizGrpStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblPotShopName %>"
                                            Width="66px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 8px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 184px; height: 30px; text-align: left">
                                        <asp:DropDownList ID="cmbShopID" runat="server" BackColor="White" CssClass="cmb160px"
                                            Width="124px">
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                        <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>"
                                            Width="66px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 7px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 129px; height: 30px; text-align: left">
                                        <asp:TextBox ID="txtBeginDate" runat="server" CssClass="textstyle" onclick="calendar()"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 100px; height: 30px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 241px; height: 30px; text-align: right">
                                        <asp:Label ID="Label1" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>"
                                            Width="66px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 8px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 184px; height: 30px; text-align: left">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="textstyle" onclick="calendar()"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                        <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>"
                                            Width="66px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 7px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 129px; height: 30px; text-align: left">
                                        <asp:TextBox ID="txtPlanDate" runat="server" CssClass="textstyle" onclick="calendar()"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 100px; height: 30px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="8" style="width: 495px; height: 12px; text-align: center">
                                        <table border="0" cellpadding="0" cellspacing="0" style="left: 12px; width: 526px;
                                            position: relative">
                                            <tbody>
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #738495">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="8" style="height: 167px">
                                        <asp:GridView ID="GrdWrkGrp" runat="server" AutoGenerateColumns="False" BackColor="White"
                                            BorderStyle="Inset" BorderWidth="1px" CellPadding="3" Height="159px" PageSize="8"
                                            Width="531px">
                                            <FooterStyle BackColor="Red" ForeColor="#000066" />
                                            <Columns>
                                                <asp:BoundField DataField="BizGrpID">
                                                    <ItemStyle CssClass="hidden" />
                                                    <HeaderStyle CssClass="hidden" />
                                                    <FooterStyle CssClass="hidden" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BizGrpCode" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizCode %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BizGrpName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpName %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="BizGrpStatusName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Note" HeaderText="<%$ Resources:BaseInfo,WrkFlw_lblBizNote %>">
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                </asp:BoundField>
                                                <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" ShowSelectButton="True">
                                                    <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                </asp:CommandField>
                                            </Columns>
                                            <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                            <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="3" style="height: 53px">
                                        &nbsp;&nbsp;
                                        <asp:Label ID="lblTotalNum" runat="server"></asp:Label>
                                        <asp:Label ID="lblCurrent" runat="server" ForeColor="Red">1</asp:Label>
                                        <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                                            Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Width="70px" /></td>
                                    <td class="tdBackColor" style="height: 53px">
                                    </td>
                                    <td class="tdBackColor" colspan="4" style="left: 30px; vertical-align: middle; width: 270px;
                                        height: 53px; text-align: left">
                                        <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                            Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNext" runat="server"
                                                CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width: 546px; height: 83px" id="PotCustomer">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 553px; height: 400px">
                    <tr>
                        <td class="tdTopBackColor" style="vertical-align: middle; width: 290px; height: 25px;
                            text-align: left" valign="top">
                            <img alt="" class="imageLeftBack" />
                            <asp:Label ID="Label2" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,BizGrp_Title %>"></asp:Label></td>
                        <td class="tdTopRightBackColor" colspan="2" style="width: 538px; height: 25px; text-align: right"
                            valign="top">
                            <img alt="" class="imageRightBack" /></td>
                    </tr>
                    <tr>
                        <td colspan="8" style="width: 533px; height: 1px; background-color: white">
                        </td>
                    </tr>
                    <tr>
                        <td class="tdBackColor" colspan="3" style="vertical-align: top; width: 535px; height: 330px;
                            text-align: center" valign="top">
                            <table style="width: 552px">
                                <tr>
                                    <td class="tdBackColor" colspan="8" style="width: 495px; height: 25px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                        <asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizCode %>"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 8px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                        <asp:DropDownList ID="DropDownList2" runat="server" BackColor="White" CssClass="cmb160px"
                                            Width="124px">
                                        </asp:DropDownList></td>
                                    <td class="tdBackColor" style="height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                        <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpName %>"
                                            Width="87px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 7px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                        <asp:TextBox ID="TextBox2" runat="server" CssClass="textstyle"></asp:TextBox></td>
                                    <td class="tdBackColor" style="width: 100px; height: 30px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                        <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpStatus %>"
                                            Width="66px"></asp:Label></td>
                                    <td class="tdBackColor" style="width: 8px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                        <asp:TextBox ID="TextBox1" runat="server" CssClass="textstyle"></asp:TextBox></td>
                                    <td class="tdBackColor" style="height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                    </td>
                                    <td class="tdBackColor" style="width: 7px; height: 30px">
                                    </td>
                                    <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                        <asp:Button ID="Button1" runat="server" CssClass="buttonSave" Height="31px" OnClick="btnSave_Click"
                                            Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" Width="70px" /></td>
                                    <td class="tdBackColor" style="width: 100px; height: 30px">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="8" rowspan="4" style="left: 30px; vertical-align: middle;
                                        text-align: left">
                                        &nbsp;</td>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                                <tr>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
                </td>
                <td style="width: 60px; height: 401px; text-align: center" valign="top">
                    <img height="401" src="../../images/shuxian.jpg" />
                </td>
            </tr>
        </table>
        <div id="chooseDiv" style="left: 508px; overflow: auto; width: 272px; position: absolute;
            top: 521px; height: 55px">
            <table>
                <tr>
                    <td id="tab1" class="selectedtab" onclick="chooseCard(0);" style="width: 70px; height: 21px">
                        <span id="tabpage1">
                            <asp:Label ID="hidShop" runat="server" Text="<%$ Resources:BaseInfo,Hidden_Shop %>"
                                Width="63px"></asp:Label></span></td>
                    <td id="tab2" class="tab" onclick="chooseCard(1);" style="width: 67px; height: 21px">
                        <span id="tabpage2">
                            <asp:Label ID="hidPalaverNode" runat="server" Text="<%$ Resources:BaseInfo,PotShop_lblPalaverNode %>"
                                Width="61px"></asp:Label></span></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
