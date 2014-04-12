<?php
//Variables for connecting to your database.
//These variable values come from your hosting account.
$hostname = "DantaDigitalData.db.4340366.hostedresource.com";
$username = "DantaDigitalData";
$dbname = "DantaDigitalData";

//These variable values need to be changed by you before deploying
$password = "Amsterd@m11";
$usertable = "QuarksScores";

$decryptedData = decryptString($_GET["data"],2);
$jsonData = json_decode($decryptedData);

$verificationQuery = 'select * from QuarksScores where playerName = "' . $jsonData->name . '" and scoreValue = ' . $jsonData->score;

//Connecting to your database
mysql_connect($hostname, $username, $password) OR DIE ("Unable to 
connect to database! Please try again later.");
mysql_select_db($dbname);

$result = mysql_query($verificationQuery);

if(!$result || mysql_num_rows($result)==0)
{
	$query = 'insert into QuarksScores values(null,"' . $jsonData->name . '",' . $jsonData->score . ',1,1)';
	$result = mysql_query($query);
	if($result)
		echo "Submit succesful!";
}
else
{
	echo "Player and score repeated, ignoring submission";
}



function decryptString($text,$displacement)
{
	//echo "text: " . $text . "<br>";
	$decodedString = urldecode($text);
	//echo "decoded: " . $decodedString . "<br>";
	$byteArray = unpack('C*',$decodedString);
	$byteCount = count($byteArray);
	//echo "byteArray: " . var_dump($byteArray) . "<br>";
	foreach($byteArray as &$singleByte)
	{
		$singleByte = $singleByte - $displacement;
	}
	//echo "byteArray: " . var_dump($byteArray) . "<br>";
	return call_user_func_array("pack", array_merge(array("C*"),$byteArray));
}



?>
            
