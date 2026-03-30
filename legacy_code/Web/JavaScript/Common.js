
/*********************************************************************************
***功能: JavaScript验证
***作者: 冯武
***时间: 2005/10
**********************************************************************************/

/*********************************************************************************
***功能: 校验字符串是否为空
***返回值：bool
***如果不为空，定义校验通过，返回true
***如果为空，校验不通过，返回false               参考提示信息：输入域不能为空！
**********************************************************************************/
function isEmpty(str)
{
	return (str.trim()=="");er
}

/********************************************************************
***功能:校验字符串是否为整型
*返回值：bool
*如果为空，定义校验通过，      返回true
*如果字串全部为数字，校验通过，返回true
*如果校验不通过，              返回false     参考提示信息：输入域必须为数字！
*********************************************************************/
function isInteger(str)
{

    if (isEmpty(str)) return true; //如果为空，则通过校验
    
    //var reg=/^(\-?)(\d+)$/;
    
    var reg = /^\d+(?=\.{0,1}\d+$|$)/;
    //var reg = ^[1-9]+\d*$
  return reg.test(str);
 
}

/*************************************************************************
***功能:校验字符串是否为浮点型
*返回值：bool
*如果为空，定义校验通过，      返回true
*如果字串为浮点型，校验通过，  返回true
*如果校验不通过，              返回false     参考提示信息：输入域不是合法的浮点数！
*************************************************************************/
function isDouble(str)
{
    if (isEmpty(str)) return true; //如果为空，则通过校验
    
    //如果是整数，则校验整数的有效性
	var reg = /^(?:\+|-)?\d+(?:\.\d+)?$/;
	return reg.test(str);	        
}

/*********************************************************************************
*校验字符串是否为日期型
*返回值：bool
*如果为空，定义校验通过，           返回true
*如果字串为日期型，校验通过，       返回true
*如果日期不合法，                   返回false    参考提示信息：输入域的时间不合法！（yyyy-MM-dd）
********************************** date ******************************************/
function isDate(str)
{
    if (isEmpty(str)) return true; //如果为空，则通过校验
     
    var pattern = /^((\d{4})|(\d{2}))-(\d{1,2})-(\d{1,2})$/g;
    if(!pattern.test(str))
        return false;
    var arrDate = str.split("-");
    if(parseInt(arrDate[0],10) < 100)
        arrDate[0] = 2000 + parseInt(arrDate[0],10) + "";
    var date =  new Date(arrDate[0],(parseInt(arrDate[1],10) -1)+"",arrDate[2]);
    if(date.getFullYear() == arrDate[0]
       && date.getMonth() == (parseInt(arrDate[1],10) -1)+""
       && date.getDate() == arrDate[2])
        return true;
    else
        return false;
}

/******************************************************************************
*校验字符串是否为email型
*返回值：bool
*如果为空，定义校验通过，           返回true
*如果字串为email型，校验通过，      返回true
*如果email不合法，                  返回false    参考提示信息：Email的格式不正確！
********************************************************************************/
function isEmail(str)
{
    if (isEmpty(str)) return true; //如果为空，则通过校验
     
    if (str.charAt(0) == "." || str.charAt(0) == "@" || str.indexOf('@', 0) == -1
        || str.indexOf('.', 0) == -1 || str.lastIndexOf("@") == str.length-1 || str.lastIndexOf(".") == str.length-1)
        return false;
    else
        return true;
}

/*********************************************************************************
*校验输入值是否在两值之间
*返回值：bool
*如果为空，定义校验通过，           返回true
**********************************************************************************/
function isBetween (val, lo, hi) {
    if ((val < lo) || (val > hi)) { return(false); }
    else { return(true); }
}


/*********************************************************************************
***功能：扩展日期函数，支持YYYY-MM-DD或YYYY-MMDD hh:mm:ss字符串格式
***返回：如果正确，则返回javascript中支持UTC日期格式，如果错误，则返回false  
***日期：2004-12-15
***举例： var myDate = isDate("2004-12-21 23:01:00");   //返回正确的日期
***       var myDate = isDate("2004-12-21");            //返回正确的日期
***       var myDate = isDate("2004-23-12 12:60:29");   //返回false，
**********************************************************************************/  
function isDateTime(str)
{
   if (str=="") return true;
   
   var strDate = str;
   if (strDate.length == 0) return false;

   //先判断是否为短日期格式：YYYY-MM-DD，如果是，将其后面加上00:00:00，转换为YYYY-MM-DD hh:mm:ss格式
   var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})/;   //短日期格式的正则表达式
   var r = strDate.match(reg);

   if (r != null)   //说明strDate是短日期格式，改造成长日期格式
      strDate = strDate + " 00:00:00";
 
    reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})/;
    r = strDate.match(reg);
    if (r == null)
    {
       //alert("你输入的日期格式有误，正确格式为：2004-12-01 或 2004-12-01 12:23:45");
       return false;
    }

    var d = new Date(r[1], r[3]-1,r[4],r[5],r[6],r[7]);
    if (d.getFullYear()==r[1]&&(d.getMonth()+1)==r[3]&&d.getDate()==r[4]&&d.getHours()==r[5]&&d.getMinutes()==r[6]&&d.getSeconds()== r[7])
    {
       return d;
    }
    else
    {
       //alert("你输入的日期或时间超出有效范围，请仔细检查！");
       return false;
    }
}

