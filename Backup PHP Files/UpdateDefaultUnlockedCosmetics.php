<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iOwnerUID"]))
        throw new Exception("not posted!"); 
    $iOwnerUID = $_POST["iOwnerUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

$query = "update tb_cosmeticList set bCosmeticUnlocked=true where iOwnerUID=? and iCosmeticID between 1 and 9";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);
$stmt->execute();
$stmt->fetch();
$cosmeticsUpdated = $stmt->affected_rows;

http_response_code(200);
echo "No of cosmetics updated: $cosmeticsUpdated";

$stmt->close();
$conn->close();
?>