   M S B A M L   `   `   `  ����D  @ModokiLab, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:ModokiLab.ViewsModokiLab.Views  5   	   ;"clr-namespace:ModokiLab.ViewModelsModokiLab.ViewModels  5   	   "    ModokiLab.Views.ConfigWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bei9http://schemas.microsoft.com/expression/2010/interactions  5   	   L HLivet, Version=1.3.0.0, Culture=neutral, PublicKeyToken=b0b1d3f711ef38cb.l&http://schemas.livet-mvvm.net/2011/wpf  5   	   %vclr-namespace:ModokiLab.Views  5   	   +vm"clr-namespace:ModokiLab.ViewModels  5   	     � WindowStartupLocation$  CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�6.    � Title$ ConfigWindow��5
   	   $	��350��6   $	��525��6+    � DataContext 5      *  $ModokiLab.ViewModels.ConfigViewModel  5   
   .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ ContentRendered��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      +  %Livet.Behaviors.LivetCallMethodAction     
MethodName$ 
Initialize��6?      MethodTarget 6&   �� 6?   5      9  3Livet.Behaviors.Messaging.InteractionMessageTrigger  5   
   	   
MessageKey$	 Close��6&   
   	Messenger
 69   �� *	Messenger+. 5      F  @Livet.Behaviors.Messaging.Windows.WindowInteractionMessageAction  5        5"   
   $ Closed��6   . 5#      .  (Livet.Behaviors.DataContextDisposeAction  5$      .��5*      � .+�5+   
   �� $��Center=�6   $��Center=�62   .+�5,      �� $��	PIN INPUT��6   $
��Wrap=�6)   $��24(�6=   $��Center=�6K   $��Center=�6h   �� 5-      $
��0,20q�6   $��24(�6a   $��50��6o   $��Center=�5.      $��Center=�6:   ��5-   $   �� *	PinCode+ �� UpdateSourceTrigger$ PropertyChanged=�5.   :   �� 5/      $
��SEND��6   $	��200��6$   $��18(�60   $��50��6>   ��50      �� *InputCommand+51      52      53      