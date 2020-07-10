using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeminarCTDLGT.Classes
{
    public class MainWindowState : INotifyPropertyChanged
    {
        private bool _isGeneratingArray;
        private bool _isSavingFile;
        private bool _isLoadingFile;
        private bool _isLinearSearching;
        private bool _isBinarySearching;
        private bool _isInterpolationSearching;
        private bool _isInterchangeSorting;
        private bool _isBubbleSorting;
        private bool _isSelectionSorting;
        private bool _isInsertionSorting;
        private bool _isHeapSorting;
        private bool _isQuickSorting;
        private bool _isMergeSorting;

        public bool IsGeneratingArray
        {
            get => _isGeneratingArray;
            set
            {
                _isGeneratingArray = value;
                GenerateButtonContent = value ? "Generating..." : "Generate Array";
                OnPropertyChanged("IsGeneratingArray");
                OnPropertyChanged("GenerateButtonContent");
            }
        }

        public bool IsSavingFile
        {
            get => _isSavingFile;
            set
            {
                _isSavingFile = value;
                SavingFileContent = value ? "Saving..." : "Save to file";
                OnPropertyChanged("IsSavingFile");
                OnPropertyChanged("SavingFileContent");
            }
        }

        public bool IsLoadingFile
        {
            get => _isLoadingFile;
            set
            {
                _isLoadingFile = value;
                LoadingFileContent = value ? "Loading..." : "Load data from file";
                OnPropertyChanged("IsLoadingFile");
                OnPropertyChanged("LoadingFileContent");
            }
        }

        public bool IsLinearSearching
        {
            get => _isLinearSearching;
            set
            {
                _isLinearSearching = value;
                LinearSearchingContent = value ? "Searching..." : "Linear Search";
                OnPropertyChanged("IsLinearSearching");
                OnPropertyChanged("LinearSearchingContent");
            }
        }

        public bool IsBinarySearching
        {
            get => _isBinarySearching;
            set
            {
                _isBinarySearching = value;
                BinarySearchingContent = value ? "Searching..." : "Binary Search";
                OnPropertyChanged("IsBinarySearching");
                OnPropertyChanged("BinarySearchingContent");
            }
        }

        public bool IsInterpolationSearching
        {
            get => _isInterpolationSearching;
            set
            {
                _isInterpolationSearching = value;
                InterpolationSearchingContent = value ? "Searching.." : "Interpolation Search";
                OnPropertyChanged("IsInterpolationSearching");
                OnPropertyChanged("InterpolationSearchingContent");
            }
        }

        public bool IsInterchangeSorting
        {
            get => _isInterchangeSorting;
            set
            {
                _isInterchangeSorting = value;
                InterchangeSortingContent = value ? "Sorting..." : "Interchange Sort";
                OnPropertyChanged("IsInterchangeSorting");
                OnPropertyChanged("InterchangeSortingContent");
            }
        }

        public bool IsBubbleSorting
        {
            get => _isBubbleSorting;
            set
            {
                _isBubbleSorting = value;
                BubbleSortingContent = value ? "Sorting..." : "Bubble Sort";
                OnPropertyChanged("IsBubbleSorting");
                OnPropertyChanged("BubbleSortingContent");
            }
        }

        public bool IsSelectionSorting
        {
            get => _isSelectionSorting;
            set
            {
                _isSelectionSorting = value;
                SelectionSortingContent = value ? "Sorting..." : "Selection Sort";
                OnPropertyChanged("IsSelectionSorting");
                OnPropertyChanged("SelectionSortingContent");
            }
        }

        public bool IsInsertionSorting
        {
            get => _isInsertionSorting;
            set
            {
                _isInsertionSorting = value;
                InsertionSortingContent = value ? "Sorting..." : "Insertion Sort";
                OnPropertyChanged("IsInsertionSorting");
                OnPropertyChanged("InsertionSortingContent");
            }
        }

        public bool IsHeapSorting
        {
            get => _isHeapSorting;
            set
            {
                _isHeapSorting = value;
                HeapSortingContent = value ? "Sorting..." : "Heap Sort";
                OnPropertyChanged("IsHeapSorting");
                OnPropertyChanged("HeapSortingContent");
            }
        }

        public bool IsQuickSorting
        {
            get => _isQuickSorting;
            set
            {
                _isQuickSorting = value;
                QuickSortingContent = value ? "Sorting.." : "Quick Sort";
                OnPropertyChanged("IsQuickSorting");
                OnPropertyChanged("QuickSortingContent");
            }
        }

        public bool IsMergeSorting
        {
            get => _isMergeSorting;
            set
            {
                _isMergeSorting = value;
                MergeSortingContent = value ? "Sorting..." : "Merge Sort";
                OnPropertyChanged("IsMergeSorting");
                OnPropertyChanged("MergeSortingContent");
            }
        }

        public string GenerateButtonContent { get; private set; }

        public string SavingFileContent { get; private set; }

        public string LoadingFileContent { get; private set; }

        public string LinearSearchingContent { get; private set; }

        public string BinarySearchingContent { get; private set; }

        public string InterpolationSearchingContent { get; private set; }

        public string InterchangeSortingContent { get; private set; }

        public string BubbleSortingContent { get; private set; }

        public string SelectionSortingContent { get; private set; }

        public string InsertionSortingContent { get; private set; }

        public string HeapSortingContent { get; private set; }

        public string QuickSortingContent { get; private set; }

        public string MergeSortingContent { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}