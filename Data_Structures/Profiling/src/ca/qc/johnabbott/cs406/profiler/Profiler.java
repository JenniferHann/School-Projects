package ca.qc.johnabbott.cs406.profiler;

import java.util.*;

/**
 * A simple profiling class.
 */
public class Profiler {

    /*
    Delimits a section or region of the profiling.
    */
    private static class Mark {

        // stores the type of mark
        private enum Type {
            START_REGION, END_REGION, START_SECTION, END_SECTION
        }

        public Type type;
        public long time;
        public String label;

        // Create mark without a label
        public Mark(Type type, long time) {
            this(type, time, null);
        }

        // Create a mark with a label
        public Mark(Type type, long time, String label) {
            this.type = type;
            this.time = time;
            this.label = label;
        }

        @Override
        public String toString() {
            return "Mark{" +
                    "type=" + type +
                    ", time=" + time +
                    ", label='" + label + '\'' +
                    '}';
        }
    }

    // store singleton instance
    private static Profiler INSTANCE;
    static {
        INSTANCE = new Profiler();
    }

    /**
     * Get profiler singleton instance.
     * @return the profiler singleton.
     */
    public static Profiler getInstance() {
        return INSTANCE;
    }

    // store marks in list
    private List<Mark> marks;

    // use to prevent regions when not wanted/needed.
    private boolean paused;
    private boolean inSection;

    // private constructor for singleton
    private Profiler() {
        // linked list, because append is a constant time operation.
        marks = new LinkedList<>();
        paused = false;
        inSection = false;
    }

    /**
     * Starts a new profiling section.
     * @param label The section label.
     */
    public void startSection(String label) {
        if(!paused) {
            marks.add(new Mark(Mark.Type.START_SECTION, System.nanoTime(), label));
            inSection = true;
        }
    }

    /**
     * Ends a section. Must be paired with a corresponding call to `startSection(..)`.
     */
    public void endSection() {
        if(!paused) {
            marks.add(new Mark(Mark.Type.END_SECTION, System.nanoTime()));
            inSection = false;
        }
    }

    /**
     * Starts a new profiling region.
     * @param label The region label.
     */
    public void startRegion(String label) {
        if(!paused && inSection)
            marks.add(new Mark(Mark.Type.START_REGION, System.nanoTime(), label));
    }

    /**
     * Ends a region. Must be paired with a corresponding call to `startRegion(..)`.
     */
    public void endRegion() {
        if(!paused && inSection)
            marks.add(new Mark(Mark.Type.END_REGION, System.nanoTime()));
    }

