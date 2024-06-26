using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static WpfAttempt3.MainWindow;

namespace WpfAttempt3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> savedRecipeNames;
        public List<Ingredient> ingredients;

        public class Ingredient
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
            public string Unit { get; set; }
            public string FoodGroup { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();
            savedRecipeNames = new List<string>();
            ingredients = new List<Ingredient>();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = RecipeNameTextBox.Text;
            if (!string.IsNullOrWhiteSpace(recipeName))
            {
                savedRecipeNames.Add(recipeName);
                RecipeNameTextBox.Clear();
                MessageBox.Show($"Recipe name '{recipeName}' saved!");
            }
            else
            {
                MessageBox.Show("Please enter a recipe name.");
            }
        }

        private void SaveIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            string name = IngredientNameTextBox.Text;
            string quantity = QuantityTextBox.Text;
            string unit = UnitTextBox.Text;
            string foodGroup = (FoodGroupComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(quantity) &&
                !string.IsNullOrWhiteSpace(unit) && foodGroup != null)
            {
                ingredients.Add(new Ingredient
                {
                    Name = name,
                    Quantity = quantity,
                    Unit = unit,
                    FoodGroup = foodGroup
                });
                MessageBox.Show($"Ingredient '{name}' has been saved!");
                ClearIngredientFields();
            }
            else
            {
                MessageBox.Show("Please fill out all ingredient fields.");
            }
        }

        private void ClearIngredientFields()
        {
            IngredientNameTextBox.Clear();
            QuantityTextBox.Clear();
            UnitTextBox.Clear();
            FoodGroupComboBox.SelectedIndex = -1;
        }
    }
}

