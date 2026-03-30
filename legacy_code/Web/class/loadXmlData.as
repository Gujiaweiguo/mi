//区域信息
function LocaXML(SubPath, array_Str)
{
	var _loc6 = new XML();
	_loc6.load(_global.globalXMLPath + SubPath);
	_loc6._parent = this;
	_loc6.ignoreWhite = true;
	_loc6.getBytesLoaded();
	_loc6.onLoad = function(e)
	{
		if (e)
		{
			var _loc5 = new XMLNode();
			_loc5 = this.childNodes;
			_root[array_Str + "_array"] = new Array();
			for (var _loc3 = 0; _loc3 < _loc5[0].childNodes.length; ++_loc3)
			{
				var _loc4 = _loc5[0].childNodes[_loc3].attributes.id * 1;
				_root[array_Str + "_array"][_loc4] = new Object();
				_root[array_Str + "_array"][_loc4].id = _loc5[0].childNodes[_loc3].attributes.id;
				_root[array_Str + "_array"][_loc4].nm = _loc5[0].childNodes[_loc3].attributes.nm;
				_root[array_Str + "_array"][_loc4].infor = _loc5[0].childNodes[_loc3].attributes.infor;
			}
			// end of for
			this._parent.LoadOK = true;
		}
		else
		{
			//_root.createTextField("t_txt", _root.getNextHighestDepth(), 0, 0, 100, 50);
			//_root.t_txt.text = "Load Error";
		}
		// end else if
	};
}
// End of the function
//城市信息
function CityXML(SubPath, array_Str)
{
	var _loc6 = new XML();
	_loc6.load(_global.globalXMLPath + SubPath);
	_loc6._parent = this;
	_loc6.ignoreWhite = true;
	_loc6.getBytesLoaded();
	_loc6.onLoad = function(e)
	{
		if (e)
		{
			var _loc5 = new XMLNode();
			_loc5 = this.childNodes;
			_root[array_Str + "_array"] = new Array();
			for (var _loc3 = 0; _loc3 < _loc5[0].childNodes.length; ++_loc3)
			{
				if (_loc5[0].childNodes[_loc3].attributes.locaId == _global.locaID)
				{
					for (var _loc6 = 0; _loc6 < _loc5[0].childNodes[_loc3].childNodes.length; ++_loc6)
					{
						var _loc4 = _loc5[0].childNodes[_loc3].childNodes[_loc6].attributes.id * 1;
						/*
						_root[array_Str + "_array"][_loc4] = new Object();
						_root[array_Str + "_array"][_loc4].id = _loc5[0].childNodes[_loc3].childNodes[_loc6].attributes.id;
						_root[array_Str + "_array"][_loc4].nm = _loc5[0].childNodes[_loc3].childNodes[_loc6].attributes.nm;
						_root[array_Str + "_array"][_loc4].infor = _loc5[0].childNodes[_loc3].childNodes[_loc6].attributes.infor;
						*/
						_root["proj" + _loc4].id = _loc5[0].childNodes[_loc3].childNodes[_loc6].attributes.id;
						_root["proj" + _loc4].nm = _loc5[0].childNodes[_loc3].childNodes[_loc6].attributes.nm;
						_root["proj" + _loc4].infor = _loc5[0].childNodes[_loc3].childNodes[_loc6].attributes.infor;
					}
					break;
				}
				else
				{
				}
			}
			// end of for
			this._parent.LoadOK = true;
		}
		else
		{
			//_root.createTextField("t_txt", _root.getNextHighestDepth(), 0, 0, 100, 50);
			//_root.t_txt.text = "Load Error";
		}
		// end else if
	};
}
// End of the function
//Mall信息
function MallXML(SubPath, array_Str)
{
	_global.mallN = 0;
	var _loc6 = new XML();
	_loc6.load(_global.globalXMLPath + SubPath);
	_loc6._parent = this;
	_loc6.ignoreWhite = true;
	_loc6.getBytesLoaded();
	_loc6.onLoad = function(e)
	{
		if (e)
		{
			var _loc5 = new XMLNode();
			_loc5 = this.childNodes;
			_root[array_Str + "_array"] = new Array();
			for (var _loc3 = 0; _loc3 < _loc5[0].childNodes.length; ++_loc3)
			{
				_root[array_Str + "_array"][_loc3] = new Object();
				_root[array_Str + "_array"][_loc3].locaID = _loc5[0].childNodes[_loc3].attributes.locaID;
				_root[array_Str + "_array"][_loc3].cityID = _loc5[0].childNodes[_loc3].attributes.cityID;
				_root[array_Str + "_array"][_loc3].mallID = _loc5[0].childNodes[_loc3].attributes.mallID;
				_root[array_Str + "_array"][_loc3].desc = _loc5[0].childNodes[_loc3].attributes.desc;
				_root[array_Str + "_array"][_loc3].adress = _loc5[0].childNodes[_loc3].attributes.adress;
				_root[array_Str + "_array"][_loc3].Img = _loc5[0].childNodes[_loc3].attributes.Img;
				_root[array_Str + "_array"][_loc3].remark = _loc5[0].childNodes[_loc3].attributes.remark;
			}
			// end of for
			//加载Mall图标
			var lenN = _root[array_Str + "_array"].length;
			var dep = 0;
			for (var i = 0; i < lenN; i++)
			{
				var Obj = _root[array_Str + "_array"][i];
				//if ((Obj.cityID == _global.cityID) && (Obj.locaID == _global.locaID))
				//{
					_global.mallN++;
					_root.attachMovie("MallShow", "M" + _global.mallN, dep);
					var m = _root["M" + _global.mallN];
					m.loadImgM.loadMovie(_global.globalXMLPath + String(Obj.Img));
					m.idTxt = Obj.mallID;
					m.nmTxt.text = "[ " + Obj.desc + " ]";
					setProperty(m, _x, m._width * (int((_global.mallN - 1) % 3)) + 10 * ((int((_global.mallN - 1) % 3)) - 1) + 20);
					setProperty(m, _y, m._height * int((_global.mallN - 1) / 3) + 10 * (int((_global.mallN - 1) / 3) - 1) + 60);
					m.onPress = function()
					{
						_global.returnFrame = _currentframe;
						nextFrame();
						var s = nameSubStr(this._name, 2, 1);
						_global.mallID = this.idTxt;
						_global.Mall = String(ExternalInterface.call("sendAreaID", String("Mall^" + _global.mallID)));
					};
					m.onRollOver = function()
					{
						//设置右键菜单
						_global.ThisLevel = "MallID";
						_global.LevelID = this.idTxt;
						_global.loadXmlMallMenu = "MallMenu.xml";
						_global.setMenuMovie = this;
						getMenu();
					};
					m.onRollOut = function()
					{
						//设置右键菜单
						//_global.ThisLevel = "CityID";
						//_global.LevelID = _global.cityID;
						//_global.loadXmlMallMenu = "CityMenu.xml";
						//_global.setMenuMovie = this;
						//getMenu();
					};
				//}
				dep++;
			}
			this._parent.shopInforOK = true;
		}
		else
		{
			//_root.createTextField("t_txt", _root.getNextHighestDepth(), 0, 0, 100, 50);
			//_root.t_txt.text = "Load Error";
		}
		// end else if
	};
}
// End of the function
//Building信息
function BuildingXML(SubPath, array_Str)
{
	_global.buildN = 0;
	var _loc6 = new XML();
	_loc6.load(_global.globalXMLPath + SubPath);
	_loc6._parent = this;
	_loc6.ignoreWhite = true;
	_loc6.getBytesLoaded();
	_loc6.onLoad = function(e)
	{
		if (e)
		{
			var _loc5 = new XMLNode();
			_loc5 = this.childNodes;
			_root[array_Str + "_array"] = new Array();
			for (var _loc3 = 0; _loc3 < _loc5[0].childNodes.length; ++_loc3)
			{
				_root[array_Str + "_array"][_loc3] = new Object();
				_root[array_Str + "_array"][_loc3].mallID = _loc5[0].childNodes[_loc3].attributes.mallID;
				_root[array_Str + "_array"][_loc3].buildID = _loc5[0].childNodes[_loc3].attributes.buildID;
				_root[array_Str + "_array"][_loc3].desc = _loc5[0].childNodes[_loc3].attributes.desc;
				_root[array_Str + "_array"][_loc3].adress = _loc5[0].childNodes[_loc3].attributes.adress;
				_root[array_Str + "_array"][_loc3].Img = _loc5[0].childNodes[_loc3].attributes.Img;
				_root[array_Str + "_array"][_loc3].remark = _loc5[0].childNodes[_loc3].attributes.remark;
			}
			// end of for
			//加载Building图标
			var lenN = _root[array_Str + "_array"].length;
			var dep = 0;
			for (var i = 0; i < lenN; i++)
			{
				var Obj = _root[array_Str + "_array"][i];
				if (Obj.mallID == _global.mallID)
				{
					_global.buildN++;
					_root.attachMovie("MallShow", "B" + _global.buildN, dep);
					var m = _root["B" + _global.buildN];
					m.loadImgM.loadMovie(_global.globalXMLPath + String(Obj.Img));
					m.idTxt = Obj.buildID;
					m.nmTxt.text = "[ " + Obj.desc + " ]";
					setProperty(m, _x, m._width * (int((_global.buildN - 1) % 3)) + 10 * ((int((_global.buildN - 1) % 3)) - 1) + 20);
					setProperty(m, _y, m._height * int((_global.buildN - 1) / 3) + 10 * (int((_global.buildN - 1) / 3) - 1) + 60);
					m.onPress = function()
					{
						_global.returnFrame = _currentframe;
						_global.buildID = this.idTxt;
						_global.Build = String(ExternalInterface.call("sendAreaID", String("Build^" + _global.buildID)));
						gotoAndStop("FloorShow");
					};
					m.onRollOver = function()
					{
						//设置右键菜单
						_global.ThisLevel = "BuildingID";
						_global.LevelID = this.idTxt;
						_global.loadXmlMallMenu = "BuildingMenu.xml";
						_global.setMenuMovie = this;
						getMenu();
					};
					m.onRollOut = function()
					{
						//设置右键菜单
						_global.ThisLevel = "MallID";
						_global.LevelID = _global.mallID;
						_global.loadXmlMallMenu = "MallMenu.xml";
						_global.setMenuMovie = this;
						getMenu();
					};
				}
				dep++;
			}
			this._parent.shopInforOK = true;
		}
		else
		{
			//_root.createTextField("t_txt", _root.getNextHighestDepth(), 0, 0, 100, 50);
			//_root.t_txt.text = "Load Error";
		}
		// end else if
	};
}
// End of the function
//Floor信息
function FloorXML(SubPath, array_Str)
{
	_global.floorN = 0;
	var _loc6 = new XML();
	_loc6.load(_global.globalXMLPath + SubPath);
	_loc6._parent = this;
	_loc6.ignoreWhite = true;
	_loc6.getBytesLoaded();
	_loc6.onLoad = function(e)
	{
		if (e)
		{
			var _loc5 = new XMLNode();
			_loc5 = this.childNodes;
			_root[array_Str + "_array"] = new Array();
			for (var _loc3 = 0; _loc3 < _loc5[0].childNodes.length; ++_loc3)
			{
				_root[array_Str + "_array"][_loc3] = new Object();
				_root[array_Str + "_array"][_loc3].mallID = _loc5[0].childNodes[_loc3].attributes.mallID;
				_root[array_Str + "_array"][_loc3].buildID = _loc5[0].childNodes[_loc3].attributes.buildID;
				_root[array_Str + "_array"][_loc3].floorID = _loc5[0].childNodes[_loc3].attributes.floorID;
				_root[array_Str + "_array"][_loc3].desc = _loc5[0].childNodes[_loc3].attributes.desc;
				_root[array_Str + "_array"][_loc3].adress = _loc5[0].childNodes[_loc3].attributes.adress;
				_root[array_Str + "_array"][_loc3].Img = _loc5[0].childNodes[_loc3].attributes.Img;
				_root[array_Str + "_array"][_loc3].remark = _loc5[0].childNodes[_loc3].attributes.remark;
			}
			// end of for
			//加载Floor图标
			var lenN = _root[array_Str + "_array"].length;
			var dep = 0;
			for (var i = 0; i < lenN; i++)
			{
				var Obj = _root[array_Str + "_array"][i];
				if ((Obj.mallID == _global.mallID) && (Obj.buildID == _global.buildID))
				{
					_global.floorN++;
					_root.attachMovie("MallShow", "F" + _global.floorN, dep);
					var m = _root["F" + _global.floorN];
					m.loadImgM.loadMovie(_global.globalXMLPath + String(Obj.Img));
					m.idTxt = Obj.floorID;
					//m.nmTxt.text = "[ "+Obj.desc+" ]";
					setProperty(m, _x, (_global.stageWidth - m._width) / 2 - m._width / 2);
					setProperty(m, _y, m._height * int((_global.floorN - 1) / 1) + 10 * (int((_global.floorN - 1) / 1) - 1) + 60);
					setProperty(m, _alpha, 100);
					m.onPress = function()
					{
						_global.returnFrame = _currentframe;
						_global.floorID = this.idTxt;
						_global.Floor = String(ExternalInterface.call("sendAreaID", String("Floor^" + _global.floorID)));
						_global.loadXmlFileName = _global.globalXMLPath + _global.ShopFile;
						gotoAndStop("ShopShow");
					};
					m.onRollOver = function()
					{
						//设置右键菜单
						_global.ThisLevel = "FloorID";
						_global.LevelID = this.idTxt;
						_global.loadXmlMallMenu = "FloorMenu.xml";
						_global.setMenuMovie = "";
						getMenu();
						this._alpha = 50;
					};
					m.onRollOut = function()
					{
						//设置右键菜单
						_global.ThisLevel = "BuildingID";
						_global.LevelID = _global.buildID;
						_global.loadXmlMallMenu = "BuildingMenu.xml";
						_global.setMenuMovie = "";
						getMenu();
						this._alpha = 100;
					};
				}
				dep++;
			}
			this._parent.shopInforOK = true;
		}
		else
		{
			//_root.createTextField("t_txt", _root.getNextHighestDepth(), 0, 0, 100, 50);
			//_root.t_txt.text = "Load Error";
		}
		// end else if
	};
}
// End of the function
//Shop信息
function ShopXML(SubPath, array_Str)
{
	_global.shopN = 0;
	var _loc6 = new XML();
	_loc6.load(_global.globalXMLPath + SubPath);
	_loc6._parent = this;
	_loc6.ignoreWhite = true;
	_loc6.getBytesLoaded();
	_loc6.onLoad = function(e)
	{
		if (e)
		{
			var _loc5 = new XMLNode();
			_loc5 = this.childNodes;
			_root[array_Str + "_array"] = new Array();
			for (var _loc3 = 0; _loc3 < _loc5[0].childNodes.length; ++_loc3)
			{
				var a = _loc5[0].childNodes[_loc3];
				_root[array_Str + "_array"] = new Array();
				//trace( "buildID:" + a.attributes.buildID +":"+ _global.buildID + "floorID:" + a.attributes.floorID +":"+ _global.floorID );
				if ((a.attributes.buildID == _global.buildID) && (a.attributes.floorID == _global.floorID))
				{
					for (var j = 0; j < a.childNodes.length; j++)
					{
						_root[array_Str + "_array"][j] = new Object();
						var Obj_array = _root[array_Str + "_array"][j];
						Obj_array.shopID = a.childNodes[j].attributes.shopID;
						Obj_array.ShopCode = a.childNodes[j].attributes.ShopCode;
						Obj_array.ShopDesc = a.childNodes[j].attributes.ShopDesc;
						Obj_array.map = a.childNodes[j].attributes.map;
						Obj_array.xN = a.childNodes[j].attributes.x;
						Obj_array.yN = a.childNodes[j].attributes.y;
						Obj_array.rb = a.childNodes[j].attributes.rb;
						Obj_array.gb = a.childNodes[j].attributes.gb;
						Obj_array.bb = a.childNodes[j].attributes.bb;
						Obj_array.NoX = a.childNodes[j].attributes.NoX;
						Obj_array.NoY = a.childNodes[j].attributes.NoY;
						Obj_array.NameX = a.childNodes[j].attributes.NameX;
						Obj_array.NameY = a.childNodes[j].attributes.NameY;
						Obj_array.Remark = a.childNodes[j].attributes.Remark;
						/**
						
						trace(Obj_array.shopID);
						trace(Obj_array.ShopCode);
						trace(Obj_array.ShopDesc);
						trace(Obj_array.map);
						trace(Obj_array.xN);
						trace(Obj_array.yN);
						trace(Obj_array.rb);
						trace(Obj_array.gb);
						trace(Obj_array.bb);
						trace(Obj_array.NoX);
						trace(Obj_array.NoY);
						trace(Obj_array.NameX);
						trace(Obj_array.NameY);
						trace(Obj_array.Remark);
						
						/**/
					}
					// end of for
					break;
				}
				// end if            
			}
			// end of for
			//加载Shop图标
			/**/
			var lenN = _root[array_Str + "_array"].length;
			var dep = 0;
			for (var i = 0; i < lenN; i++)
			{
				var Obj = _root[array_Str + "_array"][i];
				_global.shopN++;
				_root.MainMovie.loadFM.loadShops.attachMovie("loadIcon", ("S" + _global.shopN), dep, {x:0, y:0});
				//{_x:Obj.xN - 10, _y:Obj.yN + 26}
				var m = _root.MainMovie.loadFM.loadShops["S" + _global.shopN];
				m.loadImgM.loadMovie(String(_global.globalXMLPath + Obj.map));
				m.idTxt = Obj.shopID;
				var t1 = "" + Obj.rb + "" + Obj.gb + "" + Obj.bb;
				if ((t1 == "2552550") || (t1 == "0255255") || (t1 == "02550"))
				{
					m.NoTxt.htmlText = "<font color='#333333'>" + Obj.ShopCode + "</font>";
					m.NameTxt.htmlText = "<font color='#333333'>" + Obj.ShopDesc + "</font>";
				}
				else
				{
					m.NoTxt.htmlText = "<font color='#fafafa'>" + Obj.ShopCode + "</font>";
					m.NameTxt.htmlText = "<font color='#fafafa'>" + Obj.ShopDesc + "</font>";
				}
				m.NoTxt._x = Obj.NoX;
				m.NoTxt._y = Obj.NoY;
				m.NameTxt._x = Obj.NameX;
				m.NameTxt._y = Obj.NameY;
				setProperty(m, _x, Obj.xN - 10);
				setProperty(m, _y, Obj.yN - 38);
				//着色
				var my_color:Color = new Color(m.loadImgM);
				var myTransform:Object = my_color.getTransform();
				myTransform = {ra:100, rb:Obj.rb, ga:100, gb:Obj.gb, ba:100, bb:Obj.bb};
				my_color.setTransform(myTransform);
				//着色完毕
				m.onPress = function()
				{
					_global.shopID = this.idTxt;
					_global.Shop = String(ExternalInterface.call("sendAreaID", String("Shop^" + _global.shopID)));
				};
				m.onRollOver = function()
				{
					//设置右键菜单
					_global.ThisLevel = "ShopID";
					_global.LevelID = this.idTxt;
					_global.loadXmlMallMenu = "ShopMenu.xml";
					_global.setMenuMovie = "";
					getMenu();
				};
				//m.onRollOut = function()
				//{
				//设置右键菜单
				//_global.ThisLevel = "FloorID";
				//_global.LevelID = this.idTxt;
				//_global.loadXmlMallMenu = "FloorMenu.xml";
				//_global.setMenuMovie = "";
				//getMenu();
				//};
				dep++;
			}
			/**/
			this._parent.shopInforOK = true;
		}
		else
		{
			//_root.createTextField("t_txt", _root.getNextHighestDepth(), 0, 0, 100, 50);
			//_root.t_txt.text = "Load Error";
		}
		// end else if
	};
}
// End of the function
//ToolBar信息
function ToolBarXML(SubPath, array_Str)
{
	_global.toolN = 0;
	var _loc6 = new XML();
	_loc6.load(_global.globalXMLPath + SubPath);
	_loc6._parent = this;
	_loc6.ignoreWhite = true;
	_loc6.getBytesLoaded();
	_loc6.onLoad = function(e)
	{
		if (e)
		{
			var _loc5 = new XMLNode();
			_loc5 = this.childNodes;
			_root[array_Str + "_array"] = new Array();
			for (var _loc3 = 0; _loc3 < _loc5[0].childNodes.length; ++_loc3)
			{
				_root[array_Str + "_array"][_loc3] = new Object();
				_root[array_Str + "_array"][_loc3].toolbarID = _loc5[0].childNodes[_loc3].attributes.toolbarID;
				_root[array_Str + "_array"][_loc3].desc = _loc5[0].childNodes[_loc3].attributes.desc;
				_root[array_Str + "_array"][_loc3].Edesc = _loc5[0].childNodes[_loc3].attributes.Edesc;
				_root[array_Str + "_array"][_loc3].Img = _loc5[0].childNodes[_loc3].attributes.Img;
				_root[array_Str + "_array"][_loc3].remark = _loc5[0].childNodes[_loc3].attributes.remark;
				_root[array_Str + "_array"][_loc3].urlS = _loc5[0].childNodes[_loc3].attributes.urlS;
				_root[array_Str + "_array"][_loc3].clrS = _loc5[0].childNodes[_loc3].attributes.clrS;
			}
			// end of for
			//加载toolBar图标
			var lenN = _root[array_Str + "_array"].length;
			var dep = 0;
			for (var i = 0; i < lenN; i++)
			{
				var Obj = _root[array_Str + "_array"][i];
				_global.toolN++;
				_root.attachMovie("ToolIcon", "T" + _global.toolN, dep + 80);
				var m = _root["T" + _global.toolN];
				m.loadImgM.loadMovie(_global.globalXMLPath + String(Obj.Img));
				m.idTxt.text = Obj.desc;
				m.urlS = Obj.urlS;
				m.clrS = Obj.clrS;
				setProperty(m, _x, (m._width) * (int((_global.toolN - 1) % _global.toolN)) + 5 * ((int((_global.toolN - 1) % _global.toolN)) - 1) + 20);
				setProperty(m, _y, 15);
				m.mnT = Obj.desc;
				m.toolBtn.onPress = function()
				{
					for (var k = 1; k <= _global.toolN; k++)
					{
						var tt = _root["T" + k].idTxt.text;
						_root["T" + k].gotoAndStop(1);
						_root["T" + k].idTxt.text = tt;
					}
					var tt = this._parent.idTxt.text;
					this._parent.gotoAndStop(2);
					this._parent.idTxt.text = tt;
					if (this._parent.urlS != "")
					{
						_global.ColorLumpFile = this._parent.clrS;
						_global.ShopFile = this._parent.urlS;
						gotoAndStop("reloadS");
					}
					else
					{
						//提示无数据
						if (_global.shopN > 0)
						{
							for (var i = 1; i <= _global.shopN; i++)
							{
								removeMovieClip(_root["S" + i]);
							}
						}
						this._parent._parent.MainMovie.errorM.gotoAndPlay("errorMessage");
					}
				};
				m.toolBtn.onRollOver = function()
				{
					this._parent.showTextM.gotoAndPlay("over");
				};
				m.toolBtn.onRollOut = function()
				{
					this._parent.showTextM.gotoAndStop("out");
				};
				dep++;
			}
			this._parent.toolInforOK = true;
		}
		else
		{
			//_root.createTextField("t_txt", _root.getNextHighestDepth(), 0, 0, 100, 50);
			//_root.t_txt.text = "Load Error";
		}
		// end else if
	};
}
// End of the function
//colorLump信息
function ColorLumpXML(SubPath, array_Str)
{
	_global.clrN = 0;
	var _loc6 = new XML();
	_loc6.load(_global.globalXMLPath + SubPath);
	_loc6._parent = this;
	_loc6.ignoreWhite = true;
	_loc6.getBytesLoaded();
	_loc6.onLoad = function(e)
	{
		if (e)
		{
			var _loc5 = new XMLNode();
			_loc5 = this.childNodes;
			_root[array_Str + "_array"] = new Array();
			for (var _loc3 = 0; _loc3 < _loc5[0].childNodes.length; ++_loc3)
			{
				_root[array_Str + "_array"][_loc3] = new Object();
				_root[array_Str + "_array"][_loc3].toolbarID = _loc5[0].childNodes[_loc3].attributes.toolbarID;
				_root[array_Str + "_array"][_loc3].desc = _loc5[0].childNodes[_loc3].attributes.desc;
				_root[array_Str + "_array"][_loc3].rb = _loc5[0].childNodes[_loc3].attributes.rb;
				_root[array_Str + "_array"][_loc3].gb = _loc5[0].childNodes[_loc3].attributes.gb;
				_root[array_Str + "_array"][_loc3].bb = _loc5[0].childNodes[_loc3].attributes.bb;
			}
			// end of for
			//加载colorLump图标
			var lenN = _root[array_Str + "_array"].length;
			var dep = 0;
			for (var i = 1; i <= lenN; i++)
			{
				var Obj = _root[array_Str + "_array"][i - 1];
				var m = _root.MainMovie.clrLump.colorLump["colorIcon" + i];
				//m.clr = 
				m._alpha = 100;
				m.iconTxt._visible = true;
				m.iconTxt.text = Obj.desc;
				//着色
				var my_color:Color = new Color(m.clr);
				var myTransform:Object = my_color.getTransform();
				myTransform = {ra:100, rb:Obj.rb, ga:100, gb:Obj.gb, ba:100, bb:Obj.bb};
				my_color.setTransform(myTransform);
				//着色完毕
				m._parent.prevBtn.gotoAndStop(1);
				m._parent.nextBtn.gotoAndStop(1);
			}
			if (lenN < 8)
			{
				for (var i = (lenN + 1); i <= 8; i++)
				{
					var m = _root.MainMovie.clrLump.colorLump["colorIcon" + i];
					m._parent.prevBtn.gotoAndStop(2);
					m._parent.nextBtn.gotoAndStop(2);
					m._alpha = 0;
					m.iconTxt._visible = false;
				}
			}
			this._parent.colorLumpOK = true;
		}
		else
		{
			//_root.createTextField("t_txt", _root.getNextHighestDepth(), 0, 0, 100, 50);
			//_root.t_txt.text = "Load Error";
		}
		// end else if
	};
}
// End of the function
//楼层控制相关函数
//mouseIconShow
function mouseIconShow(str)
{
	Mouse.hide();
	var p = this[str];
	_global.tempX = p._x;
	_global.tempY = p._y;
	p.onEnterFrame = function()
	{
		this._x = _xmouse;
		this._y = _ymouse;
	};
}
//mouseIconHide
function mouseIconHide(str)
{
	Mouse.show();
	var p = this[str];
	delete (p.onEnterFrame);
	p._x = _global.tempX;
	p._y = _global.tempY;
}
//loadSet
function loadSet()
{
	if (_global.setXYnum != true)
	{
		/**/
		var w = Math.round(100 * (820 / loadFM._width)) / 100;
		var h = Math.round(100 * (400 / loadFM._height)) / 100;
		if (w < 1)
		{
			_global.whNum = w;
			loadFM._xscale = loadFM._xscale * w;
			loadFM._yscale = loadFM._yscale * w;
			setXY();
		}
		else if (h < 1)
		{
			_global.whNum = h;
			loadFM._xscale = loadFM._xscale * h;
			loadFM._yscale = loadFM._yscale * h;
			setXY();
		}
		else
		{
			setXY();
		}
		/**/
		_global.setXYnum = true;
	}
}
//setXY
function setXY()
{
	//trace( loadFM._xscale );
	loadFM._x = (860 - loadFM._width) / 2;
	if (loadFM._height > 420)
	{
		loadFM._y = 60;
	}
	else if (loadFM._height <= 420)
	{
		loadFM._y = 520 - loadFM._height - (520 - loadFM._height) / 2 - 5;
	}
}
