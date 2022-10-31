<?php //Register.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["sUsername"])||!isset($_POST["sEmail"])||!isset($_POST["sPassword"])||!isset($_POST["dBirthday"])||!isset($_POST["iGender"])) 
        throw new Exception("not posted!");
        
    $sUsername=$_POST["sUsername"];
    $sEmail=$_POST["sEmail"];
    $sPassword=$_POST["sPassword"];
    $dBirthday=$_POST["dBirthday"];
    $iGender=$_POST["iGender"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Encrypt password
//$sPassword = password_hash($sPassword, PASSWORD_DEFAULT);
$sPassword = hash("sha256", $sPassword);

//Set new timezone
date_default_timezone_set('UTC');
$sDateTime = date("y-m-d H:i:s");

$iTotalPlayers = require "GetTotalPlayersStatement.php";

$iGuildID = 4 - (180 - $iTotalPlayers) % 4;

//Create account here
$query="insert into tb_account values (0,?,?,?,0,?,?,1,\"Hi!\",0,0,0,{$iGuildID},0,0,0,0,1,0,0,0,\"{$sDateTime}\")";
$stmt=$conn->prepare($query);
//s - string, i - integer...make sure it matches the data types!
$stmt->bind_param("ssssi", $sUsername, $sEmail, $sPassword, $dBirthday, $iGender);

// Execute Statement
$stmt->execute();
$stmt->fetch();

$usersRegistered = $stmt->affected_rows;

http_response_code(200);
echo "No of user registered: $usersRegistered";

$stmt->close(); // Close Statement

require "AddPhonebookData.php";

$conn->close(); // Close connection
?>