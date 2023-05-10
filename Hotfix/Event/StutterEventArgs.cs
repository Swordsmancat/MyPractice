//------------------------------------------------------------
// Creator: FatCat
// Creation date: 2022/7/15/周五 10:53:27
//------------------------------------------------------------
using GameFramework;
using GameFramework.Event;

namespace Farm.Hotfix
{
    public class StutterEventArgs : GameEventArgs
    {
        public static readonly int EventId = typeof(StutterEventArgs).GetHashCode();

        public override int Id
        {
            get
            {
                return EventId;
            }
        }
        public object UserData
        {
            get;
            private set;
        }

        public static StutterEventArgs Create(object userData = null)
        {
            StutterEventArgs stutterEventArgs = ReferencePool.Acquire<StutterEventArgs>();
            stutterEventArgs.UserData = userData;
            return stutterEventArgs;
        }

        public override void Clear()
        {
            UserData = default(object);
        }
    }
}
