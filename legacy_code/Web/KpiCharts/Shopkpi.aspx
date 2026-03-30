<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Shopkpi.aspx.cs" Inherits="KpiCharts_Shopkpi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <script src="../DHtmlx/Layout/dhtmlxcommon.js" type="text/javascript"></script>
    <script src="../DHtmlx/Layout/dhtmlxlayout.js" type="text/javascript"></script>
    <script src="../DHtmlx/Layout/dhtmlxcontainer.js" type="text/javascript"></script>
    <link href="../DHtmlx/Layout/skins/dhtmlxlayout_dhx_blue.css" rel="stylesheet" type="text/css" />
    <link href="../DHtmlx/Layout/dhtmlxlayout.css" rel="stylesheet" type="text/css" />
</head>
<body style="margin-top:10px;" onload="doOnLoad();" >
    <form id="form1" runat="server">
        <div id="parentId" style="position: relative; top: 0px; left: 0px; width: 195px; height: 500px; " />
        <div id="contractinfo" style="display:none; " >
            <dl>
            <dt>
            <asp:Label ID="lblShopName" runat="server">商铺名称:</asp:Label>
            <asp:TextBox ID="shopname" runat="server"></asp:TextBox>
            </dt>
            <dt><asp:Label ID="lblBrandName" runat="server">主营品牌:</asp:Label>
            <asp:TextBox ID="brandname" runat="server"></asp:TextBox>
            </dt> 
            <dt><asp:Label ID="lblCustName" runat="server">商户名称:</asp:Label>
            <asp:TextBox ID="custname" runat="server"></asp:TextBox></dt>
            <dt>
            <asp:Label id="lblContractCode" runat="server">合同编码:</asp:Label>
            <asp:TextBox ID="contractcode" runat="server"></asp:TextBox></dt>
            <dt>
            <asp:Label ID="lblContractType" runat="server">合同类型:</asp:Label>
            <asp:TextBox ID="contrcttype" runat="server"></asp:TextBox>
            </dt>
            <dt>
            <asp:Label ID="lblContractDate" runat="server">合同日期:</asp:Label>
            <asp:TextBox ID="startdate" runat="server"></asp:TextBox>
            <asp:TextBox ID="enddate" runat="server"></asp:TextBox>
            </dt>
            </dl>
        </div>
        <div id="salesinof" style="display:none;">
            <asp:Label ID="lblShopName1" runat="server"></asp:Label>
            <dl>
            </dl>
        </div>
        <div id="invinfo" style="display:none;">
           <asp:Label ID="lblShopName2" runat="server"></asp:Label>
        </div> 
    </form>
<script type="text/javascript">
    var dhxLayout;
    function doOnLoad() {
        dhxLayout = new dhtmlXLayoutObject("parentId", "3E");
        dhxLayout.cells("a").setText("合同信息");    //标题
        dhxLayout.setCollapsedText("a","合同信息");   //隐藏后的文字
//        dhxLayout.cells("a").setHeight(200);          //设定高度
        dhxLayout.cells("a").attachObject("contractinfo"); //对象
        dhxLayout.cells("b").attachObject("salesinof");
        dhxLayout.cells("b").setText("销售信息");
        dhxLayout.setCollapsedText("b","销售信息");
    //    dhxLayout.cells("b").hideHeader(); //隐藏标题
    //    dhxLayout.cells("b").showHeader(); //显示标题
        dhxLayout.cells("b").collapse();   //隐藏
        //dhxLayout.cells("b").expand();     //展开
        dhxLayout.cells("c").setText("费用情况");
        dhxLayout.setCollapsedText("c","费用信息");
        dhxLayout.cells("c").attachObject("invinfo")
        dhxLayout.cells("c").collapse();
        dhxLayout.attachEvent("onExpand", doOnExpand);  //增加展开事件
        dhxLayout.attachEvent("onCollapse", doOnCollapse);
        dhxLayout.attachEvent("onDblClick", doOnDblClick);  //双击事件
    }
    function doOnExpand(itemId) {
        var i=0;
        var ind;
        for(i=0;i<=2;i++)
        {
            ind=dhxLayout.getIdByIndex(i);
            if(ind != itemId)
            {
                dhxLayout.cells(ind).collapse();
            }
        }
        
    }
//    function doOnCollapse(itemId) {
//        var i=0;
//        var ind;
//        for(i=0;i<=2;i++)
//        {
//            ind=dhxLayout.getIdByIndex(i);
//            if(itemId!=ind)
//            {
//                dhxLayout.cells(ind).expand();
//            }
//        }
//    }
    function doOnDblClick(itemId){
    }

</script>
</body>
</html>
