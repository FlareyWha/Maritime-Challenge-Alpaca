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
$query = "select tb_battleship.iBattleshipID, sBattleshipName, iHP, iAtk, fAtkSpd, fCritRate, fCritDmg, fMoveSpd, bBattleshipUnlocked from tb_battleship inner join tb_battleshipList on tb_battleship.iBattleshipID=tb_battleshipList.iBattleshipID where iOwnerUID=? order by tb_battleship.iBattleshipID asc";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);
$stmt->execute();
$stmt->bind_result($iBattleshipID, $sBattleshipName, $iHP, $iAtk, $fAtkSpd, $fCritRate, $fCritDmg, $fMoveSpd, $bBattleshipUnlocked);

//Bind into array to send as json
$arr = Array();
$arr["battleshipData"] = Array();

while ($stmt->fetch())
{
    $JSONBattleshipData = array (
        "iBattleshipID" => $iBattleshipID,
        "sBattleshipName" => $sBattleshipName,
        "iHP" => $iHP,
        "iAtk" => $iAtk,
        "fAtkSpd" => $fAtkSpd,
        "fCritRate" => $fCritRate,
        "fCritDmg" => $fCritDmg,
        "fMoveSpd" => $fMoveSpd,
        "bBattleshipUnlocked" => $bBattleshipUnlocked
    );
    array_push($arr["battleshipData"], $JSONBattleshipData);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>