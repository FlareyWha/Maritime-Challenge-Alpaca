<?php //DeleteFriendRequest.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["UID"])||!isset($_POST["iOwnerUID"]))
        throw new Exception("not posted!");

    $uid = $_POST["UID"];
    $iOwnerUID = $_POST["iOwnerUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Remove the friend request
$query="delete from tb_friendRequestList where iOwnerUID=? and iOtherUID=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("ii", $iOwnerUID, $uid);
$stmt->execute();
$stmt->fetch(); 
if ($stmt->affected_rows < 0)
{
    http_response_code(400);
    echo "Failed to remove friend request";
    die();
}
else
    $friendRequestsRemoved = $stmt->affected_rows;
$stmt->close();

http_response_code(200);
echo "Friend requests removed: $friendRequestsRemoved";
?>