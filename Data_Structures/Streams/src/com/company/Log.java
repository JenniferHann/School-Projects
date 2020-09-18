package com.company;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * A log entry for the logfile in Asg #1
 * @author Jennifer Hann
 */
public class Log implements Comparable<Log> {
    //constant
    private static final int SIZE_OF_LOG_STRING = 5;

    // Fields
    private Date timestamp;
    private IPAddress ipAddress;
    private String serviceName;
    private int length;

    //formatting date
    private SimpleDateFormat formatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");
    private DateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSS");

    // Constructor
    public Log(String line) throws Exception {
        String[] entries = line.split("\\s+");

        //not enough entries for a proper log
        if(entries.length != SIZE_OF_LOG_STRING)
            throw new Exception("Error: Log entry is not valid");

        //assigning value to variables
        setIPaddress(entries[0]);
        setServiceName(entries[1]);
        setDate(entries[2], entries[3]);
        setLength(entries[4]);

    }

    // Getters
    public IPAddress getIPaddress(){
        return this.ipAddress;
    }

    public Date getDate(){
        return this.timestamp;
    }

    public String getServiceName(){
        return this.serviceName;
    }

    public int getLength(){
        return this.length;
    }

    // Setters
    private void setIPaddress(String ip) throws Exception {
        try {
            this.ipAddress = new IPAddress(ip);
        }
        catch (Exception e)
        {
            throw new Exception(e.getMessage());
        }
    }

    private void setDate(String date, String time) throws Exception {
        //formatting date to be stored as date
        String dateTmp;   //string that stores the date
        Date timeTmp;     //store the date in Date class format
        dateTmp = "";     //initialize variable
        try
        {
            dateTmp = date;
            dateTmp = dateTmp + " " + time;
            timeTmp = formatter.parse(dateTmp);     //format the date
            this.timestamp = timeTmp;
        }
        catch (Exception e)
        {
            throw new Exception("Error: Invalid Date "+ dateTmp);
        }
    }

    private void setServiceName(String serviceName){
        this.serviceName = serviceName;
    }

    private void setLength(String length) throws Exception {
        try {
            this.length = Integer.parseInt(length);  //converting from string to integer
        }
        catch (Exception e)
        {
            throw new Exception("Error: Length " + length + " is not an integer");
        }
    }

    //print the formatted log
    public String toString(){
        return this.ipAddress.toString() + " "+ this.serviceName + " " + dateFormat.format(this.timestamp) + " " + this.length;
    }

    //compare two IP address
    @Override
    public int compareTo(Log rhs) {
        //boolean to check
        int ipIsBigger = 0;
        int serviceNamefirst = 0;
        int dateFirst =0;
        int lengthBigger = 0;
        int lhsBigger = 0;

        //compare IP Address
        ipIsBigger = this.ipAddress.compareTo(rhs.ipAddress);

        //compare service name
        char[] charsL = this.serviceName.toCharArray(); //getting each letter of the word
        char[] charsR = rhs.serviceName.toCharArray();  //getting each letter of the word
        for(int i = 0; i < charsL.length; i++)
        {
            //comparing the ascii value of each letter of the word to determine the alphabetical order
            if((int) charsL[i] < (int) charsR[i] || (int) charsL[i] == (int) charsR[i])
            {
                serviceNamefirst = -1;  //first word came first
                break;
            }
            else if((int) charsL[i] > (int) charsR[i])
            {
                serviceNamefirst = 1;   //first word came second
                break;
            }
        }

        //compare time
        dateFirst = this.timestamp.compareTo(rhs.timestamp);

        //compare length
        if(this.length < rhs.length || this.length == rhs.length)
            lengthBigger = -1; //first word came first
        else
            lengthBigger = 1;

        //compare all elements
        if((ipIsBigger < 0) || (ipIsBigger == 0 && serviceNamefirst < 0) || (ipIsBigger == 0 && serviceNamefirst == 0 && dateFirst < 0)  || (ipIsBigger == 0 && serviceNamefirst == 0 && dateFirst == 0 && lengthBigger < 0))
            lhsBigger = -1;    //comes first
        else if (ipIsBigger == 0 && serviceNamefirst == 0 && dateFirst == 0 && lengthBigger == 0)
            lhsBigger = 0;     //both logs are the same
        else
            lhsBigger = 1;     //comes after
        return lhsBigger;
    }
}
