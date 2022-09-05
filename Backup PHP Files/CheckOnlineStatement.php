<?php //CheckOnlineStatement.php
//Prepare statement to update the iCoins for the account with uid
$query = "select bOnline from tb_account where sEmail=?";
$stmt=$conn->prepare($query);
$stmt->bind_param("s", $sEmail);
$stmt->execute();
$stmt->bind_result($bOnline);
$stmt->fetch();
$stmt->close();

return $bOnline;
?>