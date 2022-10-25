    <?php //Login.php
// Connect database
require("dbconn_inc.php");

try
{
    //check if POST fields are received, else quit
    if(!isset($_POST["sEmail"])||!isset($_POST["sPassword"]))
        throw new Exception("not posted!");
    $sEmail=$_POST["sEmail"];
    $sPassword=$_POST["sPassword"];
}
catch (Exception $e)
{
    http_response_code(400);
    echo 'Caught exception: ', $e->getMessage();
    die();
}

$sPassword = hash("sha256", $sPassword);

//Get the hashed password
$sHashedPassword = require "GetPasswordStatement.php";

//Password check
// if (!password_verify($sPassword, $sHashedPassword))
//     die ("Login failed.");

if ($sPassword != $sHashedPassword)
{
    http_response_code(400);
    echo "Wrong email or password. Login failed.";
    die();
}

//$sPassword = $sHashedPassword;

//Get the uid
$uid = require "GetUidStatement.php";

$conn->close(); // Close connection

//Fail login if the uid does not exist
if ($uid == null)
{
    http_response_code(400);
    echo "Login failed. User doesnt exist.";
}
else
{
    http_response_code(200);
    echo $uid;
}
?>