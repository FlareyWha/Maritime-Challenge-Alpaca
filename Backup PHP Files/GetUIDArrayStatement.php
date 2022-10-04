<?php //GetUIDArrayStatement.php
//Get all the uids
$query="select UID from tb_account";
$stmt=$conn->prepare($query);
$stmt->execute();
$stmt->bind_result($foundUID);

$uidArray = Array();

while ($stmt->fetch())
{
    array_push($uidArray, $foundUID);
}

$stmt->close();

return $uidArray;
?>