    /**
     * Using the currently collected data, generate all the section data for reporting.
     * @return A list of sections.
     */
    public List<Section> produceSections() {

        //check if all started region or section has finish
        int countStart = 0;    //number of started sections or regions
        int countEnd = 0;      //number of finish sections or regions
        //loop through all marks
        for(Mark mark : marks){
            if(mark.type == Mark.Type.START_SECTION || mark.type == Mark.Type.START_REGION)   //mark is a starting a region or a section
                countStart++;
            else if(mark.type == Mark.Type.END_SECTION || mark.type == Mark.Type.END_REGION)  //mark is ending a region or a section
                countEnd++;
        }

        //check if all of regions and sections are ended
        if(countStart != countEnd)
            throw new ProfilerException();

        // TODO: compile results

        //list of sections
        List<Section> sections = new ArrayList<>();

        //section to be added to list
        Section section = null;
        //fields fro section
        long sTimestart = 0;         //starting time for a section
        long sTimeEnd = 0;           //starting time for a section
        String regionLabel = null;   //section's label

        //region to be added to section
        Region region = null;
        //fields for region
        long rElapsedtimeStart = 0;   //starting time for a region
        long rElapsedtimeEnd = 0;     //ending time for a region

        //keep track when there are nested regions
        Stack nestedRegions = new Stack();     //keep track of regions
        Stack elapsedTimeStart = new Stack();  //keep track of starting time of region
        Stack regionLabelOuter = new Stack();  //keep track of region's label

        //check if region already exist in the section
        boolean regionAlreadyExist = false;

<<<<<<< HEAD
        for (Mark mark : marks)
        {
            
            if(mark.type == Mark.Type.START_SECTION) {
=======
        //loop through all sections and regions to create a list of section
        //creating section
        //adding regions to its proper section
        //adding sections into the list
        for(Mark mark : marks)
        {
            if(mark.type == Mark.Type.START_SECTION) {       //beginning of a section
>>>>>>> master
                //create section to be add to list
                //save section label
                section = new Section(mark.label);
                //save start time of section
                sTimestart = mark.time;
            }
            else if(mark.type == Mark.Type.START_REGION){     //beginning of a region

                //check if mark already exist in section
                regionAlreadyExist = false;                               //reset
                Map<String,Region> regions = section.getRegions();        //get all regions in section

                //check all regions to find if current region already exist in section
                for (Map.Entry<String, Region> r : regions.entrySet()){
                    //region already exist in section
                    if(r.getKey() == mark.label) {
                        regionAlreadyExist = true;       //region found
                        region = r.getValue();           //get existing region
                        rElapsedtimeStart = mark.time;   //get starting time of region
                        break;                           //exit loop when region found
                    }
                }

                //check if new region, not found in section
                if( !regionAlreadyExist){
                    region = new Region();          //create new region
                    region.setSection(section);     //save the section where region located
                    region.addRun();                //start run count
                    rElapsedtimeStart = mark.time;  //save starting time of region
                    regionLabel = mark.label;       //save region label

                }
                else {
                    region.addRun();   //increment run count of already existing region in section
                }

                nestedRegions.push(region);                  //save current region
                elapsedTimeStart.push(rElapsedtimeStart);    //save current start time of region
                regionLabelOuter.push(regionLabel);          //save current label of region

            }else if(mark.type == Mark.Type.END_REGION) {    //end of a region

                //get current region from stack
                if(!nestedRegions.isEmpty()){
                    region = (Region) nestedRegions.pop();             //get region from stack
                    regionLabel = (String) regionLabelOuter.pop();     //update region label
                    rElapsedtimeStart = (long) elapsedTimeStart.pop(); //get right start time for region
                }

                rElapsedtimeEnd = mark.time;            //get end time for region to finish
                region.addElapsedTime(rElapsedtimeEnd-rElapsedtimeStart);   //find elapsed time for region

                //check if mark already exist in section
                regionAlreadyExist = false;                            //bool that check is region already exist in the section
                Map<String,Region> regions = section.getRegions();     //get the regions in section
                Set<String> keys = regions.keySet();                   //get all the labels of each regions

                //loop through all regions label to find is current label already added to section
                for (String k : keys){
                    if(k == regionLabel) {    //region already exist in section
                        regionAlreadyExist = true;
                        break;
                    }
                }

                //check if region already added to the section
                if(!regionAlreadyExist ){
                    //add region
                    section.addRegion(regionLabel, region);
                }
            }else if(mark.type == Mark.Type.END_SECTION) {     //end of a section
                //get end time for section
                sTimeEnd = mark.time;

                //adding the percent of section for each region inside of section
                Map<String, Region> regions = section.getRegions();
                for(Region r : regions.values()){
                    //add fields for TOTAL
                    if(r.getSection() == null){
                        r.setSection(section);
                        r.addRun();
                        r.addElapsedTime(sTimeEnd-sTimestart);
                        r.setPercentOfSection(1.00);
                    }
                    else {
                        if(nestedRegions.isEmpty())  //get percent of section for region called multiple times
                            r.setPercentOfSection((double)r.getElapsedTime()/(sTimeEnd-sTimestart));
                        else    //get percent of section for region called once
                            r.setPercentOfSection((double)(rElapsedtimeEnd-rElapsedtimeStart)/(sTimeEnd-sTimestart));
                    }
                }

                //add section
                sections.add(section);
            }
        }
        //reset marks
        marks.clear();
        //return list of sections containing regions
        return sections;
    }


    @Override
    public String toString() {
        // print all marks using formatting.
        StringBuilder builder = new StringBuilder();
        for(Mark mark : marks)
            builder.append(String.format("%d %13s %-30s\n", mark.time, mark.type.toString(), mark.label != null ? mark.label : ""));
        return builder.toString();
    }
}