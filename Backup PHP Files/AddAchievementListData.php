<?php //AddPhonebookData.php
require("dbconn_inc.php"); // include the external file

//Get all the uids 
$uidArray = require "GetUIDArrayStatement.php";

$query="select iAchievementID from tb_achievement";
$stmt=$conn->prepare($query);
$stmt->execute();
$stmt->bind_result($foundAchievementID);

$achievementIDArray = Array();

while ($stmt->fetch())
{
    array_push($achievementIDArray, $foundAchievementID);
}

$stmt->close();

foreach ($uidArray as $uid)
{
    foreach ($achievementIDArray as $achievementID)
    {
        //Insert all other people into the owners phonebook
        $query="insert into tb_achievementList (iOwnerUID, iAchievementID, bAchievementClaimed) select * from (select ? as iOwnerUID, ? as iAchievementID, 0 as bAchievementClaimed) as tmp where not exists (select iOwnerUID, iAchievementID from tb_achievementList where iOwnerUID=? and iAchievementID=?) LIMIT 1";
        $stmt=$conn->prepare($query);
        $stmt->bind_param("iiii", $uid, $achievementID, $uid, $achievementID);
        $stmt->execute();
        $stmt->close();
    }
}
$conn->close();
?>