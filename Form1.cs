using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ACCMetry
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          
            MemoryReader reader = new MemoryReader();
            //Set update intervals
            reader.StaticDataInterval = 5000;
            reader.PhysicsInterval = 100;
            reader.GraphicsInterval = 1000;

            //Subscribe the local methods to the events
          //  reader.PhysicsDataUpdated += updatePhysics;
          //  reader.GraphicsDataUpdated += updateGraphics;
            reader.StaticDataUpdated += updateForm;
            //Connect to memory
            reader.Connect();


        }


        private void updateForm(object sender, StaticDataEventArgs e)
        {
            //track_label.Invoke
        }

        //Updates the static data and displays in console
        static void updateStatic(object sender, StaticDataEventArgs e)
        {
            Console.WriteLine("Static Data:");
            Console.WriteLine("Track:   " + e.StaticData.Track);
            Console.WriteLine("Car:   " + e.StaticData.CarModel);
            Console.WriteLine("Player:   " + e.StaticData.PlayerName);
        }



        //Updates the physics data and displays in console
        static void updateGraphics(object sender, GraphicsEventArgs e)
        {
            Console.WriteLine("Graphics Data:");
            Console.WriteLine("Time:   " + e.GraphicsData.CurrentTime);
            Console.WriteLine("Fuel per lap:   " + e.GraphicsData.fuelPerLap);
            Console.WriteLine("Used fuel:   " + e.GraphicsData.UsedFuel);
        }

        //Updates the physics data and displays in console
        static void updatePhysics(object sender, PhysicsEventArgs e)
        {
            Console.WriteLine("Physics Data:");
            Console.WriteLine("Fuel:   " + e.PhysicsData.Fuel);
            Console.WriteLine("Gear:   " + e.PhysicsData.Gear);
            Console.WriteLine("Road temp:   " + e.PhysicsData.RoadTemp);
        }

        

    }
}
