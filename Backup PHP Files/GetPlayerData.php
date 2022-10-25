<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["UID"]))
        throw new Exception("not posted!"); 
    $uid = $_POST["UID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

// Prepare statement to get the necessary stats from tb_account
$query = "select sUsername, bShowBirthday, dBirthday, iCurrentTitleID, sBiography, iLevel, iXP, iDepartment, iGuildID, iCountry, iTotalRightshipRollars, iTotalTokens, iTotalEventCurrency, iCurrentBattleship, fPlayerXPos, fPlayerYPos, fPlayerZPos, dtLastLogin from tb_account where UID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($sUsername, $bShowBirthday, $dBirthday, $iCurrentTitleID, $sBiography, $iLevel, $iXP, $iDepartment, $iGuildID, $iCountry, $iTotalRightshipRollars, $iTotalTokens, $iTotalEventCurrency, $iCurrentBattleship, $fPlayerXPos, $fPlayerYPos, $fPlayerZPos, $dtLastLogin);

//Bind into array to send as json
$arr = Array();
$arr["playerData"] = Array();

while ($stmt->fetch())
{
    $JSONPlayerData = array (
        "sUsername" => $sUsername,
        "bShowBirthday" => $bShowBirthday,
        "dBirthday" => $dBirthday,
        "iCurrentTitleID" => $iCurrentTitleID,
        "sBiography" => $sBiography,
        "iLevel" => $iLevel,
        "iXP" => $iXP,
        "iDepartment" => $iDepartment,
        "iGuildID" => $iGuildID,
        "iCountry" => $iCountry,
        "iTotalRightshipRollars" => $iTotalRightshipRollars,
        "iTotalTokens" => $iTotalTokens,
        "iTotalEventCurrency" => $iTotalEventCurrency,
        "iCurrentBattleship" => $iCurrentBattleship,
        "fPlayerXPos" => $fPlayerXPos,
        "fPlayerYPos" => $fPlayerYPos,
        "fPlayerZPos" => $fPlayerZPos,
        "dtLastLogin" => $dtLastLogin
    );
    array_push($arr["playerData"], $JSONPlayerData);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>