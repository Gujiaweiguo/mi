<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HandBonusEnter.aspx.cs" Inherits="Associator_HandBonusEnter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=baseInfo %></title>
    <link href="../App_Themes/CSS/Rool.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css" />
    <script src="../App_Themes/DateTime/popcalendar.js" type="text/javascript"></script>
    <script type="text/javascript"  src="../App_Themes/Tabbar/js/dhtmlXCommon.js"></script>
	<script type="text/javascript" src="../App_Themes/Tabbar/js/dhtmlXTabbar.js"></script>
	<script type="text/javascript"  src="../JavaScript/setday.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" language="javascript" charset="gb2312"></script>
 <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        .style1
     {
         width: 180px;
         height: 44px;
     }
            
        -->
    </style>	
	<script type="text/javascript">
	    function Load()
	    {
	        addTabTool("<%=baseInfo %>,Associator/HandBonusEnter.aspx");
	        loadTitle();
	    }
	    
	    function ShowTree()
        {
        	strreturnval=window.showModalDialog('../Lease/Shop/SelectShop.aspx','window','dialogWidth=237px;dialogHeight=420px');
			window.document.all("allvalue").value = strreturnval;
			if ((window.document.all("allvalue").value != "undefined") && (window.document.all("allvalue").value != ""))
			{
	             var objImgBtn2 = document.getElementById('<%= LinkButton2.ClientID %>');
                 objImgBtn2.click();
            }
			else
			{
				return;	
			}  
        }
        
        function AccountBonus()
        {
            
        }
        
         //输入验证
        function InputValidator(sForm)
        {
             if(isEmpty(document.all.txtCardID.value))
            {
                parent.document.all.txtWroMessage.value =('请输入会员卡号！');
                return false;
            }
            
             if(isEmpty(document.all.txtRecepid.value))
            {
                parent.document.all.txtWroMessage.value =('请输入小票号！');
                return false;
            }
            
            if(isEmpty(document.all.txtShopCode.value))
            {
                parent.document.all.txtWroMessage.value =('请选择商铺！');
                return false;
            }
            
            if(isEmpty(document.all.txtAmt.value))
            {
                parent.document.all.txtWroMessage.value =('请输入消费金额！');
                return false;txtDate
            }
            
             if(isEmpty(document.all.txtDate.value))
            {
                parent.document.all.txtWroMessage.value =('请选择积分日期！');
                return false;
            }
            
             if(isEmpty(document.all.txtBonus.value))
            {
                parent.document.all.txtWroMessage.value =('请输入积分值！');
                return false;
            }
        }  
        
        //输入验证1
        function InputValidator1(sForm)
        {
             if(isEmpty(document.all.txtCardId1.value))
            {
                parent.document.all.txtWroMessage.value =('请输入会员卡号！');
                return false;
            }
            
             if(isEmpty(document.all.txtReceiptid1.value))
            {
                parent.document.all.txtWroMessage.value =('请输入小票号！');
                return false;
            }
            
            if(isEmpty(document.all.txtShopCode1.value))
            {
                parent.document.all.txtWroMessage.value =('请选择商铺！');
                return false;
            }
            
            if(isEmpty(document.all.txtAmt1.value))
            {
                parent.document.all.txtWroMessage.value =('请输入消费金额！');
                return false;txtDate
            }
            
             if(isEmpty(document.all.txtDate1.value))
            {
                parent.document.all.txtWroMessage.value =('请选择积分日期！');
                return false;
            }
            
             if(isEmpty(document.all.txtBonus1.value))
            {
                parent.document.all.txtWroMessage.value =('请输入积分值！');
                return false;
            }
           
        }
    
        //输入验证2
        function InputValidator2(sForm)
        {          
             if(isEmpty(document.all.txtRecepid.value))
            {
                parent.document.all.txtWroMessage.value =('请输入小票号！');
                return false;
            }
            
            if(isEmpty(document.all.txtShopCode.value))
            {
                parent.document.all.txtWroMessage.value =('请选择商铺！');
                return false;
            }
        
        }
        
        //对话框

        function ConfirmYesOrNo()
        {
            if(confirm("该小票积分已补登，是否继续补登操作？")==true)
            {
                document.getElementById("hiddenBtn").click();
            }
            else
            {
                return;
            
            }
            
        
        }
        
        //对话框

        function ConfirmYesOrNo1()
        {
            if(confirm("该小票已积分，是否继续补登操作？")==true)
            {
                document.getElementById("hidnBtn").click();
            }
            else
            {
                return;
            }        
        }
        
          //验证数字类型
        function textleave(form1)
        {   
            var key=window.event.keyCode;
            if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110 || key == 189 || key == 109)
            {
		        window.event.returnValue =true;
	        }
	        else
	        {
		        window.event.returnValue =false;
	        }
	    }
    </script>
	
