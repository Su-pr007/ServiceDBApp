   M S B A M L   `   `   `  ����B  >Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ServiceService  5   	       Service.LoginWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Service     5   	   -   5          � Icon$  ico.ico��5   	    � Title$ 
ServiceApp��5	   	   $	��300��6   $	��350��6)    � 
ResizeMode$ NoResize=�6Q    � WindowStartupLocation$ CenterScreen=�6~   .��5
      �  � RowDefinitions 5   
   �� 5       � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5         ��Name$ 
LoginLabel��5      $��Логин:��6$   $
��Left=�65   $��	10,63,0,0q�6P   $	��Top=�6c   $��38��6{   $��84��6�   $��22(�6�   �� 5   
   -   5         ��Name$ LoginTextBox��5      $
��Left=�6(   $��35��6C   $��	99,63,0,0q�6O   $	��Top=�6b   $	��226��6z   $��22(�6�   6�   �� 5   
   -   5        $ PasswordLabel��5      $��Пароль:��6'   $
��Left=�69   $��
10,104,0,0q�6T   $	��Top=�6h   $��38��6�   $��84��6�   $��22(�6�   G� 5   
   -   5         G�Name$ PasswordBox��5      $
��Left=�6+   $��35��6F   $��
99,104,0,0q�6R   $	��Top=�6f   $	��226��6~   $��22(�6�   �� 5   
   -   5        	 ��Name$	 EnterButton��5      $��
Войти��6(   $
��Left=�68   $��41��6S   $��
99,147,0,0q�6_   $	��Top=�6s   $	��146��6�   $��22(�6�   ���	#FFDDDDDD6�   �� 5   
   -   5        $	 GuestButton��5      $��Я клиент��6&   $��Right=�69   $��34��6U   $��	0,0,10,10q�6a   $��Bottom=�6t   $	��125��6�   $��22(�6�   �� 5   
   $��
ServiceApp��6   $
��Left=�6%   $��
113,19,0,0q�6@   $	��Top=�6T   $��22(�6l   �� 5   
   -   5        $	 ConnSettingWindowButton��5      $-��'Настройка соединения��62   $
��Left=�6Q   $��	10,0,0,10q�6l   $��Bottom=�6   $
��10,1q�6�   6�   5      5      