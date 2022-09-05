<?php //dbconn_inc.php
//Connect to db using address, user, password and the db name

//$hostaddress="localhost";$dbuser="root";$password="";$dbname="db_maritime_challenge_alpaca";
$hostaddress="localhost";$dbuser="root";$password="A1b2C3d4!";$dbname="db_maritime_challenge_alpaca";
$conn=new mysqli($hostaddress,$dbuser,$password,$dbname) or die("dbconn error");
?>