   M S B A M L   `   `   `  ����B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp  5   	       RiskApp.AdminNewProject   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:RiskApp     5   	   -   5          � Title$  Create new project��5   	   $��297.38��6$   $��693.61��64    � 
ResizeMode$ NoResize=�6`   ���	#FFD7EBF6.��5	      � $��0,0,0,-3q�6   $
��Left=�6   $	��345��68   .+�5
   
   �� -   5         ��Name$ 
BackButton��5
      $��40��6&   $��40��62   $	��Top=�6W   $
��Left=�6o   $��	10,10,0,0q�6�    J� RenderTransformOrigin$ 
0.54,1.047&�6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      5      �� 5   
   -   5         ��Name$ ProjectNameLabel��5      $��Project Name:��6*   $
��Left=�6B   $	��Top=�6]   $��	23,53,0,0q�6u   $ 
0.47,2.061&�6�   �� 5   
   -   5         ��Name$
 Name��5      $��113,53,0,180q�6    $
��Left=�66   $	��212��6Q   �� 5   
   -   5        $��TypeCombobox��5       �� Text$ sdgsdgs��6'   $��27,118,0,117q�66   $
��Left=�6L   $	��308��6g   .@�5      �� Web Application6*   �� 5      Mobile Application6-   �� 5      Desktop Application6.   5      �� 5   
   -   5        $ 
LabelOwner��5      $!��Choose the project's owner:��6$   $��371,13,-194,211q�6J   $
��Left=�6c   $	��168��6~   �� 5   
   -   5         ��Name$ 
listOwners��5      $��371,53,-330,0q�6&   $
��Left=�6=   $	��304��6X   $	��188��6d   $	��Top=�6q   �� 5   
   -   5        $ CreateButton��5      $��Create Project��6'   $��27,195,13,44q�6@    5        5      5      �� 5   
   $"��Choose the type of the App: ��6   $
��Left=�67   $��	23,92,0,0q�6R   $	��Top=�6e   5      5      