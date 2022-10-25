<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iOwnerUID"])||!isset($_POST["iBattleshipID"]))
        throw new Exception("not posted!"); 
    $iOwnerUID = $_POST["iOwnerUID"];
    $iBattleshipID = $_POST["iBattleshipID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

$query = "update tb_battleshipList set bBattleshipUnlocked=true where iOwnerUID=? and iBattleshipID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("ii", $iOwnerUID, $iBattleshipID);
$stmt->execute();
$stmt->fetch();
$battleshipUpdated = $stmt->affected_rows;

http_response_code(200);
echo "No of battleships updated: $battleshipUpdated";

$stmt->close();
$conn->close();
?>