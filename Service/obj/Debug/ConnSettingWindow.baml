   M S B A M L   `   `   `  ����B  >Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ServiceService  5   	       Service.ConnSettingWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Service     5   	   -   5          �Name$  ConnectionSettingsWindow��5   	    � Title$- 'Настройки соединения��5   	   $	��450��6&   $	��400��63   $	��300��6V   $	��300��6e    � Icon$ ico.ico��6u    � WindowStartupLocation$ CenterScreen=�6�    � WindowStyle$ 
ToolWindow=�6�   .��5	      � .+�5
   
   �� $3��-MySQL настройки соединения��6   $��Center=�65   $��0,25,0,0q�6R   $	��Top=�6d   $��18(�6|   �� 5   
   $��IP сервера��6   $��Center=�6%   $��	0,100,0,0q�6B   $	��Top=�6U   $��18(�6m   �� 5   
   -   5         ��Name$ DBIpTextBox��5      $��Center=�6E   $��30��6b   $��	0,140,0,0q�6n    �� TextWrapping$
 Wrap=�6�   $��	127.0.0.1��6�   $	��Top=�6�   $��18(�6�   $	��200��6�   �� 5   
   $��Порт сервера��6   $��Center=�6'   $��	0,180,0,0q�6D   $	��Top=�6W   $��18(�6o   �� 5   
   -   5        $ DBPortTextBox��5      $��Center=�6I   $��30��6f   $��	0,220,0,0q�6r   $
 Wrap=�6�   $
��3307��6�   $	��Top=�6�   $��18(�6�   $	��200��6�   �� 5   
   $,��&Название базы данных��6   $��Center=�6/   $��	0,260,0,0q�6L   $	��Top=�6_   $��18(�6w   �� 5   
   -   5        $ DBNameTextBox��5      $��Center=�6I   $��30��6f   $��	0,300,0,0q�6r   $
 Wrap=�6�   $��service��6�   $	��Top=�6�   $��18(�6�   $	��200��6�   �� 5   
   -   5         ��Name$ CancelButton��5      $��Отмена��6'   $
��Left=�68   $��	30,0,0,30q�6S   $��Bottom=�6f   $
��10,1q�6�   $��18(�6�   6�   �� 5   
   -   5        $ 
SaveButton��5      $��Принять��6%   $��Right=�67   $��	0,0,30,30q�6S   $��Bottom=�6f   $
��10,1q�6�   $��18(�6�   6�   �� 5   
   -   5        $��По умолчанию��5      $��Right=�6(   $��	0,10,10,0q�6D   $	��Top=�6W   $
��10,1q�6o   6~   5      5      