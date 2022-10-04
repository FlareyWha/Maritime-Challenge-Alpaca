<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

// try
// {
//     //check if POST fields are received, else quit
//     if (!isset($_POST["iCosmeticID"]))
//         throw new Exception("not posted!"); 
//     $iCosmeticID = $_POST["iCosmeticID"];
// }
// catch (Exception $e)
// {
//     http_response_code(400);
//     echo 'Caught exception: ', $e->getMessage();
//     die();
// }

// // Prepare statement to get the necessary stats from tb_account
// $query = "select iCosmeticID, sCosmeticName, iCosmeticCost, iCosmeticRarity, iCosmeticType from tb_cosmetic order by iCosmeticID asc";
// $stmt = $conn->prepare($query);
// $stmt->bind_param("i", $uid);
// $stmt->execute();
// $stmt->bind_result($sCosmeticName, $iCosmeticCost, $iCosmeticRarity, $iCosmeticType);

// //Bind into array to send as json
// $arr = Array();
// $arr["cosmeticData"] = Array();

// while ($stmt->fetch())
// {
//     $JSONCosmeticData = array (
//         "iCosmeticID" => $iCosmeticID,
//         "sCosmeticName" => $sCosmeticName,
//         "iCosmeticCost" => $iCosmeticCost,
//         "iCosmeticRarity" => $iCosmeticRarity,
//         "iCosmeticType" => $iCosmeticType
//     );
//     array_push($arr["cosmeticData"], $JSONCosmeticData);
// }

// http_response_code(200);
// echo json_encode($arr);

// $stmt->close();
// $conn->close();

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
$query = "select tb_cosmetic.iCosmeticID, sCosmeticName, iCosmeticCost, iCosmeticRarity, iCosmeticType, bCosmeticUnlocked from tb_cosmetic inner join tb_cosmeticList on tb_cosmetic.iCosmeticID=tb_cosmeticList.iCosmeticID where iOwnerUID=? order by tb_cosmetic.iCosmeticID asc";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($sCosmeticName, $iCosmeticCost, $iCosmeticRarity, $iCosmeticType, $bCosmeticUnlocked);

//Bind into array to send as json
$arr = Array();
$arr["cosmeticData"] = Array();

while ($stmt->fetch())
{
    $JSONCosmeticData = array (
        "iCosmeticID" => $iCosmeticID,
        "sCosmeticName" => $sCosmeticName,
        "iCosmeticCost" => $iCosmeticCost,
        "iCosmeticRarity" => $iCosmeticRarity,
        "iCosmeticType" => $iCosmeticType,
        "bCosmeticUnlocked" => $bCosmeticUnlocked
    );
    array_push($arr["cosmeticData"], $JSONCosmeticData);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>