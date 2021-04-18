using iProcessHelper.Helpers;
using iProcessHelper.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iProcessHelper.Models
{
    public class FilterField : NotifyPropertyChanged
    {
        private EntityColumn column;
        private Visibility textVisibility;
        private Visibility intVisibility;
        private Visibility decimalVisibility;
        private Visibility boolVisibility;
        private Visibility objectVisibility;
        private Visibility dateVisibility;
        private Visibility binaryVisibility;

        public EntityColumn Column
        {
            get => column;
            set
            {
                column = value;

                this.CollapseFields();

                switch (column.DataType)
                {
                    case DataType.STRING:
                        TextVisibility = Visibility.Visible;
                        break;
                    case DataType.INT:
                        IntVisibility = Visibility.Visible;
                        break;
                    case DataType.DECIMAL:
                        DecimalVisibility = Visibility.Visible;
                        break;
                    case DataType.BOOL:
                        BoolVisibility = Visibility.Visible;
                        break;
                    case DataType.DATE_TIME:
                        DateVisibility = Visibility.Visible;
                        break;
                    case DataType.GUID:
                        ObjectVisibility = Visibility.Visible;
                        break;
                    case DataType.BYTE_ARRAY:
                        BinaryVisibility = Visibility.Visible;
                        break;
                    default:
                        break;
                }
            }
        }
        public ObservableCollection<EntityColumn> Columns { get; set; }

        public string Name { get; set; }
        public OperationType OperationType { get; set; }
        public ObservableCollection<OperationType> OperationTypes { get; set; } = Constants.operationTypes;

        public string TextValue { get; set; }
        public int IntValue { get; set; }
        public decimal DecimalValue { get; set; }
        public bool BoolValue { get; set; }
        public Guid ObjectValue { get; set; }
        public DateTime DateValue { get; set; }
        public byte[] BinaryValue { get; set; }

        public Visibility TextVisibility { get => textVisibility; set { textVisibility = value; OnPropertyChanged(); } }
        public Visibility IntVisibility { get => intVisibility; set { intVisibility = value; OnPropertyChanged(); } }
        public Visibility DecimalVisibility { get => decimalVisibility; set { decimalVisibility = value; OnPropertyChanged(); } }
        public Visibility BoolVisibility { get => boolVisibility; set { boolVisibility = value; OnPropertyChanged(); } }
        public Visibility ObjectVisibility { get => objectVisibility; set { objectVisibility = value; OnPropertyChanged(); } }
        public Visibility DateVisibility { get => dateVisibility; set { dateVisibility = value; OnPropertyChanged(); } }
        public Visibility BinaryVisibility { get => binaryVisibility; set { binaryVisibility = value; OnPropertyChanged(); } }

        public FilterField()
        {
            this.CollapseFields();
        }

        private void CollapseFields()
        {
            TextVisibility = Visibility.Collapsed;
            IntVisibility = Visibility.Collapsed;
            DecimalVisibility = Visibility.Collapsed;
            BoolVisibility = Visibility.Collapsed;
            ObjectVisibility = Visibility.Collapsed;
            DateVisibility = Visibility.Collapsed;
            BinaryVisibility = Visibility.Collapsed;
        }

        internal bool IsValid(string value)
        {
            switch (column.DataType)
            {
                case DataType.STRING:
                    return value == TextValue;
                case DataType.INT:
                    return int.Parse(value) == IntValue;
                case DataType.DECIMAL:
                    return decimal.Parse(value) == DecimalValue;
                case DataType.BOOL:
                    return bool.Parse(value) == BoolValue;
                case DataType.DATE_TIME:
                    return DateTime.Parse(value) == DateValue;
                case DataType.GUID:
                    return Guid.Parse(value) == ObjectValue;
                default:
                    return false;
            }
        }
    }
}
