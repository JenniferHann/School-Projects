using System;
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
using Microsoft.Win32;

// ============================================================================
// (c) Sandy Bultena 2018
// * Released under the GNU General Public License
// ============================================================================

namespace Budget
{
    
    public partial class MainWindow : Window
    {
        // ====================================================================
        // global vars
        // ====================================================================
        HomeBudget budget;
        Boolean savedStatus;

        // ====================================================================
        // setup the gui
        // ====================================================================
        public MainWindow()
        {
            InitializeComponent();
            SetBudgetRequiredControls(false);
        }

        #region Data Grid Stuff


        // ====================================================================
        // Update the Data Grid View
        // ====================================================================
        private void UpdateDataGridView()
        {
            // don't do anything if budget is not yest defined
            if (budget == null)
            {
                return;

            }
            // filter options
            bool FilterFlag = cbFilterCategories.IsChecked == true;
            int id = -1;
            if (cmbCategories.SelectedItem != null)
            {
                id = ((Category)cmbCategories.SelectedItem).Id;
            }

            // ----------------------------------------------------------------
            // standard display
            // ----------------------------------------------------------------
            if (cbByCategory.IsChecked != true && cbByMonth.IsChecked != true)
            {
                // create columns for standard display
                CreateDataGridStandardDisplay();

                // set the ItemSource
                dataBudget.ItemsSource = null;
                dataBudget.ItemsSource = budget.GetExpenses(dpStartDate.SelectedDate, dpEndDate.SelectedDate, FilterFlag, id);
            }
            // ----------------------------------------------------------------
            // total by month
            // ----------------------------------------------------------------
            if(cbByMonth.IsChecked == true && cbByCategory.IsChecked != true)
            {
                MonthDisplayMode();

                dataBudget.ItemsSource = null;
                dataBudget.ItemsSource = budget.GetExpensesByMonth(dpStartDate.SelectedDate, dpEndDate.SelectedDate, FilterFlag, id);
            }

            // ----------------------------------------------------------------
            // total by category
            // ----------------------------------------------------------------
            if(cbByMonth.IsChecked != true && cbByCategory.IsChecked == true)
            {
                CategoryDisplayMode();

                dataBudget.ItemsSource = null;
                dataBudget.ItemsSource = budget.GetExpensesByCategory(dpStartDate.SelectedDate, dpEndDate.SelectedDate, FilterFlag, id);
            }
            // ----------------------------------------------------------------
            // total by category and month
            // ----------------------------------------------------------------
            if(cbByMonth.IsChecked == true && cbByCategory.IsChecked == true)
            {
                CategoryAndMonthDisplayMode();

                dataBudget.ItemsSource = null;
                dataBudget.ItemsSource = budget.GetExpensesByCategoryAndMonth(dpStartDate.SelectedDate, dpEndDate.SelectedDate, FilterFlag, id);
            }

        }

        // ====================================================================
        // Create the Columns for the DataGrid Standard Display mode
        // ====================================================================
        private void CreateDataGridStandardDisplay()
        {
            // create columns to display
            dataBudget.Columns.Clear();
            var col = new DataGridTextColumn();
            col.Header = "Date";
            col.Binding = new Binding("Date");
            col.Binding.StringFormat = "dd/MM/yyyy";
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Category";
            col.Binding = new Binding("Category");
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Description";
            col.Binding = new Binding("ShortDescription");
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Amount";
            col.Binding = new Binding("Amount");
            col.Binding.StringFormat = "F2";
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            col.CellStyle = s;
            dataBudget.Columns.Add(col);

            col = new DataGridTextColumn();
            col.Header = "Balance";
            col.Binding = new Binding("Balance");
            col.Binding.StringFormat = "F2";
            col.CellStyle = s;
            dataBudget.Columns.Add(col);

        }

