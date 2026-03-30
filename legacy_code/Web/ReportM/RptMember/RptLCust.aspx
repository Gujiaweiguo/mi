
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RptLCust.aspx.cs" Inherits="ReportM_Associator_RptLCust" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--
 * 修改人：hesijian
 * 
 * 修改时间：2009年7月6日 
 * 
 * 编码类型：Modify(修改)
-->

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        <!--
            table.mainTbl {width:572px;height:401px;}
            
            tr{height:28px;}
            td.lable{padding-right:5px;text-align:right;}
            
        -->
    </style>
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
    <script type="text/javascript" src="../../JavaScript/Calendar.js" charset="gb2312"></script>
    <script language="javascript" type="text/javascript" src="../../JavaScript/TabTools.js"></script>
        <script type="text/javascript">
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptMember/RptLCust.aspx");
	        loadTitle();
	    }
	    
	    function checkNm()
	    {
	        
	        if ( !(((window.event.keyCode >= 48) && (window.event.keyCode <= 57)) 
                || (window.event.keyCode == 13) || (window.event.keyCode == 46) 
                || (window.event.keyCode == 45)))
            {
                window.event.keyCode = 0 ;
            }
	    }
	    </script>
	
</head>
<body style="margin:0px" onload="Load();">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td class="tdTopRightBackColor" style="width: 5px; ">
                            <img class="imageLeftBack" /></td>
                        <td class="tdTopRightBackColor" style="text-align: left;">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_QueryAssociator %>" ></asp:Label>
                        </td>
                        <td class="tdTopRightBackColor" style="width: 5px;">
                            <img class="imageRightBack" />
                        </td>
                    </tr>
                    <tr style="height: 1px">
                        <td colspan="3" style="height: 1px; background-color: white">
                        </td>
                    </tr>
                </table>
              
                <table class="tdBackColor" style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="height: 10px">
                        <td style="width: 145px; height: 10px;">
                        </td>
                        <td style="width: 121px; height: 10px;">
                        </td>
                        <td style="width: 80px; height: 10px;">
                        </td>
                        <td style="width: 300px; height: 10px;">
                        </td>
                    </tr>
<!--会员编码-->
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 12px">
                            <asp:Label ID="Label2" runat="server" CssClass="labelStyle" Text="<%$Resources:BaseInfo,Associator_MemberCode %>"></asp:Label><br />
                        </td>
                        <td style="width: 121px; height: 12px" >
                            <asp:TextBox ID="txtCustCode" runat="server" CssClass="ipt160px" Height="20px" Width="170px"></asp:TextBox><br />
                        </td>
<!--会员生日-->                            
                        <td class="lable" style=" height: 12px; width: 80px;" >
                            &nbsp;<asp:Label ID="Label22" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_MemberBirthday %>" Width="59px" ></asp:Label></td>
                        <td style="height: 12px; width: 300px;">
                      <asp:DropDownList ID="txtEndMonth" runat="server"  Height="20px" Width="52px" >
                      </asp:DropDownList><asp:DropDownList ID="txtStartDay" runat="server"  Height="20px" Width="54px" >
                                      </asp:DropDownList><asp:DropDownList ID="txtStartMonth" runat="server"  Height="20px" Width="52px" >
                                   
                                       </asp:DropDownList><asp:DropDownList ID="txtEndDay" runat="server"  Height="20px" Width="50px" >
                      </asp:DropDownList></td>
                    </tr>
<!--姓名-->
                    <tr class="bodyTbl">
                        <td class="lable" style=" height: 15px; width: 145px;">
                            <asp:Label ID="Label21" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,User_lblUserName %>"></asp:Label></td>
                        <td style="width: 121px; height: 15px">
                            <asp:TextBox ID="txtName" runat="server" CssClass="ipt160px" Height="19px" Width="170px"></asp:TextBox></td>
   
<!--年龄范围-->
   <td  class="lable" style="width: 80px; height: 15px; vertical-align: top;">
       &nbsp;<asp:Label ID="Label23" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AgeArea %>" Width="56px" ></asp:Label></td>
                        <td style="height: 15px; width: 300px; vertical-align: top;">
                        <asp:DropDownList ID="txtYearArea" runat="server" Width="167px"></asp:DropDownList></td>
                    </tr>

