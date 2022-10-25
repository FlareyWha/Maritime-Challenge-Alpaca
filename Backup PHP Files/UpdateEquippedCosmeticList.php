<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iOwnerUID"])||!isset($_POST["iNewCosmeticID"])||!isset($_POST["iOldCosmeticID"]))
        throw new Exception("not posted!"); 
    $iOwnerUID = $_POST["iOwnerUID"];
    $iNewCosmeticID = $_POST["iNewCosmeticID"];
    $iOldCosmeticID = $_POST["iOldCosmeticID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

$query = "update tb_equippedCosmeticList set iCosmeticID=? where iOwnerUID=? and iCosmeticID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("iii", $iNewCosmeticID, $iOwnerUID, $iOldCosmeticID);
$stmt->execute();
$stmt->fetch();
$equippedCosmeticsUpdated = $stmt->affected_rows;

http_response_code(200);
echo "No of equipped cosmetics updated: $equippedCosmeticsUpdated";

$stmt->close();
$conn->close();
?>