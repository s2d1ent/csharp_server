const doc = document,
      win = window;
var cardOpen = new Array();
$(document).ready(
  ()=>{
    $(".card").on("click",Card);
  }
);
function Card(event){
  let cardsCollection = $(".card"),
      cardsArray = Array.from(cardsCollection),
      index = cardsArray.indexOf(event.currentTarget);
  if (cardOpen.length == 0 )
      for(var i = 0; i < cardsArray.Length; i++){
        cardOpen[i]=0;
        console.log(cardsArray[i] + " - " + cardOpen[i])
      }

  $(cardsArray[index]).css("height","auto")
                      .css("padding-bottom","35px");
  let ico = $(cardsArray[index]).children(".card-header").children()[1];
  $($(ico).children()[0]).css("transform: rotate","180deg");
}
