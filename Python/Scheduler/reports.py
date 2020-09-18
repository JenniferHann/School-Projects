import os
import fake_process as fp

MAX_HEIGHT = 500
MAX_WIDTH = 5000
XSCALE=10
YSCALE=10
XOFFSET = 20.5
YOFFSET_CPU = 30
YOFFSET_IO = 200

HTML_HEADER = f"""<!DOCTYPE html>
<html>
<body>

<canvas id="myCanvas" width="{MAX_WIDTH}" height="{MAX_HEIGHT}" style="border:1px solid #d3d3d3;">
Your browser does not support the HTML5 canvas tag.</canvas>

<script>
var c = document.getElementById("myCanvas");
var ctx = c.getContext("2d");
"""

HTML_FOOTER = """
</script>

</body>
</html>
"""


class Report:

    # -------------------------------------------------------------
    # initialize
    # -------------------------------------------------------------
    def __init__(self,processes,output_file,):
        global HTML_HEADER
        global MAX_HEIGHT

        self.file = output_file
        self.processes = processes
        self.x = 0
        self.y = 0

        # open the file
        self.fptr = None
        try:
            self.fptr = open (output_file,"w")
        except:
            print(f"Could not open {output_file}")

        if self.fptr is not None:

            # create the header for the html file
            self.fptr.write(HTML_HEADER)

            # initialize starting position for each process
            # and colours
            self.colours = {}
            self.xylist = {}
            pidlow=1
            pidmed=1
            pidhigh=1
            for p in self.processes:
                self.xylist[p.pid] = [XOFFSET,MAX_HEIGHT - (p.pid*YSCALE+YOFFSET_CPU)]
                if (p.pri == 0):
                    red = str(int(255-42.5 * pidhigh))
                    green = str(int(255-42.5*pidhigh))
                    pidhigh = pidhigh + 1
                    self.colours[p.pid] = f"'rgb({red},{green},0)'"
                elif (p.pri == 1):
                    red = str(int(255-42.5 * pidmed))
                    blue = str(int(255-42.5* pidmed))
                    pidmed = pidmed + 1
                    self.colours[p.pid] = f"'rgb({red},0,{blue})'"
                elif (p.pri == 2):
                    green = str(int(255-42.5 * pidlow))
                    blue = str(int(255-42.5* pidlow))
                    pidlow = pidlow + 1
                    self.colours[p.pid] = f"'rgb(0,{green},{blue})'"
                else:
                    self.colours[p.pid] = "'rgb(128,128,128)'"
                self.draw_cpu_line(p.pid)
                self.draw_io_line(p.pid)

                # draw the legend
                self.fptr.write("ctx.font = '20px serif'\n")
                y = 0.4*MAX_HEIGHT - 2*p.pid*YSCALE
                info = p.str_info()

                self.fptr.write("ctx.fillStyle = "+self.colours[p.pid]+"\n")
                self.fptr.write(f"ctx.fillText('{info}', 20, {y})\n");

    # -------------------------------------------------------------
    # update report with new info
    # -------------------------------------------------------------
    def update(self,incr):
        global MAX_HEIGHT
        if self.fptr is not None:

            for p in self.processes:
                if p.state == fp.State.ready_cpu:
                    self.process_line(pid=p.pid,width=4,x=incr*XSCALE+XOFFSET,y=MAX_HEIGHT - (p.pid*YSCALE+YOFFSET_CPU) )
                elif p.state == fp.State.active_cpu:
                    self.process_line(pid=p.pid,width=10,x=incr*XSCALE+XOFFSET,y=MAX_HEIGHT - (p.pid*YSCALE+YOFFSET_CPU) )
                elif p.state == fp.State.ready_io:
                    self.process_line(pid=p.pid,width=4,x=incr*XSCALE+XOFFSET,y=MAX_HEIGHT - (p.pid*YSCALE+YOFFSET_IO) )
                elif p.state == fp.State.active_io:
                    self.process_line(pid=p.pid,width=10,x=incr*XSCALE+XOFFSET,y=MAX_HEIGHT - (p.pid*YSCALE+YOFFSET_IO) )
                elif p.state == fp.State.sleep:
                    self.process_line(pid=p.pid,width=1,x=incr*XSCALE+XOFFSET,y=MAX_HEIGHT - (p.pid*YSCALE+YOFFSET_CPU) )

    def draw_cpu_line(self,pid):
        if self.fptr is not None:
            y=MAX_HEIGHT - (pid*YSCALE+YOFFSET_CPU)
            self.fptr.write("ctx.strokeStyle = 'rgb(200,200,200)'\n")
            self.fptr.write("ctx.lineWidth = 1\n")
            self.fptr.write("ctx.beginPath()"+ "\n")
            self.fptr.write(f"ctx.moveTo(0,{y})"+ "\n")
            self.fptr.write(f"ctx.lineTo({MAX_WIDTH},{y})"+ "\n")
            self.fptr.write("ctx.stroke()"+ "\n")

    def draw_io_line(self,pid):
        if self.fptr is not None:
            y=MAX_HEIGHT - (pid*YSCALE+YOFFSET_IO)
            self.fptr.write("ctx.strokeStyle = 'rgb(200,200,200)'\n")
            self.fptr.write("ctx.lineWidth = 1\n")
            self.fptr.write("ctx.beginPath()"+ "\n")
            self.fptr.write(f"ctx.moveTo(0,{y})"+ "\n")
            self.fptr.write(f"ctx.lineTo({MAX_WIDTH},{y})"+ "\n")
            self.fptr.write("ctx.stroke()"+ "\n")


    def process_line(self,pid,width,x,y):
        #print("********",pid,self.xylist[pid][0],self.xylist[pid][1],"to",x,y)
        if self.fptr is not None:
            self.fptr.write("ctx.strokeStyle = " + self.colours[pid] + "\n")
            self.fptr.write("ctx.lineWidth = " + str(width)+ "\n")
            self.fptr.write("ctx.beginPath()"+ "\n")
            self.fptr.write("ctx.moveTo("+str(self.xylist[pid][0])+","+str(self.xylist[pid][1])+")"+ "\n")
            self.fptr.write(f"ctx.lineTo({x},{y})"+ "\n")
            self.fptr.write("ctx.stroke()"+ "\n")
            self.xylist[pid] = [x,y]

    def tick_line(self,incr):
        #print("********",pid,self.xylist[pid][0],self.xylist[pid][1],"to",x,y)
        if self.fptr is not None:
            self.fptr.write("ctx.strokeStyle = 'rgb(200,200,200)'\n")
            self.fptr.write("ctx.lineWidth = 1\n")
            self.fptr.write("ctx.beginPath()"+ "\n")
            self.fptr.write("ctx.moveTo("+str(incr*XSCALE+XOFFSET)+",0)" + "\n")
            self.fptr.write("ctx.lineTo("+str(incr*XSCALE+XOFFSET)+f","+str(MAX_HEIGHT-20)+")"+ "\n")
            self.fptr.write("ctx.stroke()"+ "\n")

            self.fptr.write("ctx.font = '12px serif'\n")
            self.fptr.write("ctx.fillStyle = 'rgb(0,0,0)'"+"\n")
            self.fptr.write(f"ctx.fillText('{incr}', "+str(incr*XSCALE+8.5)+\
            ", "+str(MAX_HEIGHT-8)+")\n");


    def close(self):
        global HTML_FOOTER
        if self.fptr is not None:
            self.fptr.write(HTML_FOOTER)
            self.fptr.close()
