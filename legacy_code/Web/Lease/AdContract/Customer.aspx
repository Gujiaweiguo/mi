<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Lease_AdContract_Customer" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <base target="_self"/>
    <link href="../../App_Themes/Tabbar/css/webtab.css" rel="stylesheet" type="text/css"/>
    <link href="../../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <link href="../../App_Themes/Tabbar/css/dhtmlXTabbar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
 
      function clickok()
        {
            window.document.all("Hide_return").value = document.all("CustID").value+","+document.all("CustCode").value+","+document.all("CustName").value+","+document.all("CustShortName").value;
	        returnall = document.getElementById("Hide_return").value;
	        window.returnValue=returnall;
	        window.close();		
	        return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="GVCust" runat="server" AutoGenerateColumns="False" OnRowCommand="GVCust_RowCommand" OnSelectedIndexChanged="GVCust_SelectedIndexChanged" BackColor="White" BorderColor="#E1E0B2">
            <Columns>
                <asp:BoundField DataField="CustID">
                    <ItemStyle CssClass="hidden" />
                    <HeaderStyle CssClass="hidden" />
                    <FooterStyle CssClass="hidden" />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:BaseInfo,LeaseAreaType_CustCode %>" DataField="CustCode" >
                    <ItemStyle BorderColor="#E1E0B2" />
                    <HeaderStyle BackColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustName %>" DataField="CustName" >
                    <ItemStyle BorderColor="#E1E0B2" />
                    <HeaderStyle BackColor="#E1E0B2" />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:BaseInfo,PotCustomer_lblCustShortName %>" DataField="CustShortName" >
                    <ItemStyle BorderColor="#E1E0B2" />
                    <HeaderStyle BackColor="#E1E0B2" />
                </asp:BoundField>
                <asp:CommandField ShowSelectButton="True" HeaderText="<%$ Resources:BaseInfo,PotShop_Selected %>" >
                    <HeaderStyle BackColor="#E1E0B2" />
                    <ItemStyle BorderColor="#E1E0B2" />
                </asp:CommandField>
            </Columns>
        </asp:GridView>
    </div>
        <input id="Hide_return" type="hidden" runat="server" />
        <input id="shortCustName" runat="server" type="hidden" />
        <asp:HiddenField ID="CustID" runat="server" />
        <asp:HiddenField ID="CustCode" runat="server" />
        <asp:HiddenField ID="CustName" runat="server" />
        <asp:HiddenField ID="CustShortName" runat="server" />
    </form>
</body>
</html>
