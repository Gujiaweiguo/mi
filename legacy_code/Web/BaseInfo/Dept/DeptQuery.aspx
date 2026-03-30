<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptQuery.aspx.cs" Inherits="BaseInfo_Dept_DeptQuery" MasterPageFile="~/BaseInfo/User/MasterPage.master"  %>


    <asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
        <div>
            <asp:Label ID="lblDeptCode" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblDeptCode %>' Width="51px" CssClass="label"></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server" Height="14px" CssClass="ipt50px"></asp:TextBox>
            <asp:Label ID="lblDeptName" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblDeptName %>' CssClass="label"></asp:Label>
            <asp:TextBox ID="TextBox2" runat="server" Height="14px" CssClass="ipt120px" MaxLength="32"></asp:TextBox>
            <asp:Label ID="lblDeptType" runat="server" Text='<%$ Resources:BaseInfo,Dept_lblDeptType %>' CssClass="label"></asp:Label>
            <asp:DropDownList ID="DropDownList1" runat="server" CssClass="cmb120px">
            </asp:DropDownList><br />
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4"
                ForeColor="#333333" GridLines="None" Height="133px" OnRowDataBound="GridView1_RowDataBound"
                OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="100%" PageSize="5" CssClass="gridview" AllowPaging="True">
                <Columns>
                    <asp:BoundField DataField="deptcode" HeaderText="部门编码" />
                    <asp:BoundField DataField="DeptName" HeaderText="部门名称" />
                    <asp:BoundField DataField="City" HeaderText="所在城市" />
                    <asp:BoundField DataField="OfficeAddr" HeaderText="办公地址" />
                    <asp:BoundField DataField="Tel" HeaderText="联系电话" />
                    <asp:BoundField DataField="deptid">
                        <HeaderStyle CssClass="hidden" />
                        <FooterStyle CssClass="hidden" />
                        <ItemStyle CssClass="hidden" />
                    </asp:BoundField>
                    <asp:CommandField SelectText="修改" ShowSelectButton="True" />
                </Columns>
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <RowStyle BackColor="#EFF3FB" />
                <EditRowStyle BackColor="#2461BF" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查  询" CssClass="btnOn50px" /></div>
    </asp:Content>

