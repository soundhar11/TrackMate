   M S B A M L   `   `   `  ����D  @TrackMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TrackMate.PagesTrackMate.Pages  5      (    "TrackMate.Pages.ProductHistoryPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:TrackMate.Pages     5        N� Title$  ProductHistoryPage��5	       � 	Resources 5       �� 
TargetType   RoundedButton �� Property �� Value �� 
TargetType �� CornerRadius �� Padding  
IconButton��%o  &	        &	 �    �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5      #��z��5      $ 10��5      # z��5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      �� 5       � ��$
 10,5q�6(   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       �@�Cursor$
 Hand��6'   5       �� 5#   
   " ��6   .�5$      ��  � �� 5%      �� 5&      " ��6&   .R�5'      �� #��z��6!   
������ $ 5��5)      ._�5*      �� $��Center=�6/   $��Center=�6L   5+      5,      5.      �� 5/       � ��$ 5q�6(   �� 50       � ��$ 30��6&   �� 51       � ��$ 30��6'   �� 52       �@�Cursor$
 Hand��6'   �� 53       � �� �#E5E5E56+   54      55      .��58      �  � RowDefinitions 5:   
   �� 5;      $
��Auto �6   �� 5=      $��* �6   .+�5@   
   �� -   5        #��[ 5@      $
��Left=�5B   	   $	��Top=�5C   	   $��10q�5D   	   $��0��5E   
   .��5F      �� $ ��/Resources/Images/back.png��6   $��25��60   $��25��6;   5G      �� 5I   
   $��1��6   $��Stretch=�6!   $	��Top=�6?   $��10q�6W   ��5J      �� 5K      $��Product History��6   $��16(�5L      $
��Bold"�5M      ���#667ded5N      .��5Q      &   System.Windows.Controls.DataGrid  -   5        $��DetailsDataGrid��5Q      	   AutoGenerateColumns$	 False��5R      
   CanUserAddRows$
 False��5S         
IsReadOnly$
 True��5T      $��10q�5U      $��Stretch=�5V      $��Stretch=�5W      $	��400��5X         GridLinesVisibility$	 All=�5Y         RowBackground �#f5f5f5   AlternatingRowBackground �#eaeaea 5]      =  7System.Windows.Controls.Primitives.DataGridColumnHeader��%�   '        �� 5^      "  6   .�5_      ��  � �� �#667ded67   �� 5`       � ��$
 Bold"�67   �� 5a       � ��$ 14(�65   �� 5b       � ��$ Center=�6G   5c      5d         Columns 5g      0  *System.Windows.Controls.DataGridTextColumn  5h      ,  &System.Windows.Controls.DataGridColumn   Header$ Product Old Name��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 6_   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6C   �� *	OldName+6_     5i      $ Product New Name��6)   $ 2* 6_    6C   �� *	NewName+6_     5j      $ Product Old Branch��6)   $ 2* 6c    6E   �� *	OldBranch+6c     5k      $ Product New Branch��6)   $ 2* 6c    6E   �� *	NewBranch+6c     5l      $ Date and Time��6)   $ 3* 6Y    6@   �� *Date+6Y   5n      5o      5p      5q      