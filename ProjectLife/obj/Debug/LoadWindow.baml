   M S B A M L   `   `   `  ����F  BProjectLife, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ProjectLifeProjectLife  5   	       ProjectLife.LoadWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ProjectLife     5   	   -   5          � Title$  
LoadWindow��5   	   $	��300��6   $	��300��6)   .��5	      � .+�5
   
   � $��Stretch=�6   $��Stretch=�6-   �� 5   
   -   5         ��Name$ load_button��5      $
��Load��6&   $��Stretch=�65   $��5,5,5,5q�6S   $��1��6d   $��Stretch=�6q   6�   �� 5   
   -   5        $ delete_button��5      $��Delete��6(   $��Stretch=�69   $��5,5,5,5q�6W   $��1��6h   $��1��6u   $��Stretch=�6�   6�   �� 5   
   -   5        $ cancel_button��5      $��Cancel��6(   $��Stretch=�69   $��5,5,5,5q�6W   $��1��6h   $��2��6u   $��Stretch=�6�   6�   �� 5   
   $��3��6   $��Stretch=�6)   $��5,5,5,5q�6G   $��Stretch=�6X   .+�5      &   System.Windows.Controls.DataGrid  -   5        $��loading_data��5      $��Stretch=�6+   $��Stretch=�6I      AutoGenerateColumns .    CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     
IsReadOnly . 6�   5       � ColumnDefinitions 5   
   �� 5      $	��50* �6   �� 5      $	��50* �6   �� 5      $	��50* �6    � RowDefinitions 5   
   �� 5      $
��200* �6   �� 5      $��35 �6   5      5      