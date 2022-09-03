<?php 
    $array = [];

    for($i = 0; $i < 5; $i++)
    {
        if($i == 0)
        {
            $array[$i] = rand(2, 10);
            echo "array[$i]=$array[$i]<br/>";
        }
        else
        {
            $num = rand(2, 10);
            $exp = $array[$i-1];
            $array[$i] = pow($num, $exp);
            echo "array[$i]=$num<br/>";
        }
    }

    print_r($array);
?>