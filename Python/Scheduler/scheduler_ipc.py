
# =================================================================
# global variables
# =================================================================
alarm_callback = None
io_callback = None
cpu_callback = None
io_finished_callback = None
cpu_finished_callback = None

# =================================================================
# functions
# =================================================================


# -----------------------------------------------------------------
# clock tick signal
# -----------------------------------------------------------------
def clock_tick():
    global alarm_callback
    if alarm_callback is not None:
        alarm_callback()

def set_clock_callback(cb = None):
    global alarm_callback
    alarm_callback = cb

# -----------------------------------------------------------------
# cpu callbacks
# -----------------------------------------------------------------
# --- define what to do when a request for cpu is received
# it gets the function from the global variable that saved the the function 
# that is used to add a process to the cpu queue
def set_cpu_callback(cb = None):
    global cpu_callback
    cpu_callback = cb
def set_cpu_finished_callback(cb = None):
    global cpu_finished_callback
    cpu_finished_callback = cb

# -----------------------------------------------------------------
# io callbacks
# -----------------------------------------------------------------
def set_io_callback(cb = None):
    global io_callback
    io_callback = cb
def set_io_finished_callback(cb = None):
    global io_finished_callback
    io_finished_callback = cb

# -----------------------------------------------------------------
# cpu signals
# -----------------------------------------------------------------
def _process_cpu_request(proc):
    global cpu_callback
    print (proc.pid, " process cpu request")
    if cpu_callback is not None:
        cpu_callback(proc)

def _process_cpu_finished(proc):
    global cpu_finished_callback
    print (proc.pid, " process cpu finished")
    if cpu_finished_callback is not None:
        cpu_finished_callback(proc)

# -----------------------------------------------------------------
# io signals
# -----------------------------------------------------------------
def _process_io_request(proc):
    global io_callback
    print (proc.pid, " process io request")
    if io_callback is not None:
        io_callback(proc)

def _process_io_finished(proc):
    global cpu_finished_callback
    print (proc.pid, " process io finished")
    if io_finished_callback is not None:
        io_finished_callback(proc)

# -----------------------------------------------------------------
# processe finished signal
# -----------------------------------------------------------------
def _process_has_finished(proc):
    print (proc.pid, " process has finished")


# -----------------------------------------------------------------
# setting the signal traps
# -----------------------------------------------------------------

def set_signal_traps():
    global traps
    traps={} #SIG_CPU_FINISHED SIG_CPU_FINISHED
    traps["SIG_CPU_FINISHED"] = _process_cpu_finished
    traps["SIG_IO_FINISHED"] = _process_io_finished
    traps["SIG_REQUEST_IO"] = _process_io_request
    traps["SIG_REQUEST_CPU"] = _process_cpu_request
    traps["SIG_REQUEST_IO"] = _process_io_request
    traps["SIG_REQUEST_CPU"] = _process_cpu_request
    traps["SIG_PROCESS_FINISHED"] = _process_has_finished
