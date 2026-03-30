// JavaScript Document
	var Obj;
	var leftTd = document.getElementById("leftPart");
	var tabArrow = document.getElementById("myTabArrow");

	//BOF resize the bench by dragging
	function MouseDown(obj) { 
		Obj=obj; 
		Obj.setCapture(); 
		Obj.l=event.x-Obj.style.pixelLeft; 
		Obj.t=event.y-Obj.style.pixelTop; 
	} 

	function MouseMove() { 
		if(Obj!=null) { 
			Obj.style.left = event.x-Obj.l; 
			Obj.style.top = event.y-Obj.t;
			leftTd.style.width = event.x;
			
			//last 50px, the tabBar attracted by the left side of the window. 
			if (parseInt(document.getElementById("leftPart").style.width) <= 50) {
				leftTd.style.width = '0';
				leftTd.style.display = 'none';
				tabArrow.src = 'App_Themes/Main/Images/btnTabRight.gif';
					
				if(Obj!=null) { 
					Obj.releaseCapture(); 
					Obj=null; 
				}
			}
		} 
	} 
	
	function MouseUp() { 
		if(Obj!=null) { 
			Obj.releaseCapture(); 
			Obj=null; 
		} 
	}
	//EOF resize the bench by dragging
	
	//BOF one click to open or shut the bench
	function shutAndOpenLeftTab() {
		leftWidthNum = parseInt(leftTd.style.width);

		if (leftWidthNum > 0) {
			leftTd.style.width = '0';
			leftTd.style.display = 'none';
			tabArrow.src = 'App_Themes/Main/Images/btnTabRight.gif';
		} else {
			leftTd.style.width = '200';
			leftTd.style.display = 'block';
			tabArrow.src = 'App_Themes/Main/Images/btnTabLeft.gif';
		}
	}
	//EOF one click to open or shut the bench