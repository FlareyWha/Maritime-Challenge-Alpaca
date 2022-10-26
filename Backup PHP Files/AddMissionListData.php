<?php //AddPhonebookData.php
require("dbconn_inc.php"); // include the external file

//Get all the uids 
$uidArray = require "GetUIDArrayStatement.php";

$query="select iMissionID from tb_mission";
$stmt=$conn->prepare($query);
$stmt->execute();
$stmt->bind_result($foundMissionID);

$missionIDArray = Array();

while ($stmt->fetch())
{
    array_push($missionIDArray, $foundMissionID);
}

$stmt->close();

foreach ($uidArray as $uid)
{
    foreach ($missionIDArray as $missionID)
    {
        //Insert all other people into the owners phonebook
        $query="insert into tb_missionList (iOwnerUID, iMissionID, bMissionClaimed) select * from (select ? as iOwnerUID, ? as iMissionID, 0 as bMissionClaimed) as tmp where not exists (select iOwnerUID, iMissionID from tb_missionList where iOwnerUID=? and iMissionID=?) LIMIT 1";
        $stmt=$conn->prepare($query);
        $stmt->bind_param("iiii", $uid, $missionID, $uid, $missionID);
        $stmt->execute();
        $stmt->close();
    }
}
$conn->close();
?>