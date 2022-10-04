<?php //AddPhonebookData.php
//require("dbconn_inc.php"); // include the external file

$uid = require "GetUidStatement.php";

//Get all the uids 
$uidArray = require "GetUIDArrayStatement";

$query="select iTitleID from tb_title";
$stmt=$conn->prepare($query);
$stmt->execute();
$stmt->bind_result($foundTitleID);

$titleIDArray = Array();

while ($stmt->fetch())
{
    array_push($titleIDArray, $foundTitleID);
}

$stmt->close();

foreach ($uidArray as $uid)
{
    foreach ($titleIDArray as $titleID)
    {
        //Insert all other people into the owners phonebook
        $query="insert into tb_titleList (iOwnerUID, iTitleID, bTitleUnlocked) select * from (select ? as iOwnerUID, ? as iTitleID, 0 as bTitleUnlocked) as tmp where not exists (select iOwnerUID, iTitleID from tb_titleList where iOwnerUID=? and iTitleID=?) LIMIT 1";
        $stmt=$conn->prepare($query);
        $stmt->bind_param("iiii", $uid, $titleID, $uid, $titleID);
        $stmt->execute();
        $stmt->close();
    }
}

?>