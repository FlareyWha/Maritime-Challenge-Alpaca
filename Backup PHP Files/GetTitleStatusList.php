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
$query = "select iTitleID, bTitleUnlocked from tb_title where iOwnerUID=? order by iTitleID asc";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($iTitleID, $bTitleUnlocked);

//Bind into array to send as json
$arr = Array();
$arr["cosmeticData"] = Array();

while ($stmt->fetch())
{
    $JSONCosmeticData = array (
        "iTitleID" => $iTitleID,
        "bTitleUnlocked" => $bTitleUnlocked
    );
    array_push($arr["cosmeticData"], $JSONCosmeticData);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>