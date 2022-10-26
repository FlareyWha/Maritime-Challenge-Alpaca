<?php //GetUsername.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["iOwnerUID"]) || !isset($_POST["sMailTitle"]) || !isset($_POST["sMailDescription"]) || !isset($_POST["iMailItemAmount"]))
        throw new Exception("not posted!");

    $iOwnerUID = $_POST["iOwnerUID"];
    $sMailTitle = $_POST["sMailTitle"];
    $sMailDescription = $_POST["sMailDescription"];
    $iMailItemAmount = $_POST["iMailItemAmount"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement
$query="insert into tb_mailList values (0,?,?,?,?)";
$stmt=$conn->prepare($query);
$stmt->bind_param("issi", $iOwnerUID, $sMailTitle, $sMailDescription, $iMailItemAmount);
$stmt->execute();
$stmt->fetch(); 
$mailsSent = $stmt->affected_rows;

$stmt->close();
$conn->close(); // Close connection

http_response_code(200);
echo "No of mail sent: $mailsSent";
?>