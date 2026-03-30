<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GiftStockList.aspx.cs" Inherits="Associator_GiftStockList"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%=baseInfo %></title>
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
        <!--
        function Load()
	    {
	        addTabTool("<%=baseInfo %>,ReportM/RptMember/GiftStockList.aspx");
	        loadTitle();
	    }
	    //-->
	    </script>
</head>
<body style="margin:0px" onload ="Load();">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                            <table style="width:100%;" border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                     <img class="imageLeftBack" />
                                                    </td>
                                                    <td class="tdTopRightBackColor" style="text-align:left;">
                                                        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:BaseInfo,Associator_GiftStockList %>"></asp:Label>
                                                    </td>
                                                    <td style="width:5px" class="tdTopRightBackColor">
                                                        <img class="imageRightBack"/>
                                                    </td>
                                                </tr>
                                                <tr style="height:1px">
                                                    <td colspan="3" style="background-color:White; height:1px">
                                                    </td>
                                                </tr>
                                             </table>
                                              <table style="width:100%" class="tdBackColor">
                                                <tr style="height:12px">
                                                     <td style="width: 14% ">
                                                     </td>
                                                     <td style="width: 22% ">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                    <tr class="bodyTbl">
                                        <td style="width: 14%" class="lable" >
                                            <asp:Label ID="Label2" runat="server"  CssClass="labelStyle" Text="<%$ Resources:BaseInfo,Associator_GiftName %> ">
                                            </asp:Label>
                                        </td>
                                        <td style="width: 22%" class="lable" >                                                  
                                            <asp:DropDownList ID="txtGiftName" runat="server" Width="165px" >
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 50% " >
                                        </td>
                                        <td style="width: 14% " >
                                        </td>
                                    </tr>
                                    <tr class="bodyTb1">
                                        <td style="width: 14%; font-size:9pt" class="lable" >
                                            <asp:Label ID="Label3" runat="server" CssClass="lableStyle" Text="<%$ Resources:BaseInfo,Associator_ServicePaltform %> " >
                                            </asp:Label>
                                        </td>
                                        <td style="width: 22%" class="lable" >
                                        
                                            <asp:DropDownList ID="txtServer" runat="server" Width="165px">
                                            </asp:DropDownList>
                                        
                                        </td>
                                        <td style="width: 50% " >
                                        </td>
                                        <td style="width: 14% " >
                                        </td>
                                    </tr>
                                                  <tr class="bodyTb1">
                                                     <td style="width: 14%; ;">
                                                     </td>
                                                     <td style="width: 22%; ;">
                                                     </td>
                                                     <td style="width: 50%; ;">
                                                     </td>
                                                     <td style="width: 14%; ;">
                                                     </td>
                                                </tr>
                                                <tr class="bodyTb1">
                                                    <td style="width: 14%" class="lable">
                                                    </td>
                                                    <td style="width: 22%" class="lable">
                                                        <asp:Button ID="Button1" runat="server" CssClass="buttonQuery" OnClick="btnOK_Click" Text="<%$ Resources:BaseInfo,User_lblQuery %> " />
                                                        <asp:Button ID="Button2" runat="server" CssClass="buttonCancel" OnClick="BtnCel_Click" Text="<%$ Resources:BaseInfo,User_btnCancel %>" />
                                                    </td>
                                                    <td style="width: 50% " class="lable">
                                                    </td>
                                                    <td style="width: 14% " class="lable">
                                                    </td>
                                                </tr>
                                                <tr class="bodyTb1">
                                                     <td style="width: 14%">
                                                     </td>
                                                     <td style="width: 22%">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                                <tr class="bodyTb1">
                                                     <td style="width: 14%">
                                                     </td>
                                                     <td style="width: 22%">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                                  <tr class="bodyTb1">
                                                     <td style="width: 14%">
                                                     </td>
                                                     <td style="width: 22%">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                                  <tr class="bodyTb1">
                                                     <td style="width: 14%">
                                                     </td>
                                                     <td style="width: 22%">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                                  <tr class="bodyTb1">
                                                     <td style="width: 14%">
                                                     </td>
                                                     <td style="width: 22%">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                                  <tr class="bodyTb1">
                                                     <td style="width: 14%">
                                                     </td>
                                                     <td style="width: 22%">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                                  <tr class="bodyTb1">
                                                     <td style="width: 14%">
                                                     </td>
                                                     <td style="width: 22%">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                                  <tr class="bodyTb1">
                                                     <td style="width: 14%">
                                                     </td>
                                                     <td style="width: 22%">
                                                     </td>
                                                     <td style="width: 50%">
                                                     </td>
                                                     <td style="width: 14%">
                                                     </td>
                                                </tr>
                                              </table>
           </ContentTemplate>
        </asp:UpdatePanel>                                 
    </form>
</body>
</html>
