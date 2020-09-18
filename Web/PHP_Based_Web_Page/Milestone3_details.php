<?php
    require_once("ConnectDB.php");
    require_once("Fetch&ShowRecords.php");

    $button = $_POST["button"];
    $pk = $_POST["pk"];
    $_TABLENAME = 'Details';
    
    if($button == "Submit")
    {
        $pk2 = $_POST["pk2"];
        $qty = $_POST["2"];
        $unitPrice = $_POST["3"];

        $data = update($_TABLENAME, $pk, $pk2, $qty, $unitPrice);
        showRecords($data[0], $data[1], TRUE, FALSE, FALSE, TRUE);
    }
    else if($button == "Go Back")
    {

        $fields = '*';
        $condition = "ONo = '".$pk."'";
    
        $data = fetchAllRecords($_TABLENAME, $fields, $condition);
        showRecords($data, $_TABLENAME, FALSE, FALSE, FALSE, TRUE);
    }

?>