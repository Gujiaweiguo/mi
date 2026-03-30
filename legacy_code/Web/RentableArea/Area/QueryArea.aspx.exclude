<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryArea.aspx.cs" Inherits="LeaseArea_Area_QueryArea"  MasterPageFile="~/BaseInfo/User/MasterPage.master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblArea" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblArea %>"></asp:Label></td>
        </tr>
        <tr>
            <td class="workAreaMainTitleMemo" style="height: 18px" valign="middle">
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 50%; height: 238px" valign="middle">
                <table bgcolor="#fff4ae" border="0" cellpadding="10" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 10px">
                            <div class="boxTitle">
                                经营区域&nbsp;</div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="height: 185px" valign="middle">
                            <table>
                                <tr align="center">
                                    <td align="center" style="width: 309px; height: 157px" valign="middle">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                    Width="468px" OnRowEditing="GridView1_RowEditing" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="AreaID" HeaderText="区域ID " >
                            <ItemStyle CssClass="hidden" />
                            <HeaderStyle CssClass="hidden" />
                            <FooterStyle CssClass="hidden" />
                        </asp:BoundField>
                        <asp:BoundField DataField="AreaCode" HeaderText="区域编码" />
                        <asp:BoundField DataField="AreaName" HeaderText="区域名称" />
                        <asp:BoundField DataField="AreaStatusDesc" HeaderText="区域状态" />
                        <asp:BoundField DataField="Note" HeaderText="备注" />
                        <asp:CommandField EditText="修改" HeaderText="修改" ShowEditButton="True" />
                        <asp:CommandField HeaderText="作废" SelectText="作废" ShowSelectButton="True" />
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

