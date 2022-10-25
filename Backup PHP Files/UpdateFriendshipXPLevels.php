<?php //UpdateXPLevels.php
//Connect database 
require("dbconn_inc.php"); // include the external file

//Check if POST fields are received
try
{
    if (!isset($_POST["iOwnerUID"]) || !isset($_POST["iFriendUID"]) || !isset($_POST["iFriendshipLevel"]) || !isset($_POST["iFriendshipXP"]))
        throw new Exception("not posted!");
    $iOwnerUID = $_POST["iOwnerUID"];
    $iFriendUID = $_POST["iFriendUID"];
    $iFriendshipLevel = $_POST["iFriendshipLevel"];
    $iFriendshipXP = $_POST["iFriendshipXP"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to update the iLevel and iXP of the acccount with the uid
$query = "update tb_friendList set iFriendshipLevel=?, iFriendshipXP=? where iOwnerUID=? and iFriendUID=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("iiii", $iFriendshipLevel, $iFriendshipXP, $iOwnerUID, $iFriendUID);

//Execute statement
$stmt->execute();
$stmt->fetch();
//Returns number of rows updated
http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";
$stmt->close();

$conn->close();
?>