﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InformationNode.Messages;
using Newtonsoft.Json;

namespace InformationNode
{
	class Node
	{
		private TcpListener listener; 
		public string FilePath { get; set; }
		public int Port { get; set; }
		public string Address { get; set; } = "127.0.0.1";
		public List<Node> LinkedNodes { get; set; } //ко мне подключаются
		public List<Node> MyNodes { get; set; } //я подключаюсь к
		public Node() { }
		public Node(string filePath, int port, string ip)
		{
			FilePath = filePath;
			Port = port;
			Address = ip;
			LinkedNodes = new List<Node>();
			MyNodes = new List<Node>();
		}
		public Node(int port, string ip):base()
		{
			Port = port;
			Address = ip;
		}

		public void Start()
		{
			try
			{
				var udpHandler = new UdpHandler();
				udpHandler.StartUdpMulticastListenerAsync(this);
				listener = new TcpListener(IPAddress.Parse(Address), Port);
				listener.Start();

				while (true)
				{
					TcpClient client = listener.AcceptTcpClient();
					Client clientObj = new Client(client,this);

					// создаем новый поток для обслуживания нового клиента
					Thread clientThread = new Thread(new ThreadStart(clientObj.Process));
					clientThread.Start();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				listener?.Stop();
			}
		}

		

		public List<Person> GetData()
		{
			if (File.Exists(FilePath))
			{
				var str = File.ReadAllText(FilePath);
				return JsonConvert.DeserializeObject<List<Person>>(str);
			}
			return new List<Person>();
		}

		
		public void SendConnect(Node node)
		{
			try
			{
				TcpClient client = new TcpClient(node.Address, node.Port);
				var stream = client.GetStream();
				Message msg = new Message()
				{
					Body = JsonConvert.SerializeObject(this),
					Type = "Connect",
					Author = Port + ""
				};
				byte[] data = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(msg));
				// отправка сообщения
				stream.Write(data, 0, data.Length);
				stream?.Close();
				client?.Close();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
