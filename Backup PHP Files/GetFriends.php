<?php //ReadPlayerStats.php
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
$query = "select iFriendUID, sUsername from tb_friendList inner join tb_account on tb_account.UID=tb_friendList.iFriendUID where iOwnerUID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($iFriendUID, $sUsername);

//Bind into array to send as json
$arr = Array();
$arr["friends"] = Array();

while ($stmt->fetch())
{
    $JSONFriendList = array (
        "iFriendUID" => $iFriendUID,
        "sUsername" => $sUsername
    );
    array_push($arr["friends"], $JSONFriendList);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>