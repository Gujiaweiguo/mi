<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryLocation.aspx.cs" Inherits="LeaseArea_Location_QeryLocation" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblLocation" runat="server" Text='<%$ Resources:BaseInfo,RentableArea_lblLocation %>'></asp:Label></td>
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
                                方位定义</div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="height: 185px" valign="middle">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Width="609px" OnRowEditing="GridView1_RowEditing" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                                <Columns>
                                    <asp:BoundField DataField="LocationID" HeaderText="LocationID ">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LocationCode" HeaderText="方位编码" />
                                    <asp:BoundField DataField="LocationName" HeaderText="方位名称" />
                                    <asp:BoundField DataField="LocationStatusDesc" HeaderText="方位状态" />
                                    <asp:BoundField DataField="Note" HeaderText="备注" />
                                    <asp:CommandField EditText="修改" HeaderText="修改" ShowEditButton="True" />
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnOk" runat="server" CssClass="btnOn50px" OnClick="btnOk_Click"
                                Text="添加" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

