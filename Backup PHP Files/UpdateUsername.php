<?php //UpdateCoins.php
//Connect database 
require("dbconn_inc.php"); // include the external file

try
{
    //Check if POST fields are received
    if (!isset($_POST["UID"]) || !isset($_POST["sUsername"]))
        throw new Exception("not posted!");
    $uid = $_POST["UID"];
    $sUsername = $_POST["sUsername"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

//Prepare statement to update the sUsername for the account with uid
$query = "update tb_account set sUsername=? where uid=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("si", $sUsername, $uid);

//Execute statement
$stmt->execute();
$stmt->fetch();
//Returns number of rows updated
http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";
$stmt->close();

$conn->close();
?>