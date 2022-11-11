<?php //UpdateXPLevels.php
//Connect database 
require("dbconn_inc.php"); // include the external file

//Check if POST fields are received
try
{
    if (!isset($_POST["UID"])||!isset($_POST["fPlayerXPos"])||!isset($_POST["fPlayerYPos"])||!isset($_POST["fPlayerZPos"]))
        throw new Exception("not posted!");
    $UID = $_POST["UID"];
    $fPlayerXPos = $_POST["fPlayerXPos"];
    $fPlayerYPos = $_POST["fPlayerYPos"];
    $fPlayerZPos = $_POST["fPlayerZPos"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to update the iLevel and iXP of the acccount with the uid
$query = "update tb_account set fPlayerXPos=?, fPlayerYPos=?, fPlayerZPos=? where UID=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("fffi", $fPlayerXPos, $fPlayerYPos, $fPlayerZPos, $UID);

//Execute statement
$stmt->execute();
$stmt->fetch();
//Returns number of rows updated
http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";
$stmt->close();

$conn->close();
?>