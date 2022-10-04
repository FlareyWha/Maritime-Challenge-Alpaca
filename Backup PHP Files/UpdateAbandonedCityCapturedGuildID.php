<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iCapturedGuildID"])||!isset($_POST["iAbandonedCityID"]))
        throw new Exception("not posted!");
    $iCapturedGuildID = $_POST["iCapturedGuildID"];
    $iAbandonedCityID = $_POST["iAbandonedCityID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

// Prepare statement to get the necessary stats from tb_account
$query = "update tb_abandonedCity set iCapturedGuildID=? where iAbandonedCityID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("ii", $iCapturedGuildID, $iAbandonedCityID);
$stmt->execute();
$stmt->fetch();

http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";

$stmt->close();
$conn->close();
?>