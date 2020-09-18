<?php
    $_SERVERNAME = "10.39.167.244";
    $_DATABASE = "Hann-Company";
    $_TABLENAME = "EMPLOYEES";
    $_USERNAME = "Hann";
    $_PASSWORD = "1867219rdsw";

    $conn;
    function fetchEmployeeRecords()
    {
        global $_SERVERNAME, $_DATABASE, $_TABLENAME, $_USERNAME, $_PASSWORD, $conn;

        $connectionOptions = array("Database" => $_DATABASE, "UID" => $_USERNAME, "PWD" => $_PASSWORD);
        $conn = null;
        $conn = sqlsrv_connect($_SERVERNAME, $connectionOptions);
        if($conn == (false))
        {
            die(print_r( serialize(sqlsrv_errors()), true));
        }
    }

    fetchEmployeeRecords();
?>