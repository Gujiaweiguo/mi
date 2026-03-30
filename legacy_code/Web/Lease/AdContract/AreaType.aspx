<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AreaType.aspx.cs" Inherits="Lease_AdContract_AreaType" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
        <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../JavaScript/Common.js"></script>
     <script type="text/javascript">
    
        function showline()
        {
		    GridView1.borderColor="#e1e0b2";
            GridView1.borderWidth="1px";
           // parent.document.all.txtWroMessage.value = "";
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        function showlineIns()
        {
		    GridView1.borderColor="#e1e0b2";
            GridView1.borderWidth="1px";
           // parent.document.all.txtWroMessage.value = document.getElementById("hidAdd").value;
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        function showlineError()
        {
		    GridView1.borderColor="#e1e0b2";
            GridView1.borderWidth="1px";
            //parent.document.all.txtWroMessage.value = document.getElementById("hidInsert").value;
            document.getElementById("lblTotalNum").style.display="none";
            document.getElementById("lblCurrent").style.display="none";
        }
        	//text控件文本验证
    function BizGrpValidator(sForm)
    {
        if(isEmpty(document.all.txtBizGrpCode.value))  
        {
            //parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtBizGrpCode.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtBizGrpName.value))  
        {
            //parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtBizGrpName.focus();
            return false;					
        }
    }

    </script>
</head>
<body onload='showline();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
    <div>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <table id="showmain" border="0" cellpadding="0" cellspacing="0" class="tableBoderStyle"
                style="height: 445px">
                <tr height="15">
                    <td colspan="8">
                    </td>
                </tr>
                <tr>
                    <td style="width: 95px; height: 401px; text-align: center" valign="top">
                        <img height="401" src="../../images/shuxian.jpg" />
                    </td>
                    <td colspan="5" style="vertical-align: top; width: 580px; height: 401px">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 553px; height: 400px">
                                    <tr>
                                        <td class="tdTopBackColor" style="vertical-align: middle; width: 287px; height: 25px;
                                            text-align: left" valign="top">
                                            <img alt="" class="imageLeftBack" style="height: 25px" />
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
                                                    <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                        <asp:Label ID="lblAreaTypeCode" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeCode %>" Width="71px"></asp:Label></td>
                                                    <td class="tdBackColor" style="width: 8px; height: 30px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                        <asp:TextBox ID="txtAreaTypeCode" runat="server" CssClass="textstyle"></asp:TextBox></td>
                                                    <td class="tdBackColor" style="height: 30px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                        <asp:Label ID="lblAreaTypeDesc" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeDesc %>"
                                                            Width="87px"></asp:Label></td>
                                                    <td class="tdBackColor" style="width: 7px; height: 30px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                        <asp:TextBox ID="txtAreaTypeDesc" runat="server" CssClass="textstyle"></asp:TextBox></td>
                                                    <td class="tdBackColor" style="width: 100px; height: 30px">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" style="width: 293px; height: 30px; text-align: right">
                                                        <asp:Label ID="lblAreaTypeStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeStatus %>"
                                                            Width="71px"></asp:Label></td>
                                                    <td class="tdBackColor" style="width: 8px; height: 30px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 232px; height: 30px; text-align: left">
                                                        <asp:DropDownList ID="cmbAreaTypeStatus" runat="server" BackColor="White" CssClass="cmb160px"
                                                            Width="124px">
                                                        </asp:DropDownList></td>
                                                    <td class="tdBackColor" style="height: 30px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 111px; height: 30px; text-align: right">
                                                        </td>
                                                    <td class="tdBackColor" style="width: 7px; height: 30px">
                                                    </td>
                                                    <td class="tdBackColor" style="width: 127px; height: 30px; text-align: left">
                                                        </td>
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
                                                    <td colspan="8">
                                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                            BorderColor="#E1E0B2" BorderStyle="None" BorderWidth="1px" CellPadding="3" Height="197px"
                                                            OnRowDataBound="GridView1_RowDataBound" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                                                            PageSize="7" Width="531px">
                                                            <Columns>
                                                                <asp:BoundField DataField="AreaTypeID">
                                                                    <ItemStyle CssClass="hidden" />
                                                                    <HeaderStyle CssClass="hidden" />
                                                                    <FooterStyle CssClass="hidden" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AreaTypeCode" HeaderText="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeCode %>">
                                                                    <HeaderStyle CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AreaTypeDesc" HeaderText="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeDesc %>">
                                                                    <HeaderStyle CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="AreaTypeStatus" HeaderText="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeStatus %>">
                                                                    <HeaderStyle CssClass="gridviewtitle" />
                                                                </asp:BoundField>
                                                                <asp:CommandField HeaderText="<%$ Resources:BaseInfo,User_btnChang %>" ShowSelectButton="True">
                                                                    <HeaderStyle CssClass="gridviewtitle" />
                                                                </asp:CommandField>
                                                            </Columns>
                                                            <FooterStyle BackColor="Red" ForeColor="#000066" />
                                                            <RowStyle Font-Overline="False" Font-Size="10pt" ForeColor="Black" Height="10px" />
                                                            <SelectedRowStyle BackColor="#FFFFCD" Font-Bold="False" ForeColor="Black" />
                                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                                            <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Height="10px" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdBackColor" colspan="3" style="height: 53px">
                                                        &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Height="31px"
                                                            OnClick="btnSave_Click" Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>"
                                                            Width="70px" />
                                                        <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit" Enabled="False" Height="30px"
                                                            OnClick="btnEdit_Click" Text="<%$ Resources:BaseInfo,User_btnChang %>" Width="70px" />
                                                        <asp:Label ID="lblTotalNum" runat="server"></asp:Label>
                                                        <asp:Label ID="lblCurrent" runat="server" ForeColor="Red">1</asp:Label></td>
                                                    <td class="tdBackColor" style="height: 53px">
                                                    </td>
                                                    <td class="tdBackColor" colspan="4" style="left: 30px; vertical-align: middle; width: 270px;
                                                        height: 53px; text-align: left">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 257px" id="TABLE1" onclick="return TABLE1_onclick()">
                                                            <tbody>
                                                                <tr>
                                                                    <td style="left: 3px; width: 160px; position: relative; top: -5px; height: 1px; background-color: #738495">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="left: 3px; width: 160px; position: relative; top: -5px; height: 1px; background-color: #ffffff">
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="height: 30px; text-align: right">
                                                                        <asp:Button ID="btnBack" runat="server" CssClass="buttonBack" Enabled="False" OnClick="btnBack_Click"
                                                                            Text="<%$ Resources:BaseInfo,Button_back %>" /><asp:Button ID="btnNext" runat="server"
                                                                                CssClass="buttonNext" Enabled="False" OnClick="btnNext_Click" Text="<%$ Resources:BaseInfo,Button_next %>" /></td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 60px; height: 401px; text-align: center" valign="top">
                        <img height="401" src="../../images/shuxian.jpg" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hidAreaTypeCode" runat="server" Value="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeCode %>" />
        <asp:HiddenField ID="hidAreaTypeDesc" runat="server" Value="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeDesc %>" />
        <asp:HiddenField ID="hidAreaTypeStatus" runat="server" Value="<%$ Resources:BaseInfo,LeaseAreaType_AreaTypeStatus %>" />
        <asp:HiddenField ID="hidAreaTypeChang" runat="server" Value="<%$ Resources:BaseInfo,User_btnChang %>" />
        <asp:HiddenField ID="hidChang" runat="server" Value="<%$ Resources:BaseInfo,User_btnChang %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidUpdate" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidUpdate %>" />
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidMessageError" runat="server" Value="<%$ Resources:BaseInfo,BizGrp_MessageError %>" />
    
    </div>
    </form>
</body>
</html>
