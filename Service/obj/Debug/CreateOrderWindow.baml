   M S B A M L   `   `   `  ����B  >Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ServiceService  5   	       Service.CreateOrderWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Service     5   	   -   5          �Name$  CreateOrderWindow1��5   	    � Title$# Создание заказа��5   	   $	��300��6!   $	��380��6.   $	��356��6:   $	��264��6I   $��14(�6Y    � Icon$ ico.ico��6g    � WindowStartupLocation$ CenterScreen=�6v   .��5	      � .+�5   
   �� $#��Ф.И.О. заказчика:��6   $
��Left=�6,   $��	10,30,0,0q�6G   $	��Top=�6Z   �� 5   
   -   5         ��Name$ OrdererNameTB��5      $��Right=�6)   $��	0,34,10,0q�6E    �� TextWrapping$
 Wrap=�6X   $	��Top=�6l   $	��150��6�   $��21��6�   �� 5   
   $"��Тип устройства:��6   $
��Left=�6*   $��	10,64,0,0q�6E   $	��Top=�6X   �� 5   
   -   5        $ ModelTypeTB��5      $��Right=�6'   $��	0,68,10,0q�6C   $
 Wrap=�6V   $	��Top=�6j   $	��150��6�   $��21��6�   �� 5   
   $(��"Модель устройства:��6   $
��Left=�6-   $��	10,98,0,0q�6H   $	��Top=�6[   �� 5   
   -   5        $ ModelNameTB��5      $��Right=�6'   $��
0,102,10,0q�6C   $
 Wrap=�6W   $	��Top=�6k   $	��150��6�   $��21��6�   �� 5   
   $2��,Описание неисправности:��6   $
��Left=�62   $��
10,133,0,0q�6M   $	��Top=�6a   �� 5   
   -   5        $ FaultDescTB��5      $��Right=�6'   $��
0,136,10,0q�6C   $
 Wrap=�6W   $	��Top=�6k   $	��150��6�   $��21��6�   �� 5   
   $"��Серийный номер:��6   $
��Left=�6*   $��
10,166,0,0q�6E   $	��Top=�6Y   �� 5   
   -   5        $ SerialNumberTB��5      $��Right=�6*   $��
0,170,10,0q�6F   $
 Wrap=�6Z   $	��Top=�6n   $	��150��6�   $��21��6�   �� 5   
   -   5         ��Name$ SaveOrderButton��5      $��Сохранить��6*   $��Right=�6>   $��	0,0,10,10q�6Z   $��Bottom=�6m   $��75��6�   $��1��6�   $��23��6�   6�   �� 5   
   -   5        $ CancelOrderButton��5      $��Отмена��6,   $
��Left=�6=   $��	10,0,0,10q�6X   $��Bottom=�6k   $��75��6�   $��23��6�   6�   5      5      