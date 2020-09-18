# ==================================================================
# Asgt 5 INSTRUCTIONS:
# ==================================================================
# purpose: To write the code necessary to create a
#          priority pre-emptive scheduler
# rules:   The rules of the scheduler are:
#          1) If a process requests a resource, it is put in the
#             end of the appropriate queue
#          2) When the clock "ticks", ...
#             a) the current process pauses, and is put at the end
#                of the appropritate queue
#             b) the next highest priority process is given access
#                to the each of the cpu/io resources, as required
#
# starter code:
#          1) asg5_starter_code.py - this file.
#             - you must write the functions as required
#          2) fake_process.py
#             - class FakeProcess - mimics a 'real' process
#             - class Run_TestProcesses - setups and executes your code
#               HINT: modify this class if you want to reduce the number
#                     of processes while you are debugging your code
#          3) reports.py
#             - creates reports... you don't need to look at it
#          4) scheduler_ipc.py
#             - this code mimics the communication between an OS scheduler
#               and each of the processes
#          5) diagram_sandy.html - the html output from my solution.
#             - your code will produce diagram.html automatically
#
# To run your code:
#          python asg4_starter_code.py
# or in interactive mode:
#          python -i asg4_starter_code.py
#
# MARKING:
#      Functionality (max 80%)
#      55 % - works as a pre-emptive scheduler, with no priorities
#      70 % - works as a pre-emptive scheduler, with 3 priorities
#      80 % - pre-emptive + pritorites AND implements some form of aging protocol
#             (low-priority processes get bumped to a higher Priority
#               if they have to wait too long...
#              you MUST describe the algorithm in the comments)
#
#      Coding Standards (max 20%)
#      - good comments (describe the PURPOSE of the code, not just
#                       what it is doing)
#      - break things down into separate functions as necessary
#      - good variable names
#      - no magic numbers, etc.
#      - other things.
# ===========================================================================



import scheduler_ipc as ipc
import fake_process as fp
from time import sleep


# queues... priority 0 to 2, priority 0 is the highest
MAX_PRI_NUMBER=2
MIN_PRI_NUMBER=0
ready_queue = [ [], [], [] ]    # a list of 3 queues
io_queue = [ [], [], [] ]       # a list of 3 queues

# keep track of who is currently using the cpu and io
current_cpu_process = None
current_io_process = None

# -----------------------------------------------------------------
# clock tick...
# ... pause currently running process, and put it at the end
#     of the appropriate queue
# ... resume next highest priority process
# ... this is the only place where processes are interrupted!
# -----------------------------------------------------------------
def clock_tick_received () :

    #get all global variable to be used
    global current_cpu_process
    global current_io_process
    global io_queue
    global ready_queue

    #check if the variable for keeping track of the current process in the 
    #cpu is initalize or not
    if current_cpu_process is None :
        current_cpu_process = ready_queue[0].pop(0)  #get the first process
        fp.FakeProcess.resume(current_cpu_process)   #start the firt process
    else :
        if current_cpu_process.state == fp.State.active_cpu :    #current process is already running
            fp.FakeProcess.pause(current_cpu_process)            #pause the process so that the next one can start

            if current_cpu_process.state == fp.State.ready_cpu :                    #make sure the process is added to right queue (process still need the cpu after)
                ready_queue[current_cpu_process.pri].append(current_cpu_process)    #add the process to the queue   
            
        for aList in ready_queue :        #loop through the queue to get the first one 
            if len(aList) != 0 :
                current_cpu_process = aList.pop(0)
                break     #exit once first process in queue found

        if current_cpu_process.state == fp.State.ready_cpu :   #make sure process is meant to be run by the cpu
            fp.FakeProcess.resume(current_cpu_process)         #run the process

    #check if the variable for keeping track of the current process in the    
    #io is initalize or not
    if current_io_process is None :
        if len(io_queue[0]) != 0 :
            current_io_process = io_queue[0].pop(0)      #get the first process
            fp.FakeProcess.resume(current_io_process)    #start the process
    else :
        if (len(io_queue[0]) != 0) or (len(io_queue[1]) != 0) or (len(io_queue[2]) != 0):  #check if queue is empty, can't run anything if there is nothing
            #check if process was using the io
            if current_io_process.state == fp.State.active_io :
                #pause the process
                fp.FakeProcess.pause(current_io_process)
                #check if the process still need the io
                if current_io_process.state == fp.State.ready_io :
                    #add the process to the queue
                    io_queue[current_io_process.pri].append(current_io_process)

            #loop through the queue to get the first process
            for aList in io_queue :
                if len(aList) != 0 :
                    current_io_process = aList.pop(0)  #get the first process on the queue
                    break
            #start the process in the io
            fp.FakeProcess.resume(current_io_process)
     
ipc.set_clock_callback(clock_tick_received)

# -----------------------------------------------------------------
# request for cpu, or io
# -----------------------------------------------------------------

def request_for_cpu(p):
    #add the process to the queue for the cpu with the appropriate priority
    ready_queue[p.pri].append(p)

def request_for_io(p):
    #add the process to the queue for the io with the appropriate priority
    io_queue[p.pri].append(p)

def io_finished(p):
    #change the status of the finish io burst of the process to the next burst
    #it needs or if done, mark it as finish
    p.change_state

def cpu_finished(p):
    #change the status of the finish io burst of the process to the next burst
    #it needs or if done, mark it as finish
    p.change_state

ipc.set_io_callback(request_for_io)
ipc.set_cpu_callback(request_for_cpu)
ipc.set_io_finished_callback(io_finished)
ipc.set_cpu_finished_callback(cpu_finished)


# -----------------------------------------------------------------
# main loop
# -----------------------------------------------------------------

# setup the all_processes
test=fp.Run_TestProcesses()

# start the clock ticking
test.start_clock()

# NOTE: "test" object will stop this program once all processes are complete