        // ====================================================================
        // Create the Columns for the DataGrid by Month Display mode
        // ====================================================================
       public void MonthDisplayMode()
       {
            dataBudget.Columns.Clear();

            var col = new DataGridTextColumn();
            col.Header = "Month";
            col.Binding = new Binding("Month");
            col.Binding.StringFormat = "MM/yyyy";
            dataBudget.Columns.Add(col);

            var col1 = new DataGridTextColumn();
            col1.Header = "Total";
            col1.Binding = new Binding("Total");
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            col1.CellStyle = s;
            dataBudget.Columns.Add(col1);

        }

        // ====================================================================
        // Create the Columns for the DataGrid by Category Display mode
        // ====================================================================
        public void CategoryDisplayMode()
        {
            dataBudget.Columns.Clear();

            var col = new DataGridTextColumn();
            col.Header = "Category";
            col.Binding = new Binding("Category");
            dataBudget.Columns.Add(col);

            var col1 = new DataGridTextColumn();
            col1.Header = "Total";
            col1.Binding = new Binding("Total");
            dataBudget.Columns.Add(col1);

        }
        // ====================================================================
        // Create the Columns for the DataGrid by Category And Month Display mode
        // ====================================================================
        public void CategoryAndMonthDisplayMode()
        {
            dataBudget.Columns.Clear();

            var col = new DataGridTextColumn();
            col.Header = "Month";
            col.Binding = new Binding("Month");
            col.Binding.StringFormat = "MM/yyyy";
            dataBudget.Columns.Add(col);

            foreach(Category category in budget.categories.List())
            {
                var col1 = new DataGridTextColumn();
                col1.Header = category.Description;
                col1.Binding = new Binding(category.Description);
                col1.Binding.StringFormat = "F2";
                Style ss = new Style();
                ss.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
                col1.CellStyle = ss;
                dataBudget.Columns.Add(col1);
            }

            

            var col2 = new DataGridTextColumn();
            col2.Header = "Total";
            col2.Binding = new Binding("Total");
            col2.Binding.StringFormat = "F2";
            Style s = new Style();
            s.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
            col2.CellStyle = s;
            dataBudget.Columns.Add(col2);
        }

        #endregion

        #region setting controls and stuff
        // ====================================================================
        // Set the controls (enabled/disabled) that require a 
        // budget object to be defined before these controls can be used
        // and sets the menu buttons to be enabled or disabled
        // ====================================================================
        private void SetBudgetRequiredControls(bool isDirty)
        {
            // saved status is opposite of dirty
            savedStatus = !isDirty;

            // can only add an expense if budget is defined
            btnAddExpense.IsEnabled = budget != null;

            // set info on status bar
            if (savedStatus == true)
            {
                txtSavedStatus.Text = "Saved";
            }
            else if (savedStatus == false)
            {
                txtSavedStatus.Text = "Not Saved";
            }

            // -----------------------------------------------------------
            // if budget is defined, then enable/disable certain controls
            // AND update the categories list
            if (budget != null)
            {
                txbFileName.Text = budget.PathName;
            }

            // -----------------------------------------------------------
            // menu and taskbar update
            menuSave.IsEnabled = budget != null && budget.FileName != null && savedStatus != true;
            menuSaveAs.IsEnabled = budget != null;
            modifySelectedItem.IsEnabled = dataBudget.SelectedItem != null;
            toolBarSave.IsEnabled = budget != null && budget.FileName != null && savedStatus != true;
            toolBarSaveAs.IsEnabled = budget != null;
            toolBarModify.IsEnabled = dataBudget.SelectedItem != null;
        }

        // ====================================================================
        // set categories
        // ====================================================================
        private void SetCategories()
        {
            cmbCategories.ItemsSource = null;
            cmbCategories.ItemsSource = budget.categories.List().OrderBy(c => c.Description);
        }
        #endregion

