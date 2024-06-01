using Microsoft.Toolkit.Mvvm.Input;
using SoundAtlas.Models;
using SoundAtlas.Views.VirtualInstrument.Instrument.Categories;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using Microsoft.Win32;
using System.Text;
using System.IO;

namespace SoundAtlas.ViewModels.VirtualInstrument.Instrument.Categories
{
    public class CategoryViewModel
    {
        private readonly DatabaseService _databaseService;
        public ObservableCollection<CategoryItemViewModel> Categories { get; private set; }
        private ObservableCollection<CategoryItemViewModel> _allCategories; // すべてのカテゴリーを保持するコレクション

        public string? SearchText { get; set; }
        public string? SelectedSearchColumn { get; set; }
        public ICommand ShowCategoryUpdateCommand { get; private set; }
        public ICommand? DeleteSelectedCategoriesCommand { get; private set; }

        public CategoryViewModel()
        {
            _databaseService = new DatabaseService();
            Categories = new ObservableCollection<CategoryItemViewModel>();
            _allCategories = new ObservableCollection<CategoryItemViewModel>();
            Application.Current.Dispatcher.Invoke(() => {
                LoadCategories();
            });
            ShowCategoryUpdateCommand = new RelayCommand<CategoryItemViewModel>(ShowCategoryUpdate);
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            // データベースの変更があった場合に単語リストを再読み込み
            Application.Current.Dispatcher.Invoke(() => {
                LoadCategories();
            });
        }

        public void LoadCategories()
        {
            Categories.Clear(); // 既存のコレクションをクリアする
            _allCategories.Clear(); // 既存のコレクションをクリアする

            // データベースからデータを取得
            var categoryList = _databaseService.GetAllEntities<InstrumentCategoryModel>().Select(category => new CategoryItemViewModel
            {
                CategoryId = category.InstrumentCategoryId,
                Classification1 = category.Classification1,
                Classification2 = category.Classification2,
                Classification3 = category.Classification3,
                Classification4 = category.Classification4,
                IsSelected = false
            }).ToList();

            // コレクションにデータを追加
            foreach (var category in categoryList)
            {
                Categories.Add(category);
                _allCategories.Add(category);
            }
        }

        public void SearchCategories()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // 検索テキストが空の場合、全データを表示
                Categories.Clear();
                foreach (var category in _allCategories)
                {
                    Categories.Add(category);
                }
            }
            else
            {
                IEnumerable<CategoryItemViewModel> filteredCategories = _allCategories;

                if (SelectedSearchColumn == "Classification1")
                {
                    filteredCategories = filteredCategories.Where(w => w.Classification1 != null && w.Classification1.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedSearchColumn == "Classification2")
                {
                    filteredCategories = filteredCategories.Where(w => w.Classification2 != null && w.Classification2.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedSearchColumn == "Classification3")
                {
                    filteredCategories = filteredCategories.Where(w => w.Classification3 != null && w.Classification3.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }
                else if (SelectedSearchColumn == "Classification4")
                {
                    filteredCategories = filteredCategories.Where(w => w.Classification4 != null && w.Classification4.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                }

                Categories.Clear();
                foreach (var category in filteredCategories)
                {
                    Categories.Add(category);
                }
            }
        }

        private void ShowCategoryUpdate(CategoryItemViewModel? category)
        {
            if (category == null)
            {
                // 例: 適切なエラーメッセージを表示
                MessageBox.Show("The selected category is null.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // CategoryDetailViewModelのインスタンスを生成し、適切なモーダルビューを開く
            var updateViewModel = new CategoryUpdateViewModel(category.CategoryId);
            var updateModal = new CategoryUpdateModalView(updateViewModel);
            updateModal.ShowDialog();
        }

        public void ExportCsv()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv",
                FileName = "categories_export.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                StringBuilder csvContent = new StringBuilder();
                csvContent.AppendLine("Classification1,Classification2,Classification3,Classification4");

                foreach (var category in Categories)
                {
                    csvContent.AppendLine($"{category.Classification1},{category.Classification2},{category.Classification3},{category.Classification4}");
                }

                File.WriteAllText(saveFileDialog.FileName, csvContent.ToString());
                MessageBox.Show("CSV Export successful", "Export successful", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void ImportCsv()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV file (*.csv)|*.csv"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    var csvContent = File.ReadAllLines(openFileDialog.FileName);
                    var categories = new List<CategoryItemViewModel>();

                    foreach (var line in csvContent.Skip(1)) // ヘッダー行をスキップ
                    {
                        var columns = line.Split(',');
                        if (columns.Length == 4)
                        {
                            categories.Add(new CategoryItemViewModel
                            {
                                Classification1 = columns[0],
                                Classification2 = columns[1],
                                Classification3 = columns[2],
                                Classification4 = columns[3]
                            });
                        }
                    }

                    foreach (var category in categories)
                    {
                        if (!Categories.Any(c => c.Classification1 == category.Classification1 && c.Classification2 == category.Classification2 
                        && c.Classification3 == category.Classification3 && c.Classification4 == category.Classification4))
                        {
                            var newCategory = new InstrumentCategoryModel
                            {
                                Classification1 = category.Classification1,
                                Classification2 = category.Classification2,
                                Classification3 = category.Classification3,
                                Classification4 = category.Classification4
                            };

                            _databaseService.AddEntity(newCategory);
                            Categories.Add(new CategoryItemViewModel
                            {
                                CategoryId = category.CategoryId,
                                Classification1 = category.Classification1,
                                Classification2 = category.Classification2,
                                Classification3 = category.Classification3,
                                Classification4 = category.Classification4,
                                IsSelected = false
                            });
                        }
                    }

                    MessageBox.Show("CSVファイルのインポートが完了しました。", "インポート成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"CSVファイルのインポートに失敗しました: {ex.Message}", "インポート失敗", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class CategoryItemViewModel
    {
        public int CategoryId { get; set; }
        public string Classification1 { get; set; } = "";
        public string? Classification2 { get; set; }
        public string? Classification3 { get; set; }
        public string? Classification4 { get; set; }
        public bool IsSelected { get; set; }
    }

}