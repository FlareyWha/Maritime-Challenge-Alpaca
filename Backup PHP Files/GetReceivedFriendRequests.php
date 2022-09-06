<?php //GetRecievedFriendRequests.php
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
$query = "select iOwnerUID from tb_friendRequestList where iOtherUID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($iOwnerUID);

//Bind into array to send as json
$arr = Array();
$arr["receivedFriendRequests"] = Array();

while ($stmt->fetch())
{
    $JSONReceivedFriendRequest = array (
        "iOwnerUID" => $iOwnerUID
    );
    array_push($arr["receivedFriendRequests"], $JSONReceivedFriendRequest);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>