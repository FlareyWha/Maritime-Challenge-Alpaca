<?php //GetFriendInfo.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["UID"])||!isset($_POST["iFriendUID"]))
        throw new Exception("not posted!"); 
    $uid = $_POST["UID"];
    $iFriendUID = $_POST["iFriendUID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

// Prepare statement to get the necessary stats from friendUID, using iOwnerUID (which is you)
$query = "select sUsername, dBirthday, iCurrentTitleID, sBiography, iLevel, iDepartment, iGuildID, iCountry, iFriendshipLevel, iFriendshipXP from tb_account inner join tb_friendList on tb_account.UID=tb_friendList.iFriendUID where iOwnerUID=? and iFriendUID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("ii", $uid, $iFriendUID);
$stmt->execute();
$stmt->bind_result($sUsername, $dBirthday, $iCurrentTitleID, $sBiography, $iLevel, $iDepartment, $iGuildID, $iCountry, $iFriendshipLevel, $iFriendshipXP);

//Bind into array to send as json
$arr = Array();
$arr["friendData"] = Array();

while ($stmt->fetch())
{
    $JSONFriendData = array (
        "sUsername" => $sUsername,
        "dBirthday" => $dBirthday,
        "iCurrentTitleID" => $iCurrentTitleID,
        "sBiography" => $sBiography,
        "iLevel" => $iLevel,
        "iDepartment" => $iDepartment,
        "iGuildID" => $iGuildID,
        "iCountry" => $iCountry,
        "iFriendshipLevel" => $iFriendshipLevel,
        "iFriendshipXP" => $iFriendshipXP
    );
    array_push($arr["friendData"], $JSONFriendData);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>