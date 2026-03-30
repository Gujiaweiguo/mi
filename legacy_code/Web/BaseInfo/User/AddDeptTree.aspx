<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddDeptTree.aspx.cs" Inherits="BaseInfo_User_AddDeptTree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
<script language="javascript" type="text/javascript">
<!--

function Button1_onclick(str) {
    
 alert("my selection:"+str);
}

function test(){
alert("xxxxx");
window.opener.execScrip("Test(123)","JavaScrip");
}

// -->
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TreeView ID="TreeView1" runat="server" ExpandDepth="0" Height="426px" ImageSet="Faq"
                    OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" OnTreeNodeCheckChanged="TreeView1_TreeNodeCheckChanged"
                    Width="189px" ShowCheckBoxes="All">
                    <ParentNodeStyle Font-Bold="False" />
                    <HoverNodeStyle Font-Underline="True" ForeColor="Purple" />
                    <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" VerticalPadding="0px" />
                    <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="DarkBlue" HorizontalPadding="5px"
                        NodeSpacing="0px" VerticalPadding="0px" />
                </asp:TreeView>
        <br />
        <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click1" Text="确定" />
        <input id="Button1" type="button" value="button" language="javascript" onclick="return Button1_onclick()" /><br />
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
