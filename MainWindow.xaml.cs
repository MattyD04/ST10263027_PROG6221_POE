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
    /// https://www.geeksforgeeks.org/c-sharp-list-class/
    /// https://docs.jmix.io/1.x/jmix/1.5/ui/vcl/containers/group-box-layout.html
    /// https://www.geeksforgeeks.org/lambda-expressions-in-c-sharp/
    /// https://learn.microsoft.com/en-us/dotnet/api/system.string.isnullorempty?view=net-8.0
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
            public string Name { get; set; } // variable for the name of an ingredient
            public string Quantity { get; set; } // variable for the quantity of a specific ingredient
            public string Unit { get; set; } //variable for the unit of measurement
            public string FoodGroup { get; set; } //variable for the food group
            public int Calories { get; set; } // Added Calories property
        }
        //***************************************************************************************//
        public class Recipe //class to store the details related to the recipe and for better practice
        {
            public string Name { get; set; } //variable for the name of a recipe
            public List<Ingredient> Ingredients { get; set; } //ingredients list to store ingredients
            public string Steps { get; set; } //variable for the steps of the recipe
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
            FilterFoodGroupComboBox.SelectedIndex = 0; // Select "All" by default
        }
        //***************************************************************************************//
        private void SaveButton_Click(object sender, RoutedEventArgs e)// method to save the recipe name(adapted from previous console app and debugged by claude AI)
        {
            string recipeName = RecipeNameTextBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(recipeName))
            {
                savedRecipeNames.Add(recipeName);
                MessageBox.Show($"Recipe name '{recipeName}' has been saved!");// message box displays to notify user when a recipe name has been saved
                UpdateSaveRecipeButtonState();
            }
            else
            {
                MessageBox.Show("Please enter a recipe name.");//displays a message if the user does not enter a recipe name
            }
        }
        //***************************************************************************************//
        private void SaveIngredientButton_Click(object sender, RoutedEventArgs e) //method to save the ingredients and their details (debugged and corrected by chatgpt)
        {
            string name = IngredientNameTextBox.Text.Trim(); //gets the name of the ingredients text
            string quantity = QuantityTextBox.Text.Trim(); //gets quatities text
            string unit = UnitTextBox.Text.Trim();//gets units text
            string foodGroup = (FoodGroupComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string caloriesText = CaloriesTextBox.Text.Trim(); // Get calories text

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(quantity) &&
                !string.IsNullOrWhiteSpace(unit) && foodGroup != null && !string.IsNullOrWhiteSpace(caloriesText))
            {
                if (int.TryParse(caloriesText, out int calories)) // Parse calories
                {
                    //constructor for ingredients
                    ingredients.Add(new Ingredient
                    {
                        Name = name,
                        Quantity = quantity,
                        Unit = unit,
                        FoodGroup = foodGroup,
                        Calories = calories
                    });
                    MessageBox.Show($"Ingredient '{name}' has been saved! You can now enter another ingredient by filling in the fields again"); //displays if all fields have been filled
                    ClearIngredientFields();
                    UpdateSaveRecipeButtonState();
                }
                else
                {
                    MessageBox.Show("Please enter a valid number for calories."); //if user enters a negative number, this message shall display
                }
            }
            else
            {
                MessageBox.Show("Please fill out all ingredient fields."); //displays a message if a user tries to save an ingredient without filling in all the fields
            }
        }
        //***************************************************************************************//
        private void ClearIngredientFields()// method to clear the ingredients field once the ingredients have been saved
        {
            IngredientNameTextBox.Clear(); //clears recipe textbox
            QuantityTextBox.Clear(); //clears quantity textbox
            UnitTextBox.Clear(); //clears unit text box
            FoodGroupComboBox.SelectedIndex = -1; //clears food group combo box
            CaloriesTextBox.Clear(); // Clear calories textbox
        }
        //***************************************************************************************//
        private void SaveRecipeButton_Click(object sender, RoutedEventArgs e)//method to save all the details of the recipe to an array list (corrected by Claude AI)
        {
            string recipeName = RecipeNameTextBox.Text.Trim();
            string steps = StepsTextBox.Text.Trim();

            if (!string.IsNullOrWhiteSpace(recipeName) && ingredients.Count > 0 && !string.IsNullOrWhiteSpace(steps))
            {
                //constructor for the recipe
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
                //errror message that displays if a user has not provided any of the necessary information in the fields but tries saving anyway
                string errorMessage = "Please ensure you have:\n";
                if (string.IsNullOrWhiteSpace(recipeName)) errorMessage += " Entered a recipe name\n";
                if (ingredients.Count == 0) errorMessage += "Added at least one ingredient\n";
                if (string.IsNullOrWhiteSpace(steps)) errorMessage += "Provided the recipe steps";

                MessageBox.Show(errorMessage, "Incomplete Recipe", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //***************************************************************************************//
        private void DisplayRecipesButton_Click(object sender, RoutedEventArgs e) //(given by Claude AI)
        {
            DisplayRecipes();
        }
        //***************************************************************************************//
        private void UpdateSaveRecipeButtonState()// (given by Claude AI)
        {
            // enables the save recipe button once all fields have been entered
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
                                   $"Ingredients:\n{string.Join("\n", recipes[i].Ingredients.Select(ing => $"{ing.Quantity} x {ing.Name} {ing.Unit} ({ing.FoodGroup}) - {ing.Calories} calories"))}\n" +
                                    $"Total Calories: {recipes[i].Ingredients.Sum(ing => ing.Calories)}\n" +
                                    $"Steps:\n{recipes[i].Steps}"); //displays a message box with the details of the recipe
                }
            }
        }
        //***************************************************************************************//
        private void UpdateRecipeSelectComboBox() // method to update the recipe selection combo box where the recipes are displayed in alphabetical order(given by Claude AI)
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
                    //messagebox that displays when a user wants to display the contents of a specific recipe
                    string recipeDetails = $"Recipe: {selectedRecipe.Name}\n\n" +
                                           "Ingredients:\n" +
                                           string.Join("\n", selectedRecipe.Ingredients.Select(ing => $"{ing.Name} - {ing.Quantity} {ing.Unit} ({ing.FoodGroup}) - {ing.Calories} calories")) +
                                           $"\n\nTotal Calories: {selectedRecipe.Ingredients.Sum(ing => ing.Calories)}\n\n" +
                                           "Steps:\n" +
                                           selectedRecipe.Steps;

                    MessageBox.Show(recipeDetails, "Recipe Details", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Please select a recipe from the dropdown list.", "No Recipe Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        //***************************************************************************************//
        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)//method to handle the filter feature (corrected by Claude AI)
        {
            string ingredientFilter = FilterIngredientTextBox.Text.Trim().ToLower();
            string foodGroupFilter = (FilterFoodGroupComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            int maxCalories;
            int.TryParse(FilterMaxCaloriesTextBox.Text, out maxCalories);

            var filteredRecipes = recipes.Where(recipe =>
                (string.IsNullOrEmpty(ingredientFilter) || recipe.Ingredients.Any(i => i.Name.ToLower().Contains(ingredientFilter))) &&
                (foodGroupFilter == "All" || foodGroupFilter == null || recipe.Ingredients.Any(i => i.FoodGroup == foodGroupFilter)) &&
                (maxCalories == 0 || CalculateTotalCalories(recipe) <= maxCalories)
            ).ToList();

            FilteredRecipesListBox.ItemsSource = filteredRecipes.Select(r => r.Name);
        }
        //***************************************************************************************//
        private int CalculateTotalCalories(Recipe recipe) //handles the calculation of the total calories
        {
            return recipe.Ingredients.Sum(i => i.Calories);
        }
        //***************************************************************************************//
    }
}
//*****************************************************end of file******************************************//

