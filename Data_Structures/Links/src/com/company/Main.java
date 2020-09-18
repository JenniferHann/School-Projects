/*
*   Copyright (c) 2020 Jennifer Hann. All rights reserved.
*/

package com.company;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

/*
*   Find the last person in a circle to be removed using a process of counting and removing,
*   switching between the clockwise and counter-clockwise, and determine the order of each
*   person is removed.
*
*   @author Jennifer Hann
*/

public class Main {

    //class design by Ian Clement
    private static class DoubleLink<T> {
        //fields
        public T element;              //element the link will hold
        public DoubleLink<T> next;     //next element to current element
        public DoubleLink<T> prev;     //previous element to current element

        //constructors
        public DoubleLink(){}
        public DoubleLink(T element) {
            this.element = element;
            next = null;
            prev = null;
        }
    }

    //enter data using BufferedReader
    public static BufferedReader userInput = new BufferedReader(new InputStreamReader(System.in));

    public static void main(String[] args) throws IOException {

        int n;    //number of elements that will be link
        int m;    //move clockwise to the m-th position start counting from current position
        int o;    //move counter clockwise to the m-th position start counting from current position

        System.out.print("n> ");                      //ask user for input
        String input = userInput.readLine();          //reading data using readLine
        n = range(validation(input), 1);    //validate input for number and within required range

        System.out.print("m> ");
        input = userInput.readLine();
        m = range(validation(input), 2);

        System.out.print("o> ");
        input = userInput.readLine();
        o = range(validation(input), 2);

        //check if there is at least a remove in one direction
        if(m+o <= 0){
            System.out.println("Error: need to move in at least one direction");
            System.out.println("Enter another number for o");      //ask user for input
            input = userInput.readLine();                          //get input
            o = range(validation(input), 1);             //validate input
        }

        //build the links with requested number of elements
        DoubleLink<Integer> current = build(n);

        //allow removal process to switch between clockwise and counter-clockwise
        boolean isClockwise;
        isClockwise = true;

        //keep removing until no more element in circle
        while (current.next != null && n > 0){

            if(m == 0) {   //only remove in clockwise direction
                current = move(o, current, false);
            } else if (o == 0) {  //only remove in counter-clockwise direction
                current = move(m, current, true);
            }
            else {   //alternating between clockwise and counter-clockwise for removal
                if(isClockwise){
                    current = move(m, current, true); //clockwise
                    isClockwise = false;   //switch to go counter-clockwise next iteration
                }
                else {
                    current = move(o, current, false); //counter-clockwise
                    isClockwise = true;    //switch to go clockwise next iteration
                }
            }

            n = remove(current, n);     //remove the element at current position
            current = current.next;     //place position to new element
        }
    }

    //check user input for a number
    public static int validation(String string) throws IOException {

        int num;   //user input in int

        try{
            num = Integer.parseInt(string);    //converting from string to int
        }
        catch (NumberFormatException e) {   //input wasn't a number
            System.out.println("Invalid input: enter a number");
            num = -1;
            while(num == -1) {    //keep asking user until a number is given
                string = userInput.readLine();
                num = validation(string);
            }
        }
        return num;
    }

    //check if user's input is within range
    public static int range(int num, int inputCase) throws IOException {

        //input needs to be a positive number including 0
        if(inputCase == 1){
            while (num <= 0){
                System.out.println("Out of range: enter a positive number (0 inclusive)");
                String tmp = userInput.readLine();   //get input
                num = validation(tmp);               //check if it's a number
            }
        }
        else if(inputCase == 2){     //input needs to be above 0
            while (num < 0){
                System.out.println("Out of range: enter a number above 0");
                String tmp = userInput.readLine();
                num = validation(tmp);
            }
        }
        return num;
    }

    //create a circle containing elements
    public static DoubleLink<Integer> build(int n) {

        //first element of the circle
        DoubleLink<Integer> head;
        head = new DoubleLink<>(1);

        //tracker of current position in circle
        DoubleLink<Integer> current = head;

        //linking current element to the next element
        for(int i = 2; i <= n; i++){
            current.next = new DoubleLink<>();    //create new element
            current = current.next;               //step to next element
            current.element = i;                  //assigning element
        }

        //linking last element to first to create the circle
        current.next = head;

        //linking previous element to current element
        for(int i = 0; i < n; i++){
            DoubleLink<Integer> tmp = current;    //saving current element to be linked
            current = current.next;               //step to next element
            current.prev = tmp;                   //link current element to previous element
        }

        return head;    //return first element in circle
    }

    //move current position to the correct position for removal of element
    public static DoubleLink move (int m, DoubleLink<Integer> position, boolean direction) {

        if(m > 1){  //only move position when element to be removed is not the first

            //assign direction to move, either clockwise or counter-clockwise
            //step to the second position
            if(direction)
                position = position.next;  //clockwise
            else
                position = position.prev;  //counter-clockwise

            //position required to move is greater than 2
            if(m > 2) {
                //loop for the rest of the required times
                for(int i = 0; i < m-2; i++){
                    //assign direction to move, either clockwise or counter-clockwise
                    //step to the second position
                    if(direction)
                        position = position.next;
                    else
                        position = position.prev;
                }
            }
            //print out element to be removed
            System.out.print(position.element + " ");
        }
        //return new position after moving
        return position;
    }

    //remove the current element
    public static int remove (DoubleLink element, int size) {

        //current element is the element to be removed
        //link previous element to the current element to the element following current element
        element.prev.next = element.next;
        //link the element after current element to the previous element of the current element
        element.next.prev = element.prev;

        //step to the new element
        element = element.next;

        //reduce the number of element in the circle
        size--;
        return size;
    }
}
