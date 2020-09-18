/*
 * Copyright (c) 2020 Ian Clement. All rights reserved.
 */

package ca.qc.johnabbott.cs406.search;

import ca.qc.johnabbott.cs406.collections.Queue;
import ca.qc.johnabbott.cs406.collections.SparseArray;
import ca.qc.johnabbott.cs406.terrain.*;

import java.util.HashSet;
import java.util.Random;
import java.util.Set;

/**
 * A search for the goal, starting from the start cell, each cell is visited in the order they are discovered. The search
 * strategy expands outwards from the starting cell in layers.
 *
 * @author Jennifer Hann
 */
public class BFS implements Search {

    // records where we've been and what steps we've taken.
    private SparseArray<Cell> memory;

    // records the path taken
    private Queue<Location> path;

    // for tracking the "traversable" solution.
    private Location solution;
    private boolean foundSolution;

    // the terrain we're searching in.
    private Terrain terrain;

    /**
     * Create a new Breadth-first Search
     */
    public BFS() {
    }

    @Override
    public void solve(Terrain terrain) {

        // the terrain chosen to be solved
        this.terrain = terrain;

        // track locations we've been to using our terrain "memory"
        Cell defaultCell = new Cell();
        defaultCell.setColor(Color.WHITE);
        memory = new SparseArray<>(defaultCell);

        // setup clockwise direction
        Direction[] clockwise = Direction.getClockwise();

        // track the current search location, starting at the terrain start location.
        Location current = terrain.getStart();

        // direction to visit cells
        Direction direction = Direction.NONE;

        // stores the path took to find the goal
        path =  new Queue<>();

        // add to queue
        path.enqueue(current);

        while(!current.equals(terrain.getGoal()))  {

            // all accessible cells are visited and goal is still not found, goal cannot be reach
            if(path.isEmpty()){
                foundSolution = false;
                return;
            }

            // get the new cell to be checked
            current = path.dequeue();

            // current cell is the goal, goal has been found
            if(current.equals(terrain.getGoal())) {
                break;
            }

            // record that we've been here
            memory.get(current).setColor(Color.BLACK);

            // check all directions if there are new cells to be visited
            for (int i=0; i < clockwise.length; i++){

                // get next direction to check
                direction = clockwise[i];

                // find the next cell
                Location next = current.get(direction);

                // new cell that can be visited
                if(terrain.inTerrain(next) && !terrain.isWall(next) && memory.get(next).getColor() == Color.WHITE){

                    // record the step we've taken to memory to recreate the solution in the later traversal.
                    memory.get(current).setTo(direction);

                    // step into new cell
                    current = current.get(direction);

                    // record that we've seen it
                    memory.get(current).setColor(Color.GREY);

                    // save in path to come back later
                    path.enqueue(current);

                    // backtrack and save direction took
                    Direction goBack = direction.opposite();
                    memory.get(current).setFrom(goBack);

                    // step back
                    current = current.get(goBack);

                    System.out.println(memory);
                }
            }

        }

        // find the right path from starting cell to the goal
        while (!current.equals(terrain.getStart())){

            // trace backward from goal to start to get the path
            Direction to = memory.get(current).getFrom();

            // get next cell to set to the right direction
            Location next = current.get(to);

            // set to right path to the goal
            memory.get(next).setTo(to.opposite());

            // step back
            current = current.get(to);
        }

        current = terrain.getGoal();
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