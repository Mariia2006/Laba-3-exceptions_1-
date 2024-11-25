using System.IO;

namespace ConsoleApp118
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Warning! The files must be named as specified in the documentation (10.txt, 11.txt, ..., 29.txt).");
            for (int i = 1; i <= 20; i++)
            {
                Console.WriteLine($"Enter a file name: ");
                string fileName = $"{Console.ReadLine()}.txt";
                Console.WriteLine($"Enter two numbers for the file {fileName}:");

                string firstLine = Console.ReadLine();
                string secondLine = Console.ReadLine();

                File.WriteAllLines(fileName, new string[] { firstLine, secondLine });
            }
            var noFileList = new List<string>();
            var badDataList = new List<string>();
            var overflowList = new List<string>();

            long sumOfProducts = 0;
            int validProductCount = 0;
            for (int i = 10; i <= 29; i++)
            {
                string fileName = $"{i}.txt";

                try
                {
                    string[] lines;
                    using (var reader = File.OpenText(fileName))
                    {
                        lines = new string[2];
                        lines[0] = reader.ReadLine();
                        lines[1] = reader.ReadLine();
                    }

                    int firstNumber = int.Parse(lines[0]);
                    int secondNumber = int.Parse(lines[1]);

                    try
                    {
                        checked
                        {
                            int product = firstNumber * secondNumber;
                            sumOfProducts += product;
                            validProductCount++;
                        }
                    }
                    catch (OverflowException)
                    {
                        overflowList.Add(fileName);
                    }
                }
                catch (FileNotFoundException)
                {
                    noFileList.Add(fileName);
                }
                catch (FormatException)
                {
                    badDataList.Add(fileName);
                }
                catch (IndexOutOfRangeException)
                {
                    badDataList.Add(fileName);
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine($"Unable to access or write to the file: {fileName}. This file might have restricted access or be locked.");
                }
                catch (IOException ex)
                { 
                    Console.WriteLine($"Error accessing file {fileName}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error accessing file {fileName}: {ex.Message}");
                }
            }
            File.WriteAllLines("no_file.txt", noFileList);
            File.WriteAllLines("bad_data.txt", badDataList);
            File.WriteAllLines("overflow.txt", overflowList);

            if (validProductCount > 0)
            {
                double average = (double)sumOfProducts / validProductCount;
                Console.WriteLine($"\nArithmetic average of products: {average:F2}");
            }
            else
            {
                Console.WriteLine("\nThere are no valid products to calculate the arithmetic mean.");
            }
        }
    }
}