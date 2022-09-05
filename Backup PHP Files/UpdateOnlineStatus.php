<?php //UpdateOnlineStatus.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //Check if POST fields are received
    if (!isset($_POST["bOnline"]) || !isset($_POST["UID"]))
        throw new Exception("not posted!");
    $bOnline = $_POST["bOnline"];
    $uid = $_POST["UID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

if ($bOnline < 0 || $bOnline > 1)
{
    http_response_code(400);
    echo "Wrong online value";
    die();
}

//Prepare statement to update bOtherUnlocked
$query = "update tb_account set bOnline=? where UID=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("ii", $bOnline, $uid);
$stmt->execute();
$stmt->close();
http_response_code(200);
echo "Set online to " . $bOnline;
$conn->close();
