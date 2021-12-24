<?php
namespace shop;
class DataBase
{
	private $host = "localhost";
	private $username = "root";
	private $password = "root";
	private $database = "botshop";

	public function __construct(){}
	public function Connect ($table)
	{
		$connection=mysqli_connect(this.$host,this.$username,this.$password);
		$select=mysqli_select_db($connection,$table);
		return $connection;
	}
	public function Close ()
	{
		mysqli_close($connection);
	}
	public function Query ($query, $connection)
	{
		return $result=mysqli_query($connection,$query);
	}
	public function Select($field, $table, $connection)
	{
		$query = "SELECT `$field` FROM `$table`";
		return $result=mysqli_query($connection,$query);
	}
	public function SelectWhere($field, $table, $where, $connection)
	{
		$query = "SELECT `$field` FROM `$table` WHERE `$where` ";
		return $result=mysqli_query($connection,$query);
	}
	public function Insert($field, $values, $connection)
	{
		$query = "INSERT INTO `$field` VALUES `$values`";
		return $result=mysqli_query($connection,$query);
	}
	public function Update($field, $values, $where, $table, $connection)
	{
		$query = "UPDATE `$table` SET `$field` VALUES `$values` WHERE `$where` ";
		return $result=mysqli_query($connection,$query);
	}
	public function Delete ($where, $table, $connection)
	{
		$query = "DELETE FROM `$table` WHERE `$where` ";
		return $result=mysqli_query($connection,$query);
	}
	public function Truncat($table, $connection)
	{
		$query = "TRUNCATE TABLE `$table` ";
		return $result=mysqli_query($connection,$query);
	}
	public function OrderBy($result){}
	public function OrderByDesc($result){}
}

?>