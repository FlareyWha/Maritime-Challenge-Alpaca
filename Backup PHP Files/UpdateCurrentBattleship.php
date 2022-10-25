<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["UID"])||!isset($_POST["iCurrentBattleship"]))
        throw new Exception("not posted!"); 
    $UID = $_POST["UID"];
    $iCurrentBattleship = $_POST["iCurrentBattleship"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

$query = "update tb_account set iCurrentBattleship=? where UID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("ii", $iCurrentBattleship, $UID);
$stmt->execute();
$stmt->fetch();
$battleshipUpdated = $stmt->affected_rows;

http_response_code(200);
echo "No of rows updated: $battleshipUpdated";

$stmt->close();
$conn->close();
?>