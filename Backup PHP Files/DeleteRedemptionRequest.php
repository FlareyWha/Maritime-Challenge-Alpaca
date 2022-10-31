<?php //GetUsername.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["iRedemptionRequestID"]))
        throw new Exception("not posted!");

    $iRedemptionRequestID = $_POST["iRedemptionRequestID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement
$query="delete from tb_redemptionRequestList where iRedemptionRequestID=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("i", $iRedemptionRequestID);
$stmt->execute();
$stmt->fetch(); 
$redemptionRequestsRemoved = $stmt->affected_rows;

$stmt->close();
$conn->close(); // Close connection

http_response_code(200);
echo "No of redemption requests removed: $redemptionRequestsRemoved";
?>