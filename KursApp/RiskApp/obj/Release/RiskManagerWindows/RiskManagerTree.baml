   M S B A M L   `   `   `  ���� B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp      RiskApp.RiskManagerTree   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:RiskApp     -     � Title$  Risk Manager's Risk Tree�� � 
ResizeMode$ NoResize=� � WindowState$ 	Maximized=����	#FFF4E4C0.��� -    �Name$
 grid��$��0,-8,-0.4,0.6q�.+��� -    ��Name$ 
BackButton��$��40��$��40��$	��Top=�$
��Left=�$��	23,26,0,0q� J� RenderTransformOrigin$ 	0.25,0.25&� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  � -   $ canvas��$��	0,0,0,200q��� -    ��Name$ RiskName��$��30,180,800,200q�$
��Wrap=��� $��	25,0,0,25q�$	��150��$��Bottom=�$
��Left=�$	��290��.��� $	��145��$��0,0,0,0q�.+��� $��Description:��$��10,70,179.6,45q��� -    ��Name$ TextboxProbability��$
��Left=�$��
86,0,0,115q�$	��150���� -   $ TextboxCost��$
��Left=�$��
86,35,0,80q�$	��150���� -   $ TextboxDescription��$
��Left=�$��
86,70,0,45q�$	��150��$ 1.672,-3.406&��� $��Cost:��$��10,35,197.6,80q��� $��Probability:��$��10,0,184.6,115q��� -	   $ 	AddButton��$��Add new vertex��$��10,108,53.6,7q����	#FFE1F37C   �� $	��150��$��Bottom=�$��Right=�$	��290��$��25,25q�.��� $	��145��$��0,0,0,0q�.+��� -
   $ DeleteButton��$��Delete selected vertex��$��10,108,53.6,7q����	#FFE1F37C   �� -   $ CreateReport��$��Create Report��$	��Top=�$��Right=�$��	0,25,25,0q�$	��100��$��30��   