        #region New / Open
        // ====================================================================
        // New budget
        // ====================================================================
        private void NewBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // Always use a try catch, just in case
            try
            {
                // bail out if we do have not saved changed changes, and do not
                // want to continue
                if (savedStatus == false && ContinueAndLoseChanges() == false)
                {
                    return;
                }

                // create a new budget
                budget = new HomeBudget();
                UpdateDataGridView();

                // update controls
                SetBudgetRequiredControls(false);

                // reset the categories
                SetCategories();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Creating New Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //using the Click event
        /*private void menuNew_Click(object sender, RoutedEventArgs e)
        {
            // Always use a try catch, just in case
            try
            {
                // bail out if we do have not saved changed changes, and do not
                // want to continue
                if (savedStatus == false && ContinueAndLoseChanges() == false)
                {
                    return;
                }

                // create a new budget
                budget = new HomeBudget();
                UpdateDataGridView();

                // update controls
                SetBudgetRequiredControls(false);

                // reset the categories
                SetCategories();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Creating New Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }*/

        // ====================================================================
        // Open budget
        // ====================================================================
        private void OpenBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // bail out if we have not saved changes, and do not
                // want to continue
                if (savedStatus == false && ContinueAndLoseChanges() == false)
                {
                    return;
                }

                // open file dialog box
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Budget Files|*.budget|All Files|*.*";

                // if user selects a file to read
                if (openFileDialog.ShowDialog() == true)
                {
                    // create a new budget
                    budget = new HomeBudget(openFileDialog.FileName);
                    UpdateDataGridView();

                    // update controls
                    SetBudgetRequiredControls(false);

                    // reset the categories
                    SetCategories();
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Openning Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //using Click event
        /*private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // bail out if we have not saved changes, and do not
                // want to continue
                if (savedStatus == false && ContinueAndLoseChanges() == false)
                {
                    return;
                }

                // open file dialog box
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Budget Files|*.budget|All Files|*.*";

                // if user selects a file to read
                if (openFileDialog.ShowDialog() == true)
                {
                    // create a new budget
                    budget = new HomeBudget(openFileDialog.FileName);
                    UpdateDataGridView();

                    // update controls
                    SetBudgetRequiredControls(false);

                    // reset the categories
                    SetCategories();
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Openning Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }*/
        #endregion

        #region Save and SaveAs

        // ====================================================================
        // Save
        // ====================================================================
        private void SaveBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SaveBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                // if we have a pathname, save, else call SaveAs
                if (budget.PathName != null)
                {
                    budget.SaveToFile(budget.PathName);
                    SetBudgetRequiredControls(false);
                }

                else
                {
                    menuSaveAs_Click(new object(), new RoutedEventArgs());
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //using Click event
        /*private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // if we have a pathname, save, else call SaveAs
                if (budget.PathName != null)
                {
                    budget.SaveToFile(budget.PathName);
                    SetBudgetRequiredControls(false);
                }

                else
                {
                    menuSaveAs_Click(new object(), new RoutedEventArgs());
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }*/


        // ====================================================================
        // Save As
        // ====================================================================
        private void menuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Budget Files|*.budget|All Files|*.*";

                // if user selects a file to save text to
                if (saveFileDialog.ShowDialog() == true)
                {
                    budget.SaveToFile(saveFileDialog.FileName);
                }

                // update Controls
                SetBudgetRequiredControls(false);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving Budget", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        // ====================================================================
        // Do we continue and lose existing changes
        // ====================================================================
        private bool ContinueAndLoseChanges()
        {
            MessageBoxResult r = MessageBox.Show("Current Budget is not saved\nOpening a new one will erase all changes\nDo you want to Continue?", "Budget not saved", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No);
            return r == MessageBoxResult.Yes;
        }


        #endregion

        #region closing the app

        // ====================================================================
        // Are we sure we want to close this window?
        // ====================================================================
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (savedStatus == false)
            {
                e.Cancel = !ContinueAndLoseChanges();
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        // ====================================================================
        // Exit the program
        // ====================================================================

        private void ExitBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //using Click event
        /*private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/
        #endregion


        #region event handlers for requiring DataGridView refresh 
        // ====================================================================
        // Refresh DataGrid because display options have changed
        // ====================================================================
        private void cmbCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilterCategories.IsChecked == true)
            {
                UpdateDataGridView();
            }
        }

        private void cbFilterCategories_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void dpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void cbByMonth_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGridView();
        }

        private void cbByCategory_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGridView();
        }
        #endregion

        #region add expense
        // ====================================================================
        // Add Expense
        // ====================================================================
        private void btnAddExpense_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // TODO: create and show Expense Form

                ExpenseForm expense = new ExpenseForm();
                expense.Budget = budget;
                expense.Themes = Themes.Add;
                expense.ShowDialog();
                UpdateDataGridView();

                // Update budget required controls
                SetBudgetRequiredControls(true);

                // set the focus on the last element of the budget
                ResetFocusAfterUpdate(dataBudget.Items.Count - 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Adding Expense", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region ResetFocusAfterUpdate 

        // ==========================================================================
        // call this after modifying or adding an expense, it sets up the focus
        // of the datagrid
        // ==========================================================================
        private void ResetFocusAfterUpdate( int index)
        {
            try
            {
                // set the index of what we want to be selected, make sure that 
                // it is not larger than the number of items
                if (index >= dataBudget.Items.Count)
                {
                    index = dataBudget.Items.Count - 1;
                }

                // set the selected index, and then set focus onto the datagrid
                dataBudget.SelectedIndex = index;
                dataBudget.Focus();

                // if we have a "real" selected index, set the current cell (puts the 
                // focus truly on that row)
                if (index != -1)
                {
                    dataBudget.CurrentCell = new DataGridCellInfo(dataBudget.Items[index], dataBudget.Columns[0]);
                }
            }
            catch
            {
            }

        }
        #endregion

        

        private void ModifyBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        
        private void ModifyBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (modifySelectedItem.IsEnabled == true)
            {
                ExpenseForm expenseForm = new ExpenseForm();
                expenseForm.Themes = Themes.Modify;
                expenseForm.Budget = budget;

                //get the selected item to modify
                BudgetItem item = dataBudget.SelectedItem as BudgetItem;
                expenseForm.ExpenseID = item.ExpenseID;
                expenseForm.ShowDialog();
                budget = expenseForm.Budget;

                UpdateDataGridView();
                ResetFocusAfterUpdate(dataBudget.SelectedIndex);
            }
        }

        private void dataBudget_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            toolBarModify.IsEnabled = true;
            modifySelectedItem.IsEnabled = true;
        }

        private void dataBudget_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ExpenseForm expenseForm = new ExpenseForm();
            expenseForm.Themes = Themes.Modify;
            expenseForm.Budget = budget;

            //get the selected item to modify
            BudgetItem item = dataBudget.SelectedItem as BudgetItem;
            expenseForm.ExpenseID = item.ExpenseID;
            expenseForm.ShowDialog();
            budget = expenseForm.Budget;
            UpdateDataGridView();
            ResetFocusAfterUpdate(dataBudget.SelectedIndex);
        }

        private void DeleteExpense_Click(object sender, RoutedEventArgs e)
        {
            BudgetItem item = dataBudget.SelectedItem as BudgetItem;
            budget.expenses.Delete(item.ExpenseID);
            UpdateDataGridView();
        }
    }

    public static class CustomCommands
    {
        public static readonly RoutedUICommand Exit = new RoutedUICommand
        (
            "Exit",
            "Exit",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                    new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
        );

        public static readonly RoutedUICommand Modify = new RoutedUICommand
        (
           "Modify",
           "Modify",
           typeof(CustomCommands),
           new InputGestureCollection()
           {
                   new KeyGesture(Key.M, ModifierKeys.Control)
           }
        );

        public static readonly RoutedUICommand New = new RoutedUICommand
        (
            "New",
            "New",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Open = new RoutedUICommand
        (
            "Open",
            "Open",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Save = new RoutedUICommand
        (
            "Save",
            "Save",
            typeof(CustomCommands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            }
        );
    }
}

