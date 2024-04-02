using System;
using System.IO;
using NAudio.Wave;

namespace ConsoleMusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                bool exitRequested = false;

                while (!exitRequested)
                {
                    string folderPath = @"C:\Users\YourUserName\Music\";

                    string[] musicFiles = Directory.GetFiles(folderPath, "*.mp3");

                    if (musicFiles.Length == 0)
                    {
                        Console.WriteLine("Təyin edilmiş qovluqda musiqi faylları tapılmadı.");
                        return;
                    }

                    Console.WriteLine("Mövcud mahnılar:");
                    for (int i = 0; i < musicFiles.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(musicFiles[i])}");
                    }

                    Console.WriteLine("Səsli ifadəni oxuduğunuz mahnının nömrəsini daxil edin:");
                    int songIndex;
                    while (!int.TryParse(Console.ReadLine(), out songIndex) || songIndex < 1 || songIndex > musicFiles.Length)
                    {
                        Console.WriteLine("Yanlış daxil etdiniz. Zəhmət mahnı nömrəsini düzgün daxil edin:");
                    }

                    string selectedSong = musicFiles[songIndex - 1];
                    Console.WriteLine($"İndi oxunur: {Path.GetFileNameWithoutExtension(selectedSong)}");

                    using (var audioFile = new AudioFileReader(selectedSong))
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play();

                        while (outputDevice.PlaybackState == PlaybackState.Playing)
                        {
                            // ESC tuşuna basıldığında çalma işlemini durdur
                            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                            {
                                outputDevice.Stop();
                                exitRequested = true;
                                break;
                            }

                            System.Threading.Thread.Sleep(100);
                        }
                    }

                    Console.WriteLine("Mahnı başa çatdı.");
                }

                Console.WriteLine("Program bağlanır...");
            }
        }
    }
}
