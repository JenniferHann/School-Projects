package ca.qc.johnabbott.cs406.collections;

import java.util.Arrays;

/**
 * A Traversal API and Set API for Set in Asg #2
 * @author Jennifer Hann
 */
public class SortedSet<T extends Comparable<T>> implements Set<T> {

    //default capacity of the array of items that the set can have
    private static final int DEFAULT_CAPACITY = 100;

    private T[] elements;              //list that will hold the elements
    private int size;                  //count number of element in array
    private int cursor;                //counter for the traversal
    private boolean modified;          //check if array is being modified during the traversal

    //default constructor to create a sorted set
    public SortedSet() {
        this(DEFAULT_CAPACITY);
    }

    //constructor
    public SortedSet(int capacity) {
        this.size = 0;                 //initialize size to first index
        this.cursor = 0;               //initialize cursor to first index
        this.modified = false;         //array has no been modified yet
        this.elements = (T[]) new Comparable[capacity];   //initialize array
    }

    //check if element is found in the array using binary search
    //if found return true else false
    @Override
    public boolean contains(T elem) {
        // elements is sorted, so we can binary search for the element.
        return Arrays.binarySearch(elements, 0, size, elem) >= 0;
    }

    //check if elements of a list is found in the set
    //if found all return true else false
    @Override
    public boolean containsAll(Set<T> rhs) {
        if(!this.isEmpty() || !rhs.isEmpty())   //at least on of the sets are not empty
        {
            while (rhs.hasNext())               //loop through all elements in array parameter
            {
                if(!this.contains(rhs.next()))  //element is not found in the main set
                    return false;
            }
            return true;                        //all elements are found in the main set
        }
        else
            throw new EmptySetException();       //cannot be compared as one of the sets are empty
    }

    //add an element to the sorted set
    //return true if successfully added, else false
    @Override
    public boolean add(T elem) {
        //traversal is in process
        if(cursor > 0)
            modified = true;

        //set is already full and element is already in set
        if(isFull() && contains(elem))
            return false;

        //set is full
        if(isFull())
            throw new FullSetException();

        int position = 0;        //index of where added element will be
        int movingValueup;     //number of time needed to move elements to make space for new one
        if(!contains(elem))      //no duplicates
        {
            for(int i=0; i<size; i++)   //loop to find the position of new element
            {
                if(elem.compareTo(elements[i]) < 0)   //element is smaller than value of current index
                {
                    position = i;      //found index of new element
                    break;
                }
                else {
                    position = size;   //last index of array
                }
            }

            //number of elements that need to move to make place for new element
            movingValueup = size - position;
            //move needed elements up one index
            for(int i=0; i < movingValueup; i++)
            {
                elements[size-i] = elements[size-(i+1)];
            }
            //add the new element in proper index
            this.elements[position] = elem;
            size=size+1;    //number of element in array has increase
            return true;    //added successfully
        }
        else
            return false;   //element was not added
    }

    //remove an element from set
    //return true if successful, else false
    @Override
    public boolean remove(T elem) {
        //traversal in process
        if(cursor > 0)
            return modified = true;
        //element if found in the set
        if(contains(elem) == true)
        {
            int index = Arrays.binarySearch(elements, elem); //find the index
            //move all elements after removed element down by one index
            for(int i=index; i<size; i++)
            {
                elements[i] = elements[i+1];
            }
            size--;           //number of element in set decrease
            return true;      //removed successfully
        }
        else
            return false;     //element not found in set and cannot remove
    }

    //get size of the set
    @Override
    public int size() {
        return size;
    }

    //check if set is empty
    @Override
    public boolean isEmpty() {
        if(size == 0)
            return true;
        else
            return false;
    }

    //find smallest element in set
    public T min() {
        if(!isEmpty())
            return elements[0];
        else
            throw new EmptySetException();
    }

    //find largest element in set
    public T max() {
        if(!isEmpty())
            return elements[size-1];
        else
            throw new EmptySetException();
    }

    //get a subset of the set by providing where does the subset start and end
    public SortedSet<T> subset(T first, T last) {
        //set is empty
        if(this.isEmpty())
        {
            SortedSet <T> emptySet;
            return emptySet = new SortedSet<T>();
        }

        //fist parameter is smaller than the second
        if(first.compareTo(last) <= 0)
        {
            SortedSet<T> subset ;   //subset that will be return
            int beginning_subset;   //where the subset will start
            int ending_subset;      //where the subset will end

            //initializing
            subset = new SortedSet<>();
            beginning_subset = -1;
            ending_subset = -1;

            //find where the subset will start and end using the index
            for(int i=0; i<size-1; i++)
            {
                if(elements[i].compareTo(first) >= 0 && beginning_subset == -1)
                    beginning_subset = i;  //subset will start at this index
                if(elements[i].compareTo(last) >= 0 && ending_subset == -1)
                    ending_subset = i;     //subset will end at this index
            }

            //subset will take everything from the start index to the end
            if(ending_subset == -1)
                ending_subset = size;

            //add all element into the subset
            for(int i=beginning_subset; i<ending_subset;i++)
            {
                subset.add(elements[i]);
            }

            return subset;
        }
        else
            throw new IllegalArgumentException();  //first parameter was bigger than the last parameter
    }

    //check if set is full
    //return true if it full, else false
    @Override
    public boolean isFull() {
        if(size == elements.length)
            return true;
        else
            return false;
    }

    //return a string version of the array
    @Override
    public String toString() {
        String s;       //string that will save all elements from array
        s = "{";        //format for display
        if(size >= 1)   //at least one element in array
        {
            s = s + elements[0];
            for(int i = 1; i<size;i++)
            {
                s = s + ", " + elements[i];  //add the element separated by commas
            }
        }
        s = s + "}";    //format for display
        return  s;
    }

    //reset traversal
    @Override
    public void reset() {
        cursor = 0;         //start at the beginning of array
        modified = false;   //hasn't been modified yet
    }

    //get the next element in traversal
    @Override
    public T next() {
        //set has been modified during traversal
        if(modified)
            throw new TraversalException();

        T elem;     //current element in traversal
        elem = null;
        if(cursor < size)
        {
            elem = elements[cursor];  //get value of current element from set
            cursor++;  //go to next element

            return elem;
        }
        else
            throw  new TraversalException();  //current element is no longer in the set
    }

    //traversal still have another element
    @Override
    public boolean hasNext() {
        if(cursor < size)
            return true;    //still another element
        else
            return false;   //no more element
    }
}
