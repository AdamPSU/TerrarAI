using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerrarAI
{
	public class AgentClient : ModSystem
	{
		private ClientWebSocket? _websocket;
		private CancellationTokenSource? _cancellationTokenSource;
		private bool _isConnected = false;
		private bool _npcSpawned = false;
		private const string WS_URL = "ws://127.0.0.1:5000/agent";

		public override void OnWorldLoad()
		{
			_ = ConnectAsync();
			_npcSpawned = false;
		}

		public override void PostUpdateEverything()
		{
			if (!_npcSpawned && Main.netMode != 2)
			{
				Player player = Main.LocalPlayer;
				if (player != null && player.active && player.position.X > 100)
				{
					SpawnAgentNPC();
					_npcSpawned = true;
				}
			}
		}

		private void SpawnAgentNPC()
		{
			Player player = Main.LocalPlayer;
			if (player == null)
				return;

			int spawnX = (int)(player.position.X + 100);
			int spawnY = (int)player.position.Y;
			int npcType = ModContent.NPCType<Content.NPCs.AgentNPC>();

			NPC.NewNPC(null, spawnX, spawnY, npcType);
		}

		public override void OnWorldUnload()
		{
			DisconnectAsync().Wait(1000);
		}

		private async Task ConnectAsync()
		{
			try
			{
				_websocket = new ClientWebSocket();
				_cancellationTokenSource = new CancellationTokenSource();
				
				await _websocket.ConnectAsync(new Uri(WS_URL), _cancellationTokenSource.Token);
				_isConnected = true;
				
				_ = ReceiveLoopAsync();
			}
			catch (Exception ex)
			{
				Mod.Logger.Error($"Failed to connect to agent: {ex.Message}");
			}
		}

		private async Task DisconnectAsync()
		{
			_isConnected = false;
			_cancellationTokenSource?.Cancel();
			
			if (_websocket != null && _websocket.State == WebSocketState.Open)
			{
				await _websocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
			}
			
			_websocket?.Dispose();
			_cancellationTokenSource?.Dispose();
		}

		private async Task ReceiveLoopAsync()
		{
			var buffer = new byte[4096];
			
			while (_isConnected && _websocket != null && _websocket.State == WebSocketState.Open)
			{
				try
				{
					var result = await _websocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource!.Token);
					
					if (result.MessageType == WebSocketMessageType.Close)
					{
						await DisconnectAsync();
						break;
					}
					
					var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
					OnActionReceived(message);
				}
				catch (Exception ex)
				{
					Mod.Logger.Error($"Error receiving action: {ex.Message}");
					break;
				}
			}
		}

		public async Task SendGameStateAsync(object gameState)
		{
		}

		private void OnActionReceived(string message)
		{
		}
	}
}

