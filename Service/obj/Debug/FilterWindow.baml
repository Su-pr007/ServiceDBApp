   M S B A M L   `   `   `  ����B  >Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ServiceService  5   	       Service.FilterWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Service     5   	   -   5          �Name$  FilterWindow1��5   	    � Title$ Фильтр��5   	   $	��350��6   $	��350��6%   $	��300��61   $	��300��6@    � WindowStyle$ 
ToolWindow=�6P   .��5	      � .+�5
   
   �� $	��Top=�6   $��10,10,130,0q�6,   $��35��6A   �� 6N   $=�7Укажите данные для фильтрации��6R   �� 6y   $�.��6}   �� 6�   �� 6�   �� 6�   $D�>Оставьте поле пустым для пропуска��6�   6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ FilterDataGrid��5      $��10,50,10,45q�6+   $��2��6@      CanUserSortColumns .     CanUserReorderColumns .     SelectionUnit$
 Cell=�6�      CanUserAddRows .  6�   �� 5   
   -   5         ��Name$ SearchButton��5      $��
Найти��6'   $��Right=�67   $��25��6S   $��	0,0,10,10q�6_   $��Bottom=�6r   $��76��6�   6�   �� 5   
   -   5        $ CancelButton��5      $��Отмена��6'   $
��Left=�68   $��25��6S   $��	10,0,0,10q�6_   $��Bottom=�6r   $��77��6�   6�   �� 5   
   -   5        	 ��Name$	 AndCheckBox��5      $��Фильтрация "И"��6(   $
��Left=�6A   $��
217,11,0,0q�6\   $	��Top=�6p   5      5      