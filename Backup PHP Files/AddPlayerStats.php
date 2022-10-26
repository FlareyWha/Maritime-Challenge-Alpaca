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

// Prepare statement to get the necessary stats from tb_account
$query = "insert into tb_playerStats (iOwnerUID) values (?)";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);
$stmt->execute();
$stmt->fetch();

http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";

$stmt->close();
$conn->close();
?>