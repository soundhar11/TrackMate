   M S B A M L   `   `   `  ����D  @TrackMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TrackMate.PagesTrackMate.Pages  5      !    TrackMate.Pages.AddUserPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:TrackMate.Pages     5        N� Title$  AddUserPage��5	       � 	Resources 5       �� 
TargetType   RoundedButton �� Property �� Value �� 
TargetType �� CornerRadius �� Padding  
IconButton��%o  &	        &	 �    �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5      #��z��5      $ 10��5      # z��5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      �� 5       � ��$
 10,5q�6(   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       �@�Cursor$
 Hand��6'   5      �� 5"   
   " ��6   .�5#      ��  � �� 5$      �� 5%      " ��6&   .R�5&      �� #��z��6!   
������ $ 5��5(      ._�5)      �� $��Center=�6/   $��Center=�6L   5*      5+      5-      �� 5.       � ��$ 5q�6(   �� 5/       � ��$ 30��6&   �� 50       � ��$ 30��6'   �� 51       �@�Cursor$
 Hand��6'   �� 52       � �� �#E5E5E56+   53      54      .��56      � ���#F7F7F7$��10q�6     � RowDefinitions 57   
   �� 58      $
��Auto �6   �� 59      $��* �6   .+�5=   
   �� -   5        #��[ 5=      $
��Left=�5?      $	��Top=�5@      $��10q�5A      .��5B      �� $ ��/Resources/Images/back.png��6   $��25��68   $��25��6C   5C      �� 5F   
   $��1��6   $ 10��6   ���#DDD$��1q�6C   $ 20q�6W   
�������$��20q�6w   ._�5G      �� 	 �� Orientation$	 Vertical=�6   $��Center=�60   $��Center=�6M   .+�5H      �� $��	UserName:��6   $��Center=�6-   $
��Bold"�6J   ���#444$��
0,0,250,10q�6n   �� 5I      -   5        
 ��Name$
 UsernameTextBox��5I      $	��300��63   $��30��6?   $��5q�6K   ���#F9F9F9$��0,0,0,20q�6l   $��Center=�6~   �� 5L      -   5        $��	Add Users��5L      $	��150��5M      $��40��6%   #��[  5N      ���#28A745$��Center=�5P      $
��0,10q�5Q      5R      5S      5T      5U      5V      