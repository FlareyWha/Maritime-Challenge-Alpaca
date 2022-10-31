<?php //UpdateXPLevels.php
//Connect database 
require("dbconn_inc.php"); // include the external file

//Check if POST fields are received
try
{
    if (!isset($_POST["UID"])||!isset($_POST["fXPlayerPos"])||!isset($_POST["fYPlayerPos"])||!isset($_POST["fZPlayerPos"]))
        throw new Exception("not posted!");
    $UID = $_POST["UID"];
    $fXPlayerPos = $_POST["fXPlayerPos"];
    $fYPlayerPos = $_POST["fYPlayerPos"];
    $fZPlayerPos = $_POST["fZPlayerPos"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to update the iLevel and iXP of the acccount with the uid
$query = "update tb_account set fXPlayerPos=?, fYPlayerPos=?, fZPlayerPos=? where UID=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("fffi", $fXPlayerPos, $fYPlayerPos, $fZPlayerPos, $UID);

//Execute statement
$stmt->execute();
$stmt->fetch();
//Returns number of rows updated
http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";
$stmt->close();

$conn->close();
?>