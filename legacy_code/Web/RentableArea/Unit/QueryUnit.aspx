<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QueryUnit.aspx.cs" Inherits="LeaseArea_Unit_QueryUnit" MasterPageFile="~/BaseInfo/User/MasterPage.master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td rowspan="2" style="width: 40px" valign="middle">
                <img alt="" height="32" src="../../App_Themes/CSS/Images/iconNew32x32.gif" width="32" /></td>
            <td class="workAreaMainTitle" style="height: 20px" valign="middle">
                <asp:Label ID="lblUnit" runat="server" Text="<%$ Resources:BaseInfo,RentableArea_lblUnit %>"></asp:Label></td>
        </tr>
        <tr>
            <td class="workAreaMainTitleMemo" style="height: 18px" valign="middle">
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="width: 50%; height: 245px" valign="middle">
                <table bgcolor="#fff4ae" border="0" cellpadding="10" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 10px">
                            <div class="boxTitle">
                                单元定义</div>
                        </td>
                    </tr>
                    <tr align="center">
                        <td style="height: 185px" valign="middle">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="gridview"
                                Height="217px" Width="1161px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowEditing="GridView1_RowEditing">
                                <Columns>
                                    <asp:BoundField DataField="UnitID" HeaderText="UnitID">
                                        <ItemStyle CssClass="hidden" />
                                        <HeaderStyle CssClass="hidden" />
                                        <FooterStyle CssClass="hidden" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BuildingName" HeaderText="大楼" />
                                    <asp:BoundField DataField="UnitTypeName" HeaderText="单元" />
                                    <asp:BoundField DataField="AreaSizeName" HeaderText="面积大小" />
                                    <asp:BoundField DataField="TradeName" HeaderText="经营类别" />
                                    <asp:BoundField DataField="AreaName" HeaderText="经营区域" />
                                    <asp:BoundField DataField="AreaLevelName" HeaderText="租金水平" />
                                    <asp:BoundField DataField="FloorName" HeaderText="楼层" />
                                    <asp:BoundField DataField="LocationName" HeaderText="方位" />
                                    <asp:BoundField DataField="UnitCode" HeaderText="单元编码" />
                                    <asp:BoundField DataField="UnitDesc" HeaderText="单元描述" />
                                    <asp:BoundField DataField="FloorArea" HeaderText="建筑面积" />
                                    <asp:BoundField DataField="UseArea" HeaderText="使用面积" />
                                    <asp:BoundField DataField="RentArea" HeaderText="签约面积" />
                                    <asp:BoundField DataField="PlanUrl" HeaderText="平面图" />
                                    <asp:BoundField DataField="RentableStatusDesc" HeaderText="可否出租" />
                                    <asp:BoundField DataField="UnitStatusDesc" HeaderText="单元状态" />
                                    <asp:BoundField DataField="BlankOutStatusDesc" HeaderText="可否作废" />
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

