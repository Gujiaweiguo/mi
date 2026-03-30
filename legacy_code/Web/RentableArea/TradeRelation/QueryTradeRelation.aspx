<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryTradeRelation.aspx.cs" Inherits="RentableArea_TradeRelation_QueryTradeRelation" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    &nbsp;<table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblTradeRelation" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblTradeRelation %>"></asp:Label></td>
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
                                经营类别</div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="height: 185px" valign="middle">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowEditing="GridView1_RowEditing"
                                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="609px">
                                <Columns>
                                    <asp:BoundField DataField="Trade3ID" HeaderText="Trade3ID">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Trade3Name" HeaderText="三级类别名称" />
                                    <asp:BoundField DataField="Trade2Name" HeaderText="二级类别名称" />
                                    <asp:BoundField DataField="Trade1Name" HeaderText="一级类别名称" />
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
</asp:Content>

