<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryLeveArea.aspx.cs" Inherits="LeaseArea_Area_QueryArea"  MasterPageFile="~/BaseInfo/User/MasterPage.master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td  rowspan="2" style="width: 40px" valign="middle">
                            <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
                        <td class="workAreaMainTitle" valign="middle">
                            <asp:Label ID="lblAreaLevel" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblAreaLevel %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="workAreaMainTitleMemo" valign="middle" style="height: 18px">
                        </td>
                    </tr>
                </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td  style="width: 50%; height: 238px" valign="middle">
                <table bgcolor="#fff4ae" border="0" cellpadding="10" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 10px">
                            <div class="boxTitle">
                                面积级别
                          </div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="height: 185px" valign="middle">
      <table >
        <tr align="center">
            <td style="width: 309px; height: 157px;" valign="middle" align="center">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                    Width="468px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowEditing="GridView1_RowEditing">
                    <Columns>
                        <asp:BoundField DataField="AreaLevelID" HeaderText="区域ID " >
                            <ItemStyle CssClass="hidden" />
                            <HeaderStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AreaLevelCode" HeaderText="级别编码" />
                        <asp:BoundField DataField="AreaLevelName" HeaderText="级别名称" />
                        <asp:BoundField DataField="AreaLevelStatusDesc" HeaderText="级别状态" />
                        <asp:BoundField DataField="Note" HeaderText="备注" />
                        <asp:CommandField EditText="修改" ShowEditButton="True" HeaderText="修改" />
                        <asp:CommandField SelectText="作废" ShowSelectButton="True" HeaderText="作废" />
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnOk" runat="server" CssClass="btnOn50px" OnClick="btnOk_Click"
                    Text="添加" /></td>
        </tr>
    </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

