<?php //UpdateCoins.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //Check if POST fields are received
    if (!isset($_POST["UID"]) || !isset($_POST["iMissionID"]))
        throw new Exception("not posted!");
    $uid = $_POST["UID"];
    $iMissionID = $_POST["iMissionID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to update the iCoins for the account with uid
$query = "select bMissionCompleted, bMissionClaimed from tb_missionList where iOwnerUID=? and iMissionID=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("ii", $uid, $iMissionID);

//Execute statement
$stmt->execute();
$stmt->bind_result($bMissionCompleted, $bMissionClaimed);

//Bind into array to send as json
$arr = Array();
$arr["missionStatus"] = Array();

while ($stmt->fetch())
{
    $JSONMissionStatus= array (
        "bMissionCompleted" => $bMissionCompleted,
        "bMissionClaimed" => $bMissionClaimed
    );
    array_push($arr["missionStatus"], $JSONMissionStatus);
}

http_response_code(200);
echo json_encode($arr);
$stmt->close();
$conn->close();
?>