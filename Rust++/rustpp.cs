using System.Collections.Generic;
using UnityEngine;
using Facepunch.Utility;
using System.IO;
using System.Reflection;
using System.Collections;

	public class rustpp : ConsoleSystem
	{

		public static Hashtable shared_doors = new Hashtable ();

		public static bool checkowner(ulong ownerID, Controllable controllable) {

			if (ownerID == controllable.playerClient.userID)
				return true;
			ArrayList list = (ArrayList)shared_doors [ownerID];
			if(list == null)
				return false;
			if (list.Contains (controllable.playerClient.userID)) {
				return true;
			} else
				return false;
			
		}

	[ConsoleSystem.User]
	public static void unshare(ref ConsoleSystem.Arg arg, ref string[] chatargs) {
		if(config.GetSetting("Commands", "unshare") == "false") {
			API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
			return;
		}
		if (chatargs.Length > 1) {
			string name = "";
			for(int i=0; i < chatargs.Length - 1; i++) {
				name += chatargs[i + 1].Replace("\"", "") + " ";
			}
			name = name.Trim();
			
			if(name != null) {
				foreach (PlayerClient playerClient in PlayerClient.All) {
					ulong targetID = playerClient.userID;
					ulong playerID = arg.argUser.userID;
					if(playerClient.netUser.displayName.ToLower() == name.ToLower()) {
						if(targetID == playerID) {
							API.sayUser (arg.argUser.networkPlayer, "Why would you unshare with yourself?");
							return;
						}
						
						ArrayList list = (ArrayList)shared_doors[playerID];
						
						if(list != null) {
							if(list.Contains(targetID)) {
								list.Remove(targetID);
								API.sayUser (arg.argUser.networkPlayer, "You have stopped sharing doors with " + playerClient.netUser.displayName);
								API.sayUser (playerClient.netPlayer, arg.argUser.displayName + " has stopped sharing doors with you");
								return;
							} 
						} 

					}
				}
				API.sayUser (arg.argUser.networkPlayer, "No player found with the name: " + name);
			}
		} else {
			API.sayUser (arg.argUser.networkPlayer, "Sharing Doors Usage:   /unshare \"playerName\"");
		}
		
	}	


	[ConsoleSystem.User]
	public static void share(ref ConsoleSystem.Arg arg, ref string[] chatargs) {
		if(config.GetSetting("Commands", "share") == "false") {
			API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
			return;
		}
		if (chatargs.Length > 1) {
			string name = "";
			for(int i=0; i < chatargs.Length - 1; i++) {
				name += chatargs[i + 1].Replace("\"", "") + " ";
			}
			name = name.Trim();
			if(name != null) {
				foreach (PlayerClient playerClient in PlayerClient.All) {
					ulong targetID = playerClient.userID;
					ulong playerID = arg.argUser.userID;
					if(playerClient.netUser.displayName.ToLower() == name.ToLower()) {
						if(targetID == playerID) {
							API.sayUser (arg.argUser.networkPlayer, "Why would you share with yourself?");
							return;
						}

						ArrayList list = (ArrayList)shared_doors[playerID];

						if(list != null) {
							if(list.Contains(targetID)) {
								API.sayUser (arg.argUser.networkPlayer, "Doors were already shared with " + playerClient.netUser.displayName);
								return;
							} 
						} else {
							list = new ArrayList();
							shared_doors.Add (playerID, list);
						}
						list.Add (targetID);
						API.sayUser (arg.argUser.networkPlayer, "You have shared all doors with " + playerClient.netUser.displayName);
						API.sayUser (playerClient.netPlayer, arg.argUser.displayName + " has shared all doors with you");
						return;
					}
				}
				API.sayUser (arg.argUser.networkPlayer, "No player found with the name: " + name);
			}
		} else {
			API.sayUser (arg.argUser.networkPlayer, "Sharing Doors Usage:   /share \"playerName\"");
		}
		
	}	

		public static Hashtable starterkits = new Hashtable();
		public static bool disco_mode = false;
		public static float old_timescale;
		public static IniParser config;
		public static string server_message_name = "null";

		static rustpp ()
		{
			string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "rust++.cfg"));
			config = new IniParser (@path);
		}

		[ConsoleSystem.Admin]
		public static void day(ref ConsoleSystem.Arg arg) {
			API.time = 12;
		}

		[ConsoleSystem.User]
		public static void ping(ref ConsoleSystem.Arg arg) {
			if(config.GetSetting("Commands", "ping") == "false") {
				API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
				return;
			}
			API.sayUser(arg.argUser.networkPlayer, "Ping: " + arg.argUser.networkPlayer.lastPing);
		}
		
		[ConsoleSystem.Admin]
		public static void night(ref ConsoleSystem.Arg arg) {
			API.time = 0;
		}


		[ConsoleSystem.User]
		public static void history(ref ConsoleSystem.Arg arg) {
			if(config.GetSetting("Commands", "history") == "false") {
				API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
				return;
			}
			for (int i=1 + (int.Parse(config.GetSetting("Settings", "chat_history_amount"))); i > 0; i--) {
				if(chat.users.Count >= i) {
					string user = (string)chat.users[chat.users.Count - i];
					string message = (string)chat.messages[chat.messages.Count - i];
					if(user != null) {
						API.say (arg.argUser.networkPlayer, user, message);
					}
				}
			}
		}

		[ConsoleSystem.User]
		public static void location(ref ConsoleSystem.Arg arg) {
			if(config.GetSetting("Commands", "location") == "false") {
				API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
				return;
			}
			string partialNameOrIDInt = arg.argUser.displayName;
			foreach (PlayerClient playerClient in PlayerClient.FindAllWithString(partialNameOrIDInt)) {
				string s = "Location: X: " + (int)playerClient.lastKnownPosition.x + " Y: " + (int)playerClient.lastKnownPosition.y + " Z: " + (int)playerClient.lastKnownPosition.z;
				arg.ReplyWith (s);
				API.sayUser (arg.argUser.networkPlayer, "Your Location Is: X: " + (int)playerClient.lastKnownPosition.x + " Y: " + (int)playerClient.lastKnownPosition.y + " Z: " + (int)playerClient.lastKnownPosition.z);
			}
		}
		
		[ConsoleSystem.User]
		public static void players(ref ConsoleSystem.Arg arg) {

			if(config.GetSetting("Commands", "players") == "false") {
				API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
				return;
			}
			API.sayUser(arg.argUser.networkPlayer, PlayerClient.All.Count + "  Players Online:");
			int count = 0;
			int totalcount = 0;
			string str = "";
			
			foreach (PlayerClient playerClient in PlayerClient.All) {
					totalcount++;
					if(totalcount >= 60) {
						count = 0;
						break;
					}
					str+=playerClient.userName + ",  ";
					if(count == 6) {
						count = 0;
						API.sayUser(arg.argUser.networkPlayer, str.Substring(0, str.Length - 3));
						str = "";
					}	else {
						count++;
					}
				}
			if (count != 0) {
				API.sayUser (arg.argUser.networkPlayer, str.Substring (0, str.Length - 3));

			}

		}

		
		[ConsoleSystem.Admin]
		public static void shutdown(ref ConsoleSystem.Arg arg) {
			TimedEvents.shutdown ();
		}

		[ConsoleSystem.User]
		public static void help(ref ConsoleSystem.Arg arg) {
			if(config.GetSetting("Commands", "help") == "false") {
				API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
				return;
			}
			API.sayUser (arg.argUser.networkPlayer, "RUST++ Mod");
			API.sayUser (arg.argUser.networkPlayer, config.GetSetting("Settings", "help_string"));
			
		}	

		[ConsoleSystem.User]
		public static void pm(ref ConsoleSystem.Arg arg, ref string[] chatargs) {
			if(config.GetSetting("Commands", "pm") == "false") {
				API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
				return;
			}
			if (chatargs.Length > 2) {
				string name = chatargs[1].Replace("\"", "");
				string message = chatargs[2].Replace("\"", "");
				if(name != null && message != null) {
					foreach (PlayerClient playerClient in PlayerClient.All) {
						if(playerClient.netUser.displayName.ToLower() == name.ToLower()) {
							API.say (playerClient.netPlayer, "\"PM from " + arg.argUser.displayName + "\"", "\""+message+"\"");
							API.say (arg.argUser.networkPlayer, "\"PM to " + playerClient.netUser.displayName + "\"", "\""+message+"\"");
							return;
						}
					}
				API.sayUser (arg.argUser.networkPlayer, "No player found with the name: " + name);
			}
			} else {
				API.sayUser (arg.argUser.networkPlayer, "Private Message Usage:   /pm \"player\" \"message\"");
			}
			
		}	

		[ConsoleSystem.User]
		public static void starter(ref ConsoleSystem.Arg arg) {
			if(config.GetSetting("Commands", "starter") == "false") {
				API.sayUser(arg.argUser.networkPlayer, "This feature has been disabled on this server");
				return;
			}
			bool okay = false;
			if (!starterkits.ContainsKey (arg.argUser.playerClient.userID)) {
				okay = true;
				starterkits.Add (arg.argUser.playerClient.userID, System.Environment.TickCount);
			} else {
				int ms = (int)starterkits[arg.argUser.playerClient.userID];
				
			if(System.Environment.TickCount - ms < int.Parse(config.GetSetting("Settings", "starterkit_cooldown")) * 1000) {
					API.sayUser(arg.argUser.networkPlayer, "You must wait awhile before using this..");
				} else {
					okay = true;
					starterkits.Remove (arg.argUser.playerClient.userID);
				starterkits.Add (arg.argUser.playerClient.userID, System.Environment.TickCount);
				}
			}
			if (!okay)
				return;
			for(int i=1; i < int.Parse (config.GetSetting("StarterKit", "items") + 1); i++) {
				string[] outer = {
					config.GetSetting("StarterKit", "item" + i + "_name"),
					config.GetSetting("StarterKit", "item" + i + "_amount")
				};
				arg.Args = outer;
				inv.give (ref arg);
			}
			API.sayUser(arg.argUser.networkPlayer, "You have spawned a Starter Kit!");
		}



	public static void handleCommand(ref ConsoleSystem.Arg arg) {

		string name = arg.argUser.user.Displayname;
		string command = arg.GetString(0, "text").Trim();
		uLink.NetworkPlayer player = arg.argUser.networkPlayer;
		Debug.Log((object) ("[HANDLE COMMAND:] " + name + " " + command));
		string[] tmp = command.Split (' ');
		command = tmp [0].Trim ();
		for (int i=3; i < tmp.Length; i++)
			tmp [2] += " " + tmp [i];
		switch (command) {

			case "/share": 
				share (ref arg, ref tmp);
				break;

			case "/unshare": 
				unshare (ref arg, ref tmp);
				break;

			case "/pm": 
				pm (ref arg, ref tmp);
				break;

			case "/ping": 
				ping (ref arg);
				break;

			case "/help": 
				help (ref arg);
				break;
				
			case "/location":
				location (ref arg);
				break;

			case "/history":
				history (ref arg);
				break;

			case "/players":
				players (ref arg);
				break;

			case "/starter":
				starter (ref arg);
				break;


			default:
				API.sayUser (player, "Invalid Command! type /help for a list of commmands");
				break;
		}

	}

}
