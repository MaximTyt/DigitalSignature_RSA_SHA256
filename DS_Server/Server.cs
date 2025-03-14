﻿using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using DS_Lib;
class DSServer
{
    private static string basePath = "../../../keys/";
    private static string publicKey_server = basePath + "publicKey_server.pem";
    private static string privateKey_server = basePath + "privateKey_server.pem";

    private static DS eds = new DS();
    static async Task Main(string[] args)
    {
        Console.CancelKeyPress += Console_CancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;        
        Directory.CreateDirectory(basePath);
        GenerateKeys();
        var listener = new TcpListener(IPAddress.Any, 5000);
        listener.Start();
        Console.WriteLine("Сервер запущен...");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }       
    }

    
    static void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        if (Directory.Exists(basePath))
            Directory.Delete(basePath, true);
        Console.WriteLine("Exiting");        
        Environment.Exit(0);
    }
    static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
    {
        if (Directory.Exists(basePath))
            Directory.Delete(basePath, true);
        Console.WriteLine("exit");
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            string response = ProcessRequest(request);
            byte[] responseData = Encoding.UTF8.GetBytes(response);
            await stream.WriteAsync(responseData, 0, responseData.Length);
        }
    }

    private static string ProcessRequest(string request)
    {
        if (request == "GET_PUBLIC_KEY")
        {
            return eds.getPublicKeyPem();
        }
        else if (request.StartsWith("VERIFY_SIGNATURE:"))
        {
            var parts = request.Split(':');
            string message = parts[1];
            byte[] signature = Convert.FromBase64String(parts[2]);
            RSAParameters publicKey_client = DeserializeKeyPem(parts[3]);
           
            bool isVerified = eds.VerifyData(message, signature, publicKey_client);
            return isVerified.ToString();
        }
        else if (request == "GET_RANDOM_MESSAGE")
        {
            string randomMessage = GenerateRandomMessage();
            byte[] signature = eds.SignData(randomMessage);
            return $"{randomMessage}:{Convert.ToBase64String(signature)}";
        }

        return "UNKNOWN_REQUEST";
    }

    private static void GenerateKeys()
    {
        eds.GenerateKeys(out string pbKey, out string prKey);
        File.WriteAllBytes(publicKey_server, Encoding.ASCII.GetBytes(pbKey));
        File.WriteAllBytes(privateKey_server, Encoding.ASCII.GetBytes(prKey));
    }
        

    private static RSAParameters DeserializeKeyPem(string key)
    {
        using (RSA rsa = RSA.Create())
        {
            rsa.ImportFromPem(key);
            return rsa.ExportParameters(false);
        }
    }    

    private static string GenerateRandomMessage()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());
    }   
}