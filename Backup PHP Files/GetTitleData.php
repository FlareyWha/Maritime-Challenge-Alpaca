<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

// // Prepare statement to get the necessary stats from tb_account
// $query = "select iTitleID, sTitleName from tb_title order by iTitleID asc";
// $stmt = $conn->prepare($query);
// $stmt->bind_param("i", $uid);
// $stmt->execute();
// $stmt->bind_result($iTitleID, $sTitleName);

// //Bind into array to send as json
// $arr = Array();
// $arr["titleData"] = Array();

// while ($stmt->fetch())
// {
//     $JSONTitleData = array (
//         "iTitleID" => $iTitleID,
//         "sTitleName" => $sTitleName
//     );
//     array_push($arr["titleData"], $JSONTitleData);
// }

// http_response_code(200);
// echo json_encode($arr);

// $stmt->close();

try
{
    //check if POST fields are received, else quit
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

//Prepare statement to get the necessary stats from tb_account
$query = "select tb_title.iTitleID, sTitleName, bTitleUnlocked from tb_title inner join tb_titleList on tb_title.iTitleID=tb_titleList.iTitleID where iOwnerUID=? and tb_title.iTitleID > 0 order by tb_title.iTitleID asc";
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $iOwnerUID);
$stmt->execute();
$stmt->bind_result($iTitleID, $sTitleName, $bTitleUnlocked);

//Bind into array to send as json
$arr = Array();
$arr["titleData"] = Array();

while ($stmt->fetch())
{
    $JSONTitleData = array (
        "iTitleID" => $iTitleID,
        "sTitleName" => $sTitleName,
        "bTitleUnlocked" => $bTitleUnlocked
    );
    array_push($arr["titleData"], $JSONTitleData);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>