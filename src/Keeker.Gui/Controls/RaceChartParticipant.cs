//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace Keeker.Gui.Controls
//{
//    public class RaceChartParticipant : IList<RaceChartEntry>
//    {
//        #region Fields

//        private readonly List<RaceChartEntry> _entries;

//        #endregion

//        #region Constructor

//        public RaceChartParticipant()
//        {
//            _entries = new List<RaceChartEntry>();
//        }

//        #endregion

//        #region Public


//        #endregion

//        #region Internal

//        internal event EventHandler ItemAdded;

//        #endregion

//        #region IList<RaceChartEntry> Members

//        public RaceChartEntry this[int index]
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//            set
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public int IndexOf(RaceChartEntry item)
//        {
//            throw new NotImplementedException();
//        }

//        public void Insert(int index, RaceChartEntry item)
//        {
//            throw new NotImplementedException();
//        }

//        public void RemoveAt(int index)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        #region ICollection<RaceChartEntry> Members

//        public int Count
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public bool IsReadOnly
//        {
//            get
//            {
//                throw new NotImplementedException();
//            }
//        }

//        public void Add(RaceChartEntry item)
//        {
//            _entries.Add(item);
//            this.ItemAdded?.Invoke(this, EventArgs.Empty);
//        }

//        public void AddData(object data)
//        {
//            this.Add(new RaceChartEntry(data));
//        }

//        public void Clear()
//        {
//            throw new NotImplementedException();
//        }

//        public bool Contains(RaceChartEntry item)
//        {
//            throw new NotImplementedException();
//        }

//        public void CopyTo(RaceChartEntry[] array, int arrayIndex)
//        {
//            throw new NotImplementedException();
//        }

//        public bool Remove(RaceChartEntry item)
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        #region IEnumerable<RaceChartEntry> Members

//        public IEnumerator<RaceChartEntry> GetEnumerator()
//        {
//            throw new NotImplementedException();
//        }

//        #endregion

//        #region IEnumerable Members

//        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

//        #endregion
//    }
//}
