<?php //UpdateCoins.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //Check if POST fields are received
    if (!isset($_POST["iOwnerUID"]))
        throw new Exception("not posted!");
    $iOwnerUID = $_POST["iOwnerUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to update the iCoins for the account with uid
$query = "select tb_achievement.iAchievementID, sAchievementName, sAchievementDescription, iEarnedTitleID, bAchievementClaimed from tb_achievementList inner join tb_achievement on tb_achievement.iAchievementID=tb_achievementList.iAchievementID where iOwnerUID=? order by tb_achievement.iAchievementID asc";
$stmt=$conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);

//Execute statement
$stmt->execute();
$stmt->bind_result($iAchievementID, $sAchievementName, $sAchievementDescription, $iEarnedTitleID, $bAchievementClaimed);

//Bind into array to send as json
$arr = Array();
$arr["achievementData"] = Array();

while ($stmt->fetch())
{
    $JSONAchievementData= array (
        "iAchievementID" => $iAchievementID,
        "sAchievementName" => $sAchievementName,
        "sAchievementDescription" => $sAchievementDescription,
        "iEarnedTitleID" => $iEarnedTitleID,
        "bAchievementClaimed" => $bAchievementClaimed
    );
    array_push($arr["achievementData"], $JSONAchievementData);
}

http_response_code(200);
echo json_encode($arr);
$stmt->close();
$conn->close();
?>