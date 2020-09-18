<?php 
    echo "<!DOCTYPE html>";
    echo "<html>";
    echo "<head>
            <link rel='stylesheet' type='text/css' href='style.css'>
            <title>Milestone 1</title>
            </head>";

    echo "<body>";

    require_once("ConnectDB.php");
    require_once("Fetch&ShowRecords.php");

    global $_TABLENAME;
    $allowEdit = FALSE;   

    $data = fetchAllRecords("Customers", '*', '1=1');
    showRecords($data, $_TABLENAME, $allowEdit, TRUE, FALSE, FALSE);

    $data = fetchAllRecords("Depts", '*', '1=1');
    showRecords($data, $_TABLENAME, $allowEdit, TRUE, FALSE, FALSE);
    
    $data = fetchAllRecords("Details", '*', '1=1');
    showRecords($data, $_TABLENAME, $allowEdit, TRUE, FALSE, FALSE);
    
    $data = fetchAllRecords("Employees", '*', '1=1');
    showRecords($data, $_TABLENAME, $allowEdit, TRUE, FALSE, FALSE);
    
    $data = fetchAllRecords("Items", '*', '1=1');
    showRecords($data, $_TABLENAME, $allowEdit, TRUE, FALSE, FALSE);
    
    $data = fetchAllRecords("Orders", '*', '1=1');
    showRecords($data, $_TABLENAME, $allowEdit, TRUE, FALSE, FALSE);
    
    $data = fetchAllRecords("Pets", '*', '1=1');
    showRecords($data, $_TABLENAME, $allowEdit, TRUE, FALSE, FALSE);
    
    $data = fetchAllRecords("Types", '*', '1=1');
    showRecords($data, $_TABLENAME, $allowEdit, TRUE, FALSE, FALSE);

    echo "</body>";
    echo "</html>";
?>