<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //check if POST fields are received, else quit
    if (!isset($_POST["iGuildID"]))
        throw new Exception("not posted!"); 
    $iGuildID = $_POST["iGuildID"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

// Prepare statement to get the necessary stats from tb_account
$query = "select sUsername from tb_account where iGuildID=? order by UID asc";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iGuildID);
$stmt->execute();
$stmt->bind_result($sUsername);

//Bind into array to send as json
$arr = Array();
$arr["guildMembers"] = Array();

while ($stmt->fetch())
{
    $JSONGuildMember = array (
        "sUsername" => $sUsername
    );
    array_push($arr["guildMembers"], $JSONGuildMember);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>