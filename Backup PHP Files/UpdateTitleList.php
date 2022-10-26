<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iOwnerUID"])||!isset($_POST["iTitleID"]))
        throw new Exception("not posted!"); 
    $iOwnerUID = $_POST["iOwnerUID"];
    $iTitleID = $_POST["iTitleID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

$query = "update tb_titleList set bTitleUnlocked=true where iOwnerUID=? and iTitleID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("ii", $iOwnerUID, $iTitleID);
$stmt->execute();
$stmt->fetch();
$titlesUpdated = $stmt->affected_rows;

http_response_code(200);
echo "No of titles updated: $titlesUpdated";

$stmt->close();
$conn->close();
?>