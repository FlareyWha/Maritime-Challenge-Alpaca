<?php //VerifyEmail.php
// Connect database
require("dbconn_inc.php");

try
{
    //check if POST fields are received, else quit
    if(!isset($_POST["sEmail"]))die("not posted!");
    $sEmail=$_POST["sEmail"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to get whether the email already exists in tb_account, signifying that it already has an account
$query="select sEmail from tb_account where sEmail=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("s", $sEmail);
$stmt->execute();
$stmt->bind_result($sEmailCheck);
$stmt->fetch(); 
$stmt->close();
$conn->close(); // Close connection

//Fail email check if email already exists
if ($sEmailCheck != null)
{
    http_response_code(400);
    echo "Email already has an account. \nPlease enter a new email and try again.";
}
else
{
    http_response_code(200);
    echo "Success";
}
?>