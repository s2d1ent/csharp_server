<?php 
namespace shop;
class User
{
	public $UID;
	public $Email;
	public $Name;
	public $Token;
	//private $db = new \shop\DataBase();

	public function __construct()
	{
		if($_COOKIE['token'].length != 0)
		{
			this.$Tonen = $_COOKIE['token'];
		}
	}
}

?>