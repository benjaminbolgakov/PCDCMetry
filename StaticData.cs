using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACCMetry
{
    public class StaticDataEventArgs : EventArgs
    {
        //Constructor for reference to struct data
        public StaticDataEventArgs(StaticData staticData)
        {
            this.StaticData = staticData;
        }

        public StaticData StaticData { get; private set; }
    }



    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    [Serializable]

    /// <summary>
    /// Datastructure and content selection from shared memory
    /// </summary>
    public struct StaticData
    {

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String SMVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String ACVersion;

        // session static info
        public int NumberOfSessions;
        public int NumCars;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String CarModel;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String Track;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String PlayerName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String PlayerSurname;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String PlayerNick;

        public int SectorCount;

        // car static info
        public int MaxRpm;
        public float MaxFuel;
        public int PenaltiesEnabled;
        public float AidFuelRate;
        public float AidTireRate;
        public float AidMechanicalDamage;
        public int AllowTyreBlankets;
        public float AidStability;
        public int AidAutoClutch;
        public int AidAutoBlip;
        public int PitWindowStart;
        public int PitWindowEnd;
        public int IsOnline;
    }
}
