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
$query = "select iCosmeticID, bCosmeticUnlocked from tb_cosmeticList where iOwnerUID=? order by iCosmeticID asc";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($iCosmeticID, $bCosmeticUnlocked);

//Bind into array to send as json
$arr = Array();
$arr["cosmeticStatusList"] = Array();

while ($stmt->fetch())
{
    $JSONCosmeticStatus = array (
        "iCosmeticID" => $iCosmeticID,
        "bCosmeticUnlocked" => $bCosmeticUnlocked
    );
    array_push($arr["cosmeticStatusList"], $JSONCosmeticStatus);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>