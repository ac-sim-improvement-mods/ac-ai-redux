struct cai_car_controls { 
  /* all structures are aligned by 4 bytes */

  float gas;
  float brake;
  float clutch;
  float steer; /* normalize steer value from -1 to 1 */
  float handbrake;

  bool gear_up; /* single byte value, 1 for true, 0 for false */
  bool gear_dn;
  bool drs;
  bool kers;

  bool brake_balance_up;
  bool brake_balance_dn;
  bool abs_up;
  bool abs_dn;

  bool tc_up;
  bool tc_dn;
  bool turbo_up;
  bool turbo_dn;

  bool engine_brake_up;
  bool engine_brake_dn;
  bool mguk_delivery_up;
  bool mguk_delivery_dn;

  bool mguk_recovery_up;
  bool mguk_recovery_dn;
  byte mguh_mode; /* unsigned char */
  bool headlights;

  byte teleport_to; /* set to 1 to teleport to pits, set to 2 to teleport to `teleport_pos` */
  bool autoclutch_on_start;
  bool autoclutch_on_change;
  bool autoblip_active;

  float3 teleport_pos; /* three floats, 12 bytes in total */
  float3 teleport_dir;
}

struct cai_car_data {
  int packet_id; /* increments when updated */
  float gas;
  float brake;
  float clutch;
  float steer; /* steering angle in degrees */
  float handbrake;
  float fuel;
  int gear;
  float rpm;
  float speed_kmh;
  float3 velocity;
  float3 acc_g;  /* G-forces (Z is for acceleration force, X for left/right force) */
  float3 look;   /* car direction */
  float3 up;
  float3 position;
  float3 local_velocity;
  float3 local_angular_velocity;
  float cg_height;
  float car_damage[5];
  cai_wheel_data wheels[4];
  float turbo_boost;
  float final_ff;
  float final_pure_ff;
  bool pit_limiter;
  bool abs_in_action;
  bool traction_control_in_action;
  uint lap_time_ms;
  uint best_lap_time_ms;
  float drivetrain_torque;
  float spline_position;   /* position on AI spline (fast_lane.ai) */
  float collision_depth;   /* current collision depth in meters */
  uint collision_counter;  /* increments on collisions */
}

struct cai_wheel_data {
  float3 position;
  float3 contact_point;
  float3 contact_normal;
  float3 look;      /* wheel direction */
  float3 side;      /* vector pointing to wheel side */
  float3 velocity;  /* wheel velocity in world space */
  float slip_ratio;
  float load;
  float pressure;
  float angular_velocity;
  float wear;
  float dirty_level;
  float core_temperature;
  float camber_rad;
  float disc_temperature;
  float slip;
  float slip_angle_deg;
  float nd_slip;
};

struct cai_sim_control {
  bool pause;               /* set to 1 to pause simulation */
  bool restart_session;     /* set to 1 to restart current session */
  bool disable_collisions;  /* if set to 1, collisions are disabled */
  byte extra_sleep_ms;      /* in case your AI in development needs some time to come up with answer,
                               this option can slow down the simulation */
};

struct cai_debug_lines {
  int count;
  cai_debug_line lines[count];
}

struct cai_debug_line {
  float3 from;
  float3 to;
  uint color;
}
