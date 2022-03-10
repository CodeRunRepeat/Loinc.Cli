using System;
using System.IO;

namespace Loinc.Cli;

class AuthTokenManager
{
    public void LoadAuthToken()
    {
        try
        {
            using (var stream = new FileStream(GetDataFileName(), FileMode.Open))
            using (TextReader reader = new StreamReader(stream))
            {
                var contents = reader.ReadToEnd();
                var bytes = System.Convert.FromBase64String(contents);
                contents = System.Text.Encoding.ASCII.GetString(bytes);
                DotNetEnv.Env.LoadContents(contents);
            }
        }
        catch (IOException)
        {
        }
    }

    public void SaveAuthToken(string username, string password)
    {
        try
        {
            throw new NotImplementedException("SaveAuthToken");
        }
        catch (IOException)
        {
        }
    }

    private string GetDataFileName()
    {
        string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\.loinc.cli";
        Directory.CreateDirectory(appDataFolder);

        string fileName = appDataFolder + @"\token.txt";
        return fileName;
    }
}