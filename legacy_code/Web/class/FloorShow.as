/**************************************************************************************/
/** 显示x,y坐标 **/
onEnterFrame = function ()
{
	this._parent._parent.xxx.text = _xmouse;
	this._parent._parent.yyy.text = _ymouse;
};
/**************************************************************************************/
/** XML处理 **/
var MenuXml:XML = new XML();
//     新建XML对象
MenuXml.ignoreWhite = true;
//     在分析过程中将放弃仅包含空白的文本节点
var urlStr;
//     链接地址变量
var fffNum;
//     存储XML对象数量的变量
var n = 0;
//     菜单初始值
var floorFlag;
//当前楼层
/** 输出XML对象 **/
MenuXml.onLoad = function()
{
	var n = MenuXml.firstChild.childNodes.length;
	for (var k = 0; k < n; k++)
	{
		var d = MenuXml.firstChild.childNodes[k].attributes.floorID;
		if (d == _global.floorID)
		{
			floorFlag = k;
		}
		else
		{
			next;
		}
	}
	var j = MenuXml.firstChild.childNodes[floorFlag].childNodes.length;
	// XML文件中指定楼层的节点长度（店铺个数）
	fffNum = j;
	_global.localNum = j;
	for (var i = 0; i < j; i++)
	{
		var ObjF = MenuXml.firstChild.childNodes[i];
		if ((ObjF.attributes.floorID == _global.floorID) && (ObjF.attributes.buildID == _global.buildID))
		{
			var loadN = MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[13].firstChild;
			var tt = MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[7].firstChild;
			var xNum = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[15].firstChild));
			var yNum = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[16].firstChild));
			var shopNo = String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[6].firstChild);
			var DepthNum = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[17].firstChild));
			var rbN = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[18].firstChild));
			var gbN = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[19].firstChild));
			var bbN = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[20].firstChild));
			//商铺内文字坐标
			var NoX = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[21].firstChild));
			var NoY = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[22].firstChild));
			var NameX = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[23].firstChild));
			var NameY = int(String(MenuXml.firstChild.childNodes[floorFlag].childNodes[i].childNodes[24].firstChild));
			//数据读取完毕，向场景中添加商铺：
		_root.MainMovie.loadFM.loadShops.attachMovie("loadIcon", ("Icon" + i), (_root.MainMovie.loadFM.loadShops.shopSwap.getDepth() + (100 + DepthNum)), {_x:xNum, _y:yNum});
		_root.MainMovie.loadFM.loadShops["Icon" + i].swapDepths(_root.MainMovie.loadFM.loadShops.shopSwap);
		_root.MainMovie.loadFM.loadShops["Icon" + i].ShopNameT.text = tt;
		_root.MainMovie.loadFM.loadShops["Icon" + i].ShopNoT.text = shopNo;
		_root.MainMovie.loadFM.loadShops["Icon" + i].ShopNameT.selectable = false;
		_root.MainMovie.loadFM.loadShops["Icon" + i].ShopNoT.selectable = false;
		//着色
		var my_color:Color = new Color(_root.MainMovie.loadFM.loadShops["Icon" + i]);
		var myTransform:Object = my_color.getTransform();
		myTransform = {ra:100, rb:rbN, ga:100, gb:gbN, ba:100, bb:bbN};
		my_color.setTransform(myTransform);
		//着色完毕
		_root.MainMovie.loadFM.loadShops["Icon" + i].ShopNameT._x = NameX;
		_root.MainMovie.loadFM.loadShops["Icon" + i].ShopNameT._y = NameY;
		_root.MainMovie.loadFM.loadShops["Icon" + i].ShopNoT._x = NoX;
		_root.MainMovie.loadFM.loadShops["Icon" + i].ShopNoT._y = NoY;
		_root.MainMovie.loadFM.loadShops["Icon" + i].loadIconM.loadMovie(_global.globalPath + loadN);
		_root.MainMovie.loadFM.loadShops["Icon" + i].loadIconM.opaqueBackground = shopColor;
		_root.MainMovie.loadFM.loadShops["Icon" + i].onRollOver = function()
		{
			_global.ThisLevel = "ShopID";
			_global.loadXmlMallMenu = "ShopMenu.xml";
			_global.setMenuMovie = "";
			var l = length(this._name);
			var n = substring(this._name, 5, (l - 4));
			_global.ShopFlog = String(MenuXml.firstChild.childNodes[floorFlag].childNodes[n].childNodes[6].firstChild);
			_global.LevelID = _global.ShopFlog;
			_global.ShowFloorMenu = false;
			_global.ShowShopMenu = true;
			getMenu();
		};
		/*
		_root.MainMovie.loadFM.loadShops["Icon" + i].onRollOut = function()
		{
			_global.ThisLevel = "FloorID";
			_global.loadXmlMallMenu = "FloorMenu.xml";
			_global.setMenuMovie = "";
			_global.LevelID = _global.FloorNum;
			_global.ShowFloorMenu = true;
			_global.ShowShopMenu = false;
			getMenu();
		};
		*/
		}
	}
};
/**************************************************************************************/
/*
*　设置商铺右键点击事件
*/
/**************************************************************************************/
//Global Function - 公共函数
/** ShopMenu.xml **/
getMenu = function (SorF)
{
	var smenuValue:Array = new Array();
	var smenuUrl:Array = new Array();
	var Shop_xml:XML = new XML();
	Shop_xml.ignoreWhite = true;
	Shop_xml.onLoad = function()
	{
		var n = Shop_xml.firstChild.childNodes.length;
		// XML文件长度（对象个数）
		_global.sF_Number = n;
		for (var i = 0; i < n; i++)
		{
			smenuValue[i] = Shop_xml.firstChild.childNodes[i].childNodes[0].firstChild;
			smenuUrl[i] = Shop_xml.firstChild.childNodes[i].childNodes[1].firstChild;
		}
		//Shop
		var sMenu_array:Array = new Array();
		var smy_cma:ContextMenu = new ContextMenu();
		smy_cma.hideBuiltInItems();
		//smy_cma.customItems.push( new ContextMenuItem( "返回主菜单", geturlHandlera,true,true,true ) );
		for (var u = 0; u < _global.sF_Number; u++)
		{
			if (u == 0)
			{
				smy_cma.customItems.push(new ContextMenuItem(String(smenuValue[u]), eval("sgeturlHandler_item" + u), true, true, true));
			}
			else
			{
				smy_cma.customItems.push(new ContextMenuItem(String(smenuValue[u]), eval("sgeturlHandler_item" + u), false, true, true));
			}
		}
		/**
		Shop_menuItem自定义函数
		*/
		/*
		function geturlHandlera()
		{
		_global.ShowFloorMenu = false;
		_global.ShowShopMenu = false;
		_root.MainMovie.loadFM.loadShops.menu = SeleMenu;
		}
		*/
		function sgeturlHandler_item0()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[0] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[0] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[0] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[0] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item1()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[1] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[1] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[1] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[1] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item2()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[2] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[2] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[2] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[2] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item3()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[3] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[3] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[3] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[3] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item4()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[4] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[4] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[4] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[4] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item5()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[5] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[5] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[5] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[5] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item6()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[6] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[6] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[6] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[6] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item7()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[7] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[7] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[7] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[7] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item8()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[8] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[8] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[8] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[8] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item9()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[9] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[9] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[9] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[9] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item10()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[10] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[10] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[10] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[10] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item11()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[11] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[11] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[11] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[11] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item12()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[12] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[12] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[12] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[12] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item13()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[13] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[13] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[13] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[13] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item14()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[14] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[14] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[14] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[14] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item15()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[15] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[15] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[15] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[15] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item16()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[16] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[16] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[16] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[16] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item17()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[17] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[17] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[17] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[17] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item18()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[18] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[18] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[18] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[18] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item19()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[19] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[19] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[19] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[19] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item20()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[20] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[20] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[20] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[20] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item21()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[21] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[21] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[21] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[21] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item22()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[22] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[22] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[22] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[22] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item23()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[23] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[23] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[23] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[23] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item24()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[24] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[24] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[24] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[24] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item25()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[25] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[25] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[25] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[25] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item26()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[26] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[26] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[26] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[26] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item27()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[27] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[27] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[27] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[27] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item28()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[28] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[28] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[28] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[28] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item29()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[29] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[29] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[29] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[29] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item30()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[30] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[30] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[30] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[30] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item31()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[31] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[31] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[31] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[31] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item32()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[32] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[32] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[32] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[32] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item33()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[33] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[33] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[33] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[33] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item34()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[34] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[34] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[34] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[34] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		function sgeturlHandler_item35()
		{
			if (_global.ThisLevel != "FloorID")
			{
				trace(smenuUrl[35] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[35] + "&" + _global.ThisLevel + "=" + _global.LevelID + "&FloorID=" + _global.floorID, "_self");
			}
			else
			{
				trace(smenuUrl[35] + "&FloorID=" + _global.floorID);
				getURL(smenuUrl[35] + "&FloorID=" + _global.floorID, "_self");
			}
		}
		_root.MainMovie.loadFM.loadShops.menu = smy_cma;
	};
	if (_global.ShowFloorMenu == true)
	{
		Shop_xml.load(_global.loadXmlFloorMenu);
		//+ "?" + random(99999999));
	}
	else if (_global.ShowShopMenu == true)
	{
		Shop_xml.load(_global.loadXmlShopMenu);
		//+ "?" + random(99999999));
	}
};
_global.ShowShopMenu = false;
_global.ShowFloorMenu = true;
getMenu();
/***************************************************************************/
trace( _global.loadXmlFileName );
var sss = _global.loadXmlFileName;
// + "?" + random(99999999);
MenuXml.load(sss);