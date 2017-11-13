//Usage

//load your JSON (you could jQuery if you prefer)
function loadJSON(callback) {

  var xobj = new XMLHttpRequest();
  xobj.overrideMimeType("application/json");
  xobj.open('GET', 'https://localhost:44335/api/json', true); 
    // in case of AJAX call --> url : urls.PlayWheel
  //xobj.open('GET', 'WheelGame/GetWheelPlay', true); //ok..
  //xobj.send();
  xobj.onreadystatechange = function() {
    if (xobj.readyState == 4 && xobj.status == "200") {
      //Call the anonymous function (callback) passing in the response
      callback(xobj.responseText);
    }
  };
  xobj.send(null);
}

//your own function to capture the spin results
function myResult(e) {
  //e is the result object
    console.log('Spin Count: ' + e.spinCount + ' - ' + 'Win: ' + e.win + ' - ' + 'Message: ' +  e.msg);

    // if you have defined a userData object...
    if(e.userData){
      
      console.log('User defined score: ' + e.userData.score)
      var http = new XMLHttpRequest();
      var url = 'Spin';

      //var userdata = { score: '3.5' };
      //var params = "{userdata:" + JSON.stringify(userdata) + "}";
      //var userdata = e;
      var amount = $('#BetAmount').val();
      var result = { spinResult: e, betAmount: amount };
      var params = JSON.stringify(result);
      http.open('POST', url, true); //true
      http.setRequestHeader("content-type", "application/json;charset=utf-8");
      http.onreadystatechange = function () {
          if (http.readyState == 4 && http.status == 200) {
              //var rt = http.responseText;
              //alert(http.responseText);
              //$("#dvCategoryResults").load('@(Url.Action("GetUpdatedBalance","WheelGame",null, Request.Url.Scheme))');
              var newContent = $.parseHTML(http.responseText);
              //var newForm = newContent.find('form');
              for (var i= 0; i < newContent.length;++i)
              {
                  if (newContent[i].id == "logoutForm")
                  {
                      $('#logoutForm').html(newContent[i].innerHTML);
                      //$('#logoutForm').html();
                  }
              }
              //$('#logoutForm').html(http.responseText);
              //document.getElementById("logoutForm").innerHTML =
          }
      }
      http.send(params);
    }

  //if(e.spinCount == 3){
    //show the game progress when the spinCount is 3
    //console.log(e.target.getGameProgress());
    //restart it if you like
    //e.target.restart();
  //}  

}

//your own function to capture any errors
function myError(e) {
  //e is error object
  console.log('Spin Count: ' + e.spinCount + ' - ' + 'Message: ' +  e.msg);

}

function myGameEnd(e) {
  //e is gameResultsArray
  console.log(e);
  TweenMax.delayedCall(5, function(){
    /*location.reload();*/
  })


}

function init() {
  loadJSON(function(response) {
    // Parse JSON string to an object
    var jsonData = JSON.parse(response);
    //if you want to spin it using your own button, then create a reference and pass it in as spinTrigger
    var mySpinBtn = document.querySelector('.spinBtn');
    //create a new instance of Spin2Win Wheel and pass in the vars object
    var myWheel = new Spin2WinWheel();
    
    //WITH your own button
    myWheel.init({data:jsonData, onResult:myResult, onGameEnd:myGameEnd, onError:myError, spinTrigger:mySpinBtn});
    
    //WITHOUT your own button
    //myWheel.init({data:jsonData, onResult:myResult, onGameEnd:myGameEnd, onError:myError);
  });
}



//And finally call it
init();
