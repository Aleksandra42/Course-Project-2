   M S B A M L   `   `   `  ���� B  >KursApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:KursAppKursApp      KursApp.ReportForAdmin   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:KursApp     -     � Title$  ReportForAdmin�� � 
ResizeMode$ NoResize=� � WindowState$ 	Maximized=�.��� -    �Name$
 grid��.+��� -    ��Name$	 End��$	��End��$��20��$��40��$	��Top=�$��Right=�$��	0,25,25,0q��� 	 �� X1$ 75��	 �� X2$ 75��	 �� Y1$ 50��	 �� Y2$	 400��
���   ��� $ 75��$	 500��$	 400��$	 400��
���   ��� -   	 ��Name$	 Im��$��Impact��$��20,40,1100,590q��� -   $	 Probability��$��Probability��$��430,395,685,230q�� 
���   �
 � Points'
 �   �, ���hx��� �� ��� 
���   �'
 �   ��4�8� e� ����4@�#�� -    ��Name$	 cnv���� $*��$Toп 10 опасных рисков:��$��Bottom=�$��Center=�$��22(�$��	0,0,0,300q��� -    ��Name$ 	Dangerous��$��Center=�$��Bottom=�$	��250��$��0,0,0,50q� �� View �� .P���  �� Width$	 200��.��Name �� CellTemplate �� .R��� $
��Wrap=����� *
RiskName+�� $	 200��.��SoursOfRisk �� .R��� $
��Wrap=����� *SoursOfRisk+�� $	 200��.��Description �� .R��� $
��Wrap=����� *Description+�� $	 200��.��	Effects �� .R��� $
��Wrap=����� *	Effects+�� $	 200��.��Rank �� .R��� $
��Wrap=����� *Rank+