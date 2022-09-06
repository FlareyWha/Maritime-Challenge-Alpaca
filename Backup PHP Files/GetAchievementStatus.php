<?php //UpdateCoins.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //Check if POST fields are received
    if (!isset($_POST["UID"]) || !isset($_POST["iAchievementID"]))
        throw new Exception("not posted!");
    $uid = $_POST["UID"];
    $iAchievementID = $_POST["iAchievementID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to update the iCoins for the account with uid
$query = "select bAchievementCompleted, bAchievementClaimed from tb_achievementList where iOwnerUID=? and iAchievementID=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("ii", $uid, $iAchievementID);

//Execute statement
$stmt->execute();
$stmt->bind_result($bAchievementCompleted, $bAchievementClaimed);

//Bind into array to send as json
$arr = Array();
$arr["achievementStatus"] = Array();

while ($stmt->fetch())
{
    $JSONAchievementStatus= array (
        "bAchievementCompleted" => $bAchievementCompleted,
        "bAchievementClaimed" => $bAchievementClaimed
    );
    array_push($arr["achievementStatus"], $JSONAchievementStatus);
}

http_response_code(200);
echo json_encode($arr);
$stmt->close();
$conn->close();
?>