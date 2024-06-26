using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Net.Security;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void InserDb ()
    {
        // Put your code here
        string connectionString = "Server=localhost;Database=ayudenme;User Id=prueba;Password=123;Encrypt=True;TrustServerCertificate=True;";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "INSERT INTO datos (prueba) VALUES ('ayudenme')";
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                int rowsAffected = command.ExecuteNonQuery();

                // Optionally, send some feedback to the user
                SqlContext.Pipe.Send($"Rows affected: {rowsAffected}");
            }
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CallApi()
    {
        string apiUrl = "https://localhost:7050/WeatherForecast"; // Replace with your API URL

        ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(IgnoreCertificateValidationCallback);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
        request.Method = "GET";

        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    SqlContext.Pipe.Send(result);
                }
            }
            else
            {
                SqlContext.Pipe.Send("Error: " + response.StatusCode.ToString());
            }
        }
    }

    private static bool IgnoreCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return true; // Ignore all certificate validation errors
    }
}
