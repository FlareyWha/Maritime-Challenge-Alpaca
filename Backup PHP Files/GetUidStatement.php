<?php //GetUidStatement.php
//Prepare statement to get the uid that uses the email and password 
$query="select uid from tb_account where sEmail=? and sPassword=?";
$stmt_uid=$conn->prepare($query);
$stmt_uid->bind_param("ss", $sEmail, $sPassword);
$stmt_uid->execute();
$stmt_uid->bind_result($uid);
$stmt_uid->fetch(); 
$stmt_uid->close();

return $uid;
?>