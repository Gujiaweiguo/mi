using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Base.Sys
{
   public class MoveXMLFiles
    {
       public void MoveFiles(int user)
       {
           Directory.CreateDirectory("E:\\work\\mi_net\\CODE\\Web\\VisualAnalysis\\VAMenu\\" + user.ToString());
           string path = "E:/WORK/MI_NET/CODE/Web/VisualAnalysis/VAMenu/";
           if (File.Exists(path + user.ToString() + "/LocaMenu.xml") == false)
           {
               File.Copy(path + "U001/LocaMenu.xml", path + user.ToString() + "/LocaMenu.xml", true);
           }
           if (File.Exists(path + user.ToString() + "/LocaMenu.xml") == true)
           {
               File.Delete(path + user.ToString() + "/LocaMenu.xml");
               File.Copy(path + "U001/LocaMenu.xml", path + user.ToString() + "/LocaMenu.xml", true);
           }


           if (File.Exists(path + user.ToString() + "/CityMenu.xml") == false)
           {
               File.Copy(path + "U001/CityMenu.xml", path + user.ToString() + "/CityMenu.xml", true);
           }
           if (File.Exists(path + user.ToString() + "/CityMenu.xml") == true)
           {
               File.Delete(path + user.ToString() + "/CityMenu.xml");
               File.Copy(path + "U001/CityMenu.xml", path + user.ToString() + "/CityMenu.xml", true);
           }


           if (File.Exists(path + user.ToString() + "/MallMenu.xml") == false)
           {
               File.Copy(path + "U001/MallMenu.xml", path + user.ToString() + "/MallMenu.xml", true);
           }
           if (File.Exists(path + user.ToString() + "/MallMenu.xml") == true)
           {
               File.Delete(path + user.ToString() + "/MallMenu.xml");
               File.Copy(path + "U001/MallMenu.xml", path + user.ToString() + "/MallMenu.xml", true);
           }

           if (File.Exists(path + user.ToString() + "/BuildingMenu.xml") == false)
           {
               File.Copy(path + "U001/BuildingMenu.xml", path + user.ToString() + "/BuildingMenu.xml", true);
           }
           if (File.Exists(path + user.ToString() + "/BuildingMenu.xml") == true)
           {
               File.Delete(path + user.ToString() + "/BuildingMenu.xml");
               File.Copy(path + "U001/BuildingMenu.xml", path + user.ToString() + "/BuildingMenu.xml", true);
           }

           if (File.Exists(path + user.ToString() + "/FloorMenu.xml") == false)
           {
               File.Copy(path + "U001/FloorMenu.xml", path + user.ToString() + "/FloorMenu.xml", true);
           }
           if (File.Exists(path + user.ToString() + "/FloorMenu.xml") == true)
           {
               File.Delete(path + user.ToString() + "/FloorMenu.xml");
               File.Copy(path + "U001/FloorMenu.xml", path + user.ToString() + "/FloorMenu.xml", true);
           }

           if (File.Exists(path + user.ToString() + "/ShopMenu.xml") == false)
           {
               File.Copy(path + "U001/ShopMenu.xml", path + user.ToString() + "/ShopMenu.xml", true);
           }
           if (File.Exists(path + user.ToString() + "/ShopMenu.xml") == true)
           {
               File.Delete(path + user.ToString() + "/ShopMenu.xml");
               File.Copy(path + "U001/ShopMenu.xml", path + user.ToString() + "/ShopMenu.xml", true);
           }

           if (File.Exists(path + user.ToString() + "/ToolBar.xml") == false)
           {
               File.Copy(path + "U001/ToolBar.xml", path + user.ToString() + "/ToolBar.xml", true);
           }
           if (File.Exists(path + user.ToString() + "/ToolBar.xml") == true)
           {
               File.Delete(path + user.ToString() + "/ToolBar.xml");
               File.Copy(path + "U001/ToolBar.xml", path + user.ToString() + "/ToolBar.xml", true);
           }

       
       }


    }
}
