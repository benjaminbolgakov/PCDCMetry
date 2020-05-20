using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ACCMetry
{

    //The contracts 
    public delegate void PhysicsHandler(object sender, PhysicsEventArgs e);
    public delegate void GraphicsHandler(object sender, GraphicsEventArgs e);
    public delegate void StaticDataHandler(object sender, StaticDataEventArgs e);

    /// <summary>
    /// Custom exception for when shared memory isn't available. 
    /// Caused by the game not being in a running session most likely, as no data will be streamed to memory.
    /// </summary>
    public class MemoryNotActiveException : Exception
    {
        public MemoryNotActiveException() : base("Memory not active")
        {

        }
    }

    //Status flags for memory
    enum MEMORY_STATUS { DISCONNECTED, CONNECTING, CONNECTED }




    //------------------------------------------------------------------------------------------------------------------




    public class MemoryReader
    {
        //Define MMF
        MemoryMappedFile physicsDataMMF;
        MemoryMappedFile graphicsDataMMF;
        MemoryMappedFile staticDataMMF;

        //Events that are called when timers are active and data is being processed.
        public event StaticDataHandler StaticDataUpdated;
        public event GraphicsHandler GraphicsDataUpdated;
        public event PhysicsHandler PhysicsDataUpdated;

        Timer physicsTimer;
        Timer graphicsTimer;
        Timer staticDataTimer;


        //Set inital memory status to DISCONNECTED
        private MEMORY_STATUS memStatus = MEMORY_STATUS.DISCONNECTED;

        //Memory timer
        private Timer memoryTimer;

        /// <summary>
        /// Class Constructor
        /// </summary>
        public MemoryReader()
        {
            //Initialize and set the memory timer
            memoryTimer = new Timer(2000);
            memoryTimer.AutoReset = true;
            //As timer elapse, call method to connect to memory and repeat until a connection is made.
            memoryTimer.Elapsed += memoryTimer_Elapsed;



            physicsTimer = new Timer();
            physicsTimer.AutoReset = true;
            physicsTimer.Elapsed += physicsTimer_Elapsed;
            PhysicsInterval = 10;

            graphicsTimer = new Timer();
            graphicsTimer.AutoReset = true;
            graphicsTimer.Elapsed += graphicsTimer_Elapsed;
            GraphicsInterval = 1000;

            staticDataTimer = new Timer();
            staticDataTimer.AutoReset = true;
            staticDataTimer.Elapsed += staticInfoTimer_Elapsed;
            StaticDataInterval = 1000;


        }


        /// <summary>
        /// Stop the timers and dispose of the shared memory handles
        /// </summary>
        public void Stop()
        {
            memStatus = MEMORY_STATUS.DISCONNECTED;
            memoryTimer.Stop();

            // Stop the timers
            physicsTimer.Stop();
            graphicsTimer.Stop();
            staticDataTimer.Stop();
        }

        /// <summary>
        /// Starts the whole connection process by starting the memoryTimer which will trigger connection attempts to memory.
        /// </summary>
        public void Connect()
        {
            memoryTimer.Start();
        }

        /// <summary>
        /// This method is called as the memorytimer have turned one whole interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void memoryTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ConnectToMemory();
        }


        private void physicsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ProcessPhysicsData();
        }

        private void graphicsTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ProcessGraphicsData();
        }

        private void staticInfoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ProcessStaticData();
        }


        /// <summary>
        /// Assigns MMF files with the path to shared memory location and calls for processing to begin if files are found.
        /// </summary>
        /// <returns>True if a connection is made, false if no file is found.</returns>
        private bool ConnectToMemory()
        {
            try
            {
                //Starting attempt to connect to memory
                memStatus = MEMORY_STATUS.CONNECTING;

                //Open MMF paths
                staticDataMMF = MemoryMappedFile.OpenExisting("Local\\acpmf_static");
                graphicsDataMMF = MemoryMappedFile.OpenExisting("Local\\acpmf_graphics");
                physicsDataMMF = MemoryMappedFile.OpenExisting("Local\\acpmf_physics");

                //Start processing data from memory files
                graphicsTimer.Start();
                ProcessGraphicsData();

                physicsTimer.Start();
                ProcessPhysicsData();

                staticDataTimer.Start();
                ProcessStaticData();

                //Connected successfully
                memStatus = MEMORY_STATUS.CONNECTED;
                //Stop resetting connection-timer
                memoryTimer.Stop();
                return true;
            }
            catch (FileNotFoundException)
            {
                staticDataTimer.Stop();
                graphicsTimer.Stop();
                physicsTimer.Stop();
                return false;
            }
        }


        /// <summary>
        /// Interval for physics updates in milliseconds
        /// </summary>
        public double PhysicsInterval
        {
            get
            {
                return physicsTimer.Interval;
            }
            set
            {
                physicsTimer.Interval = value;
            }
        }

        /// <summary>
        /// Interval for graphics updates in milliseconds
        /// </summary>
        public double GraphicsInterval
        {
            get
            {
                return graphicsTimer.Interval;
            }
            set
            {
                graphicsTimer.Interval = value;
            }
        }

        /// <summary>
        /// Interval for static info updates in milliseconds
        /// </summary>
        public double StaticDataInterval
        {
            get
            {
                return staticDataTimer.Interval;
            }
            set
            {
                staticDataTimer.Interval = value;
            }
        }



        /// <summary>
        /// Event triggers that notifies the subscribers and passes the data as "e"
        /// </summary>
        /// <param name="e">The collected data</param>
        public virtual void OnStaticDataUpdated(StaticDataEventArgs e)
        {
            //Check if there are any subscribers to the event
            if (StaticDataUpdated != null)
                //Pass on the dataobject in "e" to subscribers.
                StaticDataUpdated(this, e);
        }

        public virtual void OnGraphicsUpdated(GraphicsEventArgs e)
        {
            if (GraphicsDataUpdated != null)
                GraphicsDataUpdated(this, e);
        }

        public virtual void OnPhysicsUpdated(PhysicsEventArgs e)
        {
            if (PhysicsDataUpdated != null)
                PhysicsDataUpdated(this, e);
        }


        /// <summary>
        /// Methods writing read byte data into respective dataobject
        /// </summary>
        private void ProcessPhysicsData()
        {
            if (memStatus == MEMORY_STATUS.DISCONNECTED)
                return;

            try
            {
                PhysicsData physicsData = ReadPhysicsData();
                OnPhysicsUpdated(new PhysicsEventArgs(physicsData));
            }
            catch (MemoryNotActiveException) { }
        }

        private void ProcessGraphicsData()
        {
            if (memStatus == MEMORY_STATUS.DISCONNECTED)
                return;

            try
            {
                GraphicsData graphicsData = ReadGraphicsData();
                OnGraphicsUpdated(new GraphicsEventArgs(graphicsData));
            }
            catch (MemoryNotActiveException) { }
        }

        private void ProcessStaticData()
        {
            if (memStatus == MEMORY_STATUS.DISCONNECTED)
                return;

            try
            {
                StaticData staticData = ReadStaticData();

                //Data has been read/gathered. Notify the subscribers of this event and pass
                //the data from "staticData"
                OnStaticDataUpdated(new StaticDataEventArgs(staticData));
            }
            catch (MemoryNotActiveException) { }
        }




        /// <summary>
        /// Static dataobject return method that opens a stream to the mapped memory file and decodes the byte data.
        /// </summary>
        /// <returns>Allocated memory data</returns>
        public StaticData ReadStaticData()
        {
            if (memStatus == MEMORY_STATUS.DISCONNECTED || staticDataMMF == null)
                throw new MemoryNotActiveException();

            //Open the stream to the mapped memory file
            using (var stream = staticDataMMF.CreateViewStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    //Size variable for the dataobject
                    var size = Marshal.SizeOf(typeof(StaticData));
                    //Read byte stream to variable
                    var bytes = reader.ReadBytes(size);
                    var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                    //Takes the unmanaged data and applies it the the data structure of StaticData struct.
                    var data = (StaticData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(StaticData));
                    handle.Free();
                    return data;
                }
            }
        }

        /// <summary>
        /// Graphics dataobject return method that opens a stream to the mapped memory file and decodes the byte data.
        /// </summary>
        /// <returns>Allocated memory data</returns>
        public GraphicsData ReadGraphicsData()
        {
            if (memStatus == MEMORY_STATUS.DISCONNECTED || graphicsDataMMF == null)
                throw new MemoryNotActiveException();

            using (var stream = graphicsDataMMF.CreateViewStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    //Size variable for the dataobject
                    var size = Marshal.SizeOf(typeof(GraphicsData));
                    //
                    var bytes = reader.ReadBytes(size);
                    //
                    var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                    //
                    var data = (GraphicsData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(GraphicsData));
                    handle.Free();
                    return data;
                }
            }
        }

        /// <summary>
        /// Physics dataobject return method that opens a stream to the mapped memory file and decodes the byte data.
        /// </summary>
        /// <returns>Allocated memory data</returns>
        public PhysicsData ReadPhysicsData()
        {
            if (memStatus == MEMORY_STATUS.DISCONNECTED || physicsDataMMF == null)
                throw new MemoryNotActiveException();

            using (var stream = physicsDataMMF.CreateViewStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    //Size variable for the dataobject
                    var size = Marshal.SizeOf(typeof(PhysicsData));
                    //
                    var bytes = reader.ReadBytes(size);
                    //
                    var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                    //
                    var data = (PhysicsData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(PhysicsData));
                    handle.Free();
                    return data;
                }
            }
        }




    }

}
