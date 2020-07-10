using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Win32;
using SeminarCTDLGT.Classes;
using SeminarCTDLGT.Windows;

namespace SeminarCTDLGT
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string OutputPath = "Output";
        private int[] _ints = new int[0];
        private int[] _intsOrdered = new int[0];

        public MainWindow()
        {
            InitializeComponent();

            MainWindowState = new MainWindowState
            {
                IsGeneratingArray = false,
                IsSavingFile = false,
                IsLoadingFile = false,
                IsLinearSearching = false,
                IsBinarySearching = false,
                IsInterchangeSorting = false,
                IsBubbleSorting = false,
                IsInterpolationSearching = false,
                IsSelectionSorting = false,
                IsInsertionSorting = false,
                IsHeapSorting = false,
                IsQuickSorting = false,
                IsMergeSorting = false
            };
            DataContext = MainWindowState;

            CreateOutputFolder();
        }

        private void CreateOutputFolder()
        {
            if (Directory.Exists(OutputPath))
                Directory.Delete(OutputPath, true);

            Directory.CreateDirectory(OutputPath);
        }

        public MainWindowState MainWindowState { get; set; }

        private void GenerateArray(int numOfInts)
        {
            var rand = new Random();
            var enumerable = Enumerable.Range(0, numOfInts);
            var ints = enumerable as int[] ?? enumerable.ToArray();
            _ints = ints.Select(i => new Tuple<int, int>(rand.Next(numOfInts), i))
                .OrderBy(i => i.Item1)
                .Select(i => i.Item2).ToArray();
            _intsOrdered = ints;
        }

        private void ButtonGenerateArray_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            var result = int.TryParse(TextBoxNumOfInts.Text, out var numOfInts);
            if (!result)
            {
                var popup = new PopupMessage("Invalid number", "Please enter a valid number!");
                popup.ShowDialog();
            }
            else if (numOfInts <= 0)
            {
                var popup = new PopupMessage("Invalid number", "Number must be greater than 0.");
                popup.ShowDialog();
            }
            else if (numOfInts > 10000000)
            {
                var popup = new PopupMessage("Invalid number", "Please try a smaller number!");
                popup.ShowDialog();
            }
            else
            {
                void Action()
                {
                    MainWindowState.IsGeneratingArray = true;
                    GenerateArray(numOfInts);
                    MainWindowState.IsGeneratingArray = false;
                }

                var task = new Task(Action);
                task.Start();
            }
        }

        private void ButtonSaveToFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress", "Please wait after task is complete!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsSavingFile)
            {
                var popup = new PopupMessage("Task is in progress", "Please wait after task is complete!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Text|*.txt",
                Title = "Save data to file"
            };
            var result = saveFileDialog.ShowDialog();
            if (result != true) return;
            var path = saveFileDialog.FileName;
            if (string.IsNullOrEmpty(path)) return;
            var content = string.Join(" ", _ints);

            void Action()
            {
                MainWindowState.IsSavingFile = true;
                File.WriteAllText(path, content);
                MainWindowState.IsSavingFile = false;
            }
            var task = new Task(Action);
            task.Start();
        }

        private void ButtonLoadDataFromFile_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress", "Please wait after task is complete!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsLoadingFile)
            {
                var popup = new PopupMessage("Task is in progress", "Please wait after task is complete!");
                popup.ShowDialog();
                return;
            }

            var openFileDialog = new OpenFileDialog()
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Text|*.txt",
            };
            var result = openFileDialog.ShowDialog();
            if (result != true) return;
            var path = openFileDialog.FileName;
            if (!File.Exists(path))
            {
                var popup = new PopupMessage("File does not exist", "Please make sure your entered path is true!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsLoadingFile = true;
                var content = File.ReadAllText(path);
                var enumerable = content.Split(' ')
                    .Select(i =>
                    {
                        int.TryParse(i, out var num);
                        return num;
                    });
                var ints = enumerable as int[] ?? enumerable.ToArray();
                _ints = ints.ToArray();
                _intsOrdered = ints.OrderBy(i => i).ToArray();
                Dispatcher.Invoke(() =>
                {
                    TextBoxNumOfInts.Text = _ints.Length.ToString();
                });
                MainWindowState.IsLoadingFile = false;
            }
            var task = new Task(Action);
            task.Start();
        }

        private void ButtonRandomX_OnClick(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            var rand = new Random();
            TextBoxX.Text = rand.Next().ToString();
        }

        private void ButtonLinearSearch_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsLinearSearching)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Linear search is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            var result = int.TryParse(TextBoxX.Text, out var x);
            if (!result)
            {
                var popup = new PopupMessage("Invalid number", "Please enter a valid number!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsLinearSearching = true;

                var a = new int[_ints.Length];
                _ints.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                var res = Algorithms.LinearSearch(a, x);
                watch.Stop();

                MainWindowState.IsLinearSearching = false;


                Dispatcher.Invoke(() =>
                {
                    var message = res == -1 ? $"{x} not found in array!" : $"{x} was found at index {res}";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Linear Search Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonBinarySearch_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsBinarySearching)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Binary search is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            var result = int.TryParse(TextBoxX.Text, out var x);
            if (!result)
            {
                var popup = new PopupMessage("Invalid number", "Please enter a valid number!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsBinarySearching = true;

                var a = new int[_ints.Length];
                _intsOrdered.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                var res = Algorithms.BinarySearch(a, x);
                watch.Stop();

                MainWindowState.IsBinarySearching = false;

                Dispatcher.Invoke(() =>
                {
                    var message = res == -1 ? $"{x} not found in array!" : $"{x} was found at index {res}";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Binary Search Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonInterpolationSearch_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 1)
            {
                var popup = new PopupMessage("The array must be at least 2 elements", "Please regenerate array with more than 2 elements!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsInterpolationSearching)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Interpolation search is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            var result = int.TryParse(TextBoxX.Text, out var x);
            if (!result)
            {
                var popup = new PopupMessage("Invalid number", "Please enter a valid number!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsInterpolationSearching = true;

                var a = new int[_ints.Length];
                _intsOrdered.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                var res = Algorithms.InterpolationSearch(a, x);
                watch.Stop();

                MainWindowState.IsInterpolationSearching = false;

                Dispatcher.Invoke(() =>
                {
                    var message = res == -1 ? $"{x} not found in array!" : $"{x} was found at index {res}";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Interpolation Search Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonInterchangeSort_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length > 50000)
            {
                var popup = new PopupMessage("The array is too large",
                    "With this sorting algorithm, it could take an unexpected time to sort! Please choose another better sorting algorithm!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsInterchangeSorting)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Interchange Sort is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsInterchangeSorting = true;

                var a = new int[_ints.Length];
                _ints.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                Algorithms.InterchangeSort(a);
                watch.Stop();

                var content = string.Join(" ", a);
                File.WriteAllText($"{OutputPath}/InterchangeSort.txt", content);

                MainWindowState.IsInterchangeSorting = false;

                Dispatcher.Invoke(() =>
                {
                    var message = "Array was sorted!";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Interchange Sort Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonBubbleSort_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length > 50000)
            {
                var popup = new PopupMessage("The array is too large",
                    "With this sorting algorithm, it could take an unexpected time to sort! Please choose another better sorting algorithm!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsBubbleSorting)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Bubble Sort is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsBubbleSorting = true;

                var a = new int[_ints.Length];
                _ints.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                Algorithms.BubbleSort(a);
                watch.Stop();

                var content = string.Join(" ", a);
                File.WriteAllText($"{OutputPath}/BubbleSort.txt", content);

                MainWindowState.IsBubbleSorting = false;

                Dispatcher.Invoke(() =>
                {
                    var message = "Array was sorted!";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Bubble Sort Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonSelectionSort_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length > 100000)
            {
                var popup = new PopupMessage("The array is too large",
                    "With this sorting algorithm, it could take an unexpected time to sort! Please choose another better sorting algorithm!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsSelectionSorting)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Selection Sort is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsSelectionSorting = true;

                var a = new int[_ints.Length];
                _ints.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                Algorithms.SelectionSort(a);
                watch.Stop();

                var content = string.Join(" ", a);
                File.WriteAllText($"{OutputPath}/SelectionSort.txt", content);

                MainWindowState.IsSelectionSorting = false;

                Dispatcher.Invoke(() =>
                {
                    var message = "Array was sorted!";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Selection Sort Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonInsertionSort_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length > 100000)
            {
                var popup = new PopupMessage("The array is too large",
                    "With this sorting algorithm, it could take an unexpected time to sort! Please choose another better sorting algorithm!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsInsertionSorting)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Insertion sort is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsInsertionSorting = true;

                var a = new int[_ints.Length];
                _ints.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                Algorithms.InsertionSort(a);
                watch.Stop();

                var content = string.Join(" ", a);
                File.WriteAllText($"{OutputPath}/InsertionSort.txt", content);

                MainWindowState.IsInsertionSorting = false;

                Dispatcher.Invoke(() =>
                {
                    var message = "Array was sorted!";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Insertion Sort Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonHeapSort_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsHeapSorting)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Heap sort is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsHeapSorting = true;

                var a = new int[_ints.Length];
                _ints.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                Algorithms.HeapSort(a, a.Length);
                watch.Stop();

                var content = string.Join(" ", a);
                File.WriteAllText($"{OutputPath}/HeapSort.txt", content);

                MainWindowState.IsHeapSorting = false;

                Dispatcher.Invoke(() =>
                {
                    var message = "Array was sorted!";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Heap Sort Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonQuickSort_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsQuickSorting)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Quick sort is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsQuickSorting = true;

                var a = new int[_ints.Length];
                _ints.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                Algorithms.QuickSort(a, 0, a.Length - 1);
                watch.Stop();

                var content = string.Join(" ", a);
                File.WriteAllText($"{OutputPath}/QuickSort.txt", content);

                MainWindowState.IsQuickSorting = false;

                Dispatcher.Invoke(() =>
                {
                    var message = "Array was sorted!";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Quick Sort Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonMergeSort_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindowState.IsGeneratingArray)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Program is generating array. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            if (_ints.Length == 0)
            {
                var popup = new PopupMessage("The array is empty", "Please generate array before doing this!");
                popup.ShowDialog();
                return;
            }

            if (MainWindowState.IsLinearSearching)
            {
                var popup = new PopupMessage("Task is in progress",
                    "Merge sort is in progress. Please wait after progress finished!");
                popup.ShowDialog();
                return;
            }

            void Action()
            {
                MainWindowState.IsMergeSorting = true;

                var a = new int[_ints.Length];
                _ints.CopyTo(a, 0);

                var watch = new Stopwatch();
                watch.Start();
                Algorithms.MergeSort(a, 0, a.Length - 1);
                watch.Stop();

                var content = string.Join(" ", a);
                File.WriteAllText($"{OutputPath}/MergeSort.txt", content);

                MainWindowState.IsMergeSorting = false;

                Dispatcher.Invoke(() =>
                {
                    var message = "Array was sorted!";
                    message += $"\nExecution time: {watch.ElapsedMilliseconds}ms";
                    var popup = new PopupMessage("Merge Sort Result", message);
                    popup.ShowDialog();
                });
            }

            var task = new Task(Action);
            task.Start();
        }

        private void ButtonMinimizeWindow_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Minimized : WindowState.Normal;
        }

        private void ListBoxGetCodeItem_OnSelected(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is ListBoxItem listBoxItem)) return;
            var path = "ViewCode";
            var title = "";
            switch (listBoxItem.Name)
            {
                case "GetCodeLinearSearch":
                    title = "Linear Search";
                    path += "/LinearSearch.txt";
                    break;
                case "GetCodeBinarySearch":
                    title = "Binary Search";
                    path += "/BinarySearch.txt";
                    break;
                case "GetCodeInterpolationSearch":
                    title = "Interpolation Search";
                    path += "/InterpolationSearch.txt";
                    break;
                case "GetCodeInterchangeSort":
                    title = "Interchange Sort";
                    path += "/InterchangeSort.txt";
                    break;
                case "GetCodeBubbleSort":
                    title = "Bubble Sort";
                    path += "/BubbleSort.txt";
                    break;
                case "GetCodeSelectionSort":
                    title = "Selection Sort";
                    path += "/SelectionSort.txt";
                    break;
                case "GetCodeInsertionSort":
                    title = "Insertion Sort";
                    path += "/InsertionSort.txt";
                    break;
                case "GetCodeHeapSort":
                    title = "Heap Sort";
                    path += "/HeapSort.txt";
                    break;
                case "GetCodeMergeSort":
                    title = "Merge Sort";
                    path += "/MergeSort.txt";
                    break;
                case "GetCodeQuickSort":
                    title = "Quick Sort";
                    path += "/QuickSort.txt";
                    break;
                default:
                    path += "LinearSearch.txt";
                    break;
            }

            var viewCode = new ViewCode(title, path);
            viewCode.ShowDialog();
        }

        private void ListBoxViewResultItem_OnSelected(object sender, RoutedEventArgs e)
        {
            if (!(e.Source is ListBoxItem listBoxItem)) return;
            var path = OutputPath;
            switch (listBoxItem.Name)
            {
                case "ViewResultInterchangeSort":
                    path += "/InterchangeSort.txt";
                    break;
                case "ViewResultBubbleSort":
                    path += "/BubbleSort.txt";
                    break;
                case "ViewResultSelectionSort":
                    path += "/SelectionSort.txt";
                    break;
                case "ViewResultInsertionSort":
                    path += "/InsertionSort.txt";
                    break;
                case "ViewResultHeapSort":
                    path += "/HeapSort.txt";
                    break;
                case "ViewResultMergeSort":
                    path += "/MergeSort.txt";
                    break;
                case "ViewResultQuickSort":
                    path += "/QuickSort.txt";
                    break;
                default:
                    path += "LinearSearch.txt";
                    break;
            }

            if (!File.Exists(path))
            {
                var popup = new PopupMessage("Array wasn't sorted", "Please sort the array to view result!");
                popup.ShowDialog();
                return;
            }

            Process.Start("notepad.exe", path);
        }

        private void ButtonAuthorInfo_OnClick(object sender, RoutedEventArgs e)
        {
            var popup = new PopupMessage(
                "Application's info",
                "Seminar Data structure and Algorithms\nAuthor: Khiem Le\nWebsite: khiemle.dev"
            );
            popup.ShowDialog();
        }

        private void ButtonToggleWindowState_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void ButtonCloseWindow_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ColorZone_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}