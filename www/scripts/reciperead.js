var userUID;
auth.onAuthStateChanged(user =>{
  if (user) {
    userUID=user.uid;
    document.body.style.display="block";
    console.log(user);
    Main(userUID);
  } else {
    document.body.style.display="none";
    window.location.href="index.htm";
  }
});
function Main() {
  let href = window.location.href;
  let url = new URL(href);
  let hash = url.hash;
  if (hash.includes("#")==true) {
    hash= hash.replace("#","");
  }
  if (hash=="") {
    return false;
  }
  rt.ref("Recipe/" + hash).get().then(snapshot=>{
    document.querySelector(".container__container-title").innerHTML=snapshot.val().name;
    document.querySelector(".container__container-cal").innerHTML+=snapshot.val().cal;
    document.querySelector(".container__container-products").innerHTML+=snapshot.val().products;
    document.querySelector(".container__container-recipe").innerHTML=snapshot.val().recipe;

  });
}
