   M S B A M L   `   `   `  ����J  FMileStoneClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MileStoneClientMileStoneClient  5   	   6    0MileStoneClient.PresentationLayer.RegisterWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:MileStoneClient     5   	   -   5          �Name$  Register��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ RegisterWindow��5	   	   $	��600��6    $	��450��6-   .��5
      � -   5         �Name$ Grid1Reg��5
      $	��450��6   $��	0,0,-8,-1q�6)   ��5   
   �� 5       �� ImageSource$3 -/MileStoneClient;component/media/register.jpg��6    � ColumnDefinitions 5   
   �� 5      .+�5   
   �� -   5        $
��Left=�5      $��35��6-    �� TextWrapping$
 Wrap=�69   $	��Top=�6M   $	��100��6e   $��198,299,0,0q�6q   $��24(�6�   ��6�   ��  �� Path$ GroupIdContent� �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   -   5        $
��Left=�5      $��35��6-   $
 Wrap=�69   $	��Top=�6M   $	��200��6e   $��217,352,0,0q�6q   $��24(�6�   ��6�   �� $ NicknameContent�$ PropertyChanged=��� 5   
   -   5        $��Register��5      $
��Left=�6$   $	��Top=�6?   $	��120��6W   $��
94,444,0,0q�6c   $��35��6w   $��24(�6�   $
��Bold"�6�   {�6�   �� *BtnRegIsEnabled+	 �� Mode$	 OneWay=��� 5   
   -   5        $
��Back��5      $
��Left=�6    $	��Top=�6;   $	��120��6S   $��234,444,0,0q�6_   $��35��6t   $��24(�6�   $
��Bold"�6�   6�   �� 5   
   $
��Left=�6   $	��Top=�6+   $��
12,402,0,0q�6C   $	��420��6W   $��30��6c   $
��Bold"�6o   
���  ��$��14(�6�   $��MV Boli)�6�   
 J� RenderTransformOrigin$
 0.5,0.5&�6�   ��6�   Z� y�6�   �� *LblRegErrorVisibility+$	 OneWay=���6%  �� *LblRegErrorContent+$	 OneWay=�z�5      V� 5      .~�5      �� �� 5      �� 5       �� Angle$ -0.048Q�6&   T� 5      5      5      �� 5   
   $
��Left=�6   $��
10,511,0,0q�6+   $	��Top=�6?   $	��277��6W   ���	#FF00B7E0$��20(�6z   $��MV Boli)�6�   $��	ExtraBold"�6�   $��35��6�   y�6�   �� *LblAddLoginVisibility+$	 OneWay=���6�   �� *LblAddLoginContent+$	 OneWay=��� 5    
   -   5        $��Login��5       $
��Left=�6!   $	��Top=�6<   $	��120��6T   $��310,511,0,0q�6`   $��35��6u   $��24(�6�   $
��Bold"�6�   y�6�   �� *BtnLoginVisibility+$	 OneWay=�5!      5"      