<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

//check if POST fields are received, else quit
try
{
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
$query = "select iOtherUID, sUsername, bOtherUnlocked from tb_phonebook inner join tb_account on tb_phonebook.iOtherUID=tb_account.UID where iOwnerUID=?";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($iOtherUID, $sUsername, $bOtherUnlocked);

//Bind into array to send as json
$arr = Array();
$arr["phonebookData"] = Array();

while ($stmt->fetch())
{
    $JSONPhonebookData = array (
        "iOtherUID" => $iOtherUID,
        "sUsername" => $sUsername,
        "bOtherUnlocked" => $bOtherUnlocked
    );
    array_push($arr["phonebookData"], $JSONPhonebookData);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>