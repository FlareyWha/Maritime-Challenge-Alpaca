<?php //GetUsername.php
// Connect database
require("dbconn_inc.php");

//check if POST fields are received, else quit
try
{
    if(!isset($_POST["UID"])||!isset($_POST["OtherUID"]))
        throw new Exception("not posted!");

    $uid = $_POST["UID"];
    $otherUid = $_POST["OtherUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to check if the uid that uses the username and password exists
$query="insert into tb_friendList values (?, ?, 1, 0), (?, ?, 1, 0)";
$stmt=$conn->prepare($query);
$stmt->bind_param("iiii", $uid, $otherUid, $otherUid, $uid);
$stmt->execute();
$stmt->fetch(); 
if ($stmt->affected_rows < 0)
{
    http_response_code(400);
    echo "Unsucessful adding of friend. Check the UID's maybe.";
}
else
    $friendsAdded = $stmt->affected_rows;
$stmt->close();
$conn->close(); // Close connection

http_response_code(200);
echo "Friends added: $friendsAdded";
?>