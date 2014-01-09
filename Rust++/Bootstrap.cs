using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Facepunch;
using LeatherLoader;
using UnityEngine;
using System.Timers;

namespace RustPP
{
	[Bootstrap]
	public class Bootstrap : Facepunch.MonoBehaviour
	{

		static Bootstrap() {

		}
		
		public void Awake()
		{
			//I copied this over from Dump Truck- don't think it's necessary for drop party, but don't have time to test removing it yet.
			//TOOD: Remember to see if I can take this out.
			DontDestroyOnLoad(this.gameObject);
		}

		public void Start()
		{
				Rust.Steam.Server.SetModded();
				Timer timed = new Timer ();
				timed.Interval = 30000; 
				timed.AutoReset = false;	
				timed.Elapsed += (x, y) => TimedEvents.startEvents ();
				timed.Start ();

		}

		public void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
		{

		}

	}
}
