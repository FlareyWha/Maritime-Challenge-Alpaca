<?php //UpdateCoins.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //Check if POST fields are received
    if (!isset($_POST["UID"]) || !isset($_POST["OtherUID"]))
        throw new Exception("not posted!");
    $uid = $_POST["UID"];
    $OtherUID = $_POST["OtherUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

$true = 1;

//Prepare statement to update bOtherUnlocked
$query = "update tb_phonebook set bOtherUnlocked=? where iOwnerUID=? and iOtherUID=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("iii", $true, $uid, $OtherUID);
$stmt->execute();
$stmt->close();

$stmt2=$conn->prepare($query);
$stmt2->bind_param("iii", $true, $OtherUID, $uid);
$stmt2->execute();
$stmt2->close();


$conn->close();
http_response_code(200);
echo "Sucessfully updated both";
?>