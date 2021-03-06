﻿using System;
using System.Net.Sockets;
using System.Text;
using Mediator.Messages;

namespace Mediator
{
	class Client
	{
		public TcpClient client;
		public string ClientName { get; set; }
		public Mediator InitMediator{ get; set; }
		public Client(TcpClient tcpClient, Mediator med)
		{
			client = tcpClient;
			InitMediator = med;
		}


		public void Process()
		{
			NetworkStream stream = null;
			try
			{
				stream = client.GetStream();
				
				byte[] data = new byte[1000000]; // буфер для получаемых данных
				// получаем сообщение
				StringBuilder builder = new StringBuilder();
				int bytes = 0;
				do
				{
					bytes = stream.Read(data, 0, data.Length);
					builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
				} while (stream.DataAvailable);

				Message msg = Message.Create(builder.ToString(), this);
				string response = msg.GetResponse();
				data = Encoding.Unicode.GetBytes(response);
				stream.Write(data, 0, data.Length);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			finally
			{
				stream?.Close();
				client?.Close();
			}
		}
	}
}
