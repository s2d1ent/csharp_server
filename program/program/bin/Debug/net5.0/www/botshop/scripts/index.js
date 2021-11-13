var doc=document,
    wiw=window;
wiw.addEventListener("scroll",(event)=>{
  let scroll = wiw.scrollY;
  if (scroll>=500) {
    $(".header").css("position","fixed");
  } else {
    $(".header").css("position","");
  }
})
$("document").ready(()=>{
    regAuthForm();
})
function regAuthForm() {
  for (var i=1;i<32;i++) {
    let option = doc.createElement("option");
    option.setAttribute("value",i);
    option.innerHTML=i;
    $(".day").append(option);
//    console.log(option)
  }
  for (var i=1;i<13;i++) {
    let option = doc.createElement("option");
    option.setAttribute("value",i);
    option.innerHTML=i;
    $(".month").append(option);
//    console.log(option)
  }
  for (var i=1930;i<2022;i++) {
    let option = doc.createElement("option");
    option.setAttribute("value",i);
    option.innerHTML=i;
    $(".year").append(option);
//    console.log(option)
  }
}

// ВАЛИДАЦИЯ
$(".input[name=password_try]").bind("input",()=>{
  let pass =$(".input[name=password]").val(),
      passTry= $(".input[name=password_try]").val();
  if (passTry!=pass) {
    $(".regMess").css("display","flex");
    doc.getElementById("error_description").innerHTML="пароли не совпадают";
  } else {
    $(".regMess").css("display","none");
  }
})

$("#auth").bind("click",()=>{
    $(".authForm").css("display","flex");
    $(".registerForm").css("display","none");
});
$("#reg").bind("click",()=>{
    $(".authForm").css("display","none");
    $(".registerForm").css("display","flex");
});
$(".browser").bind("click",()=>{
  $("#auth").click();
});
function calc () {
let rad=$(".labelRad"),
    radArr = Array.from(rad);
var lastI,
    newR,
    valTarget;
function newChange(th) {
  th.style.fontSize="14px";
  th.style.fontWeight="500";
  th.style.color="#56CCF2";
}
function lastChange(th) {
  rad[lastI].style.fontSize="12px";
  rad[lastI].style.fontWeight="400";
  rad[lastI].style.color="black";
}
function radMessage(F) {
//  console.log(F);
  let message = doc.querySelector(".calcMes");
  switch (F) {
    case "1.2": message.innerHTML="Физ.нагрузка отсутствует или минимальная";
      break;
    case "1.38":message.innerHTML="Умеренная активность 3 раза в неделю";
        break;
    case "1.46":message.innerHTML="Тренировки средней активности 5 раз в неделю";
          break;
    case "1.55":message.innerHTML="Интенсивные тренировки 5 раз в неделю";
          break;
    case "1.64":message.innerHTML="Каждодневные тренировки";
          break;
     case "1.73":message.innerHTML="Интенсивные тренировки каждый день";
          break;
     case "1.9":message.innerHTML="Ежедневная физ.нагрузка + физическая работа";
          break;
  }
}
rad.bind("click",(event)=>{
      let target = event.target,
          targetJQ = $(event.target),
          index =radArr.indexOf(target),
          radio = rad[index];
      newChange(radio);
      valTarget =targetJQ.attr("value");
      radMessage(valTarget);
      if (newR==null || newR==undefined) {
        lastI=index;
      }
      newR=index;
      if (lastI!=newR) {
        lastChange(rad[lastI]);
        lastI=newR;
      }
});
$(".btn-test").bind("click",()=>{
  let weight=parseInt($("#weightF").val()),
      height=parseInt($("#heightF").val()),
      old=parseInt($("#oldF").val()),
      genderF=parseInt($("#genderf").val());
      valTarget=parseFloat(valTarget);
  let gender;
  switch (genderF) {
    case 1:gender=5; break;
    case 0:gender=-161; break;
  }
  let dci = weight*10 + height*6.25-old*5+gender;
      dci = dci *valTarget
  $("#normalCalc").val(dci);
})
}
calc();
