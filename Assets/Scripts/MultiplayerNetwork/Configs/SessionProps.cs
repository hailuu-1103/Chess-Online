namespace MultiplayerNetwork.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using Fusion;
    
    [Serializable]
    public class SessionProps
    {
        public string RoomName = "AutoSession";
        public int    versusModeId;
        public int    PlayerLimit = 2;
        public int    BestOfNum   = 1;
        public int    StageId;
        public int    MatchTime;
        public int    BPM;

        /// <summary>
        /// Support code that allow conversion of the above fields to and from the SessionProperty dictionary needed by Fusion
        /// </summary>
        public SessionProps() { }

        public SessionProps(ReadOnlyDictionary<string, SessionProperty> props)
        {
            foreach (FieldInfo field in this.GetType().GetFields())
            {
                field.SetValue(this, this.ConvertFromSessionProp(props[field.Name], field.FieldType));
            }
        }

        public Dictionary<string, SessionProperty> Properties
        {
            get
            {
                Dictionary<string, SessionProperty> props = new Dictionary<string, SessionProperty>();
                foreach (FieldInfo field in this.GetType().GetFields())
                {
                    props[field.Name] = this.ConvertToSessionProp(field.GetValue(this));
                }

                return props;
            }
        }

        private object ConvertFromSessionProp(SessionProperty sp, Type toType)
        {
            if (toType == typeof(bool))
                return (int)sp == 1;
            if (sp.IsString)
                return (string)sp;
            return (int)sp;
        }

        private SessionProperty ConvertToSessionProp(object value)
        {
            if (value is string)
                return SessionProperty.Convert(value);
            if (value is bool b)
                return b ? 1 : 0;
            return (int)value;
        }
    }
}