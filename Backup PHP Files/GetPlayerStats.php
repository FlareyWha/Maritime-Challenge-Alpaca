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
$query = "select iEnemiesDefeated, iBossesDefeated, iFriendsAdded, iRightshipediaEntriesUnlocked, iBattleshipsOwned, iCosmeticsOwned, iTitlesUnlocked, iAchievementsCompleted, iLogin, iChatMessagesSentDaily, iChatMessagesSentWeekly, iGiftsSentDaily, iGiftsSentWeekly, iProfilesViewed from tb_playerStats where iOwnerUID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);
$stmt->execute();
$stmt->bind_result($iEnemiesDefeated, $iBossesDefeated, $iFriendsAdded, $iRightshipediaEntriesUnlocked, $iBattleshipsOwned, $iCosmeticsOwned, $iTitlesUnlocked, $iAchievementsCompleted, $iLogin, $iChatMessagesSentDaily, $iChatMessagesSentWeekly, $iGiftsSentDaily, $iGiftsSentWeekly, $iProfilesViewed);

//Bind into array to send as json
$arr = Array();
$arr["playerStats"] = Array();

while ($stmt->fetch())
{
    $JSONPlayerStats = array (
        "iEnemiesDefeated" => $iEnemiesDefeated,
        "iBossesDefeated" => $iBossesDefeated,
        "iFriendsAdded" => $iFriendsAdded,
        "iRightshipediaEntriesUnlocked" => $iRightshipediaEntriesUnlocked,
        "iBattleshipsOwned" => $iBattleshipsOwned,
        "iCosmeticsOwned" => $iCosmeticsOwned,
        "iTitlesUnlocked" => $iTitlesUnlocked,
        "iAchievementsCompleted" => $iAchievementsCompleted,
        "iLogin" => $iLogin,
        "iChatMessagesSentDaily" => $iChatMessagesSentDaily,
        "iChatMessagesSentWeekly" => $iChatMessagesSentWeekly,
        "iGiftsSentDaily" => $iGiftsSentDaily,
        "iGiftsSentWeekly" => $iGiftsSentWeekly,
        "iProfilesViewed" => $iProfilesViewed
    );
    array_push($arr["playerStats"], $JSONPlayerStats);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>