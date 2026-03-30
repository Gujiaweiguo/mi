using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using System.Web.UI;

// 有关程序集的常规信息通过下列属性集
// 控制。更改这些属性值可修改
// 与程序集关联的信息。
[assembly: AssemblyTitle("YYControls")]
[assembly: AssemblyDescription("http://webabcd.cnblogs.com")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("http://webabcd.cnblogs.com")]
[assembly: AssemblyProduct("YYControls")]
[assembly: AssemblyCopyright("版权所有 (C) http://webabcd.cnblogs.com 2007")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 使此程序集中的类型
// 对 COM 组件不可见。如果需要从 COM 访问此程序集中的类型，
// 则将该类型上的 ComVisible 属性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("afe6caf2-54cb-4ad4-8d73-8bf0af5b1880")]

// 程序集的版本信息由下面四个值组成:
//
//      主版本
//      次版本 
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“修订号”和“内部版本号”的默认值，
// 方法是按如下所示使用“*”:
[assembly: AssemblyVersion("1.3.0.0")]
[assembly: AssemblyFileVersion("1.3.0.0")]
[assembly: AssemblyInformationalVersion("1.3.0.0")]  // 用来定义 Product Version


[assembly: TagPrefix("YYControls", "yyc")]





  //   一,.NET程序集版本号
  //         1,.NET程序集版本信息组成,以及存放地址
  //                  .NET版本信息主要分为下面几个部分:
  //                          标题(Title)      [assembly: AssemblyTitle("")]         
  //                          说明(Description) [assembly: AssemblyDescription("")]
  //                          公司(Company) [assembly: AssemblyCompany("")]
  //                         产品( Product)  [assembly: AssemblyProduct("")]
  //                          版权(CopyRight)[assembly: AssemblyCopyright("")]
  //                          商标( Trademark)[assembly: AssemblyTrademark("")]
  //                          程序集版本号(Assembly Vision)[assembly: AssemblyVersion("1.0.0.0")]
  //                          文件版号:(File Version)[assembly: AssemblyFileVersion("1.0.0.0")]
  //                           GUID:[assembly: Guid("31d65aef-12cb-4ea4-b7c6-ba1daafdbd31")]
  //                           非特定语言(Neutral Language):[assembly: AssemblyCulture("")]
  //                            是否COM可见 [assembly: ComVisible(false)]
  //                    在这些部分中主要所使用的是Assembly Vision.
  //                  该版本信息都是存储在程序的Assembly .cs下面,只不过,在VS2005中Assembly 文件是存放
  //         Properties文件夹下面的,而VS2003中是直接放在项目文件夹下面.
             
  //               版本号作用:
  //                  当某个Client程序集引用Server 程序集的时候,他会在他的项目管理文件(XML格式)中加入
  //         引用关系中,当编译完成后的程序,程序集会根据其引用版本来查找相应的DLL文件,
  //          对于DLL文件有2中确定其的方法.
  //            (1)弱方法:其实也就是通过程序集的名称来决定其引用的方式,这个名称称为(friendly named)
  //            (2)强方法,不仅检查程序集名称,同时还会检查版本号名称.
  //2 版本号的组成
  //    版本号的组成主要有4个部分的号码
  //           Major(主版本号)
  //           Minor(次版本号)
  //           Build(生成版本号)
  //           Revision(修订版本号)
  //         版本号使用:
  //         一般以微软所提供的方式是,前2个组成面向公众版本号第3个是做为程序集生成版本号来处理的,而最后   一个则表示的是修订版本号,在某个更短的时间生成的
  //     程序是使用修订版本号的.
  //    3种版本号
  //           AssemblyFileVersion:(文件版本号)
  //                存放在Win32版本资源中,仅仅为一个辅助的信息.CLR不会去处理该版本号,而只是关心程序集版本
  //     号
  //           AssemblyInfomationVersionAttribute 
  //                  该版本号也只是做辅助信息来使用,CLR也不会去处理.
  //           Assembly Version (程序集版本号)
  //                   通过该版本号来对程序集进行唯一的标识.


