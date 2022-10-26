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
$query = "select iCosmeticID from tb_equippedCosmeticList where iOwnerUID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);
$stmt->execute();
$stmt->bind_result($iCosmeticID);

//Bind into array to send as json
$arr = Array();
$arr["equippedCosmetics"] = Array();

while ($stmt->fetch())
{
    $JSONEquippedCosmetic = array (
        "iCosmeticID" => $iCosmeticID,
    );
    array_push($arr["equippedCosmetics"], $JSONEquippedCosmetic);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>