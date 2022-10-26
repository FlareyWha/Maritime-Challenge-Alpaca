<?php //GetUsername.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["iOwnerUID"]))
        throw new Exception("not posted!");

    $iOwnerUID = $_POST["iOwnerUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement
$query="select iMailID, sMailTitle, sMailDescription, iMailItemAmount from tb_mailList where iOwnerUID=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);
$stmt->execute();
$stmt->fetch(); 
$stmt->bind_result($iMailID, $sMailTitle, $sMailDescription, $iMailItemAmount);

//Bind into array to send as json
$arr = Array();
$arr["mailData"] = Array();

while ($stmt->fetch())
{
    $JSONMailData = array (
        "iMailID" => $iMailID,
        "sMailTitle" => $sMailTitle,
        "sMailDescription" => $sMailDescription,
        "iMailItemAmount" => $iMailItemAmount
    );
    array_push($arr["mailData"], $JSONMailData);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close(); // Close connection
?>