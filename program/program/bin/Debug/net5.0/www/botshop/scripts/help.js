const doc = document,
      win = window;
var cardOpen = new Array();
$(document).ready(
  ()=>{
    let cardsCol = $(".card");
    let cardArr = Array.from(cardsCol);
    for(var i = 0; i < cardArr.length; i++)
      cardOpen[i]=false;
    $(".card").on("click",Card);
  }
);
function Card(event){
  let cardsCollection = $(".card"),
      cardsArray = Array.from(cardsCollection),
      index = cardsArray.indexOf(event.currentTarget);
  if(!cardOpen[index]){
    cardOpen[index] = !cardOpen[index];
    $(cardsArray[index]).css("height","auto")
                        .css("padding-bottom","35px");
    let ico = $(cardsArray[index]).children(".card-header").children()[1];
  }
  else {
    cardOpen[index] = !cardOpen[index];
    $(cardsArray[index]).css("height","60px")
                        .css("padding-bottom","0px");
    let ico = $(cardsArray[index]).children(".card-header").children()[1];
  }
}
