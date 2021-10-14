using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace Lab_1
{
    class Student
    {
        public string Name { get; set; }
        public string LName { get; set; }
        public int Group { get; set; }

        public Student(string lastname, string name, int group)
        {
            Name = name;
            LName = lastname;
            Group = group;
        }

        public override string ToString()
        {
            return
                String.Format($"Student: {LName} {Name} - from the group - {Group}, Mark - ");
        }

        public Student()
        {

        }
    }

    class DictionaryStudent
    {
        Dictionary<Student, int> list = new Dictionary<Student, int>();


        public void AddStudent(Student st, int mark)
        {
            list.Add(new Student(st.LName, st.Name, st.Group), mark);
        }

        public void AddStudents(int n)
        {
            try
            {
                Student student = new Student();

                for (int i = 0; i < n; i++)
                {
                    Console.Write("Input LastName: ");
                    student.LName = Console.ReadLine();
                    Console.Write("Input Name: ");
                    student.Name = Console.ReadLine();
                    Console.Write("Input Group: ");
                    student.Group = int.Parse(Console.ReadLine());
                    Console.Write("Input mark: ");
                    int mark = int.Parse(Console.ReadLine());

                    AddStudent(new Student(student.LName, student.Name, student.Group), mark);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        public void ListStudents()
        {
            string creationPath = @"C:\Users\Павел\Desktop\Labs\3 сем\C#\Lab_1";
            string targetFileName = "lab_1.txt";
            string targetFilePath = Path.Combine(creationPath, targetFileName);

            StreamWriter sw;
            FileInfo fi = new FileInfo(targetFilePath);
            sw = fi.AppendText();

            foreach (KeyValuePair<Student, int> obj in list)
            {
                Console.WriteLine($"Student {obj.Key.LName} {obj.Key.Name} from the group {obj.Key.Group}, get mark {obj.Value}");
                sw.WriteLine(obj.Key.LName);
                sw.WriteLine(obj.Key.Name);
                sw.WriteLine(obj.Key.Group);
                sw.WriteLine(obj.Value);
            }
            sw.Close();

        }
    }

    class Program
    {

        static void Main()
        {


            DictionaryStudent ds = new DictionaryStudent();

            do
            {
                Console.Clear();
                Console.WriteLine("Menu");
                Console.WriteLine("1.Add student\n2.Read text file\n3.Rename file\n4.Copy file in new directory\n5.Moving a file\n6.Deleted file\n" +
                    "7.Get info this file\n8.Archived folder\n9.Clear file\n10.Close");
                string k = Console.ReadLine();

                Console.Clear();
                switch (k)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("Input number of students: ");
                        int count = int.Parse(Console.ReadLine());
                        ds.AddStudents(count);
                        ds.ListStudents();
                        break;
                    case "2":
                        Console.Clear();
                        ReadFromTextFile();
                        break;
                    case "3":
                        Console.Clear();
                        RenameFile();
                        break;
                    case "4":
                        Console.Clear();
                        CopyFile();
                        break;
                    case "5":
                        Console.Clear();
                        MoveFile();
                        break;
                    case "6":
                        Console.Clear();
                        DeletedFile();
                        break;
                    case "7":
                        Console.Clear();
                        InfoFile();
                        break;
                    case "8":
                        Console.Clear();
                        CompressFolder();
                        break;
                    case "9":
                        Console.Clear();
                        ClearFile();
                        break;
                    case "10":
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Re-enter");
                        break;
                }

            } while (true);

        }

        static void InfoFile()
        {
            try
            {
                Console.WriteLine("Input directory: ");
                string creationNewPath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input name of file: ");
                string targetNewFileName = Console.ReadLine();
                string targetOldFilePath = Path.Combine(creationNewPath, targetNewFileName);
                FileInfo fileInf = new FileInfo(targetOldFilePath);
                if (fileInf.Exists)
                {
                    Console.WriteLine($"\nИмя файла: {fileInf.Name}");
                    Console.WriteLine($"Время создания: {fileInf.CreationTime}");
                    Console.WriteLine($"Размер: {fileInf.Length} kb\n");
                }
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void DeletedFile()
        {
            try
            {
                Console.WriteLine("Input directory: ");
                string creationNewPath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input name of file: ");
                string targetNewFileName = Console.ReadLine();
                string targetOldFilePath = Path.Combine(creationNewPath, targetNewFileName);
                FileInfo fileinf = new FileInfo(targetOldFilePath);
                if (fileinf.Exists)
                {
                    fileinf.Delete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void MoveFile()
        {
            try
            {
                Console.WriteLine("Input directory: ");
                string creationOldPath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input name for file: ");
                string targetOldFileName = Console.ReadLine();
                string targetOldFilePath = Path.Combine(creationOldPath, targetOldFileName);

                Console.WriteLine("Input new directory: ");
                string creationNewPath = $@"{Console.ReadLine()}";
                string targetNewFilePath = Path.Combine(creationNewPath, targetOldFileName);
                FileInfo fileinf = new FileInfo(targetOldFilePath);
                if (fileinf.Exists)
                {
                    fileinf.MoveTo(targetNewFilePath, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void CopyFile()
        {
            try
            {
                Console.WriteLine("Input directory: ");
                string creationOldPath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input name for file: ");
                string targetOldFileName = Console.ReadLine();
                string targetOldFilePath = Path.Combine(creationOldPath, targetOldFileName);

                Console.WriteLine("Input new directory: ");
                string creationNewPath = $@"{Console.ReadLine()}";
                string targetNewFilePath = Path.Combine(creationNewPath, targetOldFileName);

                FileInfo fileinf = new FileInfo(targetOldFilePath);
                if (fileinf.Exists)
                {
                    fileinf.CopyTo(targetNewFilePath, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void RenameFile()
        {
            try
            {

                Console.WriteLine("Input directory: ");
                string creationOldPath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input name for file: ");
                string targetOldFileName = Console.ReadLine();
                string targetOldFilePath = Path.Combine(creationOldPath, targetOldFileName);

                Console.Write("Input new file name: ");
                string NewFileName = Console.ReadLine();

                string targetNewFilePath = Path.Combine(creationOldPath, NewFileName);

                if (File.Exists(targetNewFilePath))
                {
                    File.Delete(targetNewFilePath);
                }
                File.Move(targetOldFilePath, targetNewFilePath);
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void ReadFromTextFile()
        {
            try
            {
                Console.WriteLine("Input directory: ");
                string creationNewPath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input name for file: ");
                string targetNewFileName = Console.ReadLine();
                string targetNewFilePath = Path.Combine(creationNewPath, targetNewFileName);

                List<string> list = new List<string>();
                Stream fs = new FileStream(targetNewFilePath, FileMode.Open, FileAccess.Read);
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                {
                    string temp = string.Empty;
                    while (sr.Peek() != -1)
                    {
                        temp = sr.ReadLine();
                        if (temp == "flag")
                            continue;
                        else
                            list.Add(temp);
                    }
                }
                fs.Close();
                foreach (string s in list)
                    Console.WriteLine(s);

                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void CompressFolder()
        {
            try
            {
                Console.WriteLine("Input folder directory: ");
                string sourceFilePath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input folder for paking: ");
                string sourceFileName = Console.ReadLine();
                string sourceFolder = Path.Combine(sourceFilePath, sourceFileName);

                string zipFile = $"{sourceFolder}.zip";

                Console.WriteLine("Input directory for unpaking file: ");
                string targetPath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input unpaking folder: ");
                string targetFolder = Console.ReadLine();
                string targetFilePath = Path.Combine(targetPath, targetFolder);

                ZipFile.CreateFromDirectory(sourceFolder, zipFile);
                Console.WriteLine($"Folder {sourceFolder} archived in file {zipFile}.\n");

                ZipFile.ExtractToDirectory(zipFile, targetFilePath);
                Console.WriteLine($"File {zipFile} unpaking in folder {targetFilePath}\n");

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }

        static void ClearFile()
        {
            try
            {
                Console.WriteLine("Input directory: ");
                string creationNewPath = $@"{Console.ReadLine()}";
                Console.WriteLine("Input name of file: ");
                string targetNewFileName = Console.ReadLine();
                string targetOldFilePath = Path.Combine(creationNewPath, targetNewFileName);

                Console.WriteLine("Deleted data:\n");
                StreamReader sr = new StreamReader(targetOldFilePath, true);
                Console.Write(sr.ReadToEnd());
                sr.Close();

                Console.WriteLine("\nSuccessful cleaning.");
                StreamWriter sw = new StreamWriter(targetOldFilePath, false);
                sw.WriteLine("");
                sw.Close();

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}