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
    /// References:
    /// https://www.tutorialspoint.com/wpf/wpf_data_binding.htm 
    /// https://learn.microsoft.com/en-us/dotnet/desktop/wpf/controls/?view=netframeworkdesktop-4.8
    /// https://www.w3schools.com/cs/cs_operators_logical.php
    /// https://learn.microsoft.com/en-us/dotnet/desktop/wpf/events/how-to-add-an-event-handler-using-code?view=netdesktop-8.0 
    /// </summary>
    public partial class MainWindow : Window
    {
        //lists to dynamically store the information about the recipes in order to store an infinite amount according to how many recipes the user enters
        public List<string> savedRecipeNames;
        public List<Ingredient> ingredients;
        public List<Recipe> recipes;
        //***************************************************************************************//
        public class Ingredient // a class to store the variables of the ingredients and for better practice
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
            public string Unit { get; set; }
            public string FoodGroup { get; set; }
        }
        //***************************************************************************************//
        public class Recipe //class to store the details related to the recipe and for better practice
        {
            public string Name { get; set; }
            public List<Ingredient> Ingredients { get; set; }
            public string Steps { get; set; }
        }
        //***************************************************************************************//
        public MainWindow()
        {
            InitializeComponent();
            savedRecipeNames = new List<string>();
            ingredients = new List<Ingredient>();
            recipes = new List<Recipe>();
            UpdateSaveRecipeButtonState();
            UpdateRecipeSelectComboBox();
        }
        //***************************************************************************************//
        private void SaveButton_Click(object sender, RoutedEventArgs e)// method to save the recipe name(adapted from previous console app and debugged by claude ai)
        {
            string recipeName = RecipeNameTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(recipeName))
            {
                savedRecipeNames.Add(recipeName);
                MessageBox.Show($"Recipe name '{recipeName}' has been saved!");
                UpdateSaveRecipeButtonState();
            }
            else
            {
                MessageBox.Show("Please enter a recipe name.");
            }
        }
        //***************************************************************************************//
        private void SaveIngredientButton_Click(object sender, RoutedEventArgs e) //method to save the ingredients and their details (debugged and corrected by chatgpt)
        {
            string name = IngredientNameTextBox.Text.Trim();
            string quantity = QuantityTextBox.Text.Trim();
            string unit = UnitTextBox.Text.Trim();
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
                UpdateSaveRecipeButtonState();
            }
            else
            {
                MessageBox.Show("Please fill out all ingredient fields.");
            }
        }
        //***************************************************************************************//
        private void ClearIngredientFields()// method to clear the ingredients field once the ingredients have been saved
        {
            IngredientNameTextBox.Clear();
            QuantityTextBox.Clear();
            UnitTextBox.Clear();
            FoodGroupComboBox.SelectedIndex = -1;
        }
        //***************************************************************************************//
        private void SaveRecipeButton_Click(object sender, RoutedEventArgs e)//method to save all the details of the recipe to an array list (corrected by Claude AI)
        {
            string recipeName = RecipeNameTextBox.Text.Trim();
            string steps = StepsTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(recipeName) && ingredients.Count > 0 && !string.IsNullOrWhiteSpace(steps))
            {
                recipes.Add(new Recipe
                {
                    Name = recipeName,
                    Ingredients = new List<Ingredient>(ingredients),
                    Steps = steps
                });
                //clears all fields to allow for the entering of a new recipe
                ingredients.Clear();
                savedRecipeNames.Clear();
                RecipeNameTextBox.Clear();
                StepsTextBox.Clear();
                ClearIngredientFields();

                MessageBox.Show($"Recipe '{recipeName}' has been saved successfully!");
                UpdateSaveRecipeButtonState();
                UpdateRecipeSelectComboBox();
            }
            else
            {
                string errorMessage = "Please ensure you have:\n";
                if (string.IsNullOrWhiteSpace(recipeName)) errorMessage += " Entered a recipe name\n";
                if (ingredients.Count == 0) errorMessage += "Added at least one ingredient\n";
                if (string.IsNullOrWhiteSpace(steps)) errorMessage += "Provided the recipe steps";

                MessageBox.Show(errorMessage, "Incomplete Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //***************************************************************************************//
        private void DisplayRecipesButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayRecipes();
        }
        //***************************************************************************************//
        private void UpdateSaveRecipeButtonState()// (given by Claude AI)
        {
            SaveRecipeButton.IsEnabled = !string.IsNullOrWhiteSpace(RecipeNameTextBox.Text) &&
                                         ingredients.Count > 0 &&
                                         !string.IsNullOrWhiteSpace(StepsTextBox.Text);
        }
        //***************************************************************************************//
        private void RecipeNameTextBox_TextChanged(object sender, TextChangedEventArgs e) //(given by Claude AI)
        {
            UpdateSaveRecipeButtonState();
        }
        //***************************************************************************************//
        private void StepsTextBox_TextChanged(object sender, TextChangedEventArgs e) //(given by Claude AI)
        {
            UpdateSaveRecipeButtonState();
        }
        //***************************************************************************************//
        public void DisplayRecipes()//method to display the recipes entered (adapted from previous console applicatiion)
        {
            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes saved yet.");
            }
            else
            {
                recipes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase));//sorts the recipes in alphabetical order
                for (int i = 0; i < recipes.Count; i++)
                {
                    MessageBox.Show($"Recipe {i + 1}: {recipes[i].Name}\n" +
                                    $"Ingredients:\n{string.Join("\n", recipes[i].Ingredients.Select(ing => $"{ing.Name} - {ing.Quantity} {ing.Unit} ({ing.FoodGroup})"))}\n" +
                                    $"Steps:\n{recipes[i].Steps}");
                }
            }
        }
        //***************************************************************************************//
        private void UpdateRecipeSelectComboBox()
        {
            RecipeSelectComboBox.Items.Clear();
            foreach (var recipe in recipes.OrderBy(r => r.Name))
            {
                RecipeSelectComboBox.Items.Add(recipe.Name);
            }
        }
        //***************************************************************************************//
        private void DisplaySelectedRecipe_Click(object sender, RoutedEventArgs e)//method to display a specific recipe from all the recipes selected (debugged and corrected by Claude AI)
        {
            if (RecipeSelectComboBox.SelectedItem != null)
            {
                string selectedRecipeName = RecipeSelectComboBox.SelectedItem.ToString();
                Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name == selectedRecipeName);

                if (selectedRecipe != null)
                {
                    string recipeDetails = $"Recipe: {selectedRecipe.Name}\n\n" +
                                           "Ingredients:\n" +
                                           string.Join("\n", selectedRecipe.Ingredients.Select(ing => $"{ing.Name} - {ing.Quantity} {ing.Unit} ({ing.FoodGroup})")) +
                                           "\n\nSteps:\n" +
                                           selectedRecipe.Steps;

                    MessageBox.Show(recipeDetails, "Recipe Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a recipe from the dropdown list.", "No Recipe Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
//*****************************************************end of file******************************************//