</head>
<body style="margin-top:0; margin-left:0" onload="Load()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
    <div>
        <table  border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td class="tdTopRightBackColor" style="width: 5px">
                    <img alt="" class="imageLeftBack" />
                </td>
                <td class="tdTopRightBackColor" style="text-align:left;">
                    <asp:Label ID="Lable1" runat="server" Text="<%$ Resources:BaseInfo,Member_BonusEnter %>"></asp:Label>
                </td>
                <td class="tdTopRightBackColor" style="width: 5px">
                    <img alt="" class="imageRightBack" />
                </td>
            </tr>
             <tr style="height:1px">
                     <td colspan="3" style="background-color:White; height:1px">
                     </td>
             </tr>
        </table>
        <table style="width:100%" class="tdBackColor">
            <tr class="bodyTbl">
                <td colspan="1" style="padding-right: 20px; padding-left: 30px; width: 68px; height: 11px;
                    text-align: right">
                </td>
                <td class="lable" style="height: 11px; text-align: center">
                </td>
                <td style="width: 180px; height: 11px">
                </td>
                <td style="padding-right: 20px; padding-left: 30px; width: 68px; height: 11px; text-align: right">
                </td>
                <td class="lable" style="height: 11px; text-align: center">
                </td>
                <td class="lable" style="width: 73px; height: 11px">
                </td>
            </tr>
            <tr class="bodyTbl">
                <td colspan="1" style="padding-right: 20px; padding-left: 30px; width: 68px; height: 28px;
                    text-align: right">
                </td>
             <td style="text-align: center;" class="lable">
                    <asp:RadioButton ID="Rdo1" runat="server" Text="<%$ Resources:BaseInfo,Associator_POSReceipt %>" GroupName="Pos" CssClass="labelStyle" Checked="true" OnCheckedChanged="POSReceiptChecked"  AutoPostBack="true" /></td>
                <td style="width: 180px;">
                    &nbsp;
                </td>
                <td style="padding-right: 20px; padding-left: 30px; width: 68px; height: 28px; text-align: right">
                </td>
               
                <td class="lable" style="text-align: center;">
                <asp:RadioButton ID="Rdo2" runat="server" Text="<%$ Resources:BaseInfo,Associator_NOPOSReceipt %>" GroupName="Pos" CssClass="labelStyle" OnCheckedChanged="NoPOSReceiptChecked" AutoPostBack="true" /></td>
                <td class="lable" style="width: 73px; height: 28px">
                </td>
            </tr>
            <tr class="bodyTbl">
                <td class="lable" colspan="1">
                    <asp:Label ID="label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAssociatorCard %>"></asp:Label></td>
             <td style="width: 73px; height: 28px;" class="lable">
                 <asp:TextBox ID="txtCardID" runat="server" CssClass="ipt160px"
                     ></asp:TextBox></td>
                <td style="width: 180px; height: 28px;">
                    </td>
                <td class="lable">
                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblAssociatorCard %>"></asp:Label></td>
               
                <td class="lable" style="width: 174px; height: 28px">
                    <asp:TextBox ID="txtCardId1" runat="server" CssClass="ipt160px"
                       ></asp:TextBox></td>
                <td class="lable" style="width: 73px; height: 28px">
                </td>
            </tr>
             <tr class="bodyTbl">
                 <td class="lable" colspan="1">
                     <asp:Label ID="Label2" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTicketID %>"></asp:Label></td>
             <td style="width: 73px; height: 28px;" class="lable">
                 <asp:TextBox ID="txtRecepid" runat="server" CssClass="ipt160px"
                     ></asp:TextBox></td>
                <td style="width: 180px; height: 28px;">
                    </td>
                 <td class="lable">
                     <asp:Label ID="Label9" runat="server" Text="<%$ Resources:BaseInfo,Associator_lblTicketID %>"></asp:Label></td>
               
                <td class="lable" style="width: 174px; height: 28px">
                    <asp:TextBox ID="txtReceiptid1" runat="server"  CssClass="ipt160px"
                        ></asp:TextBox></td>
                 <td class="lable" style="width: 73px; height: 28px">
                 </td>
            </tr>
            <tr class="bodyTbl">
                <td class="lable" colspan="1" style="height: 28px">
                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
            <td style="width: 73px; height: 28px;" class="lable">
                <asp:TextBox ID="txtShopCode" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                <td style="width: 180px; height: 28px;">
                    &nbsp;<asp:Button ID="btnQuery" runat="server" CssClass="buttonQuery"  Text="<%$ Resources:BaseInfo,User_lblQuery %>" OnClick="btnQuery_Click"  /></td>
                <td class="lable" >
                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:BaseInfo,Lease_lblShopCode %>"></asp:Label></td>
                 
                <td style="width:174px; height: 28px;" class="lable">
                    <asp:TextBox ID="txtShopCode1" runat="server" CssClass="ipt160px" ></asp:TextBox></td>
                <td style="width: 73px; height: 28px; text-align: left">
                </td>
            </tr>
            <tr class="bodyTbl">
                <td class="lable" colspan="3">
                   <hr style="width:80%; " align="center"  />
                </td>
                <td class="lable">
                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:BaseInfo,Associator_InvPayAmt %>"></asp:Label></td>
                <td class="lable" style="width: 174px; height: 28px">
                    <asp:TextBox ID="txtAmt1" runat="server" AutoPostBack="true" CssClass="ipt160px" OnTextChanged="BlurAmt1Change"
                       ></asp:TextBox></td>
                <td class="lable" style="width: 73px; color: red; text-align: left">
                </td>
            </tr>
             <tr class="bodyTbl">
                 <td class="lable" colspan="1">
                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:BaseInfo,Rpt_TransId %>"></asp:Label></td>
             <td style="width: 73px; height: 28px;" class="lable">
                    <asp:TextBox ID="txtTrans" runat="server" CssClass="ipt160px" Enabled="False"></asp:TextBox></td>
                <td style="width: 180px">
                    <asp:Label ID="txtInputText" runat="server" Text="<%$ Resources:BaseInfo,Associator_InfoExists %>" Visible="false" ></asp:Label></td>
                <!--提示语-->
                 <td class="lable">
                     <asp:Label ID="Label12" runat="server" Text="<%$ Resources:BaseInfo,Associator_BonusDate %>"></asp:Label></td>
                <td class="lable" style="width:174px; height: 28px;">
                    <asp:TextBox ID="txtDate1" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                 <td class="lable" style="width: 73px; color: red; text-align: left">
                 </td>
            </tr>
             <tr class="bodyTbl">
                 <td class="lable" colspan="1">
                     <asp:Label ID="Label5" runat="server" Text="<%$ Resources:BaseInfo,Associator_InvPayAmt %>"></asp:Label></td>
             <td style="width: 73px; height: 28px;" class="lable">
                 <asp:TextBox ID="txtAmt" runat="server" AutoPostBack="true" CssClass="ipt160px"  Enabled="false"></asp:TextBox></td>
                <td style="width: 180px">
                    </td>
                 <td class="lable">
                     <asp:Label ID="Label13" runat="server" Text="<%$ Resources:BaseInfo,Associator_Itegral %>"></asp:Label></td>
                
                <td class="lable" style="width: 174px; height: 28px">
                    <asp:TextBox ID="txtBonus1" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                 <td class="lable" style="width: 73px">
                 </td>
            </tr>
             <tr class="bodyTbl">
                 <td class="lable" colspan="1">
                     <asp:Label ID="Label6" runat="server" Text="<%$ Resources:BaseInfo,Associator_BonusDate %>"></asp:Label></td>
             <td style="width: 73px; height: 28px;" class="lable">
                 <asp:TextBox ID="txtDate" runat="server" CssClass="ipt160px" onclick="calendar()"></asp:TextBox></td>
                <td style="width: 180px">
                    </td>
                 <td class="lable" style="width: 68px">
                 </td>
                
                <td class="lable" style="width: 174px; height: 28px"><asp:Button ID="btnSave1" 
                        runat="server" CssClass="buttonSave" 
                        Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" 
                        OnClick="btnSave1_Click" />
                    &nbsp;<asp:Button ID="btnCancel1"
                            runat="server" CssClass="buttonCancel"
                            Text="<%$ Resources:BaseInfo,User_btnCancel %>" 
                        OnClick="btnCancel1_Click" /></td>
                 <td class="lable" style="width: 73px">
                 </td>
            </tr>
             <tr class="bodyTbl">
                 <td class="lable" colspan="1" style="height: 44px">
                     <asp:Label ID="Label4" runat="server" Text="<%$ Resources:BaseInfo,Associator_Itegral %>"></asp:Label></td>
             <td style="width: 73px; height: 44px;" class="lable">
                 <asp:TextBox ID="txtBonus" runat="server" CssClass="ipt160px"></asp:TextBox></td>
                <td class="style1">
                    </td>
                 <td class="lable" style="width: 68px; height: 44px;">
                 </td>
               
                <td class="lable" style="width: 174px; height: 44px">
                    </td>
                 <td class="lable" style="width: 73px; height: 44px;">
                 </td>
            </tr>
         
             <tr class="bodyTbl">
                 <td colspan="1" style="padding-right: 20px; padding-left: 30px; width: 68px; text-align: right">
                 </td>
             <td style="width: 171px; height: 28px;" class="lable"><asp:Button ID="btnSave" 
                     runat="server" CssClass="buttonSave" OnClick="btnSave_Click"
                        Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" />
                 <asp:Button ID="btnQuit"
                            runat="server" CssClass="buttonCancel" OnClick="btnQuit_Click"
                            Text="<%$ Resources:BaseInfo,User_btnCancel %>" />&nbsp;&nbsp;&nbsp;&nbsp; </td>
                <td style="width: 180px">
                    </td>
                 <td style="padding-right: 20px; padding-left: 30px; width: 68px; text-align: right">
                 </td>
            
                <td style="width: 174px">
                </td>
                 <td style="width: 73px">
                 </td>
           <%-- </tr>
            <tr class="bodyTbl">
            <td colspan="4" style="padding-right: 20px; padding-left: 30px; text-align: right">
                </td>
                <td >
                </td>
                <td style="width: 180px">
                </td>
                
                <td style="width: 40%">
                </td>
            </tr>--%>
            
        </table>
        <input id="allvalue" runat="server" style="width: 25px" type="hidden" />
        <asp:TextBox ID="txtshopid" runat="server" Visible="False"></asp:TextBox>
             <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"></asp:LinkButton>
            <asp:LinkButton ID ="LinkButton2" runat="server" OnClick="LinkButton2_Click"></asp:LinkButton>
           <asp:LinkButton ID="hiddenBtn" runat = "server"  OnClick="hiddenBtn_Click"  />
           <asp:LinkButton ID="hidnBtn" runat = "server"  OnClick="hidnBtn_Click"  />
           <input id="flagHidn" runat="server" style="width: 25px" type="hidden" />
    </div>
    </ContentTemplate>
            </asp:UpdatePanel>
    </form>
</body>
</html>