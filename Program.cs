using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Numerics;
using System.Runtime.Serialization;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

class Program
{
    static void Main(string[] args)
    {
        cai_car_controls car;
        cai_car_data car_data;

        int last_packet_id = 0;
        car_data.packet_id = 0;

        car.gas = 0;
        car.brake = 1;
        car.clutch = 0;
        car.steer = 0;
        car.handbrake = 0;

        car.gear_up = false;
        car.gear_dn = false;
        car.drs = false;
        car.kers = false;

        car.brake_balance_up = false;
        car.brake_balance_dn = false;
        car.abs_up = false;
        car.abs_dn = false;

        car.tc_up = false;
        car.tc_dn = false;
        car.turbo_up = false;
        car.turbo_dn = false;

        car.engine_brake_up = false;
        car.engine_brake_dn = false;
        car.mguk_delivery_up = false;
        car.mguk_delivery_dn = false;

        car.mguk_recovery_up = false;
        car.mguk_recovery_dn = false;
        car.mguh_mode = 0;
        car.headlights = false;

        car.teleport_to = 0;
        car.autoclutch_on_start = true;
        car.autoclutch_on_change = false;
        car.autoblip_active = false;

        car.teleport_pos = new Vector3(0,0,0);
        car.teleport_dir = new Vector3(0,0,0);

        int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(cai_car_controls));

        try
        {
            using (MemoryMappedFile mmf = MemoryMappedFile.CreateOrOpen("AcTools.CSP.NewBehaviour.CustomAI.CarControls1.v0", size))
            {
                try
                {
                    using (MemoryMappedFile mmf2 = MemoryMappedFile.OpenExisting("AcTools.CSP.NewBehaviour.CustomAI.Car1.v0"))
                    {
                        while(true){
                            using (MemoryMappedViewStream stream = mmf2.CreateViewStream()){
                                int count = Marshal.SizeOf(typeof(cai_car_data));
                                byte[] readBuffer = new byte[count];
                                BinaryReader reader = new BinaryReader(stream);
                                readBuffer = reader.ReadBytes(count);
                                GCHandle handle = GCHandle.Alloc(readBuffer, GCHandleType.Pinned);
                                car_data = (cai_car_data) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(cai_car_data));
                                handle.Free();

                                Console.WriteLine(car_data.packet_id);
                            }

                            if (car_data.packet_id != last_packet_id){
                                last_packet_id = car_data.packet_id;
                            
                                car.gas = (float)1;
                                car.brake = 0;
                                car.clutch = 1;
                                car.steer = 0;
                                car.handbrake = 0;

                                if (car.gear_up == true){
                                    car.gear_up = false;
                                }
                                else{
                                    car.gear_up = true;
                                }
                                
                                car.gear_dn = false;
                                car.drs = false;
                                car.kers = false;

                                car.brake_balance_up = false;
                                car.brake_balance_dn = false;
                                car.abs_up = false;
                                car.abs_dn = false;

                                car.tc_up = false;
                                car.tc_dn = false;
                                car.turbo_up = false;
                                car.turbo_dn = false;

                                car.engine_brake_up = false;
                                car.engine_brake_dn = false;
                                car.mguk_delivery_up = false;
                                car.mguk_delivery_dn = false;

                                car.mguk_recovery_up = false;
                                car.mguk_recovery_dn = false;
                                car.mguh_mode = 0;
                                car.headlights = false;

                                car.teleport_to = 0;
                                car.autoclutch_on_start = false;
                                car.autoclutch_on_change = false;
                                car.autoblip_active = false;

                                car.teleport_pos = new Vector3(0,0,0);
                                car.teleport_dir = new Vector3(0,0,0);

                                using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
                                {
                                    accessor.Write(0, ref car);
                                }
                            }
                        };
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("MMF2 Memory-mapped file does not exist.");
                }                
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("MMF1 Memory-mapped file does not exist.");
        }     
    }
}

public struct cai_car_controls { 
    /* all structures are aligned by 4 bytes */

    public float gas;
    public float brake;
    public float clutch;
    public float steer; /* normalize steer value from -1 to 1 */
    public float handbrake;

    public bool gear_up; /* single byte value, 1 for true, 0 for false */
    public bool gear_dn;
    public bool drs;
    public bool kers;

    public bool brake_balance_up;
    public bool brake_balance_dn;
    public bool abs_up;
    public bool abs_dn;

    public bool tc_up;
    public bool tc_dn;
    public bool turbo_up;
    public bool turbo_dn;

    public bool engine_brake_up;
    public bool engine_brake_dn;
    public bool mguk_delivery_up;
    public bool mguk_delivery_dn;

    public bool mguk_recovery_up;
    public bool mguk_recovery_dn;
    public byte mguh_mode; /* unsigned char */
    public bool headlights;

    public byte teleport_to; /* set to 1 to teleport to pits, set to 2 to teleport to `teleport_pos` */
    public bool autoclutch_on_start;
    public bool autoclutch_on_change;
    public bool autoblip_active;

    public Vector3 teleport_pos; /* three floats, 12 bytes in total */
    public Vector3 teleport_dir;
}

public struct cai_car_data {
    public int packet_id; /* increments when updated */
    public float gas;
    public float brake;
    public float clutch;
    public float steer; /* steering angle in degrees */
    public float handbrake;
    public float fuel;
    public int gear;
    public float rpm;
    public float speed_kmh;
    public Vector3 velocity;
    public Vector3 acc_g;  /* G-forces (Z is for acceleration force, X for left/right force) */
    public Vector3 look;   /* car direction */
    public Vector3 up;
    public Vector3 position;
    public Vector3 local_velocity;
    public Vector3 local_angular_velocity;
    public float cg_height;
    public float[] car_damage;
    public cai_wheel_data[] wheels;
    public float turbo_boost;
    public float final_ff;
    public float final_pure_ff;
    public bool pit_limiter;
    public bool abs_in_action;
    public bool traction_control_in_action;
    public uint lap_time_ms;
    public uint best_lap_time_ms;
    public float drivetrain_torque;
    public float spline_position;   /* position on AI spline (fast_lane.ai) */
    public float collision_depth;   /* current collision depth in meters */
    public uint collision_counter;  /* increments on collisions */
}

public struct cai_wheel_data {
    public Vector3 position;
    public Vector3 contact_point;
    public Vector3 contact_normal;
    public Vector3 look;   /* wheel direction */
    public Vector3 side;   /* vector pointing to wheel side */
    public Vector3 velocity;  /* wheel velocity in world space */
    public float slip_ratio;
    public float load;
    public float pressure;
    public float angular_velocity;
    public float wear;
    public float dirty_level;
    public float core_temperature;
    public float camber_rad;
    public float disc_temperature;
    public float slip;
    public float slip_angle_deg;
    public float nd_slip;
};