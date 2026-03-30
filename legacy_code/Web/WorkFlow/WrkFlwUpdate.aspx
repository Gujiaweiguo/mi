<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WrkFlwUpdate.aspx.cs" Inherits="WorkFlow_WrkFlwUpdate" ValidateRequest="false" %>


<html xmlns="http://www.w3.org/1999/xhtml" xmlns:v="urn:schemas-microsoft-com:vml">



    
<head id="Head1" runat="server">

    <title>无标题页</title>
    <link href="../App_Themes/CSS/ToolStyle.css" rel="stylesheet" type="text/css" />
    <link href="../App_Themes/Main/style.css" rel="stylesheet" type="text/css" />
    <script type ="text/javascript" src="../App_Themes/JS/contextMenu/context.js"></script>
    <script type="text/javascript" src="../App_Themes/JS/webflow.js"></script>
    <script type="text/javascript" src="../App_Themes/JS/function.js"></script>
    <script type="text/javascript" src="../App_Themes/JS/shiftlang.js"></script>
    <script type="text/javascript" src="../App_Themes/JS/movestep.js"></script>
        <script type="text/javascript" src="../JavaScript/Common.js"></script>
        <script type="text/javascript">
 <!--
  //提示信息
 function message()
 {
    parent.document.all.txtWroMessage.value =document.getElementById("hidInsert").value;
 }
   function hidAdd()
 {
    parent.document.all.txtWroMessage.value =document.getElementById("hidAdd").value;
    WorkFlowLoad();
 }
    function hidDelete()
 {
    parent.document.all.txtWroMessage.value =document.getElementById("hidDelete").value;
 }
 //验证数字类型
    function textleave(form1)
    {
        if(!isInteger(document.all.txtEfficiency.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidIntError").value;
            document.all.txtEfficiency.focus();
            return false;					
        }
        if(!isInteger(document.all.txtTraceDays.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidIntError").value;
            document.all.txtTraceDays.focus();
            return false;					
        }
        if(!isInteger(document.all.txtNodeWrkStep.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidIntError").value;
            document.all.txtNodeWrkStep.focus();
            return false;					
        }
        if(!isInteger(document.all.txtNodeLongestDelay.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidIntError").value;
            document.all.txtNodeLongestDelay.focus();
            return false;					
        } 
    }
    //text控件文本验证
    function WrkFlwValidator(sForm)
    {
        if(isEmpty(document.all.txtWrkFlwName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtWrkFlwName.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtEfficiency.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtEfficiency.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtTraceDays.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtTraceDays.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtNodeNodeName.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtNodeNodeName.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtNodeWrkStep.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtNodeWrkStep.focus();
            return false;					
        }
        
        if(isEmpty(document.all.txtNodeLongestDelay.value))  
        {
            parent.document.all.txtWroMessage.value=document.getElementById("hidMessage").value;
            document.all.txtNodeLongestDelay.focus();
            return false;					
        }
        return true;
    }
    //锁定工作流定义编辑
    function wrkFlwLock()
    {
        document.getElementById("txtWrkFlwName").readOnly=true;
        document.getElementById("txtWrkFlwName").className="Enabletextstyle";
        document.getElementById("txtEfficiency").readOnly=true;
        document.getElementById("txtEfficiency").className="Enabletextstyle";
        document.getElementById("txtTraceDays").readOnly=true;
        document.getElementById("txtTraceDays").className="Enabletextstyle";
        document.getElementById("txtWrkFlwProcessClass").readOnly=true;
        document.getElementById("txtWrkFlwProcessClass").className="Enabletextstyle";
        document.getElementById("cmbBizGrpID").disabled = true;
        document.getElementById("cmbInitVoucher").disabled = true;
        document.getElementById("cmbWrkFlwStatus").disabled = true;
        document.getElementById("cmbTransit").disabled = true;
        document.getElementById("cmbVoucherTypeID").disabled = true;
        document.getElementById("btnSave").disabled="";
        document.getElementById("btnDelete").disabled="";
    }
 
 
 
 var stepid = '0';
  var laststeptext= new Array();
 var laststeptid;
 var indicate='0';
 var laststepname='';
  var nodeid='0';
    function loadFromXML()
    {
        document.form1.FlowXML.value = '<WebFlow><FlowConfig><BaseProperties flowId="newflow1" flowText=""/><VMLProperties stepTextColor="blue" stepStrokeColor="green" stepShadowColor="#b3b3b3" stepFocusedStrokeColor="yellow" isStepShadow="T" actionStrokeColor="green" actionTextColor="" actionFocusedStrokeColor="yellow" sStepTextColor="blue" sStepStrokeColor="green" stepColor1="green" stepColor2="white" isStep3D="false" step3DDepth="20"/><FlowProperties flowMode="" startTime="" endTime="" ifMonitor="" runMode="" noteMode="" activeForm="" autoExe=""/></FlowConfig><Steps><Step><BaseProperties id="begin" text="开始" stepid="0" stepType="BeginStep"/><VMLProperties width="150" height="80" x="35" y="200" textWeight="" strokeWeight="" isFocused="" zIndex=""/><FlowProperties/></Step><Step><BaseProperties id="end" text="结束" stepid="99" stepType="EndStep"/><VMLProperties width="150" height="80" x="1800" y="200" textWeight="" strokeWeight="" isFocused="" zIndex=""/><FlowProperties/></Step><Step><BaseProperties id="q1" text="q1" stepid="1" stepType="NormalStep"/><VMLProperties width="180" height="100" x="410px" y="76px" textWeight="" strokeWeight="" zIndex="40"/><FlowProperties/></Step><Step><BaseProperties id="q2" text="q2" stepid="2" stepType="NormalStep"/><VMLProperties width="180" height="100" x="431px" y="285px" textWeight="" strokeWeight="" zIndex="40"/><FlowProperties/></Step></Steps><Actions><Action><BaseProperties id="action" text="action0" actionType="PolyLine" from="" to=""/><VMLProperties startArrow="" endArrow="classic" strokeWeight="" isFocused="" zIndex="39"/><FlowProperties/></Action><Action><BaseProperties id="q11" text="q1" actionType="PolyLine" from="begin" to="step1"/><VMLProperties startArrow="" endArrow="Classic" strokeWeight="" zIndex="39"/><FlowProperties/></Action><Action><BaseProperties id="q22" text="q2" actionType="PolyLine" from="begin" to="step2"/><VMLProperties startArrow="" endArrow="Classic" strokeWeight="" zIndex="39"/><FlowProperties/></Action></Actions></WebFlow>';
        redrawVML();
    }
    
    //自定义加载工作流页面时自动填充开始和结束节点
function getFlowXML(){

    /*按钮初始化文本*/
    document.getElementById("btnAddNode").value=document.getElementById("hidbtnAdd").value;
    document.getElementById("UpdateWrkFlwNode").value=document.getElementById("hidbtnUpdate").value;
    document.getElementById("btnNodeCancel").value=document.getElementById("hidbtnDelete").value;

  flowId ='newflow1';
  flowText = document.getElementById("txtWrkFlwName").value;
  beginStepId = 'begin';
  endStepId = 'end'; 
  beginStepText ='开始'; 
  endStepText ='结束' 
  
  stepTextColor = 'blue';
  stepStrokeColor = '#ece9d8';
  stepShadowColor = '#b3b3b3';
  stepFocusedStrokeColor = 'yellow';
  isStepShadow = 'T';
  actionStrokeColor = '#003366';
  actionTextColor = '';
  actionFocusedStrokeColor = 'yellow';
  sStepTextColor = 'blue';
  sStepStrokeColor = '#ece9d8';
  stepColor1 = '#ece9d8';
  stepColor2 = 'white';
  isStep3D = 'false';
  step3DDepth = '20';
  
  flowMode = '';
  startTime = '';
  endTime = '';
  ifMonitor = '';
  runMode = '';
  noteMode = '';
  activeForm = '';
  autoExe = '';

  var xml = ""; 
  xml+= '<WebFlow><FlowConfig>'
  xml+= '<BaseProperties flowId="'+flowId+'" flowText="'+flowText+'" />';
  xml+= '<VMLProperties stepTextColor="'+stepTextColor+'" stepStrokeColor="'+stepStrokeColor+'" stepShadowColor="'+stepShadowColor+'" stepFocusedStrokeColor="'+stepFocusedStrokeColor+'" isStepShadow="'+isStepShadow+'" actionStrokeColor="'+actionStrokeColor+'" actionTextColor="'+actionTextColor+'" actionFocusedStrokeColor="'+actionFocusedStrokeColor+'" sStepTextColor="'+sStepTextColor+'" sStepStrokeColor="'+sStepStrokeColor+'" stepColor1="'+stepColor1+'" stepColor2="'+stepColor2+'" isStep3D="'+isStep3D+'" step3DDepth="'+step3DDepth+'"/>';
  xml+= '<FlowProperties flowMode="'+flowMode+'" startTime="'+startTime+'" endTime="'+endTime+'" ifMonitor="'+ifMonitor+'" runMode="'+runMode+'" noteMode="'+noteMode+'" activeForm="'+activeForm+'" autoExe="'+autoExe+'" />';
  xml+= '</FlowConfig>';

	  //自动添加开始步骤
	  xml+= '<Steps><Step><BaseProperties id="'+beginStepId+'" text="'+beginStepText+'" stepid="0" stepType="BeginStep" />';
	  xml+= '<VMLProperties width="150" height="80" x="35" y="170" textWeight="" strokeWeight="" isFocused="" zIndex="" />';
	  xml+= '<FlowProperties /></Step>';
	  //自动添加结束步骤
	  xml+= '<Step><BaseProperties id="'+endStepId+'" text="'+endStepText+'" stepid="99" stepType="EndStep" />';
	  xml+= '<VMLProperties width="150" height="80" x="1300" y="170" textWeight="" strokeWeight="" isFocused="" zIndex="" />';
	  xml+= '<FlowProperties /></Step></Steps>';
	  //自动添加开始到结束的动作
	  xml+= '<Actions><Action><BaseProperties id="action" text="action0" actionType="PolyLine" from="" to="" />';
	  xml+= '<VMLProperties startArrow="" endArrow="classic" strokeWeight="" isFocused="" zIndex="" />';
	  xml+= '<FlowProperties /></Action></Actions>';
      xml+= '</WebFlow>';
	  document.form1.FlowXML.value = xml;
	  redrawVML();
}

//添加节点的函数
function getStepXML(){
  id = document.getElementById("txtNodeNodeName").value;//节点ID
  text =document.getElementById("txtNodeNodeName").value;//节点名称
  stepid=document.getElementById("txtNodeWrkStep").value;
  stepType = 'NormalStep';//添加中间节点
  if(id=='') {alert('请先填写步骤编号！\n\nPlease input Step Id!');return '';}
  if(text=='') {alert('请先填写步骤名称！\n\nPlease input Step Name!');return '';}
  
  width ='180';// 节点样子的呈现宽度
  height = '100';//节点样子的呈现高度
  x = '1300';//节点出现的x坐标
  y = '80';//节点出现的y坐标
  textWeight = '';
  strokeWeight = '';
 
  var xml = ""; 
  //生成步骤xml
  xml+= '<Step><BaseProperties id="'+id+'" text="'+text+'" stepid="'+stepid+'" stepType="'+stepType+'" />';
  xml+= '<VMLProperties width="'+width+'" height="'+height+'" x="'+x+'" y="'+y+'" textWeight="'+textWeight+'" strokeWeight="'+strokeWeight+'" zIndex="" />';
  xml+= '<FlowProperties /></Step>';

  var xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
  xmlDoc.async = false;
  xmlDoc.loadXML(document.form1.FlowXML.value);
  var xmlRoot = xmlDoc.documentElement;
  var Steps = xmlRoot.getElementsByTagName("Steps").item(0);

  //添加：查找编号冲突的Id
  //修改：查找原来的Id
  for ( var i = 0;i < Steps.childNodes.length;i++ ) {
      Step = Steps.childNodes.item(i);
	  nId = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("id");
	  
	  if(nId==id && laststepname=='') {
	    alert('新步骤编号已存在！请重新输入！\n\nThis Step Id has exist! Please input non-repeat Step Id!');return '';
	  }
  }
  
  var xmlDoc2 = new ActiveXObject('MSXML2.DOMDocument');
  xmlDoc2.async = false;
  xmlDoc2.loadXML(xml);     
  Steps.appendChild(xmlDoc2.documentElement); 
  document.form1.FlowXML.value =  xmlRoot.xml;
  redrawVML();
  getActionXML();
}

//节点间的连线
function getActionXML(){

    
  id =document.getElementById("txtNodeNodeName").value + document.getElementById("txtNodeWrkStep").value;//节点线ID
  text =document.getElementById("txtNodeNodeName").value;// 节点线名称
  actionType = 'PolyLine';
  startArrow = '';
  endArrow ='Classic';
  strokeWeight = '';
  if(stepid==1)
  {
    from = 'begin';//起始节点ID
    to = document.getElementById("txtNodeNodeName").value;
    laststeptext.push(document.getElementById("txtNodeNodeName").value);
    laststepid=stepid;
  }
    else
    {

        if(stepid == laststepid)
        {
            from = laststeptext[laststeptext.length - 2];//如果为并行节点则选择前一个节点来连接
            to = document.getElementById("txtNodeNodeName").value;
            laststeptext.push(document.getElementById("txtNodeNodeName").value);
            laststepid=stepid;
            indicate = '1';
        }
        else
        {
            from = laststeptext[laststeptext.length - 1];//getSelectValue(document.all.From);起始节点ID
            to = document.getElementById("txtNodeNodeName").value;
            laststeptext.push(document.getElementById("txtNodeNodeName").value);
            laststepid=stepid;
            if(indicate==1)
            {
                 var xml = "";
                  xml+= '<Action><BaseProperties id="'+id+'" text="'+text+'" actionType="'+actionType+'" from="'+from+'" to="'+to+'" />';
                  xml+= '<VMLProperties startArrow="'+startArrow+'" endArrow="'+endArrow+'" strokeWeight="'+strokeWeight+'" zIndex="" />';
                  xml+= '<FlowProperties /></Action>';
                  
                  var xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
                  xmlDoc.async = false;
                  xmlDoc.loadXML(document.form1.FlowXML.value);
                  var xmlRoot = xmlDoc.documentElement;
                  var Actions = xmlRoot.getElementsByTagName("Actions").item(0);

                
                  var xmlDoc2 = new ActiveXObject('MSXML2.DOMDocument');
                  xmlDoc2.async = false;
                  xmlDoc2.loadXML(xml);     
                  Actions.appendChild(xmlDoc2.documentElement); 
                  document.form1.FlowXML.value =  xmlRoot.xml;
                  redrawVML();
                  
                  from = laststeptext[laststeptext.length - 3];
                  id=id+id;
                  indicate=0;
            }
        }
    }


 
  var xml = "";
  //生成步骤xml
  xml+= '<Action><BaseProperties id="'+id+'" text="'+text+'" actionType="'+actionType+'" from="'+from+'" to="'+to+'" />';
  xml+= '<VMLProperties startArrow="'+startArrow+'" endArrow="'+endArrow+'" strokeWeight="'+strokeWeight+'" zIndex="" />';
  xml+= '<FlowProperties /></Action>';
    
  var xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
  xmlDoc.async = false;
  xmlDoc.loadXML(document.form1.FlowXML.value);
  var xmlRoot = xmlDoc.documentElement;
  var Actions = xmlRoot.getElementsByTagName("Actions").item(0);


  var xmlDoc2 = new ActiveXObject('MSXML2.DOMDocument');
  xmlDoc2.async = false;
  xmlDoc2.loadXML(xml);     
  Actions.appendChild(xmlDoc2.documentElement); 
  document.form1.FlowXML.value =  xmlRoot.xml;
  redrawVML();

}
/**********************************************************************************************************************************************************/


//保存工作流和工作流节点生成XML
function FlowStep()
{      
    var bizGrpID = document.getElementById("cmbBizGrpID").value;
    var wrkFlwName = document.getElementById("txtWrkFlwName").value;
    var voucherTypeID = document.getElementById("cmbVoucherTypeID").value;
    var initVoucher = document.getElementById("cmbInitVoucher").value;
    var efficiency = document.getElementById("txtEfficiency").value;
    var traceDays = document.getElementById("txtTraceDays").value;
    var wrkFlwStatus = document.getElementById("cmbWrkFlwStatus").value;
    var transit = document.getElementById("cmbTransit").value;
    var wrkFlwProcessClass = document.getElementById("txtWrkFlwProcessClass").value;
    var flowStep = document.getElementById("TextBox1").value;
    
    var nodeid = document.getElementById("txtNodeNodeName").value;
    var nodeNodeName = document.getElementById("txtNodeNodeName").value;
    var nodeWrkStep = document.getElementById("txtNodeWrkStep").value;
    var nodeFuncID = document.getElementById("cmbNodeFuncID").value;
    var nodeRoleID = document.getElementById("cmbNodeRoleID").value;
    var nodeSmtToMgr = document.getElementById("cmbNodeSmtToMgr").value;
    var nodeValidAfterConfirm = document.getElementById("cmbNodeValidAfterConfirm").value;
    var nodePrintAfterConfirm = document.getElementById("cmbNodePrintAfterConfirm").value;
    var nodeLongestDelay = document.getElementById("txtNodeLongestDelay").value;
    var nodeTimeoutHandler = document.getElementById("cmbNodeTimeoutHandler").value;
    var nodeProcessClass = document.getElementById("txtNodeProcessClass").value;
    
    if(flowStep!='')
    {
        var xml = ""; 
        
	  //自动添加开始步骤
	   xml+= '<Step><BaseProperties nodeID="'+nodeid+'" nodeNodeName="'+nodeNodeName+'" nodeWrkStep="'+nodeWrkStep+'" nodeFuncID="'+ nodeFuncID +'" nodeRoleID="'+nodeRoleID+'" nodeSmtToMgr="'+nodeSmtToMgr+'" nodeValidAfterConfirm="'+nodeValidAfterConfirm+'" nodePrintAfterConfirm="'+nodePrintAfterConfirm+'" nodeLongestDelay="'+nodeLongestDelay+'" nodeTimeoutHandler="'+nodeTimeoutHandler+'" nodeProcessClass="'+nodeProcessClass+'"/></Step>';

          var xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
          xmlDoc.async = false;
          xmlDoc.loadXML(document.getElementById("TextBox1").value);
          var xmlRoot = xmlDoc.documentElement;
          var Steps = xmlRoot.getElementsByTagName("Steps").item(0);
          var xmlDoc2 = new ActiveXObject('MSXML2.DOMDocument');
          xmlDoc2.async = false;
          xmlDoc2.loadXML(xml);     
          Steps.appendChild(xmlDoc2.documentElement); 
          document.getElementById("TextBox1").value =  xmlRoot.xml;
    }
    else
    {
        document.getElementById("TextBox1").value='<?xml version="1.0" encoding="GBK"?><WebFlow><FlowConfig><WrkFlw bizGrpID="'+bizGrpID+'" wrkFlwName="'+wrkFlwName+'" voucherTypeID="'+voucherTypeID+'" initVoucher="'+initVoucher+'" efficiency="'+efficiency+'" traceDays="'+traceDays+'" wrkFlwStatus="'+wrkFlwStatus+'" transit="'+transit+'" wrkFlwProcessClass="'+wrkFlwProcessClass+'"/></FlowConfig><Steps><Step><BaseProperties nodeID="'+nodeid+'" nodeNodeName="'+nodeNodeName+'" nodeWrkStep="'+nodeWrkStep+'" nodeFuncID="'+ nodeFuncID +'" nodeRoleID="'+nodeRoleID+'" nodeSmtToMgr="'+nodeSmtToMgr+'" nodeValidAfterConfirm="'+nodeValidAfterConfirm+'" nodePrintAfterConfirm="'+nodePrintAfterConfirm+'" nodeLongestDelay="'+nodeLongestDelay+'" nodeTimeoutHandler="'+nodeTimeoutHandler+'" nodeProcessClass="'+nodeProcessClass+'"/></Step></Steps></WebFlow>';
    }  
}
/************************************************************************************************************************************************************/
//右键取数据
function stepContextMenu(stepId,stepType){

  var xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
  xmlDoc.async = false;
  xmlDoc.loadXML(document.getElementById("TextBox1").value);
  var xmlRoot = xmlDoc.documentElement;
  var Steps = xmlRoot.getElementsByTagName("Steps").item(0);

  for ( var i = 0;i < Steps.childNodes.length;i++ ) {
    Step = Steps.childNodes.item(i);
	nId = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeID");
	if(nId==stepId){
	nodeid = document.getElementById("txtNodeNodeName").value = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeID");
	laststepname= document.getElementById("txtNodeNodeName").value = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeNodeName");
         document.getElementById("cmbNodeFuncID").value=Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeFuncID");
         document.getElementById("cmbNodeRoleID").value= Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeRoleID");
        document.getElementById("txtNodeNodeName").value = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeNodeName");
       document.getElementById("txtNodeWrkStep").value= Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeWrkStep");
         document.getElementById("cmbNodeSmtToMgr").valu= Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeSmtToMgr");
          document.getElementById("cmbNodeValidAfterConfirm").value= Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeValidAfterConfirm");
           document.getElementById("cmbNodePrintAfterConfirm").value = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodePrintAfterConfirm");
            document.getElementById("txtNodeLongestDelay").value = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeLongestDelay");
             document.getElementById("cmbNodeTimeoutHandler").value= Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeTimeoutHandler");
             document.getElementById("txtNodeProcessClass").value= Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeProcessClass");
	  break;
	    }
    }
}
/***********************************************************************************************************************************************************/


function UpdateWrkFlwNode_onclick() {

   if(!WrkFlwValidator(form1))
    {
        return;
    }
    var nodeNodeName = document.getElementById("txtNodeNodeName").value;
    var nodeWrkStep = document.getElementById("txtNodeWrkStep").value;
    var nodeFuncID = document.getElementById("cmbNodeFuncID").value;
    var nodeRoleID = document.getElementById("cmbNodeRoleID").value;
    var nodeSmtToMgr = document.getElementById("cmbNodeSmtToMgr").value;
    var nodeValidAfterConfirm = document.getElementById("cmbNodeValidAfterConfirm").value;
    var nodePrintAfterConfirm = document.getElementById("cmbNodePrintAfterConfirm").value;
    var nodeLongestDelay = document.getElementById("txtNodeLongestDelay").value;
    var nodeTimeoutHandler = document.getElementById("cmbNodeTimeoutHandler").value;
    var nodeProcessClass = document.getElementById("txtNodeProcessClass").value;
    
        var xml = ""; 
        
	  //自动添加开始步骤
	   xml+= '<Step><BaseProperties nodeID="'+ nodeid +'" nodeNodeName="'+nodeNodeName+'" nodeWrkStep="'+nodeWrkStep+'" nodeFuncID="'+ nodeFuncID +'" nodeRoleID="'+nodeRoleID+'" nodeSmtToMgr="'+nodeSmtToMgr+'" nodeValidAfterConfirm="'+nodeValidAfterConfirm+'" nodePrintAfterConfirm="'+nodePrintAfterConfirm+'" nodeLongestDelay="'+nodeLongestDelay+'" nodeTimeoutHandler="'+nodeTimeoutHandler+'" nodeProcessClass="'+nodeProcessClass+'"/></Step>';

          var xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
          xmlDoc.async = false;
          xmlDoc.loadXML(document.getElementById("TextBox1").value);
          var xmlRoot = xmlDoc.documentElement;
          var Steps = xmlRoot.getElementsByTagName("Steps").item(0);
          for( var i = 0;i < Steps.childNodes.length;i++ ) 
	        {
	                Step = Steps.childNodes.item(i);
	                nId = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("nodeNodeName");
	                if(nId==laststepname && laststepname!='')
	                    {
	                    Steps.removeChild(Step);
	                    laststepname='';
	                    nodeid='';
	                    break;
	                    }
	        }
          var xmlDoc2 = new ActiveXObject('MSXML2.DOMDocument');
          xmlDoc2.async = false;
          xmlDoc2.loadXML(xml);     
          Steps.appendChild(xmlDoc2.documentElement); 
          document.getElementById("TextBox1").value =  xmlRoot.xml;
          
/*修改视图*/
    var id = nId;
    var text =document.getElementById("txtNodeNodeName").value;
    var stepType="NormalStep";
      var xml = ""; 
      //生成步骤xml
      var xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
      xmlDoc.async = false;
      xmlDoc.loadXML(document.form1.FlowXML.value);
      var xmlRoot = xmlDoc.documentElement;
      var Steps = xmlRoot.getElementsByTagName("Steps").item(0);
      
      for ( var i = 0;i < Steps.childNodes.length;i++ ) {
          Step = Steps.childNodes.item(i);
	      nodeId = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("id");
    	  nodestep=Step.getElementsByTagName("BaseProperties").item(0).getAttribute("stepid");
	      if(nodeId==id && nodeId!='') {
	            xml+= '<Step><BaseProperties id="'+id+'" text="'+text+'" stepType="'+stepType+'" />';
                xml+= '<VMLProperties width="'+Step.getElementsByTagName("VMLProperties").item(0).getAttribute("width")+'" height="'+Step.getElementsByTagName("VMLProperties").item(0).getAttribute("height")+'" x="'+Step.getElementsByTagName("VMLProperties").item(0).getAttribute("x")+'" y="'+Step.getElementsByTagName("VMLProperties").item(0).getAttribute("y")+'" textWeight="" strokeWeight="" zIndex="40" />';
                xml+= '<FlowProperties /></Step>';
	        Steps.removeChild(Step);
	        break;
	      }
      }
      var xmlDoc2 = new ActiveXObject('MSXML2.DOMDocument');
      xmlDoc2.async = false;
      xmlDoc2.loadXML(xml);     
      Steps.appendChild(xmlDoc2.documentElement); 
	  document.form1.FlowXML.value = xmlRoot.xml;
	  redrawVML();
	  document.getElementById("TextBox2").value = document.form1.FlowXML.value;
 

}
function ClearText()
{
        document.getElementById("txtNodeNodeName").value='';
        document.getElementById("txtNodeWrkStep").value='';
}
function btnNodeCancel_onclick() {
        var nodeNodeName = document.getElementById("txtNodeNodeName").value;
        
   if(nodeNodeName!='')
   {
	   xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
       xmlDoc.async = false;
       xmlDoc.loadXML(document.form1.FlowXML.value);

       var xmlRoot = xmlDoc.documentElement;
	   var Steps = xmlRoot.getElementsByTagName("Steps").item(0);
       var Actions = xmlRoot.getElementsByTagName("Actions").item(0);
	   
	   var findFlag = false;
	   //循环查找Steps节点
	   for ( var i = 0;i < Steps.childNodes.length;i++ )
	   {
		   Step = Steps.childNodes.item(i);
		   id = Step.getElementsByTagName("BaseProperties").item(0).getAttribute("id");
		   if(id==nodeNodeName) 
		   {
			   Steps.removeChild(Step);
			   findFlag = true;break;
		   }
	   }
	   //循环查找Actions节点
	   for ( var i = 0;i < Actions.childNodes.length;i++ )
	   {
		   actions = Actions.childNodes.item(i);
		   id = actions.getElementsByTagName("BaseProperties").item(0).getAttribute("text");
		   if(id==nodeNodeName) 
		   {
			   Actions.removeChild(actions);
			   findFlag = true;break;
		   }
	   }
	  document.form1.FlowXML.value = xmlRoot.xml;
	  redrawVML();
   }
}

function nodeend()
{
  id ='Nodeend'//节点线ID
  text ='Nodeend';//节点线名称
  actionType = 'PolyLine';
    startArrow = '';
  endArrow ='Classic';
  strokeWeight = '';
  
    from = laststeptext[laststeptext.length - 1];//getSelectValue(document.all.From);起始节点ID
    to = 'end';
         
  var xml = "";
  //生成步骤xml
  xml+= '<Action><BaseProperties id="'+id+'" text="'+text+'" actionType="'+actionType+'" from="'+from+'" to="'+to+'" />';
  xml+= '<VMLProperties startArrow="'+startArrow+'" endArrow="'+endArrow+'" strokeWeight="'+strokeWeight+'" zIndex="40" />';
  xml+= '<FlowProperties /></Action>';
    
  var xmlDoc = new ActiveXObject('MSXML2.DOMDocument');
  xmlDoc.async = false;
  xmlDoc.loadXML(document.form1.FlowXML.value);
  var xmlRoot = xmlDoc.documentElement;
  var Actions = xmlRoot.getElementsByTagName("Actions").item(0);
  var xmlDoc2 = new ActiveXObject('MSXML2.DOMDocument');
  xmlDoc2.async = false;
  xmlDoc2.loadXML(xml);     
  Actions.appendChild(xmlDoc2.documentElement); 
  document.form1.FlowXML.value =  xmlRoot.xml;
  document.getElementById("TextBox2").value =  xmlRoot.xml;
  redrawVML();
}
function WorkFlowLoad()
{   
        document.getElementById("btnAddNode").style.visibility ="hidden";
        document.getElementById("UpdateWrkFlwNode").style.visibility = "hidden";
        document.getElementById("btnNodeCancel").style.visibility = "hidden";
    document.form1.FlowXML.value =document.getElementById("TextBox2").value;
    redrawVML();
}

//维护按钮，点击后恢复节点添加状态，隐藏本按钮，显示维护节点按钮
function btnUpdateshow() {
        document.getElementById("txtWrkFlwName").readOnly=false;
        document.getElementById("txtWrkFlwName").className="textstyle";
        document.getElementById("txtEfficiency").readOnly=false;
        document.getElementById("txtEfficiency").className="textstyle";
        document.getElementById("txtTraceDays").readOnly=false;
        document.getElementById("txtTraceDays").className="textstyle";
        document.getElementById("txtWrkFlwProcessClass").readOnly=false;
        document.getElementById("txtWrkFlwProcessClass").className="textstyle";
        document.getElementById("cmbBizGrpID").disabled = false;
        document.getElementById("cmbInitVoucher").disabled = false;
        document.getElementById("cmbWrkFlwStatus").disabled = false;
        document.getElementById("cmbTransit").disabled = false;
        document.getElementById("cmbVoucherTypeID").disabled = false;
        document.getElementById("txtNodeNodeName").readOnly=false;
        document.getElementById("txtNodeNodeName").className="textstyle";
        document.getElementById("txtNodeNodeName").value="";
        document.getElementById("txtNodeProcessClass").readOnly=false;
        document.getElementById("txtNodeProcessClass").className="textstyle";
        document.getElementById("txtNodeProcessClass").value="";
        document.getElementById("txtNodeWrkStep").readOnly=false;
        document.getElementById("txtNodeWrkStep").className="textstyle";
        document.getElementById("txtNodeWrkStep").value="";
        document.getElementById("txtNodeLongestDelay").readOnly=false;
        document.getElementById("txtNodeLongestDelay").className="textstyle";
        document.getElementById("txtNodeLongestDelay").value="";
        document.getElementById("cmbNodeRoleID").disabled=false;
        document.getElementById("cmbNodePrintAfterConfirm").disabled=false;
        document.getElementById("cmbNodeSmtToMgr").disabled=false;
        document.getElementById("cmbNodeFuncID").disabled=false;
        document.getElementById("cmbNodeValidAfterConfirm").disabled=false;
        document.getElementById("cmbNodeTimeoutHandler").disabled=false;
    
        document.getElementById("btnAddNode").style.visibility ="visible";
        document.getElementById("UpdateWrkFlwNode").style.visibility = "visible";
        document.getElementById("btnNodeCancel").style.visibility = "visible";
        document.getElementById("btnUpdate").style.visibility  = "hidden";
        
        document.getElementById("TextBox1").value = "";
        document.getElementById("TextBox2").value = "";
        document.form1.FlowXML.value="";
        getFlowXML();
        return false;
}
//添加新节点
function btnAddNode_onclick() {
    if(!WrkFlwValidator(form1))
    {
        return;
    }
     FlowStep();
    getStepXML();
    document.getElementById("TextBox2").value = document.form1.FlowXML.value;
    wrkFlwLock();
}

function BtnUp( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/btn_' + l + '.gif)';
}
function BtnOver( p )
{
	var t = String(p)
	var l = t.substring(3,15); 
	document.getElementById( p ).style.backgroundImage = 'url(../App_Themes/CSS/BtnImage/over_' + l + '.gif)';
}

    -->
        </script>
    <style type="text/css">
        v\:* { Behavior: url(#default#VML) }
    </style>
</head>
<body onload='WorkFlowLoad();' topmargin=0 leftmargin=0>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                    <table id="TABLE2" border="0" cellpadding="0" cellspacing="0" style="width: 723px;
                        height: 413px">
                        <tr>
                            <td class="tdTopBackColor" style="width: 685px; height: 22px" valign="top">
                                <img alt="" class="imageLeftBack" style="width: 5px; height: 22px; text-align: left" />
                                <asp:Label ID="lblCreateUserTitle" runat="server" CssClass="lblTitle" Text="<%$ Resources:BaseInfo,WrkFlw_lblWorkFlow %>"></asp:Label></td>
                            <td class="tdTopRightBackColor" style="vertical-align: top; width: 40px; height: 22px;
                                text-align: right" valign="top">
                                <img class="imageRightBack" style="height: 22px" /></td>
                        </tr>
                        <tr>
                            <td class="tdBackColor" colspan="2" style="width: 40px; height: 281px; text-align: left"
                                valign="top">
                                <table id="TABLE3" border="0" cellpadding="0" cellspacing="0" style="width: 723px;
                                    height: 320px">
                                    <tr>
                                        <td colspan="12" style="width: 723px; height: 1px; background-color: white">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="12" style="vertical-align: top; width: 723px; height: 2px;
                                            text-align: center">
                                            <table id="TABLE1" cellpadding="0" cellspacing="0" class="panel_style" style="width: 717px;
                                                height: 133px">
                                                <tr>
                                                    <td align="left" colspan="2" onclick="cleancontextMenu();return false;" oncontextmenu="stepContextMenu();return false;"
                                                        style="width: 795px; height: 133px" valign="top">
                            <?xml namespace="" ns="urn:schemas-microsoft-com:vml"
                                        prefix="v" ?><?xml namespace="" ns="urn:schemas-microsoft-com:vml" prefix="v" ?><?xml namespace="" ns="urn:schemas-microsoft-com:vml" prefix="v" ?>
                                                        <v:group id="FlowVML" coordsize="1500,450" style="left: 34px; width: 700px; position: absolute;
                                                            top: 40px; height: 130px">
                                                        </v:group>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="12" style="vertical-align: middle; width: 723px; height: 10px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="left: 12px; width: 701px;
                                                position: relative">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 99px; height: 23px; text-align: right">
                                            <asp:Label ID="lblBizGrpID" runat="server" CssClass="labelStyle" ForeColor="Black"
                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblBizGrpID %>" Width="56px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 4px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 139px; height: 23px">
                                            <asp:DropDownList ID="cmbBizGrpID" runat="server" BackColor="White" CssClass="cmb120px" Enabled="False">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 86px; height: 23px; text-align: right">
                                            <asp:Label ID="lblWrkFlwName" runat="server" CssClass="labelStyle" ForeColor="Black"
                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwName %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 140px; height: 23px">
                                            <asp:TextBox ID="txtWrkFlwName" runat="server" CssClass="Enabletextstyle" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 84px; height: 23px; text-align: right">
                                            <asp:Label ID="lblVoucherTypeID" runat="server" CssClass="labelStyle" ForeColor="Black"
                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblVoucherTypeID %>" Width="72px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 142px; height: 23px">
                                            <asp:DropDownList ID="cmbVoucherTypeID" runat="server" CssClass="cmb120px" Enabled="False">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 99px; height: 23px; text-align: right">
                                            <asp:Label ID="lblInitVoucher" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblInitVoucher %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 4px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 139px; height: 23px">
                                            <asp:DropDownList ID="cmbInitVoucher" runat="server" BackColor="White" CssClass="cmb120px" Enabled="False">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 86px; height: 23px; text-align: right">
                                            <asp:Label ID="lblEfficiency" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblEfficiency %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 140px; height: 23px">
                                            <asp:TextBox ID="txtEfficiency" runat="server" CssClass="Enabletextstyle" MaxLength="18"></asp:TextBox></td>
                                        <td class="tdBackColor" style="height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 84px; height: 23px; text-align: right">
                                            <asp:Label ID="lblTraceDays" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblTraceDays %>"
                                                Width="87px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 142px; height: 23px">
                                            <asp:TextBox ID="txtTraceDays" runat="server" CssClass="Enabletextstyle" MaxLength="18"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 99px; height: 23px; text-align: right">
                                            <asp:Label ID="lblWrkFlwStatus" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwStatus %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 4px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 139px; height: 23px">
                                            <asp:DropDownList ID="cmbWrkFlwStatus" runat="server" BackColor="White" CssClass="cmb120px" Enabled="False">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 86px; height: 23px; text-align: right">
                                            <asp:Label ID="lblTransit" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblTransit %>"
                                                Width="82px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 140px; height: 23px">
                                            <asp:DropDownList ID="cmbTransit" runat="server" BackColor="White" CssClass="cmb120px" Enabled="False">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 84px; height: 23px; text-align: right">
                                            <asp:Label ID="lblWrkFlwProcessClass" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblWrkFlwProcessClass %>"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 142px; height: 23px">
                                            <asp:TextBox ID="txtWrkFlwProcessClass" runat="server" CssClass="Enabletextstyle" MaxLength="128"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="12" style="vertical-align: middle; width: 723px; height: 10px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="left: 12px; width: 701px;
                                                position: relative">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 99px; height: 25px; text-align: right">
                                            <asp:Label ID="lblNodeNodetName" runat="server" CssClass="labelStyle" ForeColor="Black"
                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeNodetName %>" Width="69px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 4px; height: 25px">
                                        </td>
                                        <td class="tdBackColor" style="width: 139px; height: 25px">
                                            <asp:TextBox ID="txtNodeNodeName" runat="server" CssClass="Enabletextstyle" MaxLength="32"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 86px; height: 25px; text-align: right">
                                            <asp:Label ID="lblNodeWrkStep" runat="server" CssClass="labelStyle" ForeColor="Black"
                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeWrkStep %>" Width="73px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 25px">
                                        </td>
                                        <td class="tdBackColor" style="width: 140px; height: 25px">
                                            <asp:TextBox ID="txtNodeWrkStep" runat="server" CssClass="Enabletextstyle" MaxLength="18"></asp:TextBox></td>
                                        <td class="tdBackColor" style="height: 25px">
                                        </td>
                                        <td class="tdBackColor" style="width: 84px; height: 25px; text-align: right">
                                            <asp:Label ID="lblNodeFuncAuthID" runat="server" CssClass="labelStyle" ForeColor="Black"
                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeFuncAuthID %>" Width="69px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 25px">
                                        </td>
                                        <td class="tdBackColor" style="width: 142px; height: 25px">
                                            <asp:DropDownList ID="cmbNodeFuncID" runat="server" BackColor="White" CssClass="cmb120px">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 5px; height: 25px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 99px; height: 23px; text-align: right">
                                            <asp:Label ID="lblNodeRoleID" runat="server" CssClass="labelStyle" ForeColor="Black"
                                                Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeRoleID %>" Width="69px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 4px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 139px; height: 23px">
                                            <asp:DropDownList ID="cmbNodeRoleID" runat="server" BackColor="White" CssClass="cmb120px">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 86px; height: 23px; text-align: right">
                                            <asp:Label ID="lblNodeSmtToMgr" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeSmtToMgr %>"
                                                Width="86px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 140px; height: 23px">
                                            <asp:DropDownList ID="cmbNodeSmtToMgr" runat="server" BackColor="White" CssClass="cmb120px">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 84px; height: 23px; text-align: right">
                                            <asp:Label ID="lblNodeValidAfterConfirm" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeValidAfterConfirm %>"
                                                Width="85px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 142px; height: 23px">
                                            <asp:DropDownList ID="cmbNodeValidAfterConfirm" runat="server" BackColor="White"
                                                CssClass="cmb120px">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 99px; height: 23px; text-align: right">
                                            <asp:Label ID="lblNodePrintAfterConfirm" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodePrintAfterConfirm %>"
                                                Width="85px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 4px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 139px; height: 23px">
                                            <asp:DropDownList ID="cmbNodePrintAfterConfirm" runat="server" BackColor="White"
                                                CssClass="cmb120px">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 86px; height: 23px; text-align: right">
                                            <asp:Label ID="lblNodeLongestDelay" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeLongestDelay %>"
                                                Width="87px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 140px; height: 23px">
                                            <asp:TextBox ID="txtNodeLongestDelay" runat="server" CssClass="Enabletextstyle" MaxLength="18"></asp:TextBox></td>
                                        <td class="tdBackColor" style="height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 84px; height: 23px; text-align: right">
                                            <asp:Label ID="lblNodeTimeoutHandler" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeTimeoutHandler %>"
                                                Width="86px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                        <td class="tdBackColor" style="width: 142px; height: 23px">
                                            <asp:DropDownList ID="cmbNodeTimeoutHandler" runat="server" BackColor="White" CssClass="cmb120px">
                                            </asp:DropDownList></td>
                                        <td class="tdBackColor" style="width: 5px; height: 23px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" style="width: 99px; height: 25px; text-align: right">
                                            <asp:Label ID="lblNodeProcessClass" runat="server" CssClass="labelStyle" Text="<%$ Resources:BaseInfo,WrkFlw_lblNodeProcessClass %>"
                                                Width="64px"></asp:Label></td>
                                        <td class="tdBackColor" style="width: 4px; height: 25px">
                                        </td>
                                        <td class="tdBackColor" style="width: 139px; height: 25px">
                                            <asp:TextBox ID="txtNodeProcessClass" runat="server" CssClass="Enabletextstyle" MaxLength="128"></asp:TextBox></td>
                                        <td class="tdBackColor" style="width: 86px; height: 25px">
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 25px">
                                        </td>
                                        <td class="tdBackColor" colspan="5" style="vertical-align: middle; height: 25px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="left: 99px; width: 260px;
                                                position: relative; top: 14px">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 25px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdBackColor" colspan="4" style="vertical-align: bottom;
                                            top: 5px; height: 33px; text-align: right">
                                            <table style="width: 298px; height: 28px">
                                            <tr>
                                            <td colspan="2" style="vertical-align: bottom; height: 5px; text-align: right;">
                                            <table border="0" cellpadding="0" cellspacing="0" style="left: 12px; width: 286px;
                                                position: relative; vertical-align: top;">
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #738495">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 160px; height: 1px; background-color: #ffffff">
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            </td>
                                            </tr>
                                                <tr>
                                                    <td style="width: 3px; height: 26px; vertical-align: bottom;">
                                            <asp:Button ID="btnEdit" runat="server" CssClass="buttonEdit"
                                                Text="<%$ Resources:BaseInfo,WrkFlw_Maintenance %>" OnClick="btnUpdate_Click" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/></td>
                                                    <td style="height: 40px; vertical-align: bottom;">
                                                        <input id="btnAddNode" class="buttonEdit" onclick="return btnAddNode_onclick()"
                                                            type="button" OnMouseOver="this.style.background='url(../App_Themes/CSS/BtnImage/BtnEditing.gif) no-repeat left top';this.style.fontWeight='bold';" OnMouseOut="this.style.background='url(../App_Themes/CSS/BtnImage/BtnEdit.gif) no-repeat left top';this.style.fontWeight='normal';" />&nbsp;
                                                            <input id="UpdateWrkFlwNode"
                                                                class="buttonEdit" onclick="return UpdateWrkFlwNode_onclick()"
                                                                type="button" OnMouseOver="this.style.background='url(../App_Themes/CSS/BtnImage/BtnEditing.gif) no-repeat left top';this.style.fontWeight='bold';" OnMouseOut="this.style.background='url(../App_Themes/CSS/BtnImage/BtnEdit.gif) no-repeat left top';this.style.fontWeight='normal';" />
                                                                <input id="btnNodeCancel"
                                                                    class="buttonClear" onclick="return btnNodeCancel_onclick()"
                                                                    type="button" OnMouseOver="this.style.background='url(../App_Themes/CSS/BtnImage/BtnBlankOuting.gif) no-repeat left top';this.style.fontWeight='bold';" OnMouseOut="this.style.background='url(../App_Themes/CSS/BtnImage/BtnBlankOut.gif) no-repeat left top';this.style.fontWeight='normal';"/></td>
                                                </tr>
                                            </table>
                                            </td>
                                        <td class="tdBackColor" style="width: 5px; height: 33px">
                                        </td>
                                        <td class="tdBackColor" colspan="5" style="vertical-align: text-bottom; position: relative; height: 33px; text-align: right; left: -8px;">
                                            <asp:Button ID="btnSave" runat="server" CssClass="buttonSave" Enabled="False" OnClick="Button7_Click"
                                                Text="<%$ Resources:BaseInfo,PotCustomer_butSave %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                            <asp:Button ID="btnBlankOut" runat="server" CssClass="buttonClear" OnClick="btnDelete_Click"
                                                Text="<%$ Resources:BaseInfo,Btn_Del %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>
                                            <asp:Button ID="btnCancel" runat="server" CssClass="buttonCancel"
                                                        OnClick="btnCancel_Click" Text="<%$ Resources:BaseInfo,WrkFlw_btnCancel %>" onmouseover="BtnOver(this.id);" onmouseout="BtnUp(this.id);" onmouseup="BtnUp(this.id);"/>&nbsp;&nbsp;&nbsp;
                                            &nbsp;
                                        </td>
                                        <td class="tdBackColor" style="width: 5px; height: 33px">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
            </ContentTemplate>
        </asp:UpdatePanel>
                                            <input name="FlowXML" style="width: 350px" type="hidden" class="hidden" /><br />
        <asp:TextBox ID="TextBox1"
                                                runat="server" Width="361px" CssClass="hidden"></asp:TextBox><br />
        <asp:TextBox ID="TextBox2"
                                                    runat="server" Width="376px" CssClass="hidden"></asp:TextBox>
                                                            <asp:HiddenField ID="hidAdd" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidAdd %>" />
        <asp:HiddenField ID="hidInsert" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidInsert %>" />
        <asp:HiddenField ID="hidMessage" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidMessage %>" />
        <asp:HiddenField ID="hidIntError" runat="server" Value="<%$ Resources:BaseInfo,Hidden_IntError %>" />
        <asp:HiddenField ID="hidDelete" runat="server" Value="<%$ Resources:BaseInfo,Hidden_hidDelete %>" />
        <asp:HiddenField ID="hidbtnAdd" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_NewNode %>" /> 
        <asp:HiddenField ID="hidbtnUpdate" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_NodeChange %>" />
        <asp:HiddenField ID="hidbtnDelete" runat="server" Value="<%$ Resources:BaseInfo,WrkFlw_NodeDelete %>" />
    </form>
            
</body>
</html>



