using System.Text;
using CompilationSystem;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Ocsp;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:3636");

var app = builder.Build();


CompilationSystem.CompilationSystem compilation = new CompilationSystem.CompilationSystem();

app.UseWebSockets();

app.MapPost("/upload", async (HttpRequest request) =>
{
    if (!request.HasFormContentType)
    {
        return Results.BadRequest("O conteúdo deve ser multipart/form-data.");
    }

    var form = await request.ReadFormAsync();
    var arquivo = form.Files["Código"];

    if(arquivo is null)
        return Results.BadRequest("Nenhum arquivo encontrado. ");

    using var stream = arquivo.OpenReadStream();
    using var ms = new MemoryStream();
    await stream.CopyToAsync(ms);
    var filesBytes = ms.ToArray();

    Console.WriteLine($"Arquivo recebido: (arquivo.FileName) ({arquivo.Length} bytes)");

    return Results.Ok(new {mensagem = "upload realizado com sucesso!"});

    
});

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
      var msg = Encoding.UTF8.GetString(buffer, 0, resultvariable.Count);
      string data = new string(msg.Where(char.IsDigit).ToArray());
      compilation.CreateANewDiretory(msg, int.Parse(data));
      var response = Encoding.UTF8.GetBytes($"Echo: ${msg}");
      //await ws.SendAsync(response, System.Net.WebSockets.WebSocketMessageType.Binary, true, CancellationToken.None);
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