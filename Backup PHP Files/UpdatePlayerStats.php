<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iOwnerUID"])||!isset($_POST["sStatName"])||!isset($_POST["iStatAmount"]))
        throw new Exception("not posted!"); 
    $iOwnerUID = $_POST["iOwnerUID"];
    $sStatName = $_POST["sStatName"];
    $iStatAmount = $_POST["iStatAmount"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

// Prepare statement to get the necessary stats from tb_account
$query = "update tb_playerStats set ".$sStatName."=? where iOwnerUID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("ii", $iStatAmount, $iOwnerUID);
$stmt->execute();
$stmt->fetch();
http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";

$stmt->close();
$conn->close();
?>