<?php //UpdateXPLevels.php
//Connect database 
require("dbconn_inc.php"); // include the external file

//Check if POST fields are received
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

//Set new timezone
date_default_timezone_set('UTC');
$sDateTime = date("y-m-d H:i:s");

$query = "update tb_account set dtLastLogin=? where UID=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("si", $sDateTime, $uid);

//Execute statement
$stmt->execute();
$stmt->fetch();
//Returns number of rows updated
http_response_code(200);
echo "Num rows updated:$stmt->affected_rows";
$stmt->close();

$conn->close();
?>