/*********************************************************************************
*    EO_JSLib.js
*   javascript正则表达式检验
**********************************************************************************/
//校验是否全由汉字组成
function isChineseChar(str)
{
   if (isEmpty(str)) return true; //如果为空，则通过校验
  
   var reg=/^([\u4E00-\u9FA5]|[\uFE30-\uFFA0])*$/gi;
   return reg.test(str);  
}

//校验是否为英文字符，包括大小写
function isEngCharAll(str)
{ 
   if (isEmpty(str)) return true;	
   var reg=/^[a-zA-Z]{1,20}$/;
   return reg.test(str);
}

//校验是否为英文字符，只有大写
function isEngCharUpper(str)
{ 
   if (isEmpty(str)) return true;
   var reg=/^[A-Z]{1,20}$/;
   return reg.test(str);
  
}

//校验是否为英文字符，只有小写
function isEngCharLower(str)
{ 
   if (isEmpty(str)) return true;
   var reg=/^[a-z]{1,20}$/;
   return reg.test(str);
 
}

//校验是否为邮政编码(中国6位数字)
function isPostCode(str)
{
   if (isEmpty(str)) return true;
   var reg=/^[0-9]{6}$/;
   return reg.test(str);
  
}

//校验是否全由数字组成
function isDigit(str)
{
    if (isEmpty(str)) return true;
    var reg=/^[0-9]{1,20}$/;
    return reg.test(str);  
}

//校验是否全由数字(带小数点)组成
function isDigitDot(str)
{
    if (isEmpty(str)) return true;
    var reg=/^[0-9.]{1,20}$/;
    return reg.test(str);
}

//校验登录名：只能输入5-20个以字母开头、可带数字、“_”、“.”的字串
function isRegisterUserName(str)
{
    if (isEmpty(str)) return true;
    var reg=/^[a-zA-Z]{1}([a-zA-Z0-9]|[._]){4,19}$/;
    return reg.test(str);
    
}

//校验用户姓名：只能输入1-30个以字母开头的字串
function isTrueName(str)
{
    if (isEmpty(str)) return true;
    var reg=/^[a-zA-Z]{1,30}$/;
    return reg.test(str);
    
}

//校验密码：只能输入1-20个字母、数字、下划线
function isPasswd(str)
{
    if (isEmpty(str)) return true;
    var reg=/^(\w){1,20}$/;
    return reg.test(str);
    
}

//校验普通电话、传真号码：可以“+”开头，除数字外，可含有“-”
function isTel(str)
{
    if (isEmpty(str)) return true;
    var reg=/^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/;
    return reg.test(str);
    
}

//校验手机号码：必须以数字开头，除数字外，可含有“-”
function isMobil(str)
{
    if (isEmpty(str)) return true;
    var reg=/^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/;
    return reg.test(str);
    
}

//校验IP地址
function isIP(str)  
{
   if (isEmpty(str)) return true; 
   var reg = /^((1??\d{1,2}|2[0-4]\d|25[0-5])\.){3}(1??\d{1,2}|2[0-4]\d|25[0-5])$/;
   return reg.test(str);
}

/*********************************************************************************
*    FUNCTION:        isReal
*    PARAMETER:    heStr    AS String 
                        decLen    AS Integer (how many digits after period)
*    RETURNS:        TRUE if theStr is a float, otherwise FALSE
*    CALLS:            isInt
**********************************************************************************/
function isReal (theStr, decLen) {
    var dot1st = theStr.indexOf('.');
    var dot2nd = theStr.lastIndexOf('.');
    var OK = true;
    
    if (isEmpty(theStr)) return false;

    if (dot1st == -1) {
        if (!isInt(theStr)) return(false);
        else return(true);
    }
    
    else if (dot1st != dot2nd) return (false);
    else if (dot1st==0) return (false);
    else {
        var intPart = theStr.substring(0, dot1st);
        var decPart = theStr.substring(dot2nd+1);

        if (decPart.length > decLen) return(false);
        else if (!isInt(intPart) || !isInt(decPart)) return (false);
        else if (isEmpty(decPart)) return (false);
        else return(true);
    }
}


/********************************************************************************
*   FUNCTION:       Compare Date! Which is the latest!
*   PARAMETERS:     lessDate,moreDate AS String
*   CALLS:          isDate,isBetween
*   RETURNS:        TRUE if lessDate<moreDate 
*   while return false alert errmsg
*********************************************************************************/
function isComdateMsg (lessDate1 ,startstr, moreDate1,endstr){
	var re =/\./g;
	var lessDate = lessDate1.replace(re,"-");
	var moreDate = moreDate1.replace(re,"-");
	var re1 =/\\/g;
	var lessDate = lessDate.replace(re1,"-");
	var moreDate = moreDate.replace(re1,"-");
	if(!isComdate(lessDate,moreDate)){
		var err = endstr + "必须大于";
		err = err + startstr;
		alert(err);
	}
	return isComdate(lessDate,moreDate);
}


