<?php //UpdateCoins.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //Check if POST fields are received
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

//Prepare statement to update the iCoins for the account with uid
$query = "select tb_mission.iMissionID, sMissionTitle, iMissionType, iMissionMaxRequirementNumber, bMissionClaimed from tb_missionList inner join tb_mission on tb_mission.iMissionID=tb_missionList.iMissionID where iOwnerUID=? order by tb_mission.iMissionID asc";
$stmt=$conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);

//Execute statement
$stmt->execute();
$stmt->bind_result($iMissionID, $sMissionName, $iMissionType, $iMissionMaxRequirementNumber, $bMissionClaimed);

//Bind into array to send as json
$arr = Array();
$arr["missionData"] = Array();

while ($stmt->fetch())
{
    $JSONMissionData= array (
        "iMissionID" => $iMissionID,
        "sMissionName" => $sMissionName,
        "iMissionType" => $iMissionType,
        "iMissionMaxRequirementNumber" => $iMissionMaxRequirementNumber,
        "bMissionClaimed" => $bMissionClaimed
    );
    array_push($arr["missionData"], $JSONMissionData);
}

http_response_code(200);
echo json_encode($arr);
$stmt->close();
$conn->close();
?>