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
using System.Windows.Shapes;

namespace Budget
{
    // ====================================================================
    // enums
    // ====================================================================
    public enum Themes { Add, Modify }


    // ====================================================================
    // Expense Form
    // ====================================================================
    public partial class ExpenseForm : Window
    {

        // ====================================================================
        // global variables
        // ====================================================================
        private bool creditCategoryExists = false;
        private Category creditCategory;
        private HomeBudget _budget;
        private Themes _themes;
        //private Expense expense;
        // ====================================================================
        // properites
        // ====================================================================

        // create a property for budget...
        public HomeBudget Budget
        {
            get
            {
                return _budget;
            }
            set
            {
                _budget = value;
                AddCategoryFields();
            }
        }
        //    ... whenever it is set,
        //          add the budget.categories of the combo box!

        //create a properties for themes       
        public Themes Themes
        {
            get
            {
                return _themes;
            }
            set
            {
                _themes = value;
                if (value == Themes.Add)
                {
                    btnAdd.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    btnClose.Visibility = Visibility.Visible;
                    cbCredit.Visibility = Visibility.Visible;

                    btnModify.Visibility = Visibility.Collapsed;
                    btnDelete.Visibility = Visibility.Collapsed;
                    btnCancelExpenseForm.Visibility = Visibility.Collapsed;

                    SolidColorBrush brush = new SolidColorBrush(Colors.Red);
                    StackPanelEF.Background = brush;

                    txtMainTitle.Text = "Add Expense";
                }
                if (value == Themes.Modify)
                {
                    btnAdd.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnClose.Visibility = Visibility.Collapsed;
                    cbCredit.Visibility = Visibility.Collapsed;

                    btnModify.Visibility = Visibility.Visible;
                    btnDelete.Visibility = Visibility.Visible;
                    btnCancelExpenseForm.Visibility = Visibility.Visible;

                    SolidColorBrush brush = new SolidColorBrush(Colors.Aquamarine);
                    StackPanelEF.Background = brush;

                    txtMainTitle.Text = "Modify Expense";


                }
            }
        }

        //create a properties for an expense ID
        private int _expenseID;
        
        public int ExpenseID
        {
            get
            {
                return _expenseID;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("The exense ID can't be under 0.");
                }                
                _expenseID = value;

                Expense expense = _budget.expenses.Get(_expenseID);

                cmbCategoryList.SelectedItem = expense.Category;
                txtDate.SelectedDate = expense.Date;
                txtDescription.Text = expense.Description;
                txtAmount.Text = expense.Amount.ToString();
            }
        }

        // ====================================================================
        // setup the gui
        // ====================================================================
        public ExpenseForm()
        {
            InitializeComponent();

            DataObject.AddPastingHandler(txtAmount, NumberPasteHandler);
        }

