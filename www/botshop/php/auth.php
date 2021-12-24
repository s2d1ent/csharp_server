<?php

include_once "class-database.php";
include_once "class-data.php";
include_once "class-user.php";

$Email = $_POST["email"];
$Password = $_POST["pswrd"];

date_default_timezone_set('UTC');
$date = date("d.m.y g:i");

// foreach($_SERVER as $key => $value)
// 	echo "`$key` - `$value`";

if ($_SERVER['REMOTE_ADDR'] != "")
	$UIP = $_SERVER['REMOTE_ADDR'];
else if ($_SERVER['HTTP_CLIENT_IP'] != "")
	$UIP = $_SERVER['HTTP_CLIENT_IP'];
$UB = $_SERVER['HTTP_USER_AGENT'];
settype($UIP, "string");
settype($UB, "string");
settype($date, "string");
$token = base64_encode($date + $UIP + $UB);
?>