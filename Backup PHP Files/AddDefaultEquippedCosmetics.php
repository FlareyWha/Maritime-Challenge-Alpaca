<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iOwnerUID"])||!isset($_POST["iGender"]))
        throw new Exception("not posted!"); 
    $iOwnerUID = $_POST["iOwnerUID"];
    $iGender = $_POST["iGender"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

if ($iGender == 0)
    $query = "insert into tb_equippedCosmeticList values (?, 1), (?, 2), (?, 3), (?, 4), (?, 5)";
else if ($iGender == 1)
    $query = "insert into tb_equippedCosmeticList values (?, 1), (?, 6), (?, 7), (?, 8), (?, 9)";

$stmt = $conn->prepare($query);
$stmt->bind_param("iiiii", $iOwnerUID, $iOwnerUID, $iOwnerUID, $iOwnerUID, $iOwnerUID);
$stmt->execute();
$stmt->fetch();
$equippedCosmeticsAdded = $stmt->affected_rows;

http_response_code(200);
echo "No of equipped cosmetics added: $equippedCosmeticsAdded";

$stmt->close();
$conn->close();
?>