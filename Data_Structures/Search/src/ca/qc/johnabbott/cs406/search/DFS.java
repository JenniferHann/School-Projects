/*
 * Copyright (c) 2020 Ian Clement. All rights reserved.
 */

package ca.qc.johnabbott.cs406.search;

import ca.qc.johnabbott.cs406.collections.SparseArray;
import ca.qc.johnabbott.cs406.terrain.*;

import java.util.HashSet;
import java.util.Random;
import java.util.Set;
import java.util.Stack;

/**
 * A search for the goal, starting from the start cell, visit each cells by continuing in oen direction until there is no
 * where else to go. Then it goes back the way it came and tries the next available direction with the same strategy.
 *
 * @author Jennifer Hann
 */
public class DFS implements Search {

    // records where we've been and what steps we've taken.
    private SparseArray<Cell> memory;

    // records the path taken
    private Stack<Cell> path;

    // for tracking the "traversable" solution.
    private Location solution;
    private boolean foundSolution;

    // the terrain we're searching in.
    private Terrain terrain;

    /**
     * Create a new Dept-first Search
     */
    public DFS() {
    }

    @Override
    public void solve(Terrain terrain) {

        // tell which terrain is being used
        this.terrain = terrain;

        // track locations we've been to using our terrain "memory"
        Cell defaultCell = new Cell();
        defaultCell.setColor(Color.WHITE);
        memory = new SparseArray<>(defaultCell);

        // setup clockwise direction
        Direction[] clockwise = Direction.getClockwise();

        // track the current search location, starting at the terrain start location.
        Location current = terrain.getStart();

        // start with up direction. We will adjust this accordingly.
        int reset = 0;
        int nextDirection;
        Direction previous;

        // stores the path took to find the goal
        path =  new Stack<Cell>();
        boolean justPop = false;

        while(!current.equals(terrain.getGoal())) {

            // reset previous to up
            nextDirection = reset;
            previous = clockwise[nextDirection];

            // find the next direction
            Direction direction = Direction.NONE;
            Location next = current.get(previous);

            // reset pop boolean
            justPop = false;

            // change direction if we can't go in the previous direction
                if( (!terrain.inTerrain(next) || terrain.isWall(next)) || memory.get(next).getColor() != Color.WHITE ) {

                // keep track of what we've seen in a set of directions
                Set<Direction> checked = new HashSet<>();

                // check in all directions
                while (checked.size() < 3) {

                    // get next direction in clockwise movement
                    Direction tmp = clockwise[++nextDirection];
                    checked.add(tmp);

                    // see if stepping in that direction is possible and do it!
                    next = current.get(tmp);
                    if (terrain.inTerrain(next) && !terrain.isWall(next) && memory.get(next).getColor() == Color.WHITE) {
                        previous = direction = tmp;
                        break;
                    }
                }

                // if no direction was found and we are at still in the original location, we are stuck and leave without solution
                if(direction == Direction.NONE) {
                    if(path.isEmpty()){
                        foundSolution = false;
                        return;
                    }
                    else {
                        // if no direction was found and we are not in the original location, we need to backtrack
                        Cell cTemp = path.pop();  //get previous location
                        Direction dTemp = cTemp.getFrom();  //get the direction to move
                        direction = dTemp.opposite();  //go to the previous location

                        justPop = true;
                    }
                }

            }
            else
                direction = previous;  //can continue in same direction

            // record the step we've taken to memory to recreate the solution in the later traversal.
            memory.get(current).setTo(direction);

            // cell to save the path took
            Cell path_cell = new Cell();
            path_cell.setFrom(previous);
            path_cell.setTo(direction);

            // step
            current = current.get(direction);

            // record that we've been here
            memory.get(current).setColor(Color.BLACK);

            if(justPop == false)
            {
                // record the way we took
                path.push(path_cell);
            }

            System.out.println(memory);
        }

        // we reached the goal and have a solution.
        foundSolution = true;
    }

    @Override
    public void reset() {
        // start the traversal of our path at the terrain's start.
        solution = terrain.getStart();
    }

    @Override
    public Direction next() {
        // recall the direction at this location, move to the corresponding location and return it.
        Direction direction = memory.get(solution).getTo();
        solution = solution.get(direction);
        return direction;
    }

    @Override
    public boolean hasNext() {
        // we're only done when we get to the terrain goal.
        return !solution.equals(terrain.getGoal());
    }
}