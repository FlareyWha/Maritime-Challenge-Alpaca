<?php //ReadPlayerStats.php
//Connect database 
require("dbconn_inc.php"); // include the external file

// Prepare statement to get the necessary stats from tb_account
$query = "select iAbandonedCityID, sAbandonedCityName, iAbandonedCityAreaCellWidth, iAbandonedCityAreaCellHeight, fAbandonedCityXPos, fAbandonedCityYPos, iCapturedGuildID from tb_abandonedCity order by iAbandonedCityID asc";
$stmt = $conn->prepare($query);

$stmt->execute();

$stmt->bind_result($iAbandonedCity, $sAbandonedCityName, $iAbandonedCityAreaCellWidth, $iAbandonedCityAreaCellHeight, $fAbandonedCityXPos, $fAbandonedCityYPos, $iCapturedGuildID);

//Bind into array to send as json
$arr = Array(); //Create main array
$arr["abandonedCities"] = Array(); //Create the MAIN associate array item

// Read results one by one
while ($stmt->fetch())
{
	$JSONAbandonedCity = array( //create associative array for each record
		"iAbandonedCity" => $iAbandonedCity,
		"sAbandonedCityName" => $sAbandonedCityName,
        "iAbandonedCityAreaCellWidth" => $iAbandonedCityAreaCellWidth,
        "iAbandonedCityAreaCellHeight" => $iAbandonedCityAreaCellHeight,
        "fAbandonedCityXPos" => $fAbandonedCityXPos,
        "fAbandonedCityYPos" => $fAbandonedCityYPos,
        "iCapturedGuildID" => $iCapturedGuildID
	);
	array_push($arr["abandonedCities"], $JSONAbandonedCity); //add to index array
}

http_response_code(200); //tell the client everything ok
echo json_encode($arr);

$stmt->close();
$conn->close();
?>