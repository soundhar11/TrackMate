   M S B A M L   `   `   `  ����D  @TrackMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TrackMate.PagesTrackMate.Pages  5      )    #TrackMate.Pages.ChangeOwnershipPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:TrackMate.Pages     5        N� Title$  ChangeOwnershipPage��5	       � 	Resources 5       �� 
TargetType   RoundedButton �� Property �� Value �� 
TargetType �� CornerRadius �� Padding  
IconButton��%o  &	        &	 �    �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5      #��z��5      $ 10��5      # z��5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      �� 5       � ��$
 10,5q�6(   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       �@�Cursor$
 Hand��6'   5      �� 5"   
   " ��6   .�5#      ��  � �� 5$      �� 5%      " ��6&   .R�5&      �� #��z��6!   
������ $ 5��5(      ._�5)      �� $��Center=�6/   $��Center=�6L   5*      5+      5-      �� 5.       � ��$ 5q�6(   �� 5/       � ��$ 30��6&   �� 50       � ��$ 30��6'   �� 51       �@�Cursor$
 Hand��6'   �� 52       � �� �#E5E5E56+   53      54      .��56      � ���#F7F7F7$��10q�6     � RowDefinitions 57   
   �� 58      $
��Auto �6   �� 59      $��* �6   .+�5;   
   �� -   5        #��[ 5;      $
��Left=�5=      $	��Top=�5>      $��10q�5?      .��5@      �� $ ��/Resources/Images/back.png��6   $��25��68   $��25��6C   5A      �� 5D   
   $��1��6   $ 10��6   ���#DDD$��1q�6C   $ 20q�6W   
�������$��20q�6w   ._�5E      �� $��Center=�6   $��Center=�66   $	��400��6Q   .+�5F      �� $��Product:��6   $
��Bold"�6,   $��14(�6>   $��	50,10,0,5q�6L   ���#667ded6`   �� 5G      -   5        $��ProductNameTextBlock��5G      $��14(�68   $��	50,0,0,20q�6F   �� 5I      	 �� Orientation$	 Vertical=�6   $
��0,10q�64   .+�5J      �� $��User:��6    $
��Bold"�6-   ���#667ded$��
50,25,10,0q�6T   �� 5K      -   5        $��UsernameComboBox��5K      
 �� 
IsEditable$

 True��5L       �� IsDropDownOpen$
 True��5N      $	��300��5O      $��30��5P      $��5q�5Q      ���#F9F9F9$��10q�5S      �� 5U      $
��Date��6    $
��Bold"�6,   ���#667ded$��
50,25,10,0q�6S   (  "System.Windows.Controls.DatePicker  5V      -   5        $��
DatePicker��5V   !   $��3��63   $��1��6@   $��5q�6P   $	��300��6[   $��30��6g   6s   5W      �� 5Z      -   5        $��Update User��5Z      $	��150��5[      $��40��6%   #��[  5\      ���#28A745$
��0,10q�5_      5`      5a      5b      5c      