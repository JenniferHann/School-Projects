   M S B A M L   `   `   `  ����A  =Budget, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:BudgetBudget  5   	   clr-namespace:BudgetBudget  5   	       Budget.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Budget     5   	   %selfclr-namespace:Budget     5   	   -   5          � Title$  Home Budget��5	   	    J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command$ self:CustomCommands.Exit��5      6`     5   
   -   5        $  self:CustomCommands.Modify��5      6d     5   
   -   5        $ self:CustomCommands.New��5      6^     5   
   -   5        $ self:CustomCommands.Open��5      6`     5   
   -   5        $ self:CustomCommands.Save��5      6`    � 	Resources 5       	  Image �� 
TargetType �� Triggers P� Property P� Value �� Property	 �� Value��%�   &	        �� 5   
   " ��6    5      P� 5       � {� .  . �5      ��  �@J�Opacity$		 0.5Q�60   5      5      5      .��5      ]� .+�5   
   o� $	��Top=�6   .@�5       m� $��_File��6   .@�5!      m� -   5        $��menuNew��5!   )   $
��_New��6   
 m� Command$
 self:CustomCommands.New��69    m� Icon 5#      �� 5$      $��.\Images\NewFile_16x.png��6    5&      m� 5'      -   5        $��menuOpen��5'   *   $��_Open��6   $
 self:CustomCommands.Open��6;    5)      �� 5*      $*��$.\Images\OpenfileDialog_grey_16x.png��6    5,      m� 5-      -	   5        $��menuSave��5-   *   $��_Save��6   $
 self:CustomCommands.Save��6:    5/      �� 50      $��.\Images\Save_16x.png��6    52      m� 53      -
   5        $��
menuSaveAs��53   ,   $��_SaveAs��6    54      �� 55      $%��.\Images\SaveStatusBar1_16x.png��6    57      �� 59      m� 5:      -   5        $��menuExit��5:   *   $��_Exit��6   $
 self:CustomCommands.Exit��6;    5;      �� 5<      $��.\Images\Exit_16x.png��6    5>      5?      m� 5@      $
��Edit��6   .@�5A      m� -   5        $��modifySelectedItem��5A   9   $��Modify Selected Item��6   $ 
 self:CustomCommands.Modify��6S    5B      �� 5C      $#��.\Images\EditTableRow_16x.png��6    #���   6G   5E      5F      5G      `� 5J   
   $	��Top=�6   .�5K      c� .@�5L      ��  � ToolTip$ Create a new expense form��6   $��self:CustomCommands.New��6?   .��5N      �� $��.\Images\NewFile_16x.png��6   5O      �� 5P      $# Open an existing expense form��6   $��self:CustomCommands.Open��6B   .��5R      �� $*��$.\Images\OpenfileDialog_grey_16x.png��6   5S      �� 5T      -   5        $��toolBarSave��5T      $ Save the expense form��6-   $��self:CustomCommands.Save��6M   .��5V      �� $��.\Images\Save_16x.png��6   #���   6;   5W      �� 5X      -   5        $��toolBarSaveAs��5X   2   $2 ,Save the expense form to a specific location��6G   .��5Y      �� $%��.\Images\SaveStatusBar1_16x.png��6   #���   6E   5Z      �� 5[      -   5        $��toolBarModify��5[   >   $ ��self:CustomCommands.Modify��6   $' !Modify selection in the data grid��6S   .��5\      �� $#��.\Images\EditTableRow_16x.png��6   #���   6C   5]      �� 5^      $��self:CustomCommands.Exit��6   $) #Exit the Home Budget Expense Window��6<   .��5_      �� $��.\Images\Exit_16x.png��6   5`      5a      5b      �� 5e   
   $��Bottom=�6   
�������$��1q�6?   .@�5f      �� $��Right=�6   .��5g      �� -   5        $��txtSavedStatus��5g      $��0,0,10,0q�62    �� TextAlignment$ Right=�6D   5h      �� 5i      .��5j      �� $
��Left=�6    �� Orientation$ 
Horizontal=�63   $
��Left=�6L   $��Center=�6g   .+�5k      �� Budget File:6.   �� 5l      -   5        $��txbFileName��5l       $��10,0,0,0q�63   6G   5m      5n      5o      � 5q   
    � ColumnDefinitions 5t      �� 5u      $	��240 �6#   �� 5v      .+�5z      �� -   5        $��DisplayOptions��5z      $��10q�6/   .+�5|      �� $
��0,10q�6   $��14(�6*   Display Options6I   �� 5      $��Filter��6   .��5�      �� .+�5�      �� $��0,5,0,0q�6$   
Start Date6B   (  "System.Windows.Controls.DatePicker  5�      -   5        $��dpStartDate��5�   l   $��0,0,0,5q�6[   6�   �� 5�      $��0,5,0,0q�6$   
End Date6@     5�      -   5        $��	dpEndDate��5�   j   $��0,0,0,5q�6Y   6}   �� 5�      -   5        $��cbFilterCategories��5�   U   $��0,5,0,0q�6#   .��6o   Filter By Category?6�   �� 5�      -   5        $��cmbCategories��5�   f   $��0,0,0,5q�6U   6}   5�      5�      �� 5�      $	��0,5q�6   $��Summary��6(   .��5�      �� .+�5�      �� -   5        $��	cbByMonth��5�   L   $��0,5,0,0q�6;   .��6]   
By Month6g   �� 5�      -   5        $��cbByCategory��5�   O   $��0,5,0,0q�6>   .��6c   By Category6p   5�      5�      �� 5�      -   5        $��btnAddExpense��5�   5   $
��0,25q�6J   .��6X   Add Expense6e   5�      �� 5�      $��1��6   $��1��6%   $��2q�62   
���   �$��2q�6Q   ._�5�      &   System.Windows.Controls.DataGrid  -   5        $��
dataBudget��5�   2      FrozenColumnCount$ 1��6      AutoGenerateColumns .     
IsReadOnly .  � ContextMenu 5�      �� 5�      .@�5�      m� -   5        $��Delete��5�   '   67   m� 5�      $��Modify��6'   $ 
 self:CustomCommands.Modify��67   �� 5�      m� 5�      $��Cancel��6'   5�      5�      5�      5�      5�      5�      