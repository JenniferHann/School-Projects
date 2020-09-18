package ca.qc.johnabbott.cs406;

import com.sun.xml.internal.org.jvnet.mimepull.CleanUpExecutorFactory;

import java.awt.*;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

/**
 * Create a trie to store a collection of words (a lexicon) and check is a word is in the trie
 *
 * @author Jennifer Hann
 */
public class Trie <T extends Trie.Node> implements Lexicon  {

    //an entity that will hold a prefix and its subtrees that represent
    // the words of the lexicon that start with a certain letter
    public class Node<T> {
        public String elememt;         //prefix of a lexicon
        public List<Node> branches;    //collection of letters that can follow a prefix
        public boolean lastLetter;     //check if its the last letter of the lexicon

        //constructor
        public Node () {
            this.branches = new ArrayList<>();    //initiate the array to save all proceeding letters of a prefix
            this.elememt = "";                    //initiate the prefix, if prefix is "" then its the root/ first node
            this.lastLetter = false;              //initiate boolean, if false: current letter is not the last letter of the lexicon
                                                  // if true: this letter is the last letter of the lexicon
        }

        //representing the node in a string for debugging
        //display the important fields of the node
        @Override
        public String toString() {
            //string array that will show all the collection of letters that can follow a prefix
            String[] babyBranches = new String[root.branches.size()];

            int count = 0;    //counter for changing the cell in the string array

            //saving the elements in the collection of letter that can follow a prefix into a string array
            for(Node l : this.branches) {
                babyBranches[count] = l.elememt;    //saving the element
                count++;                            //next cell to save
            }

            //return the string with all important fields
            return "Node{" +
                    "elememt='" + elememt + '\'' +
                    ", branches=" + Arrays.toString(babyBranches) +
                    '}';
        }
    }

    //first node that start the trie
    private Node<T> root;

    //collection of letters (alphabet) to be the first collection of letters that follows the prefix of the root
    private String[] alphabet = "abcdefghijklmnopqrstuvwxyz".split("");

    //constructor
    public Trie(int alphabetLength) {

        //initiate the root
        root = new Node();

        //adding the first subtrees to the root
        for(int i=0; i<alphabetLength; i++) {
            Node letter = new Node();             //creating new node to be added to the list
            letter.elememt = alphabet[i];         //set proper letter to the node
            root.branches.add( (T) letter);       //add the node to the subtree of the root
        }
    }

    //string representation of the trie
    @Override
    public String toString() {

        //array of string that will save the first subtree of a prefix
        String[] branches = new String[root.branches.size()];

        //counter for the array of string
        int count = 0;

        //iterate over each element in the subtree
        for(Node l : root.branches) {
            branches[count] = l.elememt;    //save the element from the subtree
            count++;                        //increment counter for the next saving location
        }

        return "Trie{" +
                " root.element=" + root.elememt +
                ", root.branches=" + Arrays.toString(branches) +
                '}';
    }

    //add a word to the trie
    @Override
    public void add(String word) {

        char[] letters = word.toCharArray();     //break up the word into letter to be added in the trie structure
        int counter = 0;                         //counter that keeps track of which letter is being added

        //call method that will add the word
        addHelper(letters, counter, root);
    }

    //add each letter to its appropriate spot in the trie
    //takes an array of the letter that compose the word
    //takes the counter that keeps track of which letter is being added
    //takes the current location in the trie structure
    private void addHelper(char[] letters, int currentLetter, Node<T> current) {

        boolean exist = false;      //check if the letter already exist in the wanted position in the trie
        Node initial = current;     //save the initial position in the trie

        //iterate over each element in the subtree
        for (Node branch : current.branches) {

            //change the current position into the new prefix(node)
            current = branch;

            //check if the letter that we want to add was already added previously in the current position
            if(String.valueOf(letters[currentLetter]).equals(branch.elememt)) {
                exist = true;    //letter already there
                break;           //leave the loop
            }
        }

        //first time the letter is added to a subtree
        if(!exist) {
            current = initial;                        //change current position in the trie to the initial position(prefix)
            Node newBranche = new Node();             //create a new node(prefix)
            newBranche.elememt = String.valueOf(letters[currentLetter]);    //convert the char into a string to be save as a new prefix in the node
            current.branches.add(newBranche);         //add the letter to the subtree
            current = newBranche;                     //change the current position to the new letter in order to add the next letter
        }

        currentLetter = currentLetter + 1;     //get the next letter
        //check if there are still more letters to be added
        if(currentLetter < letters.length) {
            addHelper(letters, currentLetter, current);   //add the next letter
        }
        else {
            current.lastLetter = true;   //this is the last letter of the lexicon
        }
    }

    //check if a word is a lexicon
    @Override
    public boolean contains(String word) {

        char[] letters = word.toCharArray();     //get the letters of the word
        boolean found = false;                   //check if the word was found, if true: word is a lexicon, if false: word is not a lexicon
        int currentLetter = 0;                   //keep track of the letter that is being checked in the trie

        //check if the word is a lexicon by looking at each letter
        found = containsHelper(letters, currentLetter, root);

        return found;
    }

    //check if each letter in the word is found in the trie
    //takes an array that contains all the letters of the word
    //takes the counter that keeps track of which letter is being checked
    //takes the current position of node in the trie
    private boolean containsHelper(char[] letters, int currentLetter, Node<T> current) {

        boolean found = false;   //boolean that keeps track if the word was found or not

        //iterate over all element in the subtree of the current node(prefix)
        for (Node branch : current.branches) {

            //check if the letter is found in the subtree
            if(String.valueOf(letters[currentLetter]).equals(branch.elememt)) {
                found = true;          //letter was found
                current = branch;      //change position to check the next letter
                currentLetter = currentLetter + 1;    //get the next letter

                //check if there are still letters to be checked
                if( currentLetter < letters.length) {
                    found = containsHelper(letters, currentLetter, current);   //go check the next letter
                }

                //check if the word is an actually lexicon and not a beginning of a lexicon
                if(current.lastLetter == false && currentLetter == letters.length) {
                    found = false;
                }
                break;    //exit loop
            }
        }

        return found;
    }
}

