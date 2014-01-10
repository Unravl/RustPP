using System.Collections.Generic;
using UnityEngine;
using Facepunch.Utility;
using System;
	public class API
	{
		static API ()
		{
		}

		public static void sayAll(string arg) {
			string str1 = Facepunch.Utility.String.QuoteSafe(rustpp.server_message_name);
			string str2 = Facepunch.Utility.String.QuoteSafe(arg);
			ConsoleNetworker.Broadcast("chat.add " + str1 + " " + str2);
		}

		//string str1 = Facepunch.Utility.String.QuoteSafe(rustpp.config.GetSetting("Settings", "system_message_nameTEST"));
	

		public static void sayUser(uLink.NetworkPlayer player, string arg) {
			string str1 = Facepunch.Utility.String.QuoteSafe(rustpp.server_message_name);
			string str2 = Facepunch.Utility.String.QuoteSafe(arg);
			ConsoleNetworker.SendClientCommand(player, "chat.add " + str1 + " " + str2);
		}

		public static void say(uLink.NetworkPlayer player, string playername, string arg) {
			string str1 = playername;
			string str2 = arg;
			ConsoleNetworker.SendClientCommand(player, "chat.add " + str1 + " " + str2);
		}

		public static void freezetime() {
			EnvironmentControlCenter.ServerSetTiming(new double?((double) (double) time), new double?(0.00));
		}

		public static float time
		{
			get {
				return (float) EnvironmentControlCenter.time;
			}
			set {
				EnvironmentControlCenter.ServerSetTiming(new double?((double) value), new double?(EnvironmentControlCenter.timeScale));
			}
		}

		public static void day() {
			API.time = 12;
		}

		public static void night() {
			API.time = 0;
		}
	}

