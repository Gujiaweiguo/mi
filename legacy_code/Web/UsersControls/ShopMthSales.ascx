<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShopMthSales.ascx.cs" Inherits="UsersControls_ShopMthSales" %>

<table width="405" align="center" cellpadding="0" cellspacing="0">
<tr>
	<td>
		<div id="div1" align="center">Charts. </div>
		<script type="text/javascript">
		    var chart5 = new FusionCharts("../FusionCharts/SWF/Pie3D.swf", "ChartId", "100%", "100%", "0", "0");
		    chart5.setDataURL(parent.document.getElementById("shopsale").innerText);
		    chart5.setTransparent(true);
		    chart5.render("chartdiv5");
		</script>			
	</td>
</tr>
<tr>
	<td background="../../Images/orangeTab.gif" colspan="3" align="left" height="26" width="405">
		<span class="textBoldDark">&nbsp;&nbsp;更多统计 </span>
	</td>
	<td>
	</td>
</tr>
<tr height="27">
	<td width="127" align="center" valign="bottom" height="27">
		<a href="#start"><img src="../../Images/btnTopIndicators.jpg" border="0"  alt="My Top Selected Indicators in Charts" WIDTH="126" HEIGHT="27"></a>
	</td>
	<td width="131" align="center" valign="bottom" height="27">
		<a href="#start"><img src="../../Images/btnEmployee.jpg" border="0" alt="Sales By Employee" WIDTH="126" HEIGHT="27"></a>
	</td>
	<td align="left" valign="bottom" height="27">
		<a href="#start"><img src="../../Images/btnInventory.jpg" border="0" alt="Inventory By Categories" WIDTH="126" HEIGHT="27"></a>
	</td>
	<td>
	</td>
</tr>
<tr height="27">
	<td width="127" align="center" valign="top" height="27">
		<a href="#start"><img src="../../Images/btnSalesByCountry.jpg" border="0" alt="Sales By Country" height="27"></a>
	</td>
	<td width="131" align="center" valign="top" height="27">
		<a href="#start"><img src="../../Images/btnSalesByCat.jpg" border="0" alt="Sales By Categories (Cumulative)" height="27"></a>
	</td>
	<td align="left" valign="top" height="27">
		<a href="#start"><img src="../../Images/btnShipping.jpg" border="0" alt="Average delay in Shipping" height="27"></a>
	</td>
	<td>
	</td>
</tr>
</table>