        #region only real numbers in the textbox
        // ====================================================================
        // Limit what can be pasted into any textbox that is supposed to be
        // a number (real number)
        // ====================================================================
        private void NumberPasteHandler(object sender, DataObjectPastingEventArgs e)
        {
            // define textbox
            TextBox tb = sender as TextBox;

            // can the pasting data be converted to a string?
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                // get the pasted text
                string text = e.DataObject.GetData(typeof(string)) as string;

                // if it cannot be converted to a number, cancel the command
                Double number;
                if (!Double.TryParse(text, out number))
                {
                    e.CancelCommand();
                }

            }

        }

        // ====================================================================
        // Limit what can be typed into any textbox that is supposed to be
        // a number (real number)
        // ====================================================================
        private void NumberPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // what would the final text be?
            TextBox tb = sender as TextBox;
            String newText = tb.Text + e.Text;

            // continue the processing of the new text
            e.Handled = false;

            // could be starting to type a number with a negative, 
            // or ".", so those should be allowed
            if (newText == "." || newText == "-")
            {
                return;
            }

            // if not starting with "." or "-", the new text should be a valid number,
            // so if it is not, stop processing
            Double number;
            if (!Double.TryParse(newText, out number))
            {
                e.Handled = true;
            }

            return;

        }
        #endregion

        #region add category fields to combobox
        // ====================================================================
        // Add the category fields to the combobox
        // ====================================================================
        public void AddCategoryFields()
        {
            // add each category to the combobox
            try
            {
                List<Category> catlist = _budget.categories.List();
                foreach (Category c in catlist.OrderBy(x => x.Description))
                {
                    if (c.Type == Category.CategoryType.Credit)
                    {
                        creditCategoryExists = true;
                        creditCategory = c;
                    }
                    cmbCategoryList.Items.Add(c);
                }

                // set the selection to the first item
                cmbCategoryList.SelectedIndex = 0;

                // if we have a credit card type, then display credit checkbox
                if (creditCategoryExists)
                {
                    cbCredit.Visibility = Visibility.Visible;
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "AddCategoryFields Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        #region button event handlers

        // ====================================================================
        // Cancel Expense
        // ====================================================================
        private void CancelExpense_Click(object sender, RoutedEventArgs e)
        {

            // empty appropriate text boxes
            txtAmount.Text = "";
            txtDescription.Text = "";
            txtAmount.Text = "";
            cmbCategoryList.SelectedIndex = 0;
            cbCredit.IsChecked = false;

            // set the last action
            txtLastAction.Text = "Expense cancelled";

        }

        // ====================================================================
        // Close the Window
        // ====================================================================
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        // ====================================================================
        // clear invalid markers
        // ====================================================================
        private void ClearInvalidMarkers()
        {
            txtCategoryInvalid.Visibility = Visibility.Hidden;
            txtDateInvalid.Visibility = Visibility.Hidden;
            txtAmountInvalid.Visibility = Visibility.Hidden;
        }

        // ====================================================================
        // validate inputs, set date and amount
        // ====================================================================
        private bool ValidateInputs(out DateTime date, out double amount)
        {
            int selected_category = cmbCategoryList.SelectedIndex;
            bool allgood = true;

            if (selected_category < 0)
            {
                txtCategoryInvalid.Visibility = Visibility.Visible;
                allgood = false;
            }
            if (!DateTime.TryParse(txtDate.Text, out date))
            {
                txtDateInvalid.Visibility = Visibility.Visible;
                allgood = false;
            }
            if (!Double.TryParse(txtAmount.Text, out amount))
            {
                txtAmountInvalid.Visibility = Visibility.Visible;
                allgood = false;
            }
            return allgood;
        }

        // ====================================================================
        // Add expense to budget
        // ====================================================================
        private void SaveExpense_Click(object sender, RoutedEventArgs e)
        {
            DateTime date;
            Double amount, realAmount;
            String description = txtDescription.Text;

            ClearInvalidMarkers();

            // ----------------------------------------------------------------
            // check if the fields are correct
            // ----------------------------------------------------------------
            if (!ValidateInputs(out date, out amount))
            {
                MessageBox.Show("Cannot save expense, you have invalid data", "Input Errors");
                return;
            }

            // ----------------------------------------------------------------
            // data is valid, proceed
            // ----------------------------------------------------------------

            Category cat = (Category)cmbCategoryList.SelectedItem;

            // if this is income, then reverse the sign of the amount
            // (income is a negative expense)
            realAmount = amount;
            if (cat.Type == Category.CategoryType.Income)
            {
                realAmount = 0 - amount;
            }

            // if bought on credit, modify the description
            if (cbCredit.IsChecked == true)
            {
                description = description + " (on credit)";
            }

            // add expense
            _budget.expenses.Add(date, cat.Id, realAmount, description);

            // if this was charged to the credit card, then add this expense 
            // as well (negative expense)
            if (cbCredit.IsChecked == true)
            {
                _budget.expenses.Add(date, creditCategory.Id, 0 - realAmount, description);
            }

            // set the last action
            String action = "Saved: " + cat.ToString() + ":  $" + amount.ToString("0.00");
            txtLastAction.Text = action + ", " + description;

            // set the text to empty string for all fields,
            // ... but keep the date and the current category
            txtAmount.Text = "";
            txtDescription.Text = "";
            txtAmount.Text = "";
            cbCredit.IsChecked = false;


            return;

        }

        #endregion

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            Expense exp = _budget.expenses.Get(ExpenseID);

            exp.Category = (cmbCategoryList.SelectedItem as Category).Id;
            exp.Date = (DateTime) txtDate.SelectedDate;
            exp.Description = txtDescription.Text;
            double amount;
            double.TryParse(txtAmount.Text, out amount);
            exp.Amount = amount;
            
            _budget.expenses.Modify(exp.Id, exp.Date, exp.Category, exp.Amount, exp.Description);

            this.Close();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Expense exp = _budget.expenses.Get(ExpenseID);
            _budget.expenses.Delete(exp.Id);
            
            this.Close();
        }
    }
}

