   M S B A M L   `   `   `  ����B  >WpfTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfTestWpfTest  5   	       WpfTest.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   'localclr-namespace:WpfTest     5   	     � Title$  WPF Test��5   	   $	��350��6   $	��525��6'   .��5      � -   5         �Name$ RootLay��5       � ShowGridLines .   � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5	      $
��auto �6   �� 5
      -   5         ��Name$ ButtonColumn��5
      $	��100 �65   .+�5   
   �� $��0��6   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ 	GoodsGrid��5         AutoGenerateColumns .     Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6B   	   Width5  /System.Windows.Controls.DataGridLengthConverter$		 120 6P   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6)   �� *Name+6P     5      $ Quantity��6F   $	 60 6X   
 6)   �� *
Quantity+6X     5      $ Price��6C   $	 80 6R   
 6)   �� *Price+6R     5      $ Category��6F   $	 80 6X   
 6)   �� *
Category+6X   4  .System.Windows.Controls.DataGridCheckBoxColumn  5      $
 Mark��6F   $	 40 6T   
 6-   �� *Mark+6T   5      5      �� 5   
   $��1��6    �� ShowsPreview .  $	��5px��6<   $��Center=�6H   $��Stretch=�6e   �� 5   
   $��2��6   .+�5      �� -   5         ��Name$ 	AddButton��5      $	��ADD��6(    � ToolTip$ Add new product��6N   �� 5      -   5        $ UpdateButton��5      $��UPDATE��6+   $ Update table��6W   �� 5      -   5        $ IndexButton��5      $��INDEX��6*   $ Indexate all products��6T   �� 5       -   5        $ DeleteButton��5       $��DELETE��6+   $" Delete product from database��6W   5!      5$      5'      