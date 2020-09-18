<?php
    require_once("ConnectDB.php");
    require_once("Fetch&ShowRecords.php");

    $pk = $_POST["pk"];

    $_TABLENAME = 'Details';
    $fields = '*';
    $condition = "ONo = '".$pk."'";

    $data = fetchAllRecords($_TABLENAME, $fields, $condition);
    showRecords($data, $_TABLENAME, FALSE, FALSE, TRUE);

    
?>