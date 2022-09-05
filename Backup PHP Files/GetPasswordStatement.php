<?php //GetPasswordStatement.php
//Prepare statement to get the sPassword from tb_users which uses sEmail
$query="select sPassword from tb_account where sEmail=?";
$stmt_password=$conn->prepare($query);
$stmt_password->bind_param("s", $sEmail);
$stmt_password->execute();
$stmt_password->bind_result($sHashedPassword);
$stmt_password->fetch(); 
$stmt_password->close();

return $sHashedPassword;
?>