<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["UID"]))
        throw new Exception("not posted!"); 
    $uid = $_POST["UID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

// Prepare statement to get the necessary stats from tb_account
$query = "select sUsername from tb_account where UID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($sUsername);
$stmt->fetch();

http_response_code(200);
echo $sUsername;

$stmt->close();
$conn->close();
?>