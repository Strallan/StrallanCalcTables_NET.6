using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Infratools
{
    public static class DefineDependencyProperties
    {
        // универсальный метод для задания свойств зависимостей
        // большая часть свойств базовых типов и не очень базовых типов может быть зарегистрировано с помощью именно этого метода
        public static DependencyProperty SetDepProp(string propName, Type propertyType, Type ownerType, PropertyChangedCallback changedCallback)
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                AffectsMeasure = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true
            };
            metadata.PropertyChangedCallback += changedCallback;
            return DependencyProperty.Register(propName, propertyType, ownerType, metadata);
        }

        public static DependencyProperty SetInt32Dependency(string propertyName, Type ownerType, PropertyChangedCallback changedCallback)
        {
            FrameworkPropertyMetadata intValueMetadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = 0,
                AffectsMeasure = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true,
            };
            intValueMetadata.PropertyChangedCallback += changedCallback;
            return DependencyProperty.Register(propertyName, typeof(int), ownerType, intValueMetadata);
        }

        public static DependencyProperty SetDoubleDependency(string propertyName, double defaultValue, Type ownerType, PropertyChangedCallback changedCallBack)
        {
            FrameworkPropertyMetadata doubleValueMetadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = defaultValue,
                AffectsMeasure = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true,
            };
            doubleValueMetadata.PropertyChangedCallback += changedCallBack;
            return DependencyProperty.Register(propertyName, typeof(double), ownerType, doubleValueMetadata);
        }

        public static DependencyProperty SetDoubleDependency(string propertyName, Type ownerType, PropertyChangedCallback changedCallBack)
        {
            FrameworkPropertyMetadata doubleValueMetadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = 0d,
                AffectsMeasure = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true
            };
            doubleValueMetadata.PropertyChangedCallback += changedCallBack;
            return DependencyProperty.Register(propertyName, typeof(double), ownerType, doubleValueMetadata);
        }

        public static DependencyProperty SetIEnumerableDependency(string propName, Type ownerType, PropertyChangedCallback changedCallback)
        {
            FrameworkPropertyMetadata collectionMetadata = new FrameworkPropertyMetadata()
            {
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true
            };
            collectionMetadata.PropertyChangedCallback += changedCallback;
            return DependencyProperty.Register(propName, typeof(IEnumerable), ownerType, collectionMetadata);
        }

        public static DependencyProperty SetStringDependency(string propName, Type ownerType, PropertyChangedCallback changedCallback)
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = "",
                AffectsMeasure = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true,
            };
            metadata.PropertyChangedCallback += changedCallback;
            return DependencyProperty.Register(propName, typeof(string), ownerType, metadata);
        }

        public static DependencyProperty SetImageDependency(string propertyName, Type ownerType, PropertyChangedCallback onPropertyChanged)
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = new BitmapImage(),
                AffectsMeasure = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true
            };
            metadata.PropertyChangedCallback += onPropertyChanged;
            return DependencyProperty.Register(propertyName, typeof(Image), ownerType, metadata);
        }

        public static DependencyProperty SetBrushDependency(string propertyName, Type ownerType, PropertyChangedCallback onPropertyChanged)
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = (Brush)(new SolidColorBrush(Colors.White)),
                AffectsMeasure = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true
            };
            metadata.PropertyChangedCallback += onPropertyChanged;
            return DependencyProperty.Register(propertyName, typeof(Brush), ownerType, metadata);
        }

        public static DependencyProperty SetBoolDependency(string propertyName, Type ownerType, PropertyChangedCallback onPropertyChanged)
        {
            FrameworkPropertyMetadata metadata = new FrameworkPropertyMetadata()
            {
                DefaultValue = false,
                AffectsMeasure = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                IsNotDataBindable = false,
                BindsTwoWayByDefault = true
            };
            metadata.PropertyChangedCallback += onPropertyChanged;
            return DependencyProperty.Register(propertyName, typeof(bool), ownerType, metadata);
        }

    }
}
