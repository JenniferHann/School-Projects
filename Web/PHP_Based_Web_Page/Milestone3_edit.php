<?php
    require_once("ConnectDB.php");
    require_once("Fetch&ShowRecords.php");

    $sender = $_POST["sender"];

    if($sender == "details")
    {

        $pk = $_POST["pk"];
    
        $_TABLENAME = 'Details';
        $fields = '*';
        $condition = "ONo = '".$pk."'";
    
        $data = fetchAllRecords($_TABLENAME, $fields, $condition);
        showRecords($data, $_TABLENAME, FALSE, FALSE, TRUE, TRUE);
    }
    else if($sender == "M1")
    {
        $_TABLENAME = $_POST["edit"];
        if( $_TABLENAME === "Orders")
        {
            if(isset($_POST["hi"]))
            {
                $pk = $_POST["hi"];
                echo "<form action='Milestone3_edit.php' method='POST'>
                    <input type='hidden' value='M3_edit' name='sender'>
                    <label>CNo: </label>
                    <br>
                    <input type='text' name='cno'></input>
                    <br>
                    <label>Order Date: </label>
                    <br>
                    <input type='text' name='orderD'></input>
                    <br>
                    <input type='hidden' name='tName' value='$_TABLENAME'></input>
                    <input type='hidden' name='pk' value='$pk'></input>
                    <button>Add New Item</button>
                    </form>";
            }
            else
                echo "hi";
        }
        else
            echo "Feature coming soon!";
    }
    else if($sender == "M3_edit")
    {
        $_TABLENAME = $_POST["tName"];
        $cno = $_POST["cno"];
        $orderD = $_POST["orderD"];
        $pk = $_POST["pk"];

        $pk++;
        echo $pk;

        //$data = insert($pk, $_TABLENAME, $cno, $orderD);
        //showRecords($data[0], $data[1], FALSE, FALSE, FALSE, FALSE);
    }

?>