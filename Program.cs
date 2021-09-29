using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
namespace FreeVideoEditor
{
    struct BitmapHeader
    {

    }
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Path.AltDirectorySeparatorChar);
            switch (args.Length)
            {
                case 2:
                    Encode(args[0], args[1], 48000, 320);
                    break;
                case 3:
                    Encode(args[0], args[1], int.Parse(args[2]), 320);
                    break;
                case 4:
                    Encode(args[0], args[1], int.Parse(args[2]), int.Parse(args[3]));
                    break;
            }
        }

        public static string ffmpeg = Directory.GetCurrentDirectory().Replace('\\', '/') + @"\bin\ffmpeg.exe".Replace('\\', '/');
        public static string mp4 = Directory.GetCurrentDirectory().Replace('\\', '/') + @"\tmp\mp4\".Replace('\\', '/');
        public static string mp3 = Directory.GetCurrentDirectory().Replace('\\', '/') + @"\tmp\mp3\".Replace('\\', '/');
        public static string bgm = Directory.GetCurrentDirectory().Replace('\\', '/') + @"\tmp\mp3\bgm\".Replace('\\', '/');
        public static string tmp = Directory.GetCurrentDirectory().Replace('\\', '/') + @"\tmp\".Replace('\\', '/');


        public static string final = Directory.GetCurrentDirectory().Replace('\\', '/') + @"\tmp\final\".Replace('\\', '/');

        public static void SetPlaySpeed(string inputFile, string outputFile, float speed, int sampleLate, int startFrame, int frameCount)
        {

            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y" + " -i " + @"""" + inputFile + @"""" + " -start_number " + startFrame + " -vframes " + frameCount + " -vf setpts=PTS/" + speed + " -af aresample=" + sampleLate + ",asetrate=" + sampleLate + "*" + speed + " -ar 48000  " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void SetPlaySpeed(string inputFile, string outputFile, float speed, int sampleLate, int startFrame)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y" + " -i " + @"""" + inputFile + @"""" + " -start_number " + startFrame + " -vf setpts=PTS/" + speed + " -af aresample=" + sampleLate + ",asetrate=" + sampleLate + "*" + speed + " " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void JpgToMp4(string inputFile, string outputFile)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y -framerate 30 -i " + @"""" + inputFile + @"""" + " -vcodec libx264 -pix_fmt yuv420p" + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void Mp4ToMp3(string inputFile, string outputFile)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y -i " + @"""" + inputFile + @"""" + " -c copy " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void InsertAudioWithAudio(string inputVideoFile, string inputAudioFile, string outputFile, float volume)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y -i " + @"""" + inputVideoFile + @"""" + " -i " + @"""" + inputAudioFile + @"""" + @" -filter_complex ""[1] volume = " + volume + @"[bgm];[0][bgm] amix"" -shortest " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void InsertAudioWithoutAudio(string inputVideoFile, string inputAudioFile, string outputFile)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y -i " + @"""" + inputVideoFile + @"""" + " -i " + @"""" + inputAudioFile + @"""" + @" -shortest " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
        }
        public static void InsertText(string inputFile, string outputFile, string fontName, float fontSize, string text, float x, float y, string color)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            File.WriteAllText(tmp + "effect.txt", @"drawtext=fontfile=" + fontName + @":text=" + text + ":x=" + x + @":y=" + y + ":fontsize=" + fontSize + ":fontcolor=" + color);
            processStartInfo.Arguments = "-y -i " + @"""" + inputFile + @"""" + @" -filter_script:v """ + tmp + @"effect.txt"" """ + outputFile + @"""";
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
            File.Delete(tmp + "effect.txt");
        }
        public static int TimeToFrames(string timeToFrame, int frameLate)
        {
            string[] rows = timeToFrame.Split(':');
            return int.Parse(rows[0]) * 60 * 60 * frameLate + int.Parse(rows[1]) * 60 * frameLate + int.Parse(rows[2]) * frameLate + int.Parse(rows[3]);
        }
        public static void Encode(string projectPath, string encodePath, int audioBitrate, int videoBitrate)
        {
            try
            {
                string vEFile = File.ReadAllText(projectPath);
                string vEDir = new FileInfo(projectPath).DirectoryName;
                Console.WriteLine(vEDir);
                Dictionary<string, string> defines = new Dictionary<string, string>();
                vEFile = vEFile.Replace("\r\n", "\n");
                int mode = 0;
                string fileName = "";
                string text = "";
                List<string> text_args = new List<string>();
                string tag = "";
                List<string> bgms = new List<string>();
                List<string> tags = new List<string>();
                float volume = 0;
                foreach (string line in vEFile.Split('\n'))
                {
                    string[] rows = line.Replace("\t", "").Split(' ');
                    if (mode == 2 && rows[0] != "}")
                    {
                        text += line.Replace("\t", "");
                    }
                    switch (rows[0].ToUpper())
                    {
                        //サブコマンド
                        case "DEFINE":
                            if (mode != 0)
                                break;
                            defines.Add(rows[1], string.Join(' ', rows[2..]));
                            break;
                        case "BGM":
                            if (defines.ContainsKey(rows[1]))
                            {
                                switch (rows.Length)
                                {
                                    case 4:
                                        SetPlaySpeed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), bgm + rows[1] + ".mp3", float.Parse(rows[2]), 48000, TimeToFrames(rows[3], 30));
                                        break;
                                    case 5:
                                        SetPlaySpeed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), bgm + rows[1] + ".mp3", float.Parse(rows[2]), 48000, TimeToFrames(rows[3], 30), TimeToFrames(rows[4], 30));
                                        break;
                                }
                                bgms.Add(defines[rows[1]]);
                            }
                            break;
                        case "VIDEO":
                            if (mode != 0)
                                break;
                            if (defines.ContainsKey(rows[1]))
                            {
                                fileName = defines[rows[1]];
                                tag = rows[1];
                                switch (rows.Length)
                                {
                                    case 4:
                                        SetPlaySpeed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + fileName.Replace('\\', '/'), mp4 + "temp2.mp4", float.Parse(rows[2]), 48000, TimeToFrames(rows[3], 30));
                                        break;
                                    case 5:
                                        SetPlaySpeed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + fileName.Replace('\\', '/'), mp4 + "temp2.mp4", float.Parse(rows[2]), 48000, TimeToFrames(rows[3], 30), TimeToFrames(rows[4], 30));
                                        break;
                                }
                            }
                            break;
                        case "TEXT":
                            if (mode != 1)
                                break;
                            text_args.AddRange(rows);
                            break;
                        case "AUDIO":
                            if (mode != 1)
                                break;
                            if (defines.ContainsKey(rows[1]))
                            {
                                switch (rows.Length)
                                {
                                    case 5:
                                        SetPlaySpeed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), mp3 + rows[1] + ".mp3", float.Parse(rows[3]), audioBitrate, TimeToFrames(rows[4], 30));
                                        break;
                                    case 6:
                                        SetPlaySpeed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), mp3 + rows[1] + ".mp3", float.Parse(rows[3]), audioBitrate, TimeToFrames(rows[4], 30), TimeToFrames(rows[5], 30));
                                        break;
                                }
                                InsertAudioWithAudio(mp4 + "temp2.mp4", mp3 + rows[1] + ".mp3", mp4 + @"temp1.mp4", float.Parse(rows[2]));
                                File.Copy(mp4 + "temp1.mp4", mp4 + @"temp2.mp4", true);
                            }
                            break;
                        case "{":
                            if (rows.Length > 1)
                                break;
                            mode++;
                            break;
                        case "}":
                            if (rows.Length > 1)
                                break;
                            if (mode == 0)
                            {
                                break;
                            }
                            if (mode == 1)
                            {
                                File.Copy(mp4 + "temp2.mp4", final.Replace('\\', '/') + tag + ".mp4", true);
                                tags.Add(final.Replace('\\', '/') + tag + ".mp4");
                                foreach (string path in Directory.GetFiles(mp4))
                                {
                                    File.Delete(path);
                                }
                                fileName = "";
                            }
                            if (mode == 2)
                            {
                                if (defines.ContainsKey(text_args[1]))
                                    InsertText(mp4 + "temp2.mp4", mp4 + "temp.mp4", vEDir.Replace('\\', '/').Replace(":", "\\\\:") + "\\".Replace('\\', '/') + defines[text_args[1]].Replace('\\', '/'), float.Parse(text_args[2]), text, float.Parse(text_args[3].Split(',')[0]), float.Parse(text_args[3].Split(',')[1]), text_args[4]);
                                File.Copy(mp4 + "temp.mp4", mp4 + "temp2.mp4", true);
                                text_args.Clear();
                                text = "";
                            }
                            mode--;
                            break;
                    }
                }
                string commandArgs = "-y";
                foreach (string t in tags)
                {
                    commandArgs += " -i " + t + " -b:v " + videoBitrate + "k" + " -b:a " + audioBitrate + "k";
                }
                commandArgs += @" -acodec copy -vcodec copy " + tmp + @"last.mp4".Replace('\\', '/');
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = ffmpeg;
                processStartInfo.UseShellExecute = false;
                processStartInfo.Arguments = commandArgs;
                Process process = Process.Start(processStartInfo);
                process.WaitForExit();
                commandArgs = "-y";
                if (bgms.Count != 0)
                {
                    foreach (string bgm in bgms)
                    {
                        commandArgs += " -i " + bgm + "-b:a" + audioBitrate + "k";
                    }
                    commandArgs += @" -acodec copy " + tmp + "last.mp3";
                    processStartInfo = new ProcessStartInfo();
                    processStartInfo.FileName = ffmpeg;
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.Arguments = commandArgs;
                    process = Process.Start(processStartInfo);
                    process.WaitForExit();
                }
                if (defines.ContainsKey("BGM_VOLUME"))
                    volume = float.Parse(defines["BGM_VOLUME"]);
                else
                    volume = 0.4f;
                if (File.Exists(@"tmp\last.mp3".Replace('\\', '/')))
                {
                    if (defines.ContainsKey("ENCODE_PATH"))
                        process = Process.Start(ffmpeg, @"-y -i """ + tmp + @"last.mp4"" -i """ + tmp + @"last.mp3"" -filter_complex ""[1] volume = " + volume + @" [bgm];[0][bgm] amix"" -acodec copy -vcodec copy """ + vEDir.Replace('\\', '/') + defines["ENCODE_PATH"] + @"""");
                    else
                        process = Process.Start(ffmpeg, @"-y -i """ + tmp + @"last.mp4"" -i """ + tmp + @"last.mp3"" -filter_complex ""[1] volume = " + volume + @" [bgm];[0][bgm] amix"" -acodec copy -vcodec copy """ + vEDir.Replace('\\', '/') + encodePath.Replace('\\', '/') + @"""");
                }
                else
                {
                    if (defines.ContainsKey("ENCODE_PATH"))
                        File.Copy(tmp + "last.mp4".Replace('\\', '/'), vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines["ENCODE_PATH"].Replace('\\', '/'), true);
                    else if (encodePath != "")
                        File.Copy(tmp + "last.mp4".Replace('\\', '/'), vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + encodePath.Replace('\\', '/'), true);
                }
                process.WaitForExit();
                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            foreach (string path in Directory.GetFiles(final.Replace('\\', '/')))
            {
                File.Delete(path);
            }
        }
    }
}
