   M S B A M L   `   `   `  ����D  @TrackMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TrackMate.PagesTrackMate.Pages  5      %    TrackMate.Pages.TransactionPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      /localclr-namespace:TrackMate.Pages     5        N� Title$  TransactionPage��5	       � 	Resources 5       �� 
TargetType   RoundedButton �� Property �� Value �� 
TargetType �� CornerRadius �� Padding  
IconButton��%o  &	        &	 �    �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��5      #��z��5      $ 10��5      # z��5      ._�5      �� $��Center=�6/   $��Center=�6L   5      5      5      �� 5       � ��$
 10,5q�6(   �� 5       � ��
 �����6+   �� 5       � ��$ SemiBold"�6+   �� 5       �@�Cursor$
 Hand��6'   5       �� 5#   
   " ��6   .�5$      ��  � �� 5%      �� 5&      " ��6&   .R�5'      �� #��z��6!   
������ $ 5��5)      ._�5*      �� $��Center=�6/   $��Center=�6L   5+      5,      5.      �� 5/       � ��$ 5q�6(   �� 50       � ��$ 30��6&   �� 51       � ��$ 30��6'   �� 52       �@�Cursor$
 Hand��6'   �� 53       � �� �#E5E5E56+   54      55      .��57      �  � RowDefinitions 58   
   �� 59      $
��Auto �6   �� 5;      $��* �6   .+�5>   
   �� -   5        #��[ 5>      $
��Left=�5@   	   $	��Top=�5A   	   $��10q�5B   	   $��0��5C   
   .��5D      �� $ ��/Resources/Images/back.png��6   $��25��68   $��25��6C   5E      �� 5G   
   $��1��6   $��Stretch=�6!   $	��Top=�6?   $��10q�6W   ��5H      �� 5I      $��Transaction Details��6   $��16(�5J      $
��Bold"�5K      ���#667ded5L      .��5O      &   System.Windows.Controls.DataGrid  -   5        $��DetailsDataGrid��5O      	   AutoGenerateColumns$	 False��5P      
   CanUserAddRows$
 False��5Q         
IsReadOnly$
 True��5R      $��10q�5S      $��Stretch=�5T      $��Stretch=�5U      $	��400��5V         GridLinesVisibility$	 All=�5W         RowBackground �#f5f5f5   AlternatingRowBackground �#eaeaea 5[      =  7System.Windows.Controls.Primitives.DataGridColumnHeader��%�   '        �� 5\      "  6   .�5]      ��  � �� �#667ded67   �� 5^       � ��$
 Bold"�67   �� 5_       � ��$ 14(�65   �� 5`       � ��$ Center=�6G   5a      5b         Columns 5e      0  *System.Windows.Controls.DataGridTextColumn  5f      ,  &System.Windows.Controls.DataGridColumn   Header$ Product Name��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6a   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6?   �� *MachineryName+6a     5g      $ UserName��6)   $ * 6X    6;   �� *
UserName+6X     5h      $ 
Start Date��6)   $ * 6[    6=   �� *	StartDate+6[     5i      $ End Date��6)   $ * 6W    6;   �� *	EndDate+6W   5k      5l      5m      5n      