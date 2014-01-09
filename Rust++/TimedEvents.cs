

using System.Timers;
using Facepunch.Utility;

	public class TimedEvents
	{
		static TimedEvents ()
		{

		}

		public static bool init = false;
		public static void startEvents() {
			if (init == false) {
				
				init = true;

				rustpp.server_message_name = rustpp.config.GetSetting ("Settings", "system_message_name");
				
				if (rustpp.config.GetSetting ("Settings", "decay") == "false") {
					decay.decaytickrate = float.MaxValue;
					decay.deploy_maxhealth_sec = 0;
				}


				if (rustpp.config.GetSetting ("Settings", "pvp") == "true") {
					server.pvp = true;
				} else server.pvp = false;

				if (rustpp.config.GetSetting ("Settings", "instant_craft") == "true") {
					crafting.instant = true;
				} else crafting.instant = false;

				if (rustpp.config.GetSetting ("Settings", "sleepers") == "true") {
					sleepers.on = true;
				} else sleepers.on = false;

				if (rustpp.config.GetSetting ("Settings", "enforce_truth") == "true") {
					truth.punish = true;
				} else truth.punish = false;

				if (rustpp.config.GetSetting ("Settings", "freeze_time") == "true") {
					env.time = int.Parse (rustpp.config.GetSetting ("Settings", "time"));
					env.timescale = 0;
				}
				if (rustpp.config.GetSetting ("Settings", "decay") == "false") {
					decay.decaytickrate = float.MaxValue;
					decay.deploy_maxhealth_sec = 0;
				}
				if (rustpp.config.GetSetting ("Settings", "voice_proximity") == "false") {
					voice.distance = int.MaxValue;
				}
				if (rustpp.config.GetSetting ("Settings", "notice_enabled") == "true") {
					Timer advertise = new Timer ();
					advertise.Interval = int.Parse (rustpp.config.GetSetting ("Settings", "notice_interval"));
					advertise.AutoReset = true;	
					advertise.Elapsed += (x, y) => advertise_begin ();
					advertise.Start ();
					
				}
				/*if (rustpp.config.GetSetting ("Settings", "configurable_airdrops") == "true") {
					Timer airdrop = new Timer ();
					airdrop.Interval = int.Parse (rustpp.config.GetSetting ("Settings", "airdrop_interval")); 
					airdrop.AutoReset = true;
					airdrop.Elapsed += (x, y) => airdrop_begin ();
					airdrop.Start ();
				}*/
			}
		}

		static void airdrop_begin()
		{
			int count = int.Parse (rustpp.config.GetSetting ("Settings", "amount_of_airdrops"));
			for (int i=0; i < count; i++) {
					SupplyDropZone.CallAirDrop();
				}
		}

		

		public static void shutdown() {
			time = int.Parse (rustpp.config.GetSetting ("Settings", "shutdown_countdown"));
			Timer shutdown = new Timer();
			shutdown.Interval = 10000; 
			shutdown.AutoReset = true;
			shutdown.Elapsed += (x, y) => shutdown_tick();
			shutdown.Start();
			shutdown_tick ();
		}

		public static int time = 60;
		public static void shutdown_tick() {
			if (time == 0) {
			API.sayAll ("Server Shutdown NOW!");
			AvatarSaveProc.SaveAll();
			ServerSaveManager.AutoSave();

			} else {
				API.sayAll ("Server Shutting down in " + time + " seconds");
			}
			time -= 10;

		}

	static void advertise_begin()
	{
		for (int i=0; i < int.Parse(rustpp.config.GetSetting("Settings", "notice_messages_amount")); i++) {
			API.sayAll (rustpp.config.GetSetting("Settings", "notice" + (i + 1)));
		}
	}

		
	}


