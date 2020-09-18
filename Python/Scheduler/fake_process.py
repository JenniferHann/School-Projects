import enum
import scheduler_ipc as ipc
import os
import reports
import sys
CLOCK_TRIGGER_TIME = 5

# -----------------------------------------------------------------
# State
# -----------------------------------------------------------------
class State(enum.Enum):
    sleep = 1
    ready_cpu = 2
    ready_io = 3
    active_cpu = 4
    active_io = 5
    finished = 6

# =================================================================
# FakeProcess class
# =================================================================
class FakeProcess:

    # -------------------------------------------------------------
    # static class variables & methods
    # -------------------------------------------------------------
    __pid = 0
    @classmethod
    def __get_new_pid(cls):
        cls.__pid = cls.__pid + 1
        return cls.__pid

    def debug(fp, msg):
        if os.getenv("FP_DEBUG") is not None:
            print(fp)

    # -------------------------------------------------------------
    # constructor
    # -------------------------------------------------------------
    def __init__(self,init_sleep=0,bursts=[10,8,20,6,15,30,10], pri=0):
        self.pid = FakeProcess.__get_new_pid()
        self.bursts = bursts
        self.burst_num = -1
        self.pri = pri
        self.state = State.sleep
        self.__countdown = init_sleep
        self.prev_state = None

    # -------------------------------------------------------------
    # print object
    # -------------------------------------------------------------
    def __str__(self):
        y = "Process: Pid: " + str(self.pid) + \
        " Burst #: " + str(self.burst_num) + \
        " Countdown #: " +  str(self.__countdown) + \
        " State: " + str(self.state)
        return y

    def __repr__(self):
        return self.__str__()

    # -------------------------------------------------------------
    # just some basic info... not good for debugging
    # -------------------------------------------------------------
    def str_info(self):
        return "Pid: " + str(self.pid) + " - Priority: " + str(self.pri) + " - bursts " + str(self.bursts)

    # -------------------------------------------------------------
    # pause execution of process
    # -------------------------------------------------------------
    def pause(self):
        if self.state == State.active_cpu:
            self.state = State.ready_cpu
        elif self.state == State.active_io:
            self.state = State.ready_io
        self.debug("Paused: ")

    # -------------------------------------------------------------
    # is the process in a paused state?
    # ... note... sleep is not considered paused, because
    #             we still decriment the counter
    # -------------------------------------------------------------
    def is_paused(self):
        paused = False
        if self.state == State.ready_cpu:
            paused = True
        elif self.state == State.ready_io:
            paused = True
        elif self.state == State.finished:
            paused = True
        return paused

    # -------------------------------------------------------------
    # move the process from paused state to active state
    # -------------------------------------------------------------
    def resume(self):
        if self.state == State.ready_cpu:
            self.state = State.active_cpu
        elif self.state == State.ready_io:
            self.state = State.active_io
        self.debug("Resumed")

    # -------------------------------------------------------------
    # process one "execution" step for this process
    # -------------------------------------------------------------
    def step(self):
        self.prev_state = self.state
        if not self.is_paused():
            self.__countdown = self.__countdown - 1
            if self.__countdown == 0:
                self.change_state()

    # -------------------------------------------------------------
    # change state from old one to new one
    # -------------------------------------------------------------
    def change_state(self):
        try:
            self.burst_num = self.burst_num + 1
            if self.state == State.sleep:
                self.state = State.ready_cpu
                self.__countdown = self.bursts[self.burst_num]
                ipc.traps["SIG_REQUEST_CPU"](self)
            elif self.state == State.active_cpu:
                self.state = State.ready_io
                ipc.traps["SIG_CPU_FINISHED"](self)
                self.__countdown = self.bursts[self.burst_num]
                ipc.traps["SIG_REQUEST_IO"](self)
            elif self.state == State.active_io:
                self.state = State.ready_cpu
                ipc.traps["SIG_IO_FINISHED"](self)
                self.__countdown = self.bursts[self.burst_num]
                ipc.traps["SIG_REQUEST_CPU"](self)

        except:
            self.state = State.finished

        self.debug("changed state")



