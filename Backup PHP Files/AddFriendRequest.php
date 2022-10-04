<?php //GetUsername.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["UID"])||!isset($_POST["iOtherUID"]))
        throw new Exception("not posted!");

    $uid = $_POST["UID"];
    $iOtherUid = $_POST["iOtherUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement
$query="select * from tb_friendRequestList where iOwnerUID=? and iOtherUID=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("ii", $uid, $iOtherUid);
$stmt->execute();
$stmt->fetch(); 
if ($stmt->affected_rows > 0)
{
    http_response_code(400);
    echo "Friend request already sent.";
    die();
}
$stmt->close();


//Prepare statement
$query="insert into tb_friendRequestList values (?, ?)";
$stmt2=$conn->prepare($query);
$stmt2->bind_param("ii", $uid, $iOtherUid);
$stmt2->execute();
$stmt2->fetch(); 
if ($stmt2->affected_rows < 0)
{
    http_response_code(400);
    echo "Unsucessful friend request. Check the UID's maybe.";
}
else
    $friendsAdded = $stmt2->affected_rows;
$stmt2->close();

$conn->close(); // Close connection

http_response_code(200);
echo "No of friend requests sent: $friendsAdded";
?>