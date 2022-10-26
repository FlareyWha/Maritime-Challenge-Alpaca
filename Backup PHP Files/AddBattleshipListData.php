<?php //AddPhonebookData.php
require("dbconn_inc.php"); // include the external file

//Get all the uids 
$uidArray = require "GetUIDArrayStatement.php";

$query="select iBattleshipID from tb_battleship";
$stmt=$conn->prepare($query);
$stmt->execute();
$stmt->bind_result($foundBattleshipID);

$battleshipIDArray = Array();

while ($stmt->fetch())
{
    array_push($battleshipIDArray, $foundBattleshipID);
}

$stmt->close();

foreach ($uidArray as $uid)
{
    foreach ($battleshipIDArray as $battleshipID)
    {
        //Insert all other people into the owners phonebook
        $query="insert into tb_battleshipList (iOwnerUID, iBattleshipID, bBattleshipUnlocked) select * from (select ? as iOwnerUID, ? as iBattleshipID, 0 as bBattleshipUnlocked) as tmp where not exists (select iOwnerUID, iBattleshipID from tb_battleshipList where iOwnerUID=? and iBattleshipID=?) LIMIT 1";
        $stmt=$conn->prepare($query);
        $stmt->bind_param("iiii", $uid, $battleshipID, $uid, $battleshipID);
        $stmt->execute();
        $stmt->close();
    }
}
$conn->close();
?>