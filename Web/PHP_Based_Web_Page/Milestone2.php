<?php
    require_once("ConnectDB.php");
    require_once("Fetch&ShowRecords.php");

    global $_TABLENAME;
    $mod;
    $var;

    $senderLocation = $_POST["sender"];
    $allowEdit = TRUE;
    if($senderLocation == "BD")
    {
        $mod = $_POST["modify"];
        $tableName = $_POST["tName"];
        $pk = $_POST["pk"];

        if($mod == 'delete')
        {
            $data = delete($tableName, $pk);
            showRecords($data[0], $data[1], FALSE, FALSE, FALSE, FALSE);
        }
    }
    else if($senderLocation == "M1")
    {
        $var = $_POST["edit"];

        if($var == 'Orders')
        {
            $data = fetchAllRecords($var, '*', '1=1');
            showRecords($data, $_TABLENAME, TRUE, FALSE, FALSE, FALSE);
        }
        else
        {
            echo "Feature coming soon!";
        }
    }

    
    
?>