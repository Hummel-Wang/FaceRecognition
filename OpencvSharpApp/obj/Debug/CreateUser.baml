   M S B A M L   `   `   `  ����I  EOpencvSharpApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:OpencvSharpAppOpencvSharpApp  5   	       OpencvSharpApp.CreateUser   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:OpencvSharpApp     5   	   -   5          � Title$  
CreateUser��5      $	��400��6   $	��550��6+    � 
ResizeMode$ NoResize=�67    � WindowStartupLocation$ CenterScreen=�6M   .��5	      � 
���   � J� Opacity$	 0.8Q�6    � RowDefinitions 5
   
   �� 5      �� 5      �� 5       � ColumnDefinitions 5   
   �� 5      $
��0.3* �6   �� 5      $
��0.7* �6   .+�5   
   �� $��0��6   $��0��6!   $��	姓名：��61   $��Center=�6<   $��Right=�6W   
������$��20(�6�   6�   �� 5   
   $��1��6   $��0��6!   $��	编号：��61   $��Center=�6<   $��Right=�6W   
������$��20(�6�   6�   �� 5   
   -   5        $��tbName��5   b   $��0��6   $��1��6   $��50��6/   $	��200��6;   $
��Left=�6G   $��Center=�6p   $��20(�6�   6�   �� 5   
   -   5        $
��tbNo��5   b   $��1��6   $��1��6   $��50��6/   $	��200��6;   $
��Left=�6G   $��Center=�6n   $��20(�6�   {�.  6�   �� 5   
   -   5        $��btnCreateUser��5   F   $��2��6   $��1��6   $	��200��6.   $��50��6:   $
��Left=�6[   $��保存��6v   $��20(�6�   6�   5      5      