   M S B A M L   `   `   `  ���� D  @TrackMate, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:TrackMate.PagesTrackMate.Pages  (    "TrackMate.Pages.ProductHistoryPage   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     /localclr-namespace:TrackMate.Pages       N� Title$  ProductHistoryPage�� � 	Resources  �� 
TargetType   RoundedButton �� Property �� Value �� 
TargetType �� CornerRadius �� Padding  
IconButton��%�  &	        &	 �     �� " ��.���  � �� �� " ��.R��� #��z��#��z��#��z��$ 10��# z��._��� $��Center=�$��Center=���  � ��$
 10,5q���  � ��
 �������  � ��$ SemiBold"���  �@�Cursor$
 Hand���� " ��.���  � �� �� " ��.R��� #��z��
������ $ 5��._��� $��Center=�$��Center=���  � ��$ 5q���  � ��$ 30����  � ��$ 30����  �@�Cursor$
 Hand����  � �� �#E5E5E5.���  � RowDefinitions �� $
��Auto ��� $��* �.+��� -   #��[ $
��Left=�$	��Top=�$��10q�$��0��.���� $ ��/Resources/Images/back.png��$��25��$��25���� $��1��$��Stretch=�$	��Top=�$��10q����� $��Product History��$��16(�$
��Bold"����#667ded.��&   System.Windows.Controls.DataGrid  -   $��DetailsDataGrid��	   AutoGenerateColumns$	 False��
   CanUserAddRows$
 False��   
IsReadOnly$
 True��$��10q�$��Stretch=�$��Stretch=�$	��400��   GridLinesVisibility$	 All=�   RowBackground �#f5f5f5   AlternatingRowBackground �#eaeaea =  7System.Windows.Controls.Primitives.DataGridColumnHeader��%{   '        �� "  .���  � �� �#667ded��  � ��$
 Bold"���  � ��$ 14(���  � ��$ Center=�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Product Old Name��   Width5  /System.Windows.Controls.DataGridLengthConverter$ 2* 1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *	OldName+  $ Product New Name��$ 2*  �� *	NewName+  $ Product Old Branch��$ 2*  �� *	OldBranch+  $ Product New Branch��$ 2*  �� *	NewBranch+  $ Date and Time��$ 3*  �� *Date+