   M S B A M L   `   `   `  ���� B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp      RiskApp.ReportWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:RiskApp     -     � Title$  Report Window�� � 
ResizeMode$ NoResize=� � WindowState$ 	Maximized=����	#FFF4D3D3.��� -    �Name$
 grid��.+��� -    ��Name$ CloseButton��$��Close Report��$��20��$��92��$	��Top=�$��Right=�$��
0,10,9.6,0q� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  �� -   $��Open Document��$��20��$	��Top=�$
��Left=�$��	10,10,0,0q�   �� 	 �� X1$ 75��	 �� X2$ 75��	 �� Y1$ 50��		 �� Y2$		 400��
���   ��� $ 75��$	 500��$	 400��$		 400��
���   ��� -   
 ��Name$
 InfluenceLabel��$��Impact��$��20,40,1100,590q��� -   $
 ProbabilityLabel��$��Probability��$��430,395,685,230q�� 
���   � � Points' �   �, ���hx��� �� ��� 
���   �' �   ��4�8� e� ����4@�#�� -    ��Name$ canvas���� $��The most dangerous risks��$��Bottom=�$��Center=�$��22(�$��	0,0,0,300q��� -    ��Name$ listDangerous��$��Center=�$��Bottom=�$	��250��$��0,0,0,50q� �� View �� .P���  �� Width$	 150��.��Name of Risk �� CellTemplate �� .R��� $
��Wrap=����� *
RiskName+�� $	 150��.��Source �� .R��� $
��Wrap=����� *Source+�� $	 150��.��Possible Solution �� .R��� $
��Wrap=����� *
Solution+�� $	 150��.��	Effects �� .R��� $
��Wrap=����� *	Effects+�� $	 150��.��Rank �� .R��� $
��Wrap=����� *Rank+