//Global Function - 公共函数
/** MIGroupMenu.xml **/
function nameSubNum(str:String,n1:Number,n2:Number)
{
	var ta = int( substring( str,n1,n2 ));
	return(ta);
}
function nameSubStr(str:String,n1:Number,n2:Number)
{
	var tb = substring( str,n1,n2 );
	return(tb);
}
clearMenu = function()
{
	var smy_cmb:ContextMenu = new ContextMenu();
	smy_cmb.hideBuiltInItems();
	_root.menu = smy_cmb;
}
getMenu = function( SorF )
{
var smenuValue:Array = new Array();
var smenuUrl:Array = new Array();
var Shop_xml:XML = new XML();
Shop_xml.ignoreWhite = true;
Shop_xml.onLoad = function()
{
	var n = Shop_xml.firstChild.childNodes.length;     // XML文件长度（对象个数）
	_global.sF_Number = n;
	//trace( _global.sF_Number );
	for( var i = 0; i < n; i++ )
	{
		smenuValue[ i ] = Shop_xml.firstChild.childNodes[ i ].childNodes[ 0 ].firstChild;
		smenuUrl[ i ] = Shop_xml.firstChild.childNodes[ i ].childNodes[ 1 ].firstChild;
	}
//Shop
var sMenu_array:Array = new Array();
var smy_cma:ContextMenu = new ContextMenu();
smy_cma.hideBuiltInItems();
	//smy_cma.customItems.push( new ContextMenuItem( "返回主菜单", geturlHandlera,true,true,true ) );
	
for( var u = 0; u < _global.sF_Number; u++ )
{
	if( u == 0 )
	{
		smy_cma.customItems.push( new ContextMenuItem( String(smenuValue[u]), eval("sgeturlHandler_item"+u ),true,true,true ) );
	}else
	{
		smy_cma.customItems.push( new ContextMenuItem( String(smenuValue[u]), eval("sgeturlHandler_item"+u),false,true,true ) );
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
		_root.LoadParents.LoadFloors.menu = SeleMenu;
	}
*/
function sgeturlHandler_item0()
{
	trace( smenuUrl[ 0 ] + "&" + _global.ThisLevel + "=" + _global.LevelID );
	_root.BUrl.text = smenuUrl[ 0 ] + "&" + _global.ThisLevel + "=" + _global.LevelID;
	getURL( smenuUrl[ 0 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item1()
{
	trace( smenuUrl[ 1 ] + "&" + _global.ThisLevel + "=" + _global.LevelID );
	_root.BUrl.text = smenuUrl[ 1 ] + "&" + _global.ThisLevel + "=" + _global.LevelID;
	getURL( smenuUrl[ 1 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item2()
{
	trace( smenuUrl[ 2 ] + "&" + _global.ThisLevel + "=" + _global.LevelID );
	_root.BUrl.text = smenuUrl[ 2 ] + "&" + _global.ThisLevel + "=" + _global.LevelID;
	getURL( smenuUrl[ 2 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item3()
{
	getURL( smenuUrl[ 3 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item4()
{
	getURL( smenuUrl[ 4 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item5()
{
	getURL( smenuUrl[ 5 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item6()
{
	getURL( smenuUrl[ 6 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item7()
{
	getURL( smenuUrl[ 7 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item8()
{
	getURL( smenuUrl[ 8 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item9()
{
	getURL( smenuUrl[ 9 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item10()
{
	getURL( smenuUrl[ 10 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item11()
{
	getURL( smenuUrl[ 11 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item12()
{
	getURL( smenuUrl[ 12 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item13()
{
	getURL( smenuUrl[ 13 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item14()
{
	getURL( smenuUrl[ 14 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item15()
{
	getURL( smenuUrl[ 15 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item16()
{
	getURL( smenuUrl[ 16 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item17()
{
	getURL( smenuUrl[ 17 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item18()
{
	getURL( smenuUrl[ 18 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item19()
{
	getURL( smenuUrl[ 19 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item20()
{
	getURL( smenuUrl[ 20 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );

}
function sgeturlHandler_item21()
{
	getURL( smenuUrl[ 21 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item22()
{
	getURL( smenuUrl[ 22 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item23()
{
	getURL( smenuUrl[ 23 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item24()
{
	getURL( smenuUrl[ 24 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item25()
{
	getURL( smenuUrl[ 25 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item26()
{
	getURL( smenuUrl[ 26 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item27()
{
	getURL( smenuUrl[ 27 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item28()
{
	getURL( smenuUrl[ 28 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item29()
{
	getURL( smenuUrl[ 29 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item30()
{
	getURL( smenuUrl[ 30 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item31()
{
	getURL( smenuUrl[ 31 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item32()
{
	getURL( smenuUrl[ 32 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item33()
{
	getURL( smenuUrl[ 33 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
function sgeturlHandler_item34()
{
	getURL( smenuUrl[ 34 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}function sgeturlHandler_item35()
{
	getURL( smenuUrl[ 35 ] + "&" + _global.ThisLevel + "=" + _global.LevelID, "_self" );
}
//
if( _global.setMenuMovie != "" )
{
	if( _global.ThisLevel == "FloorID" )
	{

	}else if( _global.ThisLevel != "FloorID" )
	{
		_root[_global.setMenuMovie].menu = smy_cma;
	}
}else if( _global.setMenuMovie == "" )
{
	_root.menu = smy_cma;
}
}
/******************************************/
//Floor_xml.load( _global.loadXmlMallMenu );
//Shop_xml.load( _global.loadXmlShopMenu );
//trace( "_global.globalXMLPath : " + _global.globalXMLPath );
//trace( "加载的菜单 : " + _global.globalXMLPath + _global.loadXmlMallMenu );
if( _global.loadXmlMallMenu != "" )
{
	var sss = _global.globalXMLPath + _global.loadXmlMallMenu;// + "?" + random(99999999);
	Shop_xml.load( sss );
}
};
_global.ShowShopMenu = false;
_global.ShowFloorMenu = true;
//getMenu();
/**************************************************************************************/