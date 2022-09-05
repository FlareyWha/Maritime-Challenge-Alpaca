<?php //GetUsername.php
// Connect database
require("dbconn_inc.php");

try
{
    //check if POST fields are received, else quit
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
$query="update tb_phonebook set bOtherUnlocked=true where iOwnerUID=? AND iOtherUID=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("ii", $uid, $otherUid);
$stmt->execute();
$stmt->fetch(); 
if ($stmt->affected_rows < 0)
{
    http_response_code(400);
    echo "Unsucessful unlocking of phonebook entry. Check the UID's maybe.";
}
else
    $peopleUnlocked = $stmt->affected_rows;
$stmt->close();
$conn->close(); // Close connection

http_response_code(200);
echo "People unlocked: $peopleUnlocked";
?>