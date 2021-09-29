using System;
using System.IO;
using System.Diagnostics;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace FreeVideoEditor
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/'));
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

        public static string ffmpeg = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + @"bin\ffmpeg".Replace('\\', '/');
        public static string mp4 = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + @"tmp\mp4\".Replace('\\', '/');
        public static string mp3 = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + @"tmp\mp3\".Replace('\\', '/');
        public static string bgm = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + @"tmp\mp3\bgm\".Replace('\\', '/');
        public static string tmp = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + @"tmp\".Replace('\\', '/');


        public static string final = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + @"tmp\final\".Replace('\\', '/');
        public static Dictionary<string, string> defines = new Dictionary<string, string>();
        public static List<string> bgms = new List<string>();
        public static List<string> tags = new List<string>();

        public static void SetMp4Speed(string inputFile, string outputFile, float speed, int sampleLate, float start, float end)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y" + " -i " + @"""" + inputFile + @"""" + " -ss " + start + " -t " + end + " -vf setpts=PTS/" + speed + " -an " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void SetMp4Speed(string inputFile, string outputFile, float speed, float sampleLate, float start)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y" + " -i " + @"""" + inputFile + @"""" + " -ss " + start + " -vf setpts=PTS/" + speed + " -an " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void SetMp3Speed(string inputFile, string outputFile, float speed, int sampleLate, float start, float end)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y" + " -i " + @"""" + inputFile + @"""" + " -ss " + start + " -t " + end + " -af aresample=" + sampleLate + ",asetrate=" + sampleLate + "*" + speed + " -shortest -ar 48000 " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void SetMp3Speed(string inputFile, string outputFile, float speed, float sampleLate, float start)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y" + " -i " + @"""" + inputFile + @"""" + " -ss " + start + " -af aresample=" + sampleLate + ",asetrate=" + sampleLate + "*" + speed + " -shortest -ar 48000 " + @"""" + outputFile + @"""";
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
        public static void SetVolume(string inputFile, string outputFile, float volume)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y -i " + @"""" + inputFile + @"""" + " -af volume=" + volume + "dB " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void InsertAudioWithAudio(string inputVideoFile, string inputAudioFile, string outputFile, float startTime)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y -i " + @"""" + inputVideoFile + @""" -itsoffset " + startTime + " -i " + @"""" + inputAudioFile + @"""" + @" -filter_complex amix=inputs=2:duration=longest " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void InsertAudioWithoutAudio(string inputVideoFile, string inputAudioFile, string outputFile, float startTime)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            processStartInfo.Arguments = "-y -i " + @"""" + inputVideoFile + @""" -itsoffset " + startTime + " -i " + @"""" + inputAudioFile + @"""" + @" " + @"""" + outputFile + @"""";
            Process process = Process.Start(processStartInfo);

            process.WaitForExit();
        }
        public static void ApplyEffect(string inputFile, string outputFile, string effects, int videoScopeNumber)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            processStartInfo.FileName = ffmpeg;
            processStartInfo.UseShellExecute = false;
            File.WriteAllText(tmp + "temp" + videoScopeNumber + ".txt", effects);
            processStartInfo.Arguments = "-y -i " + @"""" + inputFile + @"""" + @" -filter_script " + tmp + "temp" + videoScopeNumber + ".txt" + @" -pix_fmt yuv420p """ + outputFile + @"""";
            Process process = Process.Start(processStartInfo);
            process.WaitForExit();
        }
        public static string InsertText(string fontName, float fontSize, string text, float x, float y, string color, float startTime, float endTime)
        {
            string effect = @"";
            int linesCount = 0;
            foreach (string line in text.Split('\n'))
            {
                effect += @"drawtext=fontfile=" + fontName + @":text=" + line + ":x=-tw/2+" + x + @":y=-th/2+" + (y + (fontSize * linesCount)) + ":fontsize=" + fontSize + ":fontcolor=" + color + @":enable='between(t," + startTime + "," + endTime + ")',";
                linesCount++;
            }
            effect = effect.Remove(effect.Length - 1);
            return effect;
        }
        public static string InsertVibrance(float intesity, float rbal, float gbal, float bbal, float startTime, float endTime)
        {
            return @"vibrance=intensity=" + intesity + ":rbal=" + (rbal / 255.0f * 10) + ":gbal=" + (gbal / 255.0f * 10) + ":bbal=" + (bbal / 255.0f * 10) + @":enable='between(t," + startTime + "," + endTime + @")'";
        }
        public static float TimeToFrames(string timeToFrame)
        {
            string[] rows = timeToFrame.Split(':');
            return float.Parse(rows[0]) * 60 * 60 + float.Parse(rows[1]) * 60 + float.Parse(rows[2]);
        }
        public static int CurrentMode(string line)
        {
            if (line == "{")
                return 1;
            else if (line == "}")
                return -1;
            else
                return 0;
        }
        public static List<int[]> ProveVideoScopes(string text)
        {
            int[] scope = new int[2];
            int linesCount = 0;
            int mode = 0;
            List<int[]> fileScopes = new List<int[]>();
            foreach (string line in text.Split('\n'))
            {
                string[] rows = line.Replace("\t", "").Split(' ');
                int ret = CurrentMode(rows[0]);
                if (ret > 0 && mode == 0)
                {
                    scope[0] = linesCount;
                }
                if (ret < 0 && mode == 1)
                {
                    scope[1] = linesCount;
                    fileScopes.Add((int[])scope.Clone());
                }
                mode += ret;
                linesCount++;
            }

            return fileScopes;
        }
        public static void ProveDefines(string text)
        {
            int linesCount = 0;
            int mode = 0;
            foreach (string line in text.Split('\n'))
            {
                string[] rows = line.Replace("\t", "").Split(' ');
                int ret = CurrentMode(rows[0]);
                if (mode == 0 && rows[0].ToUpper() == "DEFINE")
                    defines.Add(rows[1], string.Join(' ', rows[2..]));
                mode += ret;
                linesCount++;
            }
        }
        public static void EncodeParts(int videoScopeNumber, string vEFile, string vEDir, string tag, int audioBitrate, int videoBitrate)
        {
            int mode = 0;
            List<string> text_args = new List<string>();
            string text = "";
            string drawTextEffects = "";
            string filterEffects = "";
            bool containsAudio = false;
            foreach (string line in vEFile.Split('\n'))
            {
                string[] rows = line.Replace("\t", "").Split(' ');
                if (mode == 2 && rows[0] != "}")
                {
                    text += line.Replace("\t", "") + "\n";
                }
                switch (rows[0].ToUpper())
                {
                    case "TEXT":
                        if (mode != 1)
                            break;
                        text_args.AddRange(rows);
                        break;
                    case "AUDIO":
                        if (mode != 1)
                            break;
                        containsAudio = true;
                        if (defines.ContainsKey(rows[1]))
                        {
                            switch (rows.Length)
                            {
                                case 6:
                                    SetMp3Speed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), mp3 + "temp4_" + videoScopeNumber + ".mp3", float.Parse(rows[3]), 48000, TimeToFrames(rows[5]));
                                    break;
                                case 7:
                                    SetMp3Speed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), mp3 + "temp4_" + videoScopeNumber + ".mp3", float.Parse(rows[3]), 48000, TimeToFrames(rows[5]), TimeToFrames(rows[6]) - TimeToFrames(rows[5]));
                                    break;
                            }
                            SetVolume(mp3 + "temp4_" + videoScopeNumber + ".mp3", mp3 + "temp3_" + videoScopeNumber + ".mp3", float.Parse(rows[2]));
                            InsertAudioWithAudio(mp3 + "temp2_" + videoScopeNumber + ".mp3", mp3 + "temp3_" + videoScopeNumber + ".mp3", mp3 + "temp1_" + videoScopeNumber + ".mp3", TimeToFrames(rows[4]));
                            File.Copy(mp4 + "temp1_" + videoScopeNumber + ".mp4", mp4 + "temp2_" + videoScopeNumber + ".mp4", true);
                        }
                        break;
                    case "FILTER":
                        if (mode != 1)
                            break;
                        int rgb = Convert.ToInt32(rows[2].Replace("#", ""), 16);

                        filterEffects += InsertVibrance(float.Parse(rows[1]), (rgb >> 16) & 0xff, (rgb >> 8) & 0xff, rgb & 0xff, TimeToFrames(rows[3]), TimeToFrames(rows[4])) + ",";
                        break;
                    case "{":
                        if (rows.Length > 1)
                            break;
                        mode++;
                        break;
                    case "}":
                        if (rows.Length > 1)
                            break;
                        if (mode == 1)
                        {
                            string effects = "";
                            if (drawTextEffects != "")
                            {
                                effects += drawTextEffects.Remove(drawTextEffects.Length - 1) + ",";
                            }

                            if (filterEffects != "")
                            {
                                effects += filterEffects.Remove(filterEffects.Length - 1);
                            }
                            else
                            {
                                effects = effects.Remove(effects.Length - 1);
                            }
                            if (effects != "")
                            {
                                ApplyEffect(mp4 + "temp2_" + videoScopeNumber + ".mp4", mp4 + "temp1_" + videoScopeNumber + ".mp4", effects, videoScopeNumber);
                            }
                            else
                                File.Copy(mp4 + "temp2_" + videoScopeNumber + ".mp4", mp4 + "temp1_" + videoScopeNumber + ".mp4", true);
                            if (!containsAudio)
                            {
                                File.Copy(mp3 + "temp2_" + videoScopeNumber + ".mp3", mp3 + "temp1_" + videoScopeNumber + ".mp3", true);
                                InsertAudioWithoutAudio(mp4 + "temp1_" + videoScopeNumber + ".mp4", mp3 + "temp2_" + videoScopeNumber + ".mp3", final + videoScopeNumber + ".mp4", 0);
                            }

                            tags.Add(final + videoScopeNumber + ".mp4");
                        }
                        if (mode == 2)
                        {
                            drawTextEffects += InsertText(vEDir.Replace('\\', '/').Replace(":", "\\\\:") + "\\".Replace('\\', '/') + defines[text_args[1]].Replace('\\', '/'), float.Parse(text_args[2]), text, float.Parse(text_args[3].Split(',')[0]), float.Parse(text_args[3].Split(',')[1]), text_args[4], TimeToFrames(text_args[5]), TimeToFrames(text_args[6])) + ",";
                            text_args.Clear();
                            text = "";
                        }
                        mode--;
                        break;
                }
            }
        }
        public static void Encode(string projectPath, string encodePath, int audioBitrate, int videoBitrate)
        {
            try
            {
                int mode = 0;
                string vEFile = File.ReadAllText(projectPath, Encoding.UTF8);
                string vEDir = new FileInfo(projectPath).DirectoryName;
                Console.WriteLine(vEDir);
                vEFile = vEFile.Replace("\r\n", "\n");
                float volume = 0;
                List<int[]> videoScopes = ProveVideoScopes(vEFile);
                ProveDefines(vEFile);
                string[] lines = vEFile.Split('\n');
                foreach (string line in lines)
                {
                    string[] rows = line.Replace("\t", "").Split(' ');
                    switch (rows[0].ToUpper())
                    {
                        case "BGM":
                            if (mode != 0)
                                break;
                            if (defines.ContainsKey(rows[1]))
                            {
                                switch (rows.Length)
                                {
                                    case 4:
                                        SetMp3Speed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), bgm + rows[1] + ".mp3", float.Parse(rows[2]), 48000, TimeToFrames(rows[3]));
                                        break;
                                    case 5:
                                        SetMp3Speed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), bgm + rows[1] + ".mp3", float.Parse(rows[2]), 48000, TimeToFrames(rows[3]), TimeToFrames(rows[4]) - TimeToFrames(rows[3]));
                                        break;
                                }
                                bgms.Add(bgm + rows[1] + ".mp3");
                            }
                            break;
                    }
                    mode += CurrentMode(rows[0]);
                }

                ParallelLoopResult plr = Parallel.For(0, videoScopes.Count, (int videoScopeNumber) =>
                {
                    string[] rows = lines[videoScopes[videoScopeNumber][0] - 1].Replace("\t", "").Split(' ');
                    Console.WriteLine(videoScopeNumber);

                    if (defines.ContainsKey(rows[1]))
                    {
                        switch (rows.Length)
                        {
                            case 4:
                                Parallel.Invoke(() => SetMp4Speed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), mp4 + "temp2_" + videoScopeNumber + ".mp4", float.Parse(rows[2]), 48000, TimeToFrames(rows[3])), () => SetMp3Speed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), mp3 + "temp2_" + videoScopeNumber + ".mp3", float.Parse(rows[2]), 48000, TimeToFrames(rows[3])));
                                break;
                            case 5:
                                Parallel.Invoke(() => SetMp4Speed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), mp4 + "temp2_" + videoScopeNumber + ".mp4", float.Parse(rows[2]), 48000, TimeToFrames(rows[3]), TimeToFrames(rows[4]) - TimeToFrames(rows[3])), () => SetMp3Speed(vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines[rows[1]].Replace('\\', '/'), mp3 + "temp2_" + videoScopeNumber + ".mp3", float.Parse(rows[2]), 48000, TimeToFrames(rows[3]), TimeToFrames(rows[4]) - TimeToFrames(rows[3])));
                                break;
                        }
                    }
                    string text = string.Join('\n', lines[videoScopes[videoScopeNumber][0]..(videoScopes[videoScopeNumber][1] + 1)]);
                    Console.WriteLine(string.Join(' ', rows));
                    Console.WriteLine(text);
                    EncodeParts(videoScopeNumber, text, vEDir, rows[1], audioBitrate, videoBitrate);

                });
                string commandArgs = @"-y -f concat -safe 0 -i " + tmp + "list.txt";
                string concat = "";
                while (!plr.IsCompleted) ;
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                Process process = new Process();
                if (videoScopes.Count < 2)
                {
                    File.Copy(final + "0.mp4", tmp + "last2.mp4", true);
                }
                else
                {
                    for (int index = 0; index < videoScopes.Count; index++)
                    {
                        concat += "file " + final + index + ".mp4\n";
                    }
                    concat = concat.Remove(concat.Length - 1);
                    File.WriteAllText(tmp + "list.txt", concat);
                    commandArgs += " -b:a " + audioBitrate + "k" + @"  " + tmp + @"last2.mp4".Replace('\\', '/');
                    processStartInfo.FileName = ffmpeg;
                    processStartInfo.UseShellExecute = false;
                    processStartInfo.Arguments = commandArgs;
                    process = Process.Start(processStartInfo);
                    process.WaitForExit();
                }
                if (bgms.Count != 0)
                {
                    if (bgms.Count < 2)
                    {
                        File.Copy(bgms[0], tmp + "last3.mp3", true);
                    }
                    else
                    {
                        commandArgs = @"-y -f concat -safe 0 -i " + tmp + "list.txt";
                        concat = "";
                        foreach (string bgm in bgms)
                        {
                            concat += "file " + bgm + "\n";
                        }
                        File.WriteAllText(tmp + "list.txt", concat);
                        commandArgs += " -b:a " + audioBitrate + "k" + @" " + tmp + @"last3.mp3".Replace('\\', '/');
                        processStartInfo = new ProcessStartInfo();
                        processStartInfo.FileName = ffmpeg;
                        processStartInfo.UseShellExecute = false;
                        processStartInfo.Arguments = commandArgs;
                        process = Process.Start(processStartInfo);
                        process.WaitForExit();
                    }
                }
                if (defines.ContainsKey("BGM_VOLUME"))
                    volume = float.Parse(defines["BGM_VOLUME"]);
                else
                    volume = 0.4f;
                if (File.Exists(tmp + "last3.mp3"))
                {
                    Parallel.Invoke(() => SetMp4Speed(tmp + "last2.mp4", tmp + "last1.mp4", 1, 48000, 0), () => SetMp3Speed(tmp + "last2.mp4", tmp + "last2.mp3", 1, 48000, 0));

                    if (defines.ContainsKey("ENCODE_PATH"))
                    {

                        SetVolume(tmp + "last3.mp3", tmp + "last1.mp3", volume);
                        InsertAudioWithAudio(tmp + "last2.mp3", tmp + "last1.mp3", tmp + "last.mp3", 0);

                        InsertAudioWithoutAudio(tmp + "last1.mp4", tmp + "last.mp3", vEDir + "\\".Replace('\\', '/') + defines["ENCODE_PATH"], 0);

                    }
                    else
                    {
                        SetVolume(tmp + "last3.mp3", tmp + "last1.mp3", volume);
                        InsertAudioWithAudio(tmp + "last2.mp3", tmp + "last1.mp3", tmp + "last.mp3", 0);

                        InsertAudioWithoutAudio(tmp + "last1.mp4", tmp + "last.mp3", vEDir + "\\".Replace('\\', '/') + encodePath, 0);
                    }
                }
                else
                {
                    if (defines.ContainsKey("ENCODE_PATH"))
                        File.Copy(tmp + "last2.mp4".Replace('\\', '/'), vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + defines["ENCODE_PATH"].Replace('\\', '/'), true);
                    else if (encodePath != "")
                        File.Copy(tmp + "last2.mp4".Replace('\\', '/'), vEDir.Replace('\\', '/') + "\\".Replace('\\', '/') + encodePath.Replace('\\', '/'), true);
                }
                Console.WriteLine("Success");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
