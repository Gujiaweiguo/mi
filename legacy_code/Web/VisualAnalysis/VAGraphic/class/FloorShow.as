/**************************************************************************************/
/** 显示x,y坐标 **/
onEnterFrame = function()
{
	xxx.text = _xmouse;
	yyy.text = _ymouse;
}
/** XML处理 **/
var MenuXml:XML = new XML();                          //     新建XML对象
MenuXml.ignoreWhite = true;                           //     在分析过程中将放弃仅包含空白的文本节点
var urlStr;                                           //     链接地址变量
var fffNum;                                           //     存储XML对象数量的变量
var n = 0;                                            //     菜单初始值
/** 输出XML对象 **/
MenuXml.onLoad = function() 
{
	var j = MenuXml.childNodes[ _global.FloorNum-1 ].childNodes.length;     // XML文件中指定楼层的节点长度（店铺个数）
	fffNum = j;
	_global.localNum = j;
	for( var i = 0; i < j; i++ )
	{																   
		var loadN = MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 13 ].firstChild;
		var tt = MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 7 ].firstChild;
		var xNum = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 15 ].firstChild ) );
		var yNum = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 16 ].firstChild ) );
		var shopNo = String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 6 ].firstChild );
		var DepthNum = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 17 ].firstChild ) );
		var rbN = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 18 ].firstChild ) );
		var gbN = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 19 ].firstChild ) );
		var bbN = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 20 ].firstChild ) );
		//商铺内文字坐标
		var NoX = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 21 ].firstChild ) );
		var NoY = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 22 ].firstChild ) );
		var NameX = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 23 ].firstChild ) );
		var NameY = int( String( MenuXml.childNodes[ _global.FloorNum-1 ].childNodes[ i ].childNodes[ 24 ].firstChild ) );
		
		_root.LoadParents.LoadFloors.attachMovie( "loadIcon", ( "Icon" + i ) , ( _root.LoadParents.LoadFloors.LoadFloorMovie.getDepth() + (100 + DepthNum) ) , { _x:xNum , _y:yNum  } );
		_root.LoadParents.LoadFloors[ "Icon"+ i ].swapDepths( _root.LoadParents.LoadFloors.LoadFloorMovie );
		_root.LoadParents.LoadFloors[ "Icon"+ i ].ShopNameT.text = tt;
		_root.LoadParents.LoadFloors[ "Icon"+ i ].ShopNoT.text = shopNo;
		_root.LoadParents.LoadFloors[ "Icon"+ i ].ShopNameT.selectable = false;
		_root.LoadParents.LoadFloors[ "Icon"+ i ].ShopNoT.selectable = false;
		
		var my_color:Color = new Color( _root.LoadParents.LoadFloors["Icon"+ i] );
		var myTransform:Object = my_color.getTransform();
		myTransform = { ra: 100, rb: rbN, ga: 100, gb: gbN, ba: 100, bb: bbN };
		my_color.setTransform(myTransform);

		
		_root.LoadParents.LoadFloors[ "Icon"+ i ].ShopNameT._x = NameX;
		_root.LoadParents.LoadFloors[ "Icon"+ i ].ShopNameT._y = NameY;
		_root.LoadParents.LoadFloors[ "Icon"+ i ].ShopNoT._x = NoX;
		_root.LoadParents.LoadFloors[ "Icon"+ i ].ShopNoT._y = NoY;
		
		_root.LoadParents.LoadFloors[ "Icon" + i ].loadIconM.loadMovie( _global.globalPath + loadN );
		_root.LoadParents.LoadFloors[ "Icon" + i ].loadIconM.opaqueBackground = shopColor;
	}
};
/** 加载XML文件 **/
MenuXml.load( _global.loadXmlFileName );
/**************************************************************************************/
/** FloorMenu.xml **/
var menuValue:Array = new Array();
var menuUrl:Array = new Array();
var Floor_xml:XML = new XML();
Floor_xml.ignoreWhite = true;
Floor_xml.onLoad = function()
{
	var n = Floor_xml.firstChild.childNodes.length;     // XML文件长度（对象个数）
	_global.F_Number = n;
	for( var i = 0; i < n; i++ )
	{
		menuValue[ i ] = Floor_xml.firstChild.childNodes[ i ].childNodes[ 0 ].firstChild;
		menuUrl[ i ] = Floor_xml.firstChild.childNodes[ i ].childNodes[ 1 ].firstChild;
	}


/*******************/
var Menu_array:Array = new Array();
var my_cma:ContextMenu = new ContextMenu();
my_cma.hideBuiltInItems();
//my_cma.customItems.push( new ContextMenuItem( "返回主页", geturlHandlera,true,true,true ) );
	
for( var u = 0; u < _global.F_Number; u++ )
{
	if( u == 0 )
	{
		my_cma.customItems.push( new ContextMenuItem( String(menuValue[u]), eval("geturlHandler_item"+u ),true,true,true ) );
	}else
	{
		my_cma.customItems.push( new ContextMenuItem( String(menuValue[u]), eval("geturlHandler_item"+u),false,true,true ) );
	}
}
/**************************************************************************************/
/**
	menuItem自定义函数
*/
function geturlHandlera() 
{
	this.LoadFloorMovie.removeMovieClip( ( "Floor_" + _global.FloorNum ) );
	_root.gotoAndStop( 1 );
}
/**************************************************************************************
var kkkk = "0";
var menuF_name = ( "geturlHandler_item" + kkkk );
trace( menuF_name );
menuF_name = Function(menuF_name);
_root.LoadParents.LoadFloors[menuF_name] = function()
{
	trace("aaaassssssssssss");
};
eval( _root.LoadParents.LoadFloors[menuF_name] );
/**************************************************************************************
function geturlHandler_item(obj, menuItem)
{
	var Url_str = String(ExternalInterface.call("sayhello", menuItem.caption ));
	//var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 0 ] ));
}
/**************************************************************************************/
function geturlHandler_item0()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 0 ] ));
}
function geturlHandler_item1()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 1 ] ));
}
function geturlHandler_item2()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 2 ] ));
}
function geturlHandler_item3()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 3 ] ));
}
function geturlHandler_item4()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 4 ] ));
}
function geturlHandler_item5()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 5 ] ));
}
function geturlHandler_item6()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 6 ] ));
}
function geturlHandler_item7()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 7 ] ));
}
function geturlHandler_item8()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 8 ] ));
}
function geturlHandler_item9()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 9 ] ));
}
function geturlHandler_item10()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 10 ] ));
}
function geturlHandler_item11()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 11 ] ));
}
function geturlHandler_item12()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 12 ] ));
}
function geturlHandler_item13()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 13 ] ));
}
function geturlHandler_item14()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 14 ] ));
}
function geturlHandler_item15()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 15 ] ));
}
function geturlHandler_item16()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 16 ] ));
}
function geturlHandler_item17()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 17 ] ));
}
function geturlHandler_item18()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 18 ] ));
}
function geturlHandler_item19()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 19 ] ));
}
function geturlHandler_item20()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 20 ] ));
}
function geturlHandler_item21()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 21 ] ));
}
function geturlHandler_item22()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 22 ] ));
}
function geturlHandler_item23()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 23 ] ));
}
function geturlHandler_item24()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 24 ] ));
}
function geturlHandler_item25()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 25 ] ));
}
function geturlHandler_item26()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 26 ] ));
}
function geturlHandler_item27()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 27 ] ));
}
function geturlHandler_item28()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 28 ] ));
}
function geturlHandler_item29()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 29 ] ));
}
function geturlHandler_item30()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 30 ] ));
}
function geturlHandler_item31()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 31 ] ));
}
function geturlHandler_item32()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 32 ] ));
}
function geturlHandler_item33()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 33 ] ));
}
function geturlHandler_item34()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 34 ] ));
}function geturlHandler_item35()
{
	var Url_str = String(ExternalInterface.call("sayhello", menuUrl[ 35 ] ));
}
/**************************************************************************************/
_root.LoadParents.LoadFloors.menu = my_cma;
/**************************************************************************************/
};
Floor_xml.load( _global.loadXmlFloorMenu );