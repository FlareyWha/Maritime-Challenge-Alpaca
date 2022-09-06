<?php //UpdateXPLevels.php
//Connect database 
require("dbconn_inc.php"); // include the external file

//Check if POST fields are received
if (!isset($_POST["uid"]) || !isset($_POST["iXP"]) || !isset($_POST["iLevel"])) die("not posted!");
$uid = $_POST["uid"];
$iXP = $_POST["iXP"];
$iLevel = $_POST["iLevel"];

//Prepare statement to update the iXP and iLevel of the acccount with the uid
$query = "update tb_playerstats set iXP=?, iLevel=? where uid=?";
$stmt=$conn->prepare($query);

//s - string, i - integer...
$stmt->bind_param("iii", $iXP, $iLevel, $uid);

//Execute statement
$stmt->execute();
$stmt->fetch();
//Returns number of rows updated
echo "Num rows updated:$stmt->affected_rows";
$stmt->close();

$conn->close();
?>