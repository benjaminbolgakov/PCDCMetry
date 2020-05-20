using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACCMetry
{

    #region Enums

    /// <summary>
    /// Holds session states
    /// </summary>
    public enum SESSION
    {
        UNKOWN = -1,
        PRACTICE = 0,
        QUALIFYING = 1,
        RACE = 2,
        HOTLAP = 3,
        TIMETRIAL = 4,
        DRIFT = 5,
        DRAG = 6,
        HOTSTINT = 7,
        SUPERPOLE = 8
    }
    /// <summary>
    /// Holds the flag state of the session
    /// </summary>
    public enum FLAG
    {
        NO_FLAG = 0,
        BLUE = 1,
        YELLOW = 2,
        BLACK = 3,
        WHITE = 4,
        CHECKERED = 5,
        PENALTY = 6,
        GREEN = 7
    }
    /// <summary>
    /// Holds the penalty type states
    /// </summary>
    public enum PENALTY_TYPE
    {
        None = 0,
        DriveThrough_Cutting = 1,
        StopAndGo_10_Cutting = 2,
        StopAndGo_20_Cutting = 3,
        StopAndGo_30_Cutting = 4,
        Disqualified_Cutting = 5,
        RemoveBestLaptime_Cutting = 6,
        DriveThrough_PitSpeeding = 7,
        StopAndGo_10_PitSpeeding = 8,
        StopAndGo_20_PitSpeeding = 9,
        StopAndGo_30_PitSpeeding = 10,
        Disqualified_PitSpeeding = 11,
        RemoveBestLaptime_PitSpeeding = 12,
        Disqualified_IgnoredMandatoryPit = 13,
        PostRaceTime = 14,
        Disqualified_Trolling = 15,
        Disqualified_PitEntry = 16,
        Disqualified_PitExit = 17,
        Disqualified_Wrongway = 18,
        DriveThrough_IgnoredDriverStint = 19,
        Disqualified_IgnoredDriverStint = 20,
        Disqualified_ExceededDriverStintLimit = 21

    }

    /// <summary>
    /// Holds the game status states
    /// </summary>
    public enum STATUS
    {
        AC_OFF = 0,
        AC_REPLAY = 1,
        AC_LIVE = 2,
        AC_PAUSE = 3
    }
    /// <summary>
    /// Holds wheel type states
    /// </summary>
    public enum WHEEL_TYPE
    {
        FrontLeft = 0,
        FrontRight = 1,
        RearLeft = 2,
        RearRight = 3
    }

    #endregion


    public class GraphicsEventArgs : EventArgs
    {
        public GraphicsEventArgs(GraphicsData graphicsData)
        {
            this.GraphicsData = graphicsData;
        }

        public GraphicsData GraphicsData { get; private set; }
    }

    


    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    [Serializable]
    public struct GraphicsData
    {

        public int PacketId;
        public STATUS Status;
        public SESSION Session;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String CurrentTime;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String LastLaptime;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String BestLaptime;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String Split;

        public int CompletedLaps;
        public int Position;
        public int MilliCurentTime;
        public int MilliLastTime;
        public int MilliBestTime;
        public float SessionTimeLeft;
        public float DistanceTraveled;
        public int IsInPits;
        public int CurrentSectorIndex;
        public int LastSectorTime;
        public int NumberOfLaps;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String TyreCompound;

        public float NormalizedCarPosition;
        public int ActiveCars;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] CarCoordinates;


        public int CarID;

        public int PlayerCarID;
        public float PenaltyTime;

        public FLAG Flag;
        public PENALTY_TYPE Penalty;

        public int IdealLineOn;
        public int IsInPitLane;
        public float SurfaceGrip;
        public int MandatoryPitDone;
        public float WindSpeed;
        public float WindDirection;
        public int IsSetupMenuVisible;
        public int MainDisplayIndex;
        public int SecondaryDisplayIndex;
        public int TC;
        public int TCCut;
        public int EnginaMap;
        public int ABS;
        public float fuelPerLap;
        public int RainLights;
        public int FlashingLight;
        public int LightStages;
        public float ExhaustTemperature;
        public int WiperStage;
        public int DriverStintTotalTimeLeft;
        public int DriverStintTimeLeft;
        public int RainTyres;

        
        public int SessionIndex;
        public float UsedFuel;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String DeltaLapTime;

        public int MilliDeltaLaptime;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
        public String EstimatedLaptime;

        public int MilliEstimatedLaptime;
        public int MilliDeltaPositive;
        public int MilliSplit;
        public int IsValidLap;
        public float FuelEstimatedLaps;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
        public String TrackStatus;

        public int MissingMandatoryPit;

    }
}
