<?php //AddPhonebookData.php
//require("dbconn_inc.php"); // include the external file

$uid = require "GetUidStatement.php";

// try
// {
//     //Check if POST fields are received
//     if (!isset($_POST["UID"]))
//         throw new Exception("not posted!");
//     $uid = $_POST["UID"];
// }
// catch (Exception $e)
// {
//     http_response_code(400);
//     echo 'Caught exception: ', $e->getMessage();
//     die();
// }

// echo $uid;

//Get all the uids that are not this uid
$query="select UID from tb_account where UID!=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("i", $uid);
$stmt->execute();
$stmt->bind_result($allOtherUid);

$allOtherUidArray = Array();

while ($stmt->fetch())
{
    array_push($allOtherUidArray, $allOtherUid);
}

$stmt->close();

// echo $allOtherUidArray;

// // $allOtherUidArray = json_decode($allOtherUid);

// // echo $allOtherUidArray;

foreach ($allOtherUidArray as $otherUid)
{
    //Insert all other people into the owners phonebook
    $query="insert into tb_phonebook values (?, ?, 0)";
    $stmt=$conn->prepare($query);
    $stmt->bind_param("ii", $uid, $otherUid);
    $stmt->execute();
    $stmt->close();

    //Insert owner into all other peoples phonebook
    $query="insert into tb_phonebook values (?, ?, 0)";
    $stmt=$conn->prepare($query);
    $stmt->bind_param("ii", $otherUid, $uid);
    $stmt->execute();
    $stmt->close();
}

?>