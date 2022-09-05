<?php //UpdateCoins.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //Check if POST fields are received
    if (!isset($_POST["UID"]) || !isset($_POST["iMissionID"]))
        throw new Exception("not posted!");
    $uid = $_POST["UID"];
    $iMissionID = $_POST["iMissionID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to update the iCoins for the account with uid
$query = "update tb_missionList set bMissionCompleted=true where UID=? and iMissionID=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("ii", $uid, $iMissionID);

//Execute statement
$stmt->execute();
$stmt->fetch();
//Returns number of rows updated
http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";
$stmt->close();

$conn->close();
?>