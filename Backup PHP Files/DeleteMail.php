<?php //GetUsername.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["iMailID"]))
        throw new Exception("not posted!");

    $iMailID = $_POST["iMailID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement
$query="delete from tb_mailList where iMailID=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("i", $iMailID);
$stmt->execute();
$stmt->fetch(); 
$mailsRemoved = $stmt->affected_rows;

$stmt->close();
$conn->close(); // Close connection

http_response_code(200);
echo "No of mail removed: $mailsRemoved";
?>