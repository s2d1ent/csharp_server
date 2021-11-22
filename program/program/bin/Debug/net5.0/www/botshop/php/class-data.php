<?php

class Data
{
	public $data = [];
	public function Add($key, $data)
	{
		this.$data[$key] = $data;
	}
	public function RemoveAt ($key)
	{
		this.$data[$key] = null;
	}
	public function Ramove ()
	{
		this.$data = [];
	}
	public function Change($key, $data)
	{
			this.$data[$key] = $data;
	}
}

?>