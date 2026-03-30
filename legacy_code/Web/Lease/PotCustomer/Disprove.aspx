<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Disprove.aspx.cs" Inherits="Test_Default3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>单据驳回节点选择</title>
    <link href="../../App_Themes/CSS/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
    function showline()
    {
		GridView1.borderColor="#e1e0b2";
        GridView1.borderWidth="1px";
    }
    </script>
</head>
<body onload='showline();'>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Height="19px" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" Width="661px" OnRowDataBound="GridView1_RowDataBound" BackColor="White">
            <Columns>
                <asp:BoundField DataField="WrkFlwID">
                    <ItemStyle CssClass="hidden" />
                    <HeaderStyle CssClass="hidden" />
                    <FooterStyle CssClass="hidden" />
                </asp:BoundField>
                <asp:BoundField DataField="NodeID">
                    <ItemStyle CssClass="hidden" />
                    <HeaderStyle CssClass="hidden" />
                    <FooterStyle CssClass="hidden" />
                </asp:BoundField>
                <asp:BoundField DataField="WrkFlwName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_WorkFlowID %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField DataField="NodeName" HeaderText="<%$ Resources:BaseInfo,WrkFlw_NodeID %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField DataField="StartTime" HeaderText="<%$ Resources:BaseInfo,WrkFlw_StartTime %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField DataField="CompletedTime" HeaderText="<%$ Resources:BaseInfo,WrkFlw_EndTime %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField DataField="VoucherID" HeaderText="<%$ Resources:BaseInfo,WrkFlw_VoucherID %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField DataField="VoucherHints" HeaderText="<%$ Resources:BaseInfo,WrkFlw_VoucherHints %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField DataField="VoucherMemo" HeaderText="<%$ Resources:BaseInfo,WrkFlw_VoucherMemo %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField DataField="Sequence" HeaderText="<%$ Resources:BaseInfo,WrkFlw_Sequence %>">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:BoundField>
                <asp:CommandField SelectText="<%$ Resources:BaseInfo,CustPalaver_butOverrule %>" ShowSelectButton="True">
                        <HeaderStyle CssClass="gridviewtitle" BackColor="#E1E0B2" BorderColor="#E1E0B2" />
                        <ItemStyle BorderColor="#E1E0B2" />
                </asp:CommandField>
            </Columns>
                <FooterStyle BackColor="Red" ForeColor="#000066"/>
                <RowStyle ForeColor="Black" Height="10px" Font-Overline="False" Font-Size="10pt" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="False" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066"   HorizontalAlign="Left" />
                <HeaderStyle BackColor="#E1E0B2" Font-Bold="False" Font-Italic="False"  />
        </asp:GridView>
        <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
    </div>
    </form>
</body>
</html>