<!-----证件号码---->
                     <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 11px">
                            <asp:Label ID="Label8" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorIdentity %>" Width="50px" Height="15px"></asp:Label><br />
                        </td>
                        <td style="width: 121px; height: 11px">
                            <asp:TextBox ID="txtID" runat="server" CssClass="ipt160px" Height="19px" Width="170px" ></asp:TextBox><br />
                        </td>
                         <td  class="lable" style="width: 80px; height: 11px; vertical-align: text-top;">
                                        &nbsp;<asp:Label ID="Label3" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorGender %>" Width="54px" Height="13px" Font-Italic="False" ></asp:Label></td>
                        <td style="height: 11px; width: 300px; vertical-align: middle;">
                        <asp:DropDownList ID="txtGender" runat="server" Width="167px" Height="28px"></asp:DropDownList></td>
                    </tr>

<!--移动电话-->
 <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 15px">
                            <asp:Label ID="Label4" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorMobileTel %>"></asp:Label></td>
                        <td style="width: 121px; height: 15px">
                            <asp:TextBox ID="txtTel" runat="server" CssClass="ipt160px" Height="16px" Width="170px" onKeyPress="checkNm()"></asp:TextBox></td>
                            
<!--国家/地区-->
  <td  class="lable" style="width: 80px; height: 15px">
      &nbsp;<asp:Label ID="Label9" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_CountryOrArea %>" Width="51px" ></asp:Label></td>
                        <td style="height: 8px; width: 300px;">
                        <asp:DropDownList ID="txtNationalOrArea" runat="server" Width="167px"></asp:DropDownList></td>
                    </tr>
<!----地址---->
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 16px">
                            <asp:Label ID="Label7" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,RentableArea_lblBuildingAddr %>"></asp:Label></td>
                        <td style="width: 121px; height: 16px">
                            <asp:TextBox ID="txtLAddr" runat="server" CssClass="ipt160px" Height="16px" Width="170px"></asp:TextBox></td>

<!--婚姻状况-->                            
                         <td  class="lable" style="width: 80px; height: 16px">
                             &nbsp;<asp:Label ID="Label15" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorMarriage %>" Width="54px" ></asp:Label></td>
                        <td style="height: 16px; width: 300px;">
                        <asp:DropDownList ID="txtIsMarry" runat="server" Width="167px"></asp:DropDownList></td>
                    </tr>
                    
<!---入会日期--->
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 8px">
                            <asp:Label ID="Label12" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_lblInitiateDate %>"></asp:Label></td>
                        <td style="width: 190px; height: 8px">
                            <asp:TextBox ID="txtStartBizTime" runat="server" CssClass="ipt160px" Height="18px"
                                onclick="calendar()" Width="79px"></asp:TextBox>
                            <asp:TextBox ID="txtEndBizTime"  runat="server" CssClass="ipt160px" Height="18px"
                                onclick="calendar()" Width="83px"></asp:TextBox></td>
   
<!--结婚纪念日-->                            
                        <td class="lable" style="width: 80px; height: 8px" >
                            &nbsp;<asp:Label ID="Label16" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_MemberMarryDate %>" Width="63px" ></asp:Label></td>
                        <td style="height: 8px; width: 300px;">
                                        <asp:DropDownList ID="txtEndMarryMonth" runat="server"  Height="20px" Width="55px" >
                      </asp:DropDownList><asp:DropDownList ID="txtStartMarryDay" runat="server"  Height="20px" Width="54px" >
                                      </asp:DropDownList><asp:DropDownList ID="txtStartMarryMonth" runat="server"  Height="20px" Width="50px" >
                                    
                                       </asp:DropDownList><asp:DropDownList ID="txtEndMarryDay" runat="server"  Height="20px" Width="49px" >
                      </asp:DropDownList></td>
                        <td style="height: 8px">
                            </td>
                    </tr>
<!---积分范围--->
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 27px">
                            <asp:Label ID="Label10" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_BonusArea %>"></asp:Label></td>
                        <td style="width: 190px; height: 27px">
                            <asp:TextBox ID="txtFirstNumber" runat="server" CssClass="ipt160px"  Height="18px"
                                 Width="79px" onkeypress="checkNm()"></asp:TextBox>
                            <asp:TextBox ID="txtSecondNumber" runat="server" CssClass="ipt160px" Height="18px"
                                 Width="83px" onkeypress="checkNm()"></asp:TextBox></td>
                                 
