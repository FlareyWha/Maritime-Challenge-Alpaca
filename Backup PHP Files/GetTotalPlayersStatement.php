<?php //GetTotalPlayersStatement.php
//Prepare statement to get toal number of players
$query="select count(UID) from tb_account";
$stmt_uid=$conn->prepare($query);
$stmt_uid->execute();
$stmt_uid->bind_result($iTotalPlayers);
$stmt_uid->fetch(); 
$stmt_uid->close();

return $iTotalPlayers;
?>