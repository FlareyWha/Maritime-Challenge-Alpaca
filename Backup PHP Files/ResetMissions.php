<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iMissionType"]))
        throw new Exception("not posted!"); 
    $iMissionType = $_POST["iMissionType"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

$query = "update tb_missionList inner join tb_mission on tb_missionList.iMissionID=tb_mission.iMissionID set bMissionClaimed=false where iMissionType=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iMissionType);
$stmt->execute();
$stmt->fetch();
$battleshipUpdated = $stmt->affected_rows;
http_response_code(200);
echo "No of rows updated: $stmt->affected_rows";

$stmt->close();
$conn->close();
?>