<!--收入水平-->                                 
                       <td  class="lable" style="width: 80px; height: 27px;">
                           &nbsp;<asp:Label ID="Label17" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorEarning %>" Width="57px" ></asp:Label></td>
                        <td style="width: 300px; height: 27px;">
                            <asp:DropDownList ID="txtIncome" runat="server" Width="167px"></asp:DropDownList></td>
                    </tr>
<!---会员来源--->
                     <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 17px">
                            <asp:Label ID="Label11" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorOrigin %>" Width="49px"></asp:Label></td>
                        <td style="width: 121px; height: 17px">
                            <asp:DropDownList ID="txtMemberFrom" runat="server" Width="174px">
                            </asp:DropDownList></td>
                            
<!---职业--->
                         <td class="lable" style="width: 80px; height: 17px">
                                        <asp:Label ID="Label18" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorOccupation %>" Width="58px" ></asp:Label></td>
                        <td  class="lable" style="width: 300px; height: 17px; text-align: left;">
                        <asp:DropDownList ID="txtCreer" runat="server" Width="167px"></asp:DropDownList></td>
                        <td style="height: 17px; width: 1px;">
                            </td>
                    </tr>
<!---消费兴趣--->        
                     <tr class="bodyTbl" >
                        <td class="lable" style="width: 145px; height: 17px;">
                            <asp:Label ID="Label5" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_ConsumeInterest %>" ></asp:Label></td>
                    <td  style="height: 17px" colspan="3" align="left">
                        <asp:CheckBoxList ID="Buy" RepeatDirection="Horizontal" RepeatColumns="8" runat="server" CssClass="labelStyle" Width="457px" Height="31px" DataTextField="IItemName" DataValueField="IItemID">
                                 </asp:CheckBoxList></td>
                     </tr>    
                     
<!---个人爱好--->
                     <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 17px">
                            <asp:Label ID="Label6" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_PersonFavor %>"></asp:Label></td>
                    <td  style="height: 17px" colspan="3" align="left">
                        <asp:CheckBoxList ID="txtPersonFunny" RepeatDirection="Horizontal" RepeatColumns="8" runat="server" CssClass="labelStyle" Width="457px" Height="31px" DataTextField="FItemName" DataValueField="FItemID" >
                                 </asp:CheckBoxList></td>
                     </tr>  
<!--活动讯息-->
                     <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 17px">
                            <asp:Label ID="Label19" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_ActivityInfomation %>"></asp:Label></td>
                     <td  style="height: 17px" colspan="3" align="left">
                        <asp:CheckBoxList ID="txtAction" RepeatDirection="Horizontal" RepeatColumns="6" runat="server" CssClass="labelStyle" Width="457px" Height="31px" DataTextField="AItemName" DataValueField="AItemID" >
                                 </asp:CheckBoxList></td>
                     </tr>  
<!--备注-->     
              <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 17px">
                            <asp:Label ID="Label20" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_AssociatorRemark %>"></asp:Label></td>
                    <td style="width: 121px; height: 17px" >
                            <asp:TextBox ID="txtComment" runat="server" CssClass="ipt160px" Height="18px" Width="230px"></asp:TextBox></td>
                               <td style="height: 17px" colspan="2" align="left">
                            <asp:Button ID="Button1" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click"
                                Text="<%$ Resources:BaseInfo,User_lblQuery %> " />&nbsp;<asp:Button ID="Button2" runat="server"
                                    CssClass="buttonCancel" OnClick="BtnCel_Click" 
                                       Text="<%$ Resources:BaseInfo,User_btnCancel %> " /></td>
                     </tr>  

                  
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 10px">
                        </td>
                        <td align="right" class="lable" colspan="3" style="height: 10px">
                            &nbsp;
                        </td>
                    </tr>
       
                  
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 10px">
                        </td>
                        <td style="height: 10px" colspan="3">
                        </td>
                    </tr>
                  
                    <tr class="bodyTbl">
                        <td class="lable" style="width: 145px; height: 4px">
                        </td>
                        <td style="height: 4px" colspan="3">
                        </td>
                    </tr>
                </table>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
