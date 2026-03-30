<%@ Page Language="C#" MasterPageFile="~/MasterPage/mpMasterData.master" AutoEventWireup="true" CodeFile="Default12.aspx.cs" Inherits="Default12" Title="无标题页" %>
<%@ MasterType VirtualPath="~/MasterPage/mpMasterData.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <script src="DHtmlx/Layout/dhtmlxcommon.js" type="text/javascript"></script>
    <script src="DHtmlx/Layout/dhtmlxlayout.js" type="text/javascript"></script>
    <script src="DHtmlx/Layout/dhtmlxcontainer.js" type="text/javascript"></script>
    <link href="DHtmlx/Layout/skins/dhtmlxlayout_dhx_blue.css" rel="stylesheet" type="text/css" />
    <link href="DHtmlx/Layout/dhtmlxlayout.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="grid/gt_grid.css" />
    <link rel="stylesheet" type="text/css" href="grid/skin/vista/skinstyle.css" />
    <link rel="stylesheet" type="text/css" href="grid/skin/china/skinstyle.css" />
    <link rel="stylesheet" type="text/css" href="grid/skin/mac/skinstyle.css" />
    <link href="grid/highlight/style.css" rel="stylesheet" type="text/css" />
    <script src="grid/highlight/jssc3.js" type="text/javascript"></script>
    <script type="text/javascript" src="grid/gt_grid_all.js"></script>
    <script type="text/javascript" src="grid/gt_msg_en.js"></script>
    <script type="text/javascript" src="grid/gridFunc.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="conFuncNode" Runat="Server">
<asp:Label ID="txtDesc" runat="server"></asp:Label>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="conFuncForm" Runat="Server">
<asp:TextBox ID="txtFuncID" runat="server"></asp:TextBox>
<div id="datagrid" />
</asp:Content>

