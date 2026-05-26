using System;
using System.IO;
using System.Threading.Tasks;

namespace MusicPlayer.App;

public static class FileHelper
{
    public static async Task<string> SimulateFileReadAsync(string path)
    {
        if (path is null) throw new ArgumentNullException(nameof(path));

        const int maxRetries = 3;
        int attempt = 0;
        int delayMs = 200;

        while (true)
        {
            try
            {
                return await File.ReadAllTextAsync(path).ConfigureAwait(false);
            }
            catch (Exception) when (attempt < maxRetries)
            {
                await Task.Delay(delayMs).ConfigureAwait(false);
                attempt++;
                delayMs *= 2;
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to read file '{path}' after {attempt} attempts.", ex);
            }
        }
    }
}
