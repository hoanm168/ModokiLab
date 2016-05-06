using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using ModokiLab.Models;
using ModokiLab.Properties;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using ModokiLab.Views;

namespace ModokiLab.ViewModels
{
    sealed class TimeLineViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */
        readonly Authorizer authorizer = new Authorizer(Resources.ConsumerKey, Resources.ConsumerSecret);

        Twitter twitter;

        public TimeLineViewModel()
        {
            _TweetCommand = new ViewModelCommand(Tweet, () => !String.IsNullOrEmpty(Text));
        }

        public async void Initialize()
        { 
            twitter = Authorizer.IsAuthorized ? authorizer.Load() : await Authorize();
            twitter.ReadTimeLine().ObserveOn(SynchronizationContext.Current).Subscribe(UpdateTweet);
            User = await twitter.MyUser();
        }

        async void Tweet()
        {
            var id = await twitter.Tweet(Text);
            if (!String.IsNullOrEmpty(id))
            {
                Tweets.Insert(0, new TweetContent(id, Text, User));
                User = new User(User.Name, User.Image, User.TweetCount + 1);
                Status = "ツイートを送信しました";
            }
            else
            {
                Status = "ツイートに失敗しました";
            }
            Text = "";
        }
        void UpdateTweet(IEnumerable<TweetContent> tweets)
        {
            foreach (var tweet in tweets.Reverse())
            {
                if (!Tweets.Any(x => x.Id == tweet.Id))
                {
                    Tweets.Insert(0, tweet);
                }
            }
            Status = "ツイートを取得しました";
        }

        async Task<Twitter> Authorize()
        {
            Process.Start(await authorizer.AuthorizeUri());
            var config = new ConfigViewModel();
            var message = new TransitionMessage(typeof(ConfigWindow), config, TransitionMode.Modal);
            await Messenger.RaiseAsync(message);
            return await authorizer.Authorize(config.PinCode);
        }


        #region TweetCommand変更通知プロパティ
        private ViewModelCommand _TweetCommand;

        public ViewModelCommand TweetCommand
        {
            get
            {
                return _TweetCommand;
            }
        }
        #endregion
        #region Text変更通知プロパティ
        private string _Text;

        public string Text
        {
            get
            { return _Text; }
            set
            {
                if (_Text == value)
                    return;
                _Text = value;
                RaisePropertyChanged();
                TweetCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Status変更通知プロパティ
        private string _Status;

        public string Status
        {
            get
            { return _Status; }
            set
            {
                if (_Status == value)
                    return;
                _Status = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region User変更通知プロパティ

        private User _User = new User("", null, 0);
        public User User
        {
            get
            { return _User; }
            set
            {
                if (_User == value)
                    return;
                _User = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Tweets変更通知プロパティ

        private ObservableCollection<TweetContent> _Tweets = new ObservableCollection<TweetContent>();

        public ObservableCollection<TweetContent> Tweets
        {
            get
            { return _Tweets; }
        }

        #endregion
    }
}
