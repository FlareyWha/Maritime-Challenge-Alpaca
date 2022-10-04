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
$query = "select sGuildName, sGuildDescription, iOwnerUID from tb_guild where iGuildID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iGuildID);
$stmt->execute();
$stmt->bind_result($sGuildName, $sGuildDescription, $iOwnerUID);

//Bind into array to send as json
$arr = Array();
$arr["guildInfo"] = Array();

while ($stmt->fetch())
{
    $JSONGuildInfo = array (
        "sGuildName" => $sGuildName,
        "sGuildDescription" => $sGuildDescription,
        "iOwnerUID" => $iOwnerUID
    );
    array_push($arr["guildInfo"], $JSONGuildInfo);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>