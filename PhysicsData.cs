using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ACCMetry
{
    public class PhysicsEventArgs : EventArgs
    {
        public PhysicsEventArgs(PhysicsData physicsData)
        {
            this.PhysicsData = physicsData;
        }

        public PhysicsData PhysicsData { get; private set; }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode)]
    [Serializable]
    public struct PhysicsData
    {

        public int PacketId;   //Current step index
        public float Gas;      //Gas pedal input
        public float Brake;    //Brake pedal input
        public float Fuel;     //Amount of fuel left in kg
        public int Gear;       //Current gear
        public int Rpm;        //Current RPM
        public float SteerAngle;
        public float SpeedKmh;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] Velocity;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] AccG;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] WheelSlip;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] WheelPressure;                              //Tyre pressures FL FR RL RR
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] WheelAngularSpeed;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] TyreCoreTemperature;                        //Tyre core temps FL FR RL RR
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] SuspensionTravel;                           //Suspension travel FL FR RL RR

        public float TC;
        public float Heading;
        public float Pitch;
        public float Roll;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public float[] CarDamage;                                  //Car damage front=0 , rear=1, left=2, right=3, center=4

        public int PitlimiterOn;
        public float ABS;
        public int AutoShifterOn;
        public float TurboBoost;
        public float AirTemp;
        public float RoadTemp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] LocalAngularVel;

        public float FinalFF;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] BrakeTemp;

        public float Clutch;
        public int IsAIControlled;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] TyreContactPoint;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] TyreContactNormal;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] TyreContactHeading;

        public float BrakeBias;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public float[] LocalVelocity;

        public float WaterTemp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public float[] BrakePressure;

    }
}
