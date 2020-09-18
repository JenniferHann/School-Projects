<?php
    function fetchAllRecords($tableName, $fields, $condition)
    {
        global $_TABLENAME, $conn;

        $_TABLENAME = $tableName;

        $query = "SELECT $fields FROM $_TABLENAME WHERE $condition";

        $result = sqlsrv_query($conn, $query)
            or die("ERROR: Query is wrong");

        return $result;
    }

    function showRecords($record, $tableName, $edit, $editTable, $updateCell, $oTotal)
    {
        
        echo "<h1>$tableName</h1>";

        if($editTable == TRUE)
        {
            echo "<form action='Milestone2.php' method='POST'>
            <input type='hidden' value='M1' name='sender'>
            <button name='edit' value='$tableName'>Table Edit</button>
            </form>";
            echo "<form action='Milestone3_edit.php' method='POST' id='form2'>
            <input type='hidden' value='M1N' name='sender'>
            <button name='edit' value='$tableName'>Add</button>
            </form>";
        }

        $pkCell = [];
        $pkCount = 0;
        $count = 0;
        echo "<table border=1>";
        echo "<tr>";

        foreach(sqlsrv_field_metadata($record) as $fieldMetadata)
        {
            echo "<th>".$fieldMetadata['Name']."</th>";
            if($fieldMetadata['Name'] == 'ONo' || $fieldMetadata['Name'] == 'INo')
            {
                $pkCell[$pkCount] = $count;
                $pkCount++;
            }
            $count++;
        }
        if($oTotal == TRUE)
        {
            echo "<th>Order Total</th>";
        }
        echo "</tr>";
        
        while($row = sqlsrv_fetch_array($record, SQLSRV_FETCH_ASSOC))
        {
            $pk = 0;
            $pk2 = 0;
            $count = 0;
            $generalCount = 0;
            $orderTotal = 0;
            $q = 0;
            $uP = 0;
            echo "<tr>\n";
            foreach($row as $key=>$cell)
            {
                if(gettype($cell) == 'object')
                {
                    $cell = $cell->format('Y-m-d');
                    
                }
                else
                {
                    if($pk === 0)
                    {
                        $pk = $cell;
                        echo "<input type='hidden' name='hi' value='$cell' form='form2'>";
                    }
                    else if($pk2 === 0)
                    {
                        $pk2 = $cell;
                    }
                }
                

                if($updateCell == TRUE)
                {
                    if($count < count($pkCell))
                    {

                        for($i = 0; $i < count($pkCell); $i++)
                        {
                            if($count == $pkCell[$i])
                            {
                                echo "<td> $cell </td>";
                                break;
                            }
                        }
                    }
                    else
                    {
                        echo "<td>";
                        echo "<input type='text' name='$count' value='$cell' form='form1'>";
                        echo "</td>";
                        if($q === 0)
                        {
                            $q = $cell;
                        }
                        else if($uP === 0)
                        {
                            $uP = $cell;
                        }
                    }
                    $count++;
                    $orderTotal = $q * $uP;
                }
                else
                {
                    $generalCount++;
                    echo "<td> $cell </td>";
                    if($generalCount > 2)
                    {
                        if($q === 0)
                        {
                            $q = $cell;
                        }
                        else if($uP === 0)
                        {
                            $uP = $cell;
                        }
                    }
                    if($oTotal == TRUE)
                        $orderTotal = $q * $uP;
                    
                }
                if($oTotal == TRUE && $orderTotal != 0)
                {
                    echo "<td>$orderTotal</td>";
                }
            }
            if($edit == TRUE)
            {
                echo "<td>";
                echo "<form action='Milestone2.php' method='POST'>
                <input type='hidden' value='$tableName' name='tName'/>
                <input type='hidden' value='$pk' name='pk'/>
                <input type='hidden' value='BD' name='sender'>
                <button type='submit' value='delete' name='modify'>Delete</button>
                </form>";
                echo "</td>";
                
                echo "<td>";
                echo "<form action='Milestone3_edit.php' method='POST'>
                <input type='hidden' value='$pk' name='pk'/>
                <input type='hidden' value='details' name='sender'>
                <button type='submit'name='modify' value='edit'>Edit</button>
                </form>";
                echo "</td>";
            }
            echo "</tr>\n";
        }
        echo "</table>";

        if($updateCell == TRUE)
        {
            echo "<form action='Milestone3_details.php' method='POST' id='form1'>
            <input type='hidden' value='$tableName' name='tName'/>
            <input type='hidden' value='$pk' name='pk'/>
            <input type='hidden' value='$pk2' name='pk2'/>
            <button type='submit' value='Submit' name='button'>Submit</button>
            <button type='submit' value='Go Back' name='button'>Go Back</button>
            </form>";
        }
    }

    function delete($tName, $pk)
    {
        global $conn;
        $query = "DELETE FROM $tName WHERE ONo = ?";

        $result = sqlsrv_prepare($conn, $query, array(&$pk))
            or die("ERROR: Query is wrong");

        if(sqlsrv_execute($result) === false){
            die( print_r(sqlsrv_errors(), true));
        }

        $data = fetchAllRecords($tName, '*', '1=1');
        $info = [];
        $info[0] = $data;
        $info[1] = $tName;
        return $info;
    }

    function update($tName, $pk, $pk2, $qty, $unitPrice)
    {
        global $conn;
        $query = "UPDATE $tName SET Qty=?, UnitPrice=? WHERE ONo=? and INo=?";

        $result = sqlsrv_prepare($conn, $query, array(&$qty, &$unitPrice, &$pk, &$pk2))
            or die("ERROR: Query is wrong");

        if(sqlsrv_execute($result) === false){
            die( print_r(sqlsrv_errors(), true));
        }

        $data = fetchAllRecords($tName, '*', '1=1');
        $info = [];
        $info[0] = $data;
        $info[1] = $tName;
        return $info;
    }

    function insert($pk, $tName, $cno, $orderD)
    {
        global $conn;
        $query = "INSERT INTO $tName(ONo, CNo, OrderDate) VALUES (?, ?,?)";

        $result = sqlsrv_prepare($conn, $query, array(&$pk, &$cno, &$orderD))
            or die("ERROR: Query is wrong");

        if(sqlsrv_execute($result) === false){
            die( print_r(sqlsrv_errors(), true));
        }

        $data = fetchAllRecords($tName, '*', '1=1');
        $info = [];
        $info[0] = $data;
        $info[1] = $tName;
        return $info;
    }
?>