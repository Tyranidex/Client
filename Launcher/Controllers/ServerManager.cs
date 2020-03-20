﻿using System.Collections.Generic;

namespace Launcher
{
	public class ServerManager
	{
		public List<Server> availableServers { get; private set; }

		public ServerManager(string[] servers)
		{
			availableServers = new List<Server>();
			LoadServers(servers);
		}

		public Server GetServer(int index)
		{
			if (index < 0 || index > availableServers.Count)
			{
				// value out of range
				return null;
			}

			return availableServers[index];
		}

		public bool LoadServer(string backendUrl)
		{
			// ping server
			string output = "";

			try
			{
				RequestHandler.ChangeBackendUrl(backendUrl);
				output = RequestHandler.RequestConnect();
			}
			catch
			{
				// connection to remote end point failed
				return false;
			}
			
			if (output == "" || output == null)
			{
				// data is corrupted
				return false;
			}

			// add server
			availableServers.Add(Json.Deserialize<Server>(output));
			return true;
		}

		public void LoadServers(string[] servers)
		{
			foreach (string backendUrl in servers)
			{
				LoadServer(backendUrl);
			}
		}
	}
}
