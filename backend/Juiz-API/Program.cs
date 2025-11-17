
using System.Text;
using CompilationSystem;

List<string> responses;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:3636");

var app = builder.Build();


//CompilationSystem.CompilationSystem compilation = new CompilationSystem.CompilationSystem();


app.UseWebSockets();

app.Map("/", async context =>
{
  if (context.WebSockets.IsWebSocketRequest)
  {
    using var ws = await context.WebSockets.AcceptWebSocketAsync();
    Console.ForegroundColor = ConsoleColor.DarkGreen;
    Console.WriteLine("User Connected!");
    var buffer = new Byte[1024 * 4];
    while (ws.State == System.Net.WebSockets.WebSocketState.Open)
    {
      var resultvariable = await ws.ReceiveAsync(buffer, CancellationToken.None);
      if (resultvariable.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
      {
        await ws.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
        break;
      }
      var msg = Encoding.ASCII.GetString(buffer, 0, resultvariable.Count);
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine($"Recieved message: ${msg}");
      var response = Encoding.ASCII.GetBytes($"Echo: ${msg}");
      await ws.SendAsync(response, System.Net.WebSockets.WebSocketMessageType.Binary, true, CancellationToken.None);
    }
    if (ws.State == System.Net.WebSockets.WebSocketState.Closed)
    {
      Console.ForegroundColor = ConsoleColor.DarkRed;
      Console.WriteLine("Closed connection");
    }
  }
  else
  {
    context.Response.StatusCode = 400;
  }
});

app.Run();