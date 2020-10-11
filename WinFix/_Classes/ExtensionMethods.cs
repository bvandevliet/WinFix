using System;
using System.IO;
using System.Text.RegularExpressions;

namespace ExtensionsRegex
{
    public static class BeforeAndIncluding
    {
        private static string Output(string Input, string Pattern, RegexOptions options)
        {
            return Regex.Match(Input, $@"^.*?{Pattern}", options).Value;
        }

        /*
         * Returns the string before the first occurence of a regular expression pattern match including that match.
         */
        public static string StringBeforeAndIncludingFirstOccurenceOf(this string Input, string Pattern)
        {
            return StringBeforeAndIncludingFirstOccurenceOf(Input, Pattern, RegexOptions.None);
        }
        public static string StringBeforeAndIncludingFirstOccurenceOf(this string Input, string Pattern, RegexOptions options)
        {
            return Output(Input, Pattern, options);
        }

        /*
         * Returns the string before the last occurence of a regular expression pattern match including that match.
         */
        public static string StringBeforeAndIncludingLastOccurenceOf(this string Input, string Pattern)
        {
            return StringBeforeAndIncludingLastOccurenceOf(Input, Pattern, RegexOptions.None);
        }
        public static string StringBeforeAndIncludingLastOccurenceOf(this string Input, string Pattern, RegexOptions options)
        {
            return Output(Input, Pattern, options | RegexOptions.RightToLeft);
        }
    }

    public static class BeforeAndExcluding
    {
        private static string Output(string Input, string Pattern, RegexOptions options)
        {
            return Regex.Match(Input, $@"^.*?(?={Pattern})", options).Value;
        }

        /*
         * Returns the string before the first occurence of a regular expression pattern match.
         */
        public static string StringBeforeFirstOccurenceOf(this string Input, string Pattern)
        {
            return StringBeforeFirstOccurenceOf(Input, Pattern, RegexOptions.None);
        }
        public static string StringBeforeFirstOccurenceOf(this string Input, string Pattern, RegexOptions options)
        {
            return Output(Input, Pattern, options);
        }

        /*
         * Returns the string before the last occurence of a regular expression pattern match.
         */
        public static string StringBeforeLastOccurenceOf(this string Input, string Pattern)
        {
            return StringBeforeLastOccurenceOf(Input, Pattern, RegexOptions.None);
        }
        public static string StringBeforeLastOccurenceOf(this string Input, string Pattern, RegexOptions options)
        {
            return Output(Input, Pattern, options | RegexOptions.RightToLeft);
        }
    }

    public static class AfterAndIncluding
    {
        private static string Output(string Input, string Pattern, RegexOptions options)
        {
            return Regex.Match(Input, $@"{Pattern}.*?$", options).Value;
        }

        /*
         * Returns the string after the first occurence of a regular expression pattern match including that match.
         */
        public static string StringAfterAndIncludingFirstOccurenceOf(this string Input, string Pattern)
        {
            return StringAfterAndIncludingFirstOccurenceOf(Input, Pattern, RegexOptions.None);
        }
        public static string StringAfterAndIncludingFirstOccurenceOf(this string Input, string Pattern, RegexOptions options)
        {
            return Output(Input, Pattern, options);
        }

        /*
         * Returns the string after the last occurence of a regular expression pattern match including that match.
         */
        public static string StringAfterAndIncludingLastOccurenceOf(this string Input, string Pattern)
        {
            return StringAfterAndIncludingLastOccurenceOf(Input, Pattern, RegexOptions.None);
        }
        public static string StringAfterAndIncludingLastOccurenceOf(this string Input, string Pattern, RegexOptions options)
        {
            return Output(Input, Pattern, options | RegexOptions.RightToLeft);
        }
    }

    public static class AfterAndExcluding
    {
        private static string Output(string Input, string Pattern, RegexOptions options)
        {
            return Regex.Match(Input, $@"(?<={Pattern}).*?$", options).Value;
        }

        /*
         * Returns the string before the last occurence of a regular expression pattern match.
         */
        public static string StringAfterFirstOccurenceOf(this string Input, string Pattern)
        {
            return StringAfterFirstOccurenceOf(Input, Pattern, RegexOptions.None);
        }
        public static string StringAfterFirstOccurenceOf(this string Input, string Pattern, RegexOptions options)
        {
            return Output(Input, Pattern, options);
        }

        /*
         * Returns the string after the last occurence of a regular expression pattern match.
         */
        public static string StringAfterLastOccurenceOf(this string Input, string Pattern)
        {
            return StringAfterLastOccurenceOf(Input, Pattern, RegexOptions.None);
        }
        public static string StringAfterLastOccurenceOf(this string Input, string Pattern, RegexOptions options)
        {
            return Output(Input, Pattern, options | RegexOptions.RightToLeft);
        }
    }

    public static class ReplaceFirstOrLast
    {
        /*
         * Replaces the first occurence of a regular expression pattern match and returns the string.
         */
        public static string ReplaceFirstOccurenceOf(this string Input, string Pattern, string Replacement)
        {
            return ReplaceFirstOccurenceOf(Input, Pattern, Replacement, RegexOptions.None);
        }
        public static string ReplaceFirstOccurenceOf(this string Input, string Pattern, string Replacement, RegexOptions options)
        {
            return new Regex($@"{Pattern}", options).Replace(Input, Replacement, 1);
        }

        /*
         * Replaces the last occurence of a regular expression pattern match and returns the string.
         */
        public static string ReplaceLastOccurenceOf(this string Input, string Pattern, string Replacement)
        {
            return ReplaceLastOccurenceOf(Input, Pattern, Replacement, RegexOptions.None);
        }
        public static string ReplaceLastOccurenceOf(this string Input, string Pattern, string Replacement, RegexOptions options)
        {
            return new Regex($@"{Pattern}", options | RegexOptions.RightToLeft).Replace(Input, Replacement, 1);
        }
    }
}

namespace ExtensionsIO
{
    public static class Dir
    {
        public static bool CopyAll(string Source, string Destination, string PatternToExclude = null)
        {
            return CopyAll(new DirectoryInfo(Source), new DirectoryInfo(Destination), PatternToExclude);
        }
        public static bool CopyAll(string Source, DirectoryInfo Destination, string PatternToExclude = null)
        {
            return CopyAll(new DirectoryInfo(Source), Destination, PatternToExclude);
        }
        public static bool CopyAll(this DirectoryInfo Source, string Destination, string PatternToExclude = null)
        {
            return CopyAll(Source, new DirectoryInfo(Destination), PatternToExclude);
        }
        public static bool CopyAll(this DirectoryInfo Source, DirectoryInfo Destination, string PatternToExclude = null)
        {
            bool success = true;

            if (Source.FullName.ToLower() == Destination.FullName.ToLower())
            {
                return success;
            }

            if (!Destination.Exists)
            {
                Destination.Create();
            }

            foreach (FileInfo file in Source.GetFiles())
            {
                if (PatternToExclude == null || !Regex.IsMatch(file.Name, PatternToExclude, RegexOptions.IgnoreCase))
                {
                    try
                    {
                        file.CopyTo(Path.Combine(Destination.ToString(), file.Name), true);
                        //Console.WriteLine($"Copied \"{file.Name}\"");
                    }
                    catch (Exception Ex)
                    {
                        success = false;

                        Console.WriteLine(Ex);
                    }
                }
            }

            foreach (DirectoryInfo nextSource in Source.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                if (PatternToExclude == null || !Regex.IsMatch(nextSource.FullName, PatternToExclude, RegexOptions.IgnoreCase))
                {
                    DirectoryInfo nextDestination = Destination.CreateSubdirectory(nextSource.Name);

                    success = success && CopyAll(nextSource, nextDestination, PatternToExclude);
                }
            }

            return success;
        }

        public static bool MoveAll(string Source, string Destination, string PatternToExclude = null)
        {
            return MoveAll(new DirectoryInfo(Source), new DirectoryInfo(Destination), PatternToExclude);
        }
        public static bool MoveAll(string Source, DirectoryInfo Destination, string PatternToExclude = null)
        {
            return MoveAll(new DirectoryInfo(Source), Destination, PatternToExclude);
        }
        public static bool MoveAll(this DirectoryInfo Source, string Destination, string PatternToExclude = null)
        {
            return MoveAll(Source, new DirectoryInfo(Destination), PatternToExclude);
        }
        public static bool MoveAll(this DirectoryInfo Source, DirectoryInfo Destination, string PatternToExclude = null)
        {
            bool success = true;

            if (Source.FullName.ToLower() == Destination.FullName.ToLower())
            {
                return success;
            }

            if (!Destination.Exists)
            {
                Destination.Create();
            }

            foreach (FileInfo file in Source.GetFiles())
            {
                if (PatternToExclude == null || !Regex.IsMatch(file.Name, PatternToExclude, RegexOptions.IgnoreCase))
                {
                    try
                    {
                        file.MoveTo(Path.Combine(Destination.ToString(), file.Name));
                        //Console.WriteLine($"Moved \"{file.Name}\"");
                    }
                    catch (Exception Ex)
                    {
                        success = false;

                        Console.WriteLine(Ex);
                    }
                }
            }

            foreach (DirectoryInfo nextSource in Source.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                if (PatternToExclude == null || !Regex.IsMatch(nextSource.FullName, PatternToExclude, RegexOptions.IgnoreCase))
                {
                    DirectoryInfo nextDestination = Destination.CreateSubdirectory(nextSource.Name);

                    success = success && MoveAll(nextSource, nextDestination, PatternToExclude);
                }
            }

            return success;
        }

        public static void CleanUpFolder(string FullPath, string PatternToInclude, bool Recurse = false)
        {
            CleanUpFolder(new DirectoryInfo(FullPath), PatternToInclude, Recurse);
        }
        public static void CleanUpFolder(this DirectoryInfo Folder, string PatternToInclude, bool Recurse = false)
        {
            if (!Folder.Exists)
            {
                return;
            }

            foreach (FileInfo file in Folder.GetFiles())
            {
                if (Regex.IsMatch(file.Name, PatternToInclude))
                {
                    try
                    {
                        file.Delete();
                        //Console.WriteLine($"Deleted \"{file.Name}\"");
                    }
                    catch (Exception Ex)
                    {
                        Console.WriteLine(Ex);
                    }
                }
            }

            if (Recurse)
            {
                foreach (DirectoryInfo subfolder in Folder.GetDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    CleanUpFolder(subfolder, PatternToInclude, true);
                }
            }
        }

        public static bool DeleteDir(string FullPath)
        {
            return DeleteDir(new DirectoryInfo(FullPath));
        }
        public static bool DeleteDir(this DirectoryInfo dirInfo)
        {
            bool success = true;

            if (!dirInfo.Exists)
            {
                return success;
            }

            foreach (FileInfo fileInfo in dirInfo.GetFiles())
            {
                try
                {
                    fileInfo.Delete();
                }
                catch (Exception)
                {
                    success = false;
                }
            }

            foreach (DirectoryInfo subdirInfo in dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly))
            {
                success = success && DeleteDir(subdirInfo);
            }

            try
            {
                dirInfo.Delete();
                //Console.WriteLine($"Deleted \"{Folder.FullName}\"");
            }
            catch (Exception Ex)
            {
                success = false;

                Console.WriteLine(Ex);
            }

            return success;
        }

        public enum RenameMode
        {
            File,
            Folder,
            Both,
        }

        public static void RenameItems(string FullPath, string PatternToReplace, string ReplaceWith, RenameMode Mode = RenameMode.File, bool Recurse = false)
        {
            RenameItems(new DirectoryInfo(FullPath), PatternToReplace, ReplaceWith, Mode, Recurse);
        }
        public static void RenameItems(this DirectoryInfo dirInfo, string PatternToReplace, string ReplaceWith, RenameMode Mode = RenameMode.File, bool Recurse = false)
        {
            if (!dirInfo.Exists)
            {
                return;
            }

            if (Mode == RenameMode.File || Mode == RenameMode.Both)
            {
                foreach (FileInfo fileInfo in dirInfo.GetFiles())
                {
                    if (Regex.IsMatch(fileInfo.Name, PatternToReplace))
                    {
                        /**
                         * Try-catch to avoid an abort, e.g. in case the replacement already matches the output.
                         */
                        try
                        {
                            string oldFileName = fileInfo.FullName.TrimEnd('\\');
                            string newFileName = Regex.Replace(fileInfo.Name, PatternToReplace, ReplaceWith);
                            fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, newFileName));
                            //Console.WriteLine($"\n Renamed\n \"{oldFileName}\"\n \"{Path.Combine(file.DirectoryName, newFileName)}\"");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }

            if (Recurse || Mode == RenameMode.Folder || Mode == RenameMode.Both)
            {
                foreach (DirectoryInfo subdirInfo in dirInfo.GetDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    if (Recurse)
                    {
                        RenameItems(subdirInfo, PatternToReplace, ReplaceWith, Mode, true);
                    }

                    if (
                        (Mode == RenameMode.Folder || Mode == RenameMode.Both) &&
                        Regex.IsMatch(subdirInfo.Name, PatternToReplace)
                    )
                    {
                        /**
                         * Try-catch to avoid an abort, e.g. in case the replacement already matches the output.
                         */
                        try
                        {
                            string oldFolderName = subdirInfo.FullName.TrimEnd('\\');
                            string newFolderName = Regex.Replace(subdirInfo.Name, PatternToReplace, ReplaceWith);
                            subdirInfo.MoveTo(Path.Combine(dirInfo.FullName, newFolderName));
                            //Console.WriteLine($"\n Renamed\n \"{oldFolderName}\"\n \"{Path.Combine(Folder.FullName, newFolderName)}\"");
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }
}