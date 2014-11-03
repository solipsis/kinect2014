<?php
/*
	Kinect Score API
	
	Valid Calls:
	
	requestScores
		gameName(string)	Name of the game to get scores for
		startPos(string)	First position to get (In order Descending)
		count				Number of scores to retrieve
	
	submitScore
		gameName(string)	Name of the game to set score for
		value(Numeric)		Score to set
		userName(string)	Name of the user that obtained that score
*/
	//All DB connection info goes here
	function mysqli_connect_db()
	{
		$conn = new mysqli("localhost", "user", "pass", "db");
		if($conn->connect_error)
		{
			ErrorQuit(2, "Can't connect to DB");
		}
		return $conn;
	}	
	
	function ErrorQuit($code, $msg)
	{
		echo '{ "ErrCode": '.$code.', "ErrMsg": "'.$msg.'" }';
		exit;
	}
	
	function Success($data)
	{
		echo '{ "ErrCode": 0, "ErrMsg": "Success", "ScoreSet":'. json_encode($data).'}';
	}
	
	$mName = (isset($_GET["methodName"]))? $_GET["methodName"] : "";
	
	header('Content-Type: application/json');
	
	switch($mName)
	{
		case "requestScores":
			//Check passed params
			if(!isset($_GET["gameName"]) && !isset($_GET["startPos"]) && !isset($_GET["count"]))
			{
				ErrorQuit(3, "Invalid Request Score Parameters");
			}
			//Connect to db
			$db = mysqli_connect_db();
	
			$q = $db->prepare(
			
				"SELECT Kinect_Scores.Name, Kinect_Scores.Value ".
				"FROM Kinect_Scores ".
				"INNER JOIN Kinect_Games ON Kinect_Scores.GameID = Kinect_Games.id ".
				"WHERE Kinect_Games.Name = ? ".
				"ORDER BY Kinect_Scores.Value DESC ".
				"LIMIT ?, ? "
				
			);
			if(!$q)
			{
				ErrorQuit(4, "Failed to query scores");
			}		
			
			$q->bind_param("sii", $_GET["gameName"], $_GET["startPos"], $_GET["count"]);
			$q->execute();
			$q->bind_result($name, $value);
			
			$res = array();
			
			while ($q->fetch()) 
			{
				$res[] = array("Name"=>$name, "Value"=>$value);
			}
			$q->close();
			
			Success($res);
			
			exit;
			break;
		
		case "submitScore":
			//Check passed params
			if(!isset($_GET["gameName"]) && !isset($_GET["userName"]) && !isset($_GET["value"]))
			{
				ErrorQuit(5, "Invalid Submit Score Parameters");
			}
			//Connect to db
			$db = mysqli_connect_db();
	
			$q = $db->prepare(
				"INSERT INTO Kinect_Games (Name) VALUES (?)"		
			);
			if(!$q)
			{ ErrorQuit(6, "Failed to Lookup Game"); }		
			
			$q->bind_param("s", $_GET["gameName"]);
			$q->execute();
			$q->close();
			
			$q2 = $db->prepare(
				"INSERT INTO Kinect_Scores (Name, Value, GameID) ".
				"VALUES (?, ?, (SELECT id FROM Kinect_Games WHERE Kinect_Games.Name = ?))"		
			);
			if(!$q2)
			{ ErrorQuit(7, "Failed to Insert score"); }		
			
			$q2->bind_param("sis", $_GET["userName"], $_GET["value"],$_GET["gameName"]);
			$q2->execute();
			$q2->close();
			
			Success(null);
			
			exit;
			break;
		
		default:
			ErrorQuit(1, "Invalid Method");
			break;
	}



?>