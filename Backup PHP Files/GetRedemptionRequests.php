<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

//Prepare statement to get the necessary stats from tb_account
$query = "select sUsername, sRedemptionItemName from tb_redemptionRequestList inner join tb_account on tb_account.UID=tb_redemptionRequestList.iOwnerUID inner join tb_redemptionItem on tb_redemptionItem.iRedemptionItemID=tb_redemptionRequestList.iRedemptionItemID order by iOwnerUID, tb_redemptionRequestList.iRedemptionItemID asc";
$stmt = $conn->prepare($query);
$stmt->execute();
$stmt->bind_result($sUsername, $sRedemptionItemName);

//Bind into array to send as json
$arr = Array();
$arr["redemptionRequests"] = Array();

while ($stmt->fetch())
{
    $JSONRedemptionRequest = array (
        "sUsername" => $sUsername,
        "sRedemptionItemName" => $sRedemptionItemName
    );
    array_push($arr["redemptionRequests"], $JSONRedemptionRequest);
}

http_response_code(200);
echo json_encode($arr);

$stmt->close();
$conn->close();
?>