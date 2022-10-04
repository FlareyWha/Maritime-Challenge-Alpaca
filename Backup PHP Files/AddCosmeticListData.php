<?php //AddPhonebookData.php
//require("dbconn_inc.php"); // include the external file

$uid = require "GetUidStatement.php";

//Get all the uids 
$uidArray = require "GetUIDArrayStatement";

$query="select iCosmeticID from tb_cosmetic";
$stmt=$conn->prepare($query);
$stmt->execute();
$stmt->bind_result($foundCosmeticID);

$cosmeticIDArray = Array();

while ($stmt->fetch())
{
    array_push($cosmeticIDArray, $foundCosmeticID);
}

$stmt->close();

foreach ($uidArray as $uid)
{
    foreach ($cosmeticIDArray as $cosmeticID)
    {
        //Insert all other people into the owners phonebook
        $query="insert into tb_cosmeticList (iOwnerUID, iCosmeticID, bCosmeticUnlocked) select * from (select ? as iOwnerUID, ? as iCosmeticID, 0 as bCosmeticUnlocked) as tmp where not exists (select iOwnerUID, iCosmeticID from tb_cosmeticList where iOwnerUID=? and iCosmeticID=?) LIMIT 1";
        $stmt=$conn->prepare($query);
        $stmt->bind_param("iiii", $uid, $cosmeticID, $uid, $cosmeticID);
        $stmt->execute();
        $stmt->close();
    }
}

?>