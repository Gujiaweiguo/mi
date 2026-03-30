<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptAdd.aspx.cs" Inherits="DeptAdd"  MasterPageFile="~/BaseInfo/User/MasterPage.master"   %>


<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left" rowspan="2" style="width: 40px" valign="middle">
                            <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
                        <td align="left" class="workAreaMainTitle" valign="middle">
                            <asp:Label ID="lblDept" runat="server" Text="<%$ Resources:BaseInfo,Dept_lblDept %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="left" class="workAreaMainTitleMemo" valign="middle" style="height: 18px">
                        </td>
                    </tr>
                </table>
                <hr style="width: 100%; size: 1;" />

    
<table width="100%" border="0" cellspacing="0" cellpadding="0">
  <tr>
    <td align="left" valign="top" style="width:50%"><table width="100%" border="0" cellspacing="0" cellpadding="10" bgcolor="#FFF4AE">
      <tr>
        <td><div class="boxTitle">基本信息定义</div></td>
      </tr>
      <tr>
        <td>
<table style="background:#ffffff"  border="0" width="100%">
                                    
                                </table>
                                <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                    <tr>
                                        <td align="center" >
                            <asp:Label ID="lblDeptCode" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblDeptCode %>' Width="48px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 426px; color: #000000; height: 25px">
                            <asp:TextBox ID="txtDeptCode" runat="server" CssClass="ipt160px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDeptCode"
                                ErrorMessage="*" Width="1px" CssClass="txtLeft"></asp:RequiredFieldValidator></td>
                                    </tr>
                                    <tr>
                                        <td align="center" >
                            <asp:Label ID="lblDeptName" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblDeptName %>' CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 426px; color: #000000; height: 25px">
                            <asp:TextBox ID="txtDeptName" runat="server" MaxLength="32" CssClass="ipt160px"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDeptName"
                                ErrorMessage="*" CssClass="txtLeft"></asp:RequiredFieldValidator></td>
                                    </tr>
                                    <tr>
                                        <td align="center" >
                            <asp:Label ID="lblDeptLevel" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblDeptLevel %>' CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 426px; color: #000000; height: 25px">
                            <asp:TextBox ID="txtDeptLevel" runat="server" MaxLength="18" CssClass="ipt160pxdisable" ReadOnly="True"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDeptName"
                                                CssClass="txtLeft" ErrorMessage="*" Width="8px"></asp:RequiredFieldValidator></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                            <asp:Label ID="lblPDeptID" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblPDeptID %>' Width="70px" CssClass="label"></asp:Label></td>
                                        <td align="left">
                            <asp:TextBox ID="txtPDeptID" runat="server" CssClass="ipt160pxdisable" ReadOnly="True" MaxLength="32"></asp:TextBox></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                            <asp:Label ID="lblDeptType" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblDeptType %>' CssClass="label"></asp:Label></td>
                                        <td align="left">
                                        <asp:DropDownList ID="ddlstDeptType" runat="server" CssClass="cmb160px">
                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                            <asp:Label ID="lblCity" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblCity %>' CssClass="label"></asp:Label></td>
                                        <td align="left">
                                         <asp:DropDownList ID="ddlstCity" runat="server" CssClass="cmb160px">
                                <asp:ListItem Value="1">北京</asp:ListItem>
                                <asp:ListItem Value="2">上海</asp:ListItem>
                                <asp:ListItem Value="3">天津</asp:ListItem>
                                <asp:ListItem Value="4">重庆</asp:ListItem>
                                <asp:ListItem Value="5">香港</asp:ListItem>
                            </asp:DropDownList></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                                <asp:Label ID="lblRegAddr" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblRegAddr %>' CssClass="label"></asp:Label></td>
                                        <td align="left">
                                <asp:TextBox ID="txtRegAddr" runat="server" MaxLength="50" CssClass="ipt160px"></asp:TextBox></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                                <asp:Label ID="lblOfficeAddr" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblOfficeAddr %>' CssClass="label"></asp:Label></td>
                                        <td align="left">
                                <asp:TextBox ID="txtOfficeAddr" runat="server" MaxLength="64" CssClass="ipt160px"></asp:TextBox></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                            <asp:Label ID="lblPostAddr" runat="server" ForeColor="Black" Text='<%$ Resources:BaseInfo,Dept_lblPostAddr %>' Width="49px" CssClass="label"></asp:Label></td>
                                        <td align="left">
                            <asp:TextBox ID="txtPostAddr" runat="server" CssClass="ipt160px" MaxLength="64"></asp:TextBox></td>
                                    </tr>
                                    
                                     <tr style="color: #000000">
                                        <td align="center">
                            <asp:Label ID="lblPostCode" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblPostCode %>' CssClass="label"></asp:Label></td>
                                        <td align="left">
                            <asp:TextBox ID="TtxtPostCode" runat="server" Height="16px" MaxLength="64" CssClass="ipt160px"></asp:TextBox></td>
                                    </tr>
                                    
                                    <tr style="color: #000000">
                                        <td align="center">
                                <asp:Label ID="lblTel" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblTel %>' CssClass="label"></asp:Label></td>
                                        <td align="left">
                                <asp:TextBox ID="txtTel" runat="server" Height="16px" MaxLength="32" CssClass="ipt160px"></asp:TextBox></td>
                                    </tr>
                                    
                                     <tr style="color: #000000">
                                        <td align="center">
                                <asp:Label ID="lblOfficeTel" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblOfficeTel %>' CssClass="label"></asp:Label><td align="left">
                            <asp:TextBox ID="txtOfficeTel" runat="server" Height="16px" MaxLength="32" CssClass="ipt160px"></asp:TextBox></tr>
                                    
                                        <tr style="color: #000000">
                                        <td align="center">
                            <asp:Label ID="lblFax" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblFax %>' CssClass="label"></asp:Label><td align="left">
                            <asp:TextBox ID="txtFax" runat="server" Height="16px" MaxLength="16" CssClass="ipt160px"></asp:TextBox></tr>
                            
                            <tr style="color: #000000">
                                        <td align="center">
                            <asp:Label ID="lblIndepBalance" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblIndepBalance %>' CssClass="label"></asp:Label><td align="left">
                            <asp:DropDownList ID="cmbIndepBalance" runat="server" CssClass="cmb160px">
                            </asp:DropDownList>
                            </tr>
                                </table>
        </td>
      </tr>
    </table></td>
    <td width="10" align="left" valign="top">&nbsp;</td>
    <td align="left" valign="top" style="width:50%"><table width="100%" border="0" cellspacing="0" cellpadding="10" bgcolor="#FFF4AE">
      <tr>
        <td><div class="boxTitle">基本功能定义</div></td>
      </tr>
      <tr>
        <td style="height: 318px">
                                <table border="0" cellpadding="2" cellspacing="1" style="azimuth: center" width="100%">
                                    <tr>
                                        <td align="center" >
                                        <asp:Label ID="lblConcessionAuth" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblConcessionAuth %>' Width="99px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 554px; color: #000000; height: 25px">
                                        <asp:TextBox ID="txtConcessionAuth" runat="server" Height="16px" MaxLength="50" CssClass="ipt230px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="center" >
                                        <asp:Label ID="lblContractAuth" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblContractAuth %>' Width="99px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 554px; color: #000000; height: 25px">
                                        <asp:TextBox ID="txtContractAuth" runat="server" Height="16px" MaxLength="50" CssClass="ipt230px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="center" >
                                        <asp:Label ID="lblTradeAuth" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblTradeAuth %>' Width="99px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 554px; color: #000000; height: 25px">
                                        <asp:TextBox ID="txtTradeAuth" runat="server" Height="16px" MaxLength="50" CssClass="ipt230px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td align="center" >
                                        <asp:Label ID="lblFeeAuth" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblFeeAuth %>' Width="99px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 554px; color: #000000; height: 25px">
                                        <asp:TextBox ID="txtFeeAuth" runat="server" Height="16px" MaxLength="50" CssClass="ipt230px"></asp:TextBox></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                                        <asp:Label ID="lblOtherAuth" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblOtherAuth %>' Width="99px" CssClass="label"></asp:Label></td>
                                        <td align="left" style="width: 554px; height: 20px">
                                        <asp:TextBox ID="txtOtherAuth" runat="server" Height="16px" MaxLength="50" CssClass="ipt230px"></asp:TextBox></td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                                            </td>
                                        <td align="left" style="width: 554px; height: 20px">
                                            </td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                                            </td>
                                        <td align="left" style="width: 554px; height: 20px">
                                            </td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                                            </td>
                                        <td align="left" style="width: 554px; height: 20px">
                                            </td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center">
                                            </td>
                                        <td align="left" style="width: 554px; height: 21px">
                                            </td>
                                    </tr>
                                    <tr style="color: #000000">
                                        <td align="center" style="height: 25px">
                                            </td>
                                        <td align="left" style="width: 554px; height: 25px">
                                            &nbsp;</td>
                                    </tr>
                                </table>
        </td>
      </tr>
    </table></td>
  </tr>
</table>
<asp:Button ID="Button1" runat="server" Height="21px" OnClick="Button1_Click" Text="添　加"
            Width="80px" CssClass="btnOn50px" />
</asp:Content>


