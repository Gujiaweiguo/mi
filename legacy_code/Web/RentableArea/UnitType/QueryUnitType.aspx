<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryUnitType.aspx.cs" Inherits="LeaseArea_UnitType_QueryUnitType" MasterPageFile="~/BaseInfo/User/MasterPage.master"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblUnitType" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblUnitType %>"></asp:Label></td>
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
                                单元类别</div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="height: 185px" valign="middle">
                            &nbsp;<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                Style="position: relative" Width="609px" OnRowEditing="GridView1_RowEditing" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="UnitTypeID" HeaderText="UnitTypeID">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UnitTypeCode" HeaderText="单元编码" />
                                    <asp:BoundField DataField="UnitTypeName" HeaderText="单元名称" />
                                    <asp:BoundField DataField="UnitTypeStatusDesc" HeaderText="单元状态" />
                                    <asp:BoundField DataField="Note" HeaderText="备注" />
                                    <asp:CommandField ShowEditButton="True" EditText="查询" HeaderText="查询" />
                                    <asp:CommandField ShowSelectButton="True" HeaderText="作废" SelectText="作废" />
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="添加" CssClass="btnOn50px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

