var doc=document,
	  wiw=window,
 	  viewPortH = wiw.innerHeight,
 	  viewPortW = wiw.innerWidth;
var burgerCount=0,
	  burgerCountI=0;
//Высота относительно viewport(видимой области)
function content() {
	if (matchMedia("(max-width:1920px)").matches && burgerCountI==0)
		$(".view__right_container-top").css("width","90%");
	else if (matchMedia("(max-width:1920px)").matches && burgerCountI==1)
		$(".view__right_container-top").css("width","100%");

}
// выдвижной сайдбар
$(".left__burger").bind("click",()=>{
	burgerCountI=burgerCount%2;
	burgerCount++;
	if (burgerCountI==0) {
		$(".view__left-container").css("width","200px");
		content();
		setTimeout(()=>{left__profileInfoOpen()},700);
	} else if (burgerCountI==1) {
		$(".view__left-container").css("width","70px");
		content();
		left__profileInfoClose()
	}
});
// sidebar(left menu) || left__profileInfo
function left__profileInfoOpen() {
	$(".left__profileInfo-text").css("display","flex");
	$(".left__points-title").css("display","block");
}
function left__profileInfoClose() {
	$(".left__profileInfo-text").css("display","none");
	$(".left__points-title ").css("display","none");
}