# =================================================================
# CurrentProcesses class
# =================================================================
class Run_TestProcesses:
    __all_processes = []

    # -------------------------------------------------------------
    # initialize
    # -------------------------------------------------------------
    def __init__(self):
        ipc.set_signal_traps()

        # start the processes
        proc=FakeProcess(init_sleep=2,bursts=[13,8,21,6,14,21,14,7], pri=0)
        self.add(proc)
        proc=FakeProcess(init_sleep=12,bursts=[8,9,6,23,33,16,4,11], pri=0)
        self.add(proc)
        proc=FakeProcess(init_sleep=79,bursts=[12,8,24,6,14,27,9], pri=0)
        self.add(proc)
        proc=FakeProcess(init_sleep=17,bursts=[19,16,21,32,16], pri=1)
        self.add(proc)
        proc=FakeProcess(init_sleep=11,bursts=[32,8,18,6,17], pri=1)
        self.add(proc)
        proc=FakeProcess(init_sleep=9,bursts=[11,8,18,6,17], pri=2)
        self.add(proc)

        self.report = reports.Report(self.__all_processes,"diagram.html")

    # -------------------------------------------------------------
    # start the computer? clock
    # -------------------------------------------------------------
    def start_clock(self):
        global CLOCK_TRIGGER_TIME
        iter = 0
        interactive_mode = False

        while self.more_processes() and not interactive_mode:

            # for each instruction cycle within clock tick
            for i in range(CLOCK_TRIGGER_TIME):
                self.report.update(i + iter*CLOCK_TRIGGER_TIME)
                self.step()
                a = self.__wait_for_input(i+iter*CLOCK_TRIGGER_TIME)
                self.debug()
                if a.lower()[0] == "y":
                    interactive_mode = True
                    break

            # clock tick
            print("")
            print("------ tick --------")
            ipc.clock_tick()
            self.report.tick_line(i+iter*CLOCK_TRIGGER_TIME)

            # prevent ininfinite loop
            iter = iter + 1
            if iter > 100:
                print("")
                print("*******")
                print("Exceeded maximum clock iterations")
                print ("********")
                break

        self.report.close()

    # -------------------------------------------------------------
    # allow user to pause processing
    # -------------------------------------------------------------
    def __wait_for_input(self,num=0):
        a = "n"
        if os.getenv("STEP") is not None or sys.flags.interactive:
            a = input(f"{num} Hit Return to Continue, 'y' to stop: ")
            print("")
        return a or "n"

    # -------------------------------------------------------------
    # if debug var is set, then print info
    # -------------------------------------------------------------
    def debug(self):
        if os.getenv("ALL_DEBUG") is not None:
            print("ALL_DEBUG: ")
            print (self)

    # -------------------------------------------------------------
    # print object info
    # -------------------------------------------------------------
    def __str__(self):
        obj_str = ""
        for fp in self.__all_processes:
            obj_str = obj_str + str(fp) + "\n"
        return obj_str

    def __repr__(self):
        return self.__str__()

    # -------------------------------------------------------------
    # step through all the defined processes
    # -------------------------------------------------------------
    def step(self):
        for fp in self.__all_processes:
            fp.step()

    # -------------------------------------------------------------
    # add a process to the collection
    # -------------------------------------------------------------
    def add(self,fp):
        fp.debug("added to all processes")
        self.__all_processes.append(fp)


    # -------------------------------------------------------------
    # remove a process from the collection
    # -------------------------------------------------------------
    def remove(self, pid):
        self.__all_processes = \
        [fp for fp in self.__all_processes if fp.pid != pid]

    # -------------------------------------------------------------
    # are there any processes that are still running?
    # -------------------------------------------------------------
    def more_processes(self):
        not_finished = [fp for fp in self.__all_processes if fp.state != State.finished]
        if len(not_finished) == 0:
            return False
        else:
            return True
