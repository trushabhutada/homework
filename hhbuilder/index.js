
var body = document.getElementsByTag("body")[0];
var tbl = document.CreateElement("table");
tbl.setAttribute("id", "table1");
body.appendChild(tbl);


function onAddClick()
{
  var age = document.getElementsByName("age")[0].value;
  var relation = document.getElementsByName("rel")[0].value;
  var smoker = document.getElementsByName("smoker")[0].checked ? "Yes" : "No";
  if(age == undefined || age < 0 )
  {
    alert("Age cannot be less than 0 !!");
    return;
  }
  if(relation == undefined)
  {
     alert("Need Relationship !!");
     return;
  }
  
 var tbl = document.getElementById("table"); 
 var table_len=(tbl.rows.length)-1;
 var row = tbl.insertRow(table_len).outerHTML="<tr id='row"+table_len+"'><td id='age_row"+table_len+"'>"+age+"</td><td id='relation_row"+table_len+"'>"+relation+"</td><td id='smoker_row"+table_len+"'>"+smoker+"</td><td><input type='button' id='edit_button"+table_len+"' value='Edit' onclick='edit_row("+table_len+")'> <input type='button' id='save_button"+table_len+"' value='Save' onclick='save_row("+table_len+")'> <input type='button' value='Delete' onclick='delete_row("+table_len+")'></td></tr>";
  
 document.getElementsByName("age")[0].value="";
 document.getElementsByName("rel")[0].Selected=false;
 document.getElementsByName("smoker")[0].checked =false;
}

function edit_row(no)
{
  document.getElementById("edit_button"+no).style.display="none";
  document.getElementById("save_button"+no).style.display="block";
  
  var age=document.getElementById("age_row"+no);
  var relation=document.getElementById("relation_row"+no);
  var smoker=document.getElementById("smoker_row"+no);
 	
  var age_data=age.innerHTML;
  var relation_data=relation.innerHTML;
  var smoker_data=smoker.innerHTML;
 	
  age.innerHTML="<input type='text' id='age_text"+no+"' value='"+age_data+"'>";
  relation.innerHTML="<input type='text' id='relation_text"+no+"' value='"+relation_data+"'>";
  smoker.innerHTML="<input type='text' id='smoker_text"+no+"' value='"+smoker_data+"'>";  
}

function save_row(no)
{
  var age=document.getElementById("age_row"+no);
  var relation=document.getElementById("relation_row"+no);
  var smoker=document.getElementById("smoker_row"+no);
	
  document.getElementById("age_row"+no).innerHTML=age;
  document.getElementById("relation_row"+no).innerHTML=relation;
  document.getElementById("smoker_row"+no).innerHTML=smoker; 
 
  document.getElementById("edit_button"+no).style.display="block";
  document.getElementById("save_button"+no).style.display="none";
}

function delete_row(no)
{
  document.getElementById("row"+no+"").outerHTML="";
}

function submit()
{
  var tbl = document.getElementById("table");
  var data = [];
  // go through cells 
  for (var i=1; i<tbl.rows.length; i++) 
  { 
    var tableRow = tbl.rows[i]; 
    var rowData = {}; 
    for (var j=0; j<tableRow.cells.length; j++) 
    { 
      rowData[j] = tableRow.cells[j].innerHTML; 
    }
    data.push(rowData); 
  } 
  var myJSON = JSON.stringify(data);
  document.getElementsByClass("debug")[0].innerHTML = myJSON;
  this.submit();
}
