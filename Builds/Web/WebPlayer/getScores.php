<?php
//Variables for connecting to your database.
//These variable values come from your hosting account.
$hostname = "DantaDigitalData.db.4340366.hostedresource.com";
$username = "DantaDigitalData";
$dbname = "DantaDigitalData";

//These variable values need to be changed by you before deploying
$password = "Amsterd@m11";
$usertable = "highScores";
$yourfield = "scoreValue";

//Connecting to your database
mysql_connect($hostname, $username, $password) OR DIE ("Unable to 
connect to database! Please try again later.");
mysql_select_db($dbname);

//Fetching from your database table.
$query = "SELECT * FROM $usertable";
$result = mysql_query($query);
$firstRow=true;
if ($result) {
	$scoresJSON = "[";
    while($row = mysql_fetch_array($result)) {
       if($firstRow)
       {
       		$firstRow=false;
       }
       else
       {
       		$scoresJSON .= ',';
       }
       $scoresJSON .= '{"name":"' . $row["playerName"] . '","score":' . $row["scoreValue"] . '}';
    }
    $scoresJSON .= "]";
    echo $scoresJSON;
}
?>
            