function need_input(sForm)//通用文本域校验 
{ 
    for(i=0;i<sForm.length;i++)
    {  
		if(sForm[i].tagName.toUpperCase()=="TEXTAREA")
		{
			if(sForm[i].value.realLength()>sForm[i].title)
			{
				sWarn= "输入字符串超过最大值'" + sForm[i].title + "'";
				alert(sWarn);
				sForm[i].focus();
				return false;				
			}
		}
		if(sForm[i].tagName.toUpperCase()=="INPUT" &&sForm[i].type.toUpperCase()=="TEXT")
		{  
			sForm[i].value = sForm[i].value.trim();
			if(sForm[i].value.indexOf('\'')>=0)
			{
				sWarn= "不能包涵特殊字符串\"'\"!";
				alert(sWarn);
				sForm[i].focus();
				return false;				
			}
			if(sForm[i].value.realLength()>sForm[i].maxLength)
			{
				sWarn= "输入字符串超过最大值'" + sForm[i].maxLength + "'";
				alert(sWarn);
				sForm[i].focus();
				return false;				
			}
		} 
		if(sForm[i].tagName.toUpperCase()=="INPUT" &&sForm[i].type.toUpperCase()=="TEXT" && (sForm[i].title!=""))
			if(sForm[i].value=="")//
			{
				sWarn=sForm[i].title+"不能为空!";
				alert(sWarn);
				sForm[i].focus();
				return false;
			}
		}
	return true;
} 


//is trim
String.prototype.trim = function()
{
    return this.replace(/(^[\s]*)|([\s]*$)/g, "");
}
String.prototype.lTrim = function()
{
    return this.replace(/(^[\s]*)/g, "");
}
String.prototype.rTrim = function()
{
    return this.replace(/([\s]*$)/g, "");
}

//check length include Chinese
String.prototype.realLength = function()
{
  return this.replace(/[^\x00-\xff]/g,"**").length;
}


/*验证数字类型，只可输入数字*/
function TextLeave()
{   
    var key=window.event.keyCode;
    if(key==8 || key==46 || key==48 || key==49 || key==50 || key==51 || key==52 || key==53 || key==54 || key==55 || key==56 ||
               key==57 || key==190 || key == 96 || key == 97 || key == 98 || key == 99 || key == 100 || key == 101 || key == 102 ||
               key == 103 || key == 104 || key == 105 || key == 110)
    {
	    return window.event.returnValue =true;
    }else
    {
	    return window.event.returnValue =false;
    }
}


/*日期大小验证 <开始日期控件名称，结束日期控件名称，错误字符串>*/
function LicenseBoxValidator(BeginDate,EndDate,ErrorMsg)
{
    //日期
    var beginvalue=BeginDate.value;
    var endvalue=EndDate.value;

    if(beginvalue != "")
    {
        var begin=new Array();
        var end=new Array();
        begin=beginvalue.split("-");
        end=endvalue.split("-");
        var beginnum=begin[0]+begin[1]+begin[2];
        var endnum=end[0]+end[1]+end[2];

        if(endnum < beginnum)
        {
            parent.document.all.txtWroMessage.value=ErrorMsg;
            EndDate.focus();
            return false;	
        }
    }
}

/*文本框是否为空 <验证控件名称，错误字符串>*/
function TextIsNotNull(TextID,ErrorSrc)
{
    if(TextID.value.trim()=="")  
    {
        ErrorSrc.src="../../App_Themes/Main/Images/pic_error.gif";
//        TextID.focus();
    }
    else
    {
        ErrorSrc.src="../../App_Themes/Main/Images/pic_right.gif";
    }
}

/*格式话数字*/
function FormatNumber(srcStr,nAfterDot)        //nAfterDot小数位数
{
    var srcStr,nAfterDot;
    var resultStr,nTen;
    srcStr = ""+srcStr+"";
    strLen = srcStr.length;
    dotPos = srcStr.indexOf(".",0);
    if (dotPos == -1){
        resultStr = srcStr+".";
        for (i=0;i<nAfterDot;i++){
　　        resultStr = resultStr+"0";
        }
        return resultStr;
    }
    else{
        if ((strLen - dotPos - 1) >= nAfterDot){
　　        nAfter = dotPos + nAfterDot + 1;
　　        nTen =1;
　　        for(j=0;j<nAfterDot;j++){
　　　　        nTen = nTen*10;
　　        }
　　        resultStr = Math.round(parseFloat(srcStr)*nTen)/nTen;
　　        return resultStr;
        }
        else{
　　        resultStr = srcStr;
　　        for (i=0;i<(nAfterDot - strLen + dotPos + 1);i++){
　　　　        resultStr = resultStr+"0";
　　        }
　　        return resultStr;
        }
    }
} 