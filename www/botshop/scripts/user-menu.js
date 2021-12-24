const doc = document,
		win = window;
const userButton = doc.querySelector(".menu-user"),
		userMenu = doc.querySelector(".user_menu"),
		userButtonProfile = doc.querySelector("#profile"),
		userButtonSettings = doc.querySelector("#settings"),
		userButtonExit = doc.querySelector("#exit");
var EventData =
{
	menuOpen : false
};
Main();
function Main () 
{
	userButton.addEventListener("click",MenuOpen);
}
function MenuOpen() 
{
	let open = EventData.menuOpen;
	EventData.menuOpen = !EventData.menuOpen;
	console.log(open);
	if(!open)
		$(userMenu).css("display","block");
	else
		$(userMenu).css("display","none");
	console.log(open);
}

