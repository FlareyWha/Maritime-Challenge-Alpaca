<?php //GetUsername.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["iRedemptionItemID"]) || !isset($_POST["iOwnerUID"]))
        throw new Exception("not posted!");

    $iRedemptionItemID = $_POST["iRedemptionItemID"];
    $iOwnerUID = $_POST["iOwnerUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement
$query="insert into tb_redemptionRequestList values (0,?,?)";
$stmt=$conn->prepare($query);
$stmt->bind_param("ii", $iRedemptionItemID, $iOwnerUID);
$stmt->execute();
$stmt->fetch(); 
$redemptionRequestsAdded = $stmt->affected_rows;

$stmt->close();
$conn->close(); // Close connection

http_response_code(200);
echo "No of redemption requests added sent: $redemptionRequestsAdded";
?>