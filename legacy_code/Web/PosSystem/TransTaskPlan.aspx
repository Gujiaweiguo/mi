<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TransTaskPlan.aspx.cs" Inherits="PosSystem_TransTaskPlan" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%= (String)GetGlobalResourceObject("BaseInfo", "Store_TaskManage")%></title>
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlstree-basic.css" type="text/css" />
    <link rel="StyleSheet" href="../App_Themes/nlstree/nlsctxmenu.css" type="text/css" />
    <script type="text/javascript" src="../App_Themes/nlstree/nlstree.js"></script>
    <script type="text/javascript" src="../App_Themes/nlstree/nlsctxmenu.js"></script>
	<script type="text/javascript" src="../JavaScript/Common.js"></script>	
	<script language="javascript" type="text/javascript" src="../JavaScript/TabTools.js"></script>
	<script type="text/javascript" src="../JavaScript/Calendar.js" charset="gb2312"></script>
<script type="text/javascript">

    //text控件文本验证
    function textBoxCheck()
    {
    if(isEmpty(document.all.txtStartDate.value))  
        {
            parent.document.all.txtWroMessage.value="日期不能为空!";
            document.all.txtStartDate.focus();
            return false;					
        }
        if(!isDigit(document.all.txtStartTimeH.value) || isEmpty(document.all.txtStartTimeH.value))  
        {
            parent.document.all.txtWroMessage.value="时间格式不正确!";
            document.all.txtStartTimeH.focus();
            return false;					
        }
        if(!isDigit(document.all.txtStartTimeM.value) || isEmpty(document.all.txtStartTimeM.value))  
        {
            parent.document.all.txtWroMessage.value="时间格式不正确!";
            document.all.txtStartTimeM.focus();
            return false;					
        }
        if(!isDigit(document.all.txtEndTimeH.value) || isEmpty(document.all.txtEndTimeH.value))  
        {
            parent.document.all.txtWroMessage.value="时间格式不正确!";
            document.all.txtEndTimeH.focus();
            return false;					
        }
        if(!isDigit(document.all.txtEndTimeM.value) || isEmpty(document.all.txtEndTimeM.value))  
        {
            parent.document.all.txtWroMessage.value="时间格式不正确!";
            document.all.txtEndTimeM.focus();
            return false;					
        }
        
    }
    function BtnUp( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
    }
    function BtnOver( p )
    {
	    var t = String(p)
	    var l = t.substring(3,15); 
	    document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
    }
		-->
</script>

    <style type="text/css">
        .style1
        {
            height: 6px;
            width: 157px;
        }
        .style2
        {
            height: 26px;
            width: 157px;
        }
        .style3
        {
            height: 24px;
            width: 157px;
        }
        </style>

</head>
<body onload='' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="depttxt" runat="server" EnableViewState="False" />
                <asp:HiddenField ID="selectdeptid" runat="server" />
                <asp:HiddenField ID="deptid" runat="server"  />
                <table border="0" cellpadding="0" cellspacing="0" style="height: 430px; width:98%" >
                    <tr>
                        <td style="width: 35%; height: 401px; vertical-align: top;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 255px; vertical-align:top;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" style="width: 266px; height: 27px;">
                                        <img alt="" class="imageLeftBack" /><asp:Label ID="labAreaVindicate" runat="server" CssClass="lblTitle"
                                            Text="<%$ Resources:BaseInfo,Store_TaskItems %>"></asp:Label></td>
                                    <td class="tdTopRightBackColor" valign="top" style="height: 27px">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="2" style="height: 1px" class="tdTopBackColor">
                                    </td>
                                </tr>
                                <tr>
                                
                                    <td class="tdBackColor" colspan="2" rowspan="10" style="height: 340px; text-align: center"
                                        valign="top">
                                        
                                        <table style="height: 423px">
                                                  <tr>
                                        <td style="height: 320px; width: 258px;">
                                                        <asp:GridView ID="gvTransTask" runat="server" AutoGenerateColumns="False" 
                                                            Height="100%" Width="90%" onrowdatabound="gvTransTask_RowDataBound" 
                                                            AllowPaging="True" onpageindexchanging="gvTransTask_PageIndexChanging" 
                                                            onselectedindexchanged="gvTransTask_SelectedIndexChanged" BackColor="White" 
                                                            BorderStyle="Inset">
                                                            <Columns>
                                                            <asp:BoundField DataField="TaskID">
                                                             <ItemStyle CssClass="hidden" />
                                                             <HeaderStyle CssClass="hidden" />
                                                             <FooterStyle CssClass="hidden" />
                                                         </asp:BoundField>
                                                                <asp:BoundField DataField="TaskName" 
                                                                    HeaderText="<%$ Resources:BaseInfo,Store_TaskName %>">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:BoundField>                                                                    
                                                                <asp:CommandField HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" 
                                                                    ShowSelectButton="True">
                                                                    <ItemStyle BorderColor="#E1E0B2" />
                                                             <HeaderStyle BackColor="#E1E0B2" BorderColor="#E1E0B2" CssClass="gridviewtitle" />
                                                                    </asp:CommandField>
                                                            </Columns>
                                                            <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="right" />
<PagerTemplate>                                                   
<asp:LinkButton ID="LinkButtonFirstPage" runat="server" CommandArgument="First" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Small">首页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonPreviousPage" runat="server" CommandArgument="Prev" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != 0 %>" Font-Size="Small">上一页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonNextPage" runat="server" CommandArgument="Next" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Small">下一页</asp:LinkButton> 

<asp:LinkButton ID="LinkButtonLastPage" runat="server" CommandArgument="Last" CommandName="Page" 
 Visible="<%# ((GridView)Container.NamingContainer).PageIndex != ((GridView)Container.NamingContainer).PageCount - 1 %>" Font-Size="Small">尾页</asp:LinkButton> 
<asp:textbox id="txtNewPageIndex" runat="server" width="20px" text='<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>' />/
<asp:Label ID="LabelPageCount" runat="server" 
 Text="<%# ((GridView)Container.NamingContainer).PageCount %>"></asp:Label> 
<asp:linkbutton id="btnGo" runat="server" causesvalidation="False" commandargument="-1" commandname="Page" text="GO" Font-Size="Small"/> 
  </PagerTemplate>         
<PagerSettings Mode="NextPreviousFirstLast"  />

                                                        </asp:GridView>
                                        </td>
                                        </tr>
                                            <tr>
                                                <td style="height: 53px; width: 258px;" valign="top"><table border="0" cellpadding="0" cellspacing="0" style="width: 231px">
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </table>
                                                </td>
                                            </tr>
                                            
                                        </table>
                        </td>
                                </tr>
                            </table>
                        </td>
                        <td style="height: 401px; width: 4%;">
                        </td>
                        <td style="width: 55%; height: 401px; vertical-align: top;">
                            <table border="0" cellpadding="0" cellspacing="0" style="height: 288px; vertical-align:top;" width="100%">
                                <tr>
                                    <td class="tdTopBackColor" valign="top">
                                        <img alt="" class="imageLeftBack" /><asp:Label
                                                ID="labAreaTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,Store_TransTaskPlanManage %>"></asp:Label><a
                                                    style="font-size: 18px"></a></td>
                                    <td class="tdTopRightBackColor" valign="top">
                                        &nbsp;<img class="imageRightBack" /></td>
                                </tr>
                                <tr height="1">
                                    <td colspan="2" class="tdTopBackColor">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdBackColor" colspan="2" rowspan="10" style="height: 365px; text-align: center"
                                        valign="top">
                                        <table border="0" cellpadding="0" cellspacing="0" 
                                            style="width: 100%; height: 422px">
                                            <tr>
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td style="width: 194px; height: 8px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style2">
                                                    <asp:Label ID="labTaskName" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_TaskName %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 26px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:TextBox ID="txtTaskName" runat="server" CssClass="ipt160px" MaxLength="16" 
                                                        Width="146px"></asp:TextBox></td>
                                            </tr>
                                            <tr style="height: 6px;">
                                                <td style="text-align: right; " class="style1">
                                                    &nbsp;</td>
                                                <td style="height: 6px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; text-align: left; height: 6px;">
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="labRunType" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Store_RunType %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 24px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:DropDownList ID="ddlRunType" runat="server" CssClass="cmb150px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr style="height: 6px;">
                                                <td style="text-align: right" class="style1">
                                                    </td>
                                                <td style="height: 6px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 6px; text-align: left">
                                                    </td>
                                            </tr>
                                            
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblStartDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,PotShop_lblShopStartDate %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 24px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 28px; text-align: left">
                                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="ipt160px" 
                                                        MaxLength="32" Width="146px" onclick="calendar()"></asp:TextBox></td>
                                            </tr>                                            
                                            
                                                                                        <tr style="height: 6px;">
                                                <td style="text-align: right" class="style1">
                                                    </td>
                                                <td style="height: 6px; width: 5px;">
                                                    &nbsp;</td>
                                                <td style="width: 194px; height: 6px; text-align: left">
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblEndDate" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_EDate %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="height: 24px; width: 5px;">
                                                   </td>
                                                <td style="width: 194px; text-align: left" rowspan="1">
                                                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="ipt160px" Width="146px" onclick="calendar()"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="height: 6px;">
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    &nbsp;</td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    <asp:CheckBox ID="chbNoEndDate" runat="server" text="<%$ Resources:BaseInfo,Store_NoEndDate %>"/>
                                                </td>
                                            </tr>
                                            <tr style="height: 6px;">
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblStartTime" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Rpt_StartTime %>"
                                                        Width="90px"></asp:Label></td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    <asp:TextBox ID="txtStartTimeH" runat="server" CssClass="ipt160px" MaxLength="2" Width="30px"></asp:TextBox>
                                                    &nbsp;:
                                                    <asp:TextBox ID="txtStartTimeM" runat="server" CssClass="ipt160px" Width="30px" MaxLength="2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="height: 6px;">
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    <asp:Label ID="lblEndTime" runat="server" CssClass="labelStyle" 
                                                        Text="<%$ Resources:BaseInfo,ConLease_labEndDate %>" Width="90px"></asp:Label>
                                                </td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    <asp:TextBox ID="txtEndTimeH" runat="server" CssClass="ipt160px" Width="30px" MaxLength="2"></asp:TextBox>
                                                    &nbsp;:
                                                    <asp:TextBox ID="txtEndTimeM" runat="server" CssClass="ipt160px" Width="30px" MaxLength="2"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr style="height: 6px;">
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: right" class="style3">
                                                    &nbsp;</td>
                                                <td style="width: 5px; height: 24px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                    &nbsp;</td>
                                            </tr>
                                            <tr style="height: 6px;">
                                                <td style="text-align: right" class="style1">
                                                </td>
                                                <td style="width: 5px; height: 6px">
                                                </td>
                                                <td rowspan="1" style="width: 194px; text-align: left">
                                                </td>
                                            </tr>
                                                    <tr>
                                                        <td colspan="3" style="height: 22px; text-align: center">
                                                            &nbsp;<table border="0" cellpadding="0" cellspacing="0" style="width: 80%" id="TABLE1" onclick="return TABLE1_onclick()">
                                                                <tr>
                                                                    <td style="width: 100%; height: 1px; background-color: #738495; position: relative; top: 15px;">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100%; height: 1px; background-color: #ffffff; position: relative;
                                                                        top: 15px;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                            </tr>
                                              <tr>
                                                  <td colspan="3" style="height: 10px; text-align: left">
                                                  </td>
                                            </tr>                      
                                              <tr>
                                                  <td colspan="3" style="height: 30px; text-align: right; position: relative; top: 15px;">
                                                      &nbsp;<asp:Button ID="btnSave"
                                                                  runat="server" CssClass="buttonSave" 
                                                          Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" 
                                                          onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" 
                                                          onmouseup="BtnUp(this.id);" onclick="btnSave_Click" Enabled="False"/>&nbsp;&nbsp;<asp:Button
                                                                      ID="btnCancel" runat="server" CssClass="buttonCancel"
                                                                      onmouseout="BtnUp(this.id);" onmouseover="BtnOver(this.id);" onmouseup="BtnUp(this.id);"
                                                                      Text="<%$ Resources:BaseInfo,User_btnCancel %>" 
                                                          onclick="btnCancel_Click" />
                                                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                  </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" style="height: 30px; text-align: right">
                                                   </td>
                                            </tr>
                                        </table>
                                      </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidAreaNotSelect" runat="server" Value="<%$ Resources:BaseInfo,Hidden_AreaNotSelect %>" />
        <asp:HiddenField ID="hidAddArea" runat="server" Value="<%$ Resources:BaseInfo,Dept_TitleAdd %>" />
        <asp:HiddenField ID="hidlblUnitit" runat="server" Value="<%$ Resources:BaseInfo,Menu_PosServerVindicate %>" />
    </form>
</body>
</html>