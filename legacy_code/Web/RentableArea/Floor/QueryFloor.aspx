<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryFloor.aspx.cs" Inherits="LeaseArea_Floor_QueryFloor" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblFloors" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblFloors %>"></asp:Label></td>
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
                                楼层定义</div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="height: 185px" valign="middle">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                Width="449px" OnRowEditing="GridView1_RowEditing" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="FloorID" HeaderText="FloorID">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FloorCode" HeaderText="楼层编码" />
                                    <asp:BoundField DataField="FloorName" HeaderText="楼层名称" />
                                    <asp:BoundField DataField="FloorStatusDesc" HeaderText="楼层状态" />
                                    <asp:BoundField DataField="Note" HeaderText="备注" />
                                    <asp:CommandField EditText="修改" HeaderText="修改" ShowEditButton="True" />
                                    <asp:CommandField HeaderText="作废" SelectText="作废" ShowSelectButton="True" />
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnOk" runat="server" OnClick="btnOk_Click" Text="添加" CssClass="btnOn